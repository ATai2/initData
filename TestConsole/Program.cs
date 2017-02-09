using System;
using System.Collections.Generic;
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
            //            Console.WriteLine(Path.PathSeparator+ DateTime.Now.ToString("yyyy-M-d"));
            string ip = "192.168.1.198";
            Ping ping = new Ping();
            var reply = ping.Send(ip);
            if (reply != null && reply.Status == IPStatus.Success)
            {
                Console.WriteLine("lskdjfl");
            }
            else
            {
                
            }

            Console.ReadKey();
        }
    }
}
