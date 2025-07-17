namespace PagarteAPI.Application.Dtos.Payments
{
	public class CreatePaymentMethodRequest
	{
		public string Brand { get; set; } = string.Empty; // e.g., Visa, Mastercard
		public string CardNumber { get; set; } = string.Empty;
		public string CardHolderName { get; set; } = string.Empty;
		public int ExpirationMonth { get; set; } = 0;
		public int ExpirationYear { get; set; } = 0;
		public string Cvv { get; set; } = string.Empty;
	}
}
