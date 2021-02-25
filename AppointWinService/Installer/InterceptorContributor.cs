using Appoint.Application;
using Castle.Core;
using Castle.MicroKernel;
using Castle.MicroKernel.ModelBuilder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppointWinService.Installer
{
    public class InterceptorContributor : IContributeComponentModelConstruction
    {
        public void ProcessModel(IKernel kernel, ComponentModel model)
        {
            if (model.Services.Any(s => typeof(IApplicationService).IsAssignableFrom(s)))
            {
                model.Interceptors.AddIfNotInCollection(InterceptorReference.ForType<UofIntercepter>());
            }
        }
    }
}
