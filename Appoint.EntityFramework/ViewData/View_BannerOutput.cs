﻿using Appoint.EntityFramework.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Appoint.EntityFramework.ViewData
{
    public class View_BannerOutput
    {
        public string img { get; set; }
        public Enum_ImgType img_type { get; set; }
        public string url { get; set; }

    }
}
