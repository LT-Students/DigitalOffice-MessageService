namespace LT.DigitalOffice.MessageService
{
    public class NetworkCredentialsOptions
    {
        public const string NetworkCredentials = "NetworkCredentials";
        public string Host { get; set; }
        public int Port { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}