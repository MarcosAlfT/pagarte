using PagarteAPI.Domain.Payment;
using FluentResults;

namespace PagarteAPI.Application.Interfaces
{
	public interface IPaymentRepository
	{
		Task<Result> AddTransactionAsync(Transaction transaction);
		Task<Result<Transaction>> GetTransactionsByUserIdAsync(Guid userId);
	
	}
}
