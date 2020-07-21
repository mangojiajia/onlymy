﻿using System;
using System.Collections.Generic;
using System.Text;
using Yongrong.Model.Db;
using Yongrong.Model.Int;
using BaseS.File.Log;
using System.Linq;

namespace Yongrong.Srvc.Sys
{
    class AbnormalSrvc : BaseSrvc
    {
        /// <summary>
        /// 添加和更新 异常数据表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public static string AddOrUpdate(Abnormal addupdobj, string op)
        {
            if (null == addupdobj)
            {
                return ParamNull;
            }

            using (var db = DbContext)
            {
                switch (op)
                {
                    case ADD:
                        addupdobj.Id = GetSeq("SEQ_ABNORMAL");
                        addupdobj.Isdispose = "0";
                        addupdobj.Createtime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        db.Abnormal.Add(addupdobj);

                        break;
                    case UPD:
                        var tmp = db.Abnormal.FirstOrDefault(a => a.Id == addupdobj.Id);

                        if (null == tmp)
                        {
                            $"Abnormal Id:{addupdobj.Id} 不存在".Warn();
                            return UpdNoRecord;
                        }
                        addupdobj.Upd(tmp);

                        db.Abnormal.Update(tmp);
                        break;
                    case DEL:
                        var tmp1 = db.Abnormal.FirstOrDefault(a => a.Id == addupdobj.Id);

                        if (null == tmp1)
                        {
                            $"Abnormal Id:{addupdobj.Id} 不存在".Warn();
                            return DelNoRecord;
                        }

                        db.Abnormal.Remove(tmp1);

                        break;
                    default:
                        OpEmpty.Info();
                        return OpEmpty;
                }

               SaveChanges(db, "Abnormal "+ op);
            }

            return Success;
        }


        /// <summary>
        /// 异常数据查询
        /// </summary>
        /// <param name="query"></param>
        /// <param name="page"></param>
        /// <param name="baseTrailerslist"></param>
        /// <returns></returns>
        public static bool Get(Abnormal query, PageBean page, out List<Abnormal> abnormallist)
        {
            abnormallist = new List<Abnormal>();

            if (null == page)
            {
                page = new PageBean();
            }

            if (null == query)
            {
                query = new Abnormal();
            }

            //进去日志
            query.Debug();

            using (var db = DbContext)
            {
                page.Row = db.Abnormal.Count(a =>
                (string.IsNullOrWhiteSpace(query.Abnormalname) || (!string.IsNullOrWhiteSpace(a.Abnormalname) && a.Abnormalname.Contains(query.Abnormalname)))    
                && (string.IsNullOrWhiteSpace(query.Abnormaltype) || (!string.IsNullOrWhiteSpace(a.Abnormaltype) && a.Abnormaltype.Contains(query.Abnormaltype)))
                && (string.IsNullOrWhiteSpace(query.Isdispose) || query.Isdispose.Equals(a.Isdispose))
                && (string.IsNullOrWhiteSpace(query.Remark) || (!string.IsNullOrWhiteSpace(a.Remark) && a.Remark.Contains(query.Remark)))
                && (string.IsNullOrWhiteSpace(query.Starttime) || 0 >= query.Starttime.CompareTo(a.Createtime))
                && (string.IsNullOrWhiteSpace(query.Endtime) || 0 <= query.Endtime.CompareTo(a.Createtime))
                && (string.IsNullOrWhiteSpace(query.Createuser) || query.Createuser.Equals(a.Createuser))      
                && (string.IsNullOrWhiteSpace(query.Updateuser) || query.Updateuser.Equals(a.Updateuser))
                );

                page.SumPageCount();

                abnormallist.AddRange(
                    db.Abnormal.Where(a =>
                (string.IsNullOrWhiteSpace(query.Abnormalname) || (!string.IsNullOrWhiteSpace(a.Abnormalname) && a.Abnormalname.Contains(query.Abnormalname)))
                && (string.IsNullOrWhiteSpace(query.Abnormaltype) || (!string.IsNullOrWhiteSpace(a.Abnormaltype) && a.Abnormaltype.Contains(query.Abnormaltype)))
                && (string.IsNullOrWhiteSpace(query.Isdispose) || query.Isdispose.Equals(a.Isdispose))
                && (string.IsNullOrWhiteSpace(query.Remark) || (!string.IsNullOrWhiteSpace(a.Remark) && a.Remark.Contains(query.Remark)))
                && (string.IsNullOrWhiteSpace(query.Starttime) || 0 >= query.Starttime.CompareTo(a.Createtime))
                && (string.IsNullOrWhiteSpace(query.Endtime) || 0 <= query.Endtime.CompareTo(a.Createtime))
                && (string.IsNullOrWhiteSpace(query.Createuser) || query.Createuser.Equals(a.Createuser))
                && (string.IsNullOrWhiteSpace(query.Updateuser) || query.Updateuser.Equals(a.Updateuser))
                    )
                    .OrderByDescending(a => a.Id)
                    .Skip((page.Index - 1) * page.Size).Take(page.Size)
                    );
            }
            return true;
        }

    }
}
