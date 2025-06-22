using System.Data;

namespace DataAccess.Shared
{
	public interface IDbConnectionFactory
	{
			IDbConnection CreateConnection(string connectionName);
	}
}
