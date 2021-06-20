using LT.DigitalOffice.Kernel.Responses;
using LT.DigitalOffice.MessageService.Business.Commands.UnsentEmail.Interfaces;
using LT.DigitalOffice.MessageService.Models.Dto.Responses;
using Microsoft.AspNetCore.Mvc;
using System;

namespace LT.DigitalOffice.MessageService.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UnsentEmailController : ControllerBase
    {
        [HttpDelete("resend")]
        public OperationResultResponse<bool> Resend(
            [FromServices] IResendEmailCommand command,
            [FromQuery] Guid unsentEmailId)
        {
            return command.Execute(unsentEmailId);
        }

        [HttpGet("find")]
        public UnsentEmailsResponse Find(
            [FromServices] IFindUnsentEmailsCommand command,
            [FromQuery] int skipCount,
            [FromQuery] int takeCount)
        {
            return command.Execute(skipCount, takeCount);
        }

    }
}
