using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseClasses
{
    /// <summary>
    /// 分页数据
    /// </summary>
    [Serializable()]
    public class PagerHelper
    {
        private int pageSize;
        private int dataCount;

        public PagerHelper()
        {
            this.pageSize = 1;
            this.dataCount = 0;
        }

        /// <summary>
        /// 每页数量
        /// </summary>
        public int PageSize
        {
            get { return pageSize; }
            set { pageSize = value; }
        }

        /// <summary>
        /// 总数量
        /// </summary>
        public int DataCount
        {
            get { return dataCount; }
            set { dataCount = value; }
        }

        /// <summary>
        /// 总页数
        /// </summary>
        public int PageCount
        {
            get
            {
                return (DataCount / PageSize) + (DataCount % PageSize == 0 ? 0 : 1);
            }
        }
    }

    /// <summary>
    /// 非泛型
    /// </summary>
    public class ResponseDataHelper
    {
        private int resposeCode = 0;
        private string responseMessage = string.Empty;

        public ResponseDataHelper()
        {
            this.resposeCode = 0;
            this.responseMessage = "操作成功";
        }

        /// <summary>
        /// 操作响应码
        /// </summary>
        public int ResponseCode
        {
            get { return resposeCode; }
            set { resposeCode = value; }
        }

        /// <summary>
        /// 操作响应消息
        /// </summary>
        public string ResponseMessage
        {
            get { return responseMessage; }
            set { responseMessage = value; }
        }
    }


    /// <summary>
    /// 泛型
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Serializable()]
    [System.Xml.Serialization.XmlRoot("ResponseData")]
    public class ResponseDataHelper<T>
    {
        private int resposeCode = 0;
        private string responseMessage = string.Empty;

        public ResponseDataHelper()
        {
            this.resposeCode = 0;
            this.responseMessage = "操作成功";
        }

        /// <summary>
        /// 操作响应码
        /// </summary>
        public int ResponseCode
        {
            get { return resposeCode; }
            set { resposeCode = value; }
        }

        /// <summary>
        /// 操作响应消息
        /// </summary>
        public string ResponseMessage
        {
            get { return responseMessage; }
            set { responseMessage = value; }
        }

        /// <summary>
        /// 分页相关数据
        /// </summary>
        public PagerHelper PagerData
        {
            get;
            set;
        }

        /// <summary>
        /// 分页数据
        /// </summary>
        public T ResponseData
        {
            get;
            set;
        }
    }
}
