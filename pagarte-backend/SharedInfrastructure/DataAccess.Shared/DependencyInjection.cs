using System;
using Microsoft.Extensions.DependencyInjection;
namespace DataAccess.Shared
{
	public static class DependencyInjection
	{
		public static IServiceCollection AddSharedDataAccess(this IServiceCollection services)
		{
			// This tells the DI system: "When someone asks for an IDbConnectionFactory,
			services.AddSingleton<IDbConnectionFactory, DbConnectionFactory>();

			return services;
		}
	}
}
