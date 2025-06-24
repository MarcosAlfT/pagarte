using Dapper;
using DataAccess.Shared;
using FluentResults;
using IdentityService.Application.Interfaces;
using IdentityService.Domain;
using IdentityService.Dtos.Auth;
using Microsoft.Data.SqlClient;
using System.Data;

namespace IdentityService.Infrastructure.Persistence.Repositories
{
	public class UserRepository(IDbConnectionFactory dbConnectionFactory
		//, ILogger<UserRepository> logger
		) : IUserRepository
	{
		private readonly IDbConnectionFactory _dbConnectionFactory = dbConnectionFactory;
		//private readonly ILogger<UserRepository> _logger = logger;

		/// <summary>
		/// Creates a new user in the database.
		/// </summary>
		/// <param name="user"></param>
		/// <returns></returns>
		public async Task<Result> CreateAsync(User user)
		{
			using IDbConnection db = _dbConnectionFactory.CreateConnection("AuthDb");

			try
			{
				var parameters = new
				{
					user.Id,
					user.Username,
					user.Email,
					user.PasswordHash,
					CreationDate = DateTime.UtcNow,
					Token = user.ConfirmationToken,
					SentAt = DateTime.UtcNow,
					user.IsEmailConfirmed,
					user.IsActive,
				};

				await db.ExecuteAsync("CreateUser", parameters, commandType: System.Data.CommandType.StoredProcedure);

				return Result.Ok().WithSuccess("User was created successfully");
			}
			catch (SqlException sqlEx)
			{
				//_logger.LogError(sqlEx, "SQL error occurred while creating user.");
				return Result.Fail($"Database error: { sqlEx.Message}");
			}
			catch (Exception ex)
			{
				//_logger.LogError(ex, "An unexpected error occured while create user with email {user.Email}", user.Email);
				return Result.Fail($"An unexpected error occured: {ex.Message}");
			}
		}

		/// <summary>
		/// Checks if a user with the given username or email already exists in the database.
		/// </summary>
		/// <param name="userName"></param>
		/// <param name="email"></param>
		/// <returns></returns>
		public async Task<Result<UserExistenceDto>> CheckExistenceAsync(string userName, string email)
		{
			using IDbConnection db = _dbConnectionFactory.CreateConnection("AuthDb");

			try
			{
				var parameters = new
				{
					Username = userName,
					Email = email
				};

				var existenceResult = await db.QuerySingleAsync<UserExistenceDto>(
					"CheckUserExistence",
					parameters,
					commandType: CommandType.StoredProcedure);

				return Result.Ok(existenceResult);
			}
			catch (SqlException sqlEx) {
				//_logger.LogError(sqlEx, "SQL error occurred while checking user existence.");
				return Result.Fail<UserExistenceDto>($"Database error: {sqlEx.Message}");
			}
			catch (Exception ex)
			{
				//_logger.LogError(ex, "An unexpected error occurred while checking user existence.");
				return Result.Fail<UserExistenceDto>($"An unexpected error occurred: {ex.Message}");
			}
		}

		/// <summary>
		/// Retrieves a user from the database by their authentication token.
		/// </summary>
		/// <param name="token"></param>
		/// <returns></returns>
		public async Task<Result<User>> GetUserByTokenAsync(string token)
		{
			using IDbConnection db = _dbConnectionFactory.CreateConnection("AuthDb");

			try
			{
				var parameters = new { Token = token };

				User? user = await db.QuerySingleOrDefaultAsync<User>(
					"GetUserByToken",
					parameters,
					commandType: CommandType.StoredProcedure);

				// If Dapper returns null, we convert it to a specific failure result.
				if (user is null)
				{
					return Result.Fail("Token was not found.");
				}

				// Otherwise, we wrap the found user in a success result.
				return Result.Ok(user);
			}
			catch (SqlException ex)
			{
				//_logger.LogError(ex, "Database error while getting user by token.");
				return Result.Fail(ex.Message);
			}
			catch (Exception ex)
			{
				//_logger.LogError(ex, "An unexpected error occurred while getting user by token.");
				return Result.Fail(ex.Message);
			}
		}

		public async Task<Result<User>> GetUserByUsernameOrEmailAsync(string usernameOrEmail)
		{
			using IDbConnection db = _dbConnectionFactory.CreateConnection("AuthDb");
			try
			{
				var parameters = new { UsernameOrEmail = usernameOrEmail };

				User? user = await db.QuerySingleOrDefaultAsync<User>("GetUserByUsernameOrEmail", parameters, commandType: CommandType.StoredProcedure);
				if (user is null)
				{
					return Result.Fail("User not found.");
				}
				return Result.Ok(user);
			}
			catch (SqlException ex)
			{
				//_logger.LogError(ex, "Database error while getting user by email.");
				return Result.Fail(ex.Message);
			}
			catch (Exception ex)
			{
				//_logger.LogError(ex, "An unexpected error occurred while getting user by email.");
				return Result.Fail(ex.Message);
			}
		}

		/// <summary>
		/// Updates an existing user in the database.
		/// </summary>
		/// <param name="user"></param>
		/// <returns></returns>
		public async Task<Result> UpdateEmailConfirmationStatusAsync(User user)
		{
			// You need a stored procedure for this, let's call it sp_UpdateUser
			using IDbConnection db = _dbConnectionFactory.CreateConnection("AuthDb");

			try
			{
				var parameters = new
				{
					user.Id,
					user.IsEmailConfirmed,
					user.IsActive,
					user.ConfirmationToken, // To clear the token after use
					user.UpdateDate,
					user.EmailConfirmedAt
			};

				int rowsAffected = await db.ExecuteAsync("UpdateEmailConfirmationStatus", parameters, commandType: CommandType.StoredProcedure);

				// Optional: Check if the update actually affected a row
				if (rowsAffected == 0)
				{
					return Result.Fail("User could not be found to update.");
				}

				//return Result.Ok();
				return Result.Ok().WithSuccess("Email confirmed successfully. You can now log in.");
			}
			catch (SqlException ex)
			{
				//_logger.LogError(ex, "Database error while updating user {UserId}", user.Id);
				return Result.Fail(ex.Message);
			}
		}
	}
}
