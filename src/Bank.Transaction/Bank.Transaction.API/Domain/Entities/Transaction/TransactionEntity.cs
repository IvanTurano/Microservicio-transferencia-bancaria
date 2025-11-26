namespace Bank.Transaction.API.Domain.Entities.Transaction
{
    public class TransactionEntity
    {
        public int Id { get; set; }
        public required string CorrelationId { get; set; }
        public DateTime TransactionDate { get; set; }
        public required string CurrentState { get; set; }
        public decimal Amount { get; set; }
        public required string SourceAccount { get; set; }
        public required string DestinationAccount { get; set; }
        public int CustomerId { get; set; }
    }
}
