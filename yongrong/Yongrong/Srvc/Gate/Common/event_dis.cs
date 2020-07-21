// This file was generated by a tool; you should avoid making direct changes.
// Consider using 'partial classes' to extend these types
// Input: event_dis.proto

#pragma warning disable CS1591, CS0612, CS3021, IDE1006
using System.Text;

namespace MQtest
{

    [global::ProtoBuf.ProtoContract()]
    public partial class CommEventLog : global::ProtoBuf.IExtensible
    {
        private global::ProtoBuf.IExtension __pbn__extensionData;
        global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
            => global::ProtoBuf.Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

        [global::ProtoBuf.ProtoMember(1, Name = @"log_id", IsRequired = true)]
        public string LogId { get; set; }

        [global::ProtoBuf.ProtoMember(2, Name = @"event_state", IsRequired = true)]
        public int EventState { get; set; }

        [global::ProtoBuf.ProtoMember(3, Name = @"event_level")]
        public int EventLevel
        {
            get { return __pbn__EventLevel.GetValueOrDefault(); }
            set { __pbn__EventLevel = value; }
        }
        public bool ShouldSerializeEventLevel() => __pbn__EventLevel != null;
        public void ResetEventLevel() => __pbn__EventLevel = null;
        private int? __pbn__EventLevel;

        [global::ProtoBuf.ProtoMember(4, Name = @"unit_idx")]
        [global::System.ComponentModel.DefaultValue("")]
        public string UnitIdx
        {
            get { return __pbn__UnitIdx ?? ""; }
            set { __pbn__UnitIdx = value; }
        }
        public bool ShouldSerializeUnitIdx() => __pbn__UnitIdx != null;
        public void ResetUnitIdx() => __pbn__UnitIdx = null;
        private string __pbn__UnitIdx;

        [global::ProtoBuf.ProtoMember(5, Name = @"event_type")]
        public int EventType
        {
            get { return __pbn__EventType.GetValueOrDefault(); }
            set { __pbn__EventType = value; }
        }
        public bool ShouldSerializeEventType() => __pbn__EventType != null;
        public void ResetEventType() => __pbn__EventType = null;
        private int? __pbn__EventType;

        [global::ProtoBuf.ProtoMember(6, Name = @"event_type_name")]
        [global::System.ComponentModel.DefaultValue("")]
        public string EventTypeName
        {
            get { return __pbn__EventTypeName ?? ""; }
            set { __pbn__EventTypeName = value; }
        }
        public bool ShouldSerializeEventTypeName() => __pbn__EventTypeName != null;
        public void ResetEventTypeName() => __pbn__EventTypeName = null;
        private string __pbn__EventTypeName;

        [global::ProtoBuf.ProtoMember(7, Name = @"sub_sys_type")]
        public int SubSysType
        {
            get { return __pbn__SubSysType.GetValueOrDefault(); }
            set { __pbn__SubSysType = value; }
        }
        public bool ShouldSerializeSubSysType() => __pbn__SubSysType != null;
        public void ResetSubSysType() => __pbn__SubSysType = null;
        private int? __pbn__SubSysType;

        [global::ProtoBuf.ProtoMember(8, Name = @"event_name")]
        [global::System.ComponentModel.DefaultValue("")]
        public string EventName
        {
            get { return __pbn__EventName ?? ""; }
            set { __pbn__EventName = value; }
        }
        public bool ShouldSerializeEventName() => __pbn__EventName != null;
        public void ResetEventName() => __pbn__EventName = null;
        private string __pbn__EventName;

        [global::ProtoBuf.ProtoMember(9, Name = @"start_time")]
        [global::System.ComponentModel.DefaultValue("")]
        public string StartTime
        {
            get { return __pbn__StartTime ?? ""; }
            set { __pbn__StartTime = value; }
        }
        public bool ShouldSerializeStartTime() => __pbn__StartTime != null;
        public void ResetStartTime() => __pbn__StartTime = null;
        private string __pbn__StartTime;

        [global::ProtoBuf.ProtoMember(10, Name = @"stop_time")]
        [global::System.ComponentModel.DefaultValue("")]
        public string StopTime
        {
            get { return __pbn__StopTime ?? ""; }
            set { __pbn__StopTime = value; }
        }
        public bool ShouldSerializeStopTime() => __pbn__StopTime != null;
        public void ResetStopTime() => __pbn__StopTime = null;
        private string __pbn__StopTime;

        [global::ProtoBuf.ProtoMember(11, Name = @"source_idx")]
        [global::System.ComponentModel.DefaultValue("")]
        public string SourceIdx
        {
            get { return __pbn__SourceIdx ?? ""; }
            set { __pbn__SourceIdx = value; }
        }
        public bool ShouldSerializeSourceIdx() => __pbn__SourceIdx != null;
        public void ResetSourceIdx() => __pbn__SourceIdx = null;
        private string __pbn__SourceIdx;

        [global::ProtoBuf.ProtoMember(12, Name = @"source_type")]
        public int SourceType
        {
            get { return __pbn__SourceType.GetValueOrDefault(); }
            set { __pbn__SourceType = value; }
        }
        public bool ShouldSerializeSourceType() => __pbn__SourceType != null;
        public void ResetSourceType() => __pbn__SourceType = null;
        private int? __pbn__SourceType;

        [global::ProtoBuf.ProtoMember(13, Name = @"source_name")]
        [global::System.ComponentModel.DefaultValue("")]
        public string SourceName
        {
            get { return __pbn__SourceName ?? ""; }
            set { __pbn__SourceName = value; }
        }
        public bool ShouldSerializeSourceName() => __pbn__SourceName != null;
        public void ResetSourceName() => __pbn__SourceName = null;
        private string __pbn__SourceName;

        [global::ProtoBuf.ProtoMember(14, Name = @"log_txt")]
        [global::System.ComponentModel.DefaultValue("")]
        public string LogTxt
        {
            get { return __pbn__LogTxt ?? ""; }
            set { __pbn__LogTxt = value; }
        }
        public bool ShouldSerializeLogTxt() => __pbn__LogTxt != null;
        public void ResetLogTxt() => __pbn__LogTxt = null;
        private string __pbn__LogTxt;

        [global::ProtoBuf.ProtoMember(15, Name = @"region_idx")]
        [global::System.ComponentModel.DefaultValue("")]
        public string RegionIdx
        {
            get { return __pbn__RegionIdx ?? ""; }
            set { __pbn__RegionIdx = value; }
        }
        public bool ShouldSerializeRegionIdx() => __pbn__RegionIdx != null;
        public void ResetRegionIdx() => __pbn__RegionIdx = null;
        private string __pbn__RegionIdx;

        [global::ProtoBuf.ProtoMember(20, Name = @"ext_info")]
        public byte[] ExtInfo
        {
            get { return __pbn__ExtInfo; }
            set { __pbn__ExtInfo = value; }
        }
        public bool ShouldSerializeExtInfo() => __pbn__ExtInfo != null;
        public void ResetExtInfo() => __pbn__ExtInfo = null;
        private byte[] __pbn__ExtInfo;

        [global::ProtoBuf.ProtoMember(21, Name = @"user_id")]
        public int[] UserIds { get; set; }

        [global::ProtoBuf.ProtoMember(33, Name = @"rslt_msg")]
        public global::System.Collections.Generic.List<TriggerResult> RsltMsgs { get; } = new global::System.Collections.Generic.List<TriggerResult>();

        [global::ProtoBuf.ProtoMember(34, Name = @"trig_info")]
        public global::System.Collections.Generic.List<CommEventTrig> TrigInfoes { get; } = new global::System.Collections.Generic.List<CommEventTrig>();

        public string EventToString()
        {
            StringBuilder eventToStrBuilder = new StringBuilder();
            //事件详情-0：瞬时事件，保存；1：事件开始，保存；2：事件结束，更新结束时间；3：事件脉冲，客户端和服务器使用，CMS不用；
            if (this.EventState != 4)
            {
                eventToStrBuilder.Append("SubSysType:" + this.SubSysType.ToString()).Append("\r\n");
                eventToStrBuilder.Append("LogId:" + this.LogId).Append("\r\n");
                eventToStrBuilder.Append("EventLevel:" + this.EventLevel.ToString()).Append("\r\n");
                eventToStrBuilder.Append("EventName:" + this.EventName).Append("\r\n");
                eventToStrBuilder.Append("EventState:" + this.EventState.ToString()).Append("\r\n");
                eventToStrBuilder.Append("EventType:" + this.EventType).Append("\r\n");
                eventToStrBuilder.Append("EventTypeName:" + this.EventTypeName).Append("\r\n");
                eventToStrBuilder.Append("RegionIdx:" + this.RegionIdx).Append("\r\n");
                eventToStrBuilder.Append("SourceIdx:" + this.SourceIdx).Append("\r\n");
                eventToStrBuilder.Append("SourceName:" + this.SourceName).Append("\r\n");
                eventToStrBuilder.Append("SourceType:" + this.SourceType.ToString()).Append("\r\n");
                eventToStrBuilder.Append("StartTime:" + this.StartTime.ToString()).Append("\r\n");
                eventToStrBuilder.Append("StopTime:" + this.StopTime.ToString()).Append("\r\n");
                //string extInfoStr = this.ShouldSerializeExtInfo()?
                //eventToStrBuilder.Append(""+string.IsNullOrEmpty(this.ExtInfo))
            }
            //4：事件更新，联动结果更新
            else
            {
                eventToStrBuilder.Append("LogId:" + this.LogId).Append("\r\n");
                eventToStrBuilder.Append("EventState:" + this.EventState.ToString()).Append("\r\n");
                eventToStrBuilder.Append("RsltMsgs:\r\n");
                int triggerResTime = 1;
                foreach (TriggerResult triggerRes in this.RsltMsgs)
                {
                    eventToStrBuilder.Append("  TriggerResult"+ triggerResTime.ToString()+ ":\r\n");
                    eventToStrBuilder.Append("  TriggerTime:" + triggerRes.TriggerTime).Append("\r\n");
                    if (triggerRes.TriggerType == 2)
                    {
                        eventToStrBuilder.Append("  TriggerInfo_录像联动信息:" + triggerRes.TriggerInfo.ToString()).Append("\r\n");
                        string[] triggerInfos = triggerRes.TriggerInfo.ToString().Split(new char[] { ';'});
                        foreach (string triggerInfo in triggerInfos)
                        {
                            if (string.IsNullOrEmpty(triggerInfo))
                            {
                                continue;
                            }
                            string[] triggerVideoInfos = triggerInfo.Split(new char[] { ',' });
                            eventToStrBuilder.Append("      监控点名称:" + triggerVideoInfos[0]).Append("\r\n");
                            eventToStrBuilder.Append("      录像计划Id:" + triggerVideoInfos[1]).Append("\r\n");
                            eventToStrBuilder.Append("      录像类型:" + triggerVideoInfos[2]).Append("\r\n");
                            eventToStrBuilder.Append("      监控点编号:" + triggerVideoInfos[3]).Append("\r\n");
                        }
                    }
                    else if (triggerRes.TriggerType == 4)
                    {
                        eventToStrBuilder.Append("  TriggerInfo_抓图联动信息:" + triggerRes.TriggerInfo.ToString()).Append("\r\n");
                        string[] triggerInfos = triggerRes.TriggerInfo.ToString().Split(new char[] { ';' });
                        foreach (string triggerInfo in triggerInfos)
                        {
                            if (string.IsNullOrEmpty(triggerInfo))
                            {
                                continue;
                            }
                            string[] triggerVideoInfos = triggerInfo.Split(new char[] { ',' });
                            eventToStrBuilder.Append("      监控点名称:" + triggerVideoInfos[0]).Append("\r\n");
                            eventToStrBuilder.Append("      联动抓图url:" + triggerVideoInfos[1]).Append("\r\n");
                        }
                    }
                    triggerResTime++;
                }
            }
            return eventToStrBuilder.ToString();
        }
    }

}

#pragma warning restore CS1591, CS0612, CS3021, IDE1006
