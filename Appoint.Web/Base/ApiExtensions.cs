using Appoint.Web.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace Appoint.Web.Base
{
    public static class ApiExtensions
    {
        public async static Task<List<FileModel>> ReadFileCollectionAsync(this ApiController apiController)
        {
            var provider = new MultipartMemoryStreamProvider();
            //await TaskEx.Run(async () => await Request.Content.ReadAsMultipartAsync(provider))
            await Task.Run(async ()=> await apiController.Request.Content.ReadAsMultipartAsync(provider));
            var items = provider.Contents.Where(s => s.Headers.ContentDisposition.FileName != null);
            List<FileModel> files = new List<FileModel>();
            if(items!=null && items.Count() > 0)
            {
                foreach (var item in items)
                {
                    var disposition = item.Headers.ContentDisposition;
                    FileModel file = new FileModel(disposition.FileName.Trim('"'),item.Headers.ContentLength??0,await item.ReadAsStreamAsync());
                    files.Add(file);
                }
            }
            return files;
        }

        public static List<FileModel> ReadFileCollection(this ApiController apiController)
        {
            using (var t = ReadFileCollectionAsync(apiController))
            {
                t.Wait();
                return t.Result;
            }
        }

        public static void SaveFile(this Stream stream,string name)
        {
            using (var fileStream = new FileStream(name, FileMode.Create))
            {
                byte[] buffer = new byte[4096];
                while (true)
                {
                    int num = stream.Read(buffer, 0, buffer.Length);
                    if (num == 0)
                    {
                        break;
                    }
                    fileStream.Write(buffer, 0, num);
                }
                fileStream.Flush();
            }
        }
    }
}