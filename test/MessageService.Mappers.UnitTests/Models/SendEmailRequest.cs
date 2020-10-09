using LT.DigitalOffice.Broker.Requests;
using System;

namespace LT.DigitalOffice.MessageService.Mappers.UnitTests.Models
{
    class SendEmailRequest : ISendEmailRequest
    {
        public Guid? SenderId { get; set; }
        public string Receiver { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
    }
}
