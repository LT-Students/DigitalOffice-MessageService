using LT.DigitalOffice.Kernel.Attributes;

namespace LT.DigitalOffice.MessageService.Mappers.Helpers.Interfaces
{
  [AutoInject]
  public interface IResizeImageHelper
  {
    string Resize(string inputBase64, string extension);
  }
}
