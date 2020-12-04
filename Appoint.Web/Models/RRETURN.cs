using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Appoint.Web.Models
{
    public class RRETURN<T>
    {
        public int errCode { get; set; }
        public string msg { get; set; }
        public T data { get; set; }
    }
}