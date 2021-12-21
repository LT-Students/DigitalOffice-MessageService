using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using LT.DigitalOffice.Kernel.BrokerSupport.Broker;
using LT.DigitalOffice.Kernel.Extensions;
using LT.DigitalOffice.Kernel.Validators.Interfaces;
using LT.DigitalOffice.MessageService.Models.Dto.Requests.Workspace;
using LT.DigitalOffice.MessageService.Validation.Validators.Workspace.Interfaces;
using LT.DigitalOffice.Models.Broker.Common;
using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace LT.DigitalOffice.MessageService.Validation.Validators.Workspace
{
  public class CreateWorkspaceRequestValidator : AbstractValidator<CreateWorkspaceRequest>, ICreateWorkspaceRequestValidator
  {
    private IHttpContextAccessor _httpContextAccessor;
    private readonly IRequestClient<ICheckUsersExistence> _rcCheckUsersExistence;
    private readonly ILogger<CreateWorkspaceRequestValidator> _logger;

    public CreateWorkspaceRequestValidator(
      IHttpContextAccessor httpContextAccessor,
      IRequestClient<ICheckUsersExistence> rcCheckUsersExistence,
      ILogger<CreateWorkspaceRequestValidator> logger,
      IImageContentValidator imageContentValidator,
      IImageExtensionValidator imageExtensionValidator)
    {
      _httpContextAccessor = httpContextAccessor;
      _rcCheckUsersExistence = rcCheckUsersExistence;
      _logger = logger;

      RuleFor(workspace => workspace.Name)
        .NotEmpty().WithMessage("Workspace name cannot be empty.");

      When(w => w.Image != null, () =>
      {
        RuleFor(w => w.Image.Content)
          .SetValidator(imageContentValidator);

        RuleFor(w => w.Image.Extension)
          .SetValidator(imageExtensionValidator);
      });

      RuleFor(w => w.Users)
        .NotNull().WithMessage("Users cannot be null");

      When(w => w.Users != null && w.Users.Count > 0, () =>
      {
        RuleFor(w => w.Users)
          .Must(u => u.Select(i => i.UserId).ToHashSet().Count() == u.Count())
          .WithMessage("A user cannot be added to the workspace twice.")
          .Must(u => u.FirstOrDefault(i =>
            i.UserId == _httpContextAccessor.HttpContext.GetUserId()) == null)
          .WithMessage("Creator cannot be added to workspace users request.")
          .MustAsync(async (u, _) => await CheckUserExistence(u.Select(i => i.UserId).ToList()))
          .WithMessage("Some users are not available for adding to the workspace.");
      });
    }

    private async Task<bool> CheckUserExistence(List<Guid> usersIds)
    {
      if (!usersIds.Any())
      {
        return false;
      }

      try
      {
        Response<IOperationResult<ICheckUsersExistence>> response =
          await _rcCheckUsersExistence.GetResponse<IOperationResult<ICheckUsersExistence>>(
            ICheckUsersExistence.CreateObj(usersIds));

        if (response.Message.IsSuccess)
        {
          return usersIds.Count == response.Message.Body.UserIds.Count;
        }

        _logger.LogWarning(
          "Error while checkingexisting users withs this ids: {UsersIds}.\nErrors: {Errors}",
          string.Join(", ", usersIds),
          string.Join('\n', response.Message.Errors));
      }
      catch (Exception exc)
      {
        _logger.LogError(
          exc,
          "Cannot check existing users withs this ids {UsersIds}",
          string.Join(", ", usersIds));
      }

      return false;
    }
  }
}
