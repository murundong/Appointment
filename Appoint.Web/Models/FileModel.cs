using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace Appoint.Web.Models
{
    public class FileModel
    {
        public string fileName { get; set; }
        public long size { get; set; }
        public Stream stream { get; set; }
        public FileModel(string _fileName, long _size,Stream _stream)
        {
            fileName = _fileName;
            size = _size;
            stream = _stream;
        }

        public virtual void SaveAs(string filename)
        {
            using (var fileStream = new FileStream(filename, FileMode.Create))
            {
                byte[] buffer = new byte[4096];
                while (true)
                {
                    int num = this.stream.Read(buffer, 0, buffer.Length);
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