using System;

namespace LT.DigitalOffice.Broker.Requests
{
    /// <summary>
    /// Represents request for SendEmail in MassTransit logic.
    /// </summary>
    public interface ISendEmailRequest
    {
        Guid? SenderId { get; set; }
        string Receiver { get; set; }
        string Subject { get; set; }
        string Body { get; set; }
    }
}