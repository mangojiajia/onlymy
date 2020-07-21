using ICcard32.Base.Http;
using ICcard32.Base.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ICcard32
{
    public partial class Form1 : Form
    {
        static string regTruck = @"^(([京津沪渝冀豫云辽黑湘皖鲁新苏浙赣鄂桂甘晋蒙陕吉闽贵粤青藏川宁琼使领][A-Z](([0-9]{5}[DF])|([DF]([A-HJ-NP-Z0-9])[0-9]{4})))|([京津沪渝冀豫云辽黑湘皖鲁新苏浙赣鄂桂甘晋蒙陕吉闽贵粤青藏川宁琼使领][A-Z][A-HJ-NP-Z0-9]{4}[A-HJ-NP-Z0-9挂学警港澳使领]))$";

        /// <summary>
        /// 
        /// </summary>
        static OrderGoods ordergoods = null;

        public Form1()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_Valid_Click(object sender, EventArgs e)
        {
            var order = textBoxOrder.Text;
            var tractor = textBoxTruckId.Text;

            if (!string.IsNullOrWhiteSpace(tractor))
            {
                tractor = tractor.Replace("-", "").Replace("－", "");

                if ((7 > tractor.Length || 8 < tractor.Length))
                {
                    tractor = string.Empty;

                    if (string.IsNullOrWhiteSpace(order))
                    {
                        MessageBox.Show("车牌号码无效");
                        return;
                    }
                }
            }

            if (string.IsNullOrWhiteSpace(tractor) && string.IsNullOrWhiteSpace(order))
            {
                MessageBox.Show("输入信息无效,请重新输入");
                return;
            }

            var http = new HttpReqBean()
            {
                Url = Settings1.Default.ServerUrl,
                Param = "{" + $"\"Order\":\"{order}\",\"Tractor\":\"{tractor}\"" + "}"
            };

            BHttp.PostSync_WebClient(http);

            if (!http.Stat)
            {
                MessageBox.Show("网络问题调用失败");
                return;
            }

            Json.ToObj<ICQueryOrderRsp>(http.HttpRspBytes, out ICQueryOrderRsp rsp);

            if (null == rsp)
            {
                MessageBox.Show("调用失败,联系开发人员");
                return;
            }

            if (null == rsp.Data)
            {
                MessageBox.Show("预约不存在");
                return;
            }

            ordergoods = rsp.Data;

            textBoxDetail.Text = ordergoods.ToString();

            buttonWriteCard.Enabled = true;
        }

        private void buttonWriteCard_Click(object sender, EventArgs e)
        {
            byte i;
            string writestr;
            byte status;//Store the return value
            byte myareano;//sector
            byte authmode;//authentication Key type, with A Key or B Key
            byte myctrlword;//Control word
            byte[] mypicckey = new byte[6];//Key
            byte[] mypiccserial = new byte[4];//Card serial number
            byte[] mypiccdata = new byte[48]; //Card data buffer
            //control word please check our company web site provides dynamic libraries

            myctrlword = BLOCK0_EN + BLOCK1_EN + BLOCK2_EN + EXTERNKEY;

            //sector
            myareano = 8;//sector is 8
            //authentication key Type
            authmode = 1;//>0 with A Key，advice A Key

            //Key
            mypicckey[0] = 0xff;
            mypicckey[1] = 0xff;
            mypicckey[2] = 0xff;
            mypicckey[3] = 0xff;
            mypicckey[4] = 0xff;
            mypicckey[5] = 0xff;

            //Write Card Buff
            for (i = 0; i < 48; i++)
            {
                mypiccdata[i] = 0;
            }

            Json.ToString<OrderGoods>(ordergoods, out var str1);

            writestr = str1;

            mypiccdata = System.Text.Encoding.UTF8.GetBytes(writestr);

            status = piccwriteex(myctrlword, mypiccserial, myareano, authmode, mypicckey, mypiccdata);
            //In the following set breakpoints, and then look at it  " mypiccserial、mypiccdata"
            //call piccreadex function can read ic serialNo to " mypiccserial"，card infomation into "mypiccdata"
            //Developers based on their own to deal with  " mypiccserial、mypiccdata" 
            switch (status)
            {
                case 0:
                    pcdbeep(38);
                    MessageBox.Show("信息写入成功");
                    //MessageBox.Show("Operation is successful, mypiccdata arrays of data has been written to the card", "Note:", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    break;
                //......
                case 8:
                    MessageBox.Show("Please put the card on the induction area", "Note:", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    break;

                default:
                    MessageBox.Show("Eooro Code:" + status, "warn:", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    break;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBoxDetail_DoubleClick(object sender, EventArgs e)
        {
            byte status;//存放返回值
            byte myareano;//区号
            byte authmode;//密码类型，用A密码或B密码
            byte myctrlword;//控制字
            byte[] mypicckey = new byte[6];//密码
            byte[] mypiccserial = new byte[4];//卡序列号
            byte[] mypiccdata = new byte[48]; //卡数据缓冲
            //控制字指定,控制字的含义请查看本公司网站提供的动态库说明
            myctrlword = BLOCK0_EN + BLOCK1_EN + BLOCK2_EN + EXTERNKEY;

            //指定区号
            myareano = 8;//指定为第8区
            //批定密码模式
            authmode = 1;//大于0表示用A密码认证，推荐用A密码认证

            //指定密码
            mypicckey[0] = 0xff;
            mypicckey[1] = 0xff;
            mypicckey[2] = 0xff;
            mypicckey[3] = 0xff;
            mypicckey[4] = 0xff;
            mypicckey[5] = 0xff;

            status = piccreadex(myctrlword, mypiccserial, myareano, authmode, mypicckey, mypiccdata);
            //在下面设定断点，然后查看mypiccserial、mypiccdata，
            //调用完 piccreadex函数可读出卡序列号到 mypiccserial，读出卡数据到mypiccdata，
            //开发人员根据自己的需要处理mypiccserial、mypiccdata 中的数据了。


            //处理返回函数
            switch (status)
            {
                case 0:
                    //MessageBox.Show("操作成功,数据已返回在mypiccdata数组中");
                    MessageBox.Show($"卡内信息:{Encoding.UTF8.GetString(mypiccdata)}");
                    //textBoxDetail.Text += $"读取到的卡号:{Encoding.UTF8.GetString(mypiccserial)} 内部信息:{Encoding.UTF8.GetString(mypiccdata)}\n";
                    break;
                //......
                case 8:
                    MessageBox.Show("请将卡放在感应区");
                    break;
                default:
                    MessageBox.Show("返回码(对应的说明请看例子中的注释):" + GetErrStat(status));
                    break;
            }
        }
    }
}
