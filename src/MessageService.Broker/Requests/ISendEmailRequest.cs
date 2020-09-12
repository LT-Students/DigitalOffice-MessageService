namespace LT.DigitalOffice.Broker.Requests
{
    /// <summary>
    /// Represents request for SendEmail in MassTransit logic.
    /// </summary>
    public interface ISendEmailRequest
    {
        string Title { get; }
        string Content { get; }
        string SenderEmail { get; }
        string RecipientEmail { get; }
    }
}
