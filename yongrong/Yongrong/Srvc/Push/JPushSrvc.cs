﻿using Jiguang.JPush;
using Jiguang.JPush.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BaseS.File.Log;

namespace Yongrong.Srvc.Push
{
    public class JPushSrvc : BaseSrvc
    {
        /// <summary>
        /// 
        /// </summary>
        private static JPushClient client = new JPushClient(Sys.PushAppKey, Sys.PushMastSecret);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static bool SendPush(string msg, IEnumerable<string> useridList)
        {
            if (null == useridList || 0 == useridList.Count())
            {
                return false;
            }

            PushPayload pushPayload = new PushPayload()
            {
                Platform = new List<string> { "android", "ios" },
                Audience = new Audience { Alias = useridList.ToList() },
                Notification = new Notification
                {
                    Alert = msg,
                    Android = new Android
                    {
                        Alert = msg,
                        Title = "预约审核"
                    },
                    IOS = new IOS
                    {
                        Alert = msg,
                        Badge = "+1"
                    }
                },
                Message = new Message
                {
                    Title = "预约审核",
                    Content = msg,
                    Extras = new Dictionary<string, string>
                    {
                        ["key1"] = "value1"
                    }
                },
                Options = new Options
                {
                    IsApnsProduction = false ,// 设置 iOS 推送生产环境。不设置默认为开发环境。
                    TimeToLive= 86400
                }
            };

            var response = client.SendPush(pushPayload);
            response.Content.Debug();

            return true;
        }
    }
}
