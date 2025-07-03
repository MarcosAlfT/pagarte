using Microsoft.EntityFrameworkCore;
using PagarteAPI.Domain.Payment;

namespace PagarteAPI.Infrastructure.Persistence
{
	public class PaymentsDbContext: DbContext
	{
		public PaymentsDbContext(DbContextOptions<PaymentsDbContext> options) : base(options)
		{ }

		public DbSet<PaymentMethod> PaymentMethods { get; set; }
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.Entity<PaymentMethod>(entity =>
			{
				entity.HasIndex(pm => new {pm.UserId, pm.Brand, pm.LastFourDigits}).IsUnique();
			});
			
		}
	}
}
