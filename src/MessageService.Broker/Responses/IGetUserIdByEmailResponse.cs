using System;

namespace LT.DigitalOffice.Broker.Responses
{
    /// <summary>
    /// Represents response for GetUserIdByEmail in MassTransit logic.
    /// </summary>
    public interface IGetUserIdByEmailResponse
    {
        Guid UserId { get; }
    }
}