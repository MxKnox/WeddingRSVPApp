namespace WeddingApp.Options;

public enum EmailProvider
{
    SendGrid,
    Smtp
}

public class EmailOptions
{
    public EmailProvider Provider { get; set; } = EmailProvider.Smtp;
    public string FromEmail { get; set; } = "";
    public string FromName { get; set; } = "";
    public SendGridSettings? SendGrid { get; set; }
    public SmtpSettings? Smtp { get; set; }
}

public class SendGridSettings
{
    public string ApiKey { get; set; } = "";
}

public class SmtpSettings
{
    public string Host { get; set; } = "";
    public int Port { get; set; }
    public bool EnableSsl { get; set; } = true;
    public string Username { get; set; } = "";
    public string Password { get; set; } = "";
}