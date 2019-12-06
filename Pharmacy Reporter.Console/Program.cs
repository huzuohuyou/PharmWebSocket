using PrintDemo;
using System;
using WebSocketSharp.Server;

namespace Pharmacy_Reporter.Consoler
{
    class Program
    {
        static void Main(string[] args)
        {
            int num = new Print532().OpenPrinter(11, 9600, 1);
            Console.WriteLine( num == 0 ? "打印机联机 ..." : "打印机脱机 ...");
            var wssv = new WebSocketServer(6690);
            wssv.AddWebSocketService<App>("/app");
            wssv.Start();
            Console.WriteLine("Server starting, press any key to terminate the server.");
            Console.ReadKey(true);
            wssv.Stop();
        }
    }
}


