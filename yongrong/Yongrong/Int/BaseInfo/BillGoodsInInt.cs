﻿using System;
using System.Collections.Generic;
using System.Text;
using Yongrong.Model.Int.BaseInfo;
using Yongrong.Srvc.BaseInfo;
using BaseS.Serialization;
using Yongrong.Model.Int;
using System.Runtime.InteropServices.ComTypes;
using Yongrong.Srvc.Users;
using BaseS.String;
using BaseS.File.Log;
using Yongrong.Model.Db;

namespace Yongrong.Int.BaseInfo
{
    class BillGoodsInInt : BaseInt
    {
        /// <summary>
        /// 原辅料进场计划总单管理
        /// </summary>
        /// <param name="reqmap"></param>
        /// <param name="reqbytes"></param>
        /// <returns></returns>
        public static byte[] Get(Dictionary<string, string> reqmap, byte[] reqbytes)
        {
            BillGoodsInRsp rsp = new BillGoodsInRsp();

            reqbytes.Json2ObjT<BillGoodsInReq>(out BillGoodsInReq req);

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

            BillGoodsInSrvc.Get(
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

            //获取供应商
            //BaseSupplierSrvc.Get(null, pageBean, out var suppliers);
            rsp.Suppliers = new List<Model.Db.BaseSupplier>();

            //获取物料
            //BaseGoodsSrvc.Get(null, pageBean, out var goods);
            rsp.Goods = new List<BaseGoods>();

            //获取牵引车
            //BaseTractorSrvc.Get(null, pageBean, out var tractors);
            rsp.Tractors = new List<BaseTractor>();

            //获取挂车
            //BaseTrailerSrvc.Get(null, pageBean, out var trailers);
            rsp.Trailers = new List<BaseTrailer>();

            //获取司机
            //BaseDriverSrvc.Get(null, new PageBean() { Index = 1, Size = 5000 }, out var drivers);
            rsp.Drivers = new List<BaseDriver>();

            //获取押运员
            //BaseSupercargoSrvc.Get(null, pageBean, out var supercargos);
            rsp.Supercargos = new List<BaseSupercargo>();

            //运输公司
            BaseTransportSrvc.Get(null, pageBean, out var transports);
            rsp.Transports = transports;

            reqmap.TryAdd(OP_User, user?.Userid);
            reqmap.TryAdd(OP_Content, "查询原辅料进场计划总单");
            return rsp.ToBytes();
        }


        /// <summary>
        /// 原辅料进场计划总单管理新增修改
        /// </summary>
        /// <param name="reqmap"></param>
        /// <param name="reqbytes"></param>
        /// <returns></returns>
        public static byte[] AddUpd(Dictionary<string, string> reqmap, byte[] reqbytes)
        {
            BillGoodsInAddRsp rsp = new BillGoodsInAddRsp();

            reqbytes.Json2ObjT<BillGoodsInAddReq>(out BillGoodsInAddReq req);
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

            rsp.Stat = BillGoodsInSrvc.AddOrUpdate(req.Goodsextenterbill, req.Op);

            reqmap.TryAdd(OP_User, user?.Userid);
            reqmap.TryAdd(OP_Content, $"{GetOPName(req.Op)}原辅料进场计划总单");


            return rsp.ToBytes();
        }
    }
}
