﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using LT.DigitalOffice.Kernel.Broker;
using LT.DigitalOffice.MessageService.Validation.Validators.User.Interfaces;
using LT.DigitalOffice.Models.Broker.Common;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace LT.DigitalOffice.MessageService.Validation.Validators.User
{
  public class CreateChannelUserValidator : AbstractValidator<List<Guid>>, ICreateChannelUserValidator
  {
    private readonly IRequestClient<ICheckUsersExistence> _rcCheckUsersExistence;
    private readonly ILogger<CreateChannelUserValidator> _logger;

    public CreateChannelUserValidator(
      IRequestClient<ICheckUsersExistence> rcCheckUsersExistence,
      ILogger<CreateChannelUserValidator> logger)
    {
      _rcCheckUsersExistence = rcCheckUsersExistence;
      _logger = logger;

      RuleForEach(users => users)
        .Cascade(CascadeMode.Stop)
        .NotEmpty().WithMessage("Users ids must not be empty.")
        .ChildRules(users =>
          RuleFor(users => users)
            .Must(ids => ids.Distinct().Count() == ids.Count())
            .WithMessage("User cannot be added to the channel twice.")
            .MustAsync(async (ids, _) => await CheckUsersExistenceAsync(ids))
            .WithMessage("Some users does not exist."));
    }

    private async Task<bool> CheckUsersExistenceAsync(List<Guid> usersIds)
    {
      try
      {
        Response<IOperationResult<ICheckUsersExistence>> response =
          await _rcCheckUsersExistence.GetResponse<IOperationResult<ICheckUsersExistence>>(
            ICheckUsersExistence.CreateObj(usersIds));

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
