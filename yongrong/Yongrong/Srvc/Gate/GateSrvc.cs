﻿using BaseS.Net.Http;
using BaseS.Net.Http.Bean;
using BaseS.String;
using System;
using System.Collections.Generic;
using System.Text;
using Yongrong.Model.Int.Gate;
using BaseS.Serialization;
using BaseS.Security;
using BaseS.File.Log;
using Apache.NMS;
using Apache.NMS.ActiveMQ;
using Apache.NMS.ActiveMQ.Commands;
using System.IO;
using MQtest;
using Yongrong.Model.Db;
using BaseS.Const;
using System.Collections.Concurrent;
using System.Threading;
using Yongrong.Srvc.Sys;
using Yongrong.Srvc.BaseInfo;
using System.Linq;

namespace Yongrong.Srvc.Gate
{
    /// <summary>
    /// 门禁服务对接
    /// </summary>
    public class GateSrvc : BaseSrvc
    {
        /// <summary>
        /// 
        /// </summary>
        private static ConcurrentQueue<ApiGateevent> gateapiQueue = new ConcurrentQueue<ApiGateevent>();

        /// <summary>
        /// 3.2.6获取默认用户UUID
        /// </summary>
        /// <returns></returns>
        public static bool GetDefaultUUID(DateTime cur, out string uuid)
        {
            uuid = string.Empty;

            var url = "/openapi/service/base/user/getDefaultUserUuid";

            GateTokenGetReq req = new GateTokenGetReq()
            {
                Appkey = Gate.Appkey,
                Time = BString.Get1970ToNowMilliseconds(cur).ToString()
            };

            req.Obj2JsonT<GateTokenGetReq>(out string tmp);

            var token = BMD5.ToMD5String($"{url}{tmp}{Gate.Secret}", 32, null, true);

            HttpReqBean http = new HttpReqBean()
            {
                Url = $"{Gate.Url}{url}?token={token}",
                Param = tmp,
                ContentType = "application/json"
            };

            BHttp.PostSync(http);

            if (!http.Stat)
            {
                "Http调用失败,获取默认用户UUID失败".Warn();
                return false;
            }

            http.HttpRsp.Json2ObjT<GateGetUUIdRsp>(out GateGetUUIdRsp gatersp);

            if (null == gatersp)
            {
                $"获取默认用户UUID失败,反序列化失败,Rsp:{http.HttpRsp}".Warn();
                return false;
            }

            if (0 == gatersp.ErrorCode)
            {
                uuid = gatersp.Data;
            }

            return !string.IsNullOrWhiteSpace(uuid);
        }

        /// <summary>
        /// 5.6.3        添加固定车【V2.8.2】
        /// </summary>
        /// <returns></returns>
        public static bool AddCarInfo(GateAddCarReq req)
        {
            DateTime cur = DateTime.Now;

            if (!GetDefaultUUID(cur, out var uuid))
            {
                "uuid为空".Warn();
                return false;
            }

            var url = "/openapi/service/pms/vehicle/addCarInfo";

            req.Appkey = Gate.Appkey;
            req.Time = BString.Get1970ToNowMilliseconds(cur);
            req.OpUserUuid = uuid;

            req.Obj2JsonT<GateAddCarReq>(out string tmp);

            var token = BMD5.ToMD5String($"{url}{tmp}{Gate.Secret}", 32, null, true);

            HttpReqBean http = new HttpReqBean()
            {
                Url = $"{Gate.Url}{url}?token={token}",
                Param = tmp,
                ContentType = "application/json"
            };

            BHttp.PostSync(http);

            if (!http.Stat)
            {
                $"{http.HttpRsp}".Warn();
                return false;
            }

            http.HttpRsp.Json2ObjT<GateAddCarRsp>(out GateAddCarRsp rsp);

            //新增门禁日志
            GateLog gatelog = new GateLog
            {
                Tractorid = req?.PlateNo,
                Cartype = "" + req?.CarType,
                Operation = url,
                Rspcode = "" + rsp?.ErrorCode,
                Reqparameter = tmp,
                Rspmessage = http.HttpRsp
            };

            GateLogSrvc.AddOrUpdate(gatelog, ADD);

            if (0 != rsp.ErrorCode)
            {
                //新增异常表数据
                Abnormal abnormal = new Abnormal
                {
                    Abnormaltype = "添加固定车位失败",
                    Abnormalname = "门禁",
                    Abnormalcase = $"车牌:{req.PlateNo},错误信息:{http.HttpRsp}"
                };

                AbnormalSrvc.AddOrUpdate(abnormal, ADD);
            }

            rsp.Info("应答对象");

            return false;
        }

        /// <summary>
        /// 5.6.2 分页获取固定车【V2.8.2】
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public static bool GetCarInfos(GateGetCarInfoReq req)
        {
            DateTime cur = DateTime.Now;

            if (!GetDefaultUUID(cur, out var uuid))
            {
                "uuid为空".Warn();
                return false;
            }

            var url = "/openapi/service/pms/vehicle/getCarInfos";

            req.Appkey = Gate.Appkey;
            req.Time = BString.Get1970ToNowMilliseconds(cur);
            req.OpUserUuid = uuid;

            req.Obj2JsonT<GateGetCarInfoReq>(out string tmp);

            var token = BMD5.ToMD5String($"{url}{tmp}{Gate.Secret}", 32, null, true);

            HttpReqBean http = new HttpReqBean()
            {
                Url = $"{Gate.Url}{url}?token={token}",
                Param = tmp,
                ContentType = "application/json"
            };

            BHttp.PostSync(http);

            if (!http.Stat)
            {
                $"{http.HttpRsp}".Warn();
                return false;
            }

            http.HttpRsp.Json2ObjT<GateGetCarInfoRsp>(out GateGetCarInfoRsp rsp);

            rsp.Info("应答对象");

            //新增门禁日志
            GateLog gatelog = new GateLog
            {
                Tractorid = req?.PlateNo,
                Cartype = "" + rsp?.Data?.ObjList?.FirstOrDefault()?.CarType,
                Operation = url,
                Rspcode = "" + rsp?.ErrorCode,
                Reqparameter = tmp,
                Rspmessage = http.HttpRsp
            };

            GateLogSrvc.AddOrUpdate(gatelog, ADD);

            if (1 <= rsp?.Data?.Total)
            {
                return true;
            }

            if (0 != rsp.ErrorCode)
            {
                //新增异常表数据
                Abnormal abnormal = new Abnormal
                {
                    Abnormaltype = "获取固定车位失败",
                    Abnormalname = "门禁",
                    Abnormalcase = $"车牌:{req.PlateNo},错误信息:{http.HttpRsp}"
                };

                AbnormalSrvc.AddOrUpdate(abnormal, ADD);
            }
            return false;
        }

        /// <summary>
        /// 5.6.4        删除固定车【V2.8.2】
        /// </summary>
        /// <returns></returns>
        public static bool DelCarInfo(GateDelCarReq req)
        {
            DateTime cur = DateTime.Now;

            if (!GetDefaultUUID(cur, out var uuid))
            {
                "uuid为空".Warn();
                return false;
            }

            req.Appkey = Gate.Appkey;
            req.Time = BString.Get1970ToNowMilliseconds(cur);
            req.OpUserUuid = uuid;

            req.Obj2JsonT<GateDelCarReq>(out string tmp);

            var url = "/openapi/service/pms/vehicle/deleteCarInfo";

            var token = BMD5.ToMD5String($"{url}{tmp}{Gate.Secret}", 32, null, true);

            HttpReqBean http = new HttpReqBean()
            {
                Url = $"{Gate.Url}{url}?token={token}",
                Param = tmp,
                ContentType = "application/json"
            };

            BHttp.PostSync(http);

            if (!http.Stat)
            {
                $"{http.HttpRsp}".Warn();
                return false;
            }

            http.HttpRsp.Json2ObjT<GateDelCarRsp>(out GateDelCarRsp rsp);

            rsp.Info("应答对象");

            return false;
        }

        /// <summary>
        /// 5.2.1        获取所有停车场【V2.8.2】
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public static bool GetParking(GateGetParkingReq req)
        {
            DateTime cur = DateTime.Now;

            if (!GetDefaultUUID(cur, out var uuid))
            {
                "uuid为空".Warn();
                return false;
            }

            req.Appkey = Gate.Appkey;
            req.Time = BString.Get1970ToNowMilliseconds(cur);
            req.OpUserUuid = uuid;

            req.Obj2JsonT<GateGetParkingReq>(out string tmp);

            var url = "/openapi/service/pms/res/getParkingInfos";

            var token = BMD5.ToMD5String($"{url}{tmp}{Gate.Secret}", 32, null, true);

            HttpReqBean http = new HttpReqBean()
            {
                Url = $"{Gate.Url}{url}?token={token}",
                Param = tmp,
                ContentType = "application/json"
            };

            BHttp.PostSync(http);

            if (!http.Stat)
            {
                $"{http.HttpRsp}".Warn();
                return false;
            }

            http.HttpRsp.Json2ObjT<GateGetParkingRsp>(out GateGetParkingRsp rsp);

            rsp.Info("应答对象");

            return false;
        }

        /// <summary>
        /// 5.6.1        固定车充值【V2.8.2】
        /// </summary>
        /// <returns></returns>
        public static bool RechargeCarInfo(GateRechargeCarReq req)
        {
            DateTime cur = DateTime.Now;

            if (!GetDefaultUUID(cur, out var uuid))
            {
                "uuid为空".Warn();
                return false;
            }

            req.Appkey = Gate.Appkey;
            req.Time = BString.Get1970ToNowMilliseconds(cur);
            req.OpUserUuid = uuid;

            req.Obj2JsonT<GateRechargeCarReq>(out string tmp);


            var url = "/openapi/service/pms/vehicle/carRecharge";
            var token = BMD5.ToMD5String($"{url}{tmp}{Gate.Secret}", 32, null, true);

            HttpReqBean http = new HttpReqBean()
            {
                Url = $"{Gate.Url}{url}?token={token}",
                Param = tmp,
                ContentType = "application/json"
            };

            BHttp.PostSync(http);

            if (!http.Stat)
            {
                $"{http.HttpRsp}".Warn();
                return false;
            }

            http.HttpRsp.Json2ObjT<GateRechargeCarRsp>(out GateRechargeCarRsp rsp);

            //新增门禁日志
            GateLog gatelog = new GateLog
            {
                Tractorid = req?.PlateNo,
                Operation = url,
                Rspcode = "" + rsp?.ErrorCode,
                Reqparameter = tmp,
                Rspmessage = http.HttpRsp
            };

            GateLogSrvc.AddOrUpdate(gatelog, ADD);

            if (0 != rsp.ErrorCode)
            {
                //新增异常表数据
                Abnormal abnormal = new Abnormal
                {
                    Abnormaltype = "固定车位充值失败",
                    Abnormalname = "门禁",
                    Abnormalcase = $"充值车牌:{req.PlateNo},错误信息:{http.HttpRsp}"
                };

                AbnormalSrvc.AddOrUpdate(abnormal, ADD);
            }

            rsp.Info("应答对象");

            return false;
        }

        /// <summary>
        /// 13.1.3   获取事件类型【V2.8.2】
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public static bool GetEventTypes(GateGetEventTypeReq req)
        {
            DateTime cur = DateTime.Now;

            if (!GetDefaultUUID(cur, out var uuid))
            {
                "uuid为空".Warn();
                return false;
            }

            req.Appkey = Gate.Appkey;
            req.Time = BString.Get1970ToNowMilliseconds(cur);
            req.OpUserUuid = uuid;

            req.Obj2JsonT<GateGetEventTypeReq>(out string tmp);

            var url = "/openapi/service/eps/getEventTypes";
            var token = BMD5.ToMD5String($"{url}{tmp}{Gate.Secret}", 32, null, true);

            HttpReqBean http = new HttpReqBean()
            {
                Url = $"{Gate.Url}{url}?token={token}",
                Param = tmp,
                ContentType = "application/json"
            };

            BHttp.PostSync(http);

            if (!http.Stat)
            {
                $"{http.HttpRsp}".Warn();
                return false;
            }

            http.HttpRsp.Json2ObjT<GateGetEventTypeRsp>(out GateGetEventTypeRsp rsp);

            rsp.Info("应答对象");

            return false;
        }

        /// <summary>
        /// 13.1.1   事件订阅【V2.9.1】
        /// </summary>
        /// <returns></returns>
        public static bool SubscribeEventsFromMQEx(GateSubscribeEventsReq req, out GateSubscribeEventsRsp rsp)
        {
            DateTime cur = DateTime.Now;
            rsp = null;

            if (!GetDefaultUUID(cur, out var uuid))
            {
                "uuid为空".Warn();
                return false;
            }

            req.Appkey = Gate.Appkey;
            req.Time = BString.Get1970ToNowMilliseconds(cur);
            req.OpUserUuid = uuid;

            req.Obj2JsonT<GateSubscribeEventsReq>(out string tmp);

            var url = "/openapi/service/eps/subscribeEventsFromMQEx";
            var token = BMD5.ToMD5String($"{url}{tmp}{Gate.Secret}", 32, null, true);

            HttpReqBean http = new HttpReqBean()
            {
                Url = $"{Gate.Url}{url}?token={token}",
                Param = tmp,
                ContentType = "application/json"
            };

            BHttp.PostSync(http);

            if (!http.Stat)
            {
                $"{http.HttpRsp}".Warn();
                return false;
            }

            http.HttpRsp.Json2ObjT<GateSubscribeEventsRsp>(out rsp);

            rsp.Info("应答对象");

            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mqURL"></param>
        /// <param name="destTopic"></param>
        public static void InitConsumer(string mqURL, string destTopic)
        {
            if (string.IsNullOrWhiteSpace(mqURL))
            {
                mqURL = Gate.MqUrl;
            }

            if (string.IsNullOrWhiteSpace(destTopic))
            {
                destTopic = Gate.MqTopic;
            }

            IConnectionFactory factory = new ConnectionFactory(new Uri("activemq:failover:(tcp://" + mqURL + ",tcp://" + mqURL + ")"));
            //通过工厂创建连接
            IConnection connection = factory.CreateConnection();

            //启动连接
            $"启动连接:activemq:failover:(tcp://{mqURL},tcp://{mqURL})".Info();
            connection.Start();
            ISession session = connection.CreateSession();

            //通过会话创建一个消费者
            IMessageConsumer consumer = session.CreateConsumer(new Apache.NMS.ActiveMQ.Commands.ActiveMQTopic(destTopic));

            //注册监听事件
            $"监听MQ的{destTopic}成功".Info();

            consumer.Listener += new MessageListener(Consumer_Listener);
        }

        /// <summary>
        /// 消费者监听接收事件
        /// </summary>
        /// <param name="message"></param>
        private static void Consumer_Listener(IMessage message)
        {
            try
            {
                ActiveMQMessage msg = (ActiveMQMessage)message;
                byte[] ct = msg.Content;
                Stream st = new MemoryStream(ct);
                CommEventLog eventLog = new MQtest.CommEventLog();
                eventLog = ProtoBuf.Serializer.Deserialize<CommEventLog>(st);

                // eventLog.EventToString().Debug();

                // 非停车场事件不处理
                if (103355478 != eventLog.SubSysType)
                {
                    $"非停车场事件不处理".Notice();
                    return;
                }

                byte[] extByte = eventLog.ExtInfo;
                Stream extSt = new MemoryStream(extByte);
                MsgPmsEvent msgPmsEvent = new MsgPmsEvent();
                msgPmsEvent = ProtoBuf.Serializer.Deserialize<MsgPmsEvent>(extSt);
                //停车场事件字段，透传（过车事件）
                CEmuEvent cEmuEvent = msgPmsEvent.PmsEvent;
                //停车诱导事件，透传（车位变更事件）
                CPlaceEvent cPlaceEvent = msgPmsEvent.PgsEvent;
                //人员信息
                PmsPersonInfo pmsPersonInfo = msgPmsEvent.PersonInfo;

                var gateevent = new ApiGateevent()
                {
                    Subsystype = eventLog.SubSysType,
                    Eventtype = eventLog.EventType,
                    Eventtypename = eventLog.EventTypeName,
                    Starttime = eventLog.StartTime,
                    Stoptime = eventLog.StopTime,
                    Eventcmd = cEmuEvent.EventCmd,
                    Parkname = cEmuEvent.ParkName,
                    Gatename = cEmuEvent.GateName,
                    Roadwayname = cEmuEvent.RoadwayName,
                    Vehicletype = cEmuEvent.VehicleType,
                    Platetype = cEmuEvent.PlateType,
                    Vehiclecolor = cEmuEvent.VehicleColor,
                    Platecolor = cEmuEvent.PlateColor,
                    Plateno = cEmuEvent.PlateNo,
                    Cardno = cEmuEvent.CardNo,
                    Roadwaytype = cEmuEvent.RoadwayType,
                    Alarmcar = cEmuEvent.AlarmCar,
                    Createtime = DateTime.Now.ToString(BTip.DateFormater)
                };

                // 无效的停车场消息也跳过
                if (Gate.InvalidParkings.Contains(gateevent.Parkname))
                {
                    return;
                }

                if (!string.IsNullOrWhiteSpace(gateevent.Stoptime)
                    && (gateevent.Stoptime.StartsWith(DateTime.Now.ToString("yyyy-MM-dd"))
                    || gateevent.Stoptime.StartsWith(DateTime.Now.AddDays(-1.0).ToString("yyyy-MM-dd"))))
                {
                    gateevent.Info("门禁信息");
                }
                else
                {
                    // 非当日或昨日消息无效
                    return;
                }

                gateapiQueue.Enqueue(gateevent);

                ThreadPool.QueueUserWorkItem(GateToTrigger);
            }
            catch (Exception e)
            {
                $"Err:{e.Message} StackTrace:{e.StackTrace} Inner:{e?.InnerException}".Warn();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="o"></param>
        private static void GateToTrigger(object o)
        {
            List<ApiGateevent> gatelist = new List<ApiGateevent>();

            using (var db = DbContext)
            {
                while (!gateapiQueue.IsEmpty)
                {
                    gateapiQueue.TryDequeue(out var gate);

                    if (null == gate)
                    {
                        $"gateapiQueue获取到的对象为null,异常数据".Warn();
                        continue;
                    }

                    if (Gate.EventCmdInvalid.Contains(gate.Eventcmd ?? 0))
                    {
                        $"无效的事件命令值{gate.Eventcmd}".Warn();
                        continue;
                    }

                    if (!BaseTractorSrvc.Contains(gate.Plateno))
                    {
                        $"{gate.Plateno}此车牌不在智能物流系统中".Warn();
                        continue;
                    }

                    gate.Id = GetSeq("SEQ_APIGATEEVENT");

                    gatelist.Add(gate);

                    if (30 <= gatelist.Count)
                    {
                        db.ApiGateevent.AddRange(gatelist);

                        SaveChanges(db, "GateToTrigger");

                        gatelist.Clear();
                    }
                }

                if (1 <= gatelist.Count)
                {
                    db.ApiGateevent.AddRange(gatelist);

                    SaveChanges(db, "GateToTrigger-2");

                    gatelist.Clear();
                }
            }
        }
    }
}
