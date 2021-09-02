using LT.DigitalOffice.MessageService.Mappers.Models.Interfaces;
using LT.DigitalOffice.MessageService.Models.Db;
using LT.DigitalOffice.MessageService.Models.Dto.Models.Channel;
using LT.DigitalOffice.MessageService.Models.Dto.Models.Workspace;

namespace LT.DigitalOffice.MessageService.Mappers.Models
{
  public class ShortChannelInfoMapper : IShortChannelInfoMapper
  {
    public ShortChannelInfo Map(DbChannel dbChannel)
    {
      if (dbChannel == null)
      {
        return null;
      }

      return new ShortChannelInfo
      {
        Id = dbChannel.Id,
        Name = dbChannel.Name,
        IsActive = dbChannel.IsActive,
        IsPrivate = dbChannel.IsPrivate,
        Avatar = new AvatarData()
        {
          Content = dbChannel.AvatarContent,
          Extension = dbChannel.AvatarExtension
        },
      };
    }
  }
}
