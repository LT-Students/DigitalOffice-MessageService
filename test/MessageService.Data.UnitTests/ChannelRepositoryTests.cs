using LT.DigitalOffice.MessageService.Data.Interfaces;
using LT.DigitalOffice.MessageService.Data.Provider;
using LT.DigitalOffice.MessageService.Data.Provider.MsSql.Ef;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace LT.DigitalOffice.MessageService.Data.UnitTests
{
  public class ChannelRepositoryTests
    {
        private IChannelRepository _repository;
        private IDataProvider _provider;

        //[OneTimeSetUp]
        //public void OneTimeSetUp()
        //{
        //    CreateMemoryDb();
        //}

        //public void CreateMemoryDb()
        //{
        //    var dbOptions = new DbContextOptionsBuilder<MessageServiceDbContext>()
        //           .UseInMemoryDatabase(databaseName: "InMemoryDatabase")
        //           .Options;
        //    _provider = new MessageServiceDbContext(dbOptions);

        //    _repository = new ChannelRepository(_provider);
        //}

        /*[TearDown]
        public void CleanDb()
        {
            if (_provider.IsInMemory())
            {
                _provider.EnsureDeleted();
            }
        }

        #region Add Tests

        [Test]
        public void ShouldAddSuccesfuly()
        {
            DbChannel channel = new DbChannel();

            _repository.CreateAsync(channel);

            Assert.IsTrue(_provider.Channels.Contains(channel));
        }

        #endregion*/
    }
}
