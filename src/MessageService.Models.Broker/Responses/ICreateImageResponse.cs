using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LT.DigitalOffice.Broker.Responses
{
    public interface ICreateImageResponse
    {
        public Guid Id { get; }

        static object CreateObj(Guid id)
        {
            return new
            {
                Id = id
            };
        }
    }
}
