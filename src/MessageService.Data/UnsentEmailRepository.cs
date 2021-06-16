using LT.DigitalOffice.Kernel.Exceptions.Models;
using LT.DigitalOffice.MessageService.Data.Interfaces;
using LT.DigitalOffice.MessageService.Data.Provider;
using LT.DigitalOffice.MessageService.Models.Db;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LT.DigitalOffice.MessageService.Data
{
    public class UnsentEmailRepository : IUnsentEmailRepository
    {
        private readonly IDataProvider _provider;

        public UnsentEmailRepository(
            IDataProvider provider)
        {
            _provider = provider;
        }

        public DbUnsentEmail Get(Guid id)
        {
            return _provider.UnsentEmails.FirstOrDefault(eu => eu.Id == id)
                ?? throw new NotFoundException($"There is not unsent email with id {id}");
        }

        public IEnumerable<DbUnsentEmail> GetAll()
        {
            return _provider.UnsentEmails;
        }

        public bool Remove(Guid id)
        {
            DbUnsentEmail email = _provider.UnsentEmails.FirstOrDefault(eu => eu.Id == id)
                ?? throw new NotFoundException($"There is not unsent email with id {id}");

            _provider.UnsentEmails.Remove(email);
            _provider.Save();

            return true;
        }
    }
}
