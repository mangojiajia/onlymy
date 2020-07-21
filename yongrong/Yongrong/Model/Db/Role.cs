using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Yongrong.Model.Db
{
    public partial class Role
    {
        public string Roleid { get; set; }
        public string Rolename { get; set; }
        public string Descript { get; set; }
        public string Createtime { get; set; }
        public string Isenable { get; set; }
        public string Userid { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="old"></param>
        public void Upd(Role old)
        {
            old.Roleid = this.Roleid;
            old.Rolename = this.Rolename;
            old.Descript = this.Descript;
            old.Createtime = this.Createtime;
            old.Isenable = this.Isenable;
            old.Userid = this.Userid;
        }
    }
}
