namespace LT.DigitalOffice.MessageService
{
    public class SmtpCredentialsOptions
    {
        public const string SectionName = "SmtpCredentials";
        public string Host { get; set; }
        public int Port { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}