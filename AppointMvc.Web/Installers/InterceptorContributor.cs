using Castle.MicroKernel.ModelBuilder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Castle.Core;
using Castle.MicroKernel;
using Appoint.Application;

namespace AppointMvc.Web.Installers
{
    public class InterceptorContributor : IContributeComponentModelConstruction
    {
        public void ProcessModel(IKernel kernel, ComponentModel model)
        {
            //if (typeof(IApplicationService).IsAssignableFrom(model.Implementation))
            //{
            //    model.Interceptors.AddIfNotInCollection(InterceptorReference.ForType<UofIntercepter>());
            //}
            
            if (model.Services.Any(s =>typeof(IApplicationService).IsAssignableFrom(s)))
            {
                model.Interceptors.AddIfNotInCollection(InterceptorReference.ForType<UofIntercepter>());
            }


        }
    }
}