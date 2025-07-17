using FluentResults;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using PagarteAPI.Application.Interfaces;
using PagarteAPI.Domain.Payments;
using System.Data;

namespace PagarteAPI.Infrastructure.Persistence.Repository
{
	public class PaymentsRepository(PaymentsDbContext db) : IPaymentRepository
	{
		private readonly PaymentsDbContext _db = db;

		public async Task<Result> AddTransactionAsync(Transaction trx)
		{
			try
			{
				_db.ChangeTracker.AutoDetectChangesEnabled = false;

				await _db.AddAsync(trx);
				await _db.SaveChangesAsync();

				return Result.Ok().WithSuccess("Transaction was created successfully");
			}
			catch (Exception ex)
			{
				return Result.Fail(new Error("An unexpected error occurred").CausedBy(ex));
			}
			finally
			{
				_db.ChangeTracker.AutoDetectChangesEnabled = true;
			}
		}

		public Task<Result<Transaction>> GetTransactionsByUserIdAsync(Guid userId)
		{
			throw new NotImplementedException();
		}
	}
}
