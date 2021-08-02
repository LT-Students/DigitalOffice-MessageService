using LT.DigitalOffice.Kernel.AccessValidatorEngine.Interfaces;
using LT.DigitalOffice.Kernel.Enums;
using LT.DigitalOffice.Kernel.Exceptions.Models;
using LT.DigitalOffice.Kernel.Responses;
using LT.DigitalOffice.MessageService.Broker.Helpers.ParseEntity;
using LT.DigitalOffice.MessageService.Business.Commands.ParseEntity.Interface;
using System;
using System.Collections.Generic;

namespace LT.DigitalOffice.MessageService.Business.Commands.ParseEntity
{
    public class FindParseEntitiesCommand : IFindParseEntitiesCommand
    {
        private readonly IAccessValidator _accessValidator;

        public FindParseEntitiesCommand(
            IAccessValidator accessValidator)
        {
            _accessValidator = accessValidator;
        }

        public OperationResultResponse<Dictionary<string, Dictionary<string, List<string>>>> Execute()
        {
            if (!_accessValidator.IsAdmin())
            {
                throw new ForbiddenException("Not enouth rights");
            }

            Dictionary<string, Dictionary<string, List<string>>> response = new();

            foreach(KeyValuePair<string, Dictionary<string, List<string>>> service in AllParseEntities.Entities)
            {
                response.Add(service.Key, new());

                foreach(KeyValuePair<string, List<string>> entity in service.Value)
                {
                    response[service.Key].Add(entity.Key[2..], entity.Value);
                }
            }

            return new OperationResultResponse<Dictionary<string, Dictionary<string, List<string>>>>
            {
                Status = OperationResultStatusType.FullSuccess,
                Body = response
            };
        }
    }
}
