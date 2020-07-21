using System;
using System.Collections.Generic;
using System.Text;
using Yongrong.Model.Int.BaseInfo;
using Yongrong.Srvc.BaseInfo;
using BaseS.Serialization;
using Yongrong.Srvc.Sys;
using BaseS.String;
using BaseS.Collection;
using Yongrong.Srvc.Users;
using Yongrong.Model.Int;
using Yongrong.Model.Db;

namespace Yongrong.Int.BaseInfo
{
    class BaseCustomInt : BaseInt
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="reqmap"></param>
        /// <param name="reqbytes"></param>
        /// <returns></returns>
        public static byte[] Get(Dictionary<string, string> reqmap, byte[] reqbytes)
        {
            BaseCustomerRsp rsp = new BaseCustomerRsp();

            reqbytes.Json2ObjT<BaseCustomerReq>(out BaseCustomerReq req);

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

            BaseCustomSrvc.Get(
                new Model.Db.BaseCustomer() { Enterpriseid = req.Enterpriseid, Creditid = req.Creditid, Enterprisename = req.Enterprisename },
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
            rsp.Customers = tmp;


            reqmap.TryAdd(OP_Content, "查询客户");
            reqmap.TryAdd(OP_User, user.Userid);
            reqmap.TryAdd(OP_Detail, "");

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
            BaseCustomerAddRsp rsp = new BaseCustomerAddRsp();

            reqbytes.Json2ObjT<BaseCustomerAddReq>(out BaseCustomerAddReq req);

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

            rsp.Stat = BaseCustomSrvc.AddOrUpdate(req.Customer, req.Op);

            reqmap.TryAdd(OP_User, user?.Userid);
            reqmap.TryAdd(OP_Content, $"{GetOPName(req.Op)}客户");

            if(1 == req.Issynch)
            {
                BaseTransportSrvc.Get(new BaseTransport() { Name = req.Customer.Enterprisename }, new PageBean() { Index = 1, Size = 1 }, out var list);
                if(null == list || list.Count < 1)
                {
                    BaseTransportSrvc.AddOrUpdate(new BaseTransport() { Name = req.Customer.Enterprisename }, ADD);
                }
            }

            return rsp.ToBytes();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reqmap"></param>
        /// <param name="reqbytes"></param>
        /// <returns></returns>
        public static byte[] Search(Dictionary<string, string> reqmap, byte[] reqbytes)
        {
            SearchRsp rsp = new SearchRsp();

            reqbytes.Json2ObjT<BaseCustomerReq>(out BaseCustomerReq req);

            if (null == req)
            {
                rsp.Stat = JsonErr;

                reqmap.TryAdd(OP_Content, rsp.Stat);
                reqmap.TryAdd(OP_Detail, reqbytes.B2String());

                return rsp.ToBytes();
            }

            if (string.IsNullOrWhiteSpace(req.Enterprisename))
            {
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

            BaseCustomSrvc.Search(
                new Model.Db.BaseCustomer() { Enterprisename = req.Enterprisename },
                out var tmp);


            rsp.Stat = Success;
            rsp.List = tmp;


            reqmap.TryAdd(OP_Content, "联想查询客户");
            reqmap.TryAdd(OP_User, user.Userid);
            reqmap.TryAdd(OP_Detail, "");

            return rsp.ToBytes();
        }
    }
}
