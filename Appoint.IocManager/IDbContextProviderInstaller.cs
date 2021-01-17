using Appoint.EntityFramework.DbContextProvider;
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
    public class IDbContextProviderInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                Component.For(typeof(IDbContextProvider<>))
                .ImplementedBy(typeof(DbContextProvider<>))
                );
        }
    }
}
