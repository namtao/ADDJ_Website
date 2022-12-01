using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ADDJ.Entity
{
    public class GQKNMessageException : Exception
    {
        public GQKNMessageException() : base()
        { }

        public GQKNMessageException(string message)
            : base(message)
        { }

        public GQKNMessageException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
