using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Yongrong.Model.Int.Weigh
{
    [DataContract]
    public class WeighBatchInsertReq
    {
        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name ="token")]
        public string Token { get; set; }

        [DataMember(Name ="data")]
        public DataT Data { get; set; }

        
        public string SignMethod { get; set; }

        
        public string Sign { get; set; }

        
        public string TimeStamp { get; set; }

        
        public string Version { get; set; }
    }

    [DataContract]
    public class DataT
    {
        [DataMember(Name ="db_name")]
        public string DbName { get; set; }

        [DataMember(Name = "sql_command_id")]
        public string SqlCmdId { get; set; }

        [DataMember(Name = "params")]
        public List<ParamObj> Params { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    [DataContract]
    public class ParamObj
    {
        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "value")]
        public string Val { get; set; }
    }

    public class DataParamExt : DataParam
    {
        public string Driver { get; set; }

        public string Starttime { get; set; }

        public string Endtime { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<ParamObj> ToParamsFormat()
        {
            var tmp = new List<ParamObj>();

            tmp.Add(new ParamObj() { Name = "Order_id", Val = Order_id });
            tmp.Add(new ParamObj() { Name = "Msg_id", Val = Msg_id });
            tmp.Add(new ParamObj() { Name = "Weigh_time", Val = Weigh_time });
            tmp.Add(new ParamObj() { Name = "Order_type", Val = Order_type });
            tmp.Add(new ParamObj() { Name = "Driver", Val = Driver });
            tmp.Add(new ParamObj() { Name = "Tractor_id", Val = Tractor_id });
            tmp.Add(new ParamObj() { Name = "Trailer_id", Val = Trailer_id });
            tmp.Add(new ParamObj() { Name = "Starttime", Val = Starttime });
            tmp.Add(new ParamObj() { Name = "Endtime", Val = Endtime });
            tmp.Add(new ParamObj() { Name = "Sign", Val = Sign });

            return tmp;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class DataParam
    {
        public string Order_id { get; set; }

        public string Msg_id { get; set; }

        public string Weigh_time { get; set; }

        public string Order_type { get; set; }

        public string Tractor_id { get; set; }

        public string Trailer_id { get; set; }

        public string Sign { get; set; }

        
    }
}
