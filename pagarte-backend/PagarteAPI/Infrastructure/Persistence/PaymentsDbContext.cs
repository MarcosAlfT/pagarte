using Microsoft.EntityFrameworkCore;
using PagarteAPI.Domain.Payment;

namespace PagarteAPI.Infrastructure.Persistence
{
	public class PaymentsDbContext(DbContextOptions<PaymentsDbContext> options) : DbContext(options)
	{
		public DbSet<PaymentMethod> PaymentMethods { get; set; }
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.Entity<PaymentMethod>(entity =>
			{
				entity.HasKey(pm => pm.Id);
				entity.HasIndex(pm => new { pm.UserId, pm.Brand,	pm.LastFourDigits }).IsUnique();
			});
			
		}
	}
}
