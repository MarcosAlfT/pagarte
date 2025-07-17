using System.ComponentModel.DataAnnotations;

namespace PagarteAPI.Domain.Payments
{
	public class PaymentMethod
	{
		[Key]
		public Guid Id { get; private set; }
		[Required]
		public Guid UserId { get; private set; } // The ID from AuthAPI
		[Required]
		public string ProviderToken { get; private set; } = string.Empty; // The token from dLocal
		[Required]
		public string Brand { get; private set; } = string.Empty; // e.g., Visa, Mastercard
		[Required]
		public string LastFourDigits { get; private set; } = string.Empty; // The last four digits of the card
		[Required]
		public bool IsDefault { get; private set; } = false; // Indicates if this is the default payment method
		[Required]
		public bool IsActive { get; private set; } = false; // Indicates if the payment method is active
		[Required]
		public DateTime CreatedAt { get; private set; }
		public DateTime UpdatedAt { get; private set; } // The last time the payment method changed to isActive = false
		public bool IsDeleted { get; private set; } = false; // Indicates if the payment method is deleted

		public static PaymentMethod CreatePaymentMethod(Guid userId, 
			string providerToken, string brand, string lastFourDigits, bool isDefault)
		{
			return new PaymentMethod
			{
				Id = Guid.NewGuid(),
				UserId = userId,
				ProviderToken = providerToken,
				Brand = brand,
				LastFourDigits = lastFourDigits,
				IsDefault = isDefault,
				IsActive = true,
				CreatedAt = DateTime.UtcNow,
			};
		}
		public void SoftDelete()
		{ 
			this.IsActive = false;
			this.IsDeleted = true;
			this.UpdatedAt = DateTime.UtcNow;
		}

	}
}
