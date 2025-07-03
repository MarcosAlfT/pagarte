using FluentResults;
using PagarteAPI.Application.Dtos;
using PagarteAPI.Application.Dtos.Payments;

namespace PagarteAPI.Application.Interfaces
{
	public interface IPaymentService
	{
		Task<Result> CreatePaymentAsync(CreatePaymentRequest request, string userId);
	}
}
