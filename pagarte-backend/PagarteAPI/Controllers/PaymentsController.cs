using PagarteAPI.Application.Dtos.ApiResponse;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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

		[HttpPost("create-payment-method")]
		[ProducesResponseType(typeof(ApiResponse), 200)]
		[ProducesResponseType(204)]
		[ProducesResponseType(400)]
		public async Task<ActionResult<ApiResponse>> CreatePaymentMethodAsync([FromBody] CreatePaymentMethodRequest request)
		{
			if (!ModelState.IsValid)
			{
					return BadRequest(ApiResponse<string>.CreateFailure("Input data error"));
			}
			var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);

			if (!Guid.TryParse(userIdString, out Guid userId))
			{
				return Unauthorized(new { Message = "User Id missing or invalid in token" });
			}

			var result = await _paymentService.CreatePaymentMethodAsync(request, userId);

			if (result.IsFailed)
			{
				return BadRequest(result);
			}

			return Ok(ApiResponse.CreateSuccess(result.Successes[0].Message));
		}

		[HttpGet("get-payment-methods")]
		[ProducesResponseType(typeof(ApiResponse<string>), 200)]
		[ProducesResponseType(204)]
		[ProducesResponseType(400)]
		public async Task<IActionResult> GetPaymentMethodsByUserIdAsync(Guid UserIdRequest)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ApiResponse<string>.CreateFailure("Input data error"));
			}
			var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);

			if (!Guid.TryParse(userIdString, out Guid userId) || UserIdRequest != userId)
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
