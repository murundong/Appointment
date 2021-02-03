using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Appoint.EntityFramework.Enum
{
    public enum Enum_AppointStatus
    {
        /// <summary>
        /// 不显示
        /// </summary>
        SHOW_NULL=0,
        /// <summary>
        /// 显示预约
        /// </summary>
        SHOW_APPOINT,

        /// <summary>
        /// 显示取消
        /// </summary>
        SHOW_CANCEL,
    }
}
