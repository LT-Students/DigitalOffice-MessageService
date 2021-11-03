using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.Kernel.Responses;

namespace LT.DigitalOffice.MessageService.Business.Commands.User.Interfaces
{
  [AutoInject]
  public interface IRemoveWorkspaceUserCommand
  {
    Task<OperationResultResponse<bool>> ExecuteAsync(Guid workspaceId, List<Guid> usersIds);
  }
}
