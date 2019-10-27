using food_for_all_api.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace food_for_all_api.Services
{
    public class EmailService
    {
        private GlobalSettingService globalSettingService = new GlobalSettingService();
        private TokenService tokenService = new TokenService();
        private UserService userService = new UserService();

        private int IdEmailAddress = 4;
        private int IdEmailPassword = 5;
        private int IdEmailName = 6;
        private int IdEmailPort = 7;
        private int IdEmailSmtp = 8;
        private int IdEmailPathTemplate = 9;

        public EmailService()
        {

        }

        public bool sendRecoveryPassword(User user, Token token)
        {
            bool result = false;

            string emailAddress = globalSettingService.findById(IdEmailAddress).Value;
            string emailPassword = globalSettingService.findById(IdEmailPassword).Value;
            string emailName = globalSettingService.findById(IdEmailName).Value;
            int emailPort = Convert.ToInt32(globalSettingService.findById(IdEmailPort).Value);
            string emailSmtp = globalSettingService.findById(IdEmailSmtp).Value;
            string pathTemplate = globalSettingService.findById(IdEmailPathTemplate).Value;

            MailAddress from = new MailAddress(emailAddress, emailName);
            MailAddress to = new MailAddress(user.Email, user.UserName);

            SmtpClient smtpClient = new SmtpClient(emailSmtp, emailPort);

            smtpClient.EnableSsl = true;
            smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = new NetworkCredential(emailAddress, emailPassword);


            MailMessage message = new MailMessage(from, to);
            message.Subject = "Restablecer Contraseña.";
            message.Body = bodyRecoveryPassword(user, token, pathTemplate);
            message.IsBodyHtml = true;

            smtpClient.Send(message);

            result = true;

            return result;
        }

        private string bodyRecoveryPassword(User user, Token token, string pathTemplate)
        {
            string body = string.Empty;

            StreamReader streamReader = new StreamReader(@pathTemplate);

            body = streamReader.ReadToEnd();
            body = body.Replace("{to}", user.UserName);
            body = body.Replace("{url}", "http://3.91.208.66/set-password/" + token.Token1);
            body = body.Replace("{date}", DateTime.Now.ToString("yyyy-MM-dd"));

            return body;
        }
    }
}
