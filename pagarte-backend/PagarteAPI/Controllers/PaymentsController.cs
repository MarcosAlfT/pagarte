using Api.Contrats.Shared.Responses;
using Azure.Core;
using FluentResults;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PagarteAPI.Application.Dtos.Payments;
using PagarteAPI.Application.Interfaces;
using System.Security.Claims;

namespace PagarteAPI.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	[Authorize]
	public class PaymentsController(IPaymentMethodService paymentService) : Controller
	{
		private readonly IPaymentMethodService _paymentService = paymentService;

		[HttpPost("add-paymentmethod")]
		[ProducesResponseType(typeof(ApiResponse<string>), 200)]
		[ProducesResponseType(204)]
		[ProducesResponseType(400)]
		public async Task<IActionResult> AddPaymentMethodAsync([FromBody] CreatePaymentMethodRequest request)
		{
			var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);

			if (Guid.TryParse(userIdString, out Guid userId) || request.UserId != userId)
			{
				return Unauthorized(new { Message = "You are not authorized" });
			}

			var result = await _paymentService.AddPaymentMethodAsync(request, userId);

			return Ok(result);
		}

		[HttpGet("payment-methods")]
		[ProducesResponseType(typeof(ApiResponse<string>), 200)]
		[ProducesResponseType(204)]
		[ProducesResponseType(400)]
		public async Task<IActionResult> GetPaymentMethodsByUserIdAsync(Guid UserIdRequest)
		{
			var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);

			if (Guid.TryParse(userIdString, out Guid userId) || UserIdRequest != userId)
			{
				return Unauthorized(new { Message = "You are not authorized" });
			}

			var result = await _paymentService.GetPaymentMethodsByUserIdAsync(userId);

			if (result.IsFailed)
			{
				return BadRequest(result.Errors);
			}
			return Ok(result.Value);
		}
	}
}
