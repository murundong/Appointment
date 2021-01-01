using Aliyun.OSS;
using BaseClasses;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Appoint.Application.Services
{
    public class OssService : IOssService
    {
        string endpoint = ConfigurationHelper.GetAppSetting<string>("Endpoint");
        string AccessKeyID = ConfigurationHelper.GetAppSetting<string>("AccessKeyID");
        string AccessKeySecret = ConfigurationHelper.GetAppSetting<string>("AccessKeySecret");
        string BucketName = ConfigurationHelper.GetAppSetting<string>("BucketName");
        public void UploadFile(string name, Stream inputStream)
        {
            try
            {
              
                OssClient client = new OssClient(endpoint, AccessKeyID, AccessKeySecret);
                var res = client.PutObject(BucketName, name, inputStream);
            }
            catch (Exception ex)
            {
            }
        }
    }
}
