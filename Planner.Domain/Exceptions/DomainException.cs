using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Planner.Domain.Exceptions
{
    public class DomainException:Exception
    {
        public DomainException():base("Uma regra de domínio foi violado")
        {
            
        }


        public DomainException(string message) : base(message)
        {

        }
        public DomainException(string message, Exception innerException):base(message, innerException)
        {
            
        }


    }
}
