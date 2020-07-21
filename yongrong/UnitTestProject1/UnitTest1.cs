using BaseS.File;
using BaseS.Security;
using BaseS.Serialization;
using BaseS.String;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Data;
using Yongrong.Db;
using Yongrong.Model.Db;
using Yongrong.Model.Int.BaseInfo;
using Yongrong.Model.Int.Gate;
using Yongrong.Model.Int.IC;
using Yongrong.Model.Int.Weigh;
using Yongrong.Model.Srvc;
using Yongrong.Srvc.Gate;
using Yongrong.Srvc.Push;
using Yongrong.Srvc.Sys;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod3()
        {
            //JPushSrvc.SendPush(string.Empty);
            //string realWeight = "21500";
            //realWeight = (Convert.ToDecimal(realWeight) / 1000).ToString();
            //Console.WriteLine(realWeight);

            WeighSyncReq req = new WeighSyncReq();
            req.Orderid = "0297";
            req.Msgid = "1A202006200003";
            req.Weightime = "1";
            req.Tractorid = "豫RCF229";
            req.Trailerid = "";
            req.Passtime = "2020-06-20 01:18:16";
            req.Operator = "Operator";
        }

        [TestMethod]
        public void TestMethod1()
        {
            ICQueryOrderReq req1 = new ICQueryOrderReq()
            {
                Order = "",
                Tractor = ""
            };

            BJson.Obj2JsonT<ICQueryOrderReq>(req1, out string js);

            DateTime dt = DateTime.UtcNow;
            DateTime dt2 = DateTime.Now;

            string s = string.Join(',', new System.Collections.Generic.List<string>());

            BaseGoodsAddReq req = new BaseGoodsAddReq()
            {
                Goods = new Yongrong.Model.Db.BaseGoods()
                {
                    Goodsname = "1",
                    Goodsid = "2",
                }
            };

            BJson.Obj2JsonT<BaseGoodsAddReq>(req, out string json);

            Assert.IsNull(json);
        }

        [TestMethod]
        public void TestMethod2()
        {
            var str = "{\"errorCode\":0,\"errorMessage\":\"获取车辆信息成功\",\"data\":{\"total\":1,\"pageNo\":1,\"pageSize\":10,\"list\":[{\"carUuid\":\"c115c38b95dd4935bc6cbc969ccc1300\",\"plateNo\":\"闽A5G105\",\"plateType\":0,\"plateColor\":0,\"carType\":2,\"ownerId\":null,\"carColor\":1,\"personName\":null,\"phoneNo\":null,\"cardNo\":null,\"maxPassenger\":null,\"plateStart\":null}]}}";

            str.Json2ObjT<GateGetCarInfoRsp>(out GateGetCarInfoRsp rsp);

            Assert.IsNotNull(rsp);
        }

        [TestMethod]
        public void Test3()
        {
            //BExcel.ToDataTable("d:\\123.xls", "hangzhou$", out DataTable data);
            // BExcel.ToDataTable("d:\\shiguang.xlsx", "hangzhou$", out DataTable data);
           string Aa =  BString.GetString("4664.3");
           // Assert.IsNotNull(data);
        }


        [TestMethod]
        public void Test4()
        {
            //JPushSrvc.SendPush("时光", IEnumerable<string> list);
        }

        [TestMethod]
        public void Test5()
        {
            OracleHelper.Query<User>("SELECT UserId from USER_LOGIN t WHERE UserName='rfrfrf' AND ROLEIDS IN ('采购','管理员','销售')", out List<User> data11);

            //过磅单数据
            OracleHelper.Query<WeighingSheetData>("SELECT a.RIQI, a.ZNUM,b.SHNUM from (SELECT SUBSTR(CREATETIME,1,10) as RIQI,COUNT(ORDERID) AS ZNUM from ORDER_GOODS t WHERE ORDERSTAT <> '3' GROUP BY SUBSTR(CREATETIME,1,10) ) a left join (SELECT SUBSTR(CREATETIME,1,10 ) as SRIQI,COUNT(ORDERID) AS SHNUM from ORDER_GOODS t WHERE ORDERSTAT in('1','2') GROUP BY SUBSTR(CREATETIME,1,10))b on a.RIQI=b.SRIQI ORDER BY a.RIQI", out List<WeighingSheetData> data22);

            //客户提货数量汇总
            OracleHelper.Query<PickUpData>("SELECT SUBSTR(CREATETIME,1,10) AS RiQi,SUM(NETWEIGHT) AS Load from ORDER_GOODS t WHERE ISSENDBACK='2' AND ISTOEXIT='1'  AND NETWEIGHT IS NOT NULL GROUP BY SUBSTR(CREATETIME,1,10) ORDER BY SUBSTR(CREATETIME,1,10) ", out List<PickUpData> data1);

            //供应商送货数据汇总
            OracleHelper.Query<PickUpData>(" SELECT SUBSTR(CREATETIME,1,10) AS RiQi,SUM(NETWEIGHT) AS Load from ORDER_GOODS t WHERE ISSENDBACK='1' AND ISTOEXIT='1'  AND NETWEIGHT IS NOT NULL GROUP BY SUBSTR(CREATETIME,1,10)  ORDER BY SUBSTR(CREATETIME,1,10) ", out List<PickUpData> data2);

            //车辆每天入场数量数据分析
            OracleHelper.Query<VehicleEntryData>("SELECT substr(GROSSTIME,1,10)AS RiQi,count(*) AS Num from ORDER_GOODS t WHERE  GROSSTIME is not null GROUP BY  substr(GROSSTIME,1,10)  ORDER BY substr(GROSSTIME,1,10) ", out List<VehicleEntryData> data3);

            //每天预约车辆数据分析
            OracleHelper.Query<OrderVehicleData>("SELECT ORDERTIME,COUNT(*) AS Num from ORDER_GOODS t  GROUP BY ORDERTIME ORDER BY ORDERTIME ", out List<OrderVehicleData> data4);

            //每天进场总单计划数据分析统计
            OracleHelper.Query<TotalPlanData>("SELECT substr(CREATETIME,1,10) AS RiQi ,SUM(GOODSNUMBER) AS TotalNum, SUM(LEVELNUMBER) AS SurplusNum from BILL_GOODSIN t  GROUP BY  substr(CREATETIME,1,10)  ORDER BY substr(CREATETIME,1,10) ", out List<TotalPlanData> data5);

            //每天出厂总单计划数据分析统计
            OracleHelper.Query<TotalPlanData>("SELECT substr(CREATETIME,1,10) AS RiQi ,SUM(GOODSNUMBER) AS TotalNum, SUM(LEVELNUMBER) AS SurplusNum from BILL_GOODSOUT t  GROUP BY  substr(CREATETIME,1,10)  ORDER BY substr(CREATETIME,1,10) ", out List<TotalPlanData> data6);

            //每天退货总单计划数据分析统计
            OracleHelper.Query<TotalPlanData>("SELECT substr(CREATETIME,1,10) AS RiQi ,SUM(GOODSNUMBER) AS TotalNum, SUM(LEVELNUMBER) AS SurplusNum from BILL_GOODSREFUND t  GROUP BY  substr(CREATETIME,1,10)  ORDER BY substr(CREATETIME,1,10) ", out List<TotalPlanData> data7);


            Assert.IsNotNull(data1);
            Assert.IsNotNull(data2);
            Assert.IsNotNull(data3);
            Assert.IsNotNull(data4);
            Assert.IsNotNull(data5);
            Assert.IsNotNull(data6);
            Assert.IsNotNull(data7);
        }


        [TestMethod]
        public void Test6()
        {
         
            //测试 新增门禁日志
            GateLog gatelog = new GateLog
            {
                Tractorid = "闽B123456",
                Cartype = "1",
                Operation = "http://192.168.1.160:80/openapi/service/pms/vehicle/getDefaultUserUuid?token=96157488E7D3D6BF26D2D241F313E4B3",
                Rspcode = "6",
                Reqparameter = "appkey:848c11a2,opUserUuid:cc78be40ec8611e78168af26905e6f0f,pageNo:1,pageSize:10,plateN:闽A5H393,time:1592159591331}",
                Rspmessage = "errorCode:0,errorMessage:获取车辆信息成功,data:total:1,pageNo:1,pageSize:10,list:carUuid:23cb2c99bce0471cb23456113c1e6b73,plateNo:闽A5H393,plateType:0,plateColor:0,carType:2,ownerId:null,carColor:1,personName:null"
            };

            GateLogSrvc.AddOrUpdate(gatelog, "add");


        }


    }

    [TestClass]
    public class UnitTestWeigh
    {
        [TestMethod]
        public void TestMethod1()
        {
            /*List<string> slist = new List<string>();

            slist = null;

            int num = slist?.Count ??0;

            if (1 <= (slist?.Count ??0))
            {
                slist = null;
            }
            var dpext = new Yongrong.Model.Int.Weigh.DataParamExt()
            {
                Driver = "王陆",
                Endtime = DateTime.Now.AddMinutes(10).ToString("yyyy-MM-dd HH:mm:ss"),
                Msg_id = DateTime.Now.Ticks.ToString(),
                Order_id = "abc123123",
                Order_type = "送货",
                Starttime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                Tractor_id = "闽B88888",
                Trailer_id = "闽B77777",
                Weigh_time = "1"
            };

            dpext.Sign = BMD5.ToMD5String(dpext.Order_id + dpext.Msg_id + dpext.Starttime, 32);

            bool ret = WeighbridgeSrvc.BatchInsert(dpext);

            Assert.IsTrue(ret);*/
            string tel = "13606543784";
            string pwd = tel.Substring(5, 6);
            Console.WriteLine(pwd);

            var sign = BMD5.ToMD5String("138567372611590147666000" + BMD5.ToMD5String("737261", 32).ToUpper(), 32).ToUpper();
            Console.WriteLine(sign);
        }
    }
}
