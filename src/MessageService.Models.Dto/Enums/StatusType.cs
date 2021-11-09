using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace LT.DigitalOffice.MessageService.Models.Dto.Enums
{
  [JsonConverter(typeof(StringEnumConverter))]
  public enum StatusType
  {
    Sent,
    Read,
    Removed,
    Delivered
  }
}
