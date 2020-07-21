// This file was generated by a tool; you should avoid making direct changes.
// Consider using 'partial classes' to extend these types
// Input: pms_event.proto

#pragma warning disable CS1591, CS0612, CS3021, IDE1006
namespace MQtest
{

    [global::ProtoBuf.ProtoContract()]
    public partial class PmsPersonInfo : global::ProtoBuf.IExtensible
    {
        private global::ProtoBuf.IExtension __pbn__extensionData;
        global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
            => global::ProtoBuf.Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

        [global::ProtoBuf.ProtoMember(33, Name = @"person_idx", IsRequired = true)]
        public string PersonIdx { get; set; }

        [global::ProtoBuf.ProtoMember(34, Name = @"person_name", IsRequired = true)]
        public string PersonName { get; set; }

        [global::ProtoBuf.ProtoMember(35, Name = @"dept_name", IsRequired = true)]
        public string DeptName { get; set; }

        [global::ProtoBuf.ProtoMember(36, Name = @"id_type")]
        public int IdType
        {
            get { return __pbn__IdType.GetValueOrDefault(); }
            set { __pbn__IdType = value; }
        }
        public bool ShouldSerializeIdType() => __pbn__IdType != null;
        public void ResetIdType() => __pbn__IdType = null;
        private int? __pbn__IdType;

        [global::ProtoBuf.ProtoMember(37, Name = @"id_no")]
        [global::System.ComponentModel.DefaultValue("")]
        public string IdNo
        {
            get { return __pbn__IdNo ?? ""; }
            set { __pbn__IdNo = value; }
        }
        public bool ShouldSerializeIdNo() => __pbn__IdNo != null;
        public void ResetIdNo() => __pbn__IdNo = null;
        private string __pbn__IdNo;

        [global::ProtoBuf.ProtoMember(38, Name = @"phone")]
        [global::System.ComponentModel.DefaultValue("")]
        public string Phone
        {
            get { return __pbn__Phone ?? ""; }
            set { __pbn__Phone = value; }
        }
        public bool ShouldSerializePhone() => __pbn__Phone != null;
        public void ResetPhone() => __pbn__Phone = null;
        private string __pbn__Phone;

        [global::ProtoBuf.ProtoMember(39, Name = @"pinyin")]
        [global::System.ComponentModel.DefaultValue("")]
        public string Pinyin
        {
            get { return __pbn__Pinyin ?? ""; }
            set { __pbn__Pinyin = value; }
        }
        public bool ShouldSerializePinyin() => __pbn__Pinyin != null;
        public void ResetPinyin() => __pbn__Pinyin = null;
        private string __pbn__Pinyin;

        [global::ProtoBuf.ProtoMember(40, Name = @"addr")]
        [global::System.ComponentModel.DefaultValue("")]
        public string Addr
        {
            get { return __pbn__Addr ?? ""; }
            set { __pbn__Addr = value; }
        }
        public bool ShouldSerializeAddr() => __pbn__Addr != null;
        public void ResetAddr() => __pbn__Addr = null;
        private string __pbn__Addr;

        [global::ProtoBuf.ProtoMember(41, Name = @"photo_path")]
        [global::System.ComponentModel.DefaultValue("")]
        public string PhotoPath
        {
            get { return __pbn__PhotoPath ?? ""; }
            set { __pbn__PhotoPath = value; }
        }
        public bool ShouldSerializePhotoPath() => __pbn__PhotoPath != null;
        public void ResetPhotoPath() => __pbn__PhotoPath = null;
        private string __pbn__PhotoPath;

        [global::ProtoBuf.ProtoMember(42, Name = @"sex")]
        public int Sex
        {
            get { return __pbn__Sex.GetValueOrDefault(); }
            set { __pbn__Sex = value; }
        }
        public bool ShouldSerializeSex() => __pbn__Sex != null;
        public void ResetSex() => __pbn__Sex = null;
        private int? __pbn__Sex;

    }

    [global::ProtoBuf.ProtoContract()]
    public partial class MsgPmsEvent : global::ProtoBuf.IExtensible
    {
        private global::ProtoBuf.IExtension __pbn__extensionData;
        global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
            => global::ProtoBuf.Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

        [global::ProtoBuf.ProtoMember(1, Name = @"pms_event")]
        public CEmuEvent PmsEvent { get; set; }

        [global::ProtoBuf.ProtoMember(2, Name = @"pgs_event")]
        public CPlaceEvent PgsEvent { get; set; }

        [global::ProtoBuf.ProtoMember(33, Name = @"person_info")]
        public PmsPersonInfo PersonInfo { get; set; }

    }

}

#pragma warning restore CS1591, CS0612, CS3021, IDE1006
