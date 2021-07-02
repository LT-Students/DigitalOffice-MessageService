﻿using LT.DigitalOffice.Kernel.Exceptions.Models;
using LT.DigitalOffice.MessageService.Data.Interfaces;
using LT.DigitalOffice.MessageService.Data.Provider;
using LT.DigitalOffice.MessageService.Models.Db;
using System;
using System.Linq;

namespace LT.DigitalOffice.MessageService.Data
{
    public class SMTPCredentialsRepository : ISMTPCredentialsRepository
    {
        private readonly IDataProvider _provider;

        public SMTPCredentialsRepository(IDataProvider dataProvider)
        {
            _provider = dataProvider;
        }

        public void Create(DbSMTPCredentials dbSMTP)
        {
            if (dbSMTP == null)
            {
                throw new ArgumentNullException(nameof(dbSMTP));
            }

            if (_provider.SMTP.Any())
            {
                throw new BadRequestException("SMTP already exist");
            }

            _provider.SMTP.Add(dbSMTP);
            _provider.Save();
        }

        public DbSMTPCredentials Get()
        {
            if (!_provider.SMTP.Any())
            {
                throw new NotFoundException("SMTP data not found");
            }

            return _provider.SMTP.First();
        }
    }
}
