﻿using System;
using System.Collections.Generic;
using System.Text;
using Yongrong.Model.Int.BaseInfo;
using Yongrong.Srvc.BaseInfo;
using BaseS.Serialization;
using BaseS.String;
using Yongrong.Srvc.Users;
using Yongrong.Model.Db;
using BaseS.Security;
using System.Linq;
using BaseS.File;
using System.Data;
using BaseS.File.Log;
using BaseS.Data;
using Yongrong.Model.Base;

namespace Yongrong.Int.BaseInfo
{
    class BaseDriverInt : BaseInt
    {
        /// <summary>
        /// 
        /// </summary>
        static readonly Dictionary<string, string> contractMap = new Dictionary<string, string>();

        static BaseDriverInt()
        {
            contractMap.TryAdd("姓名", "Name");
            contractMap.TryAdd("手机号码", "Tel");
            contractMap.TryAdd("身份证号", "Cardid");
            contractMap.TryAdd("驾照编号", "Driverid");
            contractMap.TryAdd("驾照等级", "Driverdegree");
            contractMap.TryAdd("驾照有效期", "Drivervalid");
            contractMap.TryAdd("牵引车车牌", "TractorId");
            contractMap.TryAdd("挂车车牌", "TrailerId");
            contractMap.TryAdd("隶属客商", "Transport");
            contractMap.TryAdd("隶属运输公司", "Transport");
        }

        /// <summary>
        /// 查询驾驶员
        /// </summary>
        /// <param name="reqmap"></param>
        /// <param name="reqbytes"></param>
        /// <returns></returns>
        public static byte[] Get(Dictionary<string, string> reqmap, byte[] reqbytes)
        {
            BaseDriverRsp rsp = new BaseDriverRsp();

            reqbytes.Json2ObjT<BaseDriverReq>(out BaseDriverReq req);

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
                req.Query.Transport = user.Userid;
            }

            BaseDriverSrvc.Get(
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

            reqmap.TryAdd(OP_User, user?.Userid);
            reqmap.TryAdd(OP_Content, "查询司机");

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
            BaseDriverAddRsp rsp = new BaseDriverAddRsp();

            reqbytes.Json2ObjT<BaseDriverAddReq>(out BaseDriverAddReq req);

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

            rsp.Stat = BaseDriverSrvc.AddOrUpdate(req.Driver, req.Op);

            if (Success.Equals(rsp.Stat))
            {
                if (ADD.Equals(req.Op) || UPD.Equals(req.Op))
                {
                    UserSrvc.GetOne(new User() { Userid = req.Driver.Tel }, out var user1);

                    if (null == user1)
                    {
                        //把司机信息写入到user_login里
                        UserSrvc.AddOrUpdate(new User()
                        {
                            Userid = req.Driver.Tel.Trim(),
                            Pwd = BMD5.ToMD5String(req.Driver.Tel.Trim().Substring(5, 6), 32).ToUpper(),
                            Phone = req.Driver.Tel.Trim(),
                            Username = req.Driver.Name.Trim(),
                            Roleids = ConstRole.DRIVER,
                            Creater = user.Userid
                        }, ADD);
                    }

                    if (ADD.Equals(req.Op))
                    {
                        //把牵引车写入到牵引车表中
                        BaseTractorSrvc.Get(new BaseTractor() { TractorId = req.Driver.TractorId, Transport = req.Driver.Transport }, new Model.Int.PageBean { Index = 1, Size = 1 }, out var tractors);

                        if (null == tractors || tractors.Count < 1)
                        {
                            BaseTractorSrvc.AddOrUpdate(new BaseTractor() { TractorId = req.Driver.TractorId, Transport = req.Driver.Transport }, ADD);
                        }

                        //把挂车信息写入到挂车表中
                        BaseTrailerSrvc.Get(new BaseTrailer() { TrailerId = req.Driver.TrailerId, Transport = req.Driver.Transport }, new Model.Int.PageBean { Index = 1, Size = 1 }, out var trailers);

                        if (null == trailers || trailers.Count < 1)
                        {
                            BaseTrailerSrvc.AddOrUpdate(new BaseTrailer() { TrailerId = req.Driver.TrailerId, Transport = req.Driver.Transport }, ADD);
                        }
                    }
                }
            }

            reqmap.TryAdd(OP_User, user?.Userid);
            reqmap.TryAdd(OP_Content, $"{GetOPName(req.Op)}司机");

            return rsp.ToBytes();
        }

        /// <summary>
        /// 司机上传文件
        /// </summary>
        /// <param name="reqmap"></param>
        /// <param name="reqbytes"></param>
        /// <returns></returns>
        public static byte[] Upload(Dictionary<string, string> reqmap, byte[] reqbytes)
        {
            BaseDriverAddRsp rsp = new BaseDriverAddRsp() { Stat = Success };
            string allmsg = System.Text.Encoding.GetEncoding(936).GetString(reqbytes);
            String[] sArray = allmsg.Split('\n');
            List<string> list = new List<string>();

            if (null == sArray || 4 >= sArray.Length)
            {
                rsp.Stat = "上传文件错误！";
                return rsp.ToBytes();
            }

            for (int i = 3; i < sArray.Length - 2; i++)
            {
                if (string.IsNullOrWhiteSpace(sArray[i]))
                {
                    continue;
                }

                if (string.IsNullOrWhiteSpace(sArray[i].Trim(',')))
                {
                    continue;
                }
                if (3 != i)
                {
                    sArray[i] = sArray[i].Replace("'", "").Trim();
                    sArray[i] = sArray[i].Replace("‘", "").Trim();
                    sArray[i] = sArray[i].Replace("’", "").Trim();
                    sArray[i] = sArray[i].Replace("'", "").Trim();
                }
                list.Add(sArray[i]?.Trim());
            }

            $"0109upload总共数据为{list.Count}条".Info();

            // 首行标题中没有指定内容,判断为编码错误
            if (!list.FirstOrDefault().Contains("姓名"))
            {
                rsp.Stat = "文件编码错误！";
                return rsp.ToBytes();
            }

            BCsv.Csv2DataTable(list, out DataTable table, true, contractMap);

            var driverlist = BDataTable.ToList_Old<BaseDriver>(table);
            $"0109upload解析成对象集合共:{list.Count}个".Info();

            StringBuilder sb = new StringBuilder(777);
            // 从第二行开始才是数据
            int line = 1, errline = 0;

            foreach (var driver in driverlist ?? new List<BaseDriver>())
            {
                line++;

                var checkstat = driver.Check();

                if (!Success.Equals(checkstat))
                {
                    errline++;
                    sb.AppendLine($"第{line}行存在错误:{checkstat}");
                    driver.Warn(checkstat);
                    continue;
                }

                var addstat = BaseDriverSrvc.AddOrUpdate(driver, ADD);

                if (!Success.Equals(addstat))
                {
                    errline++;
                    sb.AppendLine($"第{line}行存在错误:{addstat}");
                    driver.Warn(addstat);
                }

                // 不限制司机是否添加成功与否,都去开通登录账号,首先判断司机是否存在情况,添加司机账号
                if (!UserSrvc.GetOne(new User() { Userid = driver.Tel }, out var user1))
                {
                    //把司机信息写入到user_login里
                    UserSrvc.AddOrUpdate(new User()
                    {
                        Userid = driver.Tel,
                        Pwd = BMD5.ToMD5String(driver.Tel.Substring(5, 6), 32, null, true),
                        Phone = driver.Tel,
                        Username = driver.Name,
                        Roleids = ConstRole.DRIVER,
                        Creater = ""
                    }, ADD);
                }
            }

            rsp.Stat = $"总共数据:{driverlist.Count}行 失败:{errline}行 ";

            if (1 <= sb.Length)
            {
                rsp.Stat += " 失败详情:\n" + sb.ToString();
            }

            return rsp.ToBytes();
        }
    }
}
