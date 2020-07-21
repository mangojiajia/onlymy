﻿using System;
using System.Collections.Generic;
using System.Text;
using Yongrong.Model.Int.Org;
using Yongrong.Model.Int.BaseInfo;
using Yongrong.Srvc.BaseInfo;
using BaseS.Serialization;
using Yongrong.Srvc.Users;
using System.Linq;
using static Yongrong.Model.Int.Org.PermissionRsp;
using Yongrong.Srvc.Sys;
using Microsoft.AspNetCore.Http.Features;
using Yongrong.Model.Db;
using Yongrong.Model.Int.User;
using BaseS.Security;
using BaseS.String;
using BaseS.File.Log;

namespace Yongrong.Int.Org
{
    class UserPermissionInt : BaseInt
    {
        /// <summary>
        /// 获取部门下人员的菜单以及权限
        /// </summary>
        /// <param name="reqmap"></param>
        /// <param name="reqbytes"></param>
        /// <returns></returns>
        public static byte[] Get(Dictionary<string, string> reqmap, byte[] reqbytes)
        {
            UserPermissionGetRsp rsp = new UserPermissionGetRsp() { Stat = Success };

            reqbytes.Json2ObjT<UserPermissionGetReq>(out UserPermissionGetReq req);
            //进日志
            req.Debug(string.Empty, Enter);
            if (null == req)
            {
                rsp.Stat = JsonErr;
                reqmap.TryAdd(OP_Content, $"格式错误");
                reqmap.TryAdd(OP_Detail, reqbytes.B2String());
                return rsp.ToBytes();
            }

            if (!Success.Equals(req.Check()))
            {
                rsp.Stat = req.Check();
                reqmap.TryAdd(OP_Content, $"格式错误");
                reqmap.TryAdd(OP_Detail, reqbytes.B2String());
                return rsp.ToBytes();
            }

            // 获取所有部门
            if (!RoleSrvc.Get(new Model.Db.Role(), new Model.Int.PageBean() { Size = 100 }, out var roles) || 0 == roles.Count)
            {
                return rsp.ToBytes();
            }

            rsp.RoleNames.AddRange(roles.Select(a => a.Roleid));

            // 没有部门信息
            if (0 == rsp.RoleNames.Count)
            {
                return rsp.ToBytes();
            }

            rsp.RoleName = req.Roleid;

            if (string.IsNullOrWhiteSpace(rsp.RoleName))
            {
                rsp.RoleName = roles.FirstOrDefault()?.Roleid;
            }

            // 无效的部门
            if (string.IsNullOrWhiteSpace(rsp.RoleName))
            {
                return rsp.ToBytes();
            }

            if (!UserSrvc.GetOne(new Model.Db.User() { Token = req.Token }, out var user))
            {
                rsp.Stat = "用户不存在";

                reqmap.TryAdd(OP_Content, $"用户不存在");
                reqmap.TryAdd(OP_Detail, $"Token:{req.Token}");

                return rsp.ToBytes();
            }

            // 获取到所有权限
            PermissionSrvc.Get(new Model.Db.Permission(), out var allps);

            RolePermissionSrvc.Get(new Model.Db.RolePermission() { Roleid = rsp.RoleName }, out var rolepermisssions);

            if (1 <= rolepermisssions.Count)
            {
                foreach (var rp in rolepermisssions.Where(a => 2 < a.Permissionid.Length))
                {
                    PagePermission pp = new PagePermission()
                    {
                        P2name = allps.FirstOrDefault(a => rp.Permissionid.Equals(a.Permissionid))?.Permissionname,
                        P3 = new List<string>()
                    };

                    if (!string.IsNullOrWhiteSpace(rp.Ispermission))
                    {
                        pp.P3.AddRange(rp.Ispermission.Split(','));
                    }

                    rsp.RolePs.Add(pp);
                }
            }

            UserSrvc.Get(new Model.Db.User() { Roleids = rsp.RoleName }, req.Page, out var users);

            req.Page.CopyPage(rsp.Page);
            
            foreach (var u in users)
            {
                UserPermissionObj up = new UserPermissionObj()
                {
                    Name = u.Username,
                    Post = u.Post,
                    RoleId = u.Roleids,
                    UserId = u.Userid,
                    Phone = u.Phone,

                    Pwd = string.Empty,
                    UserPs = new List<PagePermission>()
                };

                UserPermissionSrvc.Get(new Model.Db.UserPermission() { Userid = u.Userid }, out var userPermissions);

                if (1 <= userPermissions.Count)
                {
                    foreach (var up1 in userPermissions.Where(a => 2 < a.Permissionid.Length))
                    {
                        PagePermission pp = new PagePermission()
                        {
                            P2name = allps.FirstOrDefault(a => up1.Permissionid.Equals(a.Permissionid))?.Permissionname,
                            P3 = new List<string>()
                        };

                        if (!string.IsNullOrWhiteSpace(up1.Ispermission))
                        {
                            pp.P3.AddRange(up1.Ispermission.Split(','));
                        }

                        up.UserPs.Add(pp);
                    }
                }

                rsp.Users.Add(up);
            }

            reqmap.TryAdd(OP_User, user.Userid);
            reqmap.TryAdd(OP_Content, "获取部门下人员的菜单以及权限");

            return rsp.ToBytes();
        }

        /// <summary>
        /// 添加用户及权限
        /// </summary>
        /// <param name="reqmap"></param>
        /// <param name="reqbytes"></param>
        /// <returns></returns>
        public static byte[] AddUpd(Dictionary<string, string> reqmap, byte[] reqbytes)
        {
            UserPermissionAddRsp rsp = new UserPermissionAddRsp() { Stat = Success };

            reqbytes.Json2ObjT<UserPermissionAddReq>(out UserPermissionAddReq req);
            //进日志
            req.Debug(string.Empty, Enter);
            if (null == req)
            {
                rsp.Stat = JsonErr;
                reqmap.TryAdd(OP_Content, $"格式错误");
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

                reqmap.TryAdd(OP_Content, $"用户不存在");
                reqmap.TryAdd(OP_Detail, $"Token:{req.Token}");

                return rsp.ToBytes();
            }

            User user1 = new Model.Db.User()
            {
                Creater = user.Userid,
                Phone = req.Userinfo.Tel,
                Userid = req.Userinfo.Account,
                Roleids = req.Userinfo.Roleids,
                Username = req.Userinfo.Name,
                Post = req.Userinfo.Post
            };
            if(!string.IsNullOrWhiteSpace(req.Userinfo.Pwd))
            {
                user1.Pwd = BMD5.ToMD5String(req.Userinfo.Pwd, 32)?.ToUpper();
            }

            // 添加User
            UserSrvc.AddOrUpdate(user1, ADD); 

            reqmap.TryAdd(OP_User, user.Userid);
            reqmap.TryAdd(OP_UserDest, req.Userinfo.Account);
            reqmap.TryAdd(OP_Content, "添加用户及权限");

            PermissionSrvc.Get(new Permission(), out var allpermissions);

            // 登录用户权限集合
            List<UserPermission> permissions = new List<UserPermission>();

            foreach (var p in req.UserPermissionLsit)
            {
                if (null == p)
                {
                    continue;
                }

                UserPermission up = new UserPermission()
                {
                    Ispermission = string.Join(',', p.P3),
                    Userid = req.Userinfo.Account,
                    Permissionid = allpermissions.FirstOrDefault(a => p.P2name.Equals(a.Permissionname))?.Permissionid
                };

                permissions.Add(up);

                if (!string.IsNullOrWhiteSpace(up.Permissionid) && 2 < up.Permissionid.Length)
                {
                    // 一级权限已经存在于列表中
                    if (permissions.Any(a => up.Permissionid.Substring(0, 2).Equals(a.Permissionid)))
                    {
                        continue;
                    }

                    permissions.Add(new UserPermission()
                    {
                        Ispermission = string.Empty,
                        Userid = req.Userinfo.Account,
                        Permissionid = up.Permissionid.Substring(0, 2)
                    });
                }
            }

            rsp.Stat = UserPermissionSrvc.AddOrUpdate(permissions, req.Op);

            reqmap.TryAdd(OP_User, user.Userid);
            reqmap.TryAdd(OP_Content, "获取部门下人员的菜单以及权限");

            return rsp.ToBytes();
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="reqmap"></param>
        /// <param name="reqbytes"></param>
        /// <returns></returns>
        public static byte[] Del(Dictionary<string, string> reqmap, byte[] reqbytes)
        {
            UserPermissionAddRsp rsp = new UserPermissionAddRsp() { Stat = Success };

            reqbytes.Json2ObjT<UserPermissionAddReq>(out UserPermissionAddReq req);
            //进日志
            req.Debug(string.Empty, Enter);
            if (null == req)
            {
                rsp.Stat = JsonErr;
                reqmap.TryAdd(OP_Content, $"格式错误");
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

                reqmap.TryAdd(OP_Content, $"用户不存在");
                reqmap.TryAdd(OP_Detail, $"Token:{req.Token}");

                return rsp.ToBytes();
            }

            //删除用户权限
            rsp.Stat = UserPermissionSrvc.AddOrUpdate(new List<UserPermission>() { new UserPermission() { Userid = req.Userinfo.Account } }, DEL);

            //删除用户
            rsp.Stat = UserSrvc.AddOrUpdate(new User { Userid = req.Userinfo.Account }, DEL);

            reqmap.TryAdd(OP_User, user?.Userid);
            reqmap.TryAdd(OP_Content, "权限删除");

            return rsp.ToBytes();
        }
    }
}
