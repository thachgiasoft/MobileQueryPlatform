using DAL;
using DAL.Interface;
using Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Data;
using DAL.Helper;

namespace BLL
{
    public class DatabaseBLL
    {
        const string ORACLE_CONNECTIONSTRING = "Data Source=(DESCRIPTION = (ADDRESS_LIST = (ADDRESS = (PROTOCOL = TCP)(HOST = @DataSource)(PORT = 1521)))(CONNECT_DATA = (SERVICE_NAME = @DbName)));Persist Security Info=True;User ID=@USERID;Password=@Password";
        const string MSSQL_CONNECTIONSTRING = "Data Source=@DataSource;Initial Catalog=@DbName;Persist Security Info=True;User ID=@UserID;Password=@Password";
        
        /// <summary>
        /// 保存数据库
        /// </summary>
        /// <param name="db"></param>
        /// <returns></returns>
        public static int InsertDatabase(Database db,out string msg)
        {
            try
            {
                using (IDAL dal = DALBuilder.CreateDAL(ConfigurationManager.ConnectionStrings["SYSDB"].ConnectionString, 0))
                {
                    StringBuilder sql = new StringBuilder(256);
                    sql.Append(" INSERT INTO tDatabase ( DBCode ,DBType ,DataSource ,DbName ,UserID ,Password ,Remark) ");
                    sql.Append("VALUES  (");
                    sql.Append("@DbCode,@DbType,@DataSource,@DbName,@UserID,@Password,@Remark )");
                    dal.BeginTran();
                    int i;
                    dal.Execute(sql.ToString(), out i,
                        dal.CreateParameter("@DbCode", db.DbCode),
                        dal.CreateParameter("@DbType", db.DbType),
                        dal.CreateParameter("@DataSource", db.DataSource),
                        dal.CreateParameter("@DbName", db.DbName),
                        dal.CreateParameter("@UserID", db.UserID),
                        dal.CreateParameter("@Password", Des.EncryStrHex(db.Password,db.UserID)),
                        dal.CreateParameter("@Remark", db.Remark)
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
            catch (Exception ex)
            {
                msg = ex.Message;
                return -1;
            }
        }

        /// <summary>
        /// 更新数据库
        /// </summary>
        /// <param name="id"></param>
        /// <param name="db"></param>
        /// <returns></returns>
        public static int UpdateDatabase(int ID, Database db,out string msg)
        {
            try
            {
                using (IDAL dal = DALBuilder.CreateDAL(ConfigurationManager.ConnectionStrings["SYSDB"].ConnectionString, 0))
                {
                    StringBuilder sql = new StringBuilder(256);
                    sql.Append(" UPDATE tDatabase SET DbCode=@DbCode,DbType=@DbType,DataSource=@DataSource,DbName=@DbName,UserID=@UserID,Password=@Password,Remark=@Remark ");
                    sql.Append(" Where ID=@ID");
                    dal.BeginTran();
                    int i;
                    dal.Execute(sql.ToString(), out i,
                        dal.CreateParameter("@DbCode", db.DbCode),
                        dal.CreateParameter("@DbType", db.DbType),
                        dal.CreateParameter("@DataSource", db.DataSource),
                        dal.CreateParameter("@DbName", db.DbName),
                        dal.CreateParameter("@UserID", db.UserID),
                        dal.CreateParameter("@Password", Des.EncryStrHex(db.Password,db.UserID)),
                        dal.CreateParameter("@Remark", db.Remark),
                        dal.CreateParameter("@ID", db.ID)
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

        /// <summary>
        /// 删除数据库
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public static int DeleteDatabase(int ID,out string msg)
        {
            try
            {
                using (IDAL dal = DALBuilder.CreateDAL(ConfigurationManager.ConnectionStrings["SYSDB"].ConnectionString, 0))
                {
                    dal.BeginTran();
                    int i;
                    dal.Execute("DELETE FROM tDatabase WHERE ID=@ID", out i,
                        dal.CreateParameter("@ID", ID));
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
            catch (Exception ex)
            {
                msg = ex.Message;
                return -1;
            }
        }

        /// <summary>
        /// 列表数据源
        /// </summary>
        /// <returns></returns>
        public static ICollection<Database> ListDatabase()
        {
            try
            {
                using (IDAL dal = DALBuilder.CreateDAL(ConfigurationManager.ConnectionStrings["SYSDB"].ConnectionString, 0))
                {
                    dal.OpenReader("SELECT ID,DbCode,DbType,DataSource,DbName,UserID,Remark  FROM tDatabase");
                    ICollection<Database> rst = ObjectHelper.BuildObject<Database>(dal.DataReader);
                    return rst;
                }
            }
            catch 
            {
                return null;
            }
        }

        /// <summary>
        /// 获取数据库连接
        /// </summary>
        /// <returns></returns>
        public static int GetConnectionString(int ID,out string connectionString,out string msg)
        {
            try
            {
                using (IDAL dal = DALBuilder.CreateDAL(ConfigurationManager.ConnectionStrings["SYSDB"].ConnectionString, 0))
                {
                    decimal DbType;
                    string DataSource, DbName, UserID, Password;
                    dal.OpenReader("SELECT DbType, DataSource,DbName,UserID,Password FROM dbo.tDatabase WHERE ID=@ID",
                        dal.CreateParameter("@ID", ID));
                    if (dal.DataReader.Read())
                    {
                        DbType = Convert.ToDecimal(dal.DataReader["DbType"]);
                        DataSource = Convert.ToString(dal.DataReader["DataSource"]);
                        DbName = Convert.ToString(dal.DataReader["DbName"]);
                        UserID = Convert.ToString(dal.DataReader["UserID"]);
                        Password = Convert.ToString(dal.DataReader["Password"]);
                        Password = Des.DecryStrHex(Password, UserID);
                        connectionString = DbType == 0 ?
                            MSSQL_CONNECTIONSTRING :
                            ORACLE_CONNECTIONSTRING;
                        connectionString.Replace("@DataSource", DataSource);
                        connectionString.Replace("@DbName", DbName);
                        connectionString.Replace("@UserID", UserID);
                        connectionString.Replace("@Password", Password);
                        msg = "success";
                        return 1;
                    }
                    else
                    {
                        connectionString = null;
                        msg = "error";
                        return 0;
                    }

                }
            }
            catch (System.Exception ex)
            {
                msg = ex.Message;
                connectionString = null;
                return -1;
            }
        }

    }
}
