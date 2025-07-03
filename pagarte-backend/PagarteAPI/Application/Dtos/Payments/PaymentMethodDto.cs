using System.ComponentModel.DataAnnotations;

namespace PagarteAPI.Application.Dtos.Payments
{
	public class PaymentMethodDto
	{
		public Guid Id { get; set; }
		public Guid UserId { get; set; } // The ID from AuthAPI
		public string Brand { get; set; } = string.Empty; // e.g., Visa, Mastercard
		public string LastFourDigits { get; set; } = string.Empty; // The last four digits of the card
		public bool IsDefault { get; set; } = false; // Indicates if this is the default payment method
		public bool IsActive { get; set; } = false; // Indicates if the payment method is active

	}
}
