using System;
using System.Collections.Generic;

namespace Yongrong.Model.Db
{
    public partial class BaseCustomer
    {
        public decimal Id { get; set; }
        public string Enterpriseid { get; set; }
        public string Enterprisename { get; set; }
        public string White { get; set; }
        public string Supporttype { get; set; }
        public string Creditid { get; set; }
        public string Checkstat { get; set; }
        public string Remark { get; set; }
        public string Createtime { get; set; }
        public string Enterpriseshortname { get; set; }
        public string Legalname { get; set; }
        public string Regcapital { get; set; }
        public string Establishtime { get; set; }
        public string Contact { get; set; }
        public string Groupncid { get; set; }

        public string Jinjiangncid { get; set; }
        public string Pushsystemid { get; set; }
        public string Datasource { get; set; }
        public string Userdefined14 { get; set; }
        public string Userdefined15 { get; set; }

        public void Upd(BaseCustomer old)
        {
            old.Enterpriseid = this.Enterpriseid;
            old.Enterprisename = this.Enterprisename;
            old.White = this.White;
            old.Supporttype = this.Supporttype;
            old.Creditid = this.Creditid;
            old.Checkstat = this.Checkstat;
            old.Remark = this.Remark;
            old.Regcapital = this.Regcapital;
            old.Groupncid = this.Groupncid;
            old.Jinjiangncid = this.Jinjiangncid;
            old.Enterpriseshortname = this.Enterpriseshortname;
            old.Legalname = this.Legalname;
            old.Establishtime = this.Establishtime;
            old.Contact = this.Contact;
            old.Pushsystemid = this.Pushsystemid;
            old.Datasource = this.Datasource;
            old.Userdefined14 = this.Userdefined14;
            old.Userdefined15 = this.Userdefined15;
        }
    }
}
