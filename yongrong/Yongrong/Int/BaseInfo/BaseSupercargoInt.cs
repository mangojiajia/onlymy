﻿using System;
using System.Collections.Generic;
using System.Text;
using Yongrong.Model.Int.BaseInfo;
using Yongrong.Srvc.BaseInfo;
using BaseS.Serialization;
using BaseS.String;
using Yongrong.Srvc.Users;
using BaseS.File.Log;
using System.Data;
using BaseS.Data;
using BaseS.File;
using Yongrong.Model.Db;
using System.Linq;

namespace Yongrong.Int.BaseInfo
{
    class BaseSupercargoInt : BaseInt
    {

        /// <summary>
        /// 
        /// </summary>
        static readonly Dictionary<string, string> contractMap = new Dictionary<string, string>();

        static BaseSupercargoInt()
        {
            contractMap.TryAdd("押运员姓名", "Name");
            contractMap.TryAdd("身份证号", "Cardid");
            contractMap.TryAdd("押运证件编号", "Driverid");
            contractMap.TryAdd("对应司机", "Driver");
            contractMap.TryAdd("隶属客商", "Transport");
            contractMap.TryAdd("证件有效期", "Drivervalid");
        }

        /// <summary>
        /// 查询押运员
        /// </summary>
        /// <param name="reqmap"></param>
        /// <param name="reqbytes"></param>
        /// <returns></returns>
        public static byte[] Get(Dictionary<string, string> reqmap, byte[] reqbytes)
        {
            BaseSupercargoRsp rsp = new BaseSupercargoRsp();

            reqbytes.Json2ObjT<BaseSupercargoReq>(out BaseSupercargoReq req);

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

            BaseSupercargoSrvc.Get(
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
            reqmap.TryAdd(OP_Content, $"查询押运员");

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
            BaseSupercargoAddRsp rsp = new BaseSupercargoAddRsp();

            reqbytes.Json2ObjT<BaseSupercargoAddReq>(out BaseSupercargoAddReq req);

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

            rsp.Stat = BaseSupercargoSrvc.AddOrUpdate(req.Supercargo, req.Op);

            reqmap.TryAdd(OP_User, user?.Userid);
            reqmap.TryAdd(OP_Content, $"{GetOPName(req.Op)}押运员");

            return rsp.ToBytes();
        }

        /// <summary>
        /// 押运员上传文件
        /// </summary>
        /// <param name="reqmap"></param>
        /// <param name="reqbytes"></param>
        /// <returns></returns>
        public static byte[] Upload(Dictionary<string, string> reqmap, byte[] reqbytes)
        {
            BaseSupercargoAddRsp rsp = new BaseSupercargoAddRsp() { Stat = Success };

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

            $"0110upload总共数据为{list.Count}条".Info();

            if (!list.FirstOrDefault().Contains("姓名"))
            {
                rsp.Stat = "文件编码错误！";
                return rsp.ToBytes();
            }

            BCsv.Csv2DataTable(list, out DataTable table, true, contractMap);

            var supercargolist = BDataTable.ToList_Old<BaseSupercargo>(table);
            $"0110upload解析成对象集合共:{list.Count}个".Info();

            StringBuilder sb = new StringBuilder(777);
            // 从第二行开始才是数据
            int line = 1, errline = 0;

            foreach (var supercargo in supercargolist ?? new List<BaseSupercargo>())
            {
                line++;

                var checkstat = supercargo.Check();

                if (!Success.Equals(checkstat))
                {
                    errline++;
                    sb.AppendLine($"第{line}行存在错误:{checkstat}");
                    supercargo.Warn(checkstat);
                    continue;
                }

                var addstat = BaseSupercargoSrvc.AddOrUpdate(supercargo, ADD);

                if (!Success.Equals(addstat))
                {
                    errline++;
                    sb.AppendLine($"第{line}行存在错误:{addstat}");
                    supercargo.Warn(addstat);
                }
            }

            rsp.Stat = $"总共数据:{supercargolist.Count}行 失败:{errline}行 " ;

            if (1 <= sb.Length)
            {
                rsp.Stat += " 失败详情:\n" + sb.ToString();
            }

            return rsp.ToBytes();
        }
    }
}
