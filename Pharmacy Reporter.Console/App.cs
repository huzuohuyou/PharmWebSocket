using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Pharmacy_Reporter.Entity;
using PrintDemo;
using System;
using System.Threading;
using WebSocketSharp;
using WebSocketSharp.Server;

namespace Pharmacy_Reporter.Consoler
{
    class App : WebSocketBehavior
    {
        protected override void OnOpen()
        {
            Console.WriteLine("Connection Open");
            base.OnOpen();
        }

        private class PrintLine
        {
            public int FontHeight { get; set; }
            public int LeftOffset { get; set; }
            public string PrintValue { get; set; }
        }

        /// <summary>
        /// 传输打印模板
        /// </summary>
        private PrintLine[] CreatePrintTemplate(SigninStrip signin)
        {
            return new PrintLine[]
            {
                new PrintLine{FontHeight= 1,LeftOffset= 0,PrintValue=$"  首都医科大学附属北京朝阳医院   签到条"},
                new PrintLine{FontHeight= 1,LeftOffset= 0,PrintValue=$"    患者姓名：{signin.PTNAME}"},
                new PrintLine{FontHeight= 1,LeftOffset= 0,PrintValue=$"  患者编号：{signin.PTNO}"},
                new PrintLine{FontHeight= 3,LeftOffset= 0,PrintValue=$" 请到 {signin.WINDOW_NAME} 取药"},
                new PrintLine{FontHeight= 1,LeftOffset= 0,PrintValue=$"   签到时间：{signin.CHECK_DATE}"},
            };
        }

        private void TryOpenPrinter()
        {
            try
            {
                var printer = new Print532();
                //打印机状态，发生任何错误打印机都将返回 -１
                if (printer.RealtimeGetStatus(1) == -1)
                {
                    Console.WriteLine( "打印机故障，正在重新联机 ...");

                    if (printer.OpenPrinter(11, 9600, 1) == 0)
                        Console.WriteLine("打印机重新联机成功 ...");
                    else
                        throw new Exception("打印机已脱机 ...");
                }

                //打印机纸张状态
                switch (printer.RealtimeGetStatus(4))
                {
                    //纸张足够
                    case 18:
                        return;

                    //读取打印机状态失败
                    case -1:
                    //纸将尽检测器检测到纸张接近末端
                    case 30:
                    case 99:
                    case 114:
                    case 126:
                        throw new Exception("打印机缺纸 ...");
                }
            }
            
            catch (Exception ex)
            {
                Console.WriteLine("打印机异常:" + ex.Message);
            }
        }

        protected override void OnMessage(MessageEventArgs e)
        {

            var data = e.Data;
            if (TestJson(data))
            {

                var entity = JsonConvert.DeserializeObject<SigninStrip>(data);
                if (entity.PTNO != string.Empty && entity.PTNO != null)
                {
                    Console.WriteLine("================================================");
                    foreach (var x in this.CreatePrintTemplate(entity))
                    {
                        if (string.IsNullOrEmpty(x.PrintValue))
                            continue;

                        Print532.InStance.SelectCharacterSize((int)x.FontHeight - 1 >= 2 ? 1 : 0, (int)x.FontHeight);
                        Thread.Sleep(45);
                        var presult = Print532.InStance.PrintString(x?.PrintValue);
                        if (presult == 0)
                        {
                            Print532.InStance.PrintFeedSomeLines(1);
                            Console.WriteLine(x?.PrintValue);
                        }

                        else
                            Console.WriteLine($@"打印失败 {x?.PrintValue}");
                    }

                    new Print532().SelectCutPaper(66, 10);
                    Console.WriteLine("================================================");
                    Console.WriteLine("");
                    Console.WriteLine("");
                }
                else
                {
                    Console.WriteLine(entity.WINDOW_NAME);
                }
            }
            else
            {
                Send(JsonConvert.SerializeObject(new { code = 400, msg = "request is not a json string." }));
            }
        }

        protected override void OnClose(CloseEventArgs e)
        {
            Console.WriteLine("Connection Closed");
            base.OnClose(e);
        }

        protected override void OnError(ErrorEventArgs e)
        {
            Console.WriteLine("Error: " + e.Message);
            base.OnError(e);
        }

        private static bool TestJson(string json)
        {
            try
            {
                JToken.Parse(json);
                return true;
            }
            catch (JsonReaderException ex)
            {
                Console.WriteLine(ex);
                return false;
            }
        }
    }
}
