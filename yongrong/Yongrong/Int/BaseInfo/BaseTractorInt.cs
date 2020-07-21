﻿using System;
using System.Collections.Generic;
using System.Text;
using Yongrong.Model.Int.BaseInfo;
using BaseS.Serialization;
using Yongrong.Srvc.BaseInfo;
using BaseS.String;
using Yongrong.Srvc.Users;
using BaseS.File.Log;
using BaseS.File;
using System.Data;
using BaseS.Data;
using Yongrong.Model.Db;
using System.Linq;
using Microsoft.AspNetCore.Server.Kestrel.Core.Internal.Http;

namespace Yongrong.Int.BaseInfo
{
    class BaseTractorInt : BaseInt
    {
        /// <summary>
        /// 
        /// </summary>
        static readonly Dictionary<string, string> contractMap = new Dictionary<string, string>();

        static BaseTractorInt()
        {
            contractMap.TryAdd("牵引车车牌", "TractorId");
            contractMap.TryAdd("运输车编号", "Carid");
            contractMap.TryAdd("隶属客商", "Transport");
            contractMap.TryAdd("有效期", "Validdate");
        }

        /// <summary>
        /// 查询牵引车
        /// </summary>
        /// <param name="reqmap"></param>
        /// <param name="reqbytes"></param>
        /// <returns></returns>
        public static byte[] Get(Dictionary<string, string> reqmap, byte[] reqbytes)
        {
            BaseTractorRsp rsp = new BaseTractorRsp();

            reqbytes.Json2ObjT<BaseTractorReq>(out BaseTractorReq req);

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


            BaseTractorSrvc.Get(
                req.Query,
                req.Page,
                out var TractorList);

            if (null == req.Page)
            {
                rsp.Page = new Model.Int.PageBean();
            }
            else
            {
                req.Page.CopyPage(rsp.Page);
            }

            rsp.Stat = Success;
            rsp.List = TractorList;

            reqmap.TryAdd(OP_User, user?.Userid);
            reqmap.TryAdd(OP_Content, $"查询牵引车");

            return rsp.ToBytes();
        }


        /// <summary>
        /// 添加牵引车信息
        /// </summary>
        /// <param name="reqmap"></param>
        /// <param name="reqbytes"></param>
        /// <returns></returns>
        public static byte[] AddUpd(Dictionary<string, string> reqmap, byte[] reqbytes)
        {
            BaseTractorAddRsp rsp = new BaseTractorAddRsp();

            reqbytes.Json2ObjT<BaseTractorAddReq>(out BaseTractorAddReq req);

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

            rsp.Stat = BaseTractorSrvc.AddOrUpdate(req.Tractor, req.Op);

            reqmap.TryAdd(OP_User, user?.Userid);
            reqmap.TryAdd(OP_Content, $"{GetOPName(req.Op)}牵引车");

            return rsp.ToBytes();
        }


        /// <summary>
        /// 牵引车上传文件
        /// </summary>
        /// <param name="reqmap"></param>
        /// <param name="reqbytes"></param>
        /// <returns></returns>
        public static byte[] Upload(Dictionary<string, string> reqmap, byte[] reqbytes)
        {
            BaseTractorAddRsp rsp = new BaseTractorAddRsp() { Stat = Success };

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

            $"0107upload总共数据为{list.Count}条".Info();

            if (!list.FirstOrDefault().Contains("牵引车车牌"))
            {
                rsp.Stat = "文件编码错误！";
                return rsp.ToBytes();
            }

            BCsv.Csv2DataTable(list, out DataTable table, true, contractMap);

            var tractorlist = BDataTable.ToList_Old<BaseTractor>(table);
            $"0107upload解析成对象集合共:{list.Count}个".Info();


            StringBuilder sb = new StringBuilder(777);
            // 从第二行开始才是数据
            int line = 1, errline = 0;

            foreach (var tractor in tractorlist ?? new List<BaseTractor>())
            {
                line++;

                var checkstat = tractor.Check();

                if (!Success.Equals(checkstat))
                {
                    errline++;
                    sb.AppendLine($"第{line}行存在错误:{checkstat}");
                    tractor.Warn(checkstat);

                    continue;
                }

                var addstat = BaseTractorSrvc.AddOrUpdate(tractor, ADD);

                if (!Success.Equals(addstat))
                {
                    errline++;
                    sb.AppendLine($"第{line}行存在错误:{addstat}");
                    tractor.Warn(addstat);
                }
            }

            rsp.Stat = $"总共数据:{tractorlist.Count}行 失败:{errline}行";

            if (1 <= sb.Length)
            {
                rsp.Stat += " 失败详情:\n" + sb.ToString();
            }

            return rsp.ToBytes();
        }

    }
}
