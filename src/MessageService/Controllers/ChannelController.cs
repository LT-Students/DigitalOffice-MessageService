using System;
using LT.DigitalOffice.Kernel.Responses;
using LT.DigitalOffice.MessageService.Business.Commands.Channels.Interfaces;
using LT.DigitalOffice.MessageService.Models.Dto.Requests;
using Microsoft.AspNetCore.Mvc;

namespace LT.DigitalOffice.MessageService.Controllers
{
  [Route("[controller]")]
  [ApiController]
  public class ChannelController : ControllerBase
  {

    [HttpPost("create")]
    public OperationResultResponse<Guid?> Create(
      [FromServices] ICreateChannelCommand _command,
      [FromBody] CreateChannelRequest request)
    {
      return _command.Exequte(request);
    }
  }
}
