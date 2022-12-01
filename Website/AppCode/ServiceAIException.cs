using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Website.AppCode
{
    public class ServiceAIException : Exception
    {
        // Chỉ là để đánh dấu
        public ServiceAIException(string mess) : base(mess) { }
    }
}