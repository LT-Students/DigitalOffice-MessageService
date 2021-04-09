using LT.DigitalOffice.MessageService.Broker.Consumers;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LT.DigitalOffice.MessageService.Broker.UnitTests
{
    class SendEmailConsumerTests
    {
        private SendEmailConsumer consumer;

        [SetUp]
        public void SetUp()
        {
            Dictionary<string, string> dict = new();
            dict.Add("FirstName", "111");
            dict.Add("Link", "222");
            dict.Add("Password", "333");

            StringBuilder sb = new();
            sb.AppendLine("Hello, {FirstName}!!!");
            sb.AppendLine();
            sb.AppendLine("You receive this message because you was invited to join Digital Office community.");
            sb.AppendLine("If you sure that it is not for you just ignore this message.");
            sb.AppendLine("In other case please follow this link: {Password}");
            sb.AppendLine("Your password: {Password}");
            sb.AppendLine();
            sb.AppendLine("Best Regards,");
            sb.AppendLine("Digital Office team.");

            
        }

        [Test]
        public void Testd()
        {

        }
    }
}
