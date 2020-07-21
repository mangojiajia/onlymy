﻿using BaseS.Collection;
using BaseS.Data;
using BaseS.File;
using BaseS.File.Log;
using BaseS.Net.Http;
using BaseS.Serialization;
using BaseS.String;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using Yongrong.Db;
using Yongrong.Model.Db;
using Yongrong.Model.Int.BaseInfo;
using Yongrong.Model.Int.Sum;
using Yongrong.Model.Srvc;
using Yongrong.Srvc.BaseInfo;
using Yongrong.Srvc.Push;
using Yongrong.Srvc.Users;

namespace Yongrong.Int.BaseInfo
{
    class OrderGoodsInt : BaseInt
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="reqmap"></param>
        /// <param name="reqbytes"></param>
        /// <returns></returns>
        public static byte[] Get(Dictionary<string, string> reqmap, byte[] reqbytes)
        {
            OrderGoodsRsp rsp = new OrderGoodsRsp();

            reqbytes.Json2ObjT<OrderGoodsReq>(out OrderGoodsReq req);
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

            reqmap.TryGetValue("cmd", out var cmd);

            if (Sys.CustomRole.Contains(user.Roleids))
            {
                req.Query.Company = user.Userid;
            }

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
            reqmap.TryAdd(OP_Content, $"查询{GetCmdName(cmd)}");

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
            OrderGoodsAddRsp rsp = new OrderGoodsAddRsp();

            reqbytes.Json2ObjT<OrderGoodsAddReq>(out OrderGoodsAddReq req);
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

            if (ADD.Equals(req.Op))
            {
                rsp.Stat = OrderGoodsSrvc.SubBill(req.OrderGoods);

                if (Success.Equals(rsp.Stat) && Sys.JPushFlag)
                {
                    //如果为0，需要推送给永荣审核
                    if ("0".Equals(req.OrderGoods.Billcheck))
                    {
                        //UserSrvc.Get(new User() { Username = req.OrderGoods.Ordername }, out var userlist);

                        OracleHelper.Query<User>($"SELECT UserId from USER_LOGIN t WHERE UserName='{req.OrderGoods.Ordername}' AND ROLEIDS NOT IN( '驾驶员','司机','driver')", out var userlist);

                        JPushSrvc.SendPush($"你有一条司机{req.OrderGoods.GetBillType.Replace("进厂", "送货").Replace("出厂", "提货").Trim()}待审核的预约，预约码为{req.OrderGoods.Orderid}", userlist?.Select(a => a.Userid));
                    }

                    // 推送给客商
                    if ("0".Equals(req.OrderGoods.Customercheck))
                    {
                        JPushSrvc.SendPush($"你有一条司机{req.OrderGoods.GetBillType.Replace("进厂", "送货").Replace("出厂", "提货").Trim()}待审核的预约，预约码为{req.OrderGoods.Orderid}", new List<string>() { req.OrderGoods.Company });
                    }
                }
            }
            else
            {
                rsp.Stat = OrderGoodsSrvc.AddOrUpdate(req.OrderGoods, req.Op, user.Roleids);
            }

            reqmap.TryGetValue("cmd", out var cmd);
            reqmap.TryAdd(OP_User, user?.Userid);
            reqmap.TryAdd(OP_Content, $"查询{GetOPName(req.Op)}{GetCmdName(cmd)}");

            return rsp.ToBytes();
        }

        /// <summary>
        /// 根据id，获取要打印的预约信息
        /// </summary>
        /// <param name="reqmap"></param>
        /// <param name="reqbytes"></param>
        /// <returns></returns>
        public static byte[] QueryOrder(Dictionary<string, string> reqmap, byte[] reqbytes)
        {
            OrderGoodsRsp rsp = new OrderGoodsRsp();

            reqbytes.Json2ObjT<OrderGoodsReq>(out OrderGoodsReq req);
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

            OrderGoodsSrvc.GetOneById(req.Query, out OrderGoods order);
            if (null == order)
            {
                rsp.Stat = "预约信息不存在";
                return rsp.ToBytes();
            }

            rsp.order = order;

            rsp.Stat = Success;
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
            OrderGoodsReq req = null;

            if (null != reqbytes)
            {
                reqbytes.Json2ObjT<OrderGoodsReq>(out req);
            }

            string Token = reqmap.GetDicStr("Token");

            if (null == req)
            {
                req = new OrderGoodsReq();
            }

            if (null == req.Query)
            {

                req.Query = new OrderGoods();

            }

            req.Page = new Model.Int.PageBean() { Index = 1, Size = 10000 };
            req.Query.Starttime = reqmap.GetDicStr("Starttime", req.Query.Starttime);
            req.Query.Endtime = reqmap.GetDicStr("Endtime", req.Query.Endtime);
            req.Query.Driver = reqmap.GetDicStr("Driver", req.Query.Driver);
            req.Query.Goodsname = reqmap.GetDicStr("Goodsname", req.Query.Goodsname);
            req.Query.Tractorid = reqmap.GetDicStr("Tractorid", req.Query.Tractorid);

            if (!UserSrvc.GetOne(new Model.Db.User() { Token = Token }, out var user))
            {
                rsp.Stat = "用户不存在";

                reqmap.TryAdd(OP_Content, "用户不存在");
                reqmap.TryAdd(OP_Detail, "Token:" + Token);

                return rsp.ToBytes();
            }

            reqmap.TryAdd(OP_User, user?.Userid);
            reqmap.TryAdd(OP_Content, $"导出地磅记录");

            OrderGoodsSrvc.Get(req.Query, req.Page, out var list);

            if (null != list && list.Count > 0)
            {
                DataTable dt = list.ToDataTable();
                Dictionary<string, string> cmap = new Dictionary<string, string>();
                cmap.Add("Tractorid", "牵引车");
                cmap.Add("Trailerid", "挂车");
                cmap.Add("Billid", "单号");
                cmap.Add("Orderid", "预约号");
                cmap.Add("Issendback", "订单类型");
                cmap.Add("Goodsname", "物料");
                cmap.Add("Realweight", "预约重量");
                cmap.Add("Netweight", "净重");
                cmap.Add("Grosstime", "过毛时间");
                cmap.Add("Grossweight", "毛重");
                cmap.Add("Tareman", "过磅员");
                cmap.Add("Company", "发货单位");
                cmap.Add("Company2", "收货单位");
                cmap.Add("Driver", "司机");
                cmap.Add("Createtime", "预约时间");

                BCsv.DataTable2CsvString(dt, out string dataString, cmap);

                reqmap.TryAdd(BHttpBean.ContentDisposition, $"attachment; filename= { HttpUtility.UrlEncode("过磅数据汇总.csv")}");
                reqmap.TryAdd(BHttpBean.ContentType, "application/octet-stream; charset=utf-8");
                byte[] emptyfileBytes = new byte[] { 0xEF, 0xBB, 0xBF };
                byte[] total = null;
                byte[] body = Encoding.UTF8.GetBytes(dataString);

                total = new byte[emptyfileBytes.Length + body.Length];
                Buffer.BlockCopy(emptyfileBytes, 0, total, 0, emptyfileBytes.Length);
                Buffer.BlockCopy(body, 0, total, emptyfileBytes.Length, body.Length);

                return total;

            }
            return rsp.ToBytes();
        }
     
    }
}
