using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;

namespace ICcard
{
    public partial class Form1
    {
        //Constants defined
        public const byte BLOCK0_EN = 0x01;//Operating 0 blocks
        public const byte BLOCK1_EN = 0x02;//Operating 1 blocks
        public const byte BLOCK2_EN = 0x04;//Operating 2 blocks
        public const byte NEEDSERIAL = 0x08;//Only the specified serial number card operation
        public const byte EXTERNKEY = 0x10;
        public const byte NEEDHALT = 0x20;//Read or write CARDS after dormancy card, by the way, after dormancy, leave induction card must take, return to active area, to carry out the second operation。

        //------------------------------------------------------------------------------------------------------------------------------------------------------
        //External function declaration: make sound equipment
        [DllImport("OUR_MIFARE.dll", EntryPoint = "pcdbeep", CallingConvention = CallingConvention.StdCall)]
        static extern byte pcdbeep(UInt32 xms);//xms  milliseconds 


        //------------------------------------------------------------------------------------------------------------------------------------------------------    
        //Read-only card number
        [DllImport("OUR_MIFARE.dll", EntryPoint = "piccrequest", CallingConvention = CallingConvention.StdCall)]
        public static extern byte piccrequest(byte[] serial);//devicenumber 

        //------------------------------------------------------------------------------------------------------------------------------------------------------    
        //Read the device number, can be used as a software encryption dog, can also according to the number on the company website query warranty period
        [DllImport("OUR_MIFARE.dll", EntryPoint = "pcdgetdevicenumber", CallingConvention = CallingConvention.StdCall)]
        static extern byte pcdgetdevicenumber(byte[] devicenumber);//devicenumber


        //------------------------------------------------------------------------------------------------------------------------------------------------------
        //Easy to read
        [DllImport("OUR_MIFARE.dll", EntryPoint = "piccreadex", CallingConvention = CallingConvention.StdCall)]
        static extern byte piccreadex(byte ctrlword, byte[] serial, byte area, byte keyA1B0, byte[] picckey, byte[] piccdata0_2);
        //parameters:
        //ctrlword：
        //serial：Card serial number array, is used to specify or return card serial number
        //area：Specifies read card code
        //keyA1B0：Specified with A or B Key authentication, usually with A Key, only under the special purpose, with B Key in this to do A detailed explanation。
        //picckey：Specified card Key, 6 bytes, initial Key for the card when they leave is  6个0xff
        //piccdata0_2：Used to return to the card the 0 to 2 pieces of data, a total of 48 bytes.


        //------------------------------------------------------------------------------------------------------------------------------------------------------
        //Easy to write
        [DllImport("OUR_MIFARE.dll", EntryPoint = "piccwriteex", CallingConvention = CallingConvention.StdCall)]
        static extern byte piccwriteex(byte ctrlword, byte[] serial, byte area, byte keyA1B0, byte[] picckey, byte[] piccdata0_2);
        //parameters:
        //ctrlword：
        //serial：Card serial number array, is used to specify or return card serial number
        //area：Specifies read card code
        //keyA1B0：Specified with A or B Key authentication, usually with A Key, only under the special purpose, with B Key in this to do A detailed explanation。
        //picckey：Specified card Key, 6 bytes, initial Key for the card when they leave is  6个0xff
        //piccdata0_2：Used to return to the card the 0 to 2 pieces of data, a total of 48 bytes.


        //------------------------------------------------------------------------------------------------------------------------------------------------------
        //The Key change card list
        [DllImport("OUR_MIFARE.dll", EntryPoint = "piccchangesinglekey", CallingConvention = CallingConvention.StdCall)]
        static extern byte piccchangesinglekey(byte ctrlword, byte[] serial, byte area, byte keyA1B0, byte[] piccoldkey, byte[] piccnewkey);
        //parameters:
        //ctrlword：
        //serial：Card serial number array, is used to specify or return card serial number
        //area：Specifies read card code
        //keyA1B0：Specified with A or B Key authentication, usually with A Key, only under the special purpose, with B Key in this to do A detailed explanation。
        //piccoldkey：//Old Key
        //piccnewkey：//New Key.


        //------------------------------------------------------------------------------------------------------------------------------------------------------
        //Send Display Information to Drive
        [DllImport("OUR_MIFARE.dll", EntryPoint = "lcddispfull", CallingConvention = CallingConvention.StdCall)]
        static extern byte lcddispfull(string lcdstr);
        //parameters:
        //lcdstr：According to the content

        /// <summary>
        /// 
        /// </summary>
        static Dictionary<byte, string> errstatTab = new Dictionary<byte, string>();

        /// <summary>
        /// 
        /// </summary>
        private static void InitErrStatTab()
        {
            if (null == errstatTab)
            {
                errstatTab = new Dictionary<byte, string>();
            }

            errstatTab.Add(8, "寻卡错误");
            errstatTab.Add(9, "读序列码错误");
            errstatTab.Add(10, "选卡错误");
            errstatTab.Add(11, "装载密码错误");
            errstatTab.Add(12, "密码认证错误");
            errstatTab.Add(13, "读卡错误");
            errstatTab.Add(14, "写卡错误");

            errstatTab.Add(21, "没有动态库");
            errstatTab.Add(22, "动态库或驱动程序异常");
            errstatTab.Add(23, "驱动程序错误或尚未安装");
            errstatTab.Add(24, "操作超时，一般是动态库没有反映");
            errstatTab.Add(25, "发送字数不够");
            errstatTab.Add(26, "发送的CRC错");
            errstatTab.Add(27, "接收的字数不够");
            errstatTab.Add(28, "接收的CRC错");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="stat"></param>
        /// <returns></returns>
        public static string GetErrStat(byte stat)
        {
            if (0 == errstatTab.Count)
            {
                InitErrStatTab();
            }

            errstatTab.TryGetValue(stat, out var val);

            return val;
        }
    }
}
