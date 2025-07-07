using FluentResults;
using PagarteAPI.Application.Dtos;
using PagarteAPI.Application.Dtos.Payments;
using System.Transactions;

namespace PagarteAPI.Application.Interfaces
{
	public interface IPaymentService
	{
		Task<Result> CreatePaymentAsync(CreatePaymentRequest request, Guid userId);
		Task<Result<IEnumerable<Transaction>>> GetTransactionsOkByUserId(Guid userId);
	}
}
