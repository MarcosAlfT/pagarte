using Api.Contrats.Shared.Responses;
using FluentResults;
using IdentityService.Application.Dtos.Auth;


namespace IdentityService.Application.Interfaces
{
	public interface IAuthService
	{
		Task<Result> RegisterAsync(RegisterUserRequest user);
		Task<Result> ConfirmEmailAsync(string token);
		Task<Result<TokenResponse>> LoginAsync(LoginRequest user);
	}
}
