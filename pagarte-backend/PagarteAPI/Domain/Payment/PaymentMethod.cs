using System.ComponentModel.DataAnnotations;

namespace PagarteAPI.Domain.Payment
{
	public class PaymentMethod
	{
		[Key]
		public Guid Id { get; set; }
		[Required]
		public Guid UserId { get; set; } // The ID from AuthAPI
		[Required]
		public string ProviderToken { get; set; } = string.Empty; // The token from dLocal
		[Required]
		public string Brand { get; set; } = string.Empty; // e.g., Visa, Mastercard
		[Required]
		public string LastFourDigits { get; set; } = string.Empty; // The last four digits of the card
		[Required]
		public bool IsDefault { get; set; } = false; // Indicates if this is the default payment method
		[Required]
		public bool IsActive { get; set; } = false; // Indicates if the payment method is active
		[Required]
		public DateTime CreatedAt { get; set; }
		public DateTime UpdatedAt { get; set; } // The last time the payment method changed to isActive = false
		public bool IsDeleted { get; set; } = false; // Indicates if the payment method is deleted
		
	}
}
