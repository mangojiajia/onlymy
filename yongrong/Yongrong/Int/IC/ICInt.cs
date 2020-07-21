using System;
using System.Collections.Generic;
using System.Text;
using Yongrong.Model.Int.Pda;
using BaseS.Serialization;
using BaseS.File.Log;
using BaseS.String;
using BaseS.Collection;
using Yongrong.Model.Int.IC;
using Yongrong.Srvc.BaseInfo;
using System.Linq;

namespace Yongrong.Int.IC
{
    class ICInt : BaseInt
    {
        /// <summary>
        /// IC 查询预约
        /// </summary>
        /// <param name="reqmap"></param>
        /// <param name="reqbytes"></param>
        /// <returns></returns>
        public static byte[] QueryOrder(Dictionary<string, string> reqmap, byte[] reqbytes)
        {
            ICQueryOrderRsp rsp = new ICQueryOrderRsp() { Stat = Success };

            reqbytes.Json2ObjT<ICQueryOrderReq>(out ICQueryOrderReq req);

            //进日志
            req.Debug("ICQueryOrderReq", Enter);
            
            if (null == req)
            {
                rsp.Stat = JsonErr;
                reqmap.TryAdd(OP_Content, rsp.Stat);
                reqmap.TryAdd(OP_Detail, reqbytes.B2String());
                return rsp.ToBytes();
            }

            if (!Sys.ICClients.Contains(reqmap.GetDicStr(Http.ClientIPName)))
            {
                rsp.Stat = "ip block";
                reqmap.TryAdd(OP_Content, rsp.Stat);
                reqmap.TryAdd(OP_Detail, reqbytes.B2String());

                return rsp.ToBytes();
            }

            OrderGoodsSrvc.Get(new Model.Db.OrderGoods() { Tractorid = req.Tractor, Orderid = req.Order, Orderstat = "1", Ordertime = DateTime.Now.ToString("yyyy-MM-dd") }, null, out var list);

            rsp.Data = list.FirstOrDefault();

            reqmap.TryAdd(OP_User, "IC");
            reqmap.TryAdd(OP_Content, "IC查询预约信息");

            return rsp.ToBytes();
        }
    }
}
