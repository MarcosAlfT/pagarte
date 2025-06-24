namespace PagarteAPI.Domain.Payment
{
	public class PaymentMethod
	{
		public Guid Id { get; set; }
		public Guid UserId { get; set; } // The ID from AuthAPI
		public string ProviderToken { get; set; } = string.Empty; // The token from dLocal
		public string? CardType { get; set; }
		public string? LastFourDigits { get; set; }
		public bool IsDefault { get; set; }
		public bool IsActive { get; set; } = false;
		public DateTime CreatedAt { get; set; }
		public DateTime UpdatedAt { get; set; } // The last time the payment method changed to isActive = false
	}
}
