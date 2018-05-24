using System.Text;
using System.Net.Mail;

namespace MachineRebootAutomation {
    public class MailHandler {
        private SmtpClient smtpClient;
        private string domain;

        // Constructor
        public MailHandler(string hostName, int port, string domain) {
            this.smtpClient = new SmtpClient(hostName) {
                Port = port,
                UseDefaultCredentials = false,
                DeliveryMethod = SmtpDeliveryMethod.Network
            };
            this.domain = domain;
        }

        // Send 
        public void SendMail(string to, string from, string subject, string body) {
            string toMail = to + this.domain;
            string fromMail = from + this.domain;

            MailMessage mail = new MailMessage(fromMail, toMail, subject, body) {
                BodyEncoding = UTF8Encoding.UTF8,
            };
            smtpClient.Send(mail);
        }

    }
}
