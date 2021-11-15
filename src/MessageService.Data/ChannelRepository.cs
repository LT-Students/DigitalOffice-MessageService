using System;
using System.Linq;
using System.Threading.Tasks;
using LT.DigitalOffice.MessageService.Data.Interfaces;
using LT.DigitalOffice.MessageService.Data.Provider;
using LT.DigitalOffice.MessageService.Models.Db;
using LT.DigitalOffice.MessageService.Models.Dto.Filtres;
using Microsoft.EntityFrameworkCore;

namespace LT.DigitalOffice.MessageService.Data
{
  public class ChannelRepository : IChannelRepository
  {
    private readonly IDataProvider _provider;

    public ChannelRepository(IDataProvider provider)
    {
      _provider = provider;
    }

    public async Task<Guid?> CreateAsync(DbChannel dbChannel)
    {
      if (dbChannel is null)
      {
        return null;
      }

      _provider.Channels.Add(dbChannel);
      await _provider.SaveAsync();

      return dbChannel.Id;
    }

    public async Task<DbChannel> GetAsync(GetChannelFilter filter)
    {
      IQueryable<DbChannel> dbChannel = _provider.Channels.AsQueryable();

      /*dbChannel = dbChannel.Include(c => c.Workspace).ThenInclude(w => w.Users);

      dbChannel = dbChannel
        .Include(c => c.Users.Where(cu => cu.IsActive))
        .ThenInclude(cu => cu.WorkspaceUser);*/

      dbChannel = dbChannel
        .Include(c => c.Messages
          .OrderByDescending(m => m.CreatedAtUtc)
          .Skip(filter.SkipMessages)
          .Take(filter.TakeMessages))
        .ThenInclude(m => m.Images);
        /*.Include(c => c.Messages)
        .ThenInclude(cm => cm.Files);*/

      return await dbChannel
        .FirstOrDefaultAsync(c => c.Id == filter.ChannelId && c.IsActive);
    }
  }
}
