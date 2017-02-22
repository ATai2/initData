using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LinqDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            TableFillDataContext tableFillData = new TableFillDataContext();
            var query = tableFillData.ty_FillTableList;
            foreach (var tableList in query)
            {
                Console.WriteLine($"{tableList.SealID}   {tableList.lIndex}");
            }
            
        }
    }
}
