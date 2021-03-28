using Appoint.Application.IWeixinApi;
using Appoint.Application.Services;
using Appoint.EntityFramework;
using Appoint.EntityFramework.Data;
using Appoint.EntityFramework.WeixData;
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
        public IDoorUsersSubsMsgService _doorUserSubsMsgService { get; set; }
        public IDoorUsersQueueAppointsService _doorUserQueenService { get; set; }
        public ICoursesService _courseService { get; set; }
        public IWeixinService _wxService { get; set; }
        public IWX_TOKEN_Service _tokenService { get; set; }


        static CancellationTokenSource cts = new CancellationTokenSource();
        public AppointService()
        {
            InitializeComponent();
            try
            {
                BootstrapContainer();
                _doorUserCardService = container.Resolve<IDoorUsersCardsService>();
                _doorUserAppointService = container.Resolve<IDoorUsersAppointsService>();
                _doorUserSubsMsgService = container.Resolve<IDoorUsersSubsMsgService>();
                _doorUserQueenService = container.Resolve<IDoorUsersQueueAppointsService>();
                _courseService = container.Resolve<ICoursesService>();
                _wxService = container.Resolve<IWeixinService>();
                _tokenService = container.Resolve<IWX_TOKEN_Service>();
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
                    Dictionary<int, List<string>> lst_usercourses = new Dictionary<int, List<string>>();
                    List<int> lst_appointnotice = new List<int>();
                    _doorUserCardService.ProcessFreezeCard();
                    _doorUserAppointService.CancselCourse(out lst_usercourses);
                 
                    //课程取消
                    if (lst_usercourses?.Count > 0)
                    {
                        string token = GetNowToken();
                        var lst_course= _courseService.GetAllCourse(string.Join(",", lst_usercourses.Keys));
                        foreach (KeyValuePair<int,List<string>> item in lst_usercourses)
                        {
                            var course_item = lst_course.FirstOrDefault(s => s.id == item.Key);
                            item.Value?.ForEach(_ =>
                            {
                                W_SUBS_DATA_INPUT data = new W_SUBS_DATA_INPUT()
                                {
                                    touser = _,
                                    access_token = token,
                                    page = $"pages/Lesson/Lesson?doorId={course_item.door_id}&doorName={course_item.door_name}",
                                    template_id = ConstConfig.template_cancel,
                                    data = new { thing6 = new { value = course_item.subject_title }, date2 = new { value = course_item.course_date + " " + course_item.course_time }, thing4 = new { value = "人数不足自动取消" } } 
                                };
                                var sendres = _wxService.SendSubsCribe(data);
                                if (sendres.errCode != 0 && sendres.errCode != 43101)
                                {
                                    Log.Error($"SendSubsCribe{sendres.errCode}_{sendres.errMsg}");
                                }
                            });
                        }
                    }
                    //预约通知
                    var lst_subs = _courseService.GetAllCourse();
                    if (lst_subs?.Count > 0)
                    {
                        string token = GetNowToken();
                        lst_subs?.ForEach(s => {
                            W_SUBS_DATA_INPUT data = new W_SUBS_DATA_INPUT()
                            {
                                touser = s.open_id,
                                page = $"pages/appointment/appointment",
                                access_token = token,
                                template_id = ConstConfig.template_notice,
                                data = new { thing1 = new { value = s.subject_title }, thing3 = new { value = s.door_name }, character_string2 = new { value = s.course_date + " "+s.course_time }, thing4=new { value=$"课程开始前{s.cancel_duration}分钟内不可取消"} }
                            };
                            var sendres = _wxService.SendSubsCribe(data);
                            if (sendres.errCode != 0 && sendres.errCode != 43101)
                            {
                                Log.Error($"SendSubsCribe{sendres.errCode}_{sendres.errMsg}");
                            }
                            else lst_appointnotice.Add(s.appoint_id);
                        });
                    }
                    //已预约的打标记
                    if (lst_appointnotice?.Count >0)
                    {
                        _doorUserAppointService.UpdateNoticeAppoint(string.Join(",", lst_appointnotice));
                    }
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

        string GetNowToken()
        {
            string token = null, appid = ConstConfig.APPID;
            var TOKENITEM = _tokenService.GetToken(appid);
            if (TOKENITEM == null || string.IsNullOrWhiteSpace(TOKENITEM?.access_token) || TOKENITEM.create_time.AddSeconds(TOKENITEM.expires_in) <= DateTime.Now.AddMinutes(-10))
            {
                var WXTOKEN = _wxService.GetToken();
                if (WXTOKEN != null && WXTOKEN.errcode == 0)
                {
                    token = WXTOKEN.access_token;
                    _tokenService.InsertOrUpdateToken(new WX_TOKEN() { appid = appid, access_token = WXTOKEN.access_token, expires_in = WXTOKEN.expires_in });
                }
            }
            else token = TOKENITEM.access_token;
            return token;
        }

    }
}
