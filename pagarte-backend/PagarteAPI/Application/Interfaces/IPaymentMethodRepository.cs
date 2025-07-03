using FluentResults;
using PagarteAPI.Domain.Payment;

namespace PagarteAPI.Application.Interfaces
{
	public interface IPaymentMethodRepository
	{
		Task<Result> AddAsync(PaymentMethod paymentMethod);
		Task<Result<IEnumerable<PaymentMethod>>> GetPaymentMethodsByUserIdAsync(Guid userId);

		//Task<Result> DeleteAsync(Guid id, Guid userID);

		//Task<Result> GetByUserIdAsync(Guid paymentMethodId);

	}
}
