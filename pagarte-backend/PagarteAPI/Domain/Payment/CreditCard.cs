namespace PagarteAPI.Domain.Payment
{
	public class CreditCard
	{
		public string CardNumber { get; set; } = string.Empty;
		public string CardHolderName { get; set; } = string.Empty;
		public DateTime ExpirationDate { get; set; }
		public string Cvv { get; set; } = string.Empty;
		public string Brand { get; set; } = string.Empty; // e.g., Visa, Mastercard
		public string LastFourDigits => CardNumber.Length >= 4 ? CardNumber[^4..] : CardNumber; // Get the last four digits of the card number

	}
}
