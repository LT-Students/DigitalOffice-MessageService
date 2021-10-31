using LT.DigitalOffice.MessageService.Mappers.Models.Interfaces;
using LT.DigitalOffice.MessageService.Models.Db;
using LT.DigitalOffice.MessageService.Models.Dto.Models;
using LT.DigitalOffice.MessageService.Models.Dto.Models.Image;

namespace LT.DigitalOffice.MessageService.Mappers.Models
{
  public class ShortWorkspaceInfoMapper : IShortWorkspaceInfoMapper
  {
    public ShortWorkspaceInfo Map(DbWorkspace dbWorkspace)
    {
      if (dbWorkspace is null)
      {
        return null;
      }

      return new ShortWorkspaceInfo
      {
        Id = dbWorkspace.Id,
        Name = dbWorkspace.Name,
        Description = dbWorkspace.Description,
        IsActive = dbWorkspace.IsActive,
        Image = new ImageConsist()
        {
          Content = dbWorkspace.ImageContent,
          Extension = dbWorkspace.ImageExtension
        }
      };
    }
  }
}
