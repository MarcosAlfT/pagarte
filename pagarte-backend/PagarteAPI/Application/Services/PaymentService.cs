using FluentResults;
using PagarteAPI.Application.Dtos.Payments;
using PagarteAPI.Application.Interfaces;
using PagarteAPI.Domain.Payments;

namespace PagarteAPI.Application.Services
{
	public class PaymentService(
		IPaymentRepository paymentRepository,
		IPaymentMethodRepository paymentMethodRepository) : IPaymentService
	{
		private readonly IPaymentRepository _paymentRepository = paymentRepository;
		private readonly IPaymentMethodRepository _paymentMethodRepository = paymentMethodRepository;

		public async Task<Result> CreatePaymentAsync(CreatePaymentRequest request, Guid userId)
		{
			//  Create Transaction domain object to creat a payment
			var transaction = new Transaction
			{
				Id = Guid.NewGuid(),
				PaymentMethodId = request.PaymentMethodId,
				BillerId = request.BillerId,
				BillerAccountId = request.BillerAccountId,
				Amount = request.Amount,
				FeeTrx = 0,  // Todo: Calculate based on business logic of each transaction
				Currency = request.Currency,
				Status = "PENDING", 
				CreatedAt = DateTime.UtcNow,
				UpdatedAt = DateTime.UtcNow
			};

			var createResult = await _paymentRepository.AddTransactionAsync(transaction);

			if (createResult.IsFailed)
			{
				return Result.Fail(createResult.Errors);
			}

			/////////// Check here to put in a queue
			// Call the external payment provider (dLocal) to charge the card
			// This will be an asynchronous process.

			return Result.Ok().WithSuccess("Payment process initiated successfully. Waiting for confirmation.");
		}

		public Task<Result<IEnumerable<System.Transactions.Transaction>>> GetTransactionsOkByUserId(Guid userId)
		{
			throw new NotImplementedException();
		}
	}
}
