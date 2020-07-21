﻿using System.Collections.Generic;
using Yongrong.Model.Int.BaseInfo;
using Yongrong.Srvc.BaseInfo;
using BaseS.Serialization;
using BaseS.String;
using Yongrong.Srvc.Users;
using Yongrong.Model.Int;
using Yongrong.Model.Db;

namespace Yongrong.Int.BaseInfo
{
    class BaseSupplierInt:BaseInt
    {
        /// <summary>
        /// 查询供货商
        /// </summary>
        /// <param name="reqmap"></param>
        /// <param name="reqbytes"></param>
        /// <returns></returns>
        public static byte[] Get(Dictionary<string, string> reqmap, byte[] reqbytes)
        {
            BaseSupplierRsp rsp = new BaseSupplierRsp();

            reqbytes.Json2ObjT<BaseSupplierReq>(out BaseSupplierReq req);

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

            BaseSupplierSrvc.Get(
                new Model.Db.BaseSupplier() { Enterpriseid = req.Enterpriseid, Creditid = req.Creditid, Enterprisename = req.Enterprisename },
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
            rsp.SupplierList = tmp;

            reqmap.TryAdd(OP_User, user?.Userid);
            reqmap.TryAdd(OP_Content, $"查询供应商");

            return rsp.ToBytes();
        }


        /// <summary>
        /// 变更供货商
        /// </summary>
        /// <param name="reqmap"></param>
        /// <param name="reqbytes"></param>
        /// <returns></returns>
        public static byte[] AddUpd(Dictionary<string, string> reqmap, byte[] reqbytes)
        {
            BaseSupplierAddRsp rsp = new BaseSupplierAddRsp();

            reqbytes.Json2ObjT<BaseSupplierAddReq>(out BaseSupplierAddReq req);

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

            rsp.Stat = BaseSupplierSrvc.AddOrUpdate(req.Supplier, req.Op);

            reqmap.TryAdd(OP_User, user?.Userid);
            reqmap.TryAdd(OP_Content, $"{req.Op}供应商");

            if (1 == req.Issynch)
            {
                BaseTransportSrvc.Get(new BaseTransport() { Name = req.Supplier.Enterprisename }, new PageBean() { Index = 1, Size = 1 }, out var list);
                if (null == list || list.Count < 1)
                {
                    BaseTransportSrvc.AddOrUpdate(new BaseTransport() { Name = req.Supplier.Enterprisename }, ADD);
                }
            }

            return rsp.ToBytes();
        }

        /// <summary>
        /// 联想查询供货商
        /// </summary>
        /// <param name="reqmap"></param>
        /// <param name="reqbytes"></param>
        /// <returns></returns>
        public static byte[] Search(Dictionary<string, string> reqmap, byte[] reqbytes)
        {
            SearchRsp rsp = new SearchRsp();

            reqbytes.Json2ObjT<BaseSupplierReq>(out BaseSupplierReq req);

            if (null == req)
            {
                rsp.Stat = JsonErr;

                reqmap.TryAdd(OP_Content, rsp.Stat);
                reqmap.TryAdd(OP_Detail, reqbytes.B2String());

                return rsp.ToBytes();
            }

            if (string.IsNullOrWhiteSpace(req.Enterprisename))
            {
                rsp.Stat = "参数为空";
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

            BaseSupplierSrvc.Search(
                new Model.Db.BaseSupplier() { Enterprisename = req.Enterprisename },
                out var tmp);


            rsp.Stat = Success;
            rsp.List = tmp;

            reqmap.TryAdd(OP_User, user?.Userid);
            reqmap.TryAdd(OP_Content, $"联想查询供应商");

            return rsp.ToBytes();
        }
    }
}
