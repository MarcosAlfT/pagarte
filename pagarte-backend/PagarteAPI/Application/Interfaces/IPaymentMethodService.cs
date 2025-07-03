using FluentResults;
using PagarteAPI.Application.Dtos.Payments;

namespace PagarteAPI.Application.Interfaces
{
	public interface IPaymentMethodService
	{
		Task<Result> AddPaymentMethodAsync(CreatePaymentMethodRequest request, Guid userId);
		Task<Result<IEnumerable<PaymentMethodDto>>> GetPaymentMethodsByUserIdAsync(Guid userId);
	}
}
