namespace LT.DigitalOffice.MessageService.Models.Dto
{
    public class Email
    {
        public string SenderEmail { get; set; }
        public string Receiver { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
    }
}