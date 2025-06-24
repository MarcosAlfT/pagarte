using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using IdentityService.Domain;
using FluentResults;

namespace IdentityService.Infrastructure.Security
{
	public class JwtTokenGenerator(IConfiguration configuration) : IJwtTokenGenerator
	{
		private readonly IConfiguration _configuration = configuration;

		public Result<string> GenerateToken(User user)
		{
			// The claims identify the user. You can add more claims, like roles.
			if (user.Email == null || user.Username == null)
			{
				throw new ArgumentException("User must have a valid email and username to generate a token.");
			}
			if (user.Id == Guid.Empty)
			{
				throw new ArgumentException("User must have a valid ID to generate a token.");
			}
			// CreateAsync claims based on user information
			var claims = new List<Claim>
			{
				new(JwtRegisteredClaimNames.Sub, user.Id.ToString()), // "Subject" - a unique identifier
                new(JwtRegisteredClaimNames.Email, user.Email),
				new(JwtRegisteredClaimNames.Name, user.Username)
			};

			// Get the secret key from appsettings.json

			string? applicationUrl = _configuration["Application:Url"];
			Console.WriteLine($"applicationUrl: {applicationUrl}");

			string? secretText = _configuration["JwtSettings:Secret"];

			if (string.IsNullOrEmpty(secretText))
			{
				return Result.Fail("Server configuration error: ket is not found.");
			}
			var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretText));

			var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

			var token = new JwtSecurityToken(
				issuer: _configuration["JwtSettings:Issuer"],
				audience: _configuration["JwtSettings:Audience"],
				claims: claims,
				expires: DateTime.UtcNow.AddHours(Convert.ToDouble(_configuration["JwtSettings:ExpiryHours"])),
				signingCredentials: creds
			);

			var response =  new JwtSecurityTokenHandler().WriteToken(token);

			return Result.Ok(response);
		}
	}
}
