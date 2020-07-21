﻿using System;
using System.Collections.Generic;
using System.Text;
using Yongrong.Model.Db;
using BaseS.File.Log;
using Yongrong.Db;
using System.Linq;
using BaseS.String;
using Microsoft.AspNetCore.Http.Features;
using Yongrong.Model.Int;

namespace Yongrong.Srvc.Users
{
    class UserSrvc : BaseSrvc
    {
        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="query"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public static bool Login(User query, out User user)
        {
            user = null;

            if (null == query)
            {
                "查询条件为null,不返回对象".Warn(Exit);
                return false;
            }

            if (string.IsNullOrEmpty(query.Userid)
                && string.IsNullOrWhiteSpace(query.Phone)
                && string.IsNullOrWhiteSpace(query.Token))
            {
                "查询条件为空,不返回对象".Warn(Exit);
                return false;
            }

            using (var db = DbContext)
            {
                // token登录及刷新模式
                if (!string.IsNullOrWhiteSpace(query.Token))
                {
                    user = db.User.FirstOrDefault(a => query.Token.Equals(a.Token));

                    /* 
                     // 不刷新Token变量,因为有可能部分接口不会去更新页面上的Token
                    if (null != user)
                    {
                        if (Sys.TokenValidHours <= (DateTime.Now - (user.Flashtime ?? DateTime.MinValue)).TotalHours)
                        {
                            user.Token = (DateTime.Now.Ticks + "-" + query.Userid + user.Id).Str2Base64();
                            user.Flashtime = DateTime.Now;

                            db.User.Update(user);
                            db.SaveChanges();
                        }
                    }
                    */
                }
                // 通过密码登录
                else
                {
                    user = db.User.FirstOrDefault(a =>
                    (string.IsNullOrWhiteSpace(query.Userid) || query.Userid.Equals(a.Userid))
                    && (string.IsNullOrWhiteSpace(query.Phone) || query.Phone.Equals(a.Phone)));

                    if (null != user)
                    {
                        if (string.IsNullOrWhiteSpace(user.Token) 
                            || Sys.TokenValidHours <= (DateTime.Now - (user.Flashtime ?? DateTime.MinValue)).TotalHours)
                        {
                            user.Token = (DateTime.Now.Ticks + "-" + query.Userid + user.Id).Str2Base64();
                            user.Flashtime = DateTime.Now;
                        }
                        
                        user.Logintime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                        db.User.Update(user);
                        db.SaveChanges();
                    }
                }
            }

            return null != user;
        }

        /// <summary>
        /// 查询用户
        /// </summary>
        /// <param name="query"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public static bool Get(User query, out List<User> users)
        {
            users = new List<User>();

            if (null == query)
            {
                "查询条件为null,不返回对象".Warn(Exit);
                return false;
            }

            using (var db = DbContext)
            {
                users.AddRange(db.User.Where(a =>
                (string.IsNullOrWhiteSpace(query.Userid) || query.Userid.Equals(a.Userid))
                && (string.IsNullOrWhiteSpace(query.Phone) || query.Phone.Equals(a.Phone))
                && (string.IsNullOrWhiteSpace(query.Roleids) || query.Roleids.Equals(a.Roleids))
                && (string.IsNullOrWhiteSpace(query.Username) || query.Username.Equals(a.Username))
                ));
            }

            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="query"></param>
        /// <param name="page"></param>
        /// <param name="users"></param>
        /// <returns></returns>
        public static bool Get(User query, PageBean page, out List<User> users)
        {
            users = new List<User>();

            if (null == query)
            {
                query = new User();
                return false;
            }

            using (var db = DbContext)
            {
                page.Row = db.User.Count(a =>
                (string.IsNullOrWhiteSpace(query.Userid) || query.Userid.Equals(a.Userid))
                && (string.IsNullOrWhiteSpace(query.Phone) || query.Phone.Equals(a.Phone))
                && (string.IsNullOrWhiteSpace(query.Roleids) || query.Roleids.Equals(a.Roleids))
                );

                page.SumPageCount();

                users.AddRange(db.User.Where(a =>
                (string.IsNullOrWhiteSpace(query.Userid) || query.Userid.Equals(a.Userid))
                && (string.IsNullOrWhiteSpace(query.Phone) || query.Phone.Equals(a.Phone))
                && (string.IsNullOrWhiteSpace(query.Roleids) || query.Roleids.Equals(a.Roleids))
                ).OrderBy(a => a.Userid)
                    .Skip((page.Index - 1) * page.Size).Take(page.Size)
                    );
            }

            return true;
        }

        /// <summary>
        /// 查询用户
        /// </summary>
        /// <param name="query"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public static bool GetOne(User query, out User user)
        {
            user = null;

            if (null == query)
            {
                "查询条件为null,不返回对象".Warn(Exit);
                return false;
            }

            if (string.IsNullOrEmpty(query.Userid)
                && string.IsNullOrWhiteSpace(query.Phone)
                && string.IsNullOrWhiteSpace(query.Token))
            {
                "查询条件为空,不返回对象".Warn(Exit);
                return false;
            }

            using (var db = DbContext)
            {
                // token登录及刷新模式
                if (!string.IsNullOrWhiteSpace(query.Token))
                {
                    user = db.User.FirstOrDefault(a => query.Token.Equals(a.Token));
                }
                // 通过密码登录
                else
                {
                    user = db.User.FirstOrDefault(a =>
                    (string.IsNullOrWhiteSpace(query.Userid) || query.Userid.Equals(a.Userid))
                    && (string.IsNullOrWhiteSpace(query.Phone) || query.Phone.Equals(a.Phone)));
                }
            }

            return null != user;
        }

        /// <summary>
        /// 添加和更新 用户
        /// </summary>
        /// <param name="addupdobj"></param>
        /// <param name="op"></param>
        /// <returns></returns>
        public static string AddOrUpdate(User addupdobj, string op)
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
                        var tmp2 = db.User.FirstOrDefault(a => addupdobj.Userid.Equals(a.Userid));

                        if (null != tmp2)
                        {
                            addupdobj.Upd(tmp2);

                            db.User.Update(tmp2);
                        }
                        else
                        {
                            addupdobj.Createtime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                            addupdobj.Id = GetSeq("SEQ_USER_LOGIN");

                            db.User.Add(addupdobj);
                        }
                        break;
                    case UPD:
                        var tmp = db.User.FirstOrDefault(a => addupdobj.Userid.Equals(a.Userid));

                        if (null == tmp)
                        {
                            $"Userid:{addupdobj.Userid} 不存在".Warn();
                            return UpdNoRecord;
                        }

                        addupdobj.Upd(tmp);

                        db.User.Update(tmp);
                        break;
                    case DEL:
                        var tmp1 = db.User.FirstOrDefault(a => addupdobj.Userid.Equals(a.Userid));

                        if (null == tmp1)
                        {
                            $"Userid:{addupdobj.Userid} 不存在".Warn();
                            return DelNoRecord;
                        }

                        db.User.Remove(tmp1);
                        break;
                    default:
                        "未加操作类型".Info();
                        return OpEmpty;
                }

                SaveChanges(db, "User");
            }

            return Success;
        }

        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="user"></param>
        /// <param name="op"></param>
        /// <returns></returns>
        public static string Del(User user, string op)
        {
            List<User> users = new List<User>();

            using(var db = DbContext)
            {
                users.AddRange(db.User.Where(a => 
                (string.IsNullOrWhiteSpace(user.Userid) || user.Userid.Equals(a.Userid))
                &&(string.IsNullOrWhiteSpace(user.Roleids) || user.Roleids.Equals(a.Roleids))
                ));

                if (0 == users.Count)
                {
                    return DelNoRecord;
                }

                db.User.RemoveRange(users);

                SaveChanges(db, "User Del");
            }

            return Success;
        }
    }
}
