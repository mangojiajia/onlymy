﻿using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Yongrong.Conf;
using Yongrong.Model.Db;

namespace Yongrong.Db
{
    public partial class YrContext : DbContext
    {
        public YrContext()
        {
        }

        public YrContext(DbContextOptions<YrContext> options)
            : base(options)
        {
        }

        public virtual DbSet<PdaLog> PdaLog { get; set; }
        public virtual DbSet<GateLog> GateLog { get; set; }
        public virtual DbSet<Abnormal> Abnormal { get; set; }
        public virtual DbSet<BaseDriver> BaseDriver { get; set; }
        public virtual DbSet<BaseTractor> BaseTractor { get; set; }
        public virtual DbSet<BaseTrailer> BaseTrailer { get; set; }
        public virtual DbSet<ApiWeigh> ApiWeigh { get; set; }
        public virtual DbSet<BaseCustomer> BaseCustomer { get; set; }
        public virtual DbSet<BaseGoods> BaseGoods { get; set; }
        public virtual DbSet<BaseLoadingplace> BaseLoadingplace { get; set; }
        public virtual DbSet<BaseLogisticsgate> BaseLogisticsgate { get; set; }
        public virtual DbSet<BaseSupplier> BaseSupplier { get; set; }
        public virtual DbSet<BaseTransport> BaseTransport { get; set; }
        public virtual DbSet<Permission> Permission { get; set; }
        public virtual DbSet<Role> Role { get; set; }
        public virtual DbSet<RolePermission> RolePermission { get; set; }
        public virtual DbSet<Todolist> Todolist { get; set; }
        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<UserPermission> UserPermission { get; set; }
        public virtual DbSet<BaseSupercargo> BaseSupercargo { get; set; }
        public virtual DbSet<BillGoodsIn> BillGoodsIn { get; set; }
        public virtual DbSet<OrderGoods> OrderGoods { get; set; }
        public virtual DbSet<BillGoodsOut> BillGoodsOut { get; set; }
        public virtual DbSet<BillGoodsRefund> BillGoodsRefund { get; set; }
        public virtual DbSet<SafeCheck> SafeCheck { get; set; }
        public virtual DbSet<OrderConfig> OrderConfig { get; set; }
        public virtual DbSet<UserOplog> UserOplog { get; set; }
        public virtual DbSet<ApiGateevent> ApiGateevent { get; set; }
        public virtual DbSet<SysConfig> SysConfig { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseOracle(BaseDb.Conn, b => b.UseOracleSQLCompatibility("11"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.6-servicing-10079")
            .HasAnnotation("Relational:DefaultSchema", "YR");

            modelBuilder.Entity<PdaLog>(entity =>
            {
                entity.ToTable("PDALOG");

                entity.HasIndex(e => e.Id)
                    .HasName("KEYPDALOG1")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasColumnType("NUMBER");

                entity.Property(e => e.Orderid)
                  .HasColumnName("ORDERID")
                  .HasColumnType("VARCHAR2(64)");

                entity.Property(e => e.Tractorid)
                    .HasColumnName("TRACTORID")
                    .HasColumnType("VARCHAR2(64)");
 
                entity.Property(e => e.Operation)
                    .HasColumnName("OPERATION")
                    .HasColumnType("VARCHAR2(64)");

                entity.Property(e => e.Reqparameter)
                    .HasColumnName("REQPARAMETER")
                    .HasColumnType("VARCHAR2(2048)");

                entity.Property(e => e.Rspcode)
                    .HasColumnName("RSPCODE")
                    .HasColumnType("VARCHAR2(4)");

                entity.Property(e => e.Rspmessage)
                    .HasColumnName("RSPMESSAGE")
                    .HasColumnType("VARCHAR2(2048)");

                entity.Property(e => e.Createtime)
                    .HasColumnName("CREATETIME")
                    .HasColumnType("VARCHAR2(32)");

            });

            modelBuilder.Entity<GateLog>(entity =>
            {
                entity.ToTable("GATELOG");

                entity.HasIndex(e => e.Id)
                    .HasName("KEYGATELOG1")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasColumnType("NUMBER");

                entity.Property(e => e.Tractorid)
                    .HasColumnName("TRACTORID")
                    .HasColumnType("VARCHAR2(32)");

                entity.Property(e => e.Cartype)
                    .HasColumnName("CARTYPE")
                    .HasColumnType("VARCHAR2(4)");

                entity.Property(e => e.Operation)
                    .HasColumnName("OPERATION")
                    .HasColumnType("VARCHAR2(256)");

                entity.Property(e => e.Reqparameter)
                    .HasColumnName("REQPARAMETER")
                    .HasColumnType("VARCHAR2(2048)");

                entity.Property(e => e.Rspcode)
                    .HasColumnName("RSPCODE")
                    .HasColumnType("VARCHAR2(4)");

                entity.Property(e => e.Rspmessage)
                    .HasColumnName("RSPMESSAGE")
                    .HasColumnType("VARCHAR2(2048)");

                entity.Property(e => e.Createtime)
                    .HasColumnName("CREATETIME")
                    .HasColumnType("VARCHAR2(32)");

            });

            modelBuilder.Entity<SysConfig>(entity =>
            {
                entity.ToTable("SYS_CONFIG");

                entity.HasIndex(e => e.Id)
                    .HasName("SYS_C0011645")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasColumnType("NUMBER");

                entity.Property(e => e.Ckey)
                    .HasColumnName("CKEY")
                    .HasColumnType("VARCHAR2(128)");

                entity.Property(e => e.Ekey)
                    .HasColumnName("EKEY")
                    .HasColumnType("VARCHAR2(128)");

                entity.Property(e => e.Price)
                    .HasColumnName("PRICE")
                    .HasColumnType("VARCHAR2(2048)");
            });

            modelBuilder.Entity<Abnormal>(entity =>
            {
                entity.ToTable("ABNORMAL");

                entity.HasIndex(e => e.Id)
                    .HasName("keyabnormal1")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasColumnType("NUMBER");

                entity.Property(e => e.Abnormalcase)
                    .HasColumnName("ABNORMALCASE")
                    .HasColumnType("VARCHAR2(2048)");

                entity.Property(e => e.Abnormalname)
                    .HasColumnName("ABNORMALNAME")
                    .HasColumnType("VARCHAR2(32)");

                entity.Property(e => e.Abnormaltype)
                    .HasColumnName("ABNORMALTYPE")
                    .HasColumnType("VARCHAR2(32)");

                entity.Property(e => e.Createtime)
                    .HasColumnName("CREATETIME")
                    .HasColumnType("VARCHAR2(32)");

                entity.Property(e => e.Createuser)
                    .HasColumnName("CREATEUSER")
                    .HasColumnType("VARCHAR2(32)");

                entity.Property(e => e.Isdispose)
                    .HasColumnName("ISDISPOSE")
                    .HasColumnType("VARCHAR2(2)");

                entity.Property(e => e.Remark)
                    .HasColumnName("REMARK")
                    .HasColumnType("VARCHAR2(64)");

                entity.Property(e => e.Updatetime)
                    .HasColumnName("UPDATETIME")
                    .HasColumnType("VARCHAR2(32)");

                entity.Property(e => e.Updateuser)
                    .HasColumnName("UPDATEUSER")
                    .HasColumnType("VARCHAR2(32)");
            });

            modelBuilder.Entity<ApiWeigh>(entity =>
            {
                entity.ToTable("API_WEIGH");

                entity.HasIndex(e => e.Id)
                    .HasName("SYS_C0010866")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasColumnType("NUMBER");

                entity.Property(e => e.Createtime)
                    .HasColumnName("CREATETIME")
                    .HasColumnType("DATE");

                entity.Property(e => e.Maskid)
                    .HasColumnName("MASKID")
                    .HasColumnType("VARCHAR2(256)");

                entity.Property(e => e.Msgid)
                    .HasColumnName("MSGID")
                    .HasColumnType("VARCHAR2(64)");

                entity.Property(e => e.Operator)
                    .HasColumnName("OPERATOR")
                    .HasColumnType("VARCHAR2(64)");

                entity.Property(e => e.Orderid)
                    .HasColumnName("ORDERID")
                    .HasColumnType("VARCHAR2(64)");

                entity.Property(e => e.Ordertype)
                    .HasColumnName("ORDERTYPE")
                    .HasColumnType("VARCHAR2(64)");

                entity.Property(e => e.Passtime)
                    .HasColumnName("PASSTIME")
                    .HasColumnType("VARCHAR2(32)");

                entity.Property(e => e.Sign)
                    .HasColumnName("SIGN")
                    .HasColumnType("VARCHAR2(128)");

                entity.Property(e => e.Tractorid)
                    .HasColumnName("TRACTORID")
                    .HasColumnType("VARCHAR2(64)");

                entity.Property(e => e.Trailerid)
                    .HasColumnName("TRAILERID")
                    .HasColumnType("VARCHAR2(64)");

                entity.Property(e => e.Weight)
                    .HasColumnName("WEIGHT")
                    .HasColumnType("VARCHAR2(32)");

                entity.Property(e => e.Weightime)
                    .HasColumnName("WEIGHTIME")
                    .HasColumnType("VARCHAR2(64)");
            });

            modelBuilder.Entity<BaseCustomer>(entity =>
            {
                entity.ToTable("BASE_CUSTOMER");

                entity.HasIndex(e => e.Id)
                    .HasName("SYS_C0010830")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasColumnType("NUMBER");

                entity.Property(e => e.Checkstat)
                    .HasColumnName("CHECKSTAT")
                    .HasColumnType("VARCHAR2(64)");

                entity.Property(e => e.Contact)
                    .HasColumnName("CONTACT")
                    .HasColumnType("VARCHAR2(64)");

                entity.Property(e => e.Createtime)
                    .HasColumnName("CREATETIME")
                    .HasColumnType("VARCHAR2(32)");

                entity.Property(e => e.Creditid)
                    .HasColumnName("CREDITID")
                    .HasColumnType("VARCHAR2(64)");

                entity.Property(e => e.Datasource)
                    .HasColumnName("DATASOURCE")
                    .HasColumnType("VARCHAR2(64)");

                entity.Property(e => e.Enterpriseid)
                    .HasColumnName("ENTERPRISEID")
                    .HasColumnType("VARCHAR2(64)");

                entity.Property(e => e.Enterprisename)
                    .HasColumnName("ENTERPRISENAME")
                    .HasColumnType("VARCHAR2(256)");

                entity.Property(e => e.Enterpriseshortname)
                    .HasColumnName("ENTERPRISESHORTNAME")
                    .HasColumnType("VARCHAR2(256)");

                entity.Property(e => e.Establishtime)
                    .HasColumnName("ESTABLISHTIME")
                    .HasColumnType("DAVARCHAR2(32)");

                entity.Property(e => e.Groupncid)
                    .HasColumnName("GROUPNCID")
                    .HasColumnType("VARCHAR2(64)");

                entity.Property(e => e.Jinjiangncid)
                    .HasColumnName("JINJIANGNCID")
                    .HasColumnType("VARCHAR2(64)");

                entity.Property(e => e.Legalname)
                    .HasColumnName("LEGALNAME")
                    .HasColumnType("VARCHAR2(64)");

                entity.Property(e => e.Pushsystemid)
                    .HasColumnName("PUSHSYSTEMID")
                    .HasColumnType("VARCHAR2(64)");

                entity.Property(e => e.Regcapital)
                    .HasColumnName("REGCAPITAL")
                    .HasColumnType("VARCHAR2(64)");

                entity.Property(e => e.Remark)
                    .HasColumnName("REMARK")
                    .HasColumnType("VARCHAR2(64)");

                entity.Property(e => e.Supporttype)
                    .HasColumnName("SUPPORTTYPE")
                    .HasColumnType("VARCHAR2(64)");

                entity.Property(e => e.Userdefined14)
                    .HasColumnName("USERDEFINED14")
                    .HasColumnType("VARCHAR2(64)");

                entity.Property(e => e.Userdefined15)
                    .HasColumnName("USERDEFINED15")
                    .HasColumnType("VARCHAR2(64)");

                entity.Property(e => e.White)
                    .HasColumnName("WHITE")
                    .HasColumnType("VARCHAR2(64)");
            });

            modelBuilder.Entity<BaseDriver>(entity =>
            {
                entity.ToTable("BASE_DRIVER");

                entity.HasIndex(e => e.Id)
                    .HasName("SYS_C0010872")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedNever();

                entity.Property(e => e.Cardid)
                    .HasColumnName("CARDID")
                    .HasColumnType("VARCHAR2(64)");

                entity.Property(e => e.Code)
                    .HasColumnName("CODE")
                    .HasColumnType("VARCHAR2(128)");

                entity.Property(e => e.Createtime)
                    .HasColumnName("CREATETIME")
                    .HasColumnType("VARCHAR2(32)");

                entity.Property(e => e.Driverdegree)
                    .HasColumnName("DRIVERDEGREE")
                    .HasColumnType("VARCHAR2(32)");

                entity.Property(e => e.Driverid)
                    .HasColumnName("DRIVERID")
                    .HasColumnType("VARCHAR2(64)");

                entity.Property(e => e.Drivervalid)
                    .HasColumnName("DRIVERVALID")
                    .HasColumnType("VARCHAR2(64)");

                entity.Property(e => e.Name)
                    .HasColumnName("NAME")
                    .HasColumnType("VARCHAR2(128)");

                entity.Property(e => e.Otherid)
                    .HasColumnName("OTHERID")
                    .HasColumnType("VARCHAR2(64)");

                entity.Property(e => e.Othervalid)
                    .HasColumnName("OTHERVALID")
                    .HasColumnType("VARCHAR2(64)");

                entity.Property(e => e.Tel)
                    .HasColumnName("TEL")
                    .HasColumnType("VARCHAR2(64)");

                entity.Property(e => e.TractorId)
                    .HasColumnName("TRACTOR_ID")
                    .HasColumnType("VARCHAR2(64)");

                entity.Property(e => e.TrailerId)
                    .HasColumnName("TRAILER_ID")
                    .HasColumnType("VARCHAR2(64)");

                entity.Property(e => e.Transport)
                    .HasColumnName("TRANSPORT")
                    .HasColumnType("VARCHAR2(128)");

                entity.Property(e => e.Whiteflag)
                    .HasColumnName("WHITEFLAG")
                    .HasColumnType("VARCHAR2(64)");
            });

            modelBuilder.Entity<Permission>(entity =>
            {
                entity.ToTable("PERMISSION");

                entity.HasIndex(e => e.Permissionid)
                    .HasName("SYS_C0011144")
                    .IsUnique();

                entity.Property(e => e.Permissionid)
                    .HasColumnName("PERMISSIONID")
                    .HasColumnType("VARCHAR2(8)")
                    .ValueGeneratedNever();

                entity.Property(e => e.Description)
                    .HasColumnName("DESCRIPT")
                    .HasColumnType("VARCHAR2(512)");

                entity.Property(e => e.Levele)
                    .HasColumnName("LEVELE")
                    .HasColumnType("NUMBER");

                entity.Property(e => e.Permissionname)
                    .HasColumnName("PERMISSIONNAME")
                    .HasColumnType("VARCHAR2(64)");

                entity.Property(e => e.Ispermission)
                    .HasColumnName("ISPERMISSION")
                    .HasColumnType("VARCHAR2(64)");
            });

            modelBuilder.Entity<BaseGoods>(entity =>
            {
                entity.ToTable("BASE_GOODS");

                entity.HasIndex(e => e.Id)
                    .HasName("SYS_C0010827")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasColumnType("NUMBER");

                entity.Property(e => e.Createtime)
                    .HasColumnName("CREATETIME")
                    .HasColumnType("VARCHAR2(32)");

                entity.Property(e => e.Goodsid)
                    .HasColumnName("GOODSID")
                    .HasColumnType("VARCHAR2(64)");

                entity.Property(e => e.Goodsname)
                    .HasColumnName("GOODSNAME")
                    .HasColumnType("VARCHAR2(64)");

                entity.Property(e => e.Goodsstat)
                    .HasColumnName("GOODSSTAT")
                    .HasColumnType("VARCHAR2(64)");

                entity.Property(e => e.Goodstype)
                    .HasColumnName("GOODSTYPE")
                    .HasColumnType("VARCHAR2(512)");

                entity.Property(e => e.Mark)
                    .HasColumnName("MARK")
                    .HasColumnType("VARCHAR2(64)");

                entity.Property(e => e.Mdmcode)
                    .HasColumnName("MDMCODE")
                    .HasColumnType("VARCHAR2(64)");

                entity.Property(e => e.Remark)
                    .HasColumnName("REMARK")
                    .HasColumnType("VARCHAR2(256)");

                entity.Property(e => e.Goodsshortname)
                    .HasColumnName("GOODSSHORTNAME")
                    .HasColumnType("VARCHAR2(64)");

                entity.Property(e => e.Goodsmaterial)
                    .HasColumnName("GOODSMATERIAL")
                    .HasColumnType("VARCHAR2(64)");

                entity.Property(e => e.Estimate)
                    .HasColumnName("ESTIMATE")
                    .HasColumnType("FLOAT");

                entity.Property(e => e.Goodsclassify)
                    .HasColumnName("GOODSCLASSIFY")
                    .HasColumnType("VARCHAR2(64)");

                entity.Property(e => e.Unit)
                    .HasColumnName("UNIT")
                    .HasColumnType("VARCHAR2(64)");

                entity.Property(e => e.Unitext)
                    .HasColumnName("UNITEXT")
                    .HasColumnType("VARCHAR2(64)");

                entity.Property(e => e.Taxclassify)
                    .HasColumnName("TAXCLASSIFY")
                    .HasColumnType("VARCHAR2(64)");

                entity.Property(e => e.Ordergoodstype)
                    .HasColumnName("ORDERGOODSTYPE")
                    .HasColumnType("VARCHAR2(64)");

                entity.Property(e => e.Extgoodsid)
                    .HasColumnName("EXTGOODSID")
                    .HasColumnType("VARCHAR2(64)");

                entity.Property(e => e.Applycompany)
                    .HasColumnName("APPLYCOMPANY")
                    .HasColumnType("VARCHAR2(64)");

                entity.Property(e => e.Storagetime)
                    .HasColumnName("STORAGETIME")
                    .HasColumnType("VARCHAR2(32)");

                entity.Property(e => e.Freeitem1)
                    .HasColumnName("FREEITEM1")
                    .HasColumnType("VARCHAR2(64)");

                entity.Property(e => e.Freeitem2)
                    .HasColumnName("FREEITEM2")
                    .HasColumnType("VARCHAR2(64)");

                entity.Property(e => e.Freeitem3)
                    .HasColumnName("FREEITEM3")
                    .HasColumnType("VARCHAR2(64)");

                entity.Property(e => e.Freeitem4)
                    .HasColumnName("FREEITEM4")
                    .HasColumnType("VARCHAR2(64)");

                entity.Property(e => e.Istempgoodsid)
                    .HasColumnName("ISTEMPGOODSID")
                    .HasColumnType("VARCHAR2(32)");

                entity.Property(e => e.Tempgoodsid)
                    .HasColumnName("TEMPGOODSID")
                    .HasColumnType("VARCHAR2(64)");
            });

            modelBuilder.Entity<BaseLoadingplace>(entity =>
            {
                entity.ToTable("BASE_LOADINGPLACE");

                entity.HasIndex(e => e.Id)
                    .HasName("SYS_C0010828")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasColumnType("NUMBER");

                entity.Property(e => e.Createtime)
                    .HasColumnName("CREATETIME")
                    .HasColumnType("VARCHAR2(32)");

                entity.Property(e => e.Placeid)
                    .HasColumnName("PLACEID")
                    .HasColumnType("VARCHAR2(64)");

                entity.Property(e => e.Placename)
                    .HasColumnName("PLACENAME")
                    .HasColumnType("VARCHAR2(64)");
            });

            modelBuilder.Entity<BaseLogisticsgate>(entity =>
            {
                entity.ToTable("BASE_LOGISTICSGATE");

                entity.HasIndex(e => e.Id)
                    .HasName("SYS_C0010829")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasColumnType("NUMBER");

                entity.Property(e => e.Createtime)
                    .HasColumnName("CREATETIME")
                    .HasColumnType("VARCHAR2(32)");

                entity.Property(e => e.Gateid)
                    .HasColumnName("GATEID")
                    .HasColumnType("VARCHAR2(64)");

                entity.Property(e => e.Gatename)
                    .HasColumnName("GATENAME")
                    .HasColumnType("VARCHAR2(64)");
            });

            modelBuilder.Entity<BaseSupercargo>(entity =>
            {
                entity.ToTable("BASE_SUPERCARGO");

                entity.HasIndex(e => e.Id)
                    .HasName("SYS_C0010877")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedNever();

                entity.Property(e => e.Cardid)
                    .HasColumnName("CARDID")
                    .HasColumnType("VARCHAR2(64)");

                entity.Property(e => e.Code)
                    .HasColumnName("CODE")
                    .HasColumnType("VARCHAR2(128)");

                entity.Property(e => e.Createtime)
                    .HasColumnName("CREATETIME")
                    .HasColumnType("VARCHAR2(32)");

                entity.Property(e => e.Driver)
                    .HasColumnName("DRIVER")
                    .HasColumnType("VARCHAR2(64)");

                entity.Property(e => e.Driverid)
                    .HasColumnName("DRIVERID")
                    .HasColumnType("VARCHAR2(64)");

                entity.Property(e => e.Drivervalid)
                    .HasColumnName("DRIVERVALID")
                    .HasColumnType("VARCHAR2(64)");

                entity.Property(e => e.Name)
                    .HasColumnName("NAME")
                    .HasColumnType("VARCHAR2(128)");

                entity.Property(e => e.Otherid)
                    .HasColumnName("OTHERID")
                    .HasColumnType("VARCHAR2(64)");

                entity.Property(e => e.Othervalid)
                    .HasColumnName("OTHERVALID")
                    .HasColumnType("VARCHAR2(64)");

                entity.Property(e => e.TractorId)
                    .HasColumnName("TRACTOR_ID")
                    .HasColumnType("VARCHAR2(64)");

                entity.Property(e => e.TrailerId)
                    .HasColumnName("TRAILER_ID")
                    .HasColumnType("VARCHAR2(64)");

                entity.Property(e => e.Transport)
                    .HasColumnName("TRANSPORT")
                    .HasColumnType("VARCHAR2(128)");

                entity.Property(e => e.Whiteflag)
                    .HasColumnName("WHITEFLAG")
                    .HasColumnType("VARCHAR2(64)");
            });

            modelBuilder.Entity<BaseSupplier>(entity =>
            {
                entity.ToTable("BASE_SUPPLIER");

                entity.HasIndex(e => e.Id)
                    .HasName("SYS_C0010832")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasColumnType("NUMBER");

                entity.Property(e => e.Checkstat)
                    .HasColumnName("CHECKSTAT")
                    .HasColumnType("VARCHAR2(64)");

                entity.Property(e => e.Contact)
                    .HasColumnName("CONTACT")
                    .HasColumnType("VARCHAR2(64)");

                entity.Property(e => e.Createtime)
                    .HasColumnName("CREATETIME")
                    .HasColumnType("VARCHAR2(32)");

                entity.Property(e => e.Creditid)
                    .HasColumnName("CREDITID")
                    .HasColumnType("VARCHAR2(64)");

                entity.Property(e => e.Datasource)
                    .HasColumnName("DATASOURCE")
                    .HasColumnType("VARCHAR2(64)");

                entity.Property(e => e.Enterpriseid)
                    .HasColumnName("ENTERPRISEID")
                    .HasColumnType("VARCHAR2(64)");

                entity.Property(e => e.Enterprisename)
                    .HasColumnName("ENTERPRISENAME")
                    .HasColumnType("VARCHAR2(256)");

                entity.Property(e => e.Enterpriseshortname)
                    .HasColumnName("ENTERPRISESHORTNAME")
                    .HasColumnType("VARCHAR2(256)");

                entity.Property(e => e.Establishtime)
                    .HasColumnName("ESTABLISHTIME")
                    .HasColumnType("DAVARCHAR2(32)");

                entity.Property(e => e.Groupncid)
                    .HasColumnName("GROUPNCID")
                    .HasColumnType("VARCHAR2(64)");

                entity.Property(e => e.Jinjiangncid)
                    .HasColumnName("JINJIANGNCID")
                    .HasColumnType("VARCHAR2(64)");

                entity.Property(e => e.Legalname)
                    .HasColumnName("LEGALNAME")
                    .HasColumnType("VARCHAR2(64)");

                entity.Property(e => e.Pushsystemid)
                    .HasColumnName("PUSHSYSTEMID")
                    .HasColumnType("VARCHAR2(64)");

                entity.Property(e => e.Regcapital)
                    .HasColumnName("REGCAPITAL")
                    .HasColumnType("VARCHAR2(64)");

                entity.Property(e => e.Remark)
                    .HasColumnName("REMARK")
                    .HasColumnType("VARCHAR2(64)");

                entity.Property(e => e.Supporttype)
                    .HasColumnName("SUPPORTTYPE")
                    .HasColumnType("VARCHAR2(64)");

                entity.Property(e => e.Userdefined14)
                    .HasColumnName("USERDEFINED14")
                    .HasColumnType("VARCHAR2(64)");

                entity.Property(e => e.Userdefined15)
                    .HasColumnName("USERDEFINED15")
                    .HasColumnType("VARCHAR2(64)");

                entity.Property(e => e.White)
                    .HasColumnName("WHITE")
                    .HasColumnType("VARCHAR2(64)");

            });

            modelBuilder.Entity<BaseTractor>(entity =>
            {
                entity.ToTable("BASE_TRACTOR");

                entity.HasIndex(e => e.Id)
                    .HasName("SYS_C0010867")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedNever();

                entity.Property(e => e.Carid)
                    .HasColumnName("CARID")
                    .HasColumnType("VARCHAR2(64)");

                entity.Property(e => e.Createtime)
                    .HasColumnName("CREATETIME")
                    .HasColumnType("VARCHAR2(32)");

                entity.Property(e => e.Degree)
                    .HasColumnName("DEGREE")
                    .HasColumnType("VARCHAR2(64)");

                entity.Property(e => e.Region)
                    .HasColumnName("REGION")
                    .HasColumnType("VARCHAR2(64)");

                entity.Property(e => e.TractorId)
                    .HasColumnName("TRACTOR_ID")
                    .HasColumnType("VARCHAR2(64)");

                entity.Property(e => e.Transport)
                    .HasColumnName("TRANSPORT")
                    .HasColumnType("VARCHAR2(128)");

                entity.Property(e => e.Validdate)
                    .HasColumnName("VALIDDATE")
                    .HasColumnType("VARCHAR2(64)");
            });

            modelBuilder.Entity<BaseTrailer>(entity =>
            {
                entity.ToTable("BASE_TRAILER");

                entity.HasIndex(e => e.Id)
                    .HasName("SYS_C0010869")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedNever();

                entity.Property(e => e.Carflag)
                    .HasColumnName("CARFLAG")
                    .HasColumnType("VARCHAR2(64)");

                entity.Property(e => e.Carid)
                    .HasColumnName("CARID")
                    .HasColumnType("VARCHAR2(64)");

                entity.Property(e => e.Createtime)
                    .HasColumnName("CREATETIME")
                    .HasColumnType("VARCHAR2(32)");

                entity.Property(e => e.Degree)
                    .HasColumnName("DEGREE")
                    .HasColumnType("VARCHAR2(64)");

                entity.Property(e => e.Maxweight)
                    .HasColumnName("MAXWEIGHT")
                    .HasColumnType("NUMBER(10,3)");

                entity.Property(e => e.Region)
                    .HasColumnName("REGION")
                    .HasColumnType("VARCHAR2(64)");

                entity.Property(e => e.TrailerId)
                    .HasColumnName("TRAILER_ID")
                    .HasColumnType("VARCHAR2(64)");

                entity.Property(e => e.Transport)
                    .HasColumnName("TRANSPORT")
                    .HasColumnType("VARCHAR2(128)");

                entity.Property(e => e.Validdate)
                    .HasColumnName("VALIDDATE")
                    .HasColumnType("VARCHAR2(64)");
            });

            modelBuilder.Entity<BaseTransport>(entity =>
            {
                entity.ToTable("BASE_TRANSPORT");

                entity.HasIndex(e => e.Id)
                    .HasName("SYS_C0010833")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasColumnType("NUMBER");

                entity.Property(e => e.Addr)
                    .HasColumnName("ADDR")
                    .HasColumnType("VARCHAR2(128)");

                entity.Property(e => e.Code)
                    .HasColumnName("CODE")
                    .HasColumnType("VARCHAR2(64)");

                entity.Property(e => e.Contract)
                    .HasColumnName("CONTRACT")
                    .HasColumnType("VARCHAR2(64)");

                entity.Property(e => e.Legal)
                    .HasColumnName("LEGAL")
                    .HasColumnType("VARCHAR2(64)");

                entity.Property(e => e.Mail)
                    .HasColumnName("MAIL")
                    .HasColumnType("VARCHAR2(64)");

                entity.Property(e => e.Name)
                    .HasColumnName("NAME")
                    .HasColumnType("VARCHAR2(64)");

                entity.Property(e => e.Remark)
                    .HasColumnName("REMARK")
                    .HasColumnType("VARCHAR2(256)");

                entity.Property(e => e.Tel)
                    .HasColumnName("TEL")
                    .HasColumnType("VARCHAR2(64)");

                entity.Property(e => e.Whiteflag)
                    .HasColumnName("WHITEFLAG")
                    .HasColumnType("VARCHAR2(64)");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.HasKey(e => e.Roleid)
                    .HasName("SYS_C0011119");

                entity.ToTable("ROLE_LOGIN");

                entity.HasIndex(e => e.Roleid)
                    .HasName("SYS_C0011119")
                    .IsUnique();

                entity.Property(e => e.Roleid)
                    .HasColumnName("ROLEID")
                    .HasColumnType("VARCHAR2(32)")
                    .ValueGeneratedNever();

                entity.Property(e => e.Createtime)
                    .HasColumnName("CREATETIME")
                    .HasColumnType("VARCHAR2(32)");

                entity.Property(e => e.Descript)
                    .HasColumnName("DESCRIPT")
                    .HasColumnType("VARCHAR2(256)");

                entity.Property(e => e.Isenable)
                    .HasColumnName("ISENABLE")
                    .HasColumnType("VARCHAR2(64)");

                entity.Property(e => e.Rolename)
                    .HasColumnName("ROLENAME")
                    .HasColumnType("VARCHAR2(32)");

                entity.Property(e => e.Userid)
                    .HasColumnName("USERID")
                    .HasColumnType("VARCHAR2(64)");
            });

            modelBuilder.Entity<RolePermission>(entity =>
            {
                entity.ToTable("ROLE_PERMISSION");

                entity.HasIndex(e => e.Id)
                    .HasName("SYS_C0010814")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasColumnType("NUMBER");

                entity.Property(e => e.Createtime)
                    .HasColumnName("CREATETIME")
                    .HasColumnType("DATE");

                entity.Property(e => e.Ispermission)
                    .HasColumnName("ISPERMISSION")
                    .HasColumnType("VARCHAR2(128)");

                entity.Property(e => e.Permissionid)
                    .HasColumnName("PERMISSIONID")
                    .HasColumnType("VARCHAR2(8)");

                entity.Property(e => e.Roleid)
                    .HasColumnName("ROLEID")
                    .HasColumnType("VARCHAR2(32)");
            });

            modelBuilder.Entity<Todolist>(entity =>
            {
                entity.ToTable("TODOLIST");

                entity.HasIndex(e => e.Id)
                    .HasName("SYS_C0010825")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasColumnType("NUMBER");

                entity.Property(e => e.Checker)
                    .HasColumnName("CHECKER")
                    .HasColumnType("VARCHAR2(32)");

                entity.Property(e => e.Checktime)
                    .HasColumnName("CHECKTIME")
                    .HasColumnType("VARCHAR2(32)");

                entity.Property(e => e.Createtime)
                    .HasColumnName("CREATETIME")
                    .HasColumnType("VARCHAR2(32)");

                entity.Property(e => e.Description)
                    .HasColumnName("DESCRIPTION")
                    .HasColumnType("VARCHAR2(512)");

                entity.Property(e => e.Flag)
                    .HasColumnName("FLAG")
                    .HasColumnType("VARCHAR2(16)");

                entity.Property(e => e.Task)
                    .HasColumnName("TASK")
                    .HasColumnType("VARCHAR2(256)");

                entity.Property(e => e.Tasktype)
                    .HasColumnName("TASKTYPE")
                    .HasColumnType("VARCHAR2(32)");

                entity.Property(e => e.Userid)
                    .HasColumnName("USERID")
                    .HasColumnType("VARCHAR2(32)");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("USER_LOGIN");

                entity.HasIndex(e => e.Id)
                    .HasName("SYS_C0011118")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasColumnType("NUMBER");

                entity.Property(e => e.Creater)
                    .HasColumnName("CREATER")
                    .HasColumnType("VARCHAR2(256)");

                entity.Property(e => e.Createtime)
                    .HasColumnName("CREATETIME")
                    .HasColumnType("VARCHAR2(32)");

                entity.Property(e => e.Flashtime)
                    .HasColumnName("FLASHTIME")
                    .HasColumnType("DATE");

                entity.Property(e => e.Logintime)
                    .HasColumnName("LOGINTIME")
                    .HasColumnType("VARCHAR2(32)");

                entity.Property(e => e.Phone)
                    .HasColumnName("PHONE")
                    .HasColumnType("VARCHAR2(16)");

                entity.Property(e => e.Pwd)
                    .IsRequired()
                    .HasColumnName("PWD")
                    .HasColumnType("VARCHAR2(32)");

                entity.Property(e => e.Roleids)
                    .HasColumnName("ROLEIDS")
                    .HasColumnType("VARCHAR2(128)");

                entity.Property(e => e.Token)
                    .HasColumnName("TOKEN")
                    .HasColumnType("VARCHAR2(256)");

                entity.Property(e => e.Userid)
                    .IsRequired()
                    .HasColumnName("USERID")
                    .HasColumnType("VARCHAR2(256)");

                entity.Property(e => e.Username)
                    .IsRequired()
                    .HasColumnName("USERNAME")
                    .HasColumnType("VARCHAR2(256)");

                entity.Property(e => e.Post)
                    .IsRequired()
                    .HasColumnName("POST")
                    .HasColumnType("VARCHAR2(32)");
            });

            modelBuilder.Entity<UserPermission>(entity =>
            {
                entity.ToTable("USER_PERMISSION");

                entity.HasIndex(e => e.Id)
                    .HasName("SYS_C0010816")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasColumnType("NUMBER");

                entity.Property(e => e.Createtime)
                    .HasColumnName("CREATETIME")
                    .HasColumnType("VARCHAR2(32)");

                entity.Property(e => e.Ispermission)
                    .HasColumnName("ISPERMISSION")
                    .HasColumnType("VARCHAR2(128)");

                entity.Property(e => e.Permissionid)
                    .HasColumnName("PERMISSIONID")
                    .HasColumnType("VARCHAR2(128)");

                entity.Property(e => e.Userid)
                    .HasColumnName("USERID")
                    .HasColumnType("VARCHAR2(256)");
            });

            modelBuilder.Entity<BillGoodsIn>(entity =>
            {
                entity.ToTable("BILL_GOODSIN");

                entity.HasIndex(e => e.Id)
                    .HasName("SYS_C0011321")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasColumnType("NUMBER");

                entity.Property(e => e.Abholung)
                    .HasColumnName("ABHOLUNG")
                    .HasColumnType("VARCHAR2(16)")
                    .HasDefaultValueSql("'否'");

                entity.Property(e => e.Billid)
                    .HasColumnName("BILLID")
                    .HasColumnType("VARCHAR2(64)");

                entity.Property(e => e.Billstat)
                    .HasColumnName("BILLSTAT")
                    .HasColumnType("VARCHAR2(32)")
                    .HasDefaultValueSql(@"'进厂计划'");

                entity.Property(e => e.Company)
                    .HasColumnName("COMPANY")
                    .HasColumnType("VARCHAR2(64)");

                entity.Property(e => e.Createtime)
                    .HasColumnName("CREATETIME")
                    .HasColumnType("VARCHAR2(32)");

                entity.Property(e => e.Drivers)
                    .HasColumnName("DRIVERS")
                    .HasColumnType("VARCHAR2(3072)");

                entity.Property(e => e.Endtime)
                    .HasColumnName("ENDTIME")
                    .HasColumnType("VARCHAR2(32)");

                entity.Property(e => e.Goodsname)
                    .HasColumnName("GOODSNAME")
                    .HasColumnType("VARCHAR2(64)");

                entity.Property(e => e.Goodsnumber)
                    .HasColumnName("GOODSNUMBER")
                    .HasColumnType("NUMBER")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.Goodsstat)
                    .HasColumnName("GOODSSTAT")
                    .HasColumnType("VARCHAR2(64)");

                entity.Property(e => e.Maker)
                    .HasColumnName("MAKER")
                    .HasColumnType("VARCHAR2(64)");

                entity.Property(e => e.Starttime)
                    .HasColumnName("STARTTIME")
                    .HasColumnType("VARCHAR2(32)");

                entity.Property(e => e.Supercargo)
                    .HasColumnName("SUPERCARGO")
                    .HasColumnType("VARCHAR2(3072)");

                entity.Property(e => e.Tractorids)
                    .HasColumnName("TRACTORIDS")
                    .HasColumnType("VARCHAR2(3072)");

                entity.Property(e => e.Trailerids)
                    .HasColumnName("TRAILERIDS")
                    .HasColumnType("VARCHAR2(3072)");

                entity.Property(e => e.Transport)
                    .HasColumnName("TRANSPORT")
                    .HasColumnType("VARCHAR2(64)");

                entity.Property(e => e.Freezestatus)
                    .HasColumnName("FREEZESTATUS")
                    .HasColumnType("VARCHAR2(64)");

                entity.Property(e => e.Status)
                    .HasColumnName("STATUS")
                    .HasColumnType("VARCHAR2(64)");

                entity.Property(e => e.Levelnumber)
                    .HasColumnName("LEVELNUMBER")
                    .HasColumnType("NUMBER")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.Goodsid)
                    .HasColumnName("GOODSID")
                    .HasColumnType("VARCHAR2(64)");

                entity.Property(e => e.Remark)
                    .HasColumnName("REMARK")
                    .HasColumnType("VARCHAR2(200)");
            });

            modelBuilder.Entity<BillGoodsOut>(entity =>
            {
                entity.ToTable("BILL_GOODSOUT");

                entity.HasIndex(e => e.Id)
                    .HasName("SYS_C0011359")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasColumnType("NUMBER");

                entity.Property(e => e.Abholung)
                    .HasColumnName("ABHOLUNG")
                    .HasColumnType("VARCHAR2(16)")
                    .HasDefaultValueSql("'否'");

                entity.Property(e => e.Billid)
                    .HasColumnName("BILLID")
                    .HasColumnType("VARCHAR2(64)");

                entity.Property(e => e.Billstat)
                    .HasColumnName("BILLSTAT")
                    .HasColumnType("VARCHAR2(32)")
                    .HasDefaultValueSql(@"'出厂计划'");

                entity.Property(e => e.Company)
                    .HasColumnName("COMPANY")
                    .HasColumnType("VARCHAR2(64)");

                entity.Property(e => e.Createtime)
                    .HasColumnName("CREATETIME")
                    .HasColumnType("VARCHAR2(32)");

                entity.Property(e => e.Drivers)
                    .HasColumnName("DRIVERS")
                    .HasColumnType("VARCHAR2(3072)");

                entity.Property(e => e.Endtime)
                    .HasColumnName("ENDTIME")
                    .HasColumnType("VARCHAR2(32)");

                entity.Property(e => e.Goodsname)
                    .HasColumnName("GOODSNAME")
                    .HasColumnType("VARCHAR2(64)");

                entity.Property(e => e.Goodsnumber)
                    .HasColumnName("GOODSNUMBER")
                    .HasColumnType("NUMBER")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.Goodsstat)
                    .HasColumnName("GOODSSTAT")
                    .HasColumnType("VARCHAR2(64)");

                entity.Property(e => e.Salesman)
                    .HasColumnName("SALESMAN")
                    .HasColumnType("VARCHAR2(64)");

                entity.Property(e => e.Starttime)
                    .HasColumnName("STARTTIME")
                    .HasColumnType("VARCHAR2(32)");

                entity.Property(e => e.Supercargo)
                    .HasColumnName("SUPERCARGO")
                    .HasColumnType("VARCHAR2(3072)");

                entity.Property(e => e.Tractorids)
                    .HasColumnName("TRACTORIDS")
                    .HasColumnType("VARCHAR2(3072)");

                entity.Property(e => e.Trailerids)
                    .HasColumnName("TRAILERIDS")
                    .HasColumnType("VARCHAR2(3072)");

                entity.Property(e => e.Transport)
                    .HasColumnName("TRANSPORT")
                    .HasColumnType("VARCHAR2(64)");

                entity.Property(e => e.Freezestatus)
                    .HasColumnName("FREEZESTATUS")
                    .HasColumnType("VARCHAR2(64)");

                entity.Property(e => e.Status)
                    .HasColumnName("STATUS")
                    .HasColumnType("VARCHAR2(64)");

                entity.Property(e => e.FinishStatus)
                    .HasColumnName("FINISHSTATUS")
                    .HasColumnType("VARCHAR2(64)");

                entity.Property(e => e.Levelnumber)
                    .HasColumnName("LEVELNUMBER")
                    .HasColumnType("NUMBER")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.Goodsid)
                    .HasColumnName("GOODSID")
                    .HasColumnType("VARCHAR2(64)");

                entity.Property(e => e.Remark)
                    .HasColumnName("REMARK")
                    .HasColumnType("VARCHAR2(200)");
            });

            modelBuilder.Entity<OrderGoods>(entity =>
            {
                entity.ToTable("ORDER_GOODS");

                entity.HasIndex(e => e.Id)
                    .HasName("SYS_C0011323")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasColumnType("NUMBER");

                entity.Property(e => e.Billid)
                    .HasColumnName("BILLID")
                    .HasColumnType("VARCHAR2(64)");

                entity.Property(e => e.Capacity)
                    .HasColumnName("CAPACITY")
                    .HasColumnType("VARCHAR2(32)");

                entity.Property(e => e.Checkman)
                    .HasColumnName("CHECKMAN")
                    .HasColumnType("VARCHAR2(64)");

                entity.Property(e => e.Createtime)
                    .HasColumnName("CREATETIME")
                    .HasColumnType("VARCHAR2(32)");

                entity.Property(e => e.Driver)
                    .HasColumnName("DRIVER")
                    .HasColumnType("VARCHAR2(64)");

                entity.Property(e => e.Goodsid)
                    .HasColumnName("GOODSID")
                    .HasColumnType("VARCHAR2(64)");

                entity.Property(e => e.Goodsname)
                    .HasColumnName("GOODSNAME")
                    .HasColumnType("VARCHAR2(64)");

                entity.Property(e => e.Goodstypr)
                    .HasColumnName("GOODSTYPR")
                    .HasColumnType("VARCHAR2(64)");

                entity.Property(e => e.Grossman)
                    .HasColumnName("GROSSMAN")
                    .HasColumnType("VARCHAR2(64)");

                entity.Property(e => e.Grosstime)
                    .HasColumnName("GROSSTIME")
                    .HasColumnType("VARCHAR2(32)");

                entity.Property(e => e.Grossweight)
                    .HasColumnName("GROSSWEIGHT")
                    .HasColumnType("VARCHAR2(32)");

                entity.Property(e => e.Indoorman)
                    .HasColumnName("INDOORMAN")
                    .HasColumnType("VARCHAR2(64)");

                entity.Property(e => e.Loadweight)
                    .HasColumnName("LOADWEIGHT")
                    .HasColumnType("VARCHAR2(64)");

                entity.Property(e => e.Netweight)
                    .HasColumnName("NETWEIGHT")
                    .HasColumnType("VARCHAR2(32)");

                entity.Property(e => e.Company)
                    .HasColumnName("COMPANY")
                    .HasColumnType("VARCHAR2(2000)");

                entity.Property(e => e.Orderid)
                    .HasColumnName("ORDERID")
                    .HasColumnType("VARCHAR2(64)");

                entity.Property(e => e.Ordername)
                    .HasColumnName("ORDERNAME")
                    .HasColumnType("VARCHAR2(64)");

                entity.Property(e => e.Orderstat)
                    .HasColumnName("ORDERSTAT")
                    .HasColumnType("VARCHAR2(64)");

                entity.Property(e => e.Ordertime)
                    .HasColumnName("ORDERTIME")
                    .HasColumnType("VARCHAR2(32)");

                entity.Property(e => e.Outdoorman)
                    .HasColumnName("OUTDOORMAN")
                    .HasColumnType("VARCHAR2(64)");

                entity.Property(e => e.Printid)
                    .HasColumnName("PRINTID")
                    .HasColumnType("VARCHAR2(64)");

                entity.Property(e => e.Printman)
                    .HasColumnName("PRINTMAN")
                    .HasColumnType("VARCHAR2(64)");

                entity.Property(e => e.Printtime)
                    .HasColumnName("PRINTTIME")
                    .HasColumnType("VARCHAR2(32)");

                entity.Property(e => e.Realweight)
                    .HasColumnName("REALWEIGHT")
                    .HasColumnType("VARCHAR2(64)");

                entity.Property(e => e.Remark)
                    .HasColumnName("REMARK")
                    .HasColumnType("VARCHAR2(120)");

                entity.Property(e => e.Storeman)
                    .HasColumnName("STOREMAN")
                    .HasColumnType("VARCHAR2(64)");

                entity.Property(e => e.Supercargo)
                    .HasColumnName("SUPERCARGO")
                    .HasColumnType("VARCHAR2(64)");

                entity.Property(e => e.Tareman)
                    .HasColumnName("TAREMAN")
                    .HasColumnType("VARCHAR2(64)");

                entity.Property(e => e.Taretime)
                    .HasColumnName("TARETIME")
                    .HasColumnType("VARCHAR2(32)");

                entity.Property(e => e.Tareweight)
                    .HasColumnName("TAREWEIGHT")
                    .HasColumnType("VARCHAR2(32)");

                entity.Property(e => e.Tractorid)
                    .HasColumnName("TRACTORID")
                    .HasColumnType("VARCHAR2(64)");

                entity.Property(e => e.Trailerid)
                    .HasColumnName("TRAILERID")
                    .HasColumnType("VARCHAR2(64)");

                entity.Property(e => e.Transport)
                    .HasColumnName("TRANSPORT")
                    .HasColumnType("VARCHAR2(64)");

                entity.Property(e => e.Unit)
                    .HasColumnName("UNIT")
                    .HasColumnType("VARCHAR2(16)");

                entity.Property(e => e.Unitext)
                    .HasColumnName("UNITEXT")
                    .HasColumnType("VARCHAR2(16)");

                entity.Property(e => e.Waybillid)
                    .HasColumnName("WAYBILLID")
                    .HasColumnType("VARCHAR2(64)");

                entity.Property(e => e.Istoexit)
                    .HasColumnName("ISTOEXIT")
                    .HasColumnType("VARCHAR2(64)");

                entity.Property(e => e.Issendback)
                    .HasColumnName("ISSENDBACK")
                    .HasColumnType("VARCHAR2(64)");

                entity.Property(e => e.Billcheck)
                    .HasColumnName("BILLCHECK")
                    .HasColumnType("VARCHAR2(4)");

                entity.Property(e => e.Customercheck)
                    .HasColumnName("CUSTOMERCHECK")
                    .HasColumnType("VARCHAR2(4)");

                entity.Property(e => e.Safecheckflag)
                    .HasColumnName("SAFECHECKFLAG")
                    .HasColumnType("VARCHAR2(6)");
            });

            modelBuilder.Entity<BillGoodsRefund>(entity =>
            {
                entity.ToTable("BILL_GOODSREFUND");

                entity.HasIndex(e => e.Id)
                    .HasName("SYS_C0011393")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasColumnType("NUMBER");

                entity.Property(e => e.Abholung)
                    .HasColumnName("ABHOLUNG")
                    .HasColumnType("VARCHAR2(16)")
                    .HasDefaultValueSql("'否'");

                entity.Property(e => e.Billid)
                    .HasColumnName("BILLID")
                    .HasColumnType("VARCHAR2(64)");

                entity.Property(e => e.Company)
                    .HasColumnName("COMPANY")
                    .HasColumnType("VARCHAR2(64)");

                entity.Property(e => e.Createtime)
                    .HasColumnName("CREATETIME")
                    .HasColumnType("VARCHAR2(32)");

                entity.Property(e => e.Drivers)
                    .HasColumnName("DRIVERS")
                    .HasColumnType("VARCHAR2(3072)");

                entity.Property(e => e.Endtime)
                    .HasColumnName("ENDTIME")
                    .HasColumnType("VARCHAR2(32)");

                entity.Property(e => e.Goodsname)
                    .HasColumnName("GOODSNAME")
                    .HasColumnType("VARCHAR2(64)");

                entity.Property(e => e.Goodsnumber)
                    .HasColumnName("GOODSNUMBER")
                    .HasColumnType("NUMBER")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.Goodsstat)
                    .HasColumnName("GOODSSTAT")
                    .HasColumnType("VARCHAR2(64)");

                entity.Property(e => e.Maker)
                    .HasColumnName("MAKER")
                    .HasColumnType("VARCHAR2(64)");

                entity.Property(e => e.Starttime)
                    .HasColumnName("STARTTIME")
                    .HasColumnType("VARCHAR2(32)");

                entity.Property(e => e.Supercargo)
                    .HasColumnName("SUPERCARGO")
                    .HasColumnType("VARCHAR2(3072)");

                entity.Property(e => e.Tractorids)
                    .HasColumnName("TRACTORIDS")
                    .HasColumnType("VARCHAR2(3072)");

                entity.Property(e => e.Trailerids)
                    .HasColumnName("TRAILERIDS")
                    .HasColumnType("VARCHAR2(3072)");

                entity.Property(e => e.Transport)
                    .HasColumnName("TRANSPORT")
                    .HasColumnType("VARCHAR2(64)");

                entity.Property(e => e.Billstat)
                    .HasColumnName("BILLSTAT")
                    .HasColumnType("VARCHAR2(32)")
                    .HasDefaultValueSql(@"'退货'");

                entity.Property(e => e.Freezestatus)
                    .HasColumnName("FREEZESTATUS")
                    .HasColumnType("VARCHAR2(64)");

                entity.Property(e => e.Status)
                    .HasColumnName("STATUS")
                    .HasColumnType("VARCHAR2(64)");

                entity.Property(e => e.Levelnumber)
                    .HasColumnName("LEVELNUMBER")
                    .HasColumnType("NUMBER")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.Issendback)
                    .HasColumnName("ISSENDBACK")
                    .HasColumnType("VARCHAR2(64)");

                entity.Property(e => e.Goodsid)
                    .HasColumnName("GOODSID")
                    .HasColumnType("VARCHAR2(64)");

                entity.Property(e => e.Remark)
                    .HasColumnName("REMARK")
                    .HasColumnType("VARCHAR2(200)");
            });

            modelBuilder.Entity<SafeCheck>(entity =>
            {
                entity.ToTable("SAFE_CHECK");

                entity.HasIndex(e => e.Id)
                    .HasName("SYS_C0011397")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("ID").HasColumnType("NUMBER");

                entity.Property(e => e.Checkman)
                    .HasColumnName("CHECKMAN")
                    .HasColumnType("VARCHAR2(32)");

                entity.Property(e => e.Checktime)
                    .HasColumnName("CHECKTIME")
                    .HasColumnType("VARCHAR2(32)");

                entity.Property(e => e.Driver)
                    .HasColumnName("DRIVER")
                    .HasColumnType("VARCHAR2(64)");

                entity.Property(e => e.Expburn)
                    .HasColumnName("EXPBURN")
                    .HasColumnType("VARCHAR2(32)");

                entity.Property(e => e.Helmet)
                    .HasColumnName("HELMET")
                    .HasColumnType("VARCHAR2(64)");

                entity.Property(e => e.Remark)
                    .HasColumnName("REMARK")
                    .HasColumnType("VARCHAR2(120)");

                entity.Property(e => e.Smock)
                    .HasColumnName("SMOCK")
                    .HasColumnType("VARCHAR2(200)");

                entity.Property(e => e.Supercargo)
                    .HasColumnName("SUPERCARGO")
                    .HasColumnType("VARCHAR2(64)");

                entity.Property(e => e.Tire)
                    .HasColumnName("TIRE")
                    .HasColumnType("VARCHAR2(32)");

                entity.Property(e => e.Tractorid)
                    .HasColumnName("TRACTORID")
                    .HasColumnType("VARCHAR2(64)");

                entity.Property(e => e.Trailerid)
                    .HasColumnName("TRAILERID")
                    .HasColumnType("VARCHAR2(64)");

                entity.Property(e => e.Workshoe)
                    .HasColumnName("WORKSHOE")
                    .HasColumnType("VARCHAR2(32)");

                entity.Property(e => e.Createtime)
                    .HasColumnName("CREATETIME")
                    .HasColumnType("VARCHAR2(32)");
            });

            modelBuilder.Entity<OrderConfig>(entity =>
            {
                entity.ToTable("ORDER_CONFIG");

                entity.HasIndex(e => e.Id)
                    .HasName("SYS_C0011444")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("ID").HasColumnType("NUMBER");

                entity.Property(e => e.Cardtime)
                    .HasColumnName("CARDTIME")
                    .HasColumnType("VARCHAR2(32)");

                entity.Property(e => e.Configtype)
                    .HasColumnName("CONFIGTYPE")
                    .HasColumnType("VARCHAR2(32)")
                    .HasDefaultValueSql(@"'后台'");

                entity.Property(e => e.Createtime)
                    .HasColumnName("CREATETIME")
                    .HasColumnType("VARCHAR2(32)");

                entity.Property(e => e.Firstweightime)
                    .HasColumnName("FIRSTWEIGHTIME")
                    .HasColumnType("VARCHAR2(32)");

                entity.Property(e => e.Goodsname)
                    .HasColumnName("GOODSNAME")
                    .HasColumnType("VARCHAR2(64)");

                entity.Property(e => e.Maxcarnumber)
                    .HasColumnName("MAXCARNUMBER")
                    .HasColumnType("NUMBER")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.Ordertime)
                    .HasColumnName("ORDERTIME")
                    .HasColumnType("VARCHAR2(32)");

                entity.Property(e => e.Outtime)
                    .HasColumnName("OUTTIME")
                    .HasColumnType("VARCHAR2(32)");

                entity.Property(e => e.Secondweightime)
                    .HasColumnName("SECONDWEIGHTIME")
                    .HasColumnType("VARCHAR2(32)");

                entity.Property(e => e.Typelimit)
                    .HasColumnName("TYPELIMIT")
                    .HasColumnType("VARCHAR2(64)");

                entity.Property(e => e.Worktime)
                    .HasColumnName("WORKTIME")
                    .HasColumnType("VARCHAR2(32)");

                entity.Property(e => e.Loadarea)
                    .HasColumnName("LOADAREA")
                    .HasColumnType("VARCHAR2(64)");

                entity.Property(e => e.Unloadarea)
                    .HasColumnName("UNLOADAREA")
                    .HasColumnType("VARCHAR2(64)");
            });

            modelBuilder.Entity<UserOplog>(entity =>
            {
                entity.ToTable("USER_OPLOG");

                entity.HasIndex(e => e.Id)
                    .HasName("SYS_C0011834")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasColumnType("NUMBER");

                entity.Property(e => e.Createtime)
                    .HasColumnName("CREATETIME")
                    .HasColumnType("VARCHAR2(32)");

                entity.Property(e => e.Detail)
                    .HasColumnName("DETAIL")
                    .HasColumnType("VARCHAR2(3072)");

                entity.Property(e => e.Opcontent)
                    .HasColumnName("OPCONTENT")
                    .HasColumnType("VARCHAR2(512)");

                entity.Property(e => e.Userid)
                    .IsRequired()
                    .HasColumnName("USERID")
                    .HasColumnType("VARCHAR2(32)");

                entity.Property(e => e.UseridDest)
                    .HasColumnName("USERID_DEST")
                    .HasColumnType("VARCHAR2(32)");

                entity.Property(e => e.Stat)
                    .HasColumnName("STAT")
                    .HasColumnType("VARCHAR2(16)");

                entity.Property(e => e.Cmd)
                    .HasColumnName("CMD")
                    .HasColumnType("VARCHAR2(32)");

                entity.Property(e => e.Clientip)
                    .HasColumnName("CLIENTIP")
                    .HasColumnType("VARCHAR2(32)");
            });

            modelBuilder.Entity<ApiGateevent>(entity =>
            {
                entity.ToTable("API_GATEEVENT");

                entity.HasIndex(e => e.Id)
                    .HasName("SYS_C0012173")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasColumnType("NUMBER");

                entity.Property(e => e.Alarmcar).HasColumnName("ALARMCAR");

                entity.Property(e => e.Cardno)
                    .HasColumnName("CARDNO")
                    .HasColumnType("VARCHAR2(64)");

                entity.Property(e => e.Createtime)
                    .HasColumnName("CREATETIME")
                    .HasColumnType("VARCHAR2(32)");

                entity.Property(e => e.Eventcmd).HasColumnName("EVENTCMD");

                entity.Property(e => e.Eventtype).HasColumnName("EVENTTYPE");

                entity.Property(e => e.Eventtypename)
                    .HasColumnName("EVENTTYPENAME")
                    .HasColumnType("VARCHAR2(64)");

                entity.Property(e => e.Gatename)
                    .HasColumnName("GATENAME")
                    .HasColumnType("VARCHAR2(64)");

                entity.Property(e => e.Parkname)
                    .HasColumnName("PARKNAME")
                    .HasColumnType("VARCHAR2(64)");

                entity.Property(e => e.Platecolor).HasColumnName("PLATECOLOR");

                entity.Property(e => e.Plateno)
                    .HasColumnName("PLATENO")
                    .HasColumnType("VARCHAR2(64)");

                entity.Property(e => e.Platetype).HasColumnName("PLATETYPE");

                entity.Property(e => e.Roadwayname)
                    .HasColumnName("ROADWAYNAME")
                    .HasColumnType("VARCHAR2(64)");

                entity.Property(e => e.Roadwaytype).HasColumnName("ROADWAYTYPE");

                entity.Property(e => e.Starttime)
                    .HasColumnName("STARTTIME")
                    .HasColumnType("VARCHAR2(64)");

                entity.Property(e => e.Stoptime)
                    .HasColumnName("STOPTIME")
                    .HasColumnType("VARCHAR2(64)");

                entity.Property(e => e.Subsystype).HasColumnName("SUBSYSTYPE");

                entity.Property(e => e.Vehiclecolor).HasColumnName("VEHICLECOLOR");

                entity.Property(e => e.Vehicletype).HasColumnName("VEHICLETYPE");
            });
        }
    }
}
