using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model;
using DAL.Interface;
using DAL;
using System.Configuration;
using System.Data;
using DAL.Helper;
using System.Text.RegularExpressions;

namespace BLL
{
    /// <summary>
    /// 
    /// </summary>
    public class ReportBLL
    {
        /// <summary>
        /// 获取报表
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static Report GetReport(decimal id)
        {
            try
            {
                using (IDAL dal = DALBuilder.CreateDAL(ConfigurationManager.ConnectionStrings["SYSDB"].ConnectionString, 0))
                {
                    StringBuilder sql = new StringBuilder(256);
                    sql.Append("SELECT A.ID,DbCode, DBID ,ReportName ,Enabled ,SqlCommand,A.Remark,AllSumabled ,PageSumabled ,Pagingabled ,PageSize ");
                    sql.Append(" FROM tReport A, tDatabase B where A.DBID=B.ID AND A.ID=@ID ");
                    dal.OpenReader(sql.ToString(),
                        dal.CreateParameter("@ID", id)
                        );
                    Report rpt;
                    if (dal.DataReader.Read())
                    {
                        rpt = new Report() { 
                            ID=Convert.ToDecimal(dal.DataReader["ID"]),
                            DBID = Convert.ToDecimal(dal.DataReader["DBID"]),
                            DBCode = Convert.ToString(dal.DataReader["DbCode"]).TrimEnd(),
                            ReportName = Convert.ToString(dal.DataReader["ReportName"]).TrimEnd(),
                            Enabled = Convert.ToBoolean(dal.DataReader["Enabled"]),
                            Remark = Convert.ToString(dal.DataReader["Remark"]).TrimEnd(),
                            SqlCommand=Convert.ToString(dal.DataReader["SqlCommand"]).TrimEnd(),
                            AllSumabled=Convert.ToBoolean(dal.DataReader["AllSumabled"]),
                            PageSumabled = Convert.ToBoolean(dal.DataReader["PageSumabled"]),
                            Pagingabled = Convert.ToBoolean(dal.DataReader["Pagingabled"]),
                            PageSize = Convert.ToInt16(dal.DataReader["PageSize"])
                        };
                        dal.DataReader.Close();
                    }
                    else
                    {
                        throw new Exception("未找到报表");
                    }

                    
                    //读取字段集合
                    sql.Clear();
                    sql.Append("SELECT * FROM tReportColumn WHERE ReportID=@ReportID");
                    dal.OpenReader(sql.ToString(),
                        dal.CreateParameter("@ReportID", rpt.ID)
                        );
                    rpt.Columns = ObjectHelper.BuildObject<ReportColumn>(dal.DataReader);
                    dal.DataReader.Close();

                    //读取参数集合
                    sql.Clear();
                    sql.Append("SELECT * FROM tReportParam WHERE ReportID=@ReportID");
                    dal.OpenReader(sql.ToString(),
                        dal.CreateParameter("@ReportID",rpt.ID)
                        );
                    rpt.Params = ObjectHelper.BuildObject<ReportParam>(dal.DataReader);
                    dal.DataReader.Close();

                    //读取参数列表项集合
                    foreach (ReportParam rp in rpt.Params)
                    {
                        if (rp.ParamInputType == 0)
                        {
                            continue;
                        }
                        sql.Clear();
                        sql.Append("SELECT * FROM tReportParamItem WHERE ReportID=@ReportID AND ParamCode=@ParamCode");
                        dal.OpenReader(sql.ToString(),
                            dal.CreateParameter("@ReportID", rpt.ID),
                            dal.CreateParameter("@ParamCode",rp.ParamCode)
                            );
                        rp.ParamItems = ObjectHelper.BuildObject<ReportParamItem>(dal.DataReader);
                        dal.DataReader.Close();
                    }
                    return rpt;
                }
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 获取报表查询结构
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static Report GetReportForQuery(decimal id)
        {
            try
            {
                using (IDAL dal = DALBuilder.CreateDAL(ConfigurationManager.ConnectionStrings["SYSDB"].ConnectionString, 0))
                {
                    StringBuilder sql = new StringBuilder(256);
                    sql.Append("SELECT ID, DBID ,ReportName ,Enabled ,Remark,AllSumabled ,PageSumabled ,Pagingabled ,PageSize ");
                    sql.Append(" FROM tReport  where ID=@ID AND Enabled=1 ");
                    dal.OpenReader(sql.ToString(),
                        dal.CreateParameter("@ID", id)
                        );
                    Report rpt;
                    if (dal.DataReader.Read())
                    {
                        rpt = new Report()
                        {
                            ID = Convert.ToDecimal(dal.DataReader["ID"]),
                            DBID = Convert.ToDecimal(dal.DataReader["DBID"]),
                            ReportName = Convert.ToString(dal.DataReader["ReportName"]).TrimEnd(),
                            Enabled = Convert.ToBoolean(dal.DataReader["Enabled"]),
                            Remark = Convert.ToString(dal.DataReader["Remark"]).TrimEnd(),
                            AllSumabled = Convert.ToBoolean(dal.DataReader["AllSumabled"]),
                            PageSumabled = Convert.ToBoolean(dal.DataReader["PageSumabled"]),
                            Pagingabled = Convert.ToBoolean(dal.DataReader["Pagingabled"]),
                            PageSize = Convert.ToInt16(dal.DataReader["PageSize"])
                        };
                        dal.DataReader.Close();
                    }
                    else
                    {
                        throw new Exception("未找到报表");
                    }


                    //读取字段集合
                    sql.Clear();
                    sql.Append("SELECT * FROM tReportColumn WHERE ReportID=@ReportID");
                    dal.OpenReader(sql.ToString(),
                        dal.CreateParameter("@ReportID", rpt.ID)
                        );
                    rpt.Columns = ObjectHelper.BuildObject<ReportColumn>(dal.DataReader);
                    dal.DataReader.Close();

                    //读取参数集合
                    sql.Clear();
                    sql.Append("SELECT * FROM tReportParam WHERE ReportID=@ReportID");
                    dal.OpenReader(sql.ToString(),
                        dal.CreateParameter("@ReportID", rpt.ID)
                        );
                    rpt.Params = ObjectHelper.BuildObject<ReportParam>(dal.DataReader);
                    dal.DataReader.Close();

                    //读取参数列表项集合
                    foreach (ReportParam rp in rpt.Params)
                    {
                        if (rp.ParamInputType == 0)
                        {
                            continue;
                        }
                        sql.Clear();
                        sql.Append("SELECT * FROM tReportParamItem WHERE ReportID=@ReportID AND ParamCode=@ParamCode");
                        dal.OpenReader(sql.ToString(),
                            dal.CreateParameter("@ReportID", rpt.ID),
                            dal.CreateParameter("@ParamCode", rp.ParamCode)
                            );
                        rp.ParamItems = ObjectHelper.BuildObject<ReportParamItem>(dal.DataReader);
                        dal.DataReader.Close();
                    }
                    return rpt;
                }
            }
            catch
            {
                return null;
            }
        }
        /// <summary>
        /// 列表报表
        /// 仅报表头信息，不包含数据，字段等
        /// </summary>
        /// <returns></returns>
        public static ICollection<Report> ListReport()
        {
            try
            {
                using (IDAL dal = DALBuilder.CreateDAL(ConfigurationManager.ConnectionStrings["SYSDB"].ConnectionString, 0))
                {
                    ICollection<Report> rst;
                    StringBuilder sql = new StringBuilder(256);
                    sql.Append(" SELECT A.ID,DBID,DBCode ,ReportName ,Enabled ,A.Remark ");
                    sql.Append(" FROM tReport A, tDatabase B ");
                    sql.Append(" WHERE A.DBID=B.ID ");
                    dal.OpenReader(sql.ToString());
                    rst = ObjectHelper.BuildObject<Report>(dal.DataReader);
                    return rst;
                }
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 获取用户权限报表
        /// </summary>
        /// <returns></returns>
        public static ICollection<UserReport> ListAllUserReport(decimal userID)
        {
            try
            {
                using (IDAL dal = DALBuilder.CreateDAL(ConfigurationManager.ConnectionStrings["SYSDB"].ConnectionString, 0))
                {
                    StringBuilder sql = new StringBuilder(128);
                    sql.Append(" SELECT ID AS ReportID,@UserID AS UserID,CASE ISNULL(B.ReportID,0) WHEN 0 THEN 0 ELSE 1 END AS Enabled ,A.ReportName ");
                    sql.Append(" FROM ");
                    sql.Append(" (SELECT * FROM dbo.tReport WHERE Enabled=1) A LEFT JOIN ");
                    sql.Append(" (SELECT * FROM dbo.tUserReport WHERE UserID=@userid) B ON A.ID=B.ReportID ");
                    dal.OpenReader(sql.ToString(),
                        dal.CreateParameter("@UserID", userID)
                        );
                    return ObjectHelper.BuildObject<UserReport>(dal.DataReader);
                }
            }
            catch
            {
                return null;
            }
        }

        public static ICollection<UserReport> ListUserReport(decimal userID)
        {
            try
            {
                using (IDAL dal = DALBuilder.CreateDAL(ConfigurationManager.ConnectionStrings["SYSDB"].ConnectionString, 0))
                {
                    StringBuilder sql = new StringBuilder(128);
                    sql.Append(" SELECT B.ID as ReportID,ReportName,UserID,Enabled FROM tUserReport A,tReport B WHERE A.ReportID=B.ID AND UserID=@UserID AND Enabled=1");
                    dal.OpenReader(sql.ToString(),
                        dal.CreateParameter("@UserID", userID)
                        );
                    return ObjectHelper.BuildObject<UserReport>(dal.DataReader);
                }
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 保存用户报表
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="userReports"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static int SaveUserReport(decimal userID, UserReport[] userReports,out string msg)
        {
            try
            {
                using (IDAL dal = DALBuilder.CreateDAL(ConfigurationManager.ConnectionStrings["SYSDB"].ConnectionString, 0))
                {
                    StringBuilder sql = new StringBuilder(256);
                    sql.Append("DELETE FROM tUserReport WHERE UserID=@UserID");
                    dal.BeginTran();
                    int i;
                    dal.Execute(sql.ToString(),out i,
                        dal.CreateParameter("@UserID",userID)
                        );
                    sql.Clear();
                    sql.Append("INSERT INTO tUserReport( UserID, ReportID ) VALUES  ( @UserID, @ReportID )");
                    foreach(UserReport ur in userReports){
                        if (ur.Enabled)
                        {
                            dal.Execute(sql.ToString(), out i,
                                dal.CreateParameter("@UserID", ur.UserID),
                                dal.CreateParameter("@ReportID", ur.ReportID)
                                );
                            if (i == 0)
                            {
                                dal.RollBackTran();
                                msg = "保存失败";
                                return 0;
                            }
                        }
                    }
                    dal.CommitTran();
                    msg = "success";
                    return 1;
                }
            }
            catch (System.Exception ex)
            {
                msg = ex.Message;
                return -1;
            }
        }

        /// <summary>
        /// 添加报表
        /// </summary>
        /// <param name="report"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static int InsertReport(Report report, out string msg)
        {
            try
            {
                using (IDAL dal = DALBuilder.CreateDAL(ConfigurationManager.ConnectionStrings["SYSDB"].ConnectionString, 0))
                {
                    int i;
                    dal.BeginTran();
                    StringBuilder sql = new StringBuilder(256);
                    sql.Append(" INSERT INTO tReport( DBID ,ReportName ,Enabled ,Remark,SqlCommand,AllSumabled ,PageSumabled ,Pagingabled ,PageSize)");
                    sql.Append(" VALUES( ");
                    sql.Append(" @DBID ,@ReportName ,@Enabled ,@Remark,@SqlCommand,@AllSumabled ,@PageSumabled ,@Pagingabled ,@PageSize");
                    sql.Append(" ) ");

                    dal.Execute(sql.ToString(), out i,
                        dal.CreateParameter("@DBID", report.DBID),
                        dal.CreateParameter("@ReportName", report.ReportName),
                        dal.CreateParameter("@Enabled",report.Enabled),
                        dal.CreateParameter("@Remark",report.Remark==null?"":report.Remark),
                        dal.CreateParameter("@SqlCommand",report.SqlCommand),
                        dal.CreateParameter("@AllSumabled", report.AllSumabled),
                        dal.CreateParameter("@PageSumabled", report.PageSumabled),
                        dal.CreateParameter("@Pagingabled", report.Pagingabled),
                        dal.CreateParameter("@PageSize", report.PageSize)
                        );
                    if (i != 1)
                    {
                        dal.RollBackTran();
                        throw new Exception("插入报表头失败");
                    }
                    else{
                        sql.Clear();
                        sql.Append("SELECT IDENT_CURRENT('tReport') AS ID ");
                        dal.OpenReader(sql.ToString());
                        if (dal.DataReader.Read())
                        {
                            report.ID = Convert.ToDecimal(dal.DataReader["ID"]);//读取最新ReportID
                        }
                        else
                        {
                            dal.RollBackTran();
                            throw new Exception("获取报表ID失败");
                        }
                        
                        dal.DataReader.Close();
                    }
                    //保存字段
                    foreach (ReportColumn c in report.Columns)
                    {
                        sql.Clear();
                        sql.Append("INSERT INTO tReportColumn( ReportID , ColumnCode , ColumnName ,ColumnType,Sumabled  ,Sortabled) ");
                        sql.Append("VALUES( ");
                        sql.Append(" @ReportID ,@ColumnCode ,@ColumnName ,@ColumnType,@Sumabled ,@Sortabled ");
                        sql.Append(" ) ");

                        dal.Execute(sql.ToString(), out i,
                            dal.CreateParameter("@ReportID", report.ID),
                            dal.CreateParameter("@ColumnCode", c.ColumnCode),
                            dal.CreateParameter("@ColumnName", c.ColumnName==null?"":c.ColumnName),
                            dal.CreateParameter("@ColumnType",c.ColumnType),
                            dal.CreateParameter("@Sumabled", c.Sumabled),
                            dal.CreateParameter("@Sortabled", c.Sortabled)
                            );
                        if (i != 1)
                        {
                            dal.RollBackTran();
                            throw new Exception("字段"+c.ColumnCode+"插入失败");
                        }
                    }
                    
                    //保存参数
                    foreach (ReportParam p in report.Params)
                    {
                        sql.Clear();
                        sql.Append(" INSERT INTO tReportParam( ReportID ,ParamCode ,ParamName ,ParamType ,ParamInputType) ");
                        sql.Append(" VAlues (");
                        sql.Append(" @ReportID , @ParamCode ,@ParamName ,@ParamType ,@ParamInputType ");
                        sql.Append(" ) ");
                        dal.Execute(sql.ToString(), out i,
                            dal.CreateParameter("@ReportID", report.ID),
                            dal.CreateParameter("@ParamCode", p.ParamCode),
                            dal.CreateParameter("@ParamName", p.ParamName==null?"":p.ParamName),
                            dal.CreateParameter("@ParamType", p.ParamType),
                            dal.CreateParameter("@ParamInputType", p.ParamInputType)
                            );
                        if (i != 1)
                        {
                            dal.RollBackTran();
                            throw new Exception("参数" + p.ParamCode + "保存失败");
                        }
                        foreach (ReportParamItem op in p.ParamItems)
                        {
                            sql.Clear();
                            sql.Append(" INSERT INTO tReportParamItem( ReportID ,ParamCode ,OptionName , OptionValue) ");
                            sql.Append(" VALUES ( ");
                            sql.Append("  @ReportID ,@ParamCode ,@OptionName , @OptionValue");
                            sql.Append(" ) ");

                            dal.Execute(sql.ToString(), out i,
                                dal.CreateParameter("@ReportID", report.ID),
                                dal.CreateParameter("@ParamCode", op.ParamCode),
                                dal.CreateParameter("@OptionName", op.OptionName==null?"":op.OptionName),
                                dal.CreateParameter("@OptionValue", op.OptionValue)
                                );
                            if (i != 1)
                            {
                                dal.RollBackTran();
                                throw new Exception("参数列表选项" + op.OptionName + "插入失败");
                            }
                        }
                        
                    }
                    dal.CommitTran();
                    msg = "success";
                    return 1;
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                return -1;
            }
        }

        /// <summary>
        /// 更新报表
        /// </summary>
        /// <param name="report"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static int UpdateReport(decimal id, Report report, out string msg)
        {
            try
            {
                if (report.Columns == null && report.Params == null)
                {
                    //无字段、参数集合，确定为更改状态
                    return SetEnabled(id, report.Enabled, out msg);
                }

                using (IDAL dal = DALBuilder.CreateDAL(ConfigurationManager.ConnectionStrings["SYSDB"].ConnectionString, 0))
                {
                    StringBuilder sql = new StringBuilder(256);
                    dal.BeginTran();
                    sql.Append(" UPDATE tReport SET DBID=@DBID, ReportName=@ReportName, Enabled=@Enabled, Remark =@Remark,SqlCommand=@SqlCommand, AllSumabled=@AllSumabled ,PageSumabled=@PageSumabled ,Pagingabled=@Pagingabled ,PageSize=@PageSize WHERE ID=@ID");
                    int i;
                    //更新主表
                    dal.Execute(sql.ToString(), out i,
                        dal.CreateParameter("@DBID", report.DBID),
                        dal.CreateParameter("@ReportName", report.ReportName),
                        dal.CreateParameter("@Enabled", report.Enabled),
                        dal.CreateParameter("@Remark", report.Remark==null?"":report.Remark),
                        dal.CreateParameter("@ID", id),
                        dal.CreateParameter("@SqlCommand",report.SqlCommand==null?"":report.SqlCommand),
                        dal.CreateParameter("@AllSumabled", report.AllSumabled),
                        dal.CreateParameter("@PageSumabled", report.PageSumabled),
                        dal.CreateParameter("@Pagingabled", report.Pagingabled),
                        dal.CreateParameter("@PageSize", report.PageSize)
                        );
                    if (i != 1)
                    {
                        dal.RollBackTran();
                        throw new Exception("更新报表头错误");
                    }
                    sql.Clear();
                    //清除Column
                    sql.Append("DELETE FROM tReportColumn WHERE ReportID=@ReportID");
                    dal.Execute(sql.ToString(), out i,
                        dal.CreateParameter("@ReportID", id)
                        );
                    //清除ParamsItem
                    sql.Clear();
                    sql.Append("DELETE FROM tReportParamItem WHERE ReportID=@ReportID");
                    dal.Execute(sql.ToString(), out i,
                        dal.CreateParameter("@ReportID", id)
                        );
                    //清除Params
                    sql.Clear();
                    sql.Append("DELETE FROM tReportParam WHERE ReportID=@ReportID");
                    dal.Execute(sql.ToString(), out i,
                        dal.CreateParameter("@ReportID", id)
                        );
                    //保存字段
                    foreach (ReportColumn c in report.Columns)
                    {
                        sql.Clear();
                        sql.Append("INSERT INTO tReportColumn( ReportID , ColumnCode , ColumnName ,ColumnType,Sumabled  ,Sortabled) ");
                        sql.Append("VALUES( ");
                        sql.Append(" @ReportID ,@ColumnCode ,@ColumnName ,@ColumnType,@Sumabled ,@Sortabled ");
                        sql.Append(" ) ");

                        dal.Execute(sql.ToString(), out i,
                            dal.CreateParameter("@ReportID", id),
                            dal.CreateParameter("@ColumnCode", c.ColumnCode),
                            dal.CreateParameter("@ColumnName", c.ColumnName==null?"":c.ColumnName),
                            dal.CreateParameter("@ColumnType",c.ColumnType),
                            dal.CreateParameter("@Sumabled", Convert.ToInt16(c.Sumabled)),
                            dal.CreateParameter("@Sortabled", Convert.ToInt16(c.Sortabled))
                            );
                        if (i != 1)
                        {
                            dal.RollBackTran();
                            throw new Exception("字段" + c.ColumnCode + "插入失败");
                        }
                    }

                    //保存参数
                    foreach (ReportParam p in report.Params)
                    {
                        sql.Clear();
                        sql.Append(" INSERT INTO tReportParam( ReportID ,ParamCode ,ParamName ,ParamType ,ParamInputType) ");
                        sql.Append(" VAlues (");
                        sql.Append(" @ReportID , @ParamCode ,@ParamName ,@ParamType ,@ParamInputType ");
                        sql.Append(" ) ");
                        dal.Execute(sql.ToString(), out i,
                            dal.CreateParameter("@ReportID", id),
                            dal.CreateParameter("@ParamCode", p.ParamCode),
                            dal.CreateParameter("@ParamName", p.ParamName==null?"":p.ParamName),
                            dal.CreateParameter("@ParamType", p.ParamType),
                            dal.CreateParameter("@ParamInputType", p.ParamInputType)
                            );
                        if (i != 1)
                        {
                            dal.RollBackTran();
                            throw new Exception("参数" + p.ParamCode + "保存失败");
                        }
                        foreach (ReportParamItem op in p.ParamItems)
                        {
                            sql.Clear();
                            sql.Append(" INSERT INTO tReportParamItem( ReportID ,ParamCode ,OptionName , OptionValue) ");
                            sql.Append(" VALUES ( ");
                            sql.Append("  @ReportID ,@ParamCode ,@OptionName , @OptionValue");
                            sql.Append(" ) ");

                            dal.Execute(sql.ToString(), out i,
                                dal.CreateParameter("@ReportID", id),
                                dal.CreateParameter("@ParamCode", op.ParamCode),
                                dal.CreateParameter("@OptionName", op.OptionName==null?"":op.OptionName),
                                dal.CreateParameter("@OptionValue", op.OptionValue)
                                );
                            if (i != 1)
                            {
                                dal.RollBackTran();
                                throw new Exception("参数列表选项" + op.OptionName + "插入失败");
                            }
                        }

                    }
                    dal.CommitTran();
                    msg = "success";
                    return 1;
                }
            }
            catch (System.Exception ex)
            {
                msg = ex.Message;
                return -1;
            }
               
        }

        /// <summary>
        /// 删除报表
        /// </summary>
        /// <param name="id"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static int DeleteReport(decimal id, out string msg)
        {
            try
            {
                using (IDAL dal = DALBuilder.CreateDAL(ConfigurationManager.ConnectionStrings["SYSDB"].ConnectionString, 0))
                {
                    StringBuilder sql = new StringBuilder(256);
                    int i;
                    dal.BeginTran();
                    //清除Column
                    sql.Append("DELETE FROM tReportColumn WHERE ReportID=@ReportID");
                    dal.Execute(sql.ToString(), out i,
                        dal.CreateParameter("@ReportID", id)
                        );
                    //清除ParamsItem
                    sql.Clear();
                    sql.Append("DELETE FROM tReportParamItem WHERE ReportID=@ReportID");
                    dal.Execute(sql.ToString(), out i,
                        dal.CreateParameter("@ReportID", id)
                        );
                    //清除Params
                    sql.Clear();
                    sql.Append("DELETE FROM tReportParam WHERE ReportID=@ReportID");
                    dal.Execute(sql.ToString(), out i,
                        dal.CreateParameter("@ReportID", id)
                        );
                    //清除Report
                    sql.Clear();
                    sql.Append("DELETE FROM tReport WHERE ID=@ID");
                    dal.Execute(sql.ToString(), out i,
                        dal.CreateParameter("@ID", id)
                        );
                    dal.CommitTran();
                    msg = "success";
                    return 1;
                }
            }
            catch (System.Exception ex)
            {
                msg = ex.Message;
                return -1;
            }
        }

        const string PARAMS_REGEX = @"\w*\s*=\s*[@:]\w*";
        const string ALLSUM_FROM_REGEX = @"(?<=from)[^$]*";
        /// <summary>
        /// 执行报表
        /// </summary>
        /// <param name="id">报表ID</param>
        /// <param name="request">请求信息</param>
        /// <returns></returns>
        public static int QueryReport(ReportRequest request,out string result,out string msg)
        {
            try
            {
                Report rpt=GetReport(request.ReportID);//获取报表
                string connectionString;
                short dbType;
                int i = -1;
                i=DatabaseBLL.GetConnectionString(rpt.DBID, out connectionString, out dbType, out msg);
                if (i != 1)
                {
                    //获取数据库连接参数失败
                    result = null;
                    return i;
                }

                //开始组装sql
                StringBuilder sql = new StringBuilder(256);
                sql.Append(rpt.SqlCommand);
                if (!rpt.CommandHasOrderby && !string.IsNullOrEmpty(request.SortColumn))
                {
                    //排序请求
                    sql.AppendFormat("{0} Order By {1} {2}",
                        rpt.SqlCommand,
                        request.SortColumn,
                        request.Desc?"Desc":string.Empty
                        );
                }
                string finalSql=sql.ToString();
                DataTable rstTable;
                using (IDAL dal = DALBuilder.CreateDAL(connectionString, dbType))
                {
                    //组成parameter
                    List<IDbDataParameter> pList = new List<IDbDataParameter>();
                    for (int index = 0; index < request.Params.Length;index++ )
                    {
                        if (request.Params[index].ParamValue == null)
                        {
                            finalSql=Regex.Replace(finalSql,PARAMS_REGEX.Replace(@"\w*",request.Params[index].ParamCode)," 1=1 ");
                            continue;
                        }
                        IDbDataParameter dbp=null;
                        switch (request.Params[index].ParamType)
                        {
                            case 0:
                                dbp = dal.CreateParameter(request.Params[index].ParamCode,DbType.String);
                                dbp.Value=string.IsNullOrEmpty(request.Params[index].ParamValue) ? string.Empty : request.Params[index].ParamValue;
                                break;
                            case 1:
                                decimal v;
                                dbp = dal.CreateParameter(request.Params[index].ParamCode, DbType.Decimal);
                                if (decimal.TryParse(request.Params[index].ParamValue, out v))
                                {
                                    dbp.Value=v;
                                }
                                else
                                {
                                    dbp.Value = null;
                                }
                                break;
                            case 2:
                                DateTime d;
                                dbp=dal.CreateParameter(request.Params[index].ParamCode, DbType.DateTime);
                                if (DateTime.TryParse(request.Params[index].ParamValue, out d))
                                {
                                    dbp.Value = d;
                                }
                                else
                                {
                                    dbp.Value = null;
                                }
                                break;
                        }
                        if (dbp == null)
                        {
                            msg = "参数错误";
                            result = null;
                            return -1;
                        }
                        pList.Add(dbp);

                    }
                    if (rpt.Pagingabled)
                    {
                        //分页请求
                        rstTable = dal.Select(finalSql, rpt.PageSize * (request.Page - 1), rpt.PageSize, out i,pList.ToArray());
                    }
                    else
                    {
                        rstTable = dal.Select(finalSql, out i, pList.ToArray());
                    }

                    if (rpt.Columns.Where(c => c.Sumabled).Count() > 0)
                    {
                        if (rpt.PageSumabled)
                        {
                            //页合计请求
                            DataRow row = rstTable.NewRow();
                            foreach (ReportColumn c in rpt.Columns)
                            {
                                if (!c.Sumabled)
                                {
                                    continue;
                                }
                                row[c.ColumnCode] = rstTable.Compute("sum(" + c.ColumnCode + ")", "");
                            }
                            rstTable.Rows.InsertAt(row, rstTable.Rows.Count);
                        }

                        if (rpt.AllSumabled)
                        {
                            sql.Clear();
                            sql.Append("Select ");
                            //总合计请求
                            foreach (ReportColumn c in rpt.Columns)
                            {
                                if (c == rpt.Columns.Last())
                                {
                                    if (c.Sumabled)
                                    {
                                        sql.AppendFormat(" Sum({0}) AS{0} ",
                                            c.ColumnCode
                                            );
                                    }
                                    else
                                    {
                                        sql.AppendFormat(" null AS {0}",
                                            c.ColumnCode
                                            );
                                    }
                                }
                                else
                                {
                                    if (c.Sumabled)
                                    {
                                        sql.AppendFormat(" Sum({0}) AS {0}, ",
                                            c.ColumnCode
                                            );
                                    }
                                    else
                                    {
                                        sql.AppendFormat(" null AS {0},",
                                            c.ColumnCode
                                            );
                                    }
                                }
                            }
                            sql.AppendFormat(" From {0}", Regex.Match(finalSql, ALLSUM_FROM_REGEX, RegexOptions.IgnoreCase).Value);

                            dal.OpenReader(sql.ToString(), pList.ToArray());
                            if (dal.DataReader.Read())
                            {
                                DataRow newrow = rstTable.NewRow();
                                foreach (ReportColumn c in rpt.Columns)
                                {
                                    if (!c.Sumabled)
                                    {
                                        continue;
                                    }
                                    newrow[c.ColumnCode] = dal.DataReader[c.ColumnCode];
                                }
                                rstTable.Rows.Add(newrow);
                            }
                        }
                    }

                }

               

                result = JsonHelper.DatatableToJson(rstTable);
                return 1;
            }
            catch (System.Exception ex)
            {
                result = null;
                msg = ex.Message;
                return -1;
            }
            
        }

        const string REGEX_PARAMS = @"(?<=@)\D\w+";
        const string REGEX_PARAMS_2=@"@\D\w+";
        /// <summary>
        /// 重建报表结构
        /// </summary>
        /// <param name="id"></param>
        /// <param name="SQL"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static Report RebuildReport(decimal id,decimal dbID, string SQL,out string msg)
        {
            Report report,reportdb;
            report = new Report();
            report.Columns = new List<ReportColumn>();
            report.Params = new List<ReportParam>();
            report.SqlCommand = SQL;
            

            string tmpSql = Regex.Replace(SQL, REGEX_PARAMS_2, "NULL");//将所有参数设置为null，获取表结构
            try
            {
                string tarConn;
                short dbType;
                if (DatabaseBLL.GetConnectionString(dbID, out tarConn,out dbType, out msg) != 1)
                {
                    throw new Exception(msg);
                }

                using (IDAL dal = DALBuilder.CreateDAL(tarConn, dbType))
                {
                    //获取SQL语句中的Column
                    DataSet ds = dal.Select(tmpSql);
                    if (ds.Tables.Count != 1)
                    {
                        throw new Exception("错误：查询结果必须只有一个结果表");
                    }
                    else
                    {

                        foreach (DataColumn c in ds.Tables[0].Columns)
                        {
                            ReportColumn column = new ReportColumn() {
                                ReportID = id,
                                ColumnCode = c.ColumnName
                            };
                            switch (c.DataType.Name) {
                                case "Int16":
                                case "Int32":
                                case "Int64":
                                case "Decimal":
                                case "Float":
                                    column.ColumnType = 1;
                                    break;
                                case "String":
                                    column.ColumnType = 0;
                                    break;
                                case "Datetime":
                                    column.ColumnType = 2;
                                    break;
                                default:
                                    column.ColumnType = 0;
                                    break;
                            }
                            report.Columns.Add(column);
                        }
                    }
                }

                //获取SQL语句中的Param
                MatchCollection pms = Regex.Matches(SQL, REGEX_PARAMS);
                foreach (Match m in pms)
                {
                    //获取SQL语句中的参数
                    report.Params.Add(new ReportParam()
                    {
                        ReportID = id,
                        ParamCode = m.Value
                    });
                }

                if (id != 0)
                {
                    //获取到数据库report
                    report.ID = id;
                    reportdb = GetReport(id);
                    //两个report进行比较
                    //比较params
                    foreach (ReportParam rp in report.Params)
                    {
                        IEnumerable<ReportParam> dbrps = reportdb.Params.Where(p => p.ParamCode == rp.ParamCode);
                        if (dbrps.Count() == 0)
                        {
                            continue;
                        }
                        ReportParam dbrp = dbrps.First();
                        rp.ParamInputType = dbrp.ParamInputType;
                        rp.ParamItems = dbrp.ParamItems;
                        rp.ParamName = dbrp.ParamName;
                        rp.ParamType = dbrp.ParamType;
                    }
                    //比较columns
                    foreach (ReportColumn rc in report.Columns)
                    {
                        IEnumerable<ReportColumn> dbclms = reportdb.Columns.Where(c => c.ColumnCode == rc.ColumnCode);
                        if (dbclms.Count() == 0)
                        {
                            continue;
                        }
                        ReportColumn dbclm = dbclms.First();
                        rc.ColumnName = dbclm.ColumnName;
                        rc.Sortabled =report.CommandHasOrderby?false:dbclm.Sortabled;//如果含有OrderBy 则不允许排序
                        rc.Sumabled = dbclm.Sumabled;
                    }
                }
                msg = "success";
                return report;
                
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                return null;
            }
        }

        /// <summary>
        /// 设置可用状态
        /// </summary>
        /// <param name="id"></param>
        /// <param name="enabled"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static int SetEnabled(decimal id, bool enabled, out string msg)
        {
            try
            {
                using (IDAL dal = DALBuilder.CreateDAL(ConfigurationManager.ConnectionStrings["SYSDB"].ConnectionString, 0))
                {
                    StringBuilder sql = new StringBuilder(128);
                    sql.Append("UPDATE tReport SET Enabled=@Enabled WHERE ID=@ID");
                    int i;
                    dal.BeginTran();
                    dal.Execute(sql.ToString(), out i,
                        dal.CreateParameter("@Enabled", enabled),
                        dal.CreateParameter("@ID", id)
                        );
                    if (i == 1)
                    {
                        dal.CommitTran();
                        msg = "success";
                        return 1;
                    }
                    else
                    {
                        dal.RollBackTran();
                        msg = "error";
                        return 0;
                    }
                }
            }
            catch (System.Exception ex)
            {
                msg = ex.Message;
                return -1;
            }
        }
    }
}
