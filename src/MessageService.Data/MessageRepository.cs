using System;
using System.Threading.Tasks;
using LT.DigitalOffice.MessageService.Data.Interfaces;
using LT.DigitalOffice.MessageService.Data.Provider;
using LT.DigitalOffice.MessageService.Models.Db;
using Microsoft.EntityFrameworkCore;

namespace LT.DigitalOffice.MessageService.Data
{
  public class MessageRepository : IMessageRepository
  {
    private readonly IDataProvider _provider;

    public MessageRepository(IDataProvider provider)
    {
      _provider = provider;
    }

    public async Task<DbMessage> CreateAsync(DbMessage request)
    {
      if (request is null)
      {
        return null;
      }

      _provider.Messages.Add(request);
      await _provider.SaveAsync();

      return request;
    }

    public async Task<DbMessage> GetAsync(Guid id)
    {
      return await _provider.Messages.FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<bool> UpdateAsync(DbMessage model)
    {
      if (model is null)
      {
        return false;
      }

      await _provider.SaveAsync();

      return true;
    }
  }
}
