using IdentityService.Domain;
using FluentResults;
using IdentityService.Dtos.Auth;

namespace IdentityService.Application.Interfaces
{
	public interface IUserRepository
	{
		Result Create(User user);
		Result<UserExistenceDto> CheckExistence(string userName, string email);
		Result UpdateEmailConfirmationStatus(User user);
		Result<User> GetUserByToken(string token);
		Result<User> GetUserByUsernameOrEmail(string userEmailOrUsername);

		//Result<User> GetUserById(Guid userId);
		//Result Update(User user);
		//Result Delete(Guid userId);
		//Result<string> Login(LoginRequest user);
		//Result<User> GetUserByUsernameOrEmail(string email);
		//Result<User> GetUserByUsername(string username);
		//Result<User> GetUserByUsernameOrEmail(string email, string username);
		//Result<User> GetUserByEmailOrUsernameAndPassword(string email, string username, string password);
		//Result<User> GetUserByEmailAndPassword(string email, string password);
		//Result<User> GetUserByUsernameAndPassword(string username, string password);
		//Result<User> GetUserByIdAndPassword(Guid userId, string password);
		//Result<User> GetUserByIdAndPassword(Guid userId, string password, byte[] rowVersionStamp);
		//Result<User> GetUserByIdAndPassword(Guid userId, string password, byte[] rowVersionStamp, bool isActiveCheck = true);
		//Result<User> GetUserByIdAndPassword(Guid userId, string password, byte[] rowVersionStamp, bool isActiveCheck = true, bool isEmailConfirmedCheck = true);
		//Result<User> GetUserByIdAndPassword(Guid userId, string password, byte[] rowVersionStamp, bool isActiveCheck = true, bool isEmailConfirmedCheck = true, bool isTokenCheck = true);

	}
}
