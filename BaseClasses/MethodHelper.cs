using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BaseClasses
{
    public class MethodHelper
    {
        public static MethodBase GetMethodInfoByStackTraceCount(int stackTraceCount)
        {
            StackTrace stackTrace = new StackTrace();
            if (stackTrace.FrameCount < stackTraceCount) return null;
            var tempStackTrace = stackTrace.GetFrame(stackTraceCount);
            if (tempStackTrace == null)
            {
                return null;
            }
            return tempStackTrace.GetMethod();
        }

       
    }
}
