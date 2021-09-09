using System;
using System.Collections.Generic;
using System.Linq;
using FluentValidation;
using LT.DigitalOffice.Kernel.Broker;
using LT.DigitalOffice.Kernel.Extensions;
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

    private List<string> AllowedExtensions = new()
    { ".jpg", ".jpeg", ".png", ".gif", ".bmp", ".tga" };

    public CreateWorkspaceRequestValidator(
      IHttpContextAccessor httpContextAccessor,
      IRequestClient<ICheckUsersExistence> rcCheckUsersExistence,
      ILogger<CreateWorkspaceRequestValidator> logger)
    {
      _httpContextAccessor = httpContextAccessor;
      _rcCheckUsersExistence = rcCheckUsersExistence;
      _logger = logger;

      RuleFor(workspace => workspace.Name)
        .NotEmpty().WithMessage("Workspace name cannot be empty.");

      When(w => w.Image != null, () =>
      {
        RuleFor(w => w.Image.Content)
          .NotEmpty().WithMessage("Image content cannot be empty.")
          .Must(x =>
          {
            try
            {
              var byteString = new Span<byte>(new byte[x.Length]);
              return Convert.TryFromBase64String(x, byteString, out _);
            }
            catch
            {
              return false;
            }
          }).WithMessage("Wrong image content.");

        RuleFor(w => w.Image.Extension)
          .Must(AllowedExtensions.Contains)
          .WithMessage($"Image extension is not {string.Join('/', AllowedExtensions)}");
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
          .Must(u => CheckUserExistence(u.Select(i => i.UserId).ToList()))
          .WithMessage("Some users are not available for adding to the workspace.");
      });
    }

    private bool CheckUserExistence(List<Guid> usersIds)
    {
      if (!usersIds.Any())
      {
        return false;
      }

      try
      {
        var response = _rcCheckUsersExistence.GetResponse<IOperationResult<ICheckUsersExistence>>(
          ICheckUsersExistence.CreateObj(usersIds)).Result;
        if (response.Message.IsSuccess)
        {
          return usersIds.Count == response.Message.Body.UserIds.Count;
        }

        _logger.LogWarning("Can not find user Ids: {userIds}: " +
          $"{Environment.NewLine}{string.Join('\n', response.Message.Errors)}");
      }
      catch (Exception exc)
      {
        _logger.LogError(exc, "Cannot check existing users withs this ids {userIds}");
      }

      return false;
    }
  }
}
