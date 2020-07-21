using System;
using System.Collections.Generic;
using System.Text;

namespace Yongrong.Model.Conf
{
    public class HttpConf
    {
        /// <summary>
        /// 开启http标志
        /// </summary>
        public bool HttpFlag { get; set; }

        /// <summary>
        /// http侦听端口
        /// </summary>
        public List<int> Ports { get; set; }

        /// <summary>
        /// nginx路由的ip地址路由参数名称
        /// </summary>
        public string NginxIpRoute { get; set; }

        /// <summary>
        /// 客户端ip参数名称
        /// </summary>
        public string ClientIPName { get; set; }

        /// <summary>
        /// 命令字名称
        /// </summary>
        public string CmdName { get; set; } = "cmd";
    }
}
