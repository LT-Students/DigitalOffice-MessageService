using System.Collections.Generic;
using System.Linq;
using LT.DigitalOffice.MessageService.Mappers.Models.Interfaces;
using LT.DigitalOffice.MessageService.Models.Db;
using LT.DigitalOffice.MessageService.Models.Dto.Models.Image;
using LT.DigitalOffice.MessageService.Models.Dto.Models.User;
using LT.DigitalOffice.MessageService.Models.Dto.Responses.Workspace;

namespace LT.DigitalOffice.MessageService.Mappers.Models
{
  public class WorkspaceInfoMapper : IWorkspaceInfoMapper
  {
    private readonly IShortChannelInfoMapper _channelInfoMapper;

    public WorkspaceInfoMapper(
      IShortChannelInfoMapper channelInfoMapper)
    {
      _channelInfoMapper = channelInfoMapper;
    }

    public WorkspaceInfo Map(DbWorkspace dbWorkspace, List<UserInfo> users)
    {
      if (dbWorkspace is null)
      {
        return null;
      }

      ImageConsist image = dbWorkspace.ImageContent is null
        ? null
        : new() { Content = dbWorkspace.ImageContent, Extension = dbWorkspace.ImageExtension };

      return new WorkspaceInfo
      {
        Id = dbWorkspace.Id,
        Name = dbWorkspace.Name,
        Description = dbWorkspace.Description,
        Image = image,
        CreatedAtUtc = dbWorkspace.CreatedAtUtc,
        CreatedBy = users?.FirstOrDefault(u => u.Id == dbWorkspace.CreatedBy),
        IsActive = dbWorkspace.IsActive,
        Channels = dbWorkspace.Channels?
          .Select(_channelInfoMapper.Map).ToList(),
        Users = users?
          .Where(u => dbWorkspace.Users.Any(wu => wu.UserId == u.Id)).ToList()
      };
    }
  }
}
