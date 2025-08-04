using System.Net;
using System.Net.Mail;

namespace MySocialPet.Tools
{
        public static class EmailService
        {
        public static void Enviar(string destino, string asunto, string html)
        {
            var to = new MailAddress(destino);
            var from = new MailAddress("falexis.godoy@gmail.com");

            var email = new MailMessage(from, to)
            {
                Subject = asunto,
                Body = html,
                IsBodyHtml = true
            };

            var smtp = new SmtpClient
            {
                Host = "smtp-relay.brevo.com",
                Port = 587,
                Credentials = new NetworkCredential("93ea0d001@smtp-brevo.com", "rckQTtSCXJIRzpjn"),
                DeliveryMethod = SmtpDeliveryMethod.Network,
                EnableSsl = true
            };

            try
            {
                smtp.Send(email);
                Console.WriteLine("Correo enviado correctamente.");
            }
            catch (SmtpException ex)
            {
                Console.WriteLine("SMTP ERROR: " + ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine("GENERAL ERROR: " + ex.Message);
                throw;
            }
        }
    }
    }
    
