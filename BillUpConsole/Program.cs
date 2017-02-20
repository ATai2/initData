using System;
using System.Diagnostics;
using BillUpConsole.dal;
using BillUpConsole.controllers;

namespace BillUpConsole
{
    class Program
    {
        static void Main(string[] args)
        {
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
            Console.WriteLine("共计用时： " + sw.ElapsedMilliseconds / 1000 + "s.");
            Console.ReadKey();
        }
    }
}
