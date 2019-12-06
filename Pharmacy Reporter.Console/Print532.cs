using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Runtime.InteropServices;
namespace PrintDemo
{
    /// <summary>
    /// 532打印机
    /// </summary>
    public class Print532
    {
        #region parameter
        public static Print532 InStance = new Print532();
        #endregion

        #region dll method
        /// <summary>
        /// 打开与主机通讯的打印机的串行端口或并口
        /// </summary>
        /// <param name="iPort">为主机与打印机通讯的端口，其值为：1～12，其中１～１０为串口端口号，11～12 对应为LPT1 和LPT2。</param>
        /// <param name="baud">设置主机与打印机通讯的波特率，其值为：2400～115200。默认值为9600。</param>
        /// <param name="hedshk">设置串口型打印机的握手形式，其值为0～3。默认值为0。
        /// 当取 0 时，打印机不握手，当取1 时打印机采用RTS/CTS 握手，当取2 时打印机
        /// 采用DTR/DSR 握手，当取3 时，打印机采用XON/XOFF 软件握手。</param>
        /// <returns>如果打开串口或并口打印机成功，则打印机将返回 0，如果打开失败，则返回-1。</returns>
        [DllImport("ThLPrinterDLL.dll", EntryPoint = "GcOpenPrinter")]
        private static extern Int32 GcOpenPrinter(Int32 iPort, Int32 baud, Int32 hedshk);
       
        /// <summary>
        /// 关闭与主机通讯的打印机的串行端口或并口
        /// </summary>
        /// <returns>如果关闭串口或并口打印机成功，则打印机将返回 0。</returns>
        [DllImport("ThLPrinterDLL.dll", EntryPoint = "GcClosePrinter")]
        private static extern Int32 GcClosePrinter();

        /// <summary>
        /// 发送打印内容到缓冲区（不是真正打印，只是在缓冲区内填充字符串）。
        /// </summary>
        /// <param name="printData">打印内容</param>
        /// <returns>如果函数执行成功，则将返回 0，如果发生任何错误，将返回-1。</returns>
        [DllImport("ThLPrinterDLL.dll", EntryPoint = "GcPrintString",CharSet=CharSet.Ansi)]
        private static extern Int32 GcPrintString(String printData);

        /// <summary>
        /// 打印缓冲区的内容并走纸一行，若缓冲区为空只向前走纸一行。
        /// </summary>
        /// <returns>如果函数执行成功，则将返回 0，否则将返回-１。</returns>
        [DllImport("ThLPrinterDLL.dll", EntryPoint = "GcPrintFeedLine")]
        private static extern Int32 GcPrintFeedLine();

        /// <summary>
        /// 打印缓冲区的内容，但不走纸。
        /// </summary>
        /// <returns>如果函数执行成功，则将返回 0，否则将返回-１。</returns>
        [DllImport("ThLPrinterDLL.dll", EntryPoint = "GcPrintEnter")]
        private static extern Int32 GcPrintEnter();
   
        /// <summary>
        /// 实时获取打印机的状态。
        /// </summary>
        /// <param name="iStatus"></param>
        /// <returns>函数将返回打印机的状态字节 iStatus，如果发生任何错误打印机都将返回-１。</returns>
        [DllImport("ThLPrinterDLL.dll", EntryPoint = "GcRealtimeGetStatus")]
        private static extern Int32 GcRealtimeGetStatus(Byte iStatus);

        /// <summary>
        /// 获取打印机的状态。
        /// </summary>
        /// <param name="iStatus"></param>
        /// <returns>函数将返回打印机的状态字节 iStatus，如果发生任何错误打印机都将返回-１。</returns>
        [DllImport("ThLPrinterDLL.dll", EntryPoint = "GcTransmitStatus")]
        private static extern Int32 GcTransmitStatus(Byte iStatus);

        /// <summary>
        /// 下装图形并执行打印
        /// </summary>
        /// <param name="filePath">为所要打印的位图的路径</param>
        /// <param name="m">取值范围为０～３或４８～５１。</param>
        /// <returns>函数执行成功将返回０，如果发生任何错误打印机都将返回-１。</returns>
        [DllImport("ThLPrinterDLL.dll", EntryPoint = "GcPrintDownloadBitImage",CharSet=CharSet.Ansi)]
        private static extern Int32 GcPrintDownloadBitImage(String filePath,Int32 m);

        /// <summary>
        /// 选择切纸方式及切纸送纸。
        /// </summary>
        /// <param name="mNum">取值范围为１或４９或６６</param>
        /// <param name="nNum">取值范围为０～２５５</param>
        /// <returns>函数执行成功将返回０，如果发生任何错误打印机都将返回-１。</returns>
        [DllImport("ThLPrinterDLL.dll", EntryPoint = "GcSelectCutPaper")]
        private static extern Int32 GcSelectCutPaper(Int32 mNum, Int32 nNum);

        /// <summary>
        /// 选择标准模式。
        /// </summary>
        /// <returns>函数执行成功将返回０，如果发生任何错误打印机都将返回-１</returns>
        [DllImport("ThLPrinterDLL.dll", EntryPoint = "GcSelectStardardMode")]
        private static extern Int32 GcSelectStardardMode();

        /// <summary>
        /// 设定字符大小
        /// </summary>
        /// <param name="W">设定放大字符宽度的倍数，取值范围为０～７，默认值为０</param>
        /// <param name="H">设定放大字符高度的倍数，取值范围为０～７，默认值为０</param>
        /// <returns></returns>
        [DllImport("ThLPrinterDLL.dll", EntryPoint = "GcSelectCharacterSize")]
        private static extern Int32 GcSelectCharacterSize(Int32 w, Int32 h);


        /// <summary>
        /// 打印走纸到指定行
        /// </summary>
        /// <param name="INum"></param>
        /// <returns></returns>
        [DllImport("ThLPrinterDLL.dll", EntryPoint = "GcPrintFeedSomeLines")]
        public static extern Int32 GcPrintFeedSomeLines(Int32 INum);

        /// <summary>
        /// 设定字符行间距
        /// </summary>
        /// <param name="iSpace"></param>
        /// <returns></returns>
        [DllImport("ThLPrinterDLL.dll", EntryPoint = "GcSetLineSpace")]
        public static extern Int32 GcSetLineSpace(Int32 iSpace);

        /// <summary>
        /// 打印条形码
        /// </summary>
        /// <param name="m">参数 m 的取值范围为０～８或６５～７５。如果m 取０～８参数n 将被忽略。</param>
        /// <param name="n">参数n的取值范围为１～２５５。</param>
        /// <param name="pszInfo">表示一串要打印成条形码的规定范围内的字符。</param>
        /// <returns></returns>
        [DllImport("ThLPrinterDLL.dll", EntryPoint = "GcPrintBarCode")]
        private static extern Int32 GcPrintBarCode(Int32 m, Int32 n, String pszInfo);

        #endregion

        #region method
        public Int32 OpenPrinter(Int32 iPort, Int32 baud, Int32 hedshk)
        {
            Int32 iResult=-1;
            try
            {
               iResult= GcOpenPrinter(iPort,baud,hedshk);
            }
            catch (Exception ex)
            {
                iResult = -1;
            }
            return iResult;
        }

        public Int32 ClosePrinter()
        {
            Int32 iResult = -1;
            try
            {
                iResult = GcClosePrinter();
            }
            catch (Exception ex)
            {
                iResult = -1;
            }
            return iResult;
        }

        public Int32 PrintString(String printData)
        {
            Int32 iResult = -1;
            try
            {
                iResult = GcPrintString(printData);
            }
            catch (Exception ex)
            {
                iResult = -1;
            }
            return iResult;
        }

        public Int32 PrintFeedLine()
        {
            Int32 iResult = -1;
            try
            {
                iResult = GcPrintFeedLine();
            }
            catch (Exception ex)
            {
                iResult = -1;
            }
            return iResult;
        }

        public Int32 PrintEnter()
        {
            Int32 iResult = -1;
            try
            {
                iResult = GcPrintEnter();
            }
            catch (Exception ex)
            {
                iResult = -1;
            }
            return iResult;
        }

        public Int32 RealtimeGetStatus(Byte iStatus)
        {
            Int32 iResult = -1;
            try
            {
                iResult = GcRealtimeGetStatus(iStatus);
            }
            catch (Exception ex)
            {
                iResult = -1;
            }
            return iResult;
        }

        public Int32 TransmitStatus(Byte iStatus)
        {
            Int32 iResult = -1;
            try
            {
                iResult = GcTransmitStatus(iStatus);
            }
            catch (Exception ex)
            {
                iResult = -1;
            }
            return iResult;
        }

        public Int32 PrintDownloadBitImage(String filePath,Int32 m)
        {
            Int32 iResult = -1;
            try
            {
                iResult = GcPrintDownloadBitImage(filePath, m);
            }
            catch (Exception ex)
            {
                iResult = -1;
            }
            return iResult;
        }

        public Int32 SelectCutPaper(Int32 mNum, Int32 nNum)
        {
            Int32 iResult = -1;
            try
            {
                iResult = GcSelectCutPaper(mNum, nNum);
            }
            catch (Exception ex)
            {
                iResult = -1;
            }
            return iResult;
        }

        public Int32 SelectCharacterSize(Int32 w,Int32 h)
        {
            Int32 iResult = -1;
            try
            {
                iResult = GcSelectCharacterSize(w,h);
            }
            catch (Exception ex)
            {
                iResult = -1;
            }
            return iResult;
        }

        public Int32 SelectStardardMode()
        {
            Int32 iResult = -1;
            try
            {
                iResult = GcSelectStardardMode();
            }
            catch (Exception ex)
            {
                iResult = -1;
            }
            return iResult;
        }

        public Int32 PrintFeedSomeLines(Int32 INum)
        {
            Int32 iResult = -1;
            try
            {
                iResult = GcPrintFeedSomeLines(INum);
            }
            catch (Exception ex)
            {
                iResult = -1;
            }
            return iResult;
        }

        public Int32 SetLineSpace(Int32 iSpace)
        {
            Int32 iResult = -1;
            try
            {
                iResult = GcSetLineSpace(iSpace);
            }
            catch (Exception ex)
            {
                iResult = -1;
            }
            return iResult;
        }


        //打印条形码
        public Int32 PrintBarCode(Int32 m, Int32 n, String pszInfo)
        {
            Int32 iResult = -1;
            try
            {
                iResult = GcPrintBarCode(m, n, pszInfo);
            }
            catch (Exception ex)
            {
                iResult = -1;
            }
            return iResult;
        }

        /// <summary>
        /// 检查532打印机
        /// </summary>
        /// <param name="port">端口号</param>
        /// <param name="baud">波特率</param>
        /// <returns>状态</returns>
        public int CheckState(string port, string baud)
        {
            Int32 iResult = 0;
            try
            {
                Int32 closeResult = ClosePrinter();
                iResult = OpenPrinter(Convert.ToInt32(port),Convert.ToInt32(baud),1);
                if (iResult==0)
                {
                    //机械故障
                    iResult=RealtimeGetStatus(3);
                   
                    String strResult = String.Empty;
                    if (iResult!=-1)
                    {
                        strResult = Convert.ToString(iResult, 2);
                        if (strResult.Length >=3)
                        {
                            if (strResult.Substring(strResult.Length - 1 - 2, 1) == "1")
                            {
                                iResult = -1;
                            }
                            else
                            {
                                iResult = 0;
                            }
                        }
                        if (strResult.Length >= 4)
                        {
                            if (strResult.Substring(strResult.Length - 1 - 3, 1) == "1")
                            {
                                iResult = -1;
                            }
                            else
                            {
                                iResult = 0;
                            }
                        }
                        if (strResult.Length >= 6)
                        {
                            if (strResult.Substring(strResult.Length - 1 - 3, 1) == "1")
                            {
                                iResult = -1;
                            }
                            else
                            {
                                iResult = 0;
                            }
                        }
                        if (strResult.Length >= 7)
                        {
                            if (strResult.Substring(strResult.Length - 1 - 3, 1) == "1")
                            {
                                iResult = -1;
                            }
                            else
                            {
                                iResult = 0;
                            }
                        }
                    }
                    //纸状态
                    iResult = RealtimeGetStatus(4);
                    if (iResult!=-1)
                    {
                        strResult = Convert.ToString(iResult,2);
                        if (strResult.Length>=7)
                        {
                            if (strResult.Substring(strResult.Length - 1 - 6, 1) == "1")
                            {
                                iResult = -1;
                            }
                            else
                            {
                                iResult = 0;
                            }
                        }
                    }
                }
                closeResult = ClosePrinter();
            }
            catch (Exception ex)
            {
                iResult = -1;
            }
            return iResult;
        }
        #endregion
    }
}
