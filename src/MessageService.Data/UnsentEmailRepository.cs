using LT.DigitalOffice.Kernel.Exceptions.Models;
using LT.DigitalOffice.MessageService.Data.Interfaces;
using LT.DigitalOffice.MessageService.Data.Provider;
using LT.DigitalOffice.MessageService.Models.Db;
using Microsoft.EntityFrameworkCore;
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

        public void Add(DbUnsentEmail email)
        {
            if (email == null)
            {
                throw new ArgumentNullException(nameof(email));
            }

            _provider.UnsentEmails.Add(email);
            _provider.Save();
        }

        public DbUnsentEmail Get(Guid id)
        {
            return _provider.UnsentEmails.Include(x => x.Email).FirstOrDefault(eu => eu.Id == id)
                ?? throw new NotFoundException($"There is not unsent email with id {id}");
        }

        public IEnumerable<DbUnsentEmail> GetAll(int totalSendingCountIsLessThen)
        {
            return _provider
                .UnsentEmails
                .Where(u => u.TotalSendingCount < totalSendingCountIsLessThen)
                .Include(u => u.Email)
                .ToList();
        }

        public IEnumerable<DbUnsentEmail> Find(int skipCount, int takeCount, out int totalCount)
        {
            if (skipCount < 0)
            {
                throw new BadRequestException("Skip count can't be less than 0.");
            }

            if (takeCount <= 0)
            {
                throw new BadRequestException("Take count can't be equal or less than 0.");
            }

            totalCount = _provider.UnsentEmails.Count();

            return _provider.UnsentEmails.Include(u => u.Email).Skip(skipCount).Take(takeCount).ToList();
        }

        public bool Remove(DbUnsentEmail email)
        {
            if (email == null)
            {
                throw new ArgumentNullException(nameof(email));
            }

            _provider.UnsentEmails.Remove(email);
            _provider.Save();

            return true;
        }

        public void IncrementTotalCount(DbUnsentEmail email)
        {
            if (email == null)
            {
                throw new ArgumentNullException(nameof(email));
            }

            email.TotalSendingCount++;
            email.LastSendAt = DateTime.UtcNow;
            _provider.Save();
        }
    }
}
