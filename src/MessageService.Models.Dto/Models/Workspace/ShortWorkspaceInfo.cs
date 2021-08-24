using LT.DigitalOffice.MessageService.Models.Dto.Models.Channel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LT.DigitalOffice.MessageService.Models.Dto.Models
{
    public record ShortWorkspaceInfo
    {
        public Guid Id { get; set; }
        public ImageInfo ImageId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
