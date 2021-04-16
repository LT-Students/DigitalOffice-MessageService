using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.MessageService.Models.Dto.Configurations;
﻿using System;

namespace LT.DigitalOffice.Broker.Requests
{
    [AutoInjectRequest(nameof(RabbitMqConfig.CreateImageEndpoint))]
    public interface IAddImageRequest
    {
        public string Name { get; set; }
        public string Content { get; set; }
        public string Extension { get; set; }
        public Guid UserId { get; set; }

        static object CreateObj(
            string name,
            string content,
            string extension,
            Guid userId)
        {
            return new
            {
                Name = name,
                Content = content,
                Extension = extension,
                UserId = userId
            };
        }
    }
}
