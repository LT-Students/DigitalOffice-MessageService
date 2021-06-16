using LT.DigitalOffice.Kernel.AccessValidatorEngine.Interfaces;
using LT.DigitalOffice.MessageService.Data.Interfaces;
using LT.DigitalOffice.MessageService.Mappers.Models.Interfaces;

namespace LT.DigitalOffice.MessageService.Business.UnsentEmailCommands
{
    public class GetAllUnsentEmailsCommand
    {
        private readonly IAccessValidator _accessValidator;
        private readonly IUnsentEmailRepository _repository;
        private readonly IUnsentEmailInfoMapper _mapper;

        public GetAllUnsentEmailsCommand(
            IAccessValidator accessValidator,
            IUnsentEmailRepository repository,
            IUnsentEmailInfoMapper mapper)
        {
            _accessValidator = accessValidator;
            _repository = repository;
            _mapper = mapper;
        }
    }
}
