using LT.DigitalOffice.MessageService.Mappers.Models.Interfaces;
using LT.DigitalOffice.MessageService.Models.Db;
using LT.DigitalOffice.MessageService.Models.Dto.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LT.DigitalOffice.MessageService.Mappers.Models
{
    public class WorkspaceInfoMapper : IWorkspaceInfoMapper
    {
        private readonly IShortChannelInfoMapper _shortChannelInfoMapper;

        public WorkspaceInfoMapper(IShortChannelInfoMapper shortChannelInfoMapper)
        {
            _shortChannelInfoMapper = shortChannelInfoMapper;
        }

        public WorkspaceInfo Map(DbWorkspace workspace, List<ImageInfo> images)
        {
            if (workspace == null)
            {
                throw new ArgumentNullException(nameof(workspace));
            }

            return new WorkspaceInfo
            {
                Id = workspace.Id,
                Name = workspace.Name,
                Image = images.FirstOrDefault(i => i.Id == workspace.ImageId),
                Description = workspace.Description,
                IsActive = workspace.IsActive,
                Channels = workspace.Channels.Select(
                    ch => _shortChannelInfoMapper.Map(ch, images.FirstOrDefault(i => i.Id == ch.ImageId))).ToList()
            };
        }
    }
}
