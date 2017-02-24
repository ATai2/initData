using initData.model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Text;
using System.Timers;
using System.Windows.Forms;
using Microsoft.Win32.TaskScheduler;
using Timer = System.Timers.Timer;


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
            cm.Pic=tbPic.Text.Trim();
            //save配置文件
            cm.save(tbConfig.Text.Trim());
        }


   
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
            logFile.Filter = "XML文件|initData.xml";

            if (logFile.ShowDialog() == DialogResult.OK)
            {
                tbConfig.Text = logFile.FileName;

            }
            else
            {
                return;
            }

         
          string value = logFile.FileName;
       

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
               
                localconn?.Close();
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
                remoteconn?.Close();
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
              
                var addTask = ts.AddTask(taskName, new DailyTrigger()
                {
                    StartBoundary = dt,
                    EndBoundary = dt+TimeSpan.FromDays(10000),
                    
                    Enabled = true
                }, new ExecAction(exePath));
                var definition = addTask.Definition;
                definition.RegistrationInfo.Description = "发票备份，每天00：00：00开始执行";
                definition.Settings.DisallowStartIfOnBatteries = false;
                definition.Settings.Enabled = true;
                definition.Settings.MultipleInstances=TaskInstancesPolicy.StopExisting;
//                definition.Settings.RestartInterval=TimeSpan.FromSeconds(100);
//                definition.Settings.Compatibility
//                definition.Settings.ExecutionTimeLimit = TimeSpan.FromMinutes(2);
//                definition.Settings.IdleSettings.IdleDuration = TimeSpan.FromMinutes(3);
//                definition.Settings.AllowDemandStart = true;
                definition.Settings.Priority = ProcessPriorityClass.High;
                definition.Settings.StopIfGoingOnBatteries = false;
               //definition.Settings.
                TaskEditDialog edit = new TaskEditDialog();
//                edit.
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

        private void btnDo_Click(object sender, EventArgs e)
        {
            if(string.IsNullOrEmpty(tbTime.Text)) return;

            var interval = Convert.ToInt32(tbTime.Text);
               
                        Timer timer=new Timer(interval*1000);
                        timer.Elapsed+=new ElapsedEventHandler(DoWork);
            timer.AutoReset = true;   //设置是执行一次（false）还是一直执行(true)；   
            timer.Enabled = true;     //是否执行System.Timers.Timer.Elapsed事件； 
        }

        private void DoWork(object sender, ElapsedEventArgs e)
        {
            string path =tbExePath.Text ;//这个path就是你要调用的exe程序的绝对路径
            if (string.IsNullOrEmpty(path))
            {
                return;
            }
            System.Diagnostics.Process process = new System.Diagnostics.Process();


            process.StartInfo.FileName = path;
            process.StartInfo.WorkingDirectory = path;
            process.StartInfo.CreateNoWindow = false;
            process.Start();
            if (process.HasExited)
            {
                MessageBox.Show("程序完成退出");
            }
        }
    }
}
