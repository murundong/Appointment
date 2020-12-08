using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace BaseClasses
{
    public class BaseException : ApplicationException
    {
        private int exceptionCode;
        private string exceptionMessage;
        public BaseException()
        {
        }
        public BaseException(string message, System.Exception innerException)
            : base(message, innerException)
        {

        }
        public BaseException(string message)
            : base(message)
        {
        }
        protected BaseException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
        /// <summary>
        /// 异常编码
        /// </summary>
        public virtual int ExceptionCode
        {
            get { return exceptionCode; }
            set { value = exceptionCode; }
        }

        /// <summary>
        /// 异常详细信息
        /// </summary>
        public virtual string ExceptionMessage
        {
            get { return exceptionMessage; }
            set { value = exceptionMessage; }
        }


        public BaseException(int errorCode, string errorMessage)
            : base(errorMessage)
        {
            this.exceptionCode = errorCode;
            this.exceptionMessage = errorMessage;
        }
    }
}
