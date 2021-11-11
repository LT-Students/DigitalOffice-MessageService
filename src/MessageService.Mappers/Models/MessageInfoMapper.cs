using System.Collections.Generic;
using System.Linq;
using LT.DigitalOffice.MessageService.Mappers.Models.Interfaces;
using LT.DigitalOffice.MessageService.Models.Db;
using LT.DigitalOffice.MessageService.Models.Dto.Models.Image;
using LT.DigitalOffice.MessageService.Models.Dto.Models.User;
using LT.DigitalOffice.MessageService.Models.Dto.Responses.Message;

namespace LT.DigitalOffice.MessageService.Mappers.Models
{
  public class MessageInfoMapper : IMessageInfoMapper
  {
    public MessageInfo Map(DbMessage dbMessage, UserInfo createdBy, List<ImageInfo> images)
    {
      if (dbMessage is null)
      {
        return null;
      }

      return new()
      {
        Id = dbMessage.Id,
        Content = dbMessage.Content,
        Status = dbMessage.Status,
        ThreadMessagesCount = dbMessage.ThreadMessagesCount,
        CreatedBy = createdBy,
        CreatedAtUtc = dbMessage.CreatedAtUtc,
        FilesIds = dbMessage.Files?.Select(mf => mf.FileId).ToList(),
        Images = images
      };
    }
  }
}
