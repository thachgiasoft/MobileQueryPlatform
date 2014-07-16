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
        public static Report GetReport(int id, bool forEdit)
        {
            try
            {
                using (IDAL dal = DALBuilder.CreateDAL(ConfigurationManager.ConnectionStrings["SYSDB"].ConnectionString, 0))
                {
                    
                    StringBuilder sql = new StringBuilder(256);
                    sql.AppendFormat("SELECT A.ID,DbName, DBID ,ReportName ,{0}Enabled ,A.Remark",forEdit?"SqlCommand,":"");
                    sql.Append(" FROM tReport A, tDatabase B where A.DBID=B.ID AND ID=@ID ");
                    dal.OpenReader(sql.ToString(),
                        dal.CreateParameter("@ID", id)
                        );
                    Report rpt;
                    if (dal.DataReader.Read())
                    {
                        rpt = new Report() { 
                            ID=Convert.ToDecimal(dal.DataReader["ID"]),
                            DBID = Convert.ToDecimal(dal.DataReader["DBID"]),
                            DBName = Convert.ToString(dal.DataReader["DbName"]),
                            ReportName = Convert.ToString(dal.DataReader["ReportName"]),
                            Enabled = Convert.ToBoolean(dal.DataReader["Enabled"]),
                            Remark = Convert.ToString(dal.DataReader["Remark"])
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
                        sql.Append("SELECT * FROM tReportParamOption WHERE ReportID=@ReportID AND ParamCode=@ParamCode");
                        dal.OpenReader(sql.ToString(),
                            dal.CreateParameter("@ReportID", rpt.ID),
                            dal.CreateParameter("@ParamCode",rp.ParamCode)
                            );
                        rp.ParamItems = ObjectHelper.BuildObject<ReportParamItem>(dal.DataReader);
                        dal.DataReader.Close();
                    }

                    //读取结果项
                    sql.Clear();
                    sql.Append("SELECT * FROM tReportResult where ReportID=@ReportID");
                    dal.OpenReader(sql.ToString(),
                        dal.CreateParameter("@ReportID",rpt.ID)
                        );
                    if (dal.DataReader.Read())
                    {
                        rpt.Result = new ReportResult() {
                            ReportID = Convert.ToDecimal(dal.DataReader["ReportID"]),
                            AllSumabled = Convert.ToBoolean(dal.DataReader["AllSumabled"]),
                            PageSumabled = Convert.ToBoolean(dal.DataReader["PageSumabled"]),
                            Pagingabled = Convert.ToBoolean(dal.DataReader["Pageingabled"]),
                            PageSize = Convert.ToInt16(dal.DataReader["PageSize"])
                        };
                    }
                    dal.DataReader.Close();
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
                    sql.Append(" SELECT A.ID,DBID,DbName ,ReportName ,Enabled ,A.Remark ");
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
                    sql.Append(" INSERT INTO tReport( DBID ,ReportName ,Enabled ,Remark,SqlCommand )");
                    sql.Append(" VALUES( ");
                    sql.Append(" @DBID ,@ReportName ,@Enabled ,@Remark,@SqlCommand");
                    sql.Append(" ) ");

                    dal.Execute(sql.ToString(), out i,
                        dal.CreateParameter("@DBID", report.DBID),
                        dal.CreateParameter("@ReportName", report.ReportName),
                        dal.CreateParameter("@Enabled",Convert.ToInt16(report.Enabled)),
                        dal.CreateParameter("@Remark",report.Remark),
                        dal.CreateParameter("@SqlCommand,",report.SqlCommand)
                        );
                    if (i != 1)
                    {
                        dal.RollBackTran();
                        throw new Exception("插入报表头失败");
                    }
                    else{
                        sql.Clear();
                        sql.Append("SELECT IDENT_CURRENT('tReport')-1 AS ID ");
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
                        sql.Append("INSERT INTO tReportColumn( ReportID , ColumnCode , ColumnName ,Sumabled  ,Sortabled) ");
                        sql.Append("VALUES( ");
                        sql.Append(" @ReportID ,@ColumnCode ,@ColumnName ,@Sumabled ,@Sortabled ");
                        sql.Append(" ) ");

                        dal.Execute(sql.ToString(), out i,
                            dal.CreateParameter("@ReportID", report.ID),
                            dal.CreateParameter("@ColumnCode", c.ColumnCode),
                            dal.CreateParameter("@ColumnName", c.ColumnName),
                            dal.CreateParameter("@Sumabled", Convert.ToInt16(c.Sumabled)),
                            dal.CreateParameter("@Sortabled", Convert.ToInt16(c.Sortabled))
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
                            dal.CreateParameter("@ParamName", p.ParamName),
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
                                dal.CreateParameter("@OptionName", op.OptionName),
                                dal.CreateParameter("@OptionValue", op.OptionValue)
                                );
                            if (i != 1)
                            {
                                dal.RollBackTran();
                                throw new Exception("参数列表选项" + op.OptionName + "插入失败");
                            }
                        }
                        
                    }
                    //保存报表结果
                    sql.Clear();
                    sql.Append(" INSERT INTO tReportResult( ReportID ,AllSumabled ,PageSumabled ,Pagingabled ,PageSize) ");
                    sql.Append(" VALUES (");
                    sql.Append("  @ReportID ,@AllSumabled ,@PageSumabled ,@Pagingabled ,@PageSize )");
                    dal.Execute(sql.ToString(), out i,
                        dal.CreateParameter("@ReportID", report.ID),
                        dal.CreateParameter("@AllSumabled", report.Result.AllSumabled),
                        dal.CreateParameter("@PageSumabled", report.Result.PageSumabled),
                        dal.CreateParameter("@Pagingabled", report.Result.Pagingabled),
                        dal.CreateParameter("@PageSize", report.Result.PageSize)
                        );
                    if (i != 1)
                    {
                        dal.RollBackTran();
                        throw new Exception("报表结果项插入失败");
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
        public static int UpdateReport(int id, Report report, out string msg)
        {
            try
            {
                using (IDAL dal = DALBuilder.CreateDAL(ConfigurationManager.ConnectionStrings["SYSDB"].ConnectionString, 0))
                {
                    StringBuilder sql = new StringBuilder(256);
                    dal.BeginTran();
                    sql.Append(" UPDATE tReport SET DBID=@DBID, ReportName=@ReportName, Enabled=@Enabled, Remark =@Remark,SqlCommand=@SqlCommand WHERE ID=@ID");
                    int i;
                    //更新主表
                    dal.Execute(sql.ToString(), out i,
                        dal.CreateParameter("@DBID", report.DBID),
                        dal.CreateParameter("@ReportName", report.ReportName),
                        dal.CreateParameter("@Enabled", report.Enabled),
                        dal.CreateParameter("@Remark", report.Remark),
                        dal.CreateParameter("@ID", id),
                        dal.CreateParameter("@SqlCommand",report.SqlCommand)
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
                    //清除Command
                    sql.Clear();
                    sql.Append("DELETE FROM tReportCommand WHERE ReportID=@ReportID");
                    dal.Execute(sql.ToString(), out i,
                        dal.CreateParameter("@ReportID", id)
                        );
                    //清除Result
                    sql.Clear();
                    sql.Append("DELETE FROM tReportResult WHERE ReportID=@ReportID");
                    dal.Execute(sql.ToString(), out i,
                        dal.CreateParameter("@ReportID", id)
                        );

                    //保存字段
                    foreach (ReportColumn c in report.Columns)
                    {
                        sql.Clear();
                        sql.Append("INSERT INTO tReportColumn( ReportID , ColumnCode , ColumnName ,Sumabled  ,Sortabled) ");
                        sql.Append("VALUES( ");
                        sql.Append(" @ReportID ,@ColumnCode ,@ColumnName ,@Sumabled ,@Sortabled ");
                        sql.Append(" ) ");

                        dal.Execute(sql.ToString(), out i,
                            dal.CreateParameter("@ReportID", id),
                            dal.CreateParameter("@ColumnCode", c.ColumnCode),
                            dal.CreateParameter("@ColumnName", c.ColumnName),
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
                            dal.CreateParameter("@ParamName", p.ParamName),
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
                                dal.CreateParameter("@OptionName", op.OptionName),
                                dal.CreateParameter("@OptionValue", op.OptionValue)
                                );
                            if (i != 1)
                            {
                                dal.RollBackTran();
                                throw new Exception("参数列表选项" + op.OptionName + "插入失败");
                            }
                        }

                    }
                    //保存报表结果
                    sql.Clear();
                    sql.Append(" INSERT INTO tReportResult( ReportID ,AllSumabled ,PageSumabled ,Pagingabled ,PageSize) ");
                    sql.Append(" VALUES (");
                    sql.Append("  @ReportID ,@AllSumabled ,@PageSumabled ,@Pagingabled ,@PageSize )");
                    dal.Execute(sql.ToString(), out i,
                        dal.CreateParameter("@ReportID", id),
                        dal.CreateParameter("@AllSumabled", report.Result.AllSumabled),
                        dal.CreateParameter("@PageSumabled", report.Result.PageSumabled),
                        dal.CreateParameter("@Pagingabled", report.Result.Pagingabled),
                        dal.CreateParameter("@PageSize", report.Result.PageSize)
                        );
                    if (i != 1)
                    {
                        dal.RollBackTran();
                        throw new Exception("报表结果项插入失败");
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
        public static int DeleteReport(int id, out string msg)
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
                    //清除Command
                    sql.Clear();
                    sql.Append("DELETE FROM tReportCommand WHERE ReportID=@ReportID");
                    dal.Execute(sql.ToString(), out i,
                        dal.CreateParameter("@ReportID", id)
                        );
                    //清除Result
                    sql.Clear();
                    sql.Append("DELETE FROM tReportResult WHERE ReportID=@ReportID");
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

        /// <summary>
        /// 查询报表
        /// </summary>
        /// <param name="id">报表ID</param>
        /// <param name="request">请求信息</param>
        /// <returns></returns>
        public static int QueryReport(decimal id,ReportRequest request,out string result,out string msg)
        {
            result = "";
            msg = "";
            return -1;
        }

        const string REGEX_PARAMS=@"(?<=)@\D\w+";
        /// <summary>
        /// 重建报表结构
        /// </summary>
        /// <param name="id"></param>
        /// <param name="SQL"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static Report RebuildReport(decimal id, string SQL, out string msg)
        {

            Report report=new Report();
            MatchCollection pms= Regex.Matches(SQL,REGEX_PARAMS);
            report.Params=new List<ReportParam>();
            report.Columns=new List<ReportColumn>();
            foreach (Match m in pms)
            {
                //获取SQL语句中的参数
                report.Params.Add(new ReportParam()
                {
                    ReportID = id,
                    ParamCode = m.Value,
                    ParamName = m.Value,
                    ParamType = 0,
                    ParamInputType = 0,
                    ParamItems = new List<ReportParamItem>()
                });
            }
            string tmpSql=Regex.Replace(SQL,REGEX_PARAMS,"NULL");//将所有参数设置为null，获取表结构
            try
            {
                using(IDAL dal=DALBuilder.CreateDAL(ConfigurationManager.ConnectionStrings["SYSDB"].ConnectionString,0))
                {
                    //获取SQL语句中的Column
                    DataSet ds=dal.Select(tmpSql);
                    if(ds.Tables.Count!=1)
                    {
                        throw new Exception("错误：查询结果必须只有一个结果表");
                    }
                    else
                    {
                        foreach (DataColumn c in ds.Tables[0].Columns)
                        {
                            report.Columns.Add(new ReportColumn()
                            {
                                ReportID = id,
                                ColumnCode = c.ColumnName,
                                ColumnName = c.ColumnName,
                                Sumabled = false,
                                Sortabled = false
                            });
                        }
                    }
                    //获取之前设置的Params 
                    StringBuilder sql = new StringBuilder(256);
                    sql.Append("SELECT * FROM tReportParam WHERE ReportID=@ReportID");
                    dal.OpenReader(sql.ToString(),
                        dal.CreateParameter("@ReportID", id));
                    while (dal.DataReader.Read())
                    {
                        
                    }
                    
                    //获取之前设置的Column
                    msg="success";
                    return report;
                }
                
            }
            catch (System.Exception ex)
            {
            	msg=ex.Message;
                return null;
            }
        }
    }
}
