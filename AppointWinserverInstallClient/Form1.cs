using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Data;
using System.Drawing;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace AppointWinserverInstallClient
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        //string servicePath = $"{Application.StartupPath}\\AppointWinService.exe";
        //string serviceName = "AppointWinService";

        /// <summary>
        /// 安装服务
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txt_servname.Text) || string.IsNullOrWhiteSpace(txt_servpath.Text))
                {
                    MessageBox.Show("请先填写服务名称和服务路径");
                    return;
                }
                if (IsServiceExisted(txt_servname.Text))
                {
                    UninstallService(txt_servname.Text, txt_servpath.Text);
                }
                this.InstallService(txt_servpath.Text, txt_servpath.Text);
                MessageBox.Show("安装服务成功！");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txt_servname.Text) || string.IsNullOrWhiteSpace(txt_servpath.Text))
                {
                    MessageBox.Show("请先填写服务名称和服务路径");
                    return;
                }
                if (IsServiceExisted(txt_servname.Text))
                {
                    StartService(txt_servname.Text);
                    MessageBox.Show("服务启动成功！");
                    return;
                }
                MessageBox.Show("服务启动失败，请确认安装了该服务，并且服务代码无异常！");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txt_servname.Text) || string.IsNullOrWhiteSpace(txt_servpath.Text))
                {
                    MessageBox.Show("请先填写服务名称和服务路径");
                    return;
                }
                if (IsServiceExisted(txt_servname.Text))
                {
                    StopService(txt_servname.Text);
                    MessageBox.Show("服务停止成功！");
                    return;
                }
                MessageBox.Show("服务停止失败，请确认安装了该服务，并且服务代码无异常！");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txt_servname.Text) || string.IsNullOrWhiteSpace(txt_servpath.Text))
                {
                    MessageBox.Show("请先填写服务名称和服务路径");
                    return;
                }
                if (IsServiceExisted(txt_servname.Text))
                {
                    StopService(txt_servname.Text);
                    UninstallService(txt_servname.Text, txt_servpath.Text);
                    MessageBox.Show("服务卸载成功！");
                    return;
                }
                MessageBox.Show("服务卸载失败，请确认安装了该服务，并且服务代码无异常！");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        bool IsServiceExisted(string serviceName)
        {
            ServiceController[] services = ServiceController.GetServices();
            return services.Count(s => s.ServiceName.ToLower() == serviceName.ToLower()) > 0;
        }

        void InstallService(string serviceName,string servicePath)
        {
            using (AssemblyInstaller installer = new AssemblyInstaller())
            {
                installer.UseNewContext = true;
                installer.Path = servicePath;
                IDictionary savedState = new Hashtable();
                installer.Install(savedState);
                installer.Commit(savedState);
            }
        }

        void UninstallService(string serviceName, string servicePath)
        {
            using (AssemblyInstaller install = new AssemblyInstaller())
            {
                install.UseNewContext = true;
                install.Path = servicePath;
                install.Uninstall(null);
            }
        }

        void StartService(string serviceName)
        {
            using (ServiceController control = new ServiceController(serviceName))
            {
                if(control.Status == ServiceControllerStatus.Stopped)
                {
                    control.Start();
                }
            }
        }

        void StopService(string serviceName)
        {
            using (ServiceController control = new ServiceController(serviceName))
            {
                if(control.Status == ServiceControllerStatus.Running)
                {
                    control.Stop();
                }
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "exe文件|*.exe|所有文件|*.*";
            ofd.Title = "选择服务的路径";
            if(ofd.ShowDialog() == DialogResult.OK)
            {
                txt_servpath.Text = ofd.FileName;
            }
        }
    }
}
