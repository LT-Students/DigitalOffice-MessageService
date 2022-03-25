using System;
using System.Linq;
using System.Threading.Tasks;
using LT.DigitalOffice.Kernel.Extensions;
using LT.DigitalOffice.MessageService.Data.Interfaces;
using LT.DigitalOffice.MessageService.Data.Provider;
using LT.DigitalOffice.MessageService.Models.Db;
using LT.DigitalOffice.MessageService.Models.Dto.Filtres;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;

namespace LT.DigitalOffice.MessageService.Data
{
  public class ChannelRepository : IChannelRepository
  {
    private readonly IDataProvider _provider;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ChannelRepository(
      IDataProvider provider,
      IHttpContextAccessor httpContextAccessor)
    {
      _provider = provider;
      _httpContextAccessor = httpContextAccessor;
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

    public async Task<bool> EditAsync(DbChannel channel, JsonPatchDocument<DbChannel> document)
    {
      if (channel is null || document is null)
      {
        return false;
      }

      document.ApplyTo(channel);
      channel.ModifiedBy = _httpContextAccessor.HttpContext.GetUserId();
      channel.ModifiedAtUtc = DateTime.UtcNow;

      await _provider.SaveAsync();

      return true;
    }

    public async Task<DbChannel> GetAsync(Guid channelId, GetChannelFilter filter)
    {
      IQueryable<DbChannel> dbChannel = _provider.Channels.AsQueryable()
        .Include(c => c.Workspace)
        .ThenInclude(w => w.Users.Where(wu => wu.IsActive));

      if (filter.IncludeUsers)
      {
        dbChannel = dbChannel.Include(c => c.Users.Where(cu => cu.IsActive));
      }

      if (filter.IncludeMessages)
      {
        dbChannel = dbChannel
          .Include(c => c.Messages
            .OrderByDescending(m => m.CreatedAtUtc)
            .Skip(filter.SkipCount)
            .Take(filter.TakeCount))
          .ThenInclude(m => m.Images)
          .Include(c => c.Messages)
          .ThenInclude(m => m.Files);
      }

      return await dbChannel
        .FirstOrDefaultAsync(c => c.Id == channelId && c.IsActive);
    }

    public async Task<DbChannel> GetAsync(Guid channelId)
    {
      return await _provider.Channels
        .Include(c => c.Users.Where(u => u.IsActive && u.IsAdmin))
        .FirstOrDefaultAsync(ch => ch.Id == channelId);
    }
  }
}
