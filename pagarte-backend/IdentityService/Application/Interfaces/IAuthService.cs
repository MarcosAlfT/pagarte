using Api.Contrats.Shared.Responses;
using IdentityService.Dtos.Auth;
using FluentResults;


namespace IdentityService.Application.Interfaces
{
	public interface IAuthService
	{
		Result Register(RegisterUserRequest user);
		Result ConfirmEmail(string token);
		Result<TokenResponse> Login(LoginRequest user);
	}
}
