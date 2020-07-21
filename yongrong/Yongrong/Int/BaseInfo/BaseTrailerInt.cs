﻿using System;
using System.Collections.Generic;
using System.Text;
using Yongrong.Model.Int.BaseInfo;
using BaseS.Serialization;
using Yongrong.Srvc.BaseInfo;
using Yongrong.Srvc.Users;
using BaseS.String;
using System.Linq;
using BaseS.File;
using System.Data;
using BaseS.Data;
using Yongrong.Model.Db;
using BaseS.File.Log;


namespace Yongrong.Int.BaseInfo
{
    class BaseTrailerInt:BaseInt
    {
        /// <summary>
        /// 
        /// </summary>
        static readonly Dictionary<string, string> contractMap = new Dictionary<string, string>();

        static BaseTrailerInt()
        {
            contractMap.TryAdd("挂车车牌", "TrailerId");
            contractMap.TryAdd("挂车证件标号", "Carflag");
            contractMap.TryAdd("挂车证件编号", "Carflag");
            contractMap.TryAdd("最大荷载量(吨)", "Maxweight");
            contractMap.TryAdd("隶属客商", "Transport");
            contractMap.TryAdd("运输车编号", "Carid");
            contractMap.TryAdd("有效期", "Validdate");
            contractMap.TryAdd("吨/最大荷载量", "Maxweight");
        }

        /// <summary>
        /// 获取挂车信息
        /// </summary>
        /// <param name="reqmap"></param>
        /// <param name="reqbytes"></param>
        /// <returns></returns>
        public static byte[] Get(Dictionary<string, string> reqmap, byte[] reqbytes)
        {
            BaseTrailerRsp rsp = new BaseTrailerRsp();

            reqbytes.Json2ObjT<BaseTrailerReq>(out BaseTrailerReq req);

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

            BaseTrailerSrvc.Get(
                req.Query,
                req.Page,
                out var TrailersList);

            if (null == req.Page)
            {
                rsp.Page = new Model.Int.PageBean();
            }
            else
            {
                req.Page.CopyPage(rsp.Page);
            }

            rsp.Stat = Success;
            rsp.TrailersList = TrailersList;

            reqmap.TryAdd(OP_User, user?.Userid);
            reqmap.TryAdd(OP_Content, $"查询挂车");

            return rsp.ToBytes();
        }


        /// <summary>
        /// 添加挂车信息
        /// </summary>
        /// <param name="reqmap"></param>
        /// <param name="reqbytes"></param>
        /// <returns></returns>
        public static byte[] AddUpd(Dictionary<string, string> reqmap, byte[] reqbytes)
        {
            BaseTrailerAddRsp rsp = new BaseTrailerAddRsp();

            reqbytes.Json2ObjT<BaseTrailerAddReq>(out BaseTrailerAddReq req);

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

            rsp.Stat = BaseTrailerSrvc.AddOrUpdate(req.Trailer, req.Op);

            reqmap.TryAdd(OP_User, user?.Userid);
            reqmap.TryAdd(OP_Content, $"{GetOPName(req.Op)}挂车");

            return rsp.ToBytes();
        }

        /// <summary>
        /// 挂车上传文件
        /// </summary>
        /// <param name="reqmap"></param>
        /// <param name="reqbytes"></param>
        /// <returns></returns>
        public static byte[] Upload(Dictionary<string, string> reqmap, byte[] reqbytes)
        {
            BaseTrailerAddRsp rsp = new BaseTrailerAddRsp() { Stat = Success };

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

            $"0108upload总共数据为{list.Count}条".Info();

            // 首行标题中没有指定内容,判断为编码错误
            if (!list.FirstOrDefault().Contains("挂车车牌"))
            {
                rsp.Stat = "文件编码错误！";
                return rsp.ToBytes();
            }

            BCsv.Csv2DataTable(list, out DataTable table, true, contractMap);

            var trailerlist = BDataTable.ToList_Old<BaseTrailer>(table);

            $"0108upload解析成对象集合共:{list.Count}个".Info();

            StringBuilder sb = new StringBuilder(777);
            // 从第二行开始才是数据
            int line = 1, errline = 0;

            foreach (var trailer in trailerlist ?? new List<BaseTrailer>())
            {
                line++;

                var checkstat = trailer.Check();

                if (!Success.Equals(checkstat))
                {
                    errline++;
                    sb.AppendLine($"第{line}行存在错误:{checkstat}");
                    trailer.Warn(checkstat);
                    continue;
                }

                var addstat = BaseTrailerSrvc.AddOrUpdate(trailer, ADD);

                if (!Success.Equals(addstat))
                {
                    errline++;
                    sb.AppendLine($"第{line}行存在错误:{addstat}");
                    trailer.Warn(addstat);
                }
            }

            rsp.Stat = $"总共数据:{trailerlist.Count}行 失败:{errline}行 " ;

            if (1 <= sb.Length)
            {
                rsp.Stat += " 失败详情:\n" + sb.ToString();
            }

            return rsp.ToBytes();
        }

    }
}
