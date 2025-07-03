using Email.Shared;
using IdentityService.Application.Interfaces;
using IdentityService.Domain;
using IdentityService.Infrastructure.Security;
using FluentResults;
using IdentityService.Application.Dtos.Auth;

namespace IdentityService.Application.Services
{
	public class AuthService(IUserRepository userRepository, IPasswordHasher passwordHasher,
		IEmailConfirmationTokenGenerator emailConfirmationTokenGenerator,
		IJwtTokenGenerator jwtTokenGenerator,
		//IEmailSender emailSender,
		IConfiguration confGetEmail) : IAuthService
	{
		private readonly IUserRepository _userRepository = userRepository;
		private readonly IPasswordHasher _passwordHasher = passwordHasher;
		private readonly IEmailConfirmationTokenGenerator _emailTokenGenerator = emailConfirmationTokenGenerator;
		//private readonly IEmailSender _emailSender = emailSender;
		private readonly IConfiguration _configuration = confGetEmail;
		private readonly IJwtTokenGenerator _jwtTokenGenerator = jwtTokenGenerator;

		public async Task<Result> RegisterAsync(RegisterUserRequest request)
		{
			if (string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrEmpty(request.Email) || string.IsNullOrWhiteSpace(request.Password))
			{
				return Result.Fail("Name, email, and password are required.");
			}

			if (string.IsNullOrEmpty(_configuration["Application:Url"]))
			{
				return Result.Fail("Url not found to sent the confirmation email");
			}

			Result validateExistance = await IsUsernameAndEmailAvailable(request.Username, request.Email);

			if (validateExistance.IsFailed)
			{
				return validateExistance;
			}
			
			string hashedPassword = _passwordHasher.Hash(request.Password);
			string confirmationToken = _emailTokenGenerator.GenerateToken();

			var newUser = User.CreateNew(Guid.NewGuid(), request.Username, request.Email, hashedPassword, confirmationToken);

			Result createUserResult = await  _userRepository.CreateAsync(newUser);

			if (createUserResult.IsFailed)
			{
				return createUserResult;
			}

			// Send confirmation email

			string? applicationUrl = _configuration["Application:Url"];
			Console.WriteLine($"applicationUrl: {applicationUrl}");

			string confirmationLink = $"{applicationUrl}/api/Auth/confirm-email?token={confirmationToken}";
			string emailBody = $"Please confirm your email by clicking the link: <a href='{confirmationLink}'>Confirm Email</a>";
			Console.WriteLine($"emailBody: {emailBody}");
			//_emailSender.SendEmail(!string.IsNullOrEmpty(newUser.Email) ? newUser.Email : "", "Confirm your email", emailBody);

			return Result.Ok().WithSuccess("Registration success. Please chech your email.");
		}

		private async Task<Result> IsUsernameAndEmailAvailable(string username, string email)
		{
			Result<UserExistenceDto> existenceCheck = await _userRepository.CheckExistenceAsync(username, email);

			if (existenceCheck.IsFailed)
			{
				return Result.Fail(existenceCheck.Errors);
			}

			var validationDto = existenceCheck.Value;

			if (validationDto.ExistUsername && validationDto.ExistEmail)
			{
				return Result.Fail("Username and Email already exists.");
			}
			else if (validationDto.ExistUsername)
			{
				return Result.Fail("Username already exists.");
			}
			else if (validationDto.ExistEmail)
			{
				return Result.Fail("Email already exists.");
			}
			return Result.Ok();
		}

		public async Task<Result<TokenResponse>> LoginAsync(LoginRequest userRequest)
		{
			Result<User> userResult = await _userRepository.GetUserByUsernameOrEmailAsync(userRequest.UsernameOrEmail);

			if (userResult.IsFailed)
			{
				return Result.Fail(userResult.Errors);
			}

			User user = userResult.Value;

			if (user is null || string.IsNullOrEmpty(user.PasswordHash) || !_passwordHasher.Verify(userRequest.Password, user.PasswordHash))
			{
				return Result.Fail("Invalid email or password.");
			}

			if (!user.IsEmailConfirmed)
			{
				return Result.Fail("Email not confirmed. Please check your email for confirmation link.");
			}

			if (!user.IsActive)
			{
				return Result.Fail("User is not active. Please contact support.");
			}

			// Generate JWT token
			Result<string> tokenResult = _jwtTokenGenerator.GenerateToken(user);

			if (tokenResult.IsFailed)
			{
				return Result.Fail(tokenResult.Errors);
			}

			var response = new TokenResponse { AccessToken = tokenResult.Value };

			return Result.Ok(response);
		}

		public async Task<Result> ConfirmEmailAsync(string token)
		{
			Result<User> userResult = await _userRepository.GetUserByTokenAsync(token);

			if (userResult.IsFailed)
			{
				// Pass the failure reason up directly.
				return userResult.ToResult(); // .ToResult() converts Result<User> to Result
			}

			User user = userResult.Value;

			var userConfirmed = user.ConfirmEmail();

			if (userConfirmed.IsFailed)
			{
				return Result.Fail(userConfirmed.Errors);
			}

			return await _userRepository.UpdateEmailConfirmationStatusAsync(user);
		}
	}
}
