using BaseS.File.Log;
using BaseS.String;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.WebSockets;
using System.Text;
using System.Threading;

namespace BaseS.Net.Websock
{
    public class WsBean
    {
        public int Seq { get; set; }

        public WebSocket Wsocket { get; set; }

        public HttpListenerContext Context { get; set; }

        public string RemoteIp { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="wsseq"></param>
        /// <param name="rspbytes"></param>
        public async void SendBytes(byte[] rspbytes, WebSocketMessageType messageType = WebSocketMessageType.Binary)
        {
            ArraySegment<byte> sendbuff = new ArraySegment<byte>(rspbytes);

            try
            {
                await Wsocket.SendAsync(sendbuff, messageType, true, CancellationToken.None);
                //rspbytes.B2String().Tip("", "应答内容");
            }
            catch (Exception e)
            {
                //rspbytes.B2String().Warn("", "应答内容");
                $"Err:{e.Message} StackTrace:{e.StackTrace}".Warn();
                return;
            }
        }
    }
}
