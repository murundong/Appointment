using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Appoint.Application.Services
{
    public interface IOssService: IApplicationService
    {
        void UploadFile(string name, Stream inputStream);
    }
}
