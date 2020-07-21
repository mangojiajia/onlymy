﻿using BaseS.Security;
using BaseS.Serialization;
using BaseS.String;
using System.Collections.Generic;
using System.Linq;
using Yongrong.Model.Db;
using Yongrong.Model.Int.Org;
using Yongrong.Model.Int.Sys;
using Yongrong.Model.Int.User;
using Yongrong.Srvc.Sys;
using Yongrong.Srvc.Users;
using BaseS.File.Log;

namespace Yongrong.Int.BaseInfo
{
    class RoleInt: BaseInt
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="reqmap"></param>
        /// <param name="reqbytes"></param>
        /// <returns></returns>
        public static byte[] Get(Dictionary<string, string> reqmap, byte[] reqbytes)
        {
            RoleGetRsp rsp = new RoleGetRsp();

            reqbytes.Json2ObjT<RoleGetReq>(out RoleGetReq req);
            //进日志
            req.Debug(string.Empty, Enter);
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

            RoleSrvc.Get(
                req.Query,
                req.Page,
                out var allroles
                );

            PermissionSrvc.Get(null, out var permissions);

            // 菜单权限列表
            foreach (var p in permissions.Where(a => a.Levele == 1))
            {
                var t = new MenuBean()
                {
                    Item = p.Permissionname,
                    ItemPermission = p.Permissionid,
                    SubItems = new List<SubItem>()
                };

                var subpermissions = permissions.Where(a => a.Permissionid.StartsWith(p.Permissionid) && p.Permissionid.Length < a.Permissionid.Length);

                rsp.Menu.Add(t);

                if (null == subpermissions || 0 >= subpermissions.Count())
                {
                    continue;
                }

                foreach (var sub in subpermissions)
                {
                    t.SubItems.Add(new SubItem() { SubItemName = permissions.FirstOrDefault(a => sub.Permissionid.Equals(a.Permissionid))?.Permissionname, SubItemPermission = sub.Permissionid });
                }
            }

            
            List<RoleObj> allroleobj = new List<RoleObj>();

            foreach (var role in allroles)
            {
                var roleobj = new RoleObj()
                {
                    Account = role.Userid,
                    Createtime = role.Createtime,
                    Descript = role.Descript,
                    Roleid = role.Roleid,
                    Rolename = role.Rolename,
                    Userid = role.Userid,
                    Menuname=string.Empty

                };

                RolePermissionSrvc.Get(new RolePermission() { Roleid = role.Roleid }, out var allrolepermission);
                
                if (null != allrolepermission)
                {
                    foreach(var tt in allrolepermission.Where(a => (!string.IsNullOrWhiteSpace(a.Permissionid) && 2 < a.Permissionid.Length)))
                    {
                        roleobj.Menuname += "" + permissions.FirstOrDefault(a => tt.Permissionid.Equals(a.Permissionid))?.Permissionname + ",";
                    }

                    roleobj.Menuname = roleobj.Menuname.Replace(",,", ",").TrimEnd(',');
                }

                if (!string.IsNullOrWhiteSpace(role.Userid))
                {
                    UserSrvc.GetOne(new User() { Userid = role.Userid }, out var u);
                    roleobj.Name = u?.Username;
                    roleobj.Tel = u?.Phone;
                    roleobj.Post = u?.Post;
                }

                allroleobj.Add(roleobj);
            }

            rsp.Stat = Success;
            rsp.AllRoles = allroleobj;
            rsp.PermissionList = permissions;

            reqmap.TryAdd(OP_User, user?.Userid);
            reqmap.TryAdd(OP_Content, "查询组织架构管理");

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
            RoleAddRsp rsp = new RoleAddRsp() { Stat = Success };

            reqbytes.Json2ObjT<RoleAddReq>(out RoleAddReq req);
            //进日志
            req.Debug(string.Empty, Enter);
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
                return rsp.ToBytes();
            }

            // 添加部门
            /*RoleSrvc.AddOrUpdate(new Role()
            {
                Descript = req.Role.Desc,
                Roleid = req.Role.Roleid,
                Rolename = req.Role.Rolename,
                Userid = req.Role.Account
            }, ADD);*/

            RoleSrvc.AddOrUpdate(new Role()
            {
                Descript = req.Role.Desc,
                Roleid = req.Role.Roleid,
                Rolename = req.Role.Rolename,
                Userid = req.Role.Account
            }, req.Op);


            // 添加部门权限
            PermissionSrvc.Get(null, out var ps);
            List<RolePermission> rplist = new List<RolePermission>();

            foreach(var pname in req.Role.Menuname.Split(','))
            {
                if (string.IsNullOrWhiteSpace(pname))
                {
                    continue;
                }

                var permission = ps.FirstOrDefault(a => pname.Equals(a.Permissionname));

                if (null == permission)
                {
                    continue;
                }

                rplist.Add(new RolePermission() { Permissionid = permission.Permissionid, Roleid = req.Role.Roleid, Ispermission = permission.Ispermission });

                if (2 < permission.Permissionid.Length)
                {
                    if(rplist.Any(a => permission.Permissionid.Substring(0,2).Equals(a.Permissionid)))
                    {
                        continue;
                    }

                    var tmpp = ps.FirstOrDefault(a => permission.Permissionid.Substring(0, 2).Equals(a.Permissionid));

                    if (null != tmpp)
                    {
                        rplist.Add(new RolePermission() { Permissionid = tmpp.Permissionid, Roleid = req.Role.Roleid });
                    }
                }
            }

            if (1 <= rplist.Count)
            {
                RolePermissionSrvc.AddOrUpdate(rplist, req.Op);
            }

            User user1 = new Model.Db.User()
            {
                Creater = user.Userid == null ? "" : user.Userid.Trim(),
                Phone = req.Role.Tel == null ? "" : req.Role.Tel.Trim(),
                Userid = req.Role.Account == null ? "" : req.Role.Account.Trim(),
                Roleids = req.Role.Roleid == null ? "" : req.Role.Roleid.Trim(),
                Username = req.Role.Name == null ? "" : req.Role.Name.Trim()
            };
            if(!string.IsNullOrWhiteSpace(req.Role.Psd))
            {
                user1.Pwd = BMD5.ToMD5String(req.Role.Psd, 32)?.ToUpper();
            }

            // 添加User
            UserSrvc.AddOrUpdate(user1, req.Op);

            reqmap.TryAdd(OP_User, user?.Userid);
            reqmap.TryAdd(OP_Content, "新增修改组织架构");

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
            RoleAddRsp rsp = new RoleAddRsp() { Stat = Success };

            reqbytes.Json2ObjT<RoleAddReq>(out RoleAddReq req);
            //进日志
            req.Debug(string.Empty, Enter);
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
                return rsp.ToBytes();
            }
            
            // 限制用户

            // 删除部门
            RoleSrvc.AddOrUpdate(new Role()
            {
                Descript = req.Role.Desc,
                Roleid = req.Role.Roleid,
                Rolename = req.Role.Rolename,
                Userid = req.Role.Account
            }, DEL);

            // 删除部门权限
            RolePermissionSrvc.AddOrUpdate(new List<RolePermission>() { new RolePermission() { Roleid = req.Role.Roleid } }, DEL);

            // 删除用户权限
            UserSrvc.Get(new User() { Roleids = req.Role.Roleid }, out var rmUsers);

            foreach (var u in rmUsers)
            {
                UserPermissionSrvc.AddOrUpdate(new List<UserPermission>() { new UserPermission() { Userid = u.Userid } }, DEL);
            }

            // 删除部门下的用户
            UserSrvc.Del(new User() { Roleids = req.Role.Roleid }, DEL);

            reqmap.TryAdd(OP_User, user?.Userid);
            reqmap.TryAdd(OP_Content, $"{GetOPName(req.Op)}权限管理删除");

            return rsp.ToBytes();
        }
    }
}
