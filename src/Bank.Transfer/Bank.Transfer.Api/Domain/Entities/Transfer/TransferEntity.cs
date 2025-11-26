namespace Bank.Transfer.Api.Domain.Entities.Transfer
{
    public class TransferEntity
    {
        public int Id { get; set; }
        public required string CorrelationId { get; set; }
        public DateTime TransferDate {  get; set; }
        public required string CurrentState { get; set; }
        public required decimal Amount { get; set; }
        public required string SourceAccount { get; set; }
        public required string DestinationAccount { get; set; }
        public int CustomerId { get; set; }
    }
}
