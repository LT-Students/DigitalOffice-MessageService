namespace LT.DigitalOffice.Broker.Requests
{
    /// <summary>
    /// Represents request for SendEmail in MassTransit logic.
    /// </summary>
    public interface ISendEmailRequest
    {
        string SenderEmail { get; }
        string Receiver { get; }
        string Subject { get; }
        string Body { get; }
    }
}