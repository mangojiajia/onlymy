using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace BaseS.Net.Socket.Model
{
   public class TcpBean : SocketBean
    {
        /// <summary>
        /// 链接序号
        /// </summary>
        public int LinkSeqId;

        /// <summary>
        /// tcp侦听服务对象
        /// </summary>
        public TcpListener TcpServer;

        /// <summary>
        /// 套接字
        /// </summary>
        public System.Net.Sockets.Socket Tcp;

        /// <summary>
        /// 发送余下包标志
        /// </summary>
        public bool IsSendLeft = false;

        #region 事件部分
        public Action<TcpBean> BConnected;
        public Action<TcpBean> BAcepted;
        public Action<TcpBean> BReceived;
        public Action<TcpBean> BSended;
        public Action<TcpBean> BClosed;
        #endregion
    }
}
