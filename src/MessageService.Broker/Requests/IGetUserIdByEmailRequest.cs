namespace LT.DigitalOffice.Broker.Requests
{
    /// <summary>
    /// Represents request for GetUserIdByEmail in MassTransit logic.
    /// </summary>
    public interface IGetUserIdByEmailRequest
    {
        string UserEmail { get; }
    }
}
