using FluentResults;
using Microsoft.EntityFrameworkCore;
using PagarteAPI.Application.Interfaces;
using PagarteAPI.Domain.Payments;

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
			catch (Exception ex)
			{
				return Result.Fail(new Error("An unexpected error occurred").CausedBy(ex));
			}
			finally
			{
				_db.ChangeTracker.AutoDetectChangesEnabled = true;
			}
		}

		public async Task<Result> CheckCardAvailabilityForCreationAsync(Guid userId, string brand, string last4Digits)
		{
			try
			{
				var exists = await _db.PaymentMethods
					.AsNoTracking()
					.AnyAsync(pm => pm.UserId == userId
												&& pm.Brand == brand
												&& pm.LastFourDigits == last4Digits
												&& pm.IsActive == true);
				if (exists)
				{
					return Result.Fail("This payment method has already been added.");
				}

				return Result.Ok();

			}
			catch (Exception ex)
			{
				return Result.Fail(new Error("An unexpected error occurred").CausedBy(ex));
			}
		}

		public async Task<bool> HasUserPaymentMethod(Guid userId)
		{
			try
			{
				var exists = await _db.PaymentMethods
					.AsNoTracking()
					.AnyAsync(pm => pm.UserId == userId	&& pm.IsActive == true);

				return exists;
			}
			catch (Exception ex)
			{
				throw new Exception("An unexpected error occurred while checking user payment methods.", ex);
			}
		}

		public async Task<Result<PaymentMethod>> GetPaymentMethodByIdAsync(Guid paymentMethodId)
		{
			try
			{
				var paymentMethod = await _db.PaymentMethods
					.AsNoTracking()
					.FirstOrDefaultAsync(pm => pm.Id == paymentMethodId);

				if (paymentMethod == null)
				{
					return Result.Fail("Payment method not found.");
				}

				return Result.Ok(paymentMethod);
			}
			catch (Exception ex)
			{
				return Result.Fail(new Error("An unexpected error occurred").CausedBy(ex));
			}
		}

		public async Task<Result<IEnumerable<PaymentMethod>>> GetPaymentMethodsByUserIdAsync(Guid userId)
		{
			try
			{
				var newPaymentMethods = await _db.PaymentMethods
					.AsNoTracking()
					.Where(pm => pm.UserId == userId
							  && pm.IsActive == true
							  && pm.IsDeleted == false)
					.OrderBy(pm => pm.IsDefault)
					.ThenByDescending(pm => pm.CreatedAt)
					.ToListAsync();

				return Result.Ok<IEnumerable<PaymentMethod>>(newPaymentMethods);
			}
			catch (Exception ex)
			{
				return Result.Fail(new Error("An unexpected error occurred").CausedBy(ex));
			}
		}

		public async Task<Result> UpdatePaymentMethodAsync(PaymentMethod paymentMethod)
		{
			try
			{
				_db.PaymentMethods.Update(paymentMethod);
				var result = await _db.SaveChangesAsync();

				if (result == 0)
					return Result.Fail("Payment method was not deleted");

				return Result.Ok();
			}
			catch(Exception ex)
			{
				return Result.Fail(new Error("An unexpected error occurred").CausedBy(ex));
			}
		}
	}
}
