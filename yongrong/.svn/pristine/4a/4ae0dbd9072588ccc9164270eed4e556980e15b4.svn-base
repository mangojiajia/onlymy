using System;
using System.Collections.Generic;
using System.Text;
using Yongrong.Model.Int.Sys;
using BaseS.Serialization;
using Yongrong.Srvc.Users;
using Yongrong.Srvc.Sys;
using BaseS.String;
using BaseS.Collection;

namespace Yongrong.Int.Sys
{
    class UserOpLogInt : BaseInt
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="reqmap"></param>
        /// <param name="reqbytes"></param>
        /// <returns></returns>
        public static byte[] Get(Dictionary<string, string> reqmap, byte[] reqbytes)
        {
            UserOpLogGetRsp rsp = new UserOpLogGetRsp();

            reqbytes.Json2ObjT<UserOpLogGetReq>(out UserOpLogGetReq req);

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

            if (!UserSrvc.GetOne(new Model.Db.User() { Token = req.Token }, out var user))
            {
                rsp.Stat = "用户不存在";

                return rsp.ToBytes();
            }

            UserOpLogSrvc.Get(
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
            rsp.Data = tmp;

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
            UserOpLogGetAddRsp rsp = new UserOpLogGetAddRsp();

            reqbytes.Json2ObjT<UserOpLogGetAddReq>(out UserOpLogGetAddReq req);

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

            rsp.Stat = UserOpLogSrvc.AddOrUpdate(req.UserOplog, req.Op);

            return rsp.ToBytes();
        }
    }
}
