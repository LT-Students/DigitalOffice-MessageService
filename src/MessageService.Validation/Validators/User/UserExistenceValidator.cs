using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using LT.DigitalOffice.Kernel.BrokerSupport.Helpers;
using LT.DigitalOffice.MessageService.Validation.Validators.User.Interfaces;
using LT.DigitalOffice.Models.Broker.Common;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace LT.DigitalOffice.MessageService.Validation.Validators.User
{
  public class UserExistenceValidator : AbstractValidator<List<Guid>>, IUserExistenceValidator
  {
    private readonly IRequestClient<ICheckUsersExistence> _rcCheckUsersExistence;
    private readonly ILogger<UserExistenceValidator> _logger;

    private async Task<bool> CheckUserExistence(List<Guid> usersIds)
    {
      if (!usersIds.Any())
      {
        return true;
      }

      ICheckUsersExistence response =
        await RequestHandler.ProcessRequest<ICheckUsersExistence, ICheckUsersExistence>(
          _rcCheckUsersExistence,
          ICheckUsersExistence.CreateObj(usersIds),
          null,
          _logger);

      return usersIds.Count == response?.UserIds.Count;
    }

    public UserExistenceValidator(
      IRequestClient<ICheckUsersExistence> rcUsersExistence,
      ILogger<UserExistenceValidator> logger)
    {
      _rcCheckUsersExistence = rcUsersExistence;
      _logger = logger;

      RuleFor(array => array)
        .MustAsync(async (a, _) => await CheckUserExistence(a.ToHashSet().ToList()))
        .WithMessage("Incorrect list of users.");
    }
  }
}
