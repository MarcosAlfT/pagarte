using Dapper;
using DataAccess.Shared;
using FluentResults;
using Microsoft.Data.SqlClient;
using PagarteAPI.Application.Interfaces;
using PagarteAPI.Domain.Payment;
using System.Data;

namespace PagarteAPI.Infrastructure.Persistence.Repository
{
	public class PaymentsRepository(IDbConnectionFactory dbConnectionFactory) : IPaymentRepository
	{
		private readonly IDbConnectionFactory _dbConnectionFactory = dbConnectionFactory;

		public async Task<Result> CreateAsync(Transaction transaction)
		{
			using IDbConnection db = _dbConnectionFactory.CreateConnection("PaymentsDb");

			try
			{
				var parameters = new
				{
					transaction.Id,
					transaction.PaymentMethodId,
					transaction.BillerId,
					transaction.BillerAccountId,
					transaction.Status,
					transaction.Amount,
					transaction.FeeTrx,
					transaction.FeeInternational,
					transaction.Currency,
					transaction.FundsCollectionTxnId,
					transaction.DisbursementTxnId,
					transaction.RefundTxnId,
					transaction.FailureReason,
					transaction.CreatedAt,
					transaction.UpdatedAt
				};

				await db.ExecuteAsync("CreateTransaction", parameters, commandType: CommandType.StoredProcedure);

				return Result.Ok().WithSuccess("Transaction was created successfully");
			}
			catch (SqlException sqlEx)
			{
				return Result.Fail(new Error("Database error occurred").CausedBy(sqlEx));
			}
			catch (Exception ex)
			{
				return Result.Fail(new Error("An unexpected error occurred").CausedBy(ex));
			}
		}
		public async Task<Result<Transaction>> GetByIdAsync(Guid id)
		{
			using IDbConnection db = _dbConnectionFactory.CreateConnection("PaymentsDb");
			
			try
			{
				var parameters = new { Id = id };

				var transaction = await db.QuerySingleOrDefaultAsync<Transaction>("GetTransactionById", parameters, commandType: CommandType.StoredProcedure);
				
				if (transaction == null)
				{
					return Result.Fail(new Error("Transaction not found"));
				}
				return Result.Ok(transaction);
			}
			catch (SqlException sqlEx)
			{
				return Result.Fail(new Error("Database error occurred").CausedBy(sqlEx));
			}
			catch (Exception ex)
			{
				return Result.Fail(new Error("An unexpected error occurred").CausedBy(ex));
			}
		}
		public async Task<Result> UpdateAsync(Transaction transaction)
		{
			using IDbConnection db = _dbConnectionFactory.CreateConnection("PaymentsDb");

			try
			{
				var parameters = new
				{
					transaction.Id,
					transaction.Status,
					transaction.FundsCollectionTxnId,
					transaction.DisbursementTxnId,
					transaction.RefundTxnId,
					transaction.FailureReason,
					transaction.UpdatedAt
				};

				var affectedRows = await db.ExecuteAsync("UpdateTransactionStatus", parameters, commandType: CommandType.StoredProcedure);

				if (affectedRows == 0)
				{
					return Result.Fail(new Error("Transaction not found or could nto be updated"));
				}

				return Result.Ok().WithSuccess("Transaction updated successfully");
			}
			catch (SqlException sqlEx)
			{
				return Result.Fail(new Error("Database error occurred").CausedBy(sqlEx));
			}
			catch (Exception ex)
			{
				return Result.Fail(new Error("An unexpected error occurred").CausedBy(ex));
			}
		}
	}
}
