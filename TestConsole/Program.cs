using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace TestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            initTableDates();


            //            Console.WriteLine(Path.PathSeparator+ DateTime.Now.ToString("yyyy-M-d"));
            //            string ip = "192.168.1.198";
            //            Ping ping = new Ping();
            //            var reply = ping.Send(ip);
            //            if (reply != null && reply.Status == IPStatus.Success)
            //            {
            //                Console.WriteLine("lskdjfl");
            //            }
            //            else
            //            {
            //                
            //            }
            //
            //            Console.ReadKey();
        }

        private static void initTableDates()
        {
            string con, sql;
            //连接sqlserver数据库
            con = "server=192.168.1.198; Database = Enuo63305330; user id = sa; password = 63305330";
            SqlConnection mycon = new SqlConnection(con);
            mycon.Open();
            Stopwatch sw = new Stopwatch();
            sw.Start();
            //查询出所有要备份的表

            for (int k = 1; k < 11; k++)
            {
                string creatTableSql = "CREATE TABLE [dbo].[ty_FillInfo" + k + "] ( " +
                                       "[lIndex] bigint NOT NULL IDENTITY(1,1) , " +
                                       "[sKey] varchar(40) NOT NULL , " +
                                       "[sVoucherNo] varchar(40) NULL , " +
                                       "[SUNITNAME_1] varchar(40) NULL , " +
                                       "[PURCHASENO_1] varchar(20) NULL , " +
                                       "[CLASSITEMNAME_1] varchar(48) NULL , " +
                                       "[GOODSNO_1] varchar(20) NULL , " +
                                       "[SUSERNAME_1] varchar(16) NULL , " +
                                       "[SMEMO_1] varchar(4) NULL , " +
                                       "[SMEMO1_1] varchar(16) NULL , " +
                                       "[SMEMO2_1] varchar(16) NULL , " +
                                       "[SUBJECTNAME_1] varchar(16) NULL , " +
                                       "[LIMITNUM_1] varchar(13) NULL , " +
                                       "[SUBJECTNAME1_1] varchar(16) NULL , " +
                                       "[LIMITNUM1_1] varchar(13) NULL , " +
                                       "[SUBJECTNAME2_1] varchar(16) NULL , " +
                                       "[LIMITNUM2_1] varchar(13) NULL , " +
                                       "[SUBJECTNAME3_1] varchar(16) NULL , " +
                                       "[LIMITNUM3_1] varchar(13) NULL , " +
                                       "[SUBJECTNAME4_1] varchar(16) NULL , " +
                                       "[LIMITNUM4_1] varchar(13) NULL , " +
                                       "[SUBJECTNAME5_1] varchar(16) NULL , " +
                                       "[LIMITNUM5_1] varchar(13) NULL , " +
                                       "[SUBJECTNAME6_1] varchar(16) NULL , " +
                                       "[LIMITNUM6_1] varchar(13) NULL , " +
                                       "[SMONEYB_1] varchar(36) NULL , " +
                                       "[MEDICALCARE_1] varchar(13) NULL , " +
                                       "[MEDICALCARE1_1] varchar(13) NULL , " +
                                       "[MEDICALCARE2_1] varchar(13) NULL , " +
                                       "[MEDICALCARE3_1] varchar(13) NULL , " +
                                       "[MEDICALCARE4_1] varchar(13) NULL , " +
                                       "[MEDICALCARE5_1] varchar(13) NULL , " +
                                       "[MEDICALCARE6_1] varchar(13) NULL , " +
                                       "[MEDICALCARE7_1] varchar(13) NULL , " +
                                       "[MEDICALCARE8_1] varchar(13) NULL , " +
                                       "[SPURPOSE_1] varchar(46) NULL , " +
                                       "[SPURPOSE9_1] varchar(18) NULL , " +
                                       "[NUMBER_1] varchar(3) NULL , " +
                                       "[PRICE_1] varchar(12) NULL , " +
                                       "[SMONEY_1] varchar(12) NULL , " +
                                       "[SPURPOSE1_1] varchar(46) NULL , " +
                                       "[SPURPOSE10_1] varchar(18) NULL , " +
                                       "[NUMBER1_1] varchar(3) NULL , " +
                                       "[PRICE1_1] varchar(12) NULL , " +
                                       "[SMONEY1_1] varchar(12) NULL , " +
                                       "[SPURPOSE2_1] varchar(46) NULL , " +
                                       "[SPURPOSE11_1] varchar(18) NULL , " +
                                       "[NUMBER2_1] varchar(3) NULL , " +
                                       "[PRICE2_1] varchar(12) NULL , " +
                                       "[SMONEY2_1] varchar(12) NULL , " +
                                       "[SPURPOSE3_1] varchar(46) NULL , " +
                                       "[SPURPOSE12_1] varchar(18) NULL , " +
                                       "[NUMBER3_1] varchar(3) NULL , " +
                                       "[PRICE3_1] varchar(12) NULL , " +
                                       "[SMONEY3_1] varchar(12) NULL , " +
                                       "[SPURPOSE4_1] varchar(46) NULL , " +
                                       "[SPURPOSE13_1] varchar(18) NULL , " +
                                       "[NUMBER4_1] varchar(3) NULL , " +
                                       "[PRICE4_1] varchar(12) NULL , " +
                                       "[SMONEY4_1] varchar(12) NULL , " +
                                       "[SPURPOSE5_1] varchar(46) NULL , " +
                                       "[SPURPOSE14_1] varchar(18) NULL , " +
                                       "[NUMBER5_1] varchar(3) NULL , " +
                                       "[PRICE5_1] varchar(12) NULL , " +
                                       "[SMONEY5_1] varchar(12) NULL , " +
                                       "[SPURPOSE6_1] varchar(46) NULL , " +
                                       "[SPURPOSE15_1] varchar(18) NULL , " +
                                       "[NUMBER6_1] varchar(3) NULL , " +
                                       "[PRICE6_1] varchar(12) NULL , " +
                                       "[SMONEY6_1] varchar(12) NULL , " +
                                       "[SPURPOSE7_1] varchar(46) NULL , " +
                                       "[SPURPOSE16_1] varchar(18) NULL , " +
                                       "[NUMBER7_1] varchar(3) NULL , " +
                                       "[PRICE7_1] varchar(12) NULL , " +
                                       "[SMONEY7_1] varchar(12) NULL , " +
                                       "[SPURPOSE8_1] varchar(46) NULL , " +
                                       "[SPURPOSE17_1] varchar(18) NULL , " +
                                       "[NUMBER8_1] varchar(3) NULL , " +
                                       "[PRICE8_1] varchar(12) NULL , " +
                                       "[SMONEY8_1] varchar(12) NULL , " +
                                       "[SPURPOSE18_1] varchar(192) NULL , " +
                                       "[SMONEYRNAME_1] varchar(10) NULL , " +
                                       "[SBIRTHDAY_1] varchar(19) NULL , " +
                                       "[sShareField] varchar(MAX) NULL , " +
                                       "[sEmail] varchar(80) NULL , " +
                                       "[sState] varchar(1) NOT NULL DEFAULT ('0')  " +
                                       ") ;" +
                                       "ALTER TABLE [dbo].[ty_FillInfo" + k + "] ADD PRIMARY KEY ([lIndex])";
                SqlCommand createCommand = new SqlCommand(creatTableSql, mycon);
                createCommand.ExecuteNonQuery();
                for (int i = 0; i < 1000; i++)
                {
                    sql = "insert into ty_FillInfo" + k + " ("
                          +
                          "sKey,sVoucherNo,SUNITNAME_1,PURCHASENO_1,CLASSITEMNAME_1,GOODSNO_1,SUSERNAME_1,SMEMO_1,SMEMO1_1,SMEMO2_1,SUBJECTNAME_1,LIMITNUM_1,SUBJECTNAME1_1,LIMITNUM1_1,SUBJECTNAME2_1,LIMITNUM2_1,SUBJECTNAME3_1,LIMITNUM3_1,SUBJECTNAME4_1,LIMITNUM4_1,SUBJECTNAME5_1,LIMITNUM5_1,SUBJECTNAME6_1,LIMITNUM6_1,SMONEYB_1,MEDICALCARE_1,MEDICALCARE1_1,MEDICALCARE2_1,MEDICALCARE3_1,MEDICALCARE4_1,MEDICALCARE5_1,MEDICALCARE6_1,MEDICALCARE7_1,MEDICALCARE8_1,SPURPOSE_1,SPURPOSE9_1,NUMBER_1,PRICE_1,SMONEY_1,SPURPOSE1_1,SPURPOSE10_1,NUMBER1_1,PRICE1_1,SMONEY1_1,SPURPOSE2_1,SPURPOSE11_1,NUMBER2_1,PRICE2_1,SMONEY2_1,SPURPOSE3_1,SPURPOSE12_1,NUMBER3_1,PRICE3_1,SMONEY3_1,SPURPOSE4_1,SPURPOSE13_1,NUMBER4_1,PRICE4_1,SMONEY4_1,SPURPOSE5_1,SPURPOSE14_1,NUMBER5_1,PRICE5_1,SMONEY5_1,SPURPOSE6_1,SPURPOSE15_1,NUMBER6_1,PRICE6_1,SMONEY6_1,SPURPOSE7_1,SPURPOSE16_1,NUMBER7_1,PRICE7_1,SMONEY7_1,SPURPOSE8_1,SPURPOSE17_1,NUMBER8_1,PRICE8_1,SMONEY8_1,SPURPOSE18_1,SMONEYRNAME_1,SBIRTHDAY_1,sShareField,sEmail,sState) "
                          + "values('sKey" + i + "','xinhua" + i + "','新华医院','111223333','国有医院','201701051123" + i + "','张三" + i +
                          "','男','居民医保','1960012077775','床位费','0.0','西药费','0.0','中药费','0.0','氧气费','0.0','护理费','0.0','检查费','0.0','材料费','0.0','0.0" +
                          i + "','0.0" + i + "','0.0" + i + "','0.0" + i + "','0.0" + i + "','0.0" + i + "','0.0" + i + "','0.0" +
                          i + "','0.0" + i + "','0.0" + i +
                          "','999胃泰','瓶','3','90.00','270.00','CT','次','1','1200.00','1200.00','血常规','次','2','70.00','140.00','手术费','次','1','7000.00','7000.00','手术护理费','天','41','200.00','8000.00','床位费','天','40','120.00','4800.00','B超','次','2','600.00','1200.00','中成药','副','7','40.00','280.00','治疗费','次','8','100.00','800.00','随访','李四','2016-10-25','sShareField','sEmail','1')";

                    SqlCommand sqlCmd = new SqlCommand(sql, mycon);
                    try
                    {
                        sqlCmd.ExecuteNonQuery();
                    }
                    catch (Exception e)
                    {
                        throw e;
                    }
                }
            }

            mycon.Close();
            sw.Stop();
            Console.WriteLine(sw.Elapsed);
        }
    }
}
