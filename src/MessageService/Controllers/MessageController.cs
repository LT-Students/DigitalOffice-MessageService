using System.Threading.Tasks;
using LT.DigitalOffice.Kernel.Responses;
using LT.DigitalOffice.MessageService.Business.Commands.Message.Interfaces;
using LT.DigitalOffice.MessageService.Models.Dto.Enums;
using LT.DigitalOffice.MessageService.Models.Dto.Requests.Message;
using Microsoft.AspNetCore.Mvc;

namespace LT.DigitalOffice.MessageService.Controllers
{
  [Route("[controller]")]
  [ApiController]
  public class MessageController : ControllerBase
  {
    [HttpPost("create")]
    public async Task<OperationResultResponse<StatusType>> CreateAsync(
      [FromServices] ICreateMessageCommand command,
      [FromBody] CreateMessageRequest request)
    {
      return await command.ExecuteAsync(request);
    }
  }
}
