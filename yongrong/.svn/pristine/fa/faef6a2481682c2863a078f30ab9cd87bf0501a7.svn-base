using BaseS.Net.Http;
using BaseS.Serialization;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ICcard
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
        /// 通过车牌号查询预约信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBoxTruckId_TextChanged(object sender, EventArgs e)
        {
            if(string.IsNullOrWhiteSpace(this.textBoxTruckId?.Text))
            {
                return;
            }

            Regex regex = new Regex(regTruck);

            if (!regex.IsMatch(this.textBoxTruckId.Text.ToUpper()))
            {
                return;
            }

            // 调用接口通过车牌号获取预约信息
            MessageBox.Show("准备获取预约信息");
        }

        /// <summary>
        /// 通过预约号查询预约信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBoxOrder_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(this.textBoxOrder?.Text))
            {
                return;
            }

            // 预约码为4位
            if (4 != this.textBoxOrder.Text.Trim().Length)
            {
                return;
            }

            // 调用接口通过预约号获取预约信息
            MessageBox.Show("准备获取预约信息");
        }

        /// <summary>
        /// 
        /// </summary>
        private static bool GetICCard(out int cardId, out string cardData)
        {
            cardId = -1;
            cardData = string.Empty;

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
            authmode = 1;//>0 with A，default A

            //the Key
            mypicckey[0] = 0xff;
            mypicckey[1] = 0xff;
            mypicckey[2] = 0xff;
            mypicckey[3] = 0xff;
            mypicckey[4] = 0xff;
            mypicckey[5] = 0xff;

            status = piccreadex(myctrlword, mypiccserial, myareano, authmode, mypicckey, mypiccdata);

            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_read_Click(object sender, EventArgs e)
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

        private void buttonwrite_Click(object sender, EventArgs e)
        {
            byte i;
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

            //指定卡数据
            for (i = 0; i < 48; i++)
            {
                mypiccdata[i] = i;
            }

            status = piccwriteex(myctrlword, mypiccserial, myareano, authmode, mypicckey, mypiccdata);
            //在下面设定断点，然后查看mypiccserial、mypiccdata，
            //调用完 piccreadex函数可读出卡序列号到 mypiccserial，读出卡数据到mypiccdata，
            //开发人员根据自己的需要处理mypiccserial、mypiccdata 中的数据了。
            //处理返回函数
            switch (status)
            {
                case 0:
                    MessageBox.Show("操作成功,mypiccdata数组中的数据已写入卡中");
                    break;
                //......
                case 8:
                    MessageBox.Show("请将卡放在感应区");
                    break;

                default:
                    MessageBox.Show("返回码(对应的说明请看例子中的注释):" + status);
                    break;

            }
        }

        /// <summary>
        /// 调用Http服务端
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

                if ((7 > tractor.Length  || 8 < tractor.Length))
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

            var http = new BaseS.Net.Http.Bean.HttpReqBean()
            {
                Url = Settings1.Default.ServerUrl,
                Param = "{" + $"\"Order\":\"{order}\",\"Tractor\":\"{tractor}\"" + "}"
            };

            BHttp.PostSync(http);

            if (!http.Stat)
            {
                MessageBox.Show("网络问题调用失败");
                return;
            }

            BJson.Json2ObjT<ICQueryOrderRsp>(http.HttpRsp, out ICQueryOrderRsp rsp);

            if(null == rsp)
            {
                MessageBox.Show("调用失败,联系开发人员");
                return;
            }

            if(null ==  rsp.Data)
            {
                MessageBox.Show("预约不存在");
                return;
            }

            ordergoods = rsp.Data;

            textBoxDetail.Text = ordergoods.ToString();

            buttonWriteCard.Enabled = true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
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

            writestr = ordergoods.Orderid;
            mypiccdata = System.Text.Encoding.ASCII.GetBytes(writestr);

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

        private void textBoxDetail_TextChanged(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void textBoxOrder_TextChanged_1(object sender, EventArgs e)
        {

        }
    }
}
