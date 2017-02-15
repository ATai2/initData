using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Win32.TaskScheduler;

namespace TaskDemo
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //            TaskService.Instance.AddTask("test", QuickTriggerType.Daily, "initData.exe");
            using (TaskService ts = new TaskService())
            {
                DateTime dt = new DateTime(DateTime.Now.Year, DateTime.Now.Month
                    , DateTime.Now.Day, 0, 0, 0);
                var list = ts.AllTasks;
                string taskName = "asdf";


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
                    Enabled = false
                }, new ExecAction("notepade.exe", @"C:\Users\Administrator\Desktop\initdata.xml", @"C:\Users\Administrator\Desktop"));
                var definition = addTask.Definition;
//                definition.RegistrationInfo.
                definition.RegistrationInfo.Description = "发票备份，每天00：00：00开始执行";
                definition.Settings.DisallowStartIfOnBatteries = false;
                definition.Settings.Enabled = true;
//                definition.Settings.ExecutionTimeLimit=TimeSpan.FromHours(2);

                definition.Settings.AllowDemandStart = true;
                //definition.Settings.
                definition.Settings.Priority=ProcessPriorityClass.High;
                definition.Settings.StopIfGoingOnBatteries = false;


                Task t = addTask;
                TaskEditDialog edit = new TaskEditDialog();
                edit.Editable = true;
                edit.RegisterTaskOnAccept = true;
                edit.Initialize(t);
                edit.ShowDialog();
//                ts.RootFolder.DeleteTask("ksd");
                int i = 0;
            }
        }
    }
}