using System.Diagnostics;
using System.Net;
using System.Net.Mail;

namespace war_game.SMTP {
    public class SmtpService {
        public SmtpClient Client { get; private set; }

        public string Sender { get; private set; } 

        public SmtpService(string user, string pass) {
            var smtp = Client = new SmtpClient("smtp.gmail.com", 587);
            smtp.EnableSsl = true;
            smtp.Credentials = new NetworkCredential(user, pass);
            Sender = user;
        }

        public bool SendVerifyMail(string receiver, string refUrl) {
            try {
                using var msg = new MailMessage(Sender, receiver, "대충 이메일 확인", $"ㅇㅇ {refUrl}");
                Client.Send(msg);
                return true;
            } catch (Exception ex) {
                Trace.TraceError($"SendVeriyMail {receiver} {ex}");
                return false;
            }
        }
    }
}
