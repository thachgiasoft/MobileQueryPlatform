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
        /// 获取指定数据源明细
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public static Database GetDatabase(decimal ID)
        {
            try
            {
                using (IDAL dal = DALBuilder.CreateDAL(ConfigurationManager.ConnectionStrings["SYSDB"].ConnectionString, 0))
                {
                    StringBuilder sql = new StringBuilder(256);
                    sql.Append("SELECT * FROM dbo.tDatabase WHERE ID=@ID");
                    dal.OpenReader(sql.ToString(),
                        dal.CreateParameter("@ID", ID)
                        );
                    Database db=null;
                    if (dal.DataReader.Read())
                    {
                        db = new Database()
                        {
                            ID = Convert.ToDecimal(dal.DataReader["ID"]),
                            DbCode = Convert.ToString(dal.DataReader["DbCode"]).TrimEnd(),
                            DbType = Convert.ToInt16(dal.DataReader["DbType"]),
                            DataSource = Convert.ToString(dal.DataReader["DataSource"]).TrimEnd(),
                            DbName = Convert.ToString(dal.DataReader["DbName"]).TrimEnd(),
                            UserID = Convert.ToString(dal.DataReader["UserID"]).TrimEnd(),
                            Remark = Convert.ToString(dal.DataReader["Remark"]).TrimEnd()
                        };
                    }
                    return db;
                }
            }
            catch
            {
                return null;
            }
        }

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
        public static int UpdateDatabase(decimal ID, Database db,out string msg)
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
        public static int DeleteDatabase(decimal ID, out string msg)
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
                    dal.OpenReader("SELECT ID,DbCode,DbName,DbType,Remark  FROM tDatabase");
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
        public static int GetConnectionString(decimal ID, out string connectionString, out short dbType,out string msg)
        {
            try
            {
                using (IDAL dal = DALBuilder.CreateDAL(ConfigurationManager.ConnectionStrings["SYSDB"].ConnectionString, 0))
                {
                    string DataSource, DbName, UserID, Password;
                    dal.OpenReader("SELECT DbType, DataSource,DbName,UserID,Password FROM dbo.tDatabase WHERE ID=@ID",
                        dal.CreateParameter("@ID", ID));
                    if (dal.DataReader.Read())
                    {
                        dbType = Convert.ToInt16(dal.DataReader["DbType"]);
                        DataSource = Convert.ToString(dal.DataReader["DataSource"]).TrimEnd();
                        DbName = Convert.ToString(dal.DataReader["DbName"]).TrimEnd();
                        UserID = Convert.ToString(dal.DataReader["UserID"]).TrimEnd();
                        Password = Convert.ToString(dal.DataReader["Password"]).TrimEnd();
                        Password = Des.DecryStrHex(Password, UserID);
                        connectionString = dbType == 0 ?MSSQL_CONNECTIONSTRING :ORACLE_CONNECTIONSTRING;
                        connectionString=connectionString.Replace("@DataSource", DataSource).Replace("@DbName", DbName).Replace("@UserID", UserID).Replace("@Password", Password);
                        msg = "success";
                        return 1;
                    }
                    else
                    {
                        connectionString = null;
                        msg = "error";
                        dbType = -1;
                        return 0;
                    }

                }
            }
            catch (System.Exception ex)
            {
                msg = ex.Message;
                connectionString = null;
                dbType = -1;
                return -1;
            }
        }

    }
}
