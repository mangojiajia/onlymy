using System;
using System.Collections.Generic;

namespace Yongrong.Model.Db
{
    public partial class User
    {
        public decimal Id { get; set; }
        public string Userid { get; set; }
        public string Pwd { get; set; }
        public string Phone { get; set; }
        public string Roleids { get; set; }
        public string Logintime { get; set; }
        public string Creater { get; set; }
        public string Createtime { get; set; }
        public string Token { get; set; }

        public DateTime? Flashtime { get; set; }


        public string Username { get; set; }

        public string Post { get; set; }


        public void Upd(User old)
        {
            if (!string.IsNullOrWhiteSpace(this.Pwd))
            {
                old.Pwd = this.Pwd;
            }
            old.Phone = this.Phone;
            old.Roleids = this.Roleids;
            old.Logintime = this.Logintime;
            old.Creater = this.Creater;
            if (!string.IsNullOrWhiteSpace(this.Token))
            {
                old.Token = this.Token;
            }
            old.Flashtime = this.Flashtime;
            old.Username = this.Username;
            old.Post = this.Post;
        }

    }
}
