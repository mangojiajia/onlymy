﻿using System;
using System.Collections.Generic;
using System.Text;
using Yongrong.Model.Int.Sum;
using BaseS.String;
using BaseS.Serialization;
using Yongrong.Srvc.Users;
using Yongrong.Srvc.Gate;
using System.Data;
using BaseS.Data;
using BaseS.File;
using System.IO;
using BaseS.Collection;
using BaseS.Net.Http;
using System.Web;
using System.Runtime.InteropServices;

namespace Yongrong.Int.Sum
{
    class GateInt : BaseInt
    {
        /// <summary>
        /// 
        /// </summary>
        static readonly Dictionary<string, string> contractMap = new Dictionary<string, string>();

        /// <summary>
        /// 导出模板字段对应
        /// </summary>
        static GateInt()
        {
            contractMap.Add("Plateno", "车牌号");
            contractMap.Add("Eventtypename", "进出场类型");
            contractMap.Add("Roadwayname", "门禁出入口");
            contractMap.Add("Starttime", "进出场时间");
            contractMap.Add("Eventcmd", "旅行类型");
            contractMap.Add("Gatename", "门禁名称");
        }

        /// <summary>
        /// Pda 登录请求
        /// </summary>
        /// <param name="reqmap"></param>
        /// <param name="reqbytes"></param>
        /// <returns></returns>
        public static byte[] Get(Dictionary<string, string> reqmap, byte[] reqbytes)
        {
            GateGetRsp rsp = new GateGetRsp() { Stat = Success};

            reqbytes.Json2ObjT<GateGetReq>(out GateGetReq req);

            if (null == req)
            {
                rsp.Stat = JsonErr;

                reqmap.TryAdd(OP_Content, rsp.Stat);
                reqmap.TryAdd(OP_Detail, reqbytes.B2String());

                return rsp.ToBytes();
            }

            if (!Success.Equals(req.Check()))
            {
                rsp.Stat = req.Check();

                reqmap.TryAdd(OP_Content, "参数校验失败");
                reqmap.TryAdd(OP_Detail, reqbytes.B2String());

                return rsp.ToBytes();
            }

            if (!UserSrvc.GetOne(new Model.Db.User() { Token = req.Token }, out var user))
            {
                rsp.Stat = "用户不存在";

                reqmap.TryAdd(OP_Content, "用户不存在");
                reqmap.TryAdd(OP_Detail, "Token:" + req.Token);

                return rsp.ToBytes();
            }

            ApiGateSrvc.Get(req.Query, req.Page, out var data);

            rsp.List = data;
            req.Page.CopyPage(rsp.Page);

            reqmap.TryAdd(OP_User, user?.Userid);
            reqmap.TryAdd(OP_Content, $"查询门禁记录");

            return rsp.ToBytes();
        }

        /// <summary>
        /// 导出
        /// </summary>
        /// <param name="reqmap"></param>
        /// <param name="reqbytes"></param>
        /// <returns></returns>
        public static byte[] Output(Dictionary<string, string> reqmap, byte[] reqbytes)
        {
            OutputRsp rsp = new OutputRsp() { Stat = Success };

            string Token = reqmap.GetDicStr("Token");
            string Starttime = reqmap.GetDicStr("Starttime");
            string Stoptime = reqmap.GetDicStr("Stoptime");

            if (!UserSrvc.GetOne(new Model.Db.User() { Token = Token }, out var user))
            {
                rsp.Stat = "用户不存在";

                reqmap.TryAdd(OP_Content, "用户不存在");
                reqmap.TryAdd(OP_Detail, "Token:" + Token);

                return rsp.ToBytes();
            }
            reqmap.TryAdd(OP_User, user?.Userid);
            reqmap.TryAdd(OP_Content, $"导出门禁记录");

            ApiGateSrvc.Get(new Model.Db.ApiGateevent() { Starttime = Starttime, Stoptime = Stoptime }, new Model.Int.PageBean() { Index = 1, Size = 10000 }, out var list);

            if(null != list && list.Count > 0)
            {
                DataTable dt = list.ToDataTable();
                
                BCsv.DataTable2CsvString(dt, out string dataString, contractMap);

                reqmap.TryAdd(BHttpBean.ContentDisposition, $"attachment; filename= { HttpUtility.UrlEncode("门禁通行记录汇总.csv")}");
                reqmap.TryAdd(BHttpBean.ContentType, "application/octet-stream;charset=utf-8");
                
                byte[] emptyfileBytes = new byte[] { 0xEF, 0xBB, 0xBF};
                byte[] total = null;
                byte[] body = Encoding.UTF8.GetBytes(dataString);
                //BFile.Txt2Bytes(Sys.ExcelTemplet, ref emptyfileBytes, Encoding.Default);

                if (null != emptyfileBytes)
                {
                    total = new byte[emptyfileBytes.Length + body.Length];
                    Buffer.BlockCopy(emptyfileBytes, 0, total, 0, emptyfileBytes.Length);
                    Buffer.BlockCopy(body, 0, total, emptyfileBytes.Length, body.Length);
                }
                else
                {
                    total = body;
                }

                return total;
            }
            return rsp.ToBytes();
        }
    }
}
