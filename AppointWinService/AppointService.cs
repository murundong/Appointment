using Appoint.Application.IWeixinApi;
using Appoint.Application.Services;
using Appoint.IocManager;
using AppointWinService.Installer;
using BaseClasses;
using Castle.Windsor;
using Castle.Windsor.Installer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AppointWinService
{
    public partial class AppointService : ServiceBase
    {
        private static IWindsorContainer container;
        static void BootstrapContainer()
        {
            container = new WindsorContainer()
                .Install(FromAssembly.This());

            container.Kernel.ComponentModelBuilder.AddContributor(new InterceptorContributor());
            container.Install(new IDbContextProviderInstaller());
            container.Install(new IRepositoryInstaller());
            container.Install(new IApplicationServiceInstaller());
            container.Install(new IUofInstaller());
        }


        public IDoorUsersCardsService _doorUserCardService { get; set; }
        public IDoorUsersAppointsService _doorUserAppointService { get; set; }


        static CancellationTokenSource cts = new CancellationTokenSource();
        public AppointService()
        {
            InitializeComponent();
            try
            {
                BootstrapContainer();
                _doorUserCardService = container.Resolve<IDoorUsersCardsService>();
                _doorUserAppointService = container.Resolve<IDoorUsersAppointsService>();
            }
            catch (Exception ex)
            {
                Log.Error($"[构造函数初始化失败]:{ex.Message}");
            }
        }

        protected override void OnStart(string[] args)
        {
            try
            {
                Log.Info("The Serivce starting……");
                AsyncProcess();
            }
            catch (Exception ex)
            {
                Log.Error($"[OnStart]{ ex.Message}");
            }
        }


        protected override void OnStop()
        {
            Log.Info("The Serivce stoping……");
            try
            {

            }
            catch (Exception ex)
            {
                Log.Error($"[OnStop]{ ex.Message}");
            }
        }


        public void AsyncProcess()
        {
            Task.Run(() =>
            {
                while (!cts.IsCancellationRequested)
                {
                    _doorUserCardService.ProcessFreezeCard();
                    _doorUserAppointService.CancselCourse();
                    Thread.Sleep(3000);
                }
            }, cts.Token).ContinueWith(
            task =>
            {
                if (task.IsFaulted)
                {
                    Log.Error($"AsyncProcess[OnstartThreadError]{task.Exception.Message}", task.Exception);
                }
            }
            );
        }

    }
}
