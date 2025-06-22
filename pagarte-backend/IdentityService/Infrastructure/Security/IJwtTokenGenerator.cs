using FluentResults;
using IdentityService.Domain;

namespace IdentityService.Infrastructure.Security
{
	public interface IJwtTokenGenerator
	{
		Result<string> GenerateToken(User user);

	}
}
