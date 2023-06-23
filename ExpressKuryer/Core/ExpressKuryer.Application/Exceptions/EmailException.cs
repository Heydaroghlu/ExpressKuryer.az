using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpressKuryer.Application.Exceptions
{
    public class EmailException : Exception
    {
        public EmailException():base("Mail doğru deyil")
        {
            
        }
    }
}
