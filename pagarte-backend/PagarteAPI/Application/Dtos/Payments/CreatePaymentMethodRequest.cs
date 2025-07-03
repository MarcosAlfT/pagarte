namespace PagarteAPI.Application.Dtos.Payments
{
	public class CreatePaymentMethodRequest
	{
		public Guid UserId { get; set; } // The ID from AuthAPI
		public string Brand { get; set; } = string.Empty; // e.g., Visa, Mastercard
		public string CardNumber { get; set; } = string.Empty;
		public string CardHolderName { get; set; } = string.Empty;
		public DateTime ExpirationDate { get; set; }
		public string Cvv { get; set; } = string.Empty;
		public bool IsDefault { get; set; } = false; // Indicates if this is the default payment method
	}
}
