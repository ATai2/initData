using System;
using System.Diagnostics;
using System.Timers;
using BillUpConsole.dal;
using BillUpConsole.controllers;
using log4net;

namespace BillUpConsole
{
    class Program
    {
        static void Main(string[] args)
        {

                Do();


//            Timer timer=new Timer(2000);
//            timer.Elapsed+=new ElapsedEventHandler(DoWork);
//            Console.ReadKey();
//            Stopwatch sw = new Stopwatch();
//            sw.Start();
//            Console.WriteLine("任务开始执行……");
//            iOperator.CopyTableList();
//            iOperator.InitTable();
//            iOperator.DataTransfer();
//            //            iOperator.Close();
//
//            Console.WriteLine("任务结束。查看日志请移步日志记录……");
//            sw.Stop();
//            Console.WriteLine("共计用时： " + sw.ElapsedMilliseconds / 1000 + "s.");
//            log.Info("共计用时： " + sw.ElapsedMilliseconds / 1000 + "s.");
////            Console.ReadKey();
        }

        private static void DoWork(object sender, ElapsedEventArgs e)
        {
            Do();
        }

        private static void Do()
        {
            ILog log = LogManager.GetLogger("Main");


            IOperator iOperator = new DbSqlHelper();
            Stopwatch sw = new Stopwatch();
            sw.Start();
            Console.WriteLine("任务开始执行……");
            iOperator.CopyTableList();
            iOperator.InitTable();
            iOperator.DataTransfer();
            //            iOperator.Close();

            Console.WriteLine("任务结束。查看日志请移步日志记录……");
            sw.Stop();
            Console.WriteLine("共计用时： " + sw.ElapsedMilliseconds/1000 + "s.");
            log.Info("共计用时： " + sw.ElapsedMilliseconds/1000 + "s.");
        }
    }
}
