using System;
using System.Threading.Tasks;
using LT.DigitalOffice.Kernel.Responses;
using LT.DigitalOffice.MessageService.Business.Commands.Channels.Interfaces;
using LT.DigitalOffice.MessageService.Models.Dto.Filtres;
using LT.DigitalOffice.MessageService.Models.Dto.Requests;
using LT.DigitalOffice.MessageService.Models.Dto.Requests.Channel;
using LT.DigitalOffice.MessageService.Models.Dto.Responses.Channel;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace LT.DigitalOffice.MessageService.Controllers
{
  [Route("[controller]")]
  [ApiController]
  public class ChannelController : ControllerBase
  {
    [HttpPost]
    public async Task<OperationResultResponse<Guid?>> CreateAsync(
      [FromServices] ICreateChannelCommand command,
      [FromBody] CreateChannelRequest request)
    {
      return await command.ExeсuteAsync(request);
    }

    [HttpGet("{channelId}")]
    public async Task<OperationResultResponse<ChannelInfo>> GetAsync(
      [FromServices] IGetChannelCommand command,
      [FromRoute] Guid channelId,
      [FromQuery] GetChannelFilter filter)
    {
      return await command.ExeсuteAsync(channelId, filter);
    }

    [HttpPatch("{channelId}")]
    public async Task<OperationResultResponse<bool>> EditAsync(
      [FromServices] IEditChannelCommand command,
      [FromRoute] Guid channelId,
      [FromBody] JsonPatchDocument<EditChannelRequest> document)
    {
      return await command.ExecuteAsync(channelId, document);
    }
  }
}
