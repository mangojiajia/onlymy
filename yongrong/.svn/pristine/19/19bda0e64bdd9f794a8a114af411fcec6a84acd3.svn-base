using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Yongrong.Model.Db
{
    public partial class UserPermission
    {
        public decimal Id { get; set; }
        public string Userid { get; set; }
        public string Permissionid { get; set; }
        public string Createtime { get; set; }
        public string Ispermission { get; set; }

        public void Upd(UserPermission old)
        {
            if (null == old)
            {
                return;
            }

            old.Userid = this.Userid;
            old.Permissionid = this.Permissionid;
            old.Createtime = this.Createtime;
            old.Ispermission = this.Ispermission;
        }
    }
}
