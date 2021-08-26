using LT.DigitalOffice.MessageService.Mappers.Models.Interfaces;
using LT.DigitalOffice.MessageService.Models.Db;
using LT.DigitalOffice.MessageService.Models.Dto.Models;
using LT.DigitalOffice.MessageService.Models.Dto.Models.Channel;

namespace LT.DigitalOffice.MessageService.Mappers.Models
{
  public class ShortChannelInfoMapper : IShortChannelInfoMapper
  {
    public ShortChannelInfo Map(DbChannel channel, ImageInfo image)
    {
      if (channel == null)
      {
        return null;
      }

      return new ShortChannelInfo
      {
        Id = channel.Id,
        Image = image,
        Name = channel.Name,
        IsActive = channel.IsActive,
        IsPrivate = channel.IsPrivate
      };
    }
  }
}
