using System;
using System.Collections.Generic;
using System.Text;
using Yongrong.Model.Int.BaseInfo;
using BaseS.Serialization;
using Yongrong.Srvc.BaseInfo;
using BaseS.String;
using Yongrong.Srvc.Users;
using Yongrong.Model.Db;
using Yongrong.Model.Int;

namespace Yongrong.Int.BaseInfo
{
    class BaseTransportInt : BaseInt
    {
        /// <summary>
        /// 获取物料信息
        /// </summary>
        /// <param name="reqmap"></param>
        /// <param name="reqbytes"></param>
        /// <returns></returns>
        public static byte[] Get(Dictionary<string, string> reqmap, byte[] reqbytes)
        {
            BaseTransportRsp rsp = new BaseTransportRsp();

            reqbytes.Json2ObjT<BaseTransportReq>(out BaseTransportReq req);

            if (null == req)
            {
                rsp.Stat = JsonErr;
                reqmap.TryAdd(OP_Content, $"格式错误");
                reqmap.TryAdd(OP_Detail, reqbytes.B2String());
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

                reqmap.TryAdd(OP_Content, $"用户不存在");
                reqmap.TryAdd(OP_Detail, $"Token:{req.Token}");

                return rsp.ToBytes();
            }

            BaseTransportSrvc.Get(
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
            rsp.Transpots = tmp;
            reqmap.TryAdd(OP_User, user.Userid);
            reqmap.TryAdd(OP_Content, "查询运输公司");

            return rsp.ToBytes();
        }


        /// <summary>
        /// 添加物料信息
        /// </summary>
        /// <param name="reqmap"></param>
        /// <param name="reqbytes"></param>
        /// <returns></returns>
        public static byte[] AddUpd(Dictionary<string, string> reqmap, byte[] reqbytes)
        {
            BaseTransportAddRsp rsp = new BaseTransportAddRsp();

            reqbytes.Json2ObjT<BaseTransportAddReq>(out BaseTransportAddReq req);

            if (null == req)
            {
                rsp.Stat = JsonErr;
                reqmap.TryAdd(OP_Content, $"格式错误");
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

                reqmap.TryAdd(OP_Content, $"用户不存在");
                reqmap.TryAdd(OP_Detail, $"Token:{req.Token}");

                return rsp.ToBytes();
            }



            rsp.Stat = BaseTransportSrvc.AddOrUpdate(req.Transport, req.Op);

            reqmap.TryAdd(OP_User, user.Userid);
            reqmap.TryAdd(OP_Content, $"{req.Op}运输");

            return rsp.ToBytes();
        }

        /// <summary>
        /// 根据运输公司，返回司机，牵引车，挂车，押运员列表
        /// </summary>
        /// <param name="reqmap"></param>
        /// <param name="reqbytes"></param>
        /// <returns></returns>
        public static byte[] SearchInfo(Dictionary<string, string> reqmap, byte[] reqbytes)
        {
            TransportSearchRsp rsp = new TransportSearchRsp() { Stat = Success };

            reqbytes.Json2ObjT<BaseTransportReq>(out BaseTransportReq req);

            if (null == req)
            {
                rsp.Stat = JsonErr;
                reqmap.TryAdd(OP_Content, $"格式错误");
                reqmap.TryAdd(OP_Detail, reqbytes.B2String());
                return rsp.ToBytes();
            }

            if (!UserSrvc.GetOne(new Model.Db.User() { Token = req.Token }, out var user))
            {
                rsp.Stat = "用户不存在";

                reqmap.TryAdd(OP_Content, $"用户不存在");
                reqmap.TryAdd(OP_Detail, $"Token:{req.Token}");

                return rsp.ToBytes();
            }

            BaseDriverSrvc.Get(new BaseDriver() { Transport = req.Query.Name }, new PageBean() { Index = 1, Size = 500 }, out var drivers);
            rsp.Drivers = drivers;

            BaseTractorSrvc.Get(new BaseTractor() { Transport = req.Query.Name }, new PageBean() { Index = 1, Size = 500 }, out var tractors);
            rsp.Tractors = tractors;

            BaseTrailerSrvc.Get(new BaseTrailer() { Transport = req.Query.Name }, new PageBean() { Index = 1, Size = 500 }, out var trailers);
            rsp.Trailers = trailers;

            BaseSupercargoSrvc.Get(new BaseSupercargo() { Transport = req.Query.Name }, new PageBean() { Index = 1, Size = 500 }, out var supercargos);
            rsp.Supercargos = supercargos;
            
            reqmap.TryAdd(OP_User, user.Userid);
            reqmap.TryAdd(OP_Content, "根据运输公司查询返回司机牵引车挂车押运员");

            return rsp.ToBytes();
        }

        /// <summary>
        /// 联想运输公司
        /// </summary>
        /// <param name="reqmap"></param>
        /// <param name="reqbytes"></param>
        /// <returns></returns>
        public static byte[] Search(Dictionary<string, string> reqmap, byte[] reqbytes)
        {
            SearchRsp rsp = new SearchRsp() { Stat = Success };

            reqbytes.Json2ObjT<BaseTransportReq>(out BaseTransportReq req);

            if (null == req)
            {
                rsp.Stat = JsonErr;
                reqmap.TryAdd(OP_Content, $"格式错误");
                reqmap.TryAdd(OP_Detail, reqbytes.B2String());
                return rsp.ToBytes();
            }

            if (!UserSrvc.GetOne(new Model.Db.User() { Token = req.Token }, out var user))
            {
                rsp.Stat = "用户不存在";

                reqmap.TryAdd(OP_Content, $"用户不存在");
                reqmap.TryAdd(OP_Detail, $"Token:{req.Token}");

                return rsp.ToBytes();
            }

            BaseTransportSrvc.Search(new BaseTransport() { Name = req?.Query?.Name }, out var list);

            rsp.List = list;

            reqmap.TryAdd(OP_User, user.Userid);
            reqmap.TryAdd(OP_Content, "联想运输公司");

            return rsp.ToBytes();
        }
    }
}
