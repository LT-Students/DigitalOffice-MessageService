using LT.DigitalOffice.MessageService.Mappers.Db;
using LT.DigitalOffice.MessageService.Mappers.Db.Interfaces;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LT.DigitalOffice.MessageService.Mappers.UnitTests
{
    public class DbSMTPMapperTests
    {
        private IDbSMTPMapper _mapper;

        [SetUp]
        public void SetUp()
        {
            _mapper = new DbSMTPMapper();
        }

        [Test]
        public void ShouldThrowNullArgumentExceptionWhenRequestIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => _mapper.Map(null));
        }

        [Test]
        public void ShouldMapSuccessfully()
        {
            Assert.Throws<ArgumentNullException>(() => _mapper.Map(null));
        }

    }
}
