using BaseS.File.Log;
using System;
using System.Text;
using System.Threading;
using Yongrong.Ctrl;

namespace Yongrong
{
    class Program
    {
        static void Main(string[] args)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            BLog.Start();

            MainCtrl.Start();

            CmdCtrl.MonitorCmd();

            MainCtrl.Stop();

            Thread.Sleep(700);

            BLog.Stop();

            Environment.Exit(0);
        }
    }
}
