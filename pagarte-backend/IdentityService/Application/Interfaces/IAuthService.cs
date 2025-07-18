﻿using FluentResults;
using IdentityService.Application.Dtos.Auth;
using System.Security.Claims;


namespace IdentityService.Application.Interfaces
{
	public interface IAuthService
	{
		Task<Result> RegisterAsync(RegisterUserRequest user);
		Task<Result> ConfirmEmailAsync(string token);
		Task<Result<ClaimsPrincipal>> AuthenticateUserAndBuildPrincipalAsync(string user, string password);
	}
}
