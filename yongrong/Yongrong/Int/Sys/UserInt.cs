﻿using System;
using System.Collections.Generic;
using System.Text;
using BaseS.Serialization;
using Yongrong.Model.Int.User;
using Yongrong.Srvc;
using Yongrong.Srvc.Users;
using System.Linq;
using Yongrong.Model.Int;
using Yongrong.Srvc.Sys;
using BaseS.String;
using BaseS.Collection;
using BaseS.Const;
using BaseS.Security;
using BaseS.File.Log;
using Yongrong.Model.Db;
using Yongrong.Model.Base;

namespace Yongrong.Int
{
    class UserInt : BaseInt
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="reqmap"></param>
        /// <param name="reqbytes"></param>
        /// <returns></returns>
        public static byte[] Login(Dictionary<string, string> reqmap, byte[] reqbytes)
        {
            UserLoginRsp rsp = new UserLoginRsp() { Stat = Success };

            reqbytes.Json2ObjT<UserLoginReq>(out UserLoginReq req);
            //进日志
            req.Debug(string.Empty, Enter);
            if (null == req)
            {
                rsp.Stat = JsonErr;
                
                reqmap.TryAdd(OP_Content, "格式错误");
                reqmap.TryAdd(OP_Detail, reqbytes.B2String());
                reqmap.TryAdd(OP_User, req.UserId);

                return rsp.ToBytes();
            }

            if (!UserSrvc.Login(new Model.Db.User() { Userid = req.UserId, Token = req.Token }, out var user ))
            {
                rsp.Stat = "用户不存在";

                reqmap.TryAdd(OP_Content, "用户不存在");
                reqmap.TryAdd(OP_User, req.UserId);

                return rsp.ToBytes();
            }

            if (ConstRole.DRIVER.Equals(user.Roleids) || ConstRole.SIJI.Equals(user.Roleids) || ConstRole.EDRIVER.Equals(user.Roleids))
            {
                rsp.Stat = "司机账号不能在电脑上登录";

                reqmap.TryAdd(OP_Content, "司机不能在电脑上登录");
                reqmap.TryAdd(OP_User, req.UserId);

                return rsp.ToBytes();
            }

            // 对输入的密码进行md5加密处理
            var pwdmd5 = BMD5.ToMD5String(req.Pwd, 32, null, true).ToUpper();

            if (!pwdmd5.Equals(user.Pwd))
            {
                rsp.Stat = "密码错误";

                reqmap.TryAdd(OP_Content, "密码错误");
                reqmap.TryAdd(OP_User, req.UserId);

                return rsp.ToBytes();
            }

            rsp.Token = user.Token;

            UserPermissionSrvc.Get(new Model.Db.UserPermission() { Userid = req.UserId },out var userpermissions);
            PermissionSrvc.Get(null, out var permissions);

            if (null == userpermissions)
            {
                rsp.Stat = $"{req.UserId}用户无权限";
                
                reqmap.TryAdd(OP_Content, "用户无权限");
                reqmap.TryAdd(OP_User, req.UserId);

                return rsp.ToBytes();
            }

            rsp.User = new Model.Db.User()
            {
                Roleids = user.Roleids,
                Userid = user.Userid,
                Username = user.Username
            };

            RoleSrvc.Get(new Model.Db.Role() { Roleid = user.Roleids },null, out var role);

            rsp.Role = role.FirstOrDefault();

            // 菜单权限列表
            foreach(var p in permissions.Where(a => a.Levele == 1))
            {
                // 无此权限
                if (!userpermissions.Any(a => p.Permissionid.Equals(a.Permissionid)))
                {
                    continue;
                }

                var t = new MenuBean()
                {
                    Item = p.Permissionname,
                    ItemPermission = p.Permissionid,
                    SubItems = new List<SubItem>()
                };

                var subpermissions = userpermissions.Where(a => a.Permissionid.StartsWith(p.Permissionid) && p.Permissionid.Length < a.Permissionid.Length);

                rsp.Menu.Add(t);

                if (null == subpermissions || 0 >= subpermissions.Count())
                {
                    continue;
                }

                foreach (var sub in subpermissions)
                {
                    var subtt = new SubItem()
                    {
                        SubItemName = permissions.FirstOrDefault(a => sub.Permissionid.Equals(a.Permissionid))?.Permissionname,
                        SubItemPermission = sub.Permissionid
                    };

                    if (!string.IsNullOrWhiteSpace(sub.Ispermission))
                    {
                        subtt.P3.AddRange(sub.Ispermission.Split(','));
                    }

                    t.SubItems.Add(subtt);
                }
            }

            TodoListSrvc.Get(user.Userid, req.Page, out var todolists);

            rsp.TodoList.AddRange(todolists.Where(a => "0".Equals(a.Flag)));
            rsp.DoneList.AddRange(todolists.Where(a => "1".Equals(a.Flag)));
            rsp.Expire = DateTime.Now.AddDays(1.0).ToString(BTip.TimeFormater);

            reqmap.TryAdd(OP_Content, "用户登录成功");
            reqmap.TryAdd(OP_User, req.UserId);
            reqmap.TryAdd(OP_Detail, $"Token:{user.Token}");

            return rsp.ToBytes();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reqmap"></param>
        /// <param name="reqbytes"></param>
        /// <returns></returns>
        public static byte[] Logout(Dictionary<string, string> reqmap, byte[] reqbytes)
        {
            BaseRsp rsp = new BaseRsp() { Stat = Success };

            reqbytes.Json2ObjT<UserLogoutReq>(out UserLogoutReq req);
            //进日志
            req.Debug(string.Empty, Enter);

            if (null == req)
            {
                rsp.Stat = JsonErr;

                reqmap.TryAdd(OP_Content, "格式错误");
                reqmap.TryAdd(OP_UserDest, "");
                reqmap.TryAdd(OP_Detail, reqbytes.B2String());
                reqmap.TryAdd(OP_User, req.UserId);

                return rsp.ToBytes();
            }

            if (!UserSrvc.GetOne(new Model.Db.User() { Userid = req.UserId, Token = req.Token }, out var user))
            {
                rsp.Stat = "用户不存在";

                reqmap.TryAdd(OP_Content, "用户不存在");
                reqmap.TryAdd(OP_UserDest, "");
                reqmap.TryAdd(OP_Detail, "");
                reqmap.TryAdd(OP_User, req.UserId);

                return rsp.ToBytes();
            }

            reqmap.TryAdd(OP_User, user?.Userid);
            reqmap.TryAdd(OP_Content, $"退出登录");
            return rsp.ToBytes();
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="reqmap"></param>
        /// <param name="reqbytes"></param>
        /// <returns></returns>
        public static byte[] ResetPwd(Dictionary<string, string> reqmap, byte[] reqbytes)
        {
            BaseRsp rsp = new BaseRsp() { Stat = Success };

            reqbytes.Json2ObjT<UserResetPwdReq>(out UserResetPwdReq req);
            //进日志
            req.Debug(string.Empty, Enter);

            if (null == req)
            {
                rsp.Stat = JsonErr;

                reqmap.TryAdd(OP_Content, "格式错误");
                reqmap.TryAdd(OP_UserDest, "");
                reqmap.TryAdd(OP_Detail, reqbytes.B2String());
                reqmap.TryAdd(OP_User, req.UserId);

                return rsp.ToBytes();
            }

            if (!UserSrvc.GetOne(new Model.Db.User() { Userid = req.UserId, Token = req.Token }, out var user))
            {
                rsp.Stat = "用户不存在";

                reqmap.TryAdd(OP_Content, "用户不存在");
                reqmap.TryAdd(OP_UserDest, "");
                reqmap.TryAdd(OP_Detail, "");
                reqmap.TryAdd(OP_User, req.UserId);

                return rsp.ToBytes();
            }

            if(!user.Pwd.Equals(req.OldPwd))
            {
                rsp.Stat = "原密码不正确";
                return rsp.ToBytes();
            }

            user.Pwd = req.NewPwd;
            UserSrvc.AddOrUpdate(user, UPD);

            reqmap.TryAdd(OP_User, user?.Userid);
            reqmap.TryAdd(OP_Content, $"修改密码");

            return rsp.ToBytes();
        }
    }
}
