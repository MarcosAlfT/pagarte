using FluentResults;
using PagarteAPI.Application.Dtos;
using PagarteAPI.Application.Dtos.Payments;
using PagarteAPI.Application.Interfaces;
using PagarteAPI.Domain.Payment;
using PagarteAPI.Infrastructure.Persistence.Repository;

namespace PagarteAPI.Application.Services
{
	public class PaymentService : IPaymentService
	{
		private readonly IPaymentRepository _paymentRepository;
		private readonly IPaymentMethodRepository _paymentMethodRepository;
		// private readonly IDlocalClient _dlocalClient; // create and inject this later
		// private readonly IBillerClient _billerClient; //create and inject this later

		public PaymentService(
			IPaymentRepository paymentRepository,
			IPaymentMethodRepository paymentMethodRepository)
		{
			_paymentRepository = paymentRepository;
			_paymentMethodRepository = paymentMethodRepository;
		}

		public async Task<Result> CreatePaymentAsync(CreatePaymentRequest request, string userId)
		{
			// Validate that the payment method belongs to the user
			var paymentMethodResult = await _paymentMethodRepository.GetByUserIdAsync(request.PaymentMethodId); // We need to add GetByIdAsync to the repo

			if (paymentMethodResult.IsFailed)
				return Result.Fail("");

			if (paymentMethodResult.Value.UserId != userId)
				return Result.Fail("You do not have permission to use this payment method.");


			//  Create Payment domain object
			var payment = new Transaction
			{
				Id = Guid.NewGuid(),
				PaymentMethodId = request.PaymentMethodId,
				BillerId = request.BillerId,
				BillerAccountId = request.BillerAccountId,
				Amount = request.Amount,
				FeeTrx = CalculateFee(request.Amount), // Implement your fee logic
				Currency = request.Currency,
				Status = "PENDING", 
				CreatedAt = DateTime.UtcNow,
				UpdatedAt = DateTime.UtcNow
			};

			var createResult = await _paymentRepository.CreateAsync(payment);
			if (createResult.IsFailed)
			{
				return Result.Fail(""); //fix return
			}

			// Call the external payment provider (dLocal) to charge the card
			// This will be an asynchronous process.
			// The result will come back later via a webhook.
			Console.WriteLine($"--- Pretending to call dLocal to charge {payment.Amount + payment.FeeTrx} {payment.Currency} ---");
			Console.WriteLine($"--- for transaction ID: {payment.Id} ---");

			/////////// Check here to put in a queue

			// TODO: var chargeResult = await _dlocalClient.AddPaymentMethodAsync(transaction, paymentMethodResult.Value.ProviderToken);
			// TODO: if (chargeResult.IsFailed) { ... handle immediate failure ... }

			// If the initiation call is successful
			// Wait for the webhook from dLocal to continue the process.
			return Result.Ok().WithSuccess("Payment process initiated successfully. Waiting for confirmation.");
		}

		private decimal CalculateFee(decimal amount)
		{
			// Example fee logic: 2.5% + 30 cents
			return Math.Round((amount * 0.025m) + 0.30m, 2);
		}
	}
}
