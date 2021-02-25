using Appoint.IocManager;
using AppointWinService.Installer;
using Castle.Windsor;
using Castle.Windsor.Installer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace AppointWinService
{
    static class Program
    {

      
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        static void Main()
        {
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                new AppointService()
            };
            ServiceBase.Run(ServicesToRun);
        }

       


    }
}
