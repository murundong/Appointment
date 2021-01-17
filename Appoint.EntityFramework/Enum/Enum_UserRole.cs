﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Appoint.EntityFramework.Enum
{
    public enum Enum_UserRole
    {
        /// <summary>
        /// 黑名单
        /// </summary>
        Black=-1,
        /// <summary>
        /// 普通会员
        /// </summary>
        Member,
        /// <summary>
        /// 馆主
        /// </summary>
        Curator,
        /// <summary>
        /// 教职工
        /// </summary>
        Teacher,
        /// <summary>
        /// 管理员
        /// </summary>
        Admin,
       
    }
}
