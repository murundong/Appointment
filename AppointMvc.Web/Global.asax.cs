using Appoint.EntityFramework;
using Appoint.IocManager;
using AppointMvc.Web.Installers;
using AutoMapper;
using Castle.Windsor;
using Castle.Windsor.Installer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace AppointMvc.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        private static IWindsorContainer container;
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);



            Mapper.Initialize(cfg =>
            {
                cfg.AddProfile(new MappingConfigProfile());
            });
            BootstrapContainer();
        }

        private static void BootstrapContainer()
        {
            container = new WindsorContainer()
                .Install(FromAssembly.This());
            container.Kernel.ComponentModelBuilder.AddContributor(new InterceptorContributor());
            container.Install(new ControllersInstaller());
            container.Install(new IDbContextProviderInstaller());
            container.Install(new IRepositoryInstaller());
            container.Install(new IApplicationServiceInstaller());
            container.Install(new IUofInstaller());

            var controllerFactory = new WindsorControllerFactory(container.Kernel);
            ControllerBuilder.Current.SetControllerFactory(controllerFactory);

        }

        protected void Application_End()
        {
            container.Dispose();
        }
    }
}
