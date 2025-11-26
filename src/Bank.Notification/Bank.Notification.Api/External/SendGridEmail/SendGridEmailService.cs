using Bank.Notification.Api.Application.External.SendGridEmail;
using System.Net.Http.Headers;
using System.Text;

namespace Bank.Notification.Api.External.SendGridEmail
{
    public class SendGridEmailService : ISendGridEmailService
    {
        private readonly IConfiguration _configuration;

        public SendGridEmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<bool> Execute(string emailPayLoad)
        {
            var apiKey = _configuration["SENDGRIDAPIKEY"];
            var apiUrl = _configuration["SENDGRIDAPIURL"];

            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);
            var content = new StringContent(emailPayLoad, Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync(apiUrl, content);

            return response.IsSuccessStatusCode;
        }


    }
}
