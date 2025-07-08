using IdentityService.Application.Dtos.Response;
using IdentityService.Application.Dtos.Auth;
using IdentityService.Application.Interfaces;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using OpenIddict.Abstractions;
using OpenIddict.Server.AspNetCore;
using System.Security.Claims;

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

		[HttpPost("~/connect/token")]
		public async Task<IActionResult> Exchange()
		{
			var request = HttpContext.GetOpenIddictServerRequest();

			if (request == null)
			{
				return BadRequest(ApiResponse.CreateFailure("Invalid OpenIddict request."));
			}

			// Validate that Username and Password are not null or empty
			if (string.IsNullOrEmpty(request.Username) || string.IsNullOrEmpty(request.Password))
			{
				return BadRequest(ApiResponse.CreateFailure("Username cannot be null or empty."));
			}

			if (!request.IsPasswordGrantType())
			{
				// Return error for unsupported grant type...
				return Forbid(new AuthenticationProperties(/* ... */));
			}

			// Call the service to do ALL the work.
			var principalResult = await _authService.AuthenticateAndCreatePrincipalAsync(request.Username, request.Password);

			// Check if the service failed.
			if (principalResult.IsFailed)
			{
				// Return a generic "invalid_grant" error to the client.
				return Forbid(new AuthenticationProperties(new Dictionary<string, string?>
				{
					[OpenIddictServerAspNetCoreConstants.Properties.Error] = "invalid_grant",
					[OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] = principalResult.Errors.First().Message
				}), OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
			}

			// If the service succeeded, pass the ClaimsPrincipal it returned directly to SignIn.
			// OpenIddict will handle the rest (creating the token, sending the response).
			return SignIn(principalResult.Value, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
		}
	}
}
