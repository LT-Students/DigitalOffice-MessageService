using LT.DigitalOffice.Kernel.Responses;
using LT.DigitalOffice.MessageService.Business.UnsentEmailCommands.Interfaces;
using LT.DigitalOffice.MessageService.Models.Dto.Responses;
using Microsoft.AspNetCore.Mvc;
using System;

namespace LT.DigitalOffice.MessageService.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UnsentEmailController : ControllerBase
    {
        public OperationResultResponse<bool> Resend(
            [FromServices] IResendCommand command,
            [FromQuery] Guid id)
        {
            return command.Execute(id);
        }

        public UnsentEmailsResponse GetAll(
            [FromServices] IGetAllUnsentEmailsCommand command)
        {
            return command.Execute();
        }

    }
}
