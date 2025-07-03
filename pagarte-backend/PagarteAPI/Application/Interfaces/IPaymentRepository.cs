using PagarteAPI.Domain.Payment;
using FluentResults;

namespace PagarteAPI.Application.Interfaces
{
	public interface IPaymentRepository
	{
		Task<Result> CreateAsync(Transaction transaction);
		Task<Result<Transaction>> GetByUserIdAsync(Guid userId);
		Task<Result> UpdateAsync(Transaction transaction);
	}
}
