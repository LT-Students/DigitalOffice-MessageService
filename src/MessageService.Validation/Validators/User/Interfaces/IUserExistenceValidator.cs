using System;
using System.Collections.Generic;
using FluentValidation;
using LT.DigitalOffice.Kernel.Attributes;

namespace LT.DigitalOffice.MessageService.Validation.Validators.User.Interfaces
{
  [AutoInject]
  public interface IUserExistenceValidator : IValidator<List<Guid>>
  {
  }
}
