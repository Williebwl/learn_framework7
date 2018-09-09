using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIStudio.Framework
{
    public class DefinedException : Exception
    {
        public DefinedException()
        {

        }
        public DefinedException(string message = null, Exception innerException = null, string stackTrace = null)
            : base(message, innerException)
        {
            this.stackTrace = stackTrace;
        }
        
        private string stackTrace;
        public override string StackTrace
        {
            get
            {
                return this.stackTrace;
            }
        }
        
    }
}
