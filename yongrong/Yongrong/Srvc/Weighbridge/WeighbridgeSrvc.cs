using System;
using System.Collections.Generic;
using System.Text;
using BaseS.Net.Http;
using BaseS.Net.Http.Bean;
using BaseS.Security;
using BaseS.Serialization;
using Yongrong.Model.Int.Weigh;
using BaseS.File.Log;
using System.Linq;
using Yongrong.Model.Db;
using System.Web;
using BaseS.String;

namespace Yongrong.Srvc.Weighbridge
{
    public class WeighbridgeSrvc : BaseSrvc
    {
        /// <summary>
        /// 当前存在的对象
        /// </summary>
        private static WeighLoginRsp weighbridge = new WeighLoginRsp();

        /// <summary>
        /// 接口:登录地磅系统
        /// </summary>
        /// <returns></returns>
        private static bool Login()
        {
            HttpReqBean http = new HttpReqBean()
            {
                ContentType = "application/json; charset=utf-8",
                Url = $"{Weigh.ServUrl}/user/login?db_name={Weigh.Db_name}&user_name={Weigh.User_name}&version={Weigh.Version}&password={Weigh.Passwd.ToMD5String(32).ToLower()}",
                Param = ""
            };

            BHttp.PostSync(http);

            if (!http.Stat)
            {
                return false;
            }

            http.HttpRspBytes.Json2ObjT<WeighLoginRsp>(out WeighLoginRsp rsp);

            if (null == rsp)
            {
                $"调用登录方法失败!解析应答失败!".Notice();
                return false;
            }

            if (0 != rsp.Status)
            {
                $"调用登录方法失败!错误码:{rsp.Status} - {rsp.Message}".Notice();
                return false;
            }

            weighbridge.Token = rsp.Token;
            weighbridge.ExpireTime = rsp.ExpireTime;

            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public static bool BatchInsert(DataParamExt data)
        {
            // 无效状态下发起登录请求
            if(!IsValidStat())
            {
                if(!Login())
                {
                    return false;
                }
            }

            // 组建对象
            var tData = new DataT()
            {
                DbName = Weigh.InsertDbName,
                SqlCmdId = Weigh.InsertSqlCmdId,
                Params = data.ToParamsFormat()
            };

            tData.Obj2JsonT<DataT>(out string reqjson);

            HttpReqBean http = new HttpReqBean()
            {
                ContentType = "application/json;charset=UTF-8",
                Url = $"{Weigh.ServUrl}/db/execsql",
                Param = $"token={weighbridge.Token}&data={reqjson.Str2Url()}"
            };

            BHttp.PostSync(http);

            if (!http.Stat)
            {
                $"http提交失败,应答:{http.HttpRsp} 错误:{http.HttpErr}".Notice();
                return false;
            }

            return true;
        }

        /// <summary>
        /// 验证状态是否合法
        /// </summary>
        /// <returns></returns>
        private static bool IsValidStat()
        {
            if (null == weighbridge)
            {
                return false;
            }

            if (string.IsNullOrWhiteSpace(weighbridge.Token))
            {
                return false;
            }

            if (0 <= DateTime.Now.GetDateTimeFormats('s')[0].ToString().CompareTo(weighbridge.ExpireTime))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 登出
        /// </summary>
        /// <returns></returns>
        public static bool Logout(bool force = false)
        {
            if (!force && !IsValidStat())
            {
                "当前的token已经失效,不需要注销".Notice();
                return true;
            }

            WeighLogoutReq req = new WeighLogoutReq() { Token = weighbridge.Token };

            req.Obj2JsonT<WeighLogoutReq>(out string reqjson);

            HttpReqBean http = new HttpReqBean()
            {
                ContentType = "application/json; charset=utf-8",
                Url = $"{Weigh.ServUrl}/user/logout",
                Param = reqjson
            };

            BHttp.PostSync(http);

            if (!http.Stat)
            {
                return false;
            }

            $"提交注销请求,应答:{http.HttpRsp} 错误:{http.HttpErr}".Tip();

            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static bool AddSync(ApiWeigh api)
        {
            using(var db =DbContext)
            {
                api.Id = GetSeq("SEQ_API_WEIGH");

                api.Createtime = DateTime.Now;

                db.ApiWeigh.Add(api);

                db.SaveChanges();
            }

            return true;
        }
    }
}
