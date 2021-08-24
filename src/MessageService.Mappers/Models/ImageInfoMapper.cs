using LT.DigitalOffice.MessageService.Mappers.Models.Interfaces;
using LT.DigitalOffice.MessageService.Models.Dto.Models;
using LT.DigitalOffice.Models.Broker.Models;

namespace LT.DigitalOffice.MessageService.Mappers.Models
{
    public class ImageInfoMapper : IImageInfoMapper
    {
        public ImageInfo Map(ImageData imageData)
        {
            if (imageData == null)
            {
                return null;
            }

            return new ImageInfo
            {
                Id = imageData.ImageId,
                Name = imageData.Name,
                Content = imageData.Content,
                Extension = imageData.Extension,
                ParentId = imageData.ParentId
            };
        }
    }
}
