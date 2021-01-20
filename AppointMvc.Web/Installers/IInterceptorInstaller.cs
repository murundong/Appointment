using Castle.MicroKernel.Registration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Castle.DynamicProxy;

namespace AppointMvc.Web.Installers
{
    public class IInterceptorInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Component.For<IInterceptor>().ImplementedBy<UofIntercepter>()
                .LifestyleTransient());
        }
    }
}