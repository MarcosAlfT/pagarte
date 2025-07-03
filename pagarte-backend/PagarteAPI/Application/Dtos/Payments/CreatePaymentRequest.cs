using System.ComponentModel.DataAnnotations;

namespace PagarteAPI.Application.Dtos.Payments
{
	public class CreatePaymentRequest
	{
		[Required]
		public Guid PaymentMethodId { get; set; }

		[Required]
		public string BillerId { get; set; } = string.Empty;

		[Required]
		public string BillerAccountId { get; set; } = string.Empty;

		[Required]
		[Range(0.01, 100000.00)]
		public decimal Amount { get; set; }

		[Required]
		public string Currency { get; set; } = string.Empty;
	}
}
