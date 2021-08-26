using System;
using LT.DigitalOffice.MessageService.Mappers.Models.Interfaces;
using LT.DigitalOffice.MessageService.Models.Db;
using LT.DigitalOffice.MessageService.Models.Dto.Models;

namespace LT.DigitalOffice.MessageService.Mappers.Models
{
  public class ShortWorkspaceInfoMapper : IShortWorkspaceInfoMapper
  {
    public ShortWorkspaceInfo Map(DbWorkspace workspace, ImageInfo image)
    {
      if (workspace == null)
      {
        throw new ArgumentNullException(nameof(workspace));
      }

      return new ShortWorkspaceInfo
      {
        Id = workspace.Id,
        Name = workspace.Name,
        Image = image,
        Description = workspace.Description,
        IsActive = workspace.IsActive,
      };
    }
  }
}
