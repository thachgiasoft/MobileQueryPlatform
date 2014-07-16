/*==============================================================*/
/* DBMS name:      Microsoft SQL Server 2000                    */
/* Created on:     2014-07-16 19:52:11                          */
/*==============================================================*/


if exists (select 1
   from dbo.sysreferences r join dbo.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('dbo.tReport') and o.name = 'FK_TREPORT_REFERENCE_TDATABAS')
alter table dbo.tReport
   drop constraint FK_TREPORT_REFERENCE_TDATABAS
go

if exists (select 1
   from dbo.sysreferences r join dbo.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('dbo.tReportColumn') and o.name = 'FK_TREPORTCOLUMN_REFERENCE_TREPORT')
alter table dbo.tReportColumn
   drop constraint FK_TREPORTCOLUMN_REFERENCE_TREPORT
go

if exists (select 1
   from dbo.sysreferences r join dbo.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('dbo.tReportCommand') and o.name = 'FK_TREPORTCOMMAND_REFERENCE_TREPORT')
alter table dbo.tReportCommand
   drop constraint FK_TREPORTCOMMAND_REFERENCE_TREPORT
go

if exists (select 1
   from dbo.sysreferences r join dbo.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('dbo.tReportParam') and o.name = 'FK_TREPORTPARAM_REFERENCE_TREPORT')
alter table dbo.tReportParam
   drop constraint FK_TREPORTPARAM_REFERENCE_TREPORT
go

if exists (select 1
   from dbo.sysreferences r join dbo.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('dbo.tReportParamItem') and o.name = 'FK_TREPORTPARAMOPTION_REFERENCE_TREPORT')
alter table dbo.tReportParamItem
   drop constraint FK_TREPORTPARAMOPTION_REFERENCE_TREPORT
go

if exists (select 1
   from dbo.sysreferences r join dbo.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('dbo.tReportParamItem') and o.name = 'FK_TREPORTP_REFERENCE_TREPORTP')
alter table dbo.tReportParamItem
   drop constraint FK_TREPORTP_REFERENCE_TREPORTP
go

if exists (select 1
   from dbo.sysreferences r join dbo.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('dbo.tReportResult') and o.name = 'FK_TREPORTR_REFERENCE_TREPORT')
alter table dbo.tReportResult
   drop constraint FK_TREPORTR_REFERENCE_TREPORT
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

if exists (select 1
            from  sysobjects
           where  id = object_id('dbo.tReportCommand')
            and   type = 'U')
   drop table dbo.tReportCommand
go

alter table dbo.tReportParam
   drop constraint PK_TREPORTPARAM
go

if exists (select 1
            from  sysobjects
           where  id = object_id('dbo.tReportParam')
            and   type = 'U')
   drop table dbo.tReportParam
go

alter table dbo.tReportParamItem
   drop constraint PK_TREPORTPARAMITEM
go

if exists (select 1
            from  sysobjects
           where  id = object_id('dbo.tReportParamItem')
            and   type = 'U')
   drop table dbo.tReportParamItem
go

alter table dbo.tReportResult
   drop constraint PK_TREPORTRESULT
go

if exists (select 1
            from  sysobjects
           where  id = object_id('dbo.tReportResult')
            and   type = 'U')
   drop table dbo.tReportResult
go

alter table dbo.tSysCfg
   drop constraint PK_TSYSCFG
go

if exists (select 1
            from  sysobjects
           where  id = object_id('dbo.tSysCfg')
            and   type = 'U')
   drop table dbo.tSysCfg
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
   DbCode               char(128)            not null,
   DbType               numeric(1)           not null default 0,
   DataSource           char(50)             not null,
   DbName               char(50)             not null,
   UserID               char(50)             not null,
   Password             char(50)             not null,
   Remark               char(512)            null,
   constraint PK_TDATABASE primary key (ID),
   constraint AK_KEY_2_TDATABAS unique (DbCode)
)
go

/*==============================================================*/
/* Table: tReport                                               */
/*==============================================================*/
create table tReport (
   ID                   numeric              identity,
   DBID                 numeric              not null,
   ReportName           char(128)            not null,
   Enabled              numeric(1)           not null default 1,
   Remark               char(500)            null,
   SqlCommand           char(5000)           not null,
   constraint PK_TREPORT primary key (ID)
)
go

/*==============================================================*/
/* Table: tReportColumn                                         */
/*==============================================================*/
create table tReportColumn (
   ReportID             numeric              not null,
   ColumnCode           char(128)            not null,
   ColumnName           char(128)            not null,
   Sumabled             numeric(1)           not null default 0,
   Sortabled            numeric(1)           not null default 0,
   constraint PK_TREPORTCOLUMN primary key (ReportID, ColumnCode),
   constraint AK_KEY_2_TREPORTC unique (ColumnCode, ReportID)
)
go

/*==============================================================*/
/* Table: tReportParam                                          */
/*==============================================================*/
create table tReportParam (
   ReportID             numeric              not null,
   ParamCode            char(128)            not null,
   ParamName            char(128)            not null,
   ParamType            numeric(1)           not null default 0,
   ParamInputType       numeric(1)           not null default 0,
   constraint PK_TREPORTPARAM primary key (ReportID, ParamCode)
)
go

/*==============================================================*/
/* Table: tReportParamItem                                      */
/*==============================================================*/
create table tReportParamItem (
   ReportID             numeric              not null,
   ParamCode            char(128)            not null,
   OptionName           char(128)            not null,
   OptionValue          char(128)            not null,
   constraint PK_TREPORTPARAMITEM primary key (ReportID, ParamCode, OptionName)
)
go

/*==============================================================*/
/* Table: tReportResult                                         */
/*==============================================================*/
create table tReportResult (
   ReportID             numeric              not null,
   AllSumabled          numeric(1)           not null default 0,
   PageSumabled         numeric(1)           not null default 0,
   Pagingabled          numeric(1)           not null default 0,
   PageSize             numeric              not null default 10,
   constraint PK_TREPORTRESULT primary key (ReportID)
)
go

/*==============================================================*/
/* Table: tSysCfg                                               */
/*==============================================================*/
create table tSysCfg (
   Section              char(128)            not null,
   OptionName           char(128)            not null,
   OptionValue          char(5000)           not null,
   constraint PK_TSYSCFG primary key (Section, OptionName)
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
   IsAdmin              numeric(1)           not null default 0,
   constraint PK_TUSER primary key (ID),
   constraint AK_KEY_2_TUSER unique (UserCode)
)
go

/*==============================================================*/
/* Table: tUserReport                                           */
/*==============================================================*/
create table tUserReport (
   ID                   numeric              identity,
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
   add constraint FK_TREPORTCOLUMN_REFERENCE_TREPORT foreign key (ReportID)
      references tReport (ID)
go

alter table tReportParam
   add constraint FK_TREPORTPARAM_REFERENCE_TREPORT foreign key (ReportID)
      references tReport (ID)
go

alter table tReportParamItem
   add constraint FK_TREPORTPARAMOPTION_REFERENCE_TREPORT foreign key (ReportID)
      references tReport (ID)
go

alter table tReportParamItem
   add constraint FK_TREPORTP_REFERENCE_TREPORTP foreign key (ReportID, ParamCode)
      references tReportParam (ReportID, ParamCode)
go

alter table tReportResult
   add constraint FK_TREPORTR_REFERENCE_TREPORT foreign key (ReportID)
      references tReport (ID)
go

alter table tUserReport
   add constraint FK_TUSERREP_REFERENCE_TREPORT foreign key (ID)
      references tReport (ID)
go

alter table tUserReport
   add constraint FK_TUSERREP_REFERENCE_TUSER foreign key (UserID)
      references tUser (ID)
go

