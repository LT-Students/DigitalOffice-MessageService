using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.MessageService.Models.Db;

namespace LT.DigitalOffice.MessageService.Data.Interfaces
{
    [AutoInject]
    public interface ISMTPCredentialRepository
    {
        void Create(DbSMTPCredential dbSMTP);

        DbSMTPCredential Get();
    }
}
