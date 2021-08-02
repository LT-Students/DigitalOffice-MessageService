using LT.DigitalOffice.Kernel.Exceptions.Models;
using LT.DigitalOffice.MessageService.Data.Interfaces;
using LT.DigitalOffice.MessageService.Data.Provider;
using LT.DigitalOffice.MessageService.Models.Db;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LT.DigitalOffice.MessageService.Data
{
    public class KeywordRepository : IKeywordRepository
    {
        private readonly IDataProvider _provider;

        public KeywordRepository(IDataProvider provider)
        {
            _provider = provider;
        }

        public Guid Add(DbKeyword entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            _provider.ParseEntities.Add(entity);
            _provider.Save();

            return entity.Id;
        }

        public bool DoesKeywordExist(string keyword)
        {
            return _provider.ParseEntities.Any(pe => pe.Keyword == keyword);
        }

        public List<DbKeyword> Find(int skipCount, int takeCount, out int totalCount)
        {
            if (skipCount < 0)
            {
                throw new BadRequestException("Skip count can't be less than 0.");
            }

            if (takeCount <= 0)
            {
                throw new BadRequestException("Take count can't be equal or less than 0.");
            }

            totalCount = _provider.ParseEntities.Count();

            return _provider.ParseEntities.Skip(skipCount).Take(takeCount).ToList();
        }

        public DbKeyword Get(Guid entityId)
        {
            return _provider.ParseEntities.FirstOrDefault(pe => pe.Id == entityId)
                ?? throw new NotFoundException($"No parse entity with id: '{entityId}'.");
        }

        public bool Remove(Guid entityId)
        {
            DbKeyword entity = _provider.ParseEntities.FirstOrDefault(pe => pe.Id == entityId)
                ?? throw new NotFoundException($"No parse entity with id: '{entityId}'.");

            _provider.ParseEntities.Remove(entity);
            _provider.Save();

            return true;
        }
    }
}
