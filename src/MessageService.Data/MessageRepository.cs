﻿using LT.DigitalOffice.MessageService.Data.Interfaces;
using LT.DigitalOffice.MessageService.Data.Provider;
using LT.DigitalOffice.MessageService.Models.Db;

namespace LT.DigitalOffice.MessageService.Data
{
    public class MessageRepository : IMessageRepository
    {
        private readonly IDataProvider provider;

        public MessageRepository(IDataProvider provider)
        {
            this.provider = provider;
        }

        public void SaveEmail(DbEmail email)
        {
            provider.Emails.Add(email);
            provider.Save();
        }
    }
}