namespace LT.DigitalOffice.MessageService.Models.Dto.Requests.EmailTemplateText
{
  public record EditEmailTemplateTextRequest
  {
    public string Subject { get; set; }
    public string Text { get; set; }
    public string Language { get; set; }
  }
}
