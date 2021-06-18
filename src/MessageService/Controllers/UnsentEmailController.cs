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
        [HttpDelete("resend")]
        public OperationResultResponse<bool> Resend(
            [FromServices] IResendEmailCommand command,
            [FromQuery] Guid unsentEmailId)
        {
            return command.Execute(unsentEmailId);
        }

        [HttpGet("getall")]
        public UnsentEmailsResponse GetAll(
            [FromServices] IFindUnsentEmailsCommand command,
            [FromQuery] int skipCount,
            [FromQuery] int takeCount)
        {
            return command.Execute(skipCount, takeCount);
        }

    }
}
