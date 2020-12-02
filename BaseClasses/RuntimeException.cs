using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace BaseClasses
{
    [Serializable]
    public class RuntimeException : ApplicationException, ISerializable
    {
        public RuntimeException() : this(int.MinValue, string.Empty)
        { }

        public RuntimeException(string msg) : this(int.MinValue, msg)
        { }

        public RuntimeException(int errorCode, string msg) : this(msg, null)
        {
            this.ErrorCode = errorCode;
        }

        public RuntimeException(string msg, Exception innerException) : this(int.MinValue, msg, innerException)
        { }

        public RuntimeException(int errorCode, string msg, Exception innerException) :
            base(msg, innerException)
        {
            this.ErrorCode = errorCode;
        }

        private const string DEFAULT_ERROR_MESSAGE = "Unknown error : {0}";

        public int ErrorCode { set; get; }

        public string HexErrorCode
        {
            get
            {
                return ToHex(ErrorCode);
            }
        }

        public string ToErrorString()
        {
            if (this.ErrorCode == int.MinValue)
            {
                return base.Message;
            }
            else
            {
                return string.Format("{0} {1}", HexErrorCode, base.Message);
            }
        }



        #region Serializable
        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }
        #endregion


        public static int FromHex(string hexErrorCode)
        {
            int errorCode = int.MinValue;
            try
            {
                errorCode = Convert.ToInt32(hexErrorCode, 16);
            }
            catch
            {
                throw;
            }
            return errorCode;
        }
        public static string ToHex(int errorCode)
        {
            return string.Format("0x" + errorCode.ToString("x8"));
        }
    }
}
