using Microsoft.Extensions.Configuration;
using MimeKit;
using MailKit.Net.Smtp;
using MailKit.Security;
using EmployeeLogManager.BLL.ServiceInterfaces;

public class EmailSenderService : IEmailSenderService
{
    private readonly IConfiguration _config; //read email setting from attsettinhg.json

    public EmailSenderService(IConfiguration config)
    {
        _config = config;
    }

    public async Task SendEmailAsync(string to, string subject, string body)
    {
        //create email msg obj.
        var email = new MimeMessage();

        email.From.Add(new MailboxAddress("Admin", _config["Email:Username"]));// who will send(user from appsettings)
        email.To.Add(MailboxAddress.Parse(to));//we will receive
        email.Subject = subject;  // mail subject line  
        email.Body = new TextPart("plain") { Text = body };//mail body

        //Creates the SMTP client for sending the email.
        using var smtp = new SmtpClient();

        //Connects securely to the SMTP server(like Gmail, Outlook, or Ethereal).
        //Uses STARTTLS for encryption.
        await smtp.ConnectAsync(
            _config["Email:Host"],
            int.Parse(_config["Email:Port"]),
            SecureSocketOptions.StartTls
        );

        //Authenticates the SMTP client using the provided username and password(appsetting).
        await smtp.AuthenticateAsync(
            _config["Email:Username"],
            _config["Email:Password"]
        );

        //send the email
        await smtp.SendAsync(email);

        //Disconnects from the SMTP server.
        await smtp.DisconnectAsync(true);
    }
}
