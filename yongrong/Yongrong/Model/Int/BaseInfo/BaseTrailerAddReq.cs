using System;
using System.Collections.Generic;
using System.Text;
using Yongrong.Model.Db;

namespace Yongrong.Model.Int.BaseInfo
{
    public class BaseTrailerAddReq : BaseInfoReq
    {
        public BaseTrailer Trailer { get; set; }

        public override string Check()
        {
            if ("del".Equals(Op))
            {
                return Success;
            }

            if (null == Trailer)
            {
                return "参数为空";
            }

            if(string.IsNullOrWhiteSpace(Trailer.TrailerId))
            {
                return "挂车车牌为空";
            }

            Trailer.TrailerId = Trailer.TrailerId.Trim();

            if (string.IsNullOrWhiteSpace(Trailer.Transport))
            {
                return "运输公司为空";
            }

            Trailer.Transport = Trailer.Transport.Trim();

            if (string.IsNullOrWhiteSpace(Trailer.Carflag))
            {
                return "挂车证件标号不能为空";
            }

            Trailer.Carflag = Trailer.Carflag.Trim();

            if (string.IsNullOrWhiteSpace(Trailer.Carid))
            {
                return "运输车编号不能为空";
            }

            Trailer.Carid = Trailer.Carid.Trim();


            if (string.IsNullOrWhiteSpace(Trailer.Validdate))
            {
                return "有效期不能为空";
            }

            Trailer.Validdate = Trailer.Validdate.Trim();

            if ( 0 >= Trailer.Maxweight)
            {
                return "最大荷载量应大于0";
            }

            return Success;
        }
    }
}
