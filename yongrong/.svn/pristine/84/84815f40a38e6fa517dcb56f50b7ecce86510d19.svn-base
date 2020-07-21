using System;
using System.Collections.Generic;
using System.Text;
using Yongrong.Model.Db;
using System.Linq;
using Yongrong.Model.Int;

namespace Yongrong.Srvc
{
    class TodoListSrvc : BaseSrvc
    {
        /// <summary>
        /// 获取
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="todolists"></param>
        /// <returns></returns>
        public static bool Get(string userid, PageBean page, out List<Todolist> todolists)
        {
            todolists = new List<Todolist>();

            if (string.IsNullOrWhiteSpace(userid))
            {
                return false;
            }

            using (var db = DbContext)
            {
                if (null == page)
                {
                    todolists.AddRange(db.Todolist.Where(a => userid.Equals(a.Userid)));
                }
                else
                {
                    page.Row = db.Todolist.Count(a => userid.Equals(a.Userid));

                    page.SumPageCount();

                    todolists.AddRange(
                        db.Todolist.Where(a => userid.Equals(a.Userid) && ("0".Equals(a.Flag)))
                        .Skip((page.Index - 1) * page.Size).Take(page.Size).OrderByDescending(a => a.Createtime)
                        );

                    todolists.AddRange(
                        db.Todolist.Where(a => userid.Equals(a.Userid) && ("1".Equals(a.Flag)))
                        .Skip((page.Index - 1) * page.Size).Take(page.Size).OrderByDescending(a => a.Createtime)
                        );
                }
            }

            return true;
        }
    }
}
