using Newtonsoft.Json;

namespace Bank.Notification.Api.Application.Models
{
    public static class CreateSendGridModel
    {
        public static string Create(string status, string textPart, string fromEmail, string toEmail)
        {
            string htmlTemplate = File.ReadAllText("Template/template-email.html");

            string htmlBody = htmlTemplate
                .Replace("{{STATUS}}", status)
                .Replace("{{MESSAGE}}", textPart)
                .Replace("{{DATE}}", DateTime.UtcNow.ToString("dd/MM/yyyy HH:mm"));


            var emailPayLoad = new
            {
                from = new { email = fromEmail },
                personalizations = new List<object>
                {
                    new
                    {
                        to = new List<object>{new {email = toEmail}},
                        subject = "Transferencia bancaria"
                    }
                },
                content = new List<object>
                {
                    new {type = "text/html", value = htmlBody}
                }

            };

            return JsonConvert.SerializeObject(emailPayLoad);

        }
    }
}
