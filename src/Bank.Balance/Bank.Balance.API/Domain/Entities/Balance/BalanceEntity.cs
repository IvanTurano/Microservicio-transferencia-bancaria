namespace Bank.Balance.API.Domain.Entities.Balance
{
    public class BalanceEntity
    {
        public int Id { get; set; }
        public required string CorrelationId { get; set; }
        public DateTime BalanceDate { get; set; }
        public required string CurrentState { get; set; }
        public int CustomerId { get; set; }
    }
}
