using FluentValidation;
using LT.DigitalOffice.Kernel.FluentValidationExtensions;
using LT.DigitalOffice.MessageService.Business.Interfaces;
using LT.DigitalOffice.MessageService.Data.Interfaces;
using LT.DigitalOffice.MessageService.Mappers.Interfaces;
using LT.DigitalOffice.MessageService.Models.Db;
using LT.DigitalOffice.MessageService.Models.Dto;
using Microsoft.AspNetCore.Mvc;
using System;

namespace LT.DigitalOffice.MessageService.Business
{
    public class CreateWorkspaceCommand : ICreateWorkspaceCommand
    {
        private readonly IWorkspaceRepository _repository;
        private readonly IValidator<Workspace> _validator;
        private readonly IMapper<Workspace, DbWorkspace> _mapper;

        public CreateWorkspaceCommand(
            [FromServices] IWorkspaceRepository repository,
            [FromServices] IValidator<Workspace> validator,
            [FromServices] IMapper<Workspace, DbWorkspace> mapper)
        {
            _repository = repository;
            _validator = validator;
            _mapper = mapper;
        }

        public Guid Execute(Workspace workspace)
        {
            _validator.ValidateAndThrowCustom(workspace);

            return _repository.AddWorkspace(_mapper.Map(workspace));
        }
    }
}
