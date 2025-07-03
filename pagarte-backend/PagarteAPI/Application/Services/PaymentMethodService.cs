using FluentResults;
using PagarteAPI.Application.Dtos.Payments;
using PagarteAPI.Application.Interfaces;
using PagarteAPI.Domain.Payment;

namespace PagarteAPI.Application.Services
{
	public class PaymentMethodService : IPaymentMethodService
	{
		private readonly IPaymentMethodRepository _paymentMethodRepository;

		public PaymentMethodService(IPaymentMethodRepository paymentMethodRepository)
		{
			_paymentMethodRepository = paymentMethodRepository;
		}

		public async Task<Result> AddPaymentMethodAsync(CreatePaymentMethodRequest request, Guid  userId)
		{

			var creditCard = new CreditCard
			{
				CardNumber = request.CardNumber,
				CardHolderName = request.CardHolderName,
				ExpirationDate = request.ExpirationDate,
				Cvv = request.Cvv,
				Brand = request.Brand,
			};

			//Here developing the logic to call DLocal to create the payment method

			// For now, we will just simulate the creation of the payment method

			var callDlocalResult = true; // Simulate a successful call to dLocal

			if (!callDlocalResult)
			{
				return Result.Fail("Failed to create payment method with dLocal.");
			}
			
			var cardToken = "simulation-of-dlocal-card-token"; // Simulated token from dLocal

			// Save the payment method in the repository`
			var paymentMethod = new PaymentMethod
			{
				Id = Guid.NewGuid(),
				UserId = request.UserId,
				Brand = request.Brand,
				ProviderToken = cardToken, // This is the token from dLocal
				LastFourDigits = creditCard.LastFourDigits,
				IsActive = true, // Assuming the payment method is active upon creation
				IsDefault = request.IsDefault,
				CreatedAt = DateTime.UtcNow,
				UpdatedAt = DateTime.UtcNow
			};

			var result = await _paymentMethodRepository.AddAsync(paymentMethod);

			if (result.IsFailed)
			{
				return Result.Fail("pending error define");
			}


			return Result.Ok().WithSuccess("Payment method created successfully.");
		}

		public async Task<Result<IEnumerable<PaymentMethodDto>>> GetPaymentMethodsByUserIdAsync(Guid userId)
		{
			var result = await _paymentMethodRepository.AddAsync(paymentMethod);

			if (result.IsFailed)
			{
				return Result.Fail("pending error define");
			}


			return Result.Ok().WithSuccess("Payment method created successfully.");
		}
	}
}
