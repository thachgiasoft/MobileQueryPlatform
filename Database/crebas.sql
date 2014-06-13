/*==============================================================*/
/* DBMS name:      Microsoft SQL Server 2000                    */
/* Created on:     2014-06-13 09:37:05                          */
/*==============================================================*/


if exists (select 1
   from dbo.sysreferences r join dbo.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('dbo.tReport') and o.name = 'FK_TREPORT_REFERENCE_TDATABAS')
alter table dbo.tReport
   drop constraint FK_TREPORT_REFERENCE_TDATABAS
go

if exists (select 1
   from dbo.sysreferences r join dbo.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('dbo.tReportColumn') and o.name = 'FK_TREPORTC_REFERENCE_TREPORT')
alter table dbo.tReportColumn
   drop constraint FK_TREPORTC_REFERENCE_TREPORT
go

if exists (select 1
   from dbo.sysreferences r join dbo.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('dbo.tReportParam') and o.name = 'FK_TREPORTP_REFERENCE_TREPORT')
alter table dbo.tReportParam
   drop constraint FK_TREPORTP_REFERENCE_TREPORT
go

if exists (select 1
   from dbo.sysreferences r join dbo.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('dbo.tReportParamOption') and o.name = 'FK_TREPORTP_REFERENCE_TREPORTP')
alter table dbo.tReportParamOption
   drop constraint FK_TREPORTP_REFERENCE_TREPORTP
go

if exists (select 1
   from dbo.sysreferences r join dbo.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('dbo.tUserReport') and o.name = 'FK_TUSERREP_REFERENCE_TREPORT')
alter table dbo.tUserReport
   drop constraint FK_TUSERREP_REFERENCE_TREPORT
go

if exists (select 1
   from dbo.sysreferences r join dbo.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('dbo.tUserReport') and o.name = 'FK_TUSERREP_REFERENCE_TUSER')
alter table dbo.tUserReport
   drop constraint FK_TUSERREP_REFERENCE_TUSER
go

alter table dbo.tDatabase
   drop constraint PK_TDATABASE
go

alter table dbo.tDatabase
   drop constraint AK_KEY_2_TDATABAS
go

if exists (select 1
            from  sysobjects
           where  id = object_id('dbo.tDatabase')
            and   type = 'U')
   drop table dbo.tDatabase
go

alter table dbo.tReport
   drop constraint PK_TREPORT
go

if exists (select 1
            from  sysobjects
           where  id = object_id('dbo.tReport')
            and   type = 'U')
   drop table dbo.tReport
go

alter table dbo.tReportColumn
   drop constraint PK_TREPORTCOLUMN
go

alter table dbo.tReportColumn
   drop constraint AK_KEY_2_TREPORTC
go

if exists (select 1
            from  sysobjects
           where  id = object_id('dbo.tReportColumn')
            and   type = 'U')
   drop table dbo.tReportColumn
go

alter table dbo.tReportParam
   drop constraint PK_TREPORTPARAM
go

alter table dbo.tReportParam
   drop constraint AK_KEY_2_TREPORTP
go

if exists (select 1
            from  sysobjects
           where  id = object_id('dbo.tReportParam')
            and   type = 'U')
   drop table dbo.tReportParam
go

alter table dbo.tReportParamOption
   drop constraint PK_TREPORTPARAMOPTION
go

alter table dbo.tReportParamOption
   drop constraint AK_KEY_4_TREPORTP
go

alter table dbo.tReportParamOption
   drop constraint AK_KEY_3_TREPORTP
go

if exists (select 1
            from  sysobjects
           where  id = object_id('dbo.tReportParamOption')
            and   type = 'U')
   drop table dbo.tReportParamOption
go

alter table dbo.tSysInfo
   drop constraint PK_TSYSINFO
go

if exists (select 1
            from  sysobjects
           where  id = object_id('dbo.tSysInfo')
            and   type = 'U')
   drop table dbo.tSysInfo
go

alter table dbo.tUser
   drop constraint PK_TUSER
go

alter table dbo.tUser
   drop constraint AK_KEY_2_TUSER
go

if exists (select 1
            from  sysobjects
           where  id = object_id('dbo.tUser')
            and   type = 'U')
   drop table dbo.tUser
go

alter table dbo.tUserReport
   drop constraint PK_TUSERREPORT
go

alter table dbo.tUserReport
   drop constraint AK_KEY_2_TUSERREP
go

if exists (select 1
            from  sysobjects
           where  id = object_id('dbo.tUserReport')
            and   type = 'U')
   drop table dbo.tUserReport
go

execute sp_revokedbaccess dbo
go

/*==============================================================*/
/* Table: tDatabase                                             */
/*==============================================================*/
create table tDatabase (
   ID                   numeric              identity,
   DBCode               char(128)            not null,
   DBType               numeric              not null default 0,
   DataSource           char(128)            not null,
   InitialCatalog       char(128)            not null,
   PersistSecurityInfo  numeric              not null default 0,
   UserID               char(128)            null,
   Password             char(128)            null,
   Remark               char(512)            null,
   constraint PK_TDATABASE primary key (ID),
   constraint AK_KEY_2_TDATABAS unique (DBCode)
)
go

/*==============================================================*/
/* Table: tReport                                               */
/*==============================================================*/
create table tReport (
   ID                   numeric              identity,
   DBID                 numeric              not null,
   ReportName           char(128)            not null,
   SqlCommand           char(5000)           not null,
   PageSumabled         numeric              not null default 0,
   TotalSumabled        numeric              not null default 0,
   Pagingabled          numeric              not null default 1,
   PageSize             numeric              not null default 10,
   constraint PK_TREPORT primary key (ID)
)
go

/*==============================================================*/
/* Table: tReportColumn                                         */
/*==============================================================*/
create table tReportColumn (
   ID                   numeric              not null,
   ReportID             numeric              not null,
   ColumnCode           char(128)            not null,
   ColumnName           char(128)            not null,
   Sumabled             numeric              not null default 0,
   ColumnWidth          numeric              not null default 1,
   Sortabled            numeric              not null default 0,
   constraint PK_TREPORTCOLUMN primary key (ID),
   constraint AK_KEY_2_TREPORTC unique (ReportID, ColumnCode)
)
go

/*==============================================================*/
/* Table: tReportParam                                          */
/*==============================================================*/
create table tReportParam (
   ID                   numeric              not null,
   ReportID             numeric              not null,
   ParamCode            char(128)            not null,
   ParamName            char(128)            not null,
   ParamType            numeric              not null default 0,
   ParamInputType       numeric              not null default 0,
   constraint PK_TREPORTPARAM primary key (ID),
   constraint AK_KEY_2_TREPORTP unique (ReportID, ParamCode)
)
go

/*==============================================================*/
/* Table: tReportParamOption                                    */
/*==============================================================*/
create table tReportParamOption (
   ID                   numeric              not null,
   ParamID              numeric              not null,
   OptionIndex          numeric              not null,
   OptionText           char(128)            not null,
   OptionValue          char(128)            not null,
   constraint PK_TREPORTPARAMOPTION primary key (ID),
   constraint AK_KEY_4_TREPORTP unique (ParamID, OptionText),
   constraint AK_KEY_3_TREPORTP unique (ParamID, OptionValue)
)
go

/*==============================================================*/
/* Table: tSysInfo                                              */
/*==============================================================*/
create table tSysInfo (
   Section              char(128)            not null,
   OptionName           char(128)            not null,
   OptionValue          char(5000)           not null,
   constraint PK_TSYSINFO primary key (Section, OptionName)
)
go

/*==============================================================*/
/* Table: tUser                                                 */
/*==============================================================*/
create table tUser (
   ID                   numeric              identity,
   UserCode             char(128)            not null,
   UserName             char(128)            not null,
   UPassword            char(128)            not null,
   IsAdmin              numeric              not null default 0,
   constraint PK_TUSER primary key (ID),
   constraint AK_KEY_2_TUSER unique (UserCode)
)
go

/*==============================================================*/
/* Table: tUserReport                                           */
/*==============================================================*/
create table tUserReport (
   ID                   numeric              not null,
   UserID               numeric              not null,
   ReportID             numeric              not null,
   constraint PK_TUSERREPORT primary key (ID),
   constraint AK_KEY_2_TUSERREP unique (UserID, ReportID)
)
go

alter table tReport
   add constraint FK_TREPORT_REFERENCE_TDATABAS foreign key (DBID)
      references tDatabase (ID)
go

alter table tReportColumn
   add constraint FK_TREPORTC_REFERENCE_TREPORT foreign key (ReportID)
      references tReport (ID)
go

alter table tReportParam
   add constraint FK_TREPORTP_REFERENCE_TREPORT foreign key (ReportID)
      references tReport (ID)
go

alter table tReportParamOption
   add constraint FK_TREPORTP_REFERENCE_TREPORTP foreign key (ParamID)
      references tReportParam (ID)
go

alter table tUserReport
   add constraint FK_TUSERREP_REFERENCE_TUSER foreign key (UserID)
      references tUser (ID)
go

alter table tUserReport
   add constraint FK_TUSERREP_REFERENCE_TREPORT foreign key (ReportID)
      references tReport (ID)
go

