namespace LT.DigitalOffice.MessageService.Models.Dto.Requests.EmailTemplate
{
    public record EmailTemplateTextRequest
    {
        public string Subject { get; set; }
        public string Text { get; set; }
        public string Language { get; set; }
    }
}
