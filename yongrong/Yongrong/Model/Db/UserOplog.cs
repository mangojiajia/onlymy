using System;
using System.Collections.Generic;
using System.Text;

namespace Yongrong.Model.Db
{
    public partial class UserOplog
    {
        public decimal Id { get; set; }
        public string Userid { get; set; }
        public string UseridDest { get; set; }
        public string Opcontent { get; set; }
        public string Detail { get; set; }
        public string Createtime { get; set; }
        public string Stat { get; set; }
        public string Cmd { get; set; }
        public string Clientip { get; set; }
        public void Upd(UserOplog old)
        {
            old.Userid = this.Userid;
            old.UseridDest = this.UseridDest;
            old.Opcontent = this.Opcontent;
            old.Detail = this.Detail;
            old.Stat = this.Stat;
            old.Cmd = this.Cmd;
            old.Clientip = this.Clientip;
        }
    }
}
