using Bank.Notification.Api.Application.External.SendGridEmail;
using Bank.Notification.Api.Application.Models;
using Bank.Notification.Api.Domain.Constants;
using Bank.Notification.Api.Domain.Entities.Notification;
using Bank.Notification.API.Application.Database;
using Newtonsoft.Json;

namespace Bank.Notification.Api.Application.Features.Process
{
    public class ProcessService : IProcessService
    {
        private readonly IDatabaseService _databaseService;
        private readonly ISendGridEmailService _sendGridEmailService;
        private readonly IConfiguration _configuration;

        public ProcessService(IDatabaseService databaseService, ISendGridEmailService sendGridEmailService, IConfiguration configuration)
        {
            _databaseService = databaseService;
            _sendGridEmailService = sendGridEmailService;
            _configuration = configuration;
        }

        public async Task Execute(string message, string subscription)
        {
            var entity = JsonConvert.DeserializeObject<NotificationEntity>(message);

            string emailPayLoad = string.Empty;
            string fromEmail = _configuration["SENDGRIDFROMEMAIL"];
            string toEmail = "ivansmite12@gmail.com";

            if (subscription.Equals(ReceivedSubscriptionsConstants.TRANSACTION_COMPLETED))
            {
                entity.TransactionState = true;
                entity.Content = "Tu transferencia se ha procesado exitosamente.";

                string status = "Transferencia Exitosa";
                emailPayLoad = CreateSendGridModel.Create(status,entity.Content,fromEmail,toEmail);
            }
            else
            {
                entity.TransactionState = false;
                entity.Content = "Lo siento, ha ocurrido un error, inténtalo más tarde";

                string status = "Transferencia Fallida";
                emailPayLoad = CreateSendGridModel.Create(status, entity.Content, fromEmail, toEmail);
            }

            await _sendGridEmailService.Execute(emailPayLoad);
            await ProcessDatabase(entity);
        }

        public async Task ProcessDatabase(NotificationEntity entity)
        {
            entity.Type = "email";
            await _databaseService.AddAsync(entity);
        }
    }
}
