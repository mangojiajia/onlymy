﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using Yongrong.Srvc;
using BaseS.String;
using BaseS.File.Log;
using BaseS.Collection;
using Yongrong.Model.Int;
using Yongrong.Int.BaseInfo;
using Yongrong.Int.Weighbridge;
using Yongrong.Int.Pda;
using Yongrong.Int.Phone;
using Yongrong.Srvc.Sys;
using Yongrong.Int.Sys;
using BaseS.Const;
using Yongrong.Srvc.Users;
using Yongrong.Int.Org;
using Yongrong.Model.Db;
using Yongrong.Int.IC;
using Yongrong.Int.Sum;

namespace Yongrong.Int
{
    class BaseInt : BaseSrvc
    {
        /// <summary>
        /// 接口层命令表
        /// </summary>
        protected static ConcurrentDictionary<string, Func<Dictionary<string, string>, byte[], byte[]>> intcmdTab
            = new ConcurrentDictionary<string, Func<Dictionary<string, string>, byte[], byte[]>>();

        public static ConcurrentDictionary<string, OrderGoods> dicOrderGoods = new ConcurrentDictionary<string, OrderGoods>();

        static BaseInt()
        {
            intcmdTab.TryAdd("weighresult", WeighbridgeInt.RecvWeigh);

            intcmdTab.TryAdd("userlogin", UserInt.Login);
            intcmdTab.TryAdd("userlogout", UserInt.Logout);
            intcmdTab.TryAdd("userpwdreset", UserInt.ResetPwd);

            intcmdTab.TryAdd("ic_queryorder", ICInt.QueryOrder);

            intcmdTab.TryAdd("pda_login", PdaInt.Login);
            intcmdTab.TryAdd("pda_logout", PdaInt.Logout);
            intcmdTab.TryAdd("pda_queryorder", PdaInt.QueryOrder);
            intcmdTab.TryAdd("pda_submitcheck", PdaInt.SubmitCheck);

            intcmdTab.TryAdd("phone_login", PhoneInt.Login);
            intcmdTab.TryAdd("phone_logout", PhoneInt.Logout);
            intcmdTab.TryAdd("phone_pwdreset", PhoneInt.ResetPwd);
            intcmdTab.TryAdd("phone_order", PhoneInt.Order);
            intcmdTab.TryAdd("phone_querybill", PhoneInt.QueryBill);
            intcmdTab.TryAdd("phone_queryorder", PhoneInt.QueryOrder);
            intcmdTab.TryAdd("phone_updorder", PhoneInt.UpdOrder);
            intcmdTab.TryAdd("phone_checkorder", PhoneInt.CheckOrder);

            intcmdTab.TryAdd("0101", BaseGoodsInt.Get);
            intcmdTab.TryAdd("0101addupd", BaseGoodsInt.AddUpd);
            intcmdTab.TryAdd("0101search", BaseGoodsInt.Search);


            intcmdTab.TryAdd("0102", BaseLoadingplaceInt.Get);
            intcmdTab.TryAdd("0102addupd", BaseLoadingplaceInt.AddUpd);

            intcmdTab.TryAdd("0103", BaseLogisticsgateInt.Get);
            intcmdTab.TryAdd("0103addupd", BaseLogisticsgateInt.AddUpd);

            intcmdTab.TryAdd("0104", BaseCustomInt.Get);
            intcmdTab.TryAdd("0104addupd", BaseCustomInt.AddUpd);
            intcmdTab.TryAdd("0104search", BaseCustomInt.Search);

            intcmdTab.TryAdd("0105",BaseSupplierInt.Get);
            intcmdTab.TryAdd("0105addupd", BaseSupplierInt.AddUpd);
            intcmdTab.TryAdd("0105search", BaseSupplierInt.Search);

            intcmdTab.TryAdd("0106", BaseTransportInt.Get);
            intcmdTab.TryAdd("0106addupd", BaseTransportInt.AddUpd);
            intcmdTab.TryAdd("0106searchinfo", BaseTransportInt.SearchInfo);
            intcmdTab.TryAdd("0106search", BaseTransportInt.Search);


            intcmdTab.TryAdd("0107", BaseTractorInt.Get);
            intcmdTab.TryAdd("0107addupd", BaseTractorInt.AddUpd);
            intcmdTab.TryAdd("0107upload", BaseTractorInt.Upload);

            intcmdTab.TryAdd("0108", BaseTrailerInt.Get);
            intcmdTab.TryAdd("0108addupd", BaseTrailerInt.AddUpd);
            intcmdTab.TryAdd("0108upload", BaseTrailerInt.Upload);

            intcmdTab.TryAdd("0109", BaseDriverInt.Get);
            intcmdTab.TryAdd("0109addupd", BaseDriverInt.AddUpd);
            intcmdTab.TryAdd("0109upload", BaseDriverInt.Upload);

            intcmdTab.TryAdd("0110", BaseSupercargoInt.Get);
            intcmdTab.TryAdd("0110addupd", BaseSupercargoInt.AddUpd);
            intcmdTab.TryAdd("0110upload", BaseSupercargoInt.Upload);

            intcmdTab.TryAdd("0201", BillGoodsInInt.Get);
            intcmdTab.TryAdd("0201addupd", BillGoodsInInt.AddUpd);

            intcmdTab.TryAdd("0202", OrderGoodsInt.Get);
            intcmdTab.TryAdd("0202addupd", OrderGoodsInt.AddUpd);

            intcmdTab.TryAdd("0203", BillGoodsOutInt.Get);
            intcmdTab.TryAdd("0203addupd", BillGoodsOutInt.AddUpd);

            intcmdTab.TryAdd("0204", OrderGoodsOutInt.Get);
            intcmdTab.TryAdd("0204addupd", OrderGoodsInt.AddUpd);

            intcmdTab.TryAdd("0205", BillGoodsRefundInt.Get);
            intcmdTab.TryAdd("0205addupd", BillGoodsRefundInt.AddUpd);

            intcmdTab.TryAdd("0206", OrderGoodsRefundInt.Get);
            intcmdTab.TryAdd("0206addupd", OrderGoodsInt.AddUpd);

            intcmdTab.TryAdd("0301", OrderGoodsInt.Get);
            //intcmdTab.TryAdd("0301addupd", OrderInfoInt.AddUpd);

            intcmdTab.TryAdd("0302", SafeCheckInt.Get);
            intcmdTab.TryAdd("0302addupd", SafeCheckInt.AddUpd);
            intcmdTab.TryAdd("0302queryorder", OrderGoodsInt.QueryOrder);


            intcmdTab.TryAdd("0401", OrderGoodsInt.Get);
            intcmdTab.TryAdd("0401addupd", OrderGoodsInt.AddUpd);
            intcmdTab.TryAdd("0402", OrderGoodsInt.Get);
            intcmdTab.TryAdd("0402addupd", OrderGoodsInt.AddUpd);
            intcmdTab.TryAdd("0403", OrderGoodsInt.Get);
            intcmdTab.TryAdd("0403addupd", OrderGoodsInt.AddUpd);

            intcmdTab.TryAdd("0501", OrderConfigInt.Get);
            intcmdTab.TryAdd("0501addupd", OrderConfigInt.AddUpd);
            intcmdTab.TryAdd("0502", OrderConfigInt.Get);
            intcmdTab.TryAdd("0502addupd", OrderConfigInt.AddUpd);
            intcmdTab.TryAdd("0503", OrderConfigInt.Get);
            intcmdTab.TryAdd("0503addupd", OrderConfigInt.AddUpd);

            intcmdTab.TryAdd("0601", OrderGoodsInt.Get);
            intcmdTab.TryAdd("0601output", OrderGoodsInt.Output);
            intcmdTab.TryAdd("0602", GateInt.Get);
            intcmdTab.TryAdd("0602output", GateInt.Output);
            intcmdTab.TryAdd("0603", OrderGoodsInt.Get);
            intcmdTab.TryAdd("0604", OrderGoodsInt.Get);
            intcmdTab.TryAdd("0605", ReportInt.GetWeighingSheet);
            intcmdTab.TryAdd("0606", ReportInt.GetCustomerPickUp);
            intcmdTab.TryAdd("0607", ReportInt.GetSupplierPickUp);


            intcmdTab.TryAdd("0701", RoleInt.Get);
            intcmdTab.TryAdd("0701addupd", RoleInt.AddUpd);
            intcmdTab.TryAdd("0701del", RoleInt.Del);

            intcmdTab.TryAdd("0702", UserPermissionInt.Get);
            intcmdTab.TryAdd("0702addupd", UserPermissionInt.AddUpd);
            intcmdTab.TryAdd("0702del", UserPermissionInt.Del);

            intcmdTab.TryAdd("0704", UserOpLogInt.Get);
            intcmdTab.TryAdd("0704addupd", UserOpLogInt.AddUpd);

            intcmdTab.TryAdd("0703", AbnormalInt.Get);

            intcmdTab.TryAdd("0705", SysConfigInt.Get);
            intcmdTab.TryAdd("0705upd", SysConfigInt.AddUpd);

        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name=""></param>
        /// <param name="reqbytes"></param>
        /// <returns></returns>
        protected static byte[] ExecInt(string cmd, Dictionary<string, string> urlMap, byte[] reqbytes)
        {
            string reqStr = reqbytes.B2String();

            // reqStr.Debug(string.Empty, Enter);
            /*
            if (string.IsNullOrWhiteSpace(reqStr))
            {
                "请求包体内容为空".Warn();
                return new BaseRsp() { Stat = "无效请求" }.ToBytes();
            }
            */
            if (reqStr.IsBlockStr(false))
            {
                "恶意注入代码".Warn();
                return new BaseRsp() { Stat = "无效请求" }.ToBytes();
            }

            if (string.IsNullOrWhiteSpace(cmd))
            {
                "cmd为空,无效的命令字".Notice();
                return new BaseRsp() { Stat = "cmd为空,无效的命令字" }.ToBytes();
            }

            if (null != Sys.BlockIp && 1 <= Sys.BlockIp.Count)
            {
                urlMap.TryGetValue(Http.ClientIPName, out var ip);

                if (Sys.BlockIp.Contains(ip))
                {
                    return new BaseRsp() { Stat = "ip黑名单" }.ToBytes();
                }
            }

            // 找到命令字对应的方法
            intcmdTab.TryGetValue(cmd, out var func);

            if (null == func)
            {
                $"执行方法没有找到,命令字:{cmd}".Notice(string.Empty, Exit);
                return new BaseRsp() { Stat = "cmd error" }.ToBytes();
            }

            byte[] rspbytes = null;

            $"执行方法:{cmd}".Info();

            try
            {
                rspbytes = func(urlMap, reqbytes);
            }
            catch (Exception e)
            {
                $"方法:{cmd} Err:{e.Message} \n StackTrace:{e.StackTrace} \n InnerException:{e.InnerException?.Message}".Warn(string.Empty, Exit);

                return new BaseRsp() { Stat = "未知错误" }.ToBytes();
            }
            finally
            {
                UserOpLogSrvc.AddOrUpdate(new Model.Db.UserOplog()
                {
                    Detail = urlMap.GetDicStr(OP_Detail),
                    Userid = urlMap.GetDicStr(OP_User, ""),
                    UseridDest = urlMap.GetDicStr(OP_UserDest),
                    Createtime = DateTime.Now.ToString(BTip.TimeFormater),
                    Cmd = urlMap.GetDicStr(Http.CmdName),
                    Opcontent = urlMap.GetDicStr(OP_Content),
                    Clientip = urlMap.GetDicStr(Http.ClientIPName)
                }, ADD) ; 
            }

            return rspbytes;
        }
    }
}
