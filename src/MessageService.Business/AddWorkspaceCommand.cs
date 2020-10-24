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
    public class AddWorkspaceCommand : IAddWorkspaceCommand
    {
        private readonly IWorkspaceRepository repository;
        private readonly IValidator<AddWorkspaceRequest> validator;
        private readonly IMapper<AddWorkspaceRequest, DbWorkspace> mapper;

        public AddWorkspaceCommand(
            [FromServices] IWorkspaceRepository repository,
            [FromServices] IValidator<AddWorkspaceRequest> validator,
            [FromServices] IMapper<AddWorkspaceRequest, DbWorkspace> mapper)
        {
            this.repository = repository;
            this.validator = validator;
            this.mapper = mapper;
        }

        public Guid Execute(AddWorkspaceRequest workspace)
        {
            validator.ValidateAndThrowCustom(workspace);

            return repository.AddWorkspace(mapper.Map(workspace));
        }
    }
}
