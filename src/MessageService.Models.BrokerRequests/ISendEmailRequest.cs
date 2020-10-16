using System;

namespace LT.DigitalOffice.Broker.Requests
{
    /// <summary>
    /// Represents request for SendEmail in MassTransit logic.
    /// </summary>
    public interface ISendEmailRequest
    {
        Guid? SenderId { get; }
        string Receiver { get; }
        string Subject { get; }
        string Body { get; }
    }
}