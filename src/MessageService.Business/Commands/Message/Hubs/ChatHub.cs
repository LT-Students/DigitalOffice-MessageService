using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace LT.DigitalOffice.MessageService.Business.Commands.Message.Hubs
{
  public class ChatHub : Hub
  {
    public Task JoinChannel(string channelId)
    {
      return Groups.AddToGroupAsync(Context.ConnectionId, channelId);
    }

    public Task LeaveChannel(string channelId)
    {
      return Groups.RemoveFromGroupAsync(Context.ConnectionId, channelId);
    }
  }
}
