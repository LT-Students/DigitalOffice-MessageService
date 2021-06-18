using LT.DigitalOffice.Kernel.AccessValidatorEngine.Interfaces;
using LT.DigitalOffice.Kernel.Exceptions.Models;
using LT.DigitalOffice.MessageService.Business.UnsentEmailCommands.Interfaces;
using LT.DigitalOffice.MessageService.Data.Interfaces;
using LT.DigitalOffice.MessageService.Mappers.Models.Interfaces;
using LT.DigitalOffice.MessageService.Models.Db;
using LT.DigitalOffice.MessageService.Models.Dto.Responses;
using System.Collections.Generic;
using System.Linq;

namespace LT.DigitalOffice.MessageService.Business.UnsentEmailCommands
{
    public class FindUnsentEmailsCommand : IFindUnsentEmailsCommand
    {
        private readonly IAccessValidator _accessValidator;
        private readonly IUnsentEmailRepository _repository;
        private readonly IUnsentEmailInfoMapper _unsentEmailMapper;

        public FindUnsentEmailsCommand(
            IAccessValidator accessValidator,
            IUnsentEmailRepository repository,
            IUnsentEmailInfoMapper mapper)
        {
            _accessValidator = accessValidator;
            _repository = repository;
            _unsentEmailMapper = mapper;
        }

        public UnsentEmailsResponse Execute(int skipCount, int takeCount)
        {
            if (!_accessValidator.IsAdmin())
            {
                throw new ForbiddenException("Not enough rights.");
            }

            IEnumerable<DbUnsentEmail> emails = _repository.Find(skipCount, takeCount, out int totalCount);

            return new UnsentEmailsResponse
            {
                TotalCount = totalCount,
                Emails = emails.Select(e => _unsentEmailMapper.Map(e)).ToList()
            };
        }
    }
}
