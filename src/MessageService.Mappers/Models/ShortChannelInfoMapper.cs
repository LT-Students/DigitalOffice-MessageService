using LT.DigitalOffice.MessageService.Mappers.Models.Interfaces;
using LT.DigitalOffice.MessageService.Models.Db;
using LT.DigitalOffice.MessageService.Models.Dto.Models.Image;
using LT.DigitalOffice.MessageService.Models.Dto.Responses.Channel;

namespace LT.DigitalOffice.MessageService.Mappers.Models
{
  public class ShortChannelInfoMapper : IShortChannelInfoMapper
  {
    public ShortChannelInfo Map(DbChannel dbChannel)
    {
      if (dbChannel is null)
      {
        return null;
      }

      return new ShortChannelInfo
      {
        Id = dbChannel.Id,
        Name = dbChannel.Name,
        IsActive = dbChannel.IsActive,
        IsPrivate = dbChannel.IsPrivate,
        Avatar = new ImageConsist()
        {
          Content = dbChannel.ImageContent,
          Extension = dbChannel.ImageExtension
        },
      };
    }
  }
}
