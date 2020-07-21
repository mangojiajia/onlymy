using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace BaseS.Net.Socket.Model
{
    public class SocketBean
    {
        /// <summary>
        /// 服务端模式：对端地址IP+端口
        /// 客户端模式：本地IP+端口
        /// </summary>
        public IPEndPoint RemoteEP;

        /// <summary>
        /// 本地地址
        /// </summary>
        public IPEndPoint LocalEP;

        /// <summary>
        /// 绑定本地地址
        /// </summary>
        public IPEndPoint BindEP;

        /// <summary>
        /// 总字节
        /// </summary>
        public byte[] Data;

        /// <summary>
        /// 有效字节长度
        /// </summary>
        public int Len;
    }
}
