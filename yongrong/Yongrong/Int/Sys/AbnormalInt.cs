﻿using System;
using System.Collections.Generic;
using System.Text;
using Yongrong.Model.Int.Sys;
using BaseS.Serialization;
using BaseS.String;
using Yongrong.Srvc.Users;
using Yongrong.Srvc.Sys;
using Yongrong.Model.Db;

namespace Yongrong.Int.Sys
{
    class AbnormalInt : BaseInt
    {
        /// <summary>
        /// 查询异常数据
        /// </summary>
        /// <param name="reqmap"></param>
        /// <param name="reqbytes"></param>
        /// <returns></returns>
        public static byte[] Get(Dictionary<string, string> reqmap, byte[] reqbytes)
        {
            AbnormalGetRsp rsp = new AbnormalGetRsp();

            reqbytes.Json2ObjT<AbnormalGetReq>(out AbnormalGetReq req);

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

                reqmap.TryAdd(OP_Content, rsp.Stat);
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
        
            AbnormalSrvc.Get(
                req.Query,
                req.Page,
                out var abnormallist);

            if (null == req.Page)
            {
                rsp.Page = new Model.Int.PageBean();
            }
            else
            {
                req.Page.CopyPage(rsp.Page);
            }

            rsp.Stat = Success;
            rsp.List = abnormallist;

            reqmap.TryAdd(OP_User, user?.Userid);
            reqmap.TryAdd(OP_Content, $"查询异常数据");

            return rsp.ToBytes();
        }

    }
}
