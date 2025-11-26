namespace Bank.Balance.API.Domain.Constants
{
    public static class CurrentStateConstants
    {
        public const string PENDING = "Pending";
        public const string COMPLETED = "Completed";
        public const string CANCELED = "Canceled";
    }
    public class ReceivedSubscriptionsConstants
    {
        public const string BALANCE_INITIATED = "balance-initiated";
        public const string TRANSFER_CONFIRMED_BALANCE = "transfer-confirmed-balance";
        public const string TRANSFER_FAILED_BALANCE = "transfer-failed-balance";
    }
    public class SendSubscriptionConstants
    {
        public const string TRANSFER_INITIATED = "transfer-initiated";
        public const string BALANCE_CONFIRMED = "balance-confirmed";
        public const string BALANCE_FAILED = "balance-failed";
    }

}
