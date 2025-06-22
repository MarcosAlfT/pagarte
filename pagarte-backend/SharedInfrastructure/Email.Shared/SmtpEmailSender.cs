using System.Net.Mail;
using System.Net;

namespace Email.Shared 
{ 
	public class SmtpEmailSender(string host, int port, string username, string password) : IEmailSender
	{
		public void SendEmail(string email, string subject, string body)
		{
			using var client = new SmtpClient(host, port);
			client.Credentials = new NetworkCredential(username, password);
			client.EnableSsl = true;

			var mailMessage = new MailMessage
			{
				From = new MailAddress(username),
				Subject = subject,
				Body = body,
				IsBodyHtml = true,
			};
			mailMessage.To.Add(email);

			client.Send(mailMessage);
		}
	}
}
