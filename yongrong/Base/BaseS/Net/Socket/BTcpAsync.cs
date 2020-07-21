using BaseS.File.Log;
using BaseS.Net.Socket.Model;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace BaseS.Net.Socket
{
    public static class BTcpAsync
    {
        /// <summary>
        /// 默认缓存大小
        /// </summary>
        const int Def_Data_Len = 8192;

        /// <summary>
        /// 开启异步链接
        /// </summary>
        /// <param name="tcp"></param>
        public static void BeginConnect(this TcpBean client)
        {
            if (null == client)
            {
                ("参数为空，不进行连接").Error();
                return;
            }

            // 默认采用tcp链接
            if (null == client.Tcp)
            {
                client.Tcp = new System.Net.Sockets.Socket(SocketType.Stream, ProtocolType.Tcp);

                // 本地地址非空,则需要绑定本地地址
                if (null != client.BindEP)
                {
                    try
                    {
                        client.Tcp.Bind(client.BindEP);
                    }
                    catch (SocketException se)
                    {
                        ("套接字Bind时出现错误!" + se.Message
                            + " SocketErrorCode:" + se.SocketErrorCode
                            + " 绑定地址:" + client.BindEP.ToString()
                            ).Warn();
                    }
                }
            }

            try
            {
                client.Tcp.BeginConnect(client.RemoteEP, EndConnect, client);
            }
            catch (SocketException se)
            {
                ("尝试访问套接字时出现错误!" + se.Message + " SocketErrorCode:" + se.SocketErrorCode).Warn();
            }
            catch (ObjectDisposedException oe)
            {
                ("Socket 已关闭!" + oe.Message).Warn();
            }
            catch (Exception e)
            {
                ("Unknown Exception:" + e.Message).Warn();
            }
            finally
            {
            }
        }

        /// <summary>
        /// 完成异步链接操作
        /// </summary>
        /// <param name="iar"></param>
        private static void EndConnect(IAsyncResult iar)
        {
            TcpBean bean = (TcpBean)iar.AsyncState;

            try
            {
                bean.Tcp.EndConnect(iar);

                bean.LocalEP = (IPEndPoint)bean.Tcp.LocalEndPoint;

                if (null != bean.BConnected)
                {
                    bean.BConnected(bean);
                }

                // 链接成功开始异步接收操作
                BeginReceive(bean);
            }
            catch (ArgumentOutOfRangeException ae)
            {
                ("ArgumentOutOfRangeException：" + ae.Message).Warn();
            }
            catch (ObjectDisposedException oe)
            {
                ("ObjectDisposedException：" + oe.Message).Warn();
            }
            catch (ArgumentException ae)
            {
                ("ArgumentException：" + ae.Message).Warn();
            }
            catch (InvalidOperationException ie)
            {
                ("SocketException：" + ie.Message).Warn();
            }
            catch (SocketException se)
            {
                ("SocketException：" + se.Message + " SocketErrorCode:" + se.SocketErrorCode).Warn();
            }
            catch (Exception e)
            {
                ("Unknown Exception：" + e.Message).Warn();
            }
            finally
            {
            }
        }

        /// <summary>
        /// 异步接收客户端socket请求
        /// </summary>
        /// <param name="lis"></param>
        public static void BeginAccept(this TcpBean lis)
        {
            try
            {
                lis.TcpServer.BeginAcceptSocket(new AsyncCallback(EndAccept), lis);
            }
            catch (SocketException se)
            {
                ("尝试访问套接字时出现错误!" + se.Message).Error();
            }
            catch (ObjectDisposedException oe)
            {
                ("Socket 已关闭!" + oe.Message).Error();
            }
            catch (Exception e)
            {
                ("Unknown Exception:" + e.Message).Error();
            }
        }

        /// <summary>
        /// 完成接收客户端链接
        /// </summary>
        /// <param name="iar"></param>
        private static void EndAccept(IAsyncResult iar)
        {
            TcpBean lis = (TcpBean)iar.AsyncState;

            TcpBean bean = new TcpBean()
            {
                IsSendLeft = false,
                Data = new byte[Def_Data_Len],
                Len = 0,
                Tcp = null,
                BClosed = lis.BClosed,
                BReceived = lis.BReceived,
                BSended = lis.BSended,
                LinkSeqId = -1, // 由系统自动产生 BAcepted中产生
                LocalEP = lis.LocalEP
            };

            try
            {
                bean.Tcp = lis.TcpServer.EndAcceptSocket(iar);
                bean.RemoteEP = (IPEndPoint)bean.Tcp.RemoteEndPoint;

                if (null != lis.BAcepted)
                {
                    lis.BAcepted(bean);
                }

                // 链接成功开始异步接收操作
                BeginReceive(bean);
            }
            catch (ArgumentOutOfRangeException ae)
            {
                ("ArgumentOutOfRangeException：" + ae.Message).Warn();
            }
            catch (ObjectDisposedException oe)
            {
                ("ObjectDisposedException：" + oe.Message).Warn();
            }
            catch (ArgumentException ae)
            {
                ("ArgumentException：" + ae.Message).Warn();
            }
            catch (InvalidOperationException ie)
            {
                ("InvalidOperationException：" + ie.Message).Warn();
            }
            catch (SocketException se)
            {
                ("SocketException：" + se.Message).Warn();
            }
            catch (Exception e)
            {
                ("Unknown Exception：" + e.Message).Warn();
            }
            finally
            {
                if (null != lis)
                {
                    // 继续接收客户端请求
                    BeginAccept(lis);
                }
            }
        }

        /// <summary>
        /// 开始接收socket数据
        /// </summary>
        /// <param name="c"></param>
        private static void BeginReceive(TcpBean bean)
        {
            if (null == bean || null == bean.Tcp)
            {
                return;
            }

            // 数据缓存为空情况下新开辟空间
            if (null == bean.Data)
            {
                bean.Data = new byte[Def_Data_Len];
            }

            try
            {
                bean.Tcp.BeginReceive(bean.Data, 0, bean.Data.Length, SocketFlags.None, EndReceive, bean);
            }
            catch (SocketException e)
            {
                ("SocketException:" + e.Message + " SocketErrorCode:" + e.SocketErrorCode).Warn();
                // 针对异常情况下关闭当前socket
                Close(bean);
            }
            catch (ArgumentNullException e)
            {
                ("ArgumentNullException:" + e.Message).Warn();

                Close(bean);
            }
            catch (Exception e)
            {
                ("Exception:" + e.Message).Warn();

                Close(bean);
            }
        }

        /// <summary>
        /// 完成接收socket数据
        /// </summary>
        /// <param name="iar"></param>
        private static void EndReceive(IAsyncResult iar)
        {
            TcpBean bean = (TcpBean)iar.AsyncState;

            try
            {
                bean.Len = bean.Tcp.EndReceive(iar, out SocketError errCode);

                if (SocketError.Success == errCode && 0 < bean.Len)
                {
                    if (null != bean.BReceived)
                    {
                        TcpBean newBean = new TcpBean()
                        {
                            Data = new byte[bean.Len],
                            Len = bean.Len,
                            LinkSeqId = bean.LinkSeqId,
                            LocalEP = bean.LocalEP,
                            RemoteEP = bean.RemoteEP,
                            Tcp = bean.Tcp,
                            BSended = bean.BSended,
                            BClosed = bean.BClosed
                        };

                        Buffer.BlockCopy(bean.Data, 0, newBean.Data, 0, bean.Len);

                        bean.BReceived?.Invoke(newBean);
                    }

                    BeginReceive(bean);
                }
                // 完成接收的数据长度为0 或者socket接收状态异常
                else
                {
                    Close(bean);
                }
            }
            catch (SocketException e)
            {
                ("SocketException:" + e.Message + " SocketErrorCode:" + e.SocketErrorCode).Warn();
            }
            catch (Exception e)
            {
                ("Exception:" + e.Message).Warn();
            }
        }

        /// <summary>
        /// 准备异步发送数据
        /// </summary>
        /// <param name="bean"></param>
        public static void BeginSend(this TcpBean bean)
        {
            if (null == bean || null == bean.Tcp || null == bean.Data || 0 >= bean.Data.Length)
            {
                return;
            }

            // 发送剩余部分数据
            if (bean.IsSendLeft)
            {
                ("开发发送（剩余包体） 地址：" + bean.RemoteEP.ToString() + " 长度：" + bean.Len.ToString()).Debug();
            }

            try
            {
                bean.Tcp.BeginSend(bean.Data, 0, bean.Len, SocketFlags.None, new AsyncCallback(EndSend), bean);
            }
            catch (SocketException e)
            {
                ("SocketErrorCode:" + e.SocketErrorCode + " SocketException:" + e.Message).Warn();

                Close(bean);
            }
            catch (Exception e)
            {
                ("Exception:" + e.Message).Warn();
                Close(bean);
            }
        }

        /// <summary>
        /// 完成发送数据
        /// </summary>
        /// <param name="iar"></param>
        private static void EndSend(IAsyncResult iar)
        {
            TcpBean bean = (TcpBean)iar.AsyncState;

            if (null == bean || null == bean.Tcp)
            {
                ("异常！ TcpBean 为空！或者内部参数为空").Warn();
                return;
            }

            try
            {
                int len = bean.Tcp.EndSend(iar);

                // 存在未发送完毕的数据
                if (bean.Len > len)
                {
                    TcpBean leftBean = new TcpBean();

                    leftBean.LinkSeqId = bean.LinkSeqId;
                    leftBean.IsSendLeft = true;
                    leftBean.RemoteEP = bean.RemoteEP;
                    leftBean.Tcp = bean.Tcp;
                    leftBean.Len = bean.Len - len;
                    leftBean.Data = new byte[leftBean.Len];

                    Buffer.BlockCopy(bean.Data, len, leftBean.Data, 0, leftBean.Len);

                    // 重新发送数据
                    BeginSend(leftBean);
                }

                // 通知协议层，当前包已经发送部分或者全部，可以释放
                if (null != bean.BSended
                    && !bean.IsSendLeft)
                {
                    bean.BSended(bean);
                }
            }
            // 发送存在异常情况下需要关闭链接,后续还得处理部分
            catch (SocketException e)
            {
                string addr = null != bean ? string.Empty : bean.RemoteEP.ToString();
                ("地址：" + addr + "SocketErrorCode:" + e.SocketErrorCode + " SocketException：" + e.Message).Warn();
                Close(bean);
            }
            catch (Exception e)
            {
                string addr = null != bean ? string.Empty : bean.RemoteEP.ToString();
                ("EndSend 地址：" + addr + " Exception：" + e.Message).Warn();
                Close(bean);
            }
        }

        /// <summary>
        /// 关闭socket链接
        /// </summary>
        public static void Close(this TcpBean bean)
        {
            if (null == bean)
            {
                return;
            }

            // 先通知业务层，该socket需要关闭
            bean.BClosed?.Invoke(bean);

            try
            {
                // 关闭双方的Socket接收和发送
                bean.Tcp.Shutdown(SocketShutdown.Both);
            }
            catch (SocketException e)
            {
                ("Shutdown SocketException:" + e.Message).Warn();
            }
            catch (ObjectDisposedException e)
            {
                ("Shutdown ObjectDisposedException:" + e.Message).Warn();
            }
            catch (Exception e)
            {
                ("Shutdown Exception:" + e.Message).Warn();
            }

            try
            {
                // 关闭socket 释放socket
                bean.Tcp.Close(30);
                bean.Tcp.Dispose();
            }
            catch (Exception e)
            {
                ("Close Exception:" + e.Message).Warn();
            }

            bean.Tcp = null;
            bean.Data = null;

            bean = null;
        }
    }
}
