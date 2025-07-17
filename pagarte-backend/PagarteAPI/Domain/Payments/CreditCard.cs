using System.ComponentModel.DataAnnotations;

namespace PagarteAPI.Domain.Payments
{
	public class CreditCard
	{
		[Required]
		public string CardNumber { get; set; } = string.Empty;
		[Required] 
		public string Brand { get; set; } = string.Empty; // e.g., Visa, Mastercard
		[Required] 	
		public string CardHolderName { get; set; } = string.Empty;
		[Required]
		public int ExpirationMonth { get; set; } // Expiration month
		[Required]
		public int ExpirationYear { get; set; } // Expiration year
		[Required]
		public string Cvv { get; set; } = string.Empty;
		public string LastFourDigits => CardNumber.Length >= 4 ? CardNumber[^4..] : CardNumber; // Get the last four digits of the card number

	}
}
