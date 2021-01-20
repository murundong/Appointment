using Appoint.Application;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Appoint.IocManager
{
    public class IApplicationServiceInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                Classes.FromAssembly(typeof(IApplicationService).Assembly)
                 .BasedOn<IApplicationService>()
                 .WithService.DefaultInterfaces().LifestyleTransient()
                 );
        }

    }
}
