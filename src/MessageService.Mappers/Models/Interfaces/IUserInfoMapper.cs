using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.MessageService.Models.Dto.Models;
using LT.DigitalOffice.MessageService.Models.Dto.Models.User;
using LT.DigitalOffice.Models.Broker.Models;

namespace LT.DigitalOffice.MessageService.Mappers.Models.Interfaces
{
  [AutoInject]
  public interface IUserInfoMapper
  {
    UserInfo Map(UserData user, ImageInfo image);
  }
}
