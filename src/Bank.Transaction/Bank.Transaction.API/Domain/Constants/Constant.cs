namespace Bank.Transaction.API.Domain.Constants
{
    public static class CurrentStateConstants
    {
        public const string PENDING = "Pending";
        public const string COMPLETED = "Completed";
        public const string CANCELED = "Canceled";
    }
    public class ReceivedSubscriptionsConstants
    {
        public const string TRANSACTION_INITIATED = "transaction-initiated";
        public const string BALANCE_CONFIRMED = "balance-confirmed";
        public const string BALANCE_FAILED = "balance-failed";
        public const string TRANSFER_FAILED = "transfer-failed";
        public const string TRANSFER_CONFIRMED = "transfer-confirmed";
    }
    public class SendSubscriptionConstants
    {
        public const string BALANCE_INITIATED = "balance-initiated";
        public const string TRANSACTION_FAILED = "transaction-failed";
        public const string TRANSFER_INITIATED = "transfer-initiated";
        public const string TRANSFER_CONFIRMED_BALANCE = "transfer-confirmed-balance";
        public const string TRANSFER_FAILED_BALANCE = "transfer-failed-balance";
        public const string TRANSACTION_COMPLETED = "transaction-completed";
    }

}
