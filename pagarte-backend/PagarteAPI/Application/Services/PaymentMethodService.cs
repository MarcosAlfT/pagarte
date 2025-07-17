using FluentResults;
using PagarteAPI.Application.Dtos.Payments;
using PagarteAPI.Application.Interfaces;
using PagarteAPI.Domain.Payments;
using PagarteAPI.Infrastructure.Persistence.Repository;

namespace PagarteAPI.Application.Services
{
	public class PaymentMethodService(IPaymentMethodRepository paymentMethodRepository) : IPaymentMethodService
	{
		private readonly IPaymentMethodRepository _paymentMethodRepository = paymentMethodRepository;

		public async Task<Result> CreatePaymentMethodAsync(CreatePaymentMethodRequest request, Guid  userId)
		{
			bool isDefault = false;
			var creditCard = new CreditCard
			{
				CardNumber = request.CardNumber,
				CardHolderName = request.CardHolderName,
				ExpirationMonth = request.ExpirationMonth,
				ExpirationYear = request.ExpirationYear,
				Cvv = request.Cvv,
				Brand = request.Brand,
			};

			var hasAtLeastOneActivePaymentMethod = await _paymentMethodRepository.HasUserPaymentMethod(userId);

			if (!hasAtLeastOneActivePaymentMethod)
			{
				isDefault = true;
			}
			else
			{
				var available = await _paymentMethodRepository.CheckCardAvailabilityForCreationAsync(userId,
					creditCard.Brand, creditCard.LastFourDigits);

				if (available.IsFailed)
				{
					return available;
				}
			}

			//Here developing the logic to call DLocal to create the payment method

			// For now, we will just simulate the creation of the payment method

			var callDlocalResult = true; // Simulate a successful call to dLocal

			if (!callDlocalResult)
			{
				return Result.Fail("Failed to create payment method with dLocal.");
			}

			var cardToken = Guid.NewGuid().ToString(); // Simulated token from dLocal

			// Save the payment method in the repository`

			var newPaymentMethod = PaymentMethod.CreatePaymentMethod(userId,
				cardToken, request.Brand, creditCard.LastFourDigits, isDefault);

			var addResult = await _paymentMethodRepository.AddPaymentMethodAsync(newPaymentMethod);

			if (addResult.IsFailed)
			{
				return addResult;
			}

			return Result.Ok().WithSuccess("Payment method created successfully.");
		}

		public async Task<Result> DeletePaymentMethodAsync(Guid paymentMethodId, Guid UserId)
		{
			var result = await _paymentMethodRepository.GetPaymentMethodByIdAsync(paymentMethodId);
			
			if (result.IsFailed)
			{
				return result.ToResult();
			}

			var paymentMethod = result.Value;

			if (paymentMethod == null)
			{
				return Result.Fail("Payment method not found.");
			}
			if (paymentMethod.UserId != UserId)
			{
				return Result.Fail("You are not authorized to delete this payment method.");
			}

			paymentMethod.SoftDelete();

			var updateResult = await _paymentMethodRepository.UpdatePaymentMethodAsync(paymentMethod);

			return updateResult;
			
		}

		public async Task<Result<IEnumerable<PaymentMethodDto>>> GetPaymentMethodsByUserIdAsync(Guid userId)
		{
			var paymentMethodsResult = await _paymentMethodRepository.GetPaymentMethodsByUserIdAsync(userId);

			if (paymentMethodsResult.IsFailed)
			{
				return Result.Fail("pending error define");
			}

			var paymentMethodsDto = paymentMethodsResult.Value.Select(pm => new PaymentMethodDto
			{
				Id = pm.Id,
				UserId = pm.UserId,
				Brand = pm.Brand,
				LastFourDigits = pm.LastFourDigits,
				IsActive = pm.IsActive,
				IsDefault = pm.IsDefault,
			});

			return Result.Ok(paymentMethodsDto);
		}
	}
}
