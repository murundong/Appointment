﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Appoint.EntityFramework.ViewData
{
    public class Base_PageOutput<T>
    {
        public int total { get; set; }
        public T data { get; set; }
    }
}
