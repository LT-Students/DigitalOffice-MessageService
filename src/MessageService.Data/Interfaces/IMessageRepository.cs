using System;
using System.Threading.Tasks;
using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.MessageService.Models.Db;

namespace LT.DigitalOffice.MessageService.Data.Interfaces
{
  [AutoInject]
  public interface IMessageRepository
  {
    Task<Guid?> CreateAsync(DbMessage request);
  }
}
