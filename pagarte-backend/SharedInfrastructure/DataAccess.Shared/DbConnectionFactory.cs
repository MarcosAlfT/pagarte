using Microsoft.Extensions.Configuration;
using System.Data;
using Microsoft.Data.SqlClient;

namespace DataAccess.Shared
{
	public class DbConnectionFactory(IConfiguration configuration) : IDbConnectionFactory
	{
		public IDbConnection CreateConnection(string connectionName)
		{
			// This line reads from the "ConnectionStrings" section of your appsettings.json
			// For example, if connectionName is "AuthDb", it gets the long string we created.
			string? connectionString = configuration.GetConnectionString(connectionName);

			if (string.IsNullOrEmpty(connectionString))
			{
				throw new InvalidOperationException(
					$"Connection string for '{connectionName}' not found in configuration.");
			}

			return new SqlConnection(connectionString);
		}
	}
}
