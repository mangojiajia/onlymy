using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Yongrong.Model.Int;

namespace Yongrong.Model.Db
{
    public partial class SafeCheck
    {
        public decimal Id { get; set; }
        public string Checktime { get; set; }
        public string Driver { get; set; }
        public string Tractorid { get; set; }
        public string Trailerid { get; set; }
        public string Expburn { get; set; }
        public string Tire { get; set; }
        public string Supercargo { get; set; }
        public string Helmet { get; set; }
        public string Smock { get; set; }
        public string Workshoe { get; set; }
        public string Remark { get; set; }
        public string Checkman { get; set; }
        public string Createtime { get; set; }

        #region 门禁通行记录汇总表所加字段
        /// <summary>
        /// 进厂时间
        /// </summary>
        [NotMapped]
        public string Entertime { get; set; }
        /// <summary>
        /// 出厂时间
        /// </summary>
        [NotMapped]
        public string Leavetime { get; set; }


        /// <summary>
        /// 证件
        /// </summary>
        [NotMapped]
        public string Certificate { get; set; }
        /// <summary>
        /// 出厂时间
        /// </summary>
        //public string Leavetime { get; set; }
        #endregion
        public void Upd(SafeCheck old)
        {
            old.Checktime = this.Checktime;
            old.Driver = this.Driver;
            old.Tractorid = this.Tractorid;
            old.Trailerid = this.Trailerid;
            old.Expburn = this.Expburn;
            old.Tire = this.Tire;
            old.Supercargo = this.Supercargo;
            old.Helmet = this.Helmet;
            old.Smock = this.Smock;
            old.Workshoe = this.Workshoe;
            old.Remark = this.Remark;
            old.Checkman = this.Checkman;
        }

        /// <summary>
        ///检查是否安检成功
        /// </summary>
        /// <returns></returns>
        [NotMapped]
        public bool IsAllPass
        {
            get
            {
                if ("合格".Equals(Expburn)
                    && "合格".Equals(Tire)
                    && "合格".Equals(Helmet)
                    && "合格".Equals(Smock)
                    && "合格".Equals(Workshoe)
                    )
                {
                    return true;
                }

                return false;
            }
        }
    }
}
