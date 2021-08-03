using LT.DigitalOffice.MessageService.Business.Commands.EmailTemplate.Interfaces;
using LT.DigitalOffice.MessageService.Data.Interfaces;
using LT.DigitalOffice.MessageService.Mappers.Models.Interfaces;
using LT.DigitalOffice.MessageService.Models.Dto.Responses;
using System.Linq;

namespace LT.DigitalOffice.MessageService.Business.Commands.EmailTemplate
{
    public class FindEmailTemplateCommand : IFindEmailTemplateCommand
    {
        private readonly IEmailTemplateRepository _repository;
        private readonly IEmailTemplateInfoMapper _mapper;

        public FindEmailTemplateCommand(
            IEmailTemplateRepository repository,
            IEmailTemplateInfoMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public EmailTemplatesResponse Execute(int skipCount, int takeCount, bool? includeDeactivated)
        {
            return new EmailTemplatesResponse
            {
                Emails = _repository.Find(skipCount, takeCount, includeDeactivated, out int totalCount).Select(_mapper.Map).ToList(),
                TotalCount = totalCount
            };
        }
    }
}
