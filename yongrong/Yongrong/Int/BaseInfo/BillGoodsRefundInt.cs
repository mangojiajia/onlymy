﻿using System;
using System.Collections.Generic;
using System.Text;
using Yongrong.Model.Int.BaseInfo;
using Yongrong.Srvc.BaseInfo;
using BaseS.Serialization;
using Yongrong.Model.Int;
using BaseS.String;
using Yongrong.Srvc.Users;
using BaseS.File.Log;

namespace Yongrong.Int.BaseInfo
{
    class BillGoodsRefundInt : BaseInt
    {
        /// <summary>
        /// 产品退货管理总单查询
        /// </summary>
        /// <param name="reqmap"></param>
        /// <param name="reqbytes"></param>
        /// <returns></returns>
        public static byte[] Get(Dictionary<string, string> reqmap, byte[] reqbytes)
        {
            BillGoodsRefundRsp rsp = new BillGoodsRefundRsp();

            reqbytes.Json2ObjT<BillGoodsRefundReq>(out BillGoodsRefundReq req);
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

            BillGoodsRefundSrvc.Get(
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

            rsp.Stat = Success;
            rsp.List = tmp;

            PageBean pageBean = new PageBean() { Index = 1, Size = 5000 };

            //获取客户集合
            //BaseCustomSrvc.Get(null, pageBean, out var customers);
            rsp.Customers = new List<Model.Db.BaseCustomer>();

            //获取供应商
            //BaseSupplierSrvc.Get(null, pageBean, out var suppliers);
            rsp.Suppliers = new List<Model.Db.BaseSupplier>();

            //获取物料
            //BaseGoodsSrvc.Get(null, pageBean, out var goods);
            rsp.Goods = new List<Model.Db.BaseGoods>();

            //获取牵引车
            //BaseTractorSrvc.Get(null, pageBean, out var tractors);
            rsp.Tractors = new List<Model.Db.BaseTractor>();

            //获取挂车
            //BaseTrailerSrvc.Get(null, pageBean, out var trailers);
            rsp.Trailers = new List<Model.Db.BaseTrailer>();

            //获取司机
            //BaseDriverSrvc.Get(null, pageBean, out var drivers);
            rsp.Drivers = new List<Model.Db.BaseDriver>();

            //获取押运员
            //BaseSupercargoSrvc.Get(null, pageBean, out var supercargos);
            rsp.Supercargos = new List<Model.Db.BaseSupercargo>();

            //运输公司
            BaseTransportSrvc.Get(null, pageBean, out var transports);
            rsp.Transports = transports;

            reqmap.TryAdd(OP_User, user?.Userid);
            reqmap.TryAdd(OP_Content, "查询产品退货计划总单");

            return rsp.ToBytes();
        }


        /// <summary>
        /// 新增 修改 产品退货总单管理
        /// </summary>
        /// <param name="reqmap"></param>
        /// <param name="reqbytes"></param>
        /// <returns></returns>
        public static byte[] AddUpd(Dictionary<string, string> reqmap, byte[] reqbytes)
        {
            BillGoodsRefundAddRsp rsp = new BillGoodsRefundAddRsp();

            reqbytes.Json2ObjT<BillGoodsRefundAddReq>(out BillGoodsRefundAddReq req);
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

            rsp.Stat = BillGoodsRefundSrvc.AddOrUpdate(req.GoodsRefund, req.Op);

            reqmap.TryAdd(OP_User, user?.Userid);
            reqmap.TryAdd(OP_Content, $"{GetOPName(req.Op)}产品退货计划总单");

            return rsp.ToBytes();
        }
    }
}