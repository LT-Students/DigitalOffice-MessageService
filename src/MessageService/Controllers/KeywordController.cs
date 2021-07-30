using LT.DigitalOffice.Kernel.Enums;
using LT.DigitalOffice.Kernel.Responses;
using LT.DigitalOffice.MessageService.Business.Commands.ParseEntity.Interface;
using LT.DigitalOffice.MessageService.Models.Dto.Enums;
using LT.DigitalOffice.MessageService.Models.Dto.Models;
using LT.DigitalOffice.MessageService.Models.Dto.Requests.ParseEntity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Net;

namespace LT.DigitalOffice.MessageService.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class KeywordController : ControllerBase
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public KeywordController(
            IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet("FindParseEntities")]
        public OperationResultResponse<Dictionary<string, Dictionary<string, List<string>>>> FindParseEntities(
            [FromServices] IFindParseEntitiesCommand command)
        {
            return command.Execute();
        }

        [HttpGet("Find")]
        public FindResultResponse<KeywordInfo> FindParsedProperties(
            [FromServices] IFindKeywordCommand command,
            [FromQuery] int skipCount,
            [FromQuery] int takeCount)
        {
            return command.Execute(skipCount, takeCount);
        }

        [HttpPost("Add")]
        public OperationResultResponse<Guid> AddKeyword(
            [FromServices] IAddKeywordCommand command,
            [FromBody] AddKeywordRequest request)
        {
            var result = command.Execute(request);

            if (result.Status != OperationResultStatusType.Failed)
            {
                _httpContextAccessor.HttpContext.Response.StatusCode = (int)HttpStatusCode.Created;
            }

            return result;
        }
    }
}
