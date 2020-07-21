﻿using System;
using System.Collections.Generic;
using System.Text;
using Yongrong.Model.Int.Phone;
using BaseS.Serialization;
using Yongrong.Srvc.Users;
using BaseS.Security;
using BaseS.Const;
using BaseS.Collection;
using Yongrong.Srvc.BaseInfo;
using System.Linq;
using Yongrong.Model.Db;
using BaseS.String;
using BaseS.File.Log;
using Yongrong.Model.Int;
using Yongrong.Srvc.Sys;
using System.Text.RegularExpressions;
using Yongrong.Model.Base;
using Yongrong.Srvc.Push;
using Yongrong.Model.Conf;
using Yongrong.Db;

namespace Yongrong.Int.Phone
{
    class PhoneInt : BaseInt
    {
        /// <summary>
        /// 手机端登录
        /// </summary>
        /// <param name="reqmap"></param>
        /// <param name="reqbytes"></param>
        /// <returns></returns>
        public static byte[] Login(Dictionary<string, string> reqmap, byte[] reqbytes)
        {
            PhoneLoginRsp rsp = new PhoneLoginRsp() { Stat = Success };

            reqbytes.Json2ObjT<PhoneLoginReq>(out PhoneLoginReq req);
            //进日志
            req.Debug(string.Empty, Enter);

            reqmap.TryAdd("Access-Control-Allow-Origin", "*");

            if (null == req)
            {
                rsp.Stat = JsonErr;
                reqmap.TryAdd(OP_Content, rsp.Stat);
                reqmap.TryAdd(OP_Detail, reqbytes.B2String());
                return rsp.ToBytes();
            }

            if (!UserSrvc.Login(new Model.Db.User() { Userid = req.Uid, Token = req.Token }, out var user))
            {
                rsp.Stat = "用户不存在";

                if (BMobile.IsAllPhone(req.Uid))
                {
                    Abnormal abnormal = new Abnormal
                    {
                        Abnormaltype = "司机登录失败",
                        Abnormalname = req.Uid,
                        Abnormalcase = $"司机登录时,显示用户:{req.Uid}不存在",
                        Createuser = req.Uid
                    };

                    AbnormalSrvc.AddOrUpdate(abnormal, ADD);
                }

                reqmap.TryAdd(OP_Content, rsp.Stat);
                reqmap.TryAdd(OP_Detail, reqbytes.B2String());
                return rsp.ToBytes();
            }

            var sign = BMD5.ToMD5String($"{req.Uid}{req.Time}{user.Pwd}", 32).ToUpper();

            if (!req.Sign.Equals(sign))
            {
                rsp.Stat = "密码错误";

                if (BMobile.IsAllPhone(req.Uid))
                {
                    Abnormal abnormal = new Abnormal
                    {
                        Abnormaltype = "司机登录失败",
                        Abnormalname = req.Uid,
                        Abnormalcase = $"司机:{req.Uid}登录时,显示密码错误",
                        Createuser = user.Username
                    };

                    AbnormalSrvc.AddOrUpdate(abnormal, ADD);
                }

                reqmap.TryAdd(OP_Content, rsp.Stat);
                reqmap.TryAdd(OP_Detail, reqbytes.B2String());
                return rsp.ToBytes();
            }

            rsp.Token = user.Token;
            rsp.Expire = DateTime.Now.AddDays(1.0).ToString(BTip.TimeFormater);
            rsp.Roleid = user.Roleids;
            rsp.Id = user.Id;

            reqmap.TryAdd(OP_User, user?.Userid);
            reqmap.TryAdd(OP_Content, "手机登录");

            return rsp.ToBytes();
        }

        /// <summary>
        /// 手机端注销
        /// </summary>
        /// <param name="reqmap"></param>
        /// <param name="reqbytes"></param>
        /// <returns></returns>
        public static byte[] Logout(Dictionary<string, string> reqmap, byte[] reqbytes)
        {
            PhoneLoginRsp rsp = new PhoneLoginRsp() { Stat = Success };

            reqbytes.Json2ObjT<PhoneLoginReq>(out PhoneLoginReq req);
            //进日志
            req.Debug(string.Empty, Enter);
            if (null == req)
            {
                rsp.Stat = JsonErr;
                reqmap.TryAdd(OP_Content, rsp.Stat);
                reqmap.TryAdd(OP_Detail, reqbytes.B2String());
                return rsp.ToBytes();
            }

            if (!UserSrvc.GetOne(new Model.Db.User() { Userid = req.Uid, Token = req.Token }, out var user))
            {
                rsp.Stat = "用户不存在";
                reqmap.TryAdd(OP_Content, rsp.Stat);
                reqmap.TryAdd(OP_Detail, reqbytes.B2String());
                return rsp.ToBytes();
            }

            var sign = BMD5.ToMD5String($"{req.Uid}{req.Time}{user.Pwd}", 32).ToUpper();

            if (!req.Sign.Equals(sign))
            {
                rsp.Stat = "签名错误";
                reqmap.TryAdd(OP_Content, rsp.Stat);
                reqmap.TryAdd(OP_Detail, reqbytes.B2String());
                return rsp.ToBytes();
            }

            user.Token = string.Empty;

            UserSrvc.AddOrUpdate(user, UPD);

            rsp.Expire = DateTime.Now.AddDays(1.0).ToString(BTip.TimeFormater);

            reqmap.TryAdd(OP_User, user?.Userid);
            reqmap.TryAdd(OP_Content, "手机退出登录");

            return rsp.ToBytes();
        }

        /// <summary>
        /// 重置密码
        /// </summary>
        /// <param name="reqmap"></param>
        /// <param name="reqbytes"></param>
        /// <returns></returns>
        public static byte[] ResetPwd(Dictionary<string, string> reqmap, byte[] reqbytes)
        {
            PhoneResetPwdRsp rsp = new PhoneResetPwdRsp() { Stat = Success };

            reqbytes.Json2ObjT<PhoneResetPwdReq>(out PhoneResetPwdReq req);
            //进日志
            req.Debug(string.Empty, Enter);

            if (reqmap.ContainsKey(TOKEN))
            {
                req.Token = reqmap.GetDicStr(TOKEN);
            }

            if (null == req)
            {
                rsp.Stat = JsonErr;
                reqmap.TryAdd(OP_Content, rsp.Stat);
                reqmap.TryAdd(OP_Detail, reqbytes.B2String());
                return rsp.ToBytes();
            }

            if (!UserSrvc.GetOne(new Model.Db.User() { Token = req.Token }, out var user))
            {
                rsp.Stat = "用户不存在";
                reqmap.TryAdd(OP_Content, rsp.Stat);
                reqmap.TryAdd(OP_Detail, reqbytes.B2String());
                return rsp.ToBytes();
            }

            var sign = BMD5.ToMD5String($"{req.Time}{req.Phone}{user.Pwd}", 32).ToUpper();

            if (!req.Sign.Equals(sign))
            {
                rsp.Stat = "签名错误";
                reqmap.TryAdd(OP_Content, rsp.Stat);
                reqmap.TryAdd(OP_Detail, reqbytes.B2String());
                return rsp.ToBytes();
            }

            /*if (!req.Phone.Equals(user.Phone))
            {
                rsp.Stat = "手机号码不符";
                reqmap.TryAdd(OP_Content, rsp.Stat);
                reqmap.TryAdd(OP_Detail, reqbytes.B2String());
                return rsp.ToBytes();
            }*/

            if(!user.Pwd.Equals(req.OldPwd))
            {
                rsp.Stat = "旧密码错误";
                return rsp.ToBytes();
            }

            user.Pwd = req.NewPwd;
            UserSrvc.AddOrUpdate(user, UPD);

            reqmap.TryAdd(OP_User, user?.Userid);
            reqmap.TryAdd(OP_Content, "手机重置密码");

            return rsp.ToBytes();
        }

        /// <summary>
        /// 查询可预约的总单
        /// 分别返回可预约的卸车，装车，退货总单
        /// </summary>
        /// <param name="reqmap"></param>
        /// <param name="reqbytes"></param>
        /// <returns></returns>
        public static byte[] QueryBill(Dictionary<string, string> reqmap, byte[] reqbytes)
        {
            PhoneQueryBillRsp rsp = new PhoneQueryBillRsp();
            reqbytes.Json2ObjT<PhoneQueryBillReq>(out PhoneQueryBillReq req);
            //进日志
            req.Debug(string.Empty, Enter);

            if (reqmap.ContainsKey(TOKEN))
            {
                req.Token = reqmap.GetDicStr(TOKEN);
            }

            if (null == req)
            {
                rsp.Stat = JsonErr;
                reqmap.TryAdd(OP_Content, rsp.Stat);
                reqmap.TryAdd(OP_Detail, reqbytes.B2String());
                return rsp.ToBytes();
            }

            if (!UserSrvc.GetOne(new Model.Db.User() { Token = req.Token }, out var user))
            {
                rsp.Stat = "用户不存在";
                reqmap.TryAdd(OP_Content, rsp.Stat);
                reqmap.TryAdd(OP_Detail, reqbytes.B2String());
                return rsp.ToBytes();
            }

            if(!"driver".Equals(user.Roleids) && !ConstRole.SIJI.Equals(user.Roleids) && !ConstRole.DRIVER.Equals(user.Roleids))
            {
                rsp.Stat = "不是司机，无法申请";
                reqmap.TryAdd(OP_Content, rsp.Stat);
                reqmap.TryAdd(OP_Detail, reqbytes.B2String());
                return rsp.ToBytes();
            }

            BaseDriverSrvc.Get(new Model.Db.BaseDriver() { Tel = user.Phone, Name = user.Username }, new Model.Int.PageBean() { Index = 1, Size = 1 }, out List<BaseDriver> driverList);
            if(null == driverList || driverList.Count < 1)
            {
                rsp.Stat = "司机不存在";
                reqmap.TryAdd(OP_Content, rsp.Stat);
                reqmap.TryAdd(OP_Detail, reqbytes.B2String());
                return rsp.ToBytes();
            }

            BaseDriver baseDriver = null;
            baseDriver = driverList[0];

            rsp.Driver = baseDriver;


            BillGoodsInSrvc.Get(new BillGoodsIn() { Drivers = baseDriver.Name, Billstat = "进厂计划" }, new Model.Int.PageBean() { Index = 1, Size = 1000 }, out var temp);
            if(null != temp && temp.Count > 0)
            {
                rsp.GoodsInList.AddRange(temp.Where(a => DateTime.Now.ToString(BTip.DateFormater).CompareTo(a.Starttime) >=
                0 && DateTime.Now.ToString(BTip.DateFormater).CompareTo(a.Endtime) <= 0 && a.Levelnumber > 0));
            }
            
            
            BillGoodsOutSrvc.Get(new BillGoodsOut() { Drivers = baseDriver.Name, Billstat = "出厂计划" }, new Model.Int.PageBean() { Index = 1, Size = 1000 }, out var temp2);
            if (temp2 != null && temp2.Count > 0)
            {
                rsp.GoodsOutList.AddRange(temp2.Where(a => DateTime.Now.ToString(BTip.DateFormater).CompareTo(a.Starttime) >=
                0 && DateTime.Now.ToString(BTip.DateFormater).CompareTo(a.Endtime) <= 0 && a.Levelnumber > 0));
            }
            
            BillGoodsRefundSrvc.Get(new BillGoodsRefund() { Drivers = baseDriver.Name, Billstat = "退货" }, new Model.Int.PageBean() { Index = 1, Size = 1000 }, out var temp3);
            if (null != temp3 && temp3.Count>0)
            {
                rsp.GoodsRefundList.AddRange(temp3.Where(a => DateTime.Now.ToString(BTip.DateFormater).CompareTo(a.Starttime) >= 0
                && DateTime.Now.ToString(BTip.DateFormater).CompareTo(a.Endtime) <= 0 && a.Levelnumber > 0));
            }

            BaseTrailerSrvc.Get(new BaseTrailer() { TrailerId = baseDriver.TrailerId }, new PageBean() { Index = 1, Size = 1 }, out var baseTrailers);
            rsp.Trailer = baseTrailers?.FirstOrDefault();

            BaseSupercargoSrvc.Get(new BaseSupercargo() { Driver = baseDriver.Name }, new PageBean() { Index = 1, Size = 1 }, out var baseSupercargos);
            if(null != baseSupercargos)
            {
                rsp.Supercargo = baseSupercargos.FirstOrDefault();
            }

            rsp.Stat = Success;
            reqmap.TryAdd(OP_User, user?.Userid);
            reqmap.TryAdd(OP_Content, "手机查询可预约总单");

            return rsp.ToBytes();
        }

        /// <summary>
        /// 司机提交预约
        /// </summary>
        /// <param name="reqmap"></param>
        /// <param name="reqbytes"></param>
        /// <returns></returns>
        public static byte[] Order(Dictionary<string, string> reqmap, byte[] reqbytes)
        {
            PhoneOrderRsp rsp = new PhoneOrderRsp() { Stat = Success };

            reqbytes.Json2ObjT<PhoneOrderReq>(out PhoneOrderReq req);
            //进日志
            req.Debug(string.Empty, Enter);

            if (reqmap.ContainsKey(TOKEN))
            {
                req.Token = reqmap.GetDicStr(TOKEN);
            }

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
                reqmap.TryAdd(OP_Content, rsp.Stat);
                reqmap.TryAdd(OP_Detail, reqbytes.B2String());
                return rsp.ToBytes();
            }

            var sign = BMD5.ToMD5String($"{user.Userid}{req.Time}{user.Pwd}", 32).ToUpper();

            if (!req.Sign.Equals(sign))
            {
                rsp.Stat = "签名错误";
                reqmap.TryAdd(OP_Content, rsp.Stat);
                reqmap.TryAdd(OP_Detail, reqbytes.B2String());
                return rsp.ToBytes();
            }

            if (!ConstRole.EDRIVER.Equals(user.Roleids) && !ConstRole.SIJI.Equals(user.Roleids) && !ConstRole.DRIVER.Equals(user.Roleids))
            {
                rsp.Stat = "不是司机，无法申请";
                reqmap.TryAdd(OP_Content, rsp.Stat);
                reqmap.TryAdd(OP_Detail, reqbytes.B2String());
                return rsp.ToBytes();
            }

            BaseDriverSrvc.Get(new Model.Db.BaseDriver() { Tel = user.Phone, Name = user.Username }, new Model.Int.PageBean() { Index = 1, Size = 1 }, out List<BaseDriver> driverList);

            if (null == driverList || driverList.Count < 1)
            {
                rsp.Stat = "司机不存在";

                AbnormalSrvc.AddOrUpdate(new Abnormal
                {
                    Abnormaltype = "司机预约失败",
                    Abnormalname = user.Phone,
                    Abnormalcase = $"司机:{user.Phone}提交预约时,显示司机不存在",
                    Createuser = user.Username
                }, ADD);

                reqmap.TryAdd(OP_Content, rsp.Stat);
                reqmap.TryAdd(OP_Detail, reqbytes.B2String());
                return rsp.ToBytes();
            }

            //driverList[0].Debug("司机信息显示");
            if (null != driverList[0].Whiteflag && "-1".Equals(driverList[0].Whiteflag))
            {
                rsp.Stat = "司机黑名单，预约失败";

                Abnormal abnormal = new Abnormal
                {
                    Abnormaltype = "司机预约失败",
                    Abnormalname = user.Phone,
                    Abnormalcase = $"司机:{user.Phone}黑名单，预约失败",
                    Createuser = user.Username
                };

                AbnormalSrvc.AddOrUpdate(abnormal, ADD);

                return rsp.ToBytes();
            }

            rsp.Stat = OrderGoodsSrvc.SubBill(req.AddObj);
            rsp.OrderCode = req.AddObj.Orderid;

            if (!Success.Equals(rsp.Stat))
            {
                return rsp.ToBytes();
            }

            if (Sys.JPushFlag) 
            {
                //如果为0，需要推送给永荣审核
                if ("0".Equals(req.AddObj.Billcheck))
                {
                    //UserSrvc.Get(new User() { Username = req.AddObj.Ordername }, out var userlist);

                    OracleHelper.Query<User>($"SELECT UserId from USER_LOGIN t WHERE UserName='{req.AddObj.Ordername}' AND ROLEIDS NOT IN( '驾驶员','司机','driver')", out var userlist);
                    
                    JPushSrvc.SendPush($"你有一条司机{req.AddObj.GetBillType.Replace("进厂", "送货").Replace("出厂", "提货").Trim()}待审核的预约，预约码为{req.AddObj.Orderid}", userlist?.Select(a => a.Userid));
                }

                // 推送给客商
                if ("0".Equals(req.AddObj.Customercheck))
                {
                    JPushSrvc.SendPush($"你有一条司机{req.AddObj.GetBillType.Replace("进厂", "送货").Replace("出厂", "提货").Trim()}待审核的预约，预约码为{req.AddObj.Orderid}", new List<string>() { req.AddObj.Company });
                }
            }

            rsp.Stat = Success;

            reqmap.TryAdd(OP_User, user?.Userid);
            reqmap.TryAdd(OP_Content, "提交预约");

            return rsp.ToBytes();
        }

        /// <summary>
        /// 判断进厂预约是否满足条件
        /// </summary>
        /// <param name="rsp"></param>
        /// <param name="req"></param>
        /// <returns></returns>
        private static bool CheckOrderGoodsIn(ref PhoneOrderRsp rsp, PhoneOrderReq req)
        {
            //进日志
            req.Debug(string.Empty, Enter);

            if (!BillGoodsInSrvc.GetOne(new Model.Db.BillGoodsIn() { Billid = req.AddObj.Billid }, out var billGoodsIn))
            {
                rsp.Stat = "总单不存在";
                return false;
            }
            if (0 == billGoodsIn.Goodsnumber)
            {
                rsp.Stat = "总提量为0";
                return false;
            }
            if (0 == billGoodsIn.Levelnumber)
            {
                rsp.Stat = "剩余量为0";
                return false;
            }
            if (billGoodsIn.Levelnumber < Convert.ToDecimal(req.AddObj.Realweight))
            {
                rsp.Stat = "预约量超过剩余量";
                return false;
            }
            //预约表里没数据的话会报错
            int orderNumber=0;
            int maxNumber = 0;
            orderNumber = OrderGoodsSrvc.CountOrderGoods(new Model.Db.OrderGoods() { Billid = req.AddObj.Billid });
            if (orderNumber>0)
            {
                maxNumber = OrderConfigSrvc.GetMaxNumber(new Model.Db.OrderConfig() { Goodsname = billGoodsIn.Goodsname, Unloadarea = "卸货区" });
                if (0 == maxNumber)
                {
                    rsp.Stat = "储运调度管理里未设置‘当日预约车辆总数/大货车数量限制’";
                    return false;
                }
                if (orderNumber >= maxNumber)
                {
                    rsp.Stat = "车辆预约数已经达到最大";
                    return false;
                }
            }
            if(string.IsNullOrWhiteSpace(billGoodsIn.Drivers) || !billGoodsIn.Drivers.Contains(req.AddObj.Driver))
            {
                rsp.Stat = "预约的司机不在总单里";
                return false;
            }
            if(string.IsNullOrWhiteSpace(billGoodsIn.Tractorids) || !billGoodsIn.Tractorids.Contains(req.AddObj.Tractorid))
            {
                rsp.Stat = "预约的牵引车车牌号不在总单里";
                return false;
            }
            if(string.IsNullOrWhiteSpace(billGoodsIn.Trailerids) || !billGoodsIn.Trailerids.Contains(req.AddObj.Trailerid))
            {
                rsp.Stat = "预约的挂车车牌号不在总单里";
                return false;
            }
            /*if (string.IsNullOrWhiteSpace(billGoodsIn.Supercargo) || !billGoodsIn.Supercargo.Contains(req.AddObj.Supercargo))
            {
                rsp.Stat = "预约的押运员不在总单里";
                return false;
            }*/

            req.AddObj.Orderid = OrderGoodsSrvc.CreateOrderId().ToString("0000");
            req.AddObj.Ordername = billGoodsIn.Maker;
            req.AddObj.Company = billGoodsIn.Company;
            req.AddObj.Goodsid = billGoodsIn.Goodsid;
            req.AddObj.Goodsname = billGoodsIn.Goodsname;
            req.AddObj.Istoexit = "1";
            req.AddObj.Issendback = "1";
            req.AddObj.Waybillid = DateTime.Now.ToString("yyyyMMdd") + req.AddObj.Orderid;
            req.AddObj.Printid = DateTime.Now.ToString("yyyyMMdd") + req.AddObj.Orderid;
            req.AddObj.Billcheck = "0";
            req.AddObj.Customercheck = "0";
            if (string.IsNullOrWhiteSpace(billGoodsIn.Status))
            {
                billGoodsIn.Status = "11";
            }

            if("11".Equals(billGoodsIn.Status))
            {
                req.AddObj.Orderstat = "1";
                req.AddObj.Billcheck = "1";
                req.AddObj.Customercheck = "1";
            }
            else
            {
                req.AddObj.Orderstat = "0";

                if (2 == billGoodsIn.Status.Length)
                {
                    req.AddObj.Billcheck = billGoodsIn.Status.Substring(0, 1);
                    req.AddObj.Customercheck = billGoodsIn.Status.Substring(1, 1);
                }
            }

            OrderGoodsSrvc.AddOrUpdate(req.AddObj, ADD, string.Empty);

            billGoodsIn.Levelnumber = billGoodsIn.Levelnumber - Convert.ToDecimal(req.AddObj.Realweight);
            BillGoodsInSrvc.AddOrUpdate(billGoodsIn, UPD);

            rsp.OrderCode = req.AddObj.Orderid;
            return true;
        }

        /// <summary>
        /// 判断出厂预约是否满足条件
        /// </summary>
        /// <param name="rsp"></param>
        /// <param name="req"></param>
        /// <returns></returns>
        private static bool CheckOrderGoodsOut(ref PhoneOrderRsp rsp, PhoneOrderReq req)
        {
            //进日志
            req.Debug(string.Empty, Enter);

            if (!BillGoodsOutSrvc.GetOne(new Model.Db.BillGoodsOut() { Billid = req.AddObj.Billid }, out var billGoodsOut))
            {
                rsp.Stat = "总单不存在";
                return false;
            }
            if (0 == billGoodsOut.Goodsnumber)
            {
                rsp.Stat = "总提量为0";
                return false;
            }
            if (0 == billGoodsOut.Levelnumber)
            {
                rsp.Stat = "剩余量为0";
                return false;
            }
            if (billGoodsOut.Levelnumber < Convert.ToDecimal(req.AddObj.Realweight))
            {
                rsp.Stat = "预约量超过剩余量";
                return false;
            }
            int orderNumber = OrderGoodsSrvc.CountOrderGoods(new Model.Db.OrderGoods() { Billid = req.AddObj.Billid });
            int maxNumber = OrderConfigSrvc.GetMaxNumber(new Model.Db.OrderConfig() { Goodsname = billGoodsOut.Goodsname, Loadarea = "装货区" });
            if (0 == maxNumber)
            {
                rsp.Stat = "储运调度管理里未设置‘当日预约车辆总数/大货车数量限制’";
                return false;
            }
            if (orderNumber >= maxNumber)
            {
                rsp.Stat = "车辆预约数已经达到最大";
                return false;
            }
            req.AddObj.Orderid = OrderGoodsSrvc.CreateOrderId().ToString("0000");
            req.AddObj.Ordername = billGoodsOut.Salesman;
            req.AddObj.Company = billGoodsOut.Company;
            req.AddObj.Goodsid = billGoodsOut.Goodsid;
            req.AddObj.Goodsname = billGoodsOut.Goodsname;
            req.AddObj.Istoexit = "1";
            req.AddObj.Issendback = "2";
            req.AddObj.Waybillid = DateTime.Now.ToString("yyyyMMdd") + req.AddObj.Orderid;
            req.AddObj.Printid = DateTime.Now.ToString("yyyyMMdd") + req.AddObj.Orderid;
            req.AddObj.Billcheck = "0";
            req.AddObj.Customercheck = "0";
            if (string.IsNullOrWhiteSpace(billGoodsOut.Status))
            {
                billGoodsOut.Status = "11";
            }
            if ("11".Equals(billGoodsOut.Status))
            {
                req.AddObj.Orderstat = "1";
                req.AddObj.Billcheck = "1";
                req.AddObj.Customercheck = "1";
            }
            else
            {
                req.AddObj.Orderstat = "0";
                if (2 == billGoodsOut.Status.Length)
                {
                    req.AddObj.Billcheck = billGoodsOut.Status.Substring(0, 1);
                    req.AddObj.Customercheck = billGoodsOut.Status.Substring(1, 1);
                }
            }

            OrderGoodsSrvc.AddOrUpdate(req.AddObj, ADD, string.Empty);

            billGoodsOut.Levelnumber = billGoodsOut.Levelnumber - Convert.ToDecimal(req.AddObj.Realweight);
            BillGoodsOutSrvc.AddOrUpdate(billGoodsOut, UPD);

            rsp.OrderCode = req.AddObj.Orderid;
            return true;
        }

        /// <summary>
        /// 判断退货预约是否满足条件
        /// </summary>
        /// <param name="rsp"></param>
        /// <param name="req"></param>
        /// <returns></returns>
        private static bool CheckOrderGoodsRefund(ref PhoneOrderRsp rsp, PhoneOrderReq req)
        {
            //进日志
            req.Debug(string.Empty, Enter);

            if (!BillGoodsRefundSrvc.GetOne(new Model.Db.BillGoodsRefund() { Billid = req.AddObj.Billid }, out var billGoodsRefund))
            {
                rsp.Stat = "总单不存在";
                return false;
            }
            if (0 == billGoodsRefund.Goodsnumber)
            {
                rsp.Stat = "总提量为0";
                return false;
            }
            if (0 == billGoodsRefund.Levelnumber)
            {
                rsp.Stat = "剩余量为0";
                return false;
            }
            if (billGoodsRefund.Levelnumber < Convert.ToDecimal(req.AddObj.Realweight))
            {
                rsp.Stat = "预约量超过剩余量";
                return false;
            }
            int orderNumber = OrderGoodsSrvc.CountOrderGoods(new Model.Db.OrderGoods() { Billid = req.AddObj.Billid });
            int maxNumber = OrderConfigSrvc.GetMaxNumber(new Model.Db.OrderConfig() { Goodsname = billGoodsRefund.Goodsname, Unloadarea = billGoodsRefund.Issendback.Equals("1") ? "卸货区" : "装货区" });
            if (0 == maxNumber)
            {
                rsp.Stat = "储运调度管理里未设置‘当日预约车辆总数/大货车数量限制’";
                return false;
            }
            if (orderNumber >= maxNumber)
            {
                rsp.Stat = "车辆预约数已经达到最大";
                return false;
            }
            req.AddObj.Orderid = OrderGoodsSrvc.CreateOrderId().ToString("0000");
            req.AddObj.Ordername = billGoodsRefund.Maker;
            req.AddObj.Company = billGoodsRefund.Company;
            req.AddObj.Goodsid = billGoodsRefund.Goodsid;
            req.AddObj.Goodsname = billGoodsRefund.Goodsname;
            req.AddObj.Istoexit = "2";
            req.AddObj.Issendback = billGoodsRefund.Issendback;
            req.AddObj.Waybillid = DateTime.Now.ToString("yyyyMMdd") + req.AddObj.Orderid;
            req.AddObj.Printid = DateTime.Now.ToString("yyyyMMdd") + req.AddObj.Orderid;
            req.AddObj.Billcheck = "0";
            req.AddObj.Customercheck = "0";
            if (string.IsNullOrWhiteSpace(billGoodsRefund.Status))
            {
                billGoodsRefund.Status = "11";
            }
            if ("11".Equals(billGoodsRefund.Status))
            {
                req.AddObj.Orderstat = "1";
                req.AddObj.Billcheck = "1";
                req.AddObj.Customercheck = "1";
            }
            else
            {
                req.AddObj.Orderstat = "0";
                if (2 == billGoodsRefund.Status.Length)
                {
                    req.AddObj.Billcheck = billGoodsRefund.Status.Substring(0, 1);
                    req.AddObj.Customercheck = billGoodsRefund.Status.Substring(1, 1);
                }
            }

            OrderGoodsSrvc.AddOrUpdate(req.AddObj, ADD, string.Empty);

            billGoodsRefund.Levelnumber = billGoodsRefund.Levelnumber - Convert.ToDecimal(req.AddObj.Realweight);
            BillGoodsRefundSrvc.AddOrUpdate(billGoodsRefund, UPD);
            rsp.OrderCode = req.AddObj.Orderid;
            return true;
        }

        /// <summary>
        /// 查询预约
        /// </summary>
        /// <param name="reqmap"></param>
        /// <param name="reqbytes"></param>
        /// <returns></returns>
        public static byte[] QueryOrder(Dictionary<string, string> reqmap, byte[] reqbytes)
        {
            PhoneQueryOrderRsp rsp = new PhoneQueryOrderRsp() { Stat = Success };

            reqbytes.Json2ObjT<PhoneQueryOrderReq>(out PhoneQueryOrderReq req);

            //进日志
            req.Debug(string.Empty, Enter);

            if (reqmap.ContainsKey(TOKEN))
            {
                req.Token = reqmap.GetDicStr(TOKEN);
            }

            if (null == req)
            {
                rsp.Stat = JsonErr;
                reqmap.TryAdd(OP_Content, rsp.Stat);
                reqmap.TryAdd(OP_Detail, reqbytes.B2String());
                return rsp.ToBytes();
            }

            if (!UserSrvc.GetOne(new Model.Db.User() { Token = req.Token }, out var user))
            {
                rsp.Stat = "用户不存在";
                reqmap.TryAdd(OP_Content, rsp.Stat);
                reqmap.TryAdd(OP_Detail, reqbytes.B2String());
                return rsp.ToBytes();
            }

            var sign = BMD5.ToMD5String($"{req.Uid}{req.Time}{user.Pwd}", 32).ToUpper();

            if (!req.Sign.Equals(sign))
            {
                rsp.Stat = "签名错误";
                reqmap.TryAdd(OP_Content, rsp.Stat);
                reqmap.TryAdd(OP_Detail, reqbytes.B2String());
                return rsp.ToBytes();
            }

            // 默认客户登录,user的Roleids为客户的公司名称
            var query = new Model.Db.OrderGoods() {  Company = user.Userid };

            // 驾驶员模式
            if (ConstRole.DRIVER.Equals(user.Roleids) || "driver".Equals(user.Roleids))
            {
                BaseDriverSrvc.Get(new BaseDriver() { Name = user.Username,Tel=user.Phone }, null, out var drivers);
                query.Driver = drivers?.FirstOrDefault()?.Name;
                query.Company = string.Empty;

                if (string.IsNullOrWhiteSpace(query.Driver))
                {
                    rsp.Stat = "司机信息不存在";
                    reqmap.TryAdd(OP_Content, rsp.Stat);
                    reqmap.TryAdd(OP_Detail, reqbytes.B2String());
                    return rsp.ToBytes();
                }
            }
            
            if (ConstRole.SELLERPART.Equals(user.Roleids))
            {
                query.Company = string.Empty;
            }

       

            OrderGoodsSrvc.GetIphoneOrderData(query, req.Page, out var list);

            req.Page.CopyPage(rsp.Page);
            rsp.Data = list;
      
            reqmap.TryAdd(OP_User, user?.Userid);
            reqmap.TryAdd(OP_Content, "手机查询已预约信息");

            return rsp.ToBytes();
        }

        /// <summary>
        /// 更新预约 或者更改审批
        /// </summary>
        /// <param name="reqmap"></param>
        /// <param name="reqbytes"></param>
        /// <returns></returns>
        public static byte[] UpdOrder(Dictionary<string, string> reqmap, byte[] reqbytes)
        {
            PhoneUpdOrderRsp rsp = new PhoneUpdOrderRsp() { Stat = Success };

            reqbytes.Json2ObjT<PhoneUpdOrderReq>(out PhoneUpdOrderReq req);
            //进日志
            req.Debug(string.Empty, Enter);

            if (reqmap.ContainsKey(TOKEN))
            {
                req.Token = reqmap.GetDicStr(TOKEN);
            }

            if (null == req)
            {
                rsp.Stat = JsonErr;
                reqmap.TryAdd(OP_Content, rsp.Stat);
                reqmap.TryAdd(OP_Detail, reqbytes.B2String());
                return rsp.ToBytes();
            }

            if (!UserSrvc.GetOne(new Model.Db.User() { Token = req.Token }, out var user))
            {
                rsp.Stat = "用户不存在";
                reqmap.TryAdd(OP_Content, rsp.Stat);
                reqmap.TryAdd(OP_Detail, reqbytes.B2String());
                return rsp.ToBytes();
            }
          

            //签名为null,PC端调用，不为null手机端调用
            if ( !string.IsNullOrWhiteSpace(req.Sign))
            {
                var sign = BMD5.ToMD5String($"{req.Time}{req.Id}{req.Orderstat}{user.Pwd}", 32).ToUpper();

                if (!req.Sign.Equals(sign))
                {
                    rsp.Stat = "签名错误";
                    reqmap.TryAdd(OP_Content, rsp.Stat);
                    reqmap.TryAdd(OP_Detail, reqbytes.B2String());
                    return rsp.ToBytes();
                }

            }

           bool ok = SafeCheckSrvc.UpdOrderGoodSafeCheck(req.Id);

            if (ok)
            {
                rsp.Stat = "已经安检通过,不能取消预约";
                reqmap.TryAdd(OP_Content, rsp.Stat);
                reqmap.TryAdd(OP_Detail, reqbytes.B2String());
                return rsp.ToBytes();
            }

            OrderGoodsSrvc.GetOneById(new Model.Db.OrderGoods() { Id = decimal.Parse(req.Id) }, out var order);

            if (null == order)
            {
                rsp.Stat = "对象不存在";
                reqmap.TryAdd(OP_Content, rsp.Stat);
                reqmap.TryAdd(OP_Detail, reqbytes.B2String());
                return rsp.ToBytes();
            }

            /*if (!order.Driver.Equals(user.Username))
            {
                rsp.Stat = "账号不一致";
                reqmap.TryAdd(OP_Content, rsp.Stat);
                reqmap.TryAdd(OP_Detail, reqbytes.B2String());
                return rsp.ToBytes();
            }*/

            order.Orderstat = req.Orderstat;

            OrderGoodsSrvc.AddOrUpdate(order, UPD, user.Roleids);

            rsp.Stat = Success;
            reqmap.TryAdd(OP_User, user?.Userid);
            reqmap.TryAdd(OP_Content, "手机修改预约信息");
            
            return rsp.ToBytes();
        }

        /// <summary>
        /// 销售客户/供应商查询需要审核的预约信息
        /// </summary>
        /// <param name="reqmap"></param>
        /// <param name="reqbytes"></param>
        /// <returns></returns>
        public static byte[] CheckOrder(Dictionary<string, string> reqmap, byte[] reqbytes)
        {
            PhoneQueryOrderRsp rsp = new PhoneQueryOrderRsp() { Stat = Success };

            reqbytes.Json2ObjT<PhoneQueryOrderReq>(out PhoneQueryOrderReq req);

            //进日志
            req.Debug(string.Empty, Enter);

            if (reqmap.ContainsKey(TOKEN))
            {
                req.Token = reqmap.GetDicStr(TOKEN);
            }

            if (null == req)
            {
                rsp.Stat = JsonErr;
                reqmap.TryAdd(OP_Content, rsp.Stat);
                reqmap.TryAdd(OP_Detail, reqbytes.B2String());
                return rsp.ToBytes();
            }

            if (!UserSrvc.GetOne(new Model.Db.User() { Token = req.Token }, out var user))
            {
                rsp.Stat = "用户不存在";
                reqmap.TryAdd(OP_Content, rsp.Stat);
                reqmap.TryAdd(OP_Detail, reqbytes.B2String());
                return rsp.ToBytes();
            }

            var sign = BMD5.ToMD5String($"{req.Uid}{req.Time}{user.Pwd}", 32).ToUpper();

            if (!req.Sign.Equals(sign))
            {
                rsp.Stat = "签名错误";
                reqmap.TryAdd(OP_Content, rsp.Stat);
                reqmap.TryAdd(OP_Detail, reqbytes.B2String());
                return rsp.ToBytes();
            }

            if (null == req.Page)
            {
                req.Page = new PageBean() { Index = 1, Size = 10 };
            }
            // 默认客户登录,user的Userid为客户的公司名称
            var query = new Model.Db.OrderGoods() { Company = user.Userid };


            // 驾驶员模式
            if (ConstRole.DRIVER.Equals(user.Roleids) || "driver".Equals(user.Roleids))
            {
                BaseDriverSrvc.Get(new BaseDriver() { Name = user.Username, Tel = user.Phone }, null, out var drivers);
                query.Driver = drivers?.FirstOrDefault()?.Name;
                query.Company = string.Empty;
                

                if (string.IsNullOrWhiteSpace(query.Driver))
                {
                    rsp.Stat = "司机信息不存在";
                    reqmap.TryAdd(OP_Content, rsp.Stat);
                    reqmap.TryAdd(OP_Detail, reqbytes.B2String());
                    return rsp.ToBytes();
                }
            }

            // 采购销售不需要对公司进行限定
            if (ConstRole.SELLERPART.Equals(user.Roleids))
            {
                query.Company = string.Empty;
            }

            
            OrderGoodsSrvc.GetIphoneOrderData(query, req.Page, out var list);

            if (null != list)
            {
                SafeCheckSrvc.UpdOrderGoodsSafeCheck(list);
            }
           
            req.Page.CopyPage(rsp.Page);
            rsp.Data = list;

            reqmap.TryAdd(OP_User, user?.Userid);
            reqmap.TryAdd(OP_Content, "手机查询需要审核的预约信息");

            return rsp.ToBytes();
        }
    }
}
