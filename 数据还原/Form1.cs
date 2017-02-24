using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Timers;
using Timer = System.Timers.Timer;

namespace 数据还原
{
    public partial class Form1 : Form
    {
        delegate void UpdateUI(string str);

        private UpdateUI updelegate;
        private ConfigModel configModel;

        public Form1()
        {
            InitializeComponent();
            configModel = new ConfigModel();
            configModel.init("initdata.xml");
            // updelegate=new UpdateUI();
        }

        /// <summary>
        /// 将移走的图片回复到原来的位置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPic_Click(object sender, EventArgs e)
        {
            //获得路径下所有的图片
            var path = configModel.Pic;
            if (!Directory.Exists(path))
            {
                MessageBox.Show("图片路径不存在，请初始化参数");
                return;
            }

            var fileList = Directory.GetFiles(path);
            foreach (var file in fileList)
            {
                if (File.Exists(file))
                {
                    var fileName = Path.GetFileName(file);
                    if (File.Exists(fileName))
                    {
                        File.Delete(fileName);
                    }

                    File.Move(file, @"D:\天庸公司\数字凭证(制作)\" + fileName);
                }
            }
            MessageBox.Show("图片移动完毕");
        }

        Timer timer = new Timer();
        List<int> list=new List<int>();



        private void button1_Click(object sender, EventArgs e)

        {
            rtBox.Focus();
            rtBox.Select(rtBox.TextLength, 0);
            rtBox.ScrollToCaret();
            rtBox.Text = "";
            rtBox.AppendText("开始系统恢复：\r\n");

            SqlHelper.SetConnString(configModel.Remote);
            var remoteConn = SqlHelper.GetConnection();

            SqlHelper.SetConnString(configModel.Local);
            var localConn = SqlHelper.GetConnection();

            var ds = SqlHelper.ExecuteDataset(remoteConn, CommandType.Text,
                "select name from sysobjects where xtype='U'");
            var table = ds.Tables[0];
            var sb = new StringBuilder();

            for (int i = 0; i < table.Rows.Count; i++)
            {
                sb.Append("drop table ").Append(table.Rows[i][0]).Append(";");
            }
            var drop = sb.ToString();
            if (!string.IsNullOrEmpty(drop))
            {
                SqlHelper.ExecuteNonQuery(remoteConn, CommandType.Text, drop);
                RichBoxText("执行：" + drop + "\r\n");
            }
            //获得路径下所有的图片
            var path = configModel.Pic;
            if (!Directory.Exists(path))
            {
                MessageBox.Show("图片路径不存在，请初始化参数");
                return;
            }

            var fileList = Directory.GetFiles(path);
            foreach (var file in fileList)
            {
                if (File.Exists(file))
                {
                    if (File.Exists(file))
                    {
                        RichBoxText("目标文件夹tiff文件删除：" + file + "\r\n");
                        File.Delete(file);
                    }
                }
            }

            if (string.IsNullOrEmpty(textBox3.Text))
            {
                return;
            }
           
            timer.Elapsed += new ElapsedEventHandler(DoWork);
            timer.Interval = Convert.ToInt32(textBox3.Text);
            timer.AutoReset = true; //设置是执行一次（false）还是一直执行(true)；   
            timer.Enabled = true; //是否执行System.Timers.Timer.Elapsed事件； 
            //            timer.Interval = 100;
            //           
            //            timer.Tick += Timer_Tick;
            // timer.Start();
            updelegate = new UpdateUI(RichBoxText);
//            updelegate += RichBoxText;
            Thread th = new Thread(Operater);
            th.IsBackground = true;
            th.Start();
            //  Operater();
        }

        private void DoWork(object sender, ElapsedEventArgs e)
        {
            Operater();
        }
        Random random=new Random();
        private void Operater()
        {
            //根据表数量添加表  默认5张表
            var sqlsb = new StringBuilder();

//            SqlHelper.SetConnString("server=127.0.0.1;database=Enuo63305330;uid=sa;pwd=63305330");
            SqlHelper.SetConnString(configModel.Local);
//            INSERT INTO[dbo].[ty_FillInfo4] ([lIndex], [sKey], [sVoucherNo], [SUNITNAME_1], [PURCHASENO_1], [CLASSITEMNAME_1], [GOODSNO_1], [SUSERNAME_1], [SMEMO_1], [SMEMO1_1], [SMEMO2_1], [SUBJECTNAME_1], [LIMITNUM_1], [SUBJECTNAME1_1], [LIMITNUM1_1], [SUBJECTNAME2_1], [LIMITNUM2_1], [SUBJECTNAME3_1], [LIMITNUM3_1], [SUBJECTNAME4_1], [LIMITNUM4_1], [SUBJECTNAME5_1], [LIMITNUM5_1], [SUBJECTNAME6_1], [LIMITNUM6_1], [SMONEYB_1], [MEDICALCARE_1], [MEDICALCARE1_1], [MEDICALCARE2_1], [MEDICALCARE3_1], [MEDICALCARE4_1], [MEDICALCARE5_1], [MEDICALCARE6_1], [MEDICALCARE7_1], [MEDICALCARE8_1], [SPURPOSE_1], [SPURPOSE9_1], [NUMBER_1], [PRICE_1], [SMONEY_1], [SPURPOSE1_1], [SPURPOSE10_1], [NUMBER1_1], [PRICE1_1], [SMONEY1_1], [SPURPOSE2_1], [SPURPOSE11_1], [NUMBER2_1], [PRICE2_1], [SMONEY2_1], [SPURPOSE3_1], [SPURPOSE12_1], [NUMBER3_1], [PRICE3_1], [SMONEY3_1], [SPURPOSE4_1], [SPURPOSE13_1], [NUMBER4_1], [PRICE4_1], [SMONEY4_1], [SPURPOSE5_1], [SPURPOSE14_1], [NUMBER5_1], [PRICE5_1], [SMONEY5_1], [SPURPOSE6_1], [SPURPOSE15_1], [NUMBER6_1], [PRICE6_1], [SMONEY6_1], [SPURPOSE7_1], [SPURPOSE16_1], [NUMBER7_1], [PRICE7_1], [SMONEY7_1], [SPURPOSE8_1], [SPURPOSE17_1], [NUMBER8_1], [PRICE8_1], [SMONEY8_1], [SPURPOSE18_1], [SMONEYRNAME_1], [SBIRTHDAY_1], [sShareField], [sEmail], [sState]) VALUES(N'6', N'sKey5', N'xinhua45', N'新华医院', N'111223333', N'国有医院', N'2017010511235', N'张三5', N'男', N'居民医保', N'1960012077775', N'床位费', N'0.0', N'西药费', N'0.0', N'中药费', N'0.0', N'氧气费', N'0.0', N'护理费', N'0.0', N'检查费', N'0.0', N'材料费', N'0.0', N'0.05', N'0.05', N'0.05', N'0.05', N'0.05', N'0.05', N'0.05', N'0.05', N'0.05', N'0.05', N'999胃泰', N'瓶', N'3', N'90.00', N'270.00', N'CT', N'次', N'1', N'1200.00', N'1200.00', N'血常规', N'次', N'2', N'70.00', N'140.00', N'手术费', N'次', N'1', N'7000.00', N'7000.00', N'手术护理费', N'天', N'41', N'200.00', N'8000.00', N'床位费', N'天', N'40', N'120.00', N'4800.00', N'B超', N'次', N'2', N'600.00', N'1200.00', N'中成药', N'副', N'7', N'40.00', N'280.00', N'治疗费', N'次', N'8', N'100.00', N'800.00', N'随访', N'李四', N'2016-10-25', N'sShareField', N'sEmail', N'1')

            if (string.IsNullOrEmpty(textBox2.Text))
            {
                return;
            }

            var size = Convert.ToInt32(textBox2.Text);
            int i, j;
            i = random.Next(1, 5);
            j = random.Next(0, 10000);

            if (list.Contains(j))
            {
                return;
            }

            var sql = "INSERT INTO[dbo].[ty_FillInfo" +
                      i +
                      "] ( [sKey], [sVoucherNo], [SUNITNAME_1], [PURCHASENO_1], " +
                      "[CLASSITEMNAME_1], [GOODSNO_1], [SUSERNAME_1], [SMEMO_1], [SMEMO1_1], [SMEMO2_1], " +
                      "[SUBJECTNAME_1], [LIMITNUM_1], [SUBJECTNAME1_1], [LIMITNUM1_1], [SUBJECTNAME2_1], " +
                      "[LIMITNUM2_1], [SUBJECTNAME3_1], [LIMITNUM3_1], [SUBJECTNAME4_1], [LIMITNUM4_1], " +
                      "[SUBJECTNAME5_1], [LIMITNUM5_1], [SUBJECTNAME6_1], [LIMITNUM6_1], [SMONEYB_1], [MEDICALCARE_1]," +
                      " [MEDICALCARE1_1], [MEDICALCARE2_1], [MEDICALCARE3_1], [MEDICALCARE4_1], [MEDICALCARE5_1], " +
                      "[MEDICALCARE6_1], [MEDICALCARE7_1], [MEDICALCARE8_1], [SPURPOSE_1], [SPURPOSE9_1], [NUMBER_1]," +
                      " [PRICE_1], [SMONEY_1], [SPURPOSE1_1], [SPURPOSE10_1], [NUMBER1_1], [PRICE1_1], [SMONEY1_1], " +
                      "[SPURPOSE2_1], [SPURPOSE11_1], [NUMBER2_1], [PRICE2_1], [SMONEY2_1], [SPURPOSE3_1], [SPURPOSE12_1]," +
                      " [NUMBER3_1], [PRICE3_1], [SMONEY3_1], [SPURPOSE4_1], [SPURPOSE13_1], [NUMBER4_1], [PRICE4_1], " +
                      "[SMONEY4_1], [SPURPOSE5_1], [SPURPOSE14_1], [NUMBER5_1], [PRICE5_1], [SMONEY5_1], [SPURPOSE6_1]," +
                      " [SPURPOSE15_1], [NUMBER6_1], [PRICE6_1], [SMONEY6_1], [SPURPOSE7_1], [SPURPOSE16_1], [NUMBER7_1], " +
                      "[PRICE7_1], [SMONEY7_1], [SPURPOSE8_1], [SPURPOSE17_1], [NUMBER8_1], [PRICE8_1], [SMONEY8_1], " +
                      "[SPURPOSE18_1], [SMONEYRNAME_1], [SBIRTHDAY_1], [sShareField], [sEmail], [sState]) VALUES( N'sKey" +
                      i + j.ToString("0000") +
                      "'," +
                      " N'xinhua" +
                      i + j.ToString("0000") +
                      "', N'新华医院', N'111223333', N'国有医院', N'2017010511235', " +
                      "N'张三5', N'男', N'居民医保', N'1960012077775', N'床位费', N'0.0', N'西药费', N'0.0'," +
                      " N'中药费', N'0.0', N'氧气费', N'0.0', N'护理费', N'0.0', N'检查费', N'0.0', N'材料费'," +
                      " N'0.0', N'0.05', N'0.05', N'0.05', N'0.05', N'0.05', N'0.05', N'0.05', N'0.05', N'0.05'," +
                      " N'0.05', N'999胃泰', N'瓶', N'3', N'90.00', N'270.00', N'CT', N'次', N'1', N'1200.00', N'1200.00', " +
                      "N'血常规', N'次', N'2', N'70.00', N'140.00', N'手术费', N'次', N'1', N'7000.00', N'7000.00', " +
                      "N'手术护理费', N'天', N'41', N'200.00', N'8000.00', N'床位费', N'天', N'40', N'120.00', N'4800.00'," +
                      " N'B超', N'次', N'2', N'600.00', N'1200.00', N'中成药', N'副', N'7', N'40.00', N'280.00', N'治疗费', " +
                      "N'次', N'8', N'100.00', N'800.00', N'随访', N'李四', N'2016-10-25', N'sShareField', N'sEmail', N's')";


            var testConn = SqlHelper.GetConnection();
            ;
            if (SqlHelper.ExecuteNonQuery(testConn, CommandType.Text, sql) > 0)
            {
                RichBoxText("向数据库表：" +
                            i +
                            "插入一条数据。\r\n");
            }
        }

        private void RichBoxText(string str)
        {
            Action<String> uidele = delegate(string s) { rtBox.AppendText(s); };
            // this.Invoke(updelegate);
            rtBox.Invoke(uidele, new object[] {str});
        }

        private void button2_Click(object sender, EventArgs e)
        {
            timer.Stop();
            RichBoxText("停止插入数据\r\n");
        }

        private void btnDb_Click(object sender, EventArgs e)
        {
            RichBoxText("开始系统恢复：\r\n");

            SqlHelper.SetConnString(configModel.Remote);
            var remoteConn = SqlHelper.GetConnection();

            SqlHelper.SetConnString(configModel.Local);
            var localConn = SqlHelper.GetConnection();

            var ds = SqlHelper.ExecuteDataset(remoteConn, CommandType.Text,
                "select name from sysobjects where xtype='U'");
            var table = ds.Tables[0];

            if (table != null && table.Rows.Count > 0)
            {
                var sb = new StringBuilder();

                for (int i = 0; i < table.Rows.Count; i++)
                {
                    sb.Append("drop table ").Append(table.Rows[i][0]).Append(";");
                }

                var drop = sb.ToString();

                SqlHelper.ExecuteNonQuery(remoteConn, CommandType.Text, drop);

                RichBoxText("执行：" + drop+"\r\n");
            }


            //根据表数量添加表  默认5张表
            var sqlsb = new StringBuilder();

            //根据数据量添加记录    不定

            //根据记录生成图片

            for (int i = 1; i <= Convert.ToInt16(textBox1.Text.Trim()); i++)
            {
                for (int j = 0; j < Convert.ToInt16(textBox2.Text.Trim()); j++)
                {
                   
                        var sql = "INSERT INTO[dbo].[ty_FillInfo" +
                    i +
                    "] ( [sKey], [sVoucherNo], [SUNITNAME_1], [PURCHASENO_1], " +
                    "[CLASSITEMNAME_1], [GOODSNO_1], [SUSERNAME_1], [SMEMO_1], [SMEMO1_1], [SMEMO2_1], " +
                    "[SUBJECTNAME_1], [LIMITNUM_1], [SUBJECTNAME1_1], [LIMITNUM1_1], [SUBJECTNAME2_1], " +
                    "[LIMITNUM2_1], [SUBJECTNAME3_1], [LIMITNUM3_1], [SUBJECTNAME4_1], [LIMITNUM4_1], " +
                    "[SUBJECTNAME5_1], [LIMITNUM5_1], [SUBJECTNAME6_1], [LIMITNUM6_1], [SMONEYB_1], [MEDICALCARE_1]," +
                    " [MEDICALCARE1_1], [MEDICALCARE2_1], [MEDICALCARE3_1], [MEDICALCARE4_1], [MEDICALCARE5_1], " +
                    "[MEDICALCARE6_1], [MEDICALCARE7_1], [MEDICALCARE8_1], [SPURPOSE_1], [SPURPOSE9_1], [NUMBER_1]," +
                    " [PRICE_1], [SMONEY_1], [SPURPOSE1_1], [SPURPOSE10_1], [NUMBER1_1], [PRICE1_1], [SMONEY1_1], " +
                    "[SPURPOSE2_1], [SPURPOSE11_1], [NUMBER2_1], [PRICE2_1], [SMONEY2_1], [SPURPOSE3_1], [SPURPOSE12_1]," +
                    " [NUMBER3_1], [PRICE3_1], [SMONEY3_1], [SPURPOSE4_1], [SPURPOSE13_1], [NUMBER4_1], [PRICE4_1], " +
                    "[SMONEY4_1], [SPURPOSE5_1], [SPURPOSE14_1], [NUMBER5_1], [PRICE5_1], [SMONEY5_1], [SPURPOSE6_1]," +
                    " [SPURPOSE15_1], [NUMBER6_1], [PRICE6_1], [SMONEY6_1], [SPURPOSE7_1], [SPURPOSE16_1], [NUMBER7_1], " +
                    "[PRICE7_1], [SMONEY7_1], [SPURPOSE8_1], [SPURPOSE17_1], [NUMBER8_1], [PRICE8_1], [SMONEY8_1], " +
                    "[SPURPOSE18_1], [SMONEYRNAME_1], [SBIRTHDAY_1], [sShareField], [sEmail], [sState]) VALUES( N'sKey" +
                    i + j.ToString("0000") +
                    "'," +
                    " N'xinhua" +
                    i + j.ToString("0000") +
                    "', N'新华医院', N'111223333', N'国有医院', N'2017010511235', " +
                    "N'张三5', N'男', N'居民医保', N'1960012077775', N'床位费', N'0.0', N'西药费', N'0.0'," +
                    " N'中药费', N'0.0', N'氧气费', N'0.0', N'护理费', N'0.0', N'检查费', N'0.0', N'材料费'," +
                    " N'0.0', N'0.05', N'0.05', N'0.05', N'0.05', N'0.05', N'0.05', N'0.05', N'0.05', N'0.05'," +
                    " N'0.05', N'999胃泰', N'瓶', N'3', N'90.00', N'270.00', N'CT', N'次', N'1', N'1200.00', N'1200.00', " +
                    "N'血常规', N'次', N'2', N'70.00', N'140.00', N'手术费', N'次', N'1', N'7000.00', N'7000.00', " +
                    "N'手术护理费', N'天', N'41', N'200.00', N'8000.00', N'床位费', N'天', N'40', N'120.00', N'4800.00'," +
                    " N'B超', N'次', N'2', N'600.00', N'1200.00', N'中成药', N'副', N'7', N'40.00', N'280.00', N'治疗费', " +
                    "N'次', N'8', N'100.00', N'800.00', N'随访', N'李四', N'2016-10-25', N'sShareField', N'sEmail', N's')";
                    sqlsb.Append(sql);
                     
                    }
                   
                }
            

            SqlHelper.SetConnString(configModel.Local);
            var testConn = SqlHelper.GetConnection();
            ;
            if (SqlHelper.ExecuteNonQuery(testConn, CommandType.Text, sqlsb.ToString()) > 0)
            {
                RichBoxText("向Local数据库插入数据成功\r\n");
            }
        }
    }
}