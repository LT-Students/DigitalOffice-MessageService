using System;
using System.Threading.Tasks;
using LT.DigitalOffice.Kernel.Responses;
using LT.DigitalOffice.MessageService.Business.Commands.Channels.Interfaces;
using LT.DigitalOffice.MessageService.Models.Dto.Filtres;
using LT.DigitalOffice.MessageService.Models.Dto.Requests;
using LT.DigitalOffice.MessageService.Models.Dto.Responses.Channel;
using Microsoft.AspNetCore.Mvc;

namespace LT.DigitalOffice.MessageService.Controllers
{
  [Route("[controller]")]
  [ApiController]
  public class ChannelController : ControllerBase
  {
    [HttpPost("create")]
    public async Task<OperationResultResponse<Guid?>> CreateAsync(
      [FromServices] ICreateChannelCommand command,
      [FromBody] CreateChannelRequest request)
    {
      return await command.ExeсuteAsync(request);
    }

    [HttpGet("get")]
    public async Task<OperationResultResponse<ChannelInfo>> GetAsync(
      [FromServices] IGetChannelCommand command,
      [FromQuery] GetChannelFilter filter)
    {
      return await command.ExeсuteAsync(filter);
    }
  }
}
