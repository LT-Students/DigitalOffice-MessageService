using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LT.DigitalOffice.Kernel.Extensions;
using LT.DigitalOffice.MessageService.Data.Interfaces;
using LT.DigitalOffice.MessageService.Data.Provider;
using LT.DigitalOffice.MessageService.Models.Db;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
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

    public async Task<bool> ChannelUserExistAsync(Guid channelId, Guid userId)
    {
      return await _provider.ChannelsUsers
        .Where(u => u.ChannelId == channelId && u.UserId == userId && u.IsActive)
        .AnyAsync();
    }

    public async Task CreateAsync(List<DbChannelUser> dbChannelUsers)
    {
      _provider.ChannelsUsers.AddRange(dbChannelUsers);
      await _provider.SaveAsync();
    }

    public async Task<List<DbChannelUser>> GetByChannelIdAsync(Guid channelId)
    {
      return await _provider.ChannelsUsers.Where(u => u.IsActive && u.ChannelId == channelId).ToListAsync();
    }

    public async Task<bool> IsAdminAsync(Guid channelId, Guid userId)
    {
      return await _provider.ChannelsUsers
        .AnyAsync(u => u.ChannelId == channelId && u.UserId == userId && u.IsActive && u.IsAdmin);
    }

    public async Task<bool> EditAsync(Guid channelId, Guid userId, JsonPatchDocument<DbChannelUser> document)
    {
      DbChannelUser user =
        await _provider.ChannelsUsers.FirstOrDefaultAsync(u => u.ChannelId == channelId && u.UserId == userId);

      if (user is null)
      {
        return false;
      }

      document.ApplyTo(user);
      user.ModifiedBy = _httpContextAccessor.HttpContext.GetUserId();
      user.ModifiedAtUtc = DateTime.UtcNow;

      await _provider.SaveAsync();

      return true;
    }

    public async Task RemoveAsync(Guid workspaceId, List<Guid> usersIds)
    {
      if (usersIds is null)
      {
        return;
      }

      await _provider.ChannelsUsers
        .Include(u => u.WorkspaceUser)
        .Where(u => usersIds.Contains(u.UserId) && u.WorkspaceUser.WorkspaceId == workspaceId)
        .ForEachAsync(u =>
        {
          u.IsActive = false;
          u.ModifiedAtUtc = DateTime.UtcNow;
          u.ModifiedBy = _httpContextAccessor.HttpContext.GetUserId();
        });

      await _provider.SaveAsync();
    }

    public async Task RemoveAsync(Guid channelId, List<DbChannelUser> users)
    {
      if (users is null)
      {
        return;
      }

      Guid userId = _httpContextAccessor.HttpContext.GetUserId();

      users
        .ForEach(u =>
        {
          u.IsActive = false;
          u.ModifiedAtUtc = DateTime.UtcNow;
          u.ModifiedBy = userId;
        });

      await _provider.SaveAsync();
    }
  }
}
