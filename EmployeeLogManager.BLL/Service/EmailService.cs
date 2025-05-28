//// EmailService.cs
//using EmployeeLogManager.BLL.ServiceInterfaces;
//using MailKit.Net.Smtp;
//using MailKit.Security;
//using MimeKit;
//using MimeKit.Text;


//public class EmailService : IEmailService
//{
//    public void SendTestEmail(string body)
//    {
//        var email = new MimeMessage();
//        email.From.Add(MailboxAddress.Parse("elijah.quitzon50@ethereal.email"));
//        email.To.Add(MailboxAddress.Parse("elijah.quitzon50@ethereal.email"));
//        email.Subject = "Hangfire Scheduled Email";
//        email.Body = new TextPart(TextFormat.Plain) { Text = body };

//        using var smtp = new SmtpClient();
//        smtp.Connect("smtp.ethereal.email", 587, SecureSocketOptions.StartTls);
//        smtp.Authenticate("elijah.quitzon50@ethereal.email", "kX6qhnsnBgFlRWjzgQk");
//        smtp.Send(email);
//        smtp.Disconnect(true);
//    }
//}






