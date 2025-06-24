namespace PagarteAPI.Domain.Payment
{
	public class Transaction
	{
		public Guid Id { get; set; }
		public Guid PaymentMethodId { get; set; }
		public string BillerId { get; set; } = string.Empty;
		public string BillerAccountId { get; set; } = string.Empty;
		public string Status { get; set; } = string.Empty; // PENDING, COMPLETED, FAILED, FUNDED.
		public decimal Amount { get; set; }
		public decimal FeeTrx { get; set; }
		public decimal FeeInternational { get; set; }
		public string Currency { get; set; } = string.Empty;
		public string? FundsCollectionTxnId { get; set; }
		public string? DisbursementTxnId { get; set; }
		public string? RefundTxnId { get; set; }
		public string? FailureReason { get; set; }
		public DateTime CreatedAt { get; set; }
		public DateTime UpdatedAt { get; set; }
	}
}
