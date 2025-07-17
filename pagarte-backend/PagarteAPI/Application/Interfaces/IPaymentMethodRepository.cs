using FluentResults;
using PagarteAPI.Domain.Payments;

namespace PagarteAPI.Application.Interfaces
{
	public interface IPaymentMethodRepository
	{
		Task<Result> AddPaymentMethodAsync(PaymentMethod paymentMethod);
		Task<Result<IEnumerable<PaymentMethod>>> GetPaymentMethodsByUserIdAsync(Guid userId);
		Task<Result> CheckCardAvailabilityForCreationAsync(Guid userId, string brand, string last4Digits);


		Task<Result<PaymentMethod>> GetPaymentMethodByIdAsync(Guid paymentMethodId);
		Task<Result> UpdatePaymentMethodAsync(PaymentMethod paymentMethod);
		Task<bool> HasUserPaymentMethod(Guid userId);

	}
}
