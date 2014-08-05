using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.OracleClient;
using System.Data;
using DAL.Interface;

namespace DAL
{
    /// <summary>
    /// Oracle操作类
    /// </summary>
    internal class OracleDAL:IDisposable,IDAL
    {
        #region 构造函数
        /// <summary>
        /// 构造函数
        /// create by xuehuibo 2014-03-21
        /// </summary>
        /// <param name="connectionString">数据库连接参数</param>
        public OracleDAL(string connectionString)
        {
            Connection = new OracleConnection(connectionString);
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
        private OracleConnection Connection
        {
            get;
            set;
        }

        /// <summary>
        /// 事务
        /// </summary>
        private OracleTransaction Tran
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
        /// <param name="OracleString">Oracle语句</param>
        /// <param name="paramCollection">参数集合</param>
        /// <returns></returns>
        public DataTable Select(string OracleString,out int i, params IDbDataParameter[] paramArray)
        {
            try
            {
                OracleCommand cmd = new OracleCommand(OracleString, Connection);
                cmd.Transaction = Tran;
                if (paramArray != null && paramArray.Length > 0)
                {
                    cmd.Parameters.AddRange(paramArray);
                }
                OracleDataAdapter adapter = new OracleDataAdapter(cmd);
                DataTable dataTable = new DataTable();
                i=adapter.Fill(dataTable);
                return dataTable;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// 查询
        /// create by xuehuibo 2014-03-21
        /// </summary>
        /// <param name="OracleString">Oracle语句</param>
        /// <param name="paramCollection">参数集合</param>
        /// <returns></returns>
        public DataTable Select(string OracleString,int startRecord,int maxRecords, out int i, params IDbDataParameter[] paramArray)
        {
            try
            {
                OracleCommand cmd = new OracleCommand(OracleString, Connection);
                cmd.Transaction = Tran;
                if (paramArray != null && paramArray.Length > 0)
                {
                    cmd.Parameters.AddRange(paramArray);
                }
                OracleDataAdapter adapter = new OracleDataAdapter(cmd);
                DataTable dataTable = new DataTable();
                i = adapter.Fill(startRecord,maxRecords,dataTable);
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
                OracleCommand cmd = new OracleCommand(sqlString, Connection);
                cmd.Transaction = Tran;
                if (paramArray != null && paramArray.Length > 0)
                {
                    cmd.Parameters.AddRange(paramArray);
                }
                OracleDataAdapter adapter = new OracleDataAdapter(cmd);
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                return ds;
            }
            catch
            {
                throw;
            }
        }

        public bool OpenReader(string OracleString, params IDbDataParameter[] paramArray)
        {
            try
            {
                if (DataReader != null && !DataReader.IsClosed)
                {
                    //如果reader未关闭，则抛异常
                    throw new Exception("DataReader未关闭");
                }
                OracleCommand cmd = new OracleCommand(OracleString, Connection);
                cmd.Transaction = Tran;
                if (paramArray != null && paramArray.Length > 0)
                {
                    cmd.Parameters.AddRange(paramArray);
                }
                DataReader= cmd.ExecuteReader();
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
                OracleCommand cmd = new OracleCommand(procedureName, Connection);
                if (paramArray != null && paramArray.Length > 0)
                {
                    cmd.Parameters.AddRange(paramArray);
                }
                cmd.Transaction = Tran;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.ExecuteNonQuery();
                foreach (OracleParameter p in paramArray)
                {
                    if (p.OracleType == OracleType.Cursor)
                    {
                        ds.Tables.Add(p.ParameterName);//添加表
                        //游标类型
                        OracleDataReader reader = (OracleDataReader)p.Value;
                        //创建表结构
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            DataColumn col = new DataColumn();
                            col.ColumnName = reader.GetName(i);
                            col.DataType = reader.GetFieldType(i);
                            ds.Tables[p.ParameterName].Columns.Add(col);
                        }
                        //填充数据
                        while (reader.Read())
                        {
                            DataRow row = ds.Tables[p.ParameterName].NewRow();
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                row[i] = reader[i];
                            }
                            ds.Tables[p.ParameterName].Rows.Add(row);
                        }
                        reader.Close();
                    }
                }
                return ds;
            }
            catch
            {
                throw;
            }
        }

        #endregion

        #region Insert
        public bool Save(DataTable dt,  out int i)
        {
            try
            {
                OracleDataAdapter oda = new OracleDataAdapter();
                StringBuilder selectOracle = new StringBuilder(256);
                selectOracle.AppendFormat(" Select * From {0} Where 1!=1 ", dt.TableName);
                OracleCommand selectCommand = new OracleCommand(selectOracle.ToString(), Connection);
                selectCommand.Transaction = Tran;
                oda.SelectCommand = selectCommand;
                OracleCommandBuilder ocb=new OracleCommandBuilder(oda);
                oda.InsertCommand = ocb.GetInsertCommand();
                oda.UpdateCommand = ocb.GetUpdateCommand();
                i=oda.Update(dt);
                return true;
            }
            catch
            {
                throw;
            }
        }
        #endregion

        #region Execute
        public void Execute(string OracleString, out int i, params IDbDataParameter[] paramArray)
        {
            try
            {
                OracleCommand cmd = new OracleCommand(OracleString, Connection);
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
            if (paramName[0] == '@')
            {
                paramName = ":" + paramName.Substring(1, paramName.Length - 1);//如果变量以@开头，替换为：
            }
            if (paramName[0] != ':')
            {
                paramName = ":" + paramName;//如果变量不以：开头，则添加：
            }
            return new OracleParameter(paramName, value);
        }

        public IDbDataParameter CreateParameter(string paramName, DbType dbType)
        {
            if (paramName[0] != ':')
            {
                paramName = ":" + paramName;
            }
            return new OracleParameter()
            {
                ParameterName = paramName,
                DbType = dbType
            };
        }

        public IDbDataParameter CreateParameter(string paramName, DbType dbType, int size)
        {
            if (paramName[0] != ':')
            {
                paramName = ":" + paramName;
            }
            return new OracleParameter()
            {
                ParameterName = paramName,
                DbType = dbType,
                Size = size
            };
        }

        public IDbDataParameter CreateParameter(string paramName, DbType dbType, ParameterDirection direction)
        {
            if (paramName[0] != ':')
            {
                paramName = ":" + paramName;
            }
            return new OracleParameter()
            {
                ParameterName = paramName,
                DbType = dbType,
                Direction = direction
            };
        }

        public IDbDataParameter CreateParameter(string paramName, DbType dbType, int size, ParameterDirection direction)
        {
            if (paramName[0] != ':')
            {
                paramName = ":" + paramName;
            }
            return new OracleParameter()
            {
                ParameterName = paramName,
                DbType = dbType,
                Direction = direction,
                Size = size
            };
        }

        /// <summary>
        /// 复制参数
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public IDbDataParameter CloneParameter(IDbDataParameter param)
        {
            return new OracleParameter()
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
