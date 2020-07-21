﻿using System;
using System.Collections.Generic;
using System.Text;
using Yongrong.Model.Int.BaseInfo;
using Yongrong.Srvc.BaseInfo;
using BaseS.Serialization;
using BaseS.String;
using Yongrong.Srvc.Users;
using BaseS.File.Log;

namespace Yongrong.Int.BaseInfo
{
    class OrderGoodsOutInt : BaseInt
    {
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="reqmap"></param>
        /// <param name="reqbytes"></param>
        /// <returns></returns>
        public static byte[] Get(Dictionary<string, string> reqmap, byte[] reqbytes)
        {
            OrderGoodsOutRsp rsp = new OrderGoodsOutRsp();
            reqbytes.Json2ObjT<OrderGoodsOutReq>(out OrderGoodsOutReq req);
            //进日志
            req.Debug(string.Empty, Enter);
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

            if (Sys.CustomRole.Contains(user.Roleids))
            {
                req.Query.Company = user.Userid;
            }

            //req.Query.Issendback = "2";
            //req.Query.Istoexit = "1";

            OrderGoodsSrvc.Get(
                req.Query,
                req.Page,
                out var tmp);

            if (null == req.Page)
            {
                rsp.Page = new Model.Int.PageBean();
            }
            else
            {
                req.Page.CopyPage(rsp.Page);
            }

            SafeCheckSrvc.UpdOrderGoodsSafeCheck(tmp);

            rsp.Stat = Success;
            rsp.List = tmp;
            reqmap.TryAdd(OP_User, user?.Userid);
            reqmap.TryAdd(OP_Content, "查询产品出厂预约单");

            return rsp.ToBytes();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="reqmap"></param>
        /// <param name="reqbytes"></param>
        /// <returns></returns>
        public static byte[] AddUpd(Dictionary<string, string> reqmap, byte[] reqbytes)
        {
            OrderGoodsOutAddRsp rsp = new OrderGoodsOutAddRsp();

            reqbytes.Json2ObjT<OrderGoodsOutAddReq>(out OrderGoodsOutAddReq req);
            //进日志
            req.Debug(string.Empty, Enter);
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

            if (!req.Op.Equals("del"))
            {
                req.OrderGoodsOut.Issendback = "2";
                req.OrderGoodsOut.Istoexit = "1";
            }

            rsp.Stat = OrderGoodsSrvc.AddOrUpdate(req.OrderGoodsOut, req.Op, user.Roleids);

            reqmap.TryAdd(OP_User, user?.Userid);
            reqmap.TryAdd(OP_Content, $"{GetOPName(req.Op)}产品出厂预约单");

            return rsp.ToBytes();
        }
    }
}