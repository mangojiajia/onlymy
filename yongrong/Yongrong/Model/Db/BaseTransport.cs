using System;
using System.Collections.Generic;

namespace Yongrong.Model.Db
{
    public partial class BaseTransport
    {
        public decimal Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Contract { get; set; }
        public string Legal { get; set; }
        public string Tel { get; set; }
        public string Mail { get; set; }
        public string Addr { get; set; }
        public string Remark { get; set; }
        public string Whiteflag { get; set; }

        public void Upd(BaseTransport old)
        {
            old.Name = this.Name;
            old.Code = this.Code;
            old.Contract = this.Contract;
            old.Legal = this.Legal;
            old.Tel = this.Tel;
            old.Mail = this.Mail;
            old.Addr = this.Addr;
            old.Remark = this.Remark;
            old.Whiteflag = this.Whiteflag;
        }
    }
}
