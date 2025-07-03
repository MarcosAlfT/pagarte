using IdentityService.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Api.Contrats.Shared.Responses;
using IdentityService.Application.Dtos.Auth;

namespace IdentityService.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AuthController(IAuthService authService) : Controller
	{
		private readonly IAuthService _authService = authService;

		[HttpPost("register")]
		[ProducesResponseType(typeof(ApiResponse<string>), 200)]
		[ProducesResponseType(204)]
		[ProducesResponseType(400)]
		public async Task<ActionResult<ApiResponse>> RegisterAsync(RegisterUserRequest newUser)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ApiResponse<string>.CreateFailure("Input data error"));
			}

			var registerResponse = await _authService.RegisterAsync(newUser);

			if (registerResponse.IsFailed)
			{
				return BadRequest(ApiResponse.CreateFailure(registerResponse.Errors.First().Message));
			}

			return Ok(ApiResponse.CreateSuccess(registerResponse.Successes.First().Message));
		}

		[HttpGet("confirm-email")]
		[ProducesResponseType(typeof(ApiResponse), 200)]
		[ProducesResponseType(204)]
		[ProducesResponseType(400)]
		public async Task<ActionResult<ApiResponse>> ConfirmEmailAsync([FromQuery] string token)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ApiResponse.CreateFailure("Input data error."));
			}
			if (string.IsNullOrEmpty(token))
			{
				return BadRequest(ApiResponse.CreateFailure("Missing confirmation token."));
			}

			var confirmResponse = await _authService.ConfirmEmailAsync(token);

			if (confirmResponse.IsFailed)
			{
				return BadRequest(ApiResponse.CreateFailure(confirmResponse.Errors.First().Message));
			}

			return Ok(ApiResponse.CreateSuccess(confirmResponse.Successes.First().Message));
		}

		[HttpPost("login")]
		[ProducesResponseType(typeof(ApiResponse<string>), 200)]
		[ProducesResponseType(204)]
		[ProducesResponseType(400)]
		public async Task<ActionResult<ApiResponse<string>>> LoginAsync([FromBody]LoginRequest loginRequest)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ApiResponse<string>.CreateFailure("Input data error."));
			}
			var loginResponse = await _authService.LoginAsync(loginRequest);

			if (loginResponse.IsFailed)
			{
				return BadRequest(ApiResponse<string>.CreateFailure(loginResponse.Errors.First().Message));
			}

			return Ok(ApiResponse<TokenResponse>.CreateSuccess(loginResponse.Value));

		}
	}
}
