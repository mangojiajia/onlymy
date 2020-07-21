﻿using System;
using System.Collections.Generic;
using System.Text;
using Yongrong.Model.Db;
using Yongrong.Model.Int;
using System.Linq;
using BaseS.File.Log;

namespace Yongrong.Srvc.BaseInfo
{
    class BaseTransportSrvc : BaseSrvc
    {
        /// <summary>
        /// 获取运输公司
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="basegoodslist"></param>
        /// <returns></returns>
        public static bool Get(BaseTransport query, PageBean page, out List<BaseTransport> basegoodslist)
        {
            basegoodslist = new List<BaseTransport>();

            if (null == page)
            {
                page = new PageBean();
            }

            if (null == query)
            {
                query = new BaseTransport();
            }

            query.Debug();

            using (var db = DbContext)
            {
                page.Row = db.BaseTransport.Count(a =>
                (string.IsNullOrWhiteSpace(query.Name) || (!string.IsNullOrWhiteSpace(a.Name) && a.Name.Contains(query.Name)))
                && (string.IsNullOrWhiteSpace(query.Code) || query.Code.Equals(a.Code))
                && (string.IsNullOrWhiteSpace(query.Contract) || (!string.IsNullOrWhiteSpace(a.Contract) && a.Contract.Contains(query.Contract)))
                && (string.IsNullOrWhiteSpace(query.Legal) || (!string.IsNullOrWhiteSpace(a.Legal) && a.Legal.Contains(query.Legal)))
                && (string.IsNullOrWhiteSpace(query.Tel) || query.Tel.Equals(a.Tel))
                && (string.IsNullOrWhiteSpace(query.Mail) || (!string.IsNullOrWhiteSpace(a.Mail) && a.Mail.Contains(query.Mail)))
                );

                page.SumPageCount();

                basegoodslist.AddRange(
                    db.BaseTransport.Where(a =>
                    (string.IsNullOrWhiteSpace(query.Name) || (!string.IsNullOrWhiteSpace(a.Name) && a.Name.Contains(query.Name)))
                && (string.IsNullOrWhiteSpace(query.Code) || query.Code.Equals(a.Code))
                && (string.IsNullOrWhiteSpace(query.Contract) || (!string.IsNullOrWhiteSpace(a.Contract) && a.Contract.Contains(query.Contract)))
                && (string.IsNullOrWhiteSpace(query.Legal) || (!string.IsNullOrWhiteSpace(a.Legal) && a.Legal.Contains(query.Legal)))
                && (string.IsNullOrWhiteSpace(query.Tel) || query.Tel.Equals(a.Tel))
                && (string.IsNullOrWhiteSpace(query.Mail) || (!string.IsNullOrWhiteSpace(a.Mail) && a.Mail.Contains(query.Mail))))
                    .OrderByDescending(a => a.Id)
                    .Skip((page.Index - 1) * page.Size).Take(page.Size)
                    );
            }

            return true;
        }


        /// <summary>
        /// 运输公司的添加和更新
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public static string AddOrUpdate(BaseTransport addupdobj, string op)
        {
            if (null == addupdobj)
            {
                return ParamNull;
            }

            using (var db = DbContext)
            {
                switch(op)
                {
                    case ADD:
                        var old = db.BaseTransport.FirstOrDefault(a => addupdobj.Name.Equals(a.Name));
                        if(null == old)
                        { 
                            addupdobj.Id = GetSeq("SEQ_BASE_TRANSPORT");
                            addupdobj.Whiteflag = "1";
                            db.BaseTransport.Add(addupdobj);
                        }
                        else
                        {
                            $"BaseTransport Name:{addupdobj.Name} 已存在".Warn();
                            return AddRecord;
                        }
                        break;
                    case UPD:
                        var tmp = db.BaseTransport.FirstOrDefault(a => a.Id == addupdobj.Id);

                        if (null == tmp)
                        {
                            $"BaseTransport Id:{addupdobj.Id} 不存在".Warn();
                            return UpdNoRecord;
                        }

                        addupdobj.Upd(tmp);

                        db.BaseTransport.Update(tmp);
                        break;
                    case DEL:
                        var tmp1 = db.BaseTransport.FirstOrDefault(a => a.Id == addupdobj.Id);

                        if (null == tmp1)
                        {
                            $"BaseTransport Id:{addupdobj.Id} 不存在".Warn();
                            return DelNoRecord;
                        }

                        db.BaseTransport.Remove(tmp1);
                        break;
                    default:
                        "未加操作类型".Info();
                        return OpEmpty;
                }

                db.SaveChanges();
            }

            return Success;
        }

        /// <summary>
        /// 联想运输公司
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="basegoodslist"></param>
        /// <returns></returns>
        public static bool Search(BaseTransport query, out List<string> basegoodslist)
        {
            basegoodslist = new List<string>();

            if (null == query)
            {
                query = new BaseTransport();
            }

            query.Debug();

            using (var db = DbContext)
            {
                basegoodslist.AddRange(
                    db.BaseTransport.Where(a =>
                     (string.IsNullOrWhiteSpace(query.Name) || (!string.IsNullOrWhiteSpace(a.Name) && a.Name.Contains(query.Name))))
                    .Select(a => a.Name));
            }

            return true;
        }
    }
}
