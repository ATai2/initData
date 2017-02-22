using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace 数据还原
{
    public partial class Form1 : Form
    {
        private ConfigModel configModel;
        public Form1()
        {
            InitializeComponent();
            configModel=new ConfigModel();
            configModel.init("initdata.xml");
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

            var fileList=Directory.GetFiles(path);
            foreach (var file in fileList)
            {
                if (File.Exists(file))
                {
                    var fileName = Path.GetFileName(file);
                    File.Move(file, @"D:\天庸公司\数字凭证(制作)\"+fileName);
                }
            }
            MessageBox.Show("图片移动完毕");

        }

        private void btnDb_Click(object sender, EventArgs e)
        {
            rtBox.Text += "开始系统恢复：";

            SqlHelper.SetConnString(configModel.Remote);
            var remoteConn = SqlHelper.GetConnection();

            SqlHelper.SetConnString(configModel.Local);
            var localConn = SqlHelper.GetConnection();

            var ds=SqlHelper.ExecuteDataset(remoteConn, CommandType.Text, "select name from sysobjects where xtype='U'");
            var table=ds.Tables[0];

            var sb=new StringBuilder();

            for (int i = 0; i < table.Rows.Count; i++)
            {
                sb.Append("drop table ").Append(table.Rows[i][0]).Append(";");
            }

            var drop = sb.ToString();
            
            SqlHelper.ExecuteNonQuery(remoteConn, CommandType.Text, drop);

            rtBox.Text += "执行："+drop;





            //根据表数量添加表  默认5张表
            var sql = "";

            //根据数据量添加记录    不定
            
            
            //根据记录生成图片

            for (int i = 1; i < 6; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    try
                    {
                        //获得模板图片    D:\天庸公司\\上海市财政局_非税收入_上海交通大学医学院附属新华医院_xinhua4100.tiff
                        //图片编号，第一位表  后三位记录编号
                        var destFileName = @"D:\天庸公司\数字凭证(制作)\上海市财政局_非税收入_上海交通大学医学院附属新华医院_xinhua" + i + j.ToString("000") + ".tiff";
                        var copyFileName = @"D:\BILL\tiff\上海市财政局_非税收入_上海交通大学医学院附属新华医院_xinhua" + i + j.ToString("000") + ".tiff";
                        if (File.Exists(destFileName))
                        {
                            File.Delete(destFileName);
                            rtBox.Text += "删除图片" + destFileName;
                        }

                        if (File.Exists(copyFileName))
                        {
                            File.Delete(copyFileName);
                            rtBox.Text += "删除图片" + copyFileName;

                        }
                        File.Copy(@"D:\天庸公司\上海市财政局_非税收入_上海交通大学医学院附属新华医院_xinhua4100.tiff", destFileName);
                        rtBox.Text += "复制图片" + destFileName;
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("文件读取出错，请手动移动文件");
//                        throw;
                    }

                }
            }


        }
    }
}
