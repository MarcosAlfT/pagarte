using Microsoft.Extensions.DependencyInjection;

namespace Email.Shared
{
	public static class DependencyInjection
	{
		// The API project will have to provide the configuration details.
		public static IServiceCollection AddSharedEmail(this IServiceCollection services,
			string host, int port, string username, string password)
		{
			// We register the service with the specific details it needs.
			services.AddSingleton<IEmailSender>(new SmtpEmailSender(host, port, username, password));

			return services;
		}
	}
}
