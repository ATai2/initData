using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using BillBackUpcs.controllers;
using BillBackUpcs.dal;
using BillBackUpcs.test;
using log4net;

namespace BillBackUpcs
{
    class Program
    {
        static void Main(string[] args)
        {

            IOperator iOperator=new DbSqlHelper();
            Stopwatch sw=new Stopwatch();
            sw.Start();
            Console.WriteLine("任务开始执行……");
            iOperator.InitTable();
            iOperator.DataTransfer();
//            iOperator.Close();

            Console.WriteLine("任务结束。查看日志请移步日志记录……");
            sw.Stop();
            Console.WriteLine("共计用时： " + sw.ElapsedMilliseconds/1000 + "s.");
            Console.ReadKey();
        }
    }
}
