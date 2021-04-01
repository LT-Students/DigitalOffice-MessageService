using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LT.DigitalOffice.Broker.Requests
{
    public interface ICreateImageRequest
    {
        public string Image { get; }

        static object CreateObj(string image)
        {
            return new
            {
                Image = image
            };
        }
    }
}
