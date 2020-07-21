using System;
using System.Collections.Generic;
using System.Text;
using Yongrong.Model.Db;
using Yongrong.Model.Int;
using BaseS.File.Log;
using System.Linq;

namespace Yongrong.Srvc.Gate
{
    class ApiGateSrvc : BaseSrvc
    {
        /// <summary>
        /// 获取门禁通信记录
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static bool Get(ApiGateevent query, PageBean page, out List<ApiGateevent> data)
        {
            data = new List<ApiGateevent>();

            if (null == page)
            {
                page = new PageBean();
            }

            if (null == query)
            {
                query = new ApiGateevent();
            }

            query.Debug();

            using (var db = DbContext)
            {
                page.Row = db.ApiGateevent.Count();

                page.SumPageCount();

                data.AddRange(
                    db.ApiGateevent.Where(a =>
                    // 查询对象中的参数Starttime 和Stoptime都是针对 数据库中的Starttime字段进行筛选
                    (string.IsNullOrWhiteSpace(query.Starttime)  || 0 >= query.Starttime.CompareTo(a.Starttime))
                    && (string.IsNullOrWhiteSpace(query.Stoptime) || 0 <= query.Stoptime.CompareTo(a.Starttime)))
                    .OrderByDescending(a => a.Id)
                    .Skip((page.Index - 1) * page.Size).Take(page.Size)
                    );
            }

            return true;
        }
    }
}
