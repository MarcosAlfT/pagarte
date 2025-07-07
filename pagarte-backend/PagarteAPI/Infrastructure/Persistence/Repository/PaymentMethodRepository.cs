using FluentResults;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using PagarteAPI.Application.Interfaces;
using PagarteAPI.Domain.Payment;

namespace PagarteAPI.Infrastructure.Persistence.Repository
{
	public class PaymentMethodRepository(PaymentsDbContext db) : IPaymentMethodRepository
	{
		private readonly PaymentsDbContext _db = db;

		public async Task<Result> AddPaymentMethodAsync(PaymentMethod paymentMethod)
		{
			try
			{
				_db.ChangeTracker.AutoDetectChangesEnabled = false;

				await _db.AddAsync(paymentMethod);
				await _db.SaveChangesAsync();

				return Result.Ok().WithSuccess("Transaction was created successfully");
			}
			catch (DbUpdateException dbEx)
			{
				if (dbEx.InnerException is SqlException sqlEx && sqlEx.Number == 2627)
				{
					// Handle SQL specific exceptions here
					return Result.Fail("This payment method has already been added to your account.");
				}
				// Log the exception details here if needed
				return Result.Fail(new Error("A database error occurred while saving the payment method.").CausedBy(dbEx));
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

		public async Task<Result<IEnumerable<PaymentMethod>>> GetPaymentMethodsByUserIdAsync(Guid userId)
		{
			try
			{
				var newPaymentMethods = await _db.PaymentMethods
					.AsNoTracking()
					.Where(pm => pm.UserId == userId && pm.IsActive == true)
					.OrderBy(pm => pm.IsDefault)
					.ThenByDescending(pm => pm.CreatedAt)
					.ToListAsync();

				return Result.Ok<IEnumerable<PaymentMethod>>(newPaymentMethods);
			}
			catch (Exception ex)
			{
				return Result.Fail(new Error("Unexpected error occurred.").CausedBy(ex));
			}
		}
	}
}
