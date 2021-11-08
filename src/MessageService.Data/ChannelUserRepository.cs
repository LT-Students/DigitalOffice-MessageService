using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LT.DigitalOffice.Kernel.Extensions;
using LT.DigitalOffice.MessageService.Data.Interfaces;
using LT.DigitalOffice.MessageService.Data.Provider;
using LT.DigitalOffice.MessageService.Models.Db;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace LT.DigitalOffice.MessageService.Data
{
  public class ChannelUserRepository : IChannelUserRepository
  {
    private readonly IDataProvider _provider;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ChannelUserRepository(
      IDataProvider provider,
      IHttpContextAccessor httpContextAccessor)
    {
      _provider = provider;
      _httpContextAccessor = httpContextAccessor;
    }

    public async Task<bool> CreateAsync(List<DbChannelUser> dbChannelUsers)
    {
      if (dbChannelUsers == null || !dbChannelUsers.Any())
      {
        return false;
      }

      _provider.ChannelsUsers.AddRange(dbChannelUsers);
      await _provider.SaveAsync();

      return true;
    }

    public async Task<DbChannelUser> GetAsync(Guid userId, Guid channelId)
    {
      return await _provider.ChannelsUsers
        .FirstOrDefaultAsync(x => x.IsActive && x.ChannelId == channelId && x.WorkspaceUserId == userId);
    }

    public async Task<bool> RemoveAsync(Guid channelId, IEnumerable<Guid> usersIds)
    {
      List<DbChannelUser> dbChannelUsers = await _provider.ChannelsUsers
        .Where(cu => cu.IsActive && cu.ChannelId == channelId && usersIds.Contains(cu.WorkspaceUserId))
        .ToListAsync();

      if (!dbChannelUsers.Any())
      {
        return false;
      }

      foreach (DbChannelUser dbChannelUser in dbChannelUsers)
      {
        dbChannelUser.IsActive = false;
        dbChannelUser.ModifiedAtUtc = DateTime.UtcNow;
        dbChannelUser.ModifiedBy = _httpContextAccessor.HttpContext.GetUserId();
      }

      _provider.ChannelsUsers.UpdateRange(dbChannelUsers);
      await _provider.SaveAsync();

      return true;
    }

    public async Task<bool> IsChannelAdminAsync(Guid channelId, Guid userId)
    {
      return await _provider.ChannelsUsers.AnyAsync(cu
        =>
          cu.IsActive &&
          cu.IsAdmin &&
          cu.ChannelId == channelId &&
          cu.WorkspaceUserId == userId);
    }
  }
}
