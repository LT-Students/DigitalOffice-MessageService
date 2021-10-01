using LT.DigitalOffice.Kernel.AccessValidatorEngine.Interfaces;
using LT.DigitalOffice.Kernel.Enums;
using LT.DigitalOffice.Kernel.Exceptions.Models;
using LT.DigitalOffice.Kernel.Responses;
using LT.DigitalOffice.MessageService.Business.Commands.ParseEntity.Interface;
using LT.DigitalOffice.MessageService.Data.Interfaces;
using LT.DigitalOffice.MessageService.Mappers.Models.Interfaces;
using LT.DigitalOffice.MessageService.Models.Db;
using LT.DigitalOffice.MessageService.Models.Dto.Enums;
using LT.DigitalOffice.MessageService.Models.Dto.Models;
using System.Collections.Generic;
using System.Linq;

namespace LT.DigitalOffice.MessageService.Business.Commands.ParseEntity
{
    public class FindKeywordCommand : IFindKeywordCommand
    {
        private readonly IAccessValidator _accessValidator;
        private readonly IKeywordRepository _repository;
        private readonly IKeywordInfoMapper _mapper;

        public FindKeywordCommand(
            IAccessValidator accessValidator,
            IKeywordRepository repository,
            IKeywordInfoMapper mapper)
        {
            _accessValidator = accessValidator;
            _repository = repository;
            _mapper = mapper;
        }

        public FindResultResponse<KeywordInfo> Execute(int skipCount, int takeCount)
        {
            if (!_accessValidator.IsAdmin())
            {
                throw new ForbiddenException("Not enouth rights");
            }

            return new()
            {
                Status = OperationResultStatusType.FullSuccess,
                Body = _repository.Find(skipCount, takeCount, out int totalCount).Select(_mapper.Map).ToList(),
                TotalCount = totalCount
            };
        }
    }
}
