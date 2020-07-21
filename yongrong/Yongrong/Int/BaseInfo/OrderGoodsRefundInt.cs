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
    class OrderGoodsRefundInt: BaseInt
    {
        /// <summary>
        /// 产品退货预约管理
        /// </summary>
        /// <param name="reqmap"></param>
        /// <param name="reqbytes"></param>
        /// <returns></returns>
        public static byte[] Get(Dictionary<string, string> reqmap, byte[] reqbytes)
        {
            OrderGoodsRefundRsp rsp = new OrderGoodsRefundRsp();

            reqbytes.Json2ObjT<OrderGoodsRefundReq>(out OrderGoodsRefundReq req);
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

            //req.Query.Istoexit = "2";
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
            reqmap.TryAdd(OP_Content, "查询产品退货预约单");

            return rsp.ToBytes();
        }

        /*
        /// <summary>
        /// 
        /// </summary>
        /// <param name="reqmap"></param>
        /// <param name="reqbytes"></param>
        /// <returns></returns>
        public static byte[] AddUpd(Dictionary<string, string> reqmap, byte[] reqbytes)
        {
            OrderGoodsRefundAddRsp rsp = new OrderGoodsRefundAddRsp();

            reqbytes.Json2ObjT<OrderGoodsRefundAddReq>(out OrderGoodsRefundAddReq req);
            //进日志
            req.Debug(string.Empty, Enter);
            if (null == req)
            {
                rsp.Stat = JsonErr;
                return rsp.ToBytes();
            }

            if (!Success.Equals(req.Check()))
            {
                rsp.Stat = req.Check();
                return rsp.ToBytes();
            }

            req.OrderGoodsRefund.Istoexit = "2";
            rsp.Stat = OrderGoodsSrvc.AddOrUpdate(req.OrderGoodsRefund, req.Op);

            return rsp.ToBytes();
        }
        */
    }
}
