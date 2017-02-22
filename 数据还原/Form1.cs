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
            configModel = new ConfigModel();
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

            var fileList = Directory.GetFiles(path);
            foreach (var file in fileList)
            {
                if (File.Exists(file))
                {
                    var fileName = Path.GetFileName(file);
                    File.Move(file, @"D:\天庸公司\数字凭证(制作)\" + fileName);
                }
            }
            MessageBox.Show("图片移动完毕");
        }

        private void btnDb_Click(object sender, EventArgs e)
        {
            rtBox.AppendText("开始系统恢复：");

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

            SqlHelper.ExecuteNonQuery(remoteConn, CommandType.Text, drop);

            rtBox.AppendText("执行：" + drop);


            //根据表数量添加表  默认5张表
            var sqlsb = new StringBuilder();

            //根据数据量添加记录    不定

            //根据记录生成图片

            for (int i = 1; i <= Convert.ToInt16(textBox1.Text.Trim()); i++)
            {
                for (int j = 0; j < Convert.ToInt16(textBox2.Text.Trim()); j++)
                {
                    try
                    {
                        var sql = "insert into testTable ("
                                  + "sKey,sVoucherNo,SUNITNAME_1,PURCHASENO_1," +
                                  "CLASSITEMNAME_1,GOODSNO_1,SUSERNAME_1,SMEMO_1," +
                                  "SMEMO1_1,SMEMO2_1,SUBJECTNAME_1,LIMITNUM_1," +
                                  "SUBJECTNAME1_1,LIMITNUM1_1,SUBJECTNAME2_1," +
                                  "LIMITNUM2_1,SUBJECTNAME3_1,LIMITNUM3_1,SUBJECTNAME4_1," +
                                  "LIMITNUM4_1,SUBJECTNAME5_1,LIMITNUM5_1,SUBJECTNAME6_1," +
                                  "LIMITNUM6_1,SMONEYB_1,MEDICALCARE_1,MEDICALCARE1_1," +
                                  "MEDICALCARE2_1,MEDICALCARE3_1,MEDICALCARE4_1,MEDICALCARE5_1,MEDICALCARE6_1,MEDICALCARE7_1,MEDICALCARE8_1,SPURPOSE_1,SPURPOSE9_1,NUMBER_1,PRICE_1,SMONEY_1,SPURPOSE1_1,SPURPOSE10_1,NUMBER1_1,PRICE1_1,SMONEY1_1,SPURPOSE2_1,SPURPOSE11_1,NUMBER2_1,PRICE2_1,SMONEY2_1,SPURPOSE3_1,SPURPOSE12_1,NUMBER3_1,PRICE3_1,SMONEY3_1,SPURPOSE4_1,SPURPOSE13_1,NUMBER4_1,PRICE4_1,SMONEY4_1,SPURPOSE5_1,SPURPOSE14_1,NUMBER5_1,PRICE5_1,SMONEY5_1,SPURPOSE6_1,SPURPOSE15_1,NUMBER6_1,PRICE6_1,SMONEY6_1,SPURPOSE7_1,SPURPOSE16_1,NUMBER7_1,PRICE7_1,SMONEY7_1,SPURPOSE8_1,SPURPOSE17_1,NUMBER8_1,PRICE8_1,SMONEY8_1,SPURPOSE18_1,SMONEYRNAME_1,SBIRTHDAY_1,sShareField,sEmail,sState,sVoucherKey,sVoucherName,lArchiveIndex,sTableName,sHadUseCode,SealID,WriteTime,signInfo,sTableGroupName,UpdateTime,iState,sUniquekey,sSymmetrickey,iSymmetrickeyLen,sPortState1,sPortState2,sMailState,sDirState,sRcvAck,sFilePathName,dtMakeTime,sDesc) "
                                  + "values('sKey" + j + "','xinhua" + j + "','新华医院','111223333','国有医院','201701051123" +
                                  j + "','张三" + j + "','男','居民医保'," +
                                  "'1960012077775','床位费','0.0','西药费','0.0','中药费','0.0','氧气费','0.0','护理费','0.0','检查费','0.0','材料费','0.0','0.0" +
                                  j + "','0.0" + j + "','0.0" + j + "','0.0" + j + "','0.0" + j + "','0.0" + j +
                                  "','0.0" + j + "','0.0" + j + "','0.0" + i + "','0.0" + i +
                                  "','999胃泰','瓶','3','90.00','270.00','CT','次','1','1200.00','1200.00','血常规','次','2','70.00','140.00','手术费','次','1','7000.00','7000.00','手术护理费','天','41','200.00','8000.00','床位费','天','40','120.00','4800.00','B超','次','2','600.00','1200.00','中成药','副','7','40.00','280.00','治疗费','次','8'," +
                                  "'100.00','800.00','随访','李四','2016-10-25','sShareField','sEmail','1','8E8B0A56866C4C23AB9B24AA2C1BDD1A48BE4B0911E0AEC47054DAE36A9F9521','上海市财政局_非税收入','0'," +
                                  "'ty_FillInfo" + i +
                                  "','0080000000','0000005132','2017-01-06 09:47:21','047BAB8711C948A2235E6ED7954363D326402E89B9432908CACF3D80FF148779584BE8A5CAEC7CD1AC4BB7EF58372B237069E44BA39D591159EBE0B4D4664EA1FE4D83B2018FF718F342B3B4953C94DCCF984A0AA138F89D03247B98F7616F8B11BB00EE79A6A377D13F3CA476204BB324BDBF3FE6A2C196D5878ECF162C9EE0',''," +
                                  "'2017-01-06 09:47:21','0','111','2C1BAEC344F10A51F12404E8E176E9EFED2D7090A95FEF1A396CCEA366CC3CF1','64','2','2','2','2','0'," +
                                  "'D:\\天庸公司\\数字凭证(制作)\\上海市财政局_非税收入_上海交通大学医学院附属新华医院_xinhua" + i + j.ToString("000") +
                                  ".tiff','20170109103132','');";

                        sqlsb.Append(sql);

                        //获得模板图片    D:\天庸公司\\上海市财政局_非税收入_上海交通大学医学院附属新华医院_xinhua4100.tiff
                        //图片编号，第一位表  后三位记录编号
                        var destFileName = @"D:\天庸公司\数字凭证(制作)\上海市财政局_非税收入_上海交通大学医学院附属新华医院_xinhua" + i +
                                           j.ToString("000") + ".tiff";
                        var copyFileName = @"D:\BILL\tiff\上海市财政局_非税收入_上海交通大学医学院附属新华医院_xinhua" + i + j.ToString("000") +
                                           ".tiff";
                        if (File.Exists(destFileName))
                        {
                            File.Delete(destFileName);
                            rtBox.AppendText("删除图片" + destFileName);
                        }

                        if (File.Exists(copyFileName))
                        {
                            File.Delete(copyFileName);
                            rtBox.AppendText("删除图片" + copyFileName);
                        }
                        File.Copy(@"D:\天庸公司\上海市财政局_非税收入_上海交通大学医学院附属新华医院_xinhua4100.tiff", destFileName);
                        rtBox.AppendText("复制图片" + destFileName);
                    }
                    catch (Exception)
                    {
                        rtBox.AppendText("文件读取出错，请手动移动文件");
                    }
                }
            }

            SqlHelper.SetConnString("server=192.168.1.198;database=test;uid=sa;pwd=63305330");
            var testConn = SqlHelper.GetConnection();
            ;
            if (SqlHelper.ExecuteNonQuery(testConn, CommandType.Text, sqlsb.ToString()) > 0)
            {
                rtBox.AppendText("向test数据库插入数据成功");
            }
        }
    }
}