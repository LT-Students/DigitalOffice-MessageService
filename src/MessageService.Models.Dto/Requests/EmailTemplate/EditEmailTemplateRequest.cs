using LT.DigitalOffice.Models.Broker.Enums;

namespace LT.DigitalOffice.MessageService.Models.Dto.Requests.EmailTemplate
{
  public record EditEmailTemplateRequest
  {
    public string Name { get; set; }
    public EmailTemplateType Type { get; set; }
    public bool IsActive { get; set; }
  }
}
