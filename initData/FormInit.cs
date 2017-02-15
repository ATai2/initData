using initData.model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;
using Microsoft.Win32.TaskScheduler;


namespace initData
{
    /// <summary>
    /// atai
    /// </summary>
    public partial class FormInit : Form
    {

        ConfigModel cm;
        public FormInit()
        {
            InitializeComponent();
            cm = new ConfigModel();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
        
        private void btnOk_Click(object sender, EventArgs e)
        {

            //server = 192.168.1.198; database = feng; uid = sa; pwd = 63305330
            StringBuilder local = new StringBuilder();
            local.Append("server = ").Append(tbLocalIPaddress.Text.Trim()).Append("; database = ").Append(tbLocalDatabase.Text.Trim()).Append("; uid = ").Append(tbLocalUsername.Text.Trim()).Append("; pwd =").Append(tbLocalPwd.Text.Trim());
            StringBuilder remote = new StringBuilder();
            remote.Append("server = ").Append(tbRemoteIPaddress.Text.Trim()).Append("; database = ").Append(tbRemoteDatabase.Text.Trim()).Append("; uid = ").Append(tbRemoteUsername.Text.Trim()).Append("; pwd =").Append(tbRemotePwd.Text.Trim());

            cm.Local = local.ToString();
            cm.Remote = remote.ToString();
            cm.Hisid = tbHisID.Text.Trim();
            //save配置文件
            cm.save(tbConfig.Text.Trim());
        }


        ///// <summary>
        ///// 选择日志存放目录
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //private void button1_Click(object sender, EventArgs e)
        //{
        //    var logFile = new FolderBrowserDialog();
        //    logFile.Description = "请选择日志文件存放目录";
        //    if (logFile.ShowDialog() == DialogResult.OK)
        //    {
        //        tbLog.Text = logFile.SelectedPath;
        //    }
        //}


        /// <summary>
        /// 选择tiff图片存放目录
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPic_Click(object sender, EventArgs e)
        {
            var picFile = new FolderBrowserDialog();
            picFile.Description = "请选择日志文件存放目录";
            if (picFile.ShowDialog() == DialogResult.OK)
            {
                tbPic.Text = picFile.SelectedPath;
            }
        }

        private void btnCancle_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        /// <summary>
        /// 选择配置文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnConfigPath_Click(object sender, EventArgs e)
        {
            var logFile = new OpenFileDialog();

            if (logFile.ShowDialog() == DialogResult.OK)
            {
                tbConfig.Text = logFile.FileName;
            }

            //配置文件设置

            //var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            //if (!config.HasFile)
            //{
            //    throw new ArgumentException("程序配置文件缺失！");
            //}
            //string key = "path";
          string value = logFile.FileName;
            //KeyValueConfigurationElement _key = config.AppSettings.Settings[key];
            //if (_key == null)
            //    config.AppSettings.Settings.Add(key, value);
            //else
            //    config.AppSettings.Settings[key].Value = value;
            //config.Save(ConfigurationSaveMode.Modified);


            //读取配置文件
            cm.init(value);

            tbLocalDatabase.Text = cm.Localdatabase.Trim();
            tbLocalIPaddress.Text = cm.Localip.Trim();
            tbLocalUsername.Text = cm.Localuser.Trim();
            tbLocalPwd.Text = cm.Localpwd.Trim();

            tbRemoteDatabase.Text = cm.Remotedatabase.Trim();
            tbRemoteIPaddress.Text = cm.Remoteip.Trim();
            tbRemotePwd.Text = cm.Remotepwd.Trim();
            tbRemoteUsername.Text = cm.Remoteuser.Trim();

            tbPic.Text = cm.Pic;
            tbHisID.Text = cm.Hisid.Trim();
        }

        private void btnConTest_Click(object sender, EventArgs e)
        {
            lbConn.Text = "连接正在测试中，请勿进行其他操作！";
            bool local = false, remote = false;
            SqlConnection remoteconn = new SqlConnection(cm.Remote);
            SqlConnection localconn = new SqlConnection(cm.Local);

            try
            {
                localconn.Open();
                local = true;
            }
            catch (Exception)
            {

            }
            finally
            {
                localconn.Close();
            }

            try
            {
                remoteconn.Open();
                remote = true;
            }
            catch (Exception)
            {

            }
            finally
            {
                remoteconn.Close();
            }

            lbConn.Text = "本地数据库连接" + (local ? "成功" : "失败") +
                "；远程数据库连接" + (remote ? "成功" : "失败");

        }

        private void btnTask_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(tbExePath.Text.Trim()))
            {
                MessageBox.Show("请选择执行文件");
                return;
            }
            var exePath = tbExePath.Text.Trim();
            //            TaskService.Instance.AddTask("test", QuickTriggerType.Daily, "initData.exe");
            using (TaskService ts = new TaskService())
            {
                DateTime dt = new DateTime(DateTime.Now.Year, DateTime.Now.Month
                    , DateTime.Now.Day, 0, 0, 0);
                var list = ts.AllTasks;
                string taskName = lbTaskName.Text.Trim();
                var taskNameList = new List<string>();
                foreach (var task in list)
                {
                    taskNameList.Add(task.Name);
                }

                if (taskNameList.Contains(taskName))
                {
                    ts.RootFolder.DeleteTask(taskName);
                }

                var addTask = ts.AddTask(taskName, new TimeTrigger()
                {
                    StartBoundary = dt,
                    EndBoundary = dt+TimeSpan.FromDays(1000),
                    
                    Enabled = true
                }, new ExecAction(exePath));
                var definition = addTask.Definition;
                definition.RegistrationInfo.Description = "发票备份，每天00：00：00开始执行";
                definition.Settings.DisallowStartIfOnBatteries = false;
                definition.Settings.Enabled = true;
                definition.Settings.AllowDemandStart = true;
                definition.Settings.Priority = ProcessPriorityClass.High;
                definition.Settings.StopIfGoingOnBatteries = false;
               
                TaskEditDialog edit = new TaskEditDialog();
                edit.Editable = true;
                edit.RegisterTaskOnAccept = true;
                edit.Initialize(addTask);
                edit.ShowDialog();
                //                ts.RootFolder.DeleteTask("ksd");
//                int i = 0;
            }
        }

        private void btnExe_Click(object sender, EventArgs e)
        {
            var taskExe=new OpenFileDialog();

            if (taskExe.ShowDialog()==DialogResult.OK)
            {
                tbExePath.Text = taskExe.FileName;
            }


        }
    }
}
