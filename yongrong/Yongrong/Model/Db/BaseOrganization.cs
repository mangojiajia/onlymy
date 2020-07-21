using System;
using System.Collections.Generic;
using System.Text;

namespace Yongrong.Model.Db
{
    /// <summary>
    /// 组织架构管理
    /// </summary>
    public partial class BaseOrganization
    {
        public decimal Id { get; set; }
        public string Department { get; set; }
        public string Position { get; set; }
        public string Name { get; set; }
        public string Tel { get; set; }
        public string Createtime { get; set; }

        public void Upd(BaseOrganization old)
        {
            old.Department = this.Department;
            old.Position = this.Position;
            old.Name = this.Name;
            old.Tel = this.Tel;
        }
    }
}
