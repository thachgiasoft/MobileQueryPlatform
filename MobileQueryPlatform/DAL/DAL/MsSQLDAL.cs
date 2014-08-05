using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Collections;
using DAL.Interface;

namespace DAL
{
    internal class MsSQLDAL:IDisposable,IDAL
    {
        #region 构造函数
        /// <summary>
        /// 构造函数
        /// create by xuehuibo 2014-03-21
        /// </summary>
        /// <param name="connectionString">数据库连接参数</param>
        public MsSQLDAL(string connectionString)
        {
            Connection = new SqlConnection(connectionString);
            try
            {
                Connection.Open();
            }
            catch
            {
                throw;
            }
        }
        #endregion

        #region 私有变量
        /// <summary>
        /// 数据库连接
        /// </summary>
        private SqlConnection Connection
        {
            get;
            set;
        }

        /// <summary>
        /// 事务
        /// </summary>
        private SqlTransaction Tran
        {
            get;
            set;
        }

        #endregion

        #region Query

        /// <summary>
        /// 查询
        /// create by xuehuibo 2014-03-21
        /// </summary>
        /// <param name="sqlString">sql语句</param>
        /// <param name="paramCollection">参数集合</param>
        /// <returns></returns>
        public DataTable Select(string sqlString,out int i, params IDbDataParameter[] paramArray)
        {
            try
            {
                SqlCommand cmd = new SqlCommand(sqlString, Connection);
                cmd.Transaction = Tran;
                if (paramArray != null && paramArray.Length > 0)
                {
                    cmd.Parameters.AddRange(paramArray);
                }
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dataTable = new DataTable();
                i=adapter.Fill(dataTable);
                return dataTable;
            }
            catch
            {
                throw;
            }
        }

        public DataTable Select(string sqlString,int startRecord,int maxRecords, out int i, params IDbDataParameter[] paramArray)
        {
            try
            {
                SqlCommand cmd = new SqlCommand(sqlString, Connection);
                cmd.Transaction = Tran;
                if (paramArray != null && paramArray.Length > 0)
                {
                    cmd.Parameters.AddRange(paramArray);
                }
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dataTable = new DataTable();
                i = adapter.Fill(startRecord,maxRecords, dataTable);
                return dataTable;
            }
            catch
            {
                throw;
            }
        }

        public DataSet Select(string sqlString, params IDbDataParameter[] paramArray)
        {
            try
            {
                SqlCommand cmd = new SqlCommand(sqlString, Connection);
                cmd.Transaction = Tran;
                if (paramArray != null && paramArray.Length > 0)
                {
                    cmd.Parameters.AddRange(paramArray);
                }
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                return ds;
            }
            catch
            {
                throw;
            }
        }

        public bool OpenReader(string sqlString, params IDbDataParameter[] paramArray)
        {
            try
            {
                if (DataReader != null && !DataReader.IsClosed)
                {
                    //如果reader未关闭，则抛异常
                    throw new Exception("DataReader未关闭");
                }
                SqlCommand cmd = new SqlCommand(sqlString, Connection);
                cmd.Transaction = Tran;
                if (paramArray != null && paramArray.Length > 0)
                {
                    cmd.Parameters.AddRange(paramArray);
                }
                DataReader=cmd.ExecuteReader();
                return true;
            }
            catch
            {
                throw;
            }
        }
        #endregion

        #region Procedure

        /// <summary>
        /// 执行存储过程
        /// create by xuehuibo 2014-03-21
        /// </summary>
        /// <param name="procedureName">存储过程名称</param>
        /// <param name="paramArray">存储过程参数</param>
        /// <returns></returns>
        public DataSet RunProcedure(string procedureName, params IDbDataParameter[] paramArray)
        {
            try
            {
                DataSet ds = new DataSet();
                SqlTransaction tran = Tran;
                SqlCommand cmd = new SqlCommand(procedureName, Connection);
                if (paramArray != null && paramArray.Length > 0)
                {
                    cmd.Parameters.AddRange(paramArray);
                }
                cmd.Transaction = tran;
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                sda.Fill(ds);
                return ds;
            }
            catch
            {
                throw;
            }
        }

        #endregion

        #region Insert
        public bool Save(DataTable dt, out int i)
        {
            try
            {
                SqlBulkCopy sbc = new SqlBulkCopy(Connection,SqlBulkCopyOptions.Default,Tran);
                sbc.DestinationTableName=dt.TableName;
                sbc.WriteToServer(dt);
                i = dt.Rows.Count;
                sbc.Close();
                return true;
            }
            catch
            {
                throw;
            }
        }
        #endregion

        #region Execute
        public void Execute(string sqlString, out int i, params IDbDataParameter[] paramArray)
        {
            try
            {
                SqlCommand cmd = new SqlCommand(sqlString, Connection);
                cmd.Transaction = Tran;
                if (paramArray != null && paramArray.Length > 0)
                {
                    cmd.Parameters.AddRange(paramArray);
                }
                i = cmd.ExecuteNonQuery();
            }
            catch
            {
                throw;
            }
        }
        #endregion

        #region IDisposable 成员

        public void Dispose()
        {
            if (DataReader != null && !DataReader.IsClosed)
            {
                DataReader.Close();
            }
            //关闭连接并释放
            Connection.Close();
            Connection = null;
        }

        #endregion

        #region Transcation

        public void BeginTran()
        {
            try
            {
                Tran = Connection.BeginTransaction();
            }
            catch 
            {
                throw;
            }
        }

        public void CommitTran()
        {
            try
            {
                Tran.Commit();
            }
            catch
            {
                try
                {
                    Tran.Rollback();
                }
                catch
                {

                }
                throw;
            }
        }

        public void RollBackTran()
        {
            try
            {
                Tran.Rollback();
            }
            catch
            {
                throw;
            }
        }
        #endregion

        #region ParameterCreater

        public IDbDataParameter CreateParameter(string paramName, object value)
        {
            if (paramName[0] == ':')
            {
                paramName = "@" + paramName.Substring(1, paramName.Length - 1);//如果变量以:开头，替换为@
            }
            if (paramName[0] != '@')
            {
                paramName = "@" + paramName;
            }
            return new SqlParameter(paramName, value);
        }

        public IDbDataParameter CreateParameter(string paramName, DbType dbType)
        {
            if (paramName[0] != '@')
            {
                paramName = "@" + paramName;
            }

            return new SqlParameter()
            {
                ParameterName=paramName,
                DbType=dbType
            };
        }

        public IDbDataParameter CreateParameter(string paramName, DbType dbType, int size)
        {
            if (paramName[0] != '@')
            {
                paramName = "@" + paramName;
            }

            return new SqlParameter()
            {
                ParameterName=paramName,
                DbType=dbType,
                Size=size
            };
        }

        public IDbDataParameter CreateParameter(string paramName, DbType dbType, ParameterDirection direction)
        {
            if (paramName[0] != '@')
            {
                paramName = "@" + paramName;
            }

            return new SqlParameter()
            {
                ParameterName=paramName,
                DbType=dbType,
                Direction=direction
            };
        }

        public IDbDataParameter CreateParameter(string paramName, DbType dbType, int size, ParameterDirection direction)
        {
            if (paramName[0] != '@')
            {
                paramName = "@" + paramName;
            }

            return new SqlParameter()
            {
                ParameterName = paramName,
                DbType = dbType,
                Direction = direction,
                Size=size
            };
        }

        /// <summary>
        /// 复制参数
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public IDbDataParameter CloneParameter(IDbDataParameter param)
        {
            return new SqlParameter()
            {
                ParameterName = param.ParameterName,
                DbType = param.DbType,
                Direction = param.Direction,
                Size = param.Size,
                Value=param.Value
            };
        }
        #endregion


        public IDataReader DataReader
        {
            get;
            set;
        }
    }
}
