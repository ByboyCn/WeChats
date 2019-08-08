using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace motuiDotnetSdkDemo.exception
{
    public class NoResponseDataException : ApplicationException
    {
        public NoResponseDataException(string msg ):base(msg)
        {
                
        }
        public override string Message => base.Message;
    }
}
