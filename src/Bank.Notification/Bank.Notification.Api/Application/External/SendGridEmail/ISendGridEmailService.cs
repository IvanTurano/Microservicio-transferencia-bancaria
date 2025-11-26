namespace Bank.Notification.Api.Application.External.SendGridEmail
{
    public interface ISendGridEmailService
    {
        Task<bool> Execute(string emailPayLoad);
    }
}
