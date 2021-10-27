using System;
using System.Threading.Tasks;
using LT.DigitalOffice.MessageService.Data.Interfaces;
using LT.DigitalOffice.MessageService.Data.Provider;
using LT.DigitalOffice.MessageService.Models.Db;

namespace LT.DigitalOffice.MessageService.Data
{
  public class MessageRepository : IMessageRepository
  {
    private readonly IDataProvider _provider;

    public MessageRepository(IDataProvider provider)
    {
      _provider = provider;
    }

    public async Task<Guid?> CreateAsync(DbMessage request)
    {
      if (request == null)
      {
        return null;
      }

      _provider.Messages.Add(request);
      await _provider.SaveAsync();

      return request.Id;
    }
  }
}
