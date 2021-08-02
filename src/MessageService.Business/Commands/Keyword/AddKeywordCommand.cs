using LT.DigitalOffice.Kernel.AccessValidatorEngine.Interfaces;
using LT.DigitalOffice.Kernel.Exceptions.Models;
using LT.DigitalOffice.Kernel.FluentValidationExtensions;
using LT.DigitalOffice.Kernel.Responses;
using LT.DigitalOffice.MessageService.Business.Commands.ParseEntity.Interface;
using LT.DigitalOffice.MessageService.Data.Interfaces;
using LT.DigitalOffice.MessageService.Mappers.Db.Interfaces;
using LT.DigitalOffice.MessageService.Models.Dto.Requests.ParseEntity;
using LT.DigitalOffice.MessageService.Validation.ParseEntity.Interfaces;
using System;

namespace LT.DigitalOffice.MessageService.Business.Commands.ParseEntity
{
    public class AddKeywordCommand : IAddKeywordCommand
    {
        private readonly IAccessValidator _accessValidator;
        private readonly IKeywordRepository _repository;
        private readonly IDbKeywordMapper _mapper;
        private readonly IAddKeywordRequestValidator _validator;

        public AddKeywordCommand(
            IAccessValidator accessValidator,
            IKeywordRepository repository,
            IDbKeywordMapper mapper,
            IAddKeywordRequestValidator validator)
        {
            _accessValidator = accessValidator;
            _repository = repository;
            _mapper = mapper;
            _validator = validator;
        }

        public OperationResultResponse<Guid> Execute(AddKeywordRequest request)
        {
            if (!_accessValidator.IsAdmin())
            {
                throw new ForbiddenException("Not enouth rights.");
            }

            _validator.ValidateAndThrowCustom(request);

            return new()
            {
                Status = Kernel.Enums.OperationResultStatusType.FullSuccess,
                Body = _repository.Add(_mapper.Map(request))
            };
        }
    }
}
