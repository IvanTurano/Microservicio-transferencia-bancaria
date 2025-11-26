namespace Bank.Gateway.Api.Application.Models
{
    public class EndPointModel
    {
        public int CustomerId { get; set; }
        public decimal Amount { get; set; }
        public required string SourceAccount { get; set; }
        public required string DestinationAccount { get; set; }
    }
}
