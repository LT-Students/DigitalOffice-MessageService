using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace LT.DigitalOffice.MessageService.Business.Commands.Message.Hubs
{
  public class ChatHub : Hub
  {
    public async Task JoinChannel(string channelId)
    {
      await Groups.AddToGroupAsync(Context.ConnectionId, channelId);
    }

    public async Task LeaveChannel(string channelId)
    {
      await Groups.RemoveFromGroupAsync(Context.ConnectionId, channelId);
    }
  }
}
