﻿using System;
using System.Collections.Generic;
using BaseS.Collection;
using BaseS.Data;
using BaseS.File;
using BaseS.File.Log;
using BaseS.Serialization;
using BaseS.String;
using System.Data;
using System.Linq;
using Yongrong.Db;
using Yongrong.Model.Int.Sum;
using Yongrong.Model.Srvc;
using Yongrong.Srvc.Users;

namespace Yongrong.Int.Sum
{
    class ReportInt : BaseInt
    {

        /// <summary>
        /// 接受过磅单数据
        /// </summary>
        /// <param name="reqmap"></param>
        /// <param name="reqbytes"></param>
        /// <returns></returns>
        public static byte[] GetWeighingSheet(Dictionary<string, string> reqmap, byte[] reqbytes)
        {
            WeighingSheetRsp rsp = new WeighingSheetRsp();

            reqbytes.Json2ObjT<WeighingSheetReq>(out WeighingSheetReq req);
            //进日志
            req.Debug(string.Empty, Enter);

            if (!UserSrvc.GetOne(new Model.Db.User() { Token = req.Token }, out var user))
            {
                rsp.Stat = "用户不存在";

                reqmap.TryAdd(OP_Content, "用户不存在");
                reqmap.TryAdd(OP_Detail, "Token:" + req.Token);

                return rsp.ToBytes();
            }

            if (!Success.Equals(req.Check()))
            {
                rsp.Stat = req.Check();
                reqmap.TryAdd(OP_Content, rsp.Stat);
                reqmap.TryAdd(OP_Detail, reqbytes.B2String());
                return rsp.ToBytes();
            }

            if (string.IsNullOrWhiteSpace(req.Starttime))
            {
                req.Starttime = DateTime.Now.AddDays(-6).ToString("yyyy-MM-dd");
            }

            if (string.IsNullOrWhiteSpace(req.Endtime))
            {
                req.Endtime = DateTime.Now.ToString("yyyy-MM-dd");
            }


            OracleHelper.Query<WeighingSheetData>($@"SELECT a.Dataday, a.Total1,b.Complete1 from  (SELECT ORDERTIME as Dataday,COUNT(ORDERID) AS Total1 from ORDER_GOODS t WHERE ORDERTIME >='{ req.Starttime}' AND  ORDERTIME <='{ req.Endtime} ' AND ORDERSTAT <> '3' GROUP BY ORDERTIME ) a left join (SELECT ORDERTIME as Dataday1,COUNT(ORDERID) AS Complete1 from ORDER_GOODS t WHERE  ORDERTIME >= '{ req.Starttime}' AND  ORDERTIME <= '{ req.Endtime} ' AND ORDERSTAT in('1','2') GROUP BY ORDERTIME)b on a.Dataday=b.Dataday1 ORDER BY a.Dataday", out List<WeighingSheetData> list);

            DateTime.TryParseExact(req.Starttime, "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.None, out DateTime startTime);
            DateTime.TryParseExact(req.Endtime, "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.None, out DateTime endTime);

            for (int i = 0; i <= (endTime - startTime).TotalDays; i++)
            {
                if (!list.Any(a => a.Dataday.Equals(startTime.AddDays(i).ToString("yyyy-MM-dd"))))
                {
                    list.Add(new WeighingSheetData()
                    {
                        Dataday = startTime.AddDays(i).ToString("yyyy-MM-dd"),
                        Total1 = 0,
                        Complete1 = 0
                    });
                }
            }

            rsp.Stat = Success;
            rsp.List = list.OrderBy(a => a.Dataday).ToList();

            reqmap.TryAdd(OP_User, user?.Userid);
            reqmap.TryAdd(OP_Content, $"查询每天过磅单数据");

            return rsp.ToBytes();
        }


        /// <summary>
        /// 接受客户提货数量数据
        /// </summary>
        /// <param name="reqmap"></param>
        /// <param name="reqbytes"></param>
        /// <returns></returns>
        public static byte[] GetCustomerPickUp(Dictionary<string, string> reqmap, byte[] reqbytes)
        {
            PickUpRsp rsp = new PickUpRsp();

            reqbytes.Json2ObjT<PickUpReq>(out PickUpReq req);
            //进日志
            req.Debug(string.Empty, Enter);


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

            if (string.IsNullOrWhiteSpace(req.Starttime))
            {
                req.Starttime = DateTime.Now.AddDays(-6).ToString("yyyy-MM-dd");
            }

            if (string.IsNullOrWhiteSpace(req.Endtime))
            {
                req.Endtime = DateTime.Now.ToString("yyyy-MM-dd") + " 23:59:59";
            }
            else
            {
                req.Endtime += " 23:59:59";
            }
            
            OracleHelper.Query<PickUpData>($@"SELECT SUBSTR(GROSSTIME,1,10) AS Dataday,SUM(NETWEIGHT) AS TotalLoad from ORDER_GOODS t WHERE  NETWEIGHT IS NOT NULL AND GROSSTIME >='{ req.Starttime}' AND  GROSSTIME < '{ req.Endtime} ' AND ISSENDBACK='2' AND ISTOEXIT='1'   GROUP BY SUBSTR(GROSSTIME,1,10) ORDER BY SUBSTR(GROSSTIME,1,10) ", out List<PickUpData> list);

            string[] end=req.Endtime.Split(" ");
            DateTime.TryParseExact(req.Starttime, "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.None, out DateTime startTime);
            DateTime.TryParseExact(end[0], "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.None, out DateTime endTime);

            for (int i = 0; i <=(endTime - startTime).TotalDays; i++)
            {
                if (!list.Any(a => a.Dataday.Equals(startTime.AddDays(i).ToString("yyyy-MM-dd"))))
                {
                    list.Add(new PickUpData()
                    {
                        Dataday = startTime.AddDays(i).ToString("yyyy-MM-dd"),
                        TotalLoad = 0,
                    });
                }
            }

            rsp.Stat = Success;
            rsp.List = list.OrderBy(a => a.Dataday).ToList();

            reqmap.TryAdd(OP_User, user?.Userid);
            reqmap.TryAdd(OP_Content, $"查询客户提货数量");

            return rsp.ToBytes();
        }


        /// <summary>
        /// 接受供应商送货数量数据
        /// </summary>
        /// <param name="reqmap"></param>
        /// <param name="reqbytes"></param>
        /// <returns></returns>
        public static byte[] GetSupplierPickUp(Dictionary<string, string> reqmap, byte[] reqbytes)
        {
            PickUpRsp rsp = new PickUpRsp();

            reqbytes.Json2ObjT<PickUpReq>(out PickUpReq req);
            //进日志
            req.Debug(string.Empty, Enter);


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

            if (string.IsNullOrWhiteSpace(req.Starttime))
            {
                req.Starttime = DateTime.Now.AddDays(-6).ToString("yyyy-MM-dd");
            }

            if (string.IsNullOrWhiteSpace(req.Endtime))
            {
                req.Endtime = DateTime.Now.ToString("yyyy-MM-dd") + " 23:59:59";
            }
            else
            {
                req.Endtime += " 23:59:59";
            }

            OracleHelper.Query<PickUpData>($" SELECT SUBSTR(TARETIME,1,10) AS Dataday,SUM(NETWEIGHT) AS TotalLoad from ORDER_GOODS t WHERE  TARETIME >='{ req.Starttime}' AND  TARETIME < '{ req.Endtime} ' AND ISSENDBACK='1' AND ISTOEXIT='1'  AND NETWEIGHT IS NOT NULL GROUP BY SUBSTR(TARETIME,1,10)  ORDER BY SUBSTR(TARETIME,1,10) ", out List<PickUpData> list);

            string[] end = req.Endtime.Split(" ");
            DateTime.TryParseExact(req.Starttime, "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.None, out DateTime startTime);
            DateTime.TryParseExact(end[0], "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.None, out DateTime endTime);

            for (int i = 0; i <= (endTime - startTime).TotalDays; i++)
            {
                if (!list.Any(a => a.Dataday.Equals(startTime.AddDays(i).ToString("yyyy-MM-dd"))))
                {
                    list.Add(new PickUpData()
                    {
                        Dataday = startTime.AddDays(i).ToString("yyyy-MM-dd"),
                        TotalLoad = 0,
                    });
                }
            }

            rsp.Stat = Success;
            rsp.List = list.OrderBy(a => a.Dataday).ToList();

            reqmap.TryAdd(OP_User, user?.Userid);
            reqmap.TryAdd(OP_Content, $"查询供应商送货数量");

            return rsp.ToBytes();
        }


        /// <summary>
        /// 接受车辆每天入场数量数据
        /// </summary>
        /// <param name="reqmap"></param>
        /// <param name="reqbytes"></param>
        /// <returns></returns>
        public static byte[] GetVehicleEntry(Dictionary<string, string> reqmap, byte[] reqbytes)
        {
            VehicleEntryRsp rsp = new VehicleEntryRsp();

            reqbytes.Json2ObjT<VehicleEntryReq>(out VehicleEntryReq req);
            //进日志
            req.Debug(string.Empty, Enter);

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

            if (string.IsNullOrWhiteSpace(req.Starttime))
            {
                req.Starttime = DateTime.Now.AddDays(-6).ToString("yyyy-MM-dd");
            }

            if (string.IsNullOrWhiteSpace(req.Endtime))
            {
                req.Endtime = DateTime.Now.ToString("yyyy-MM-dd") + " 23:59:59";
            }
            else
            {
                req.Endtime += " 23:59:59";
            }

            OracleHelper.Query<VehicleEntryData>($"SELECT substr(GROSSTIME,1,10)AS Dataday,count(*) AS Num from ORDER_GOODS t WHERE  GROSSTIME is not null AND GROSSTIME >='{ req.Starttime}' AND  GROSSTIME < '{ req.Endtime} ' GROUP BY  substr(GROSSTIME,1,10)  ORDER BY substr(GROSSTIME,1,10) ", out List<VehicleEntryData> list);

            string[] end = req.Endtime.Split(" ");
            DateTime.TryParseExact(req.Starttime, "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.None, out DateTime startTime);
            DateTime.TryParseExact(end[0], "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.None, out DateTime endTime);

            for (int i = 0; i <= (endTime - startTime).TotalDays; i++)
            {
                if (!list.Any(a => a.Dataday.Equals(startTime.AddDays(i).ToString("yyyy-MM-dd"))))
                {
                    list.Add(new VehicleEntryData()
                    {
                        Dataday = startTime.AddDays(i).ToString("yyyy-MM-dd"),
                        Num = 0,
                    });
                }
            }

            rsp.Stat = Success;
            rsp.List = list.OrderBy(a => a.Dataday).ToList();

            reqmap.TryAdd(OP_User, user?.Userid);
            reqmap.TryAdd(OP_Content, $"查询车辆每天入场数量");

            return rsp.ToBytes();
        }

        /// <summary>
        /// 接受每天预约车辆数据
        /// </summary>
        /// <param name="reqmap"></param>
        /// <param name="reqbytes"></param>
        /// <returns></returns>
        public static byte[] GetOrderVehicle(Dictionary<string, string> reqmap, byte[] reqbytes)
        {
            OrderVehicleRsp rsp = new OrderVehicleRsp();

            reqbytes.Json2ObjT<OrderVehicleReq>(out OrderVehicleReq req);
            //进日志
            req.Debug(string.Empty, Enter);

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

            if (string.IsNullOrWhiteSpace(req.Starttime))
            {
                req.Starttime = DateTime.Now.AddDays(-6).ToString("yyyy-MM-dd");
            }

            if (string.IsNullOrWhiteSpace(req.Endtime))
            {
                req.Endtime = DateTime.Now.ToString("yyyy-MM-dd");
            }

            OracleHelper.Query<OrderVehicleData>($"SELECT ORDERTIME,COUNT(*) AS Num from ORDER_GOODS t WHERE ORDERTIME >='{ req.Starttime}' AND  ORDERTIME <='{ req.Endtime}' GROUP BY ORDERTIME ORDER BY ORDERTIME ", out List<OrderVehicleData> list);

            DateTime.TryParseExact(req.Starttime, "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.None, out DateTime startTime);
            DateTime.TryParseExact(req.Endtime, "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.None, out DateTime endTime);

            for (int i = 0; i <= (endTime - startTime).TotalDays; i++)
            {
                if (!list.Any(a => a.OrderTime.Equals(startTime.AddDays(i).ToString("yyyy-MM-dd"))))
                {
                    list.Add(new OrderVehicleData()
                    {
                        OrderTime = startTime.AddDays(i).ToString("yyyy-MM-dd"),
                        Num = 0
                    });
                }
            }

            rsp.Stat = Success;
            rsp.List = list.OrderBy(a => a.OrderTime).ToList();

            reqmap.TryAdd(OP_User, user?.Userid);
            reqmap.TryAdd(OP_Content, $"查询每天预约车辆");

            return rsp.ToBytes();
        }
    }
}
