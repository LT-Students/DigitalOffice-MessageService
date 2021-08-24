using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.MessageService.Models.Db;
using LT.DigitalOffice.MessageService.Models.Dto.Models;
using System.Collections.Generic;

namespace LT.DigitalOffice.MessageService.Mappers.Models.Interfaces
{
    [AutoInject]
    public interface IWorkspaceInfoMapper
    {
        WorkspaceInfo Map(DbWorkspace workspace, List<ImageInfo> images);
    }
}
