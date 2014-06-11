using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.OracleClient;
using System.Data;

namespace DAL
{
    /// <summary>
    /// Oracle操作类
    /// </summary>
    public class OracleDAL:IDisposable
    {
        /// <summary>
        /// 数据库连接
        /// </summary>
        public OracleConnection Connection
        {
            get;
            set;
        }

        /// <summary>
        /// 构造函数
        /// create by xuehuibo 2014-03-21
        /// </summary>
        /// <param name="connectionString">数据库连接参数</param>
        public OracleDAL(string connectionString)
        {
            Connection = new OracleConnection(connectionString);
        }

        public void Open()
        {
            try
            {
                Connection.Open();
            }
            catch 
            {
                //数据库连接失败，抛出异常
                throw;
            }
        }

        #region Query

        /// <summary>
        /// 查询
        /// create by xuehuibo 2014-03-21
        /// </summary>
        /// <param name="sqlString">sql语句</param>
        /// <param name="paramCollection">参数集合</param>
        /// <returns></returns>
        public DataTable Select(string sqlString,out int i, params OracleParameter[] paramArray)
        {
            try
            {
                //OracleTransaction tran = Connection.BeginTransaction();
                OracleCommand cmd = new OracleCommand(sqlString, Connection);
                cmd.Parameters.AddRange(paramArray);
                //cmd.Transaction = tran;
                OracleDataAdapter adapter = new OracleDataAdapter(cmd);
                DataTable dataTable = new DataTable();
                i=adapter.Fill(dataTable);
                //tran.Commit();
                return dataTable;
            }
            catch
            {
                throw;
            }
        }

        public DataTable Select(string sqlString, OracleTransaction tran,out int i,params OracleParameter[] paramArray)
        {
            try
            {
                OracleCommand cmd = new OracleCommand(sqlString, Connection);
                cmd.Parameters.AddRange(paramArray);
                cmd.Transaction = tran;
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

        public OracleDataReader Select(string sqlString, params OracleParameter[] paramArray)
        {
            try
            {
                OracleCommand cmd = new OracleCommand(sqlString, Connection);
                cmd.Parameters.AddRange(paramArray);
                return cmd.ExecuteReader();
            }
            catch
            {
                throw;
            }
        }

        public OracleDataReader Select(string sqlString, OracleTransaction tran, params OracleParameter[] paramArray)
        {
            try
            {
                OracleCommand cmd = new OracleCommand(sqlString, Connection);
                cmd.Transaction = tran;
                cmd.Parameters.AddRange(paramArray);
                return cmd.ExecuteReader();
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
        /// <param name="outCursorParameters">输出的游标参数</param>
        /// <returns></returns>
        public DataSet RunProcedure(string procedureName, params OracleParameter[] paramArray)
        {
            try
            {
                DataSet ds = new DataSet();
                OracleTransaction tran = Connection.BeginTransaction();
                OracleCommand cmd = new OracleCommand(procedureName, Connection);
                cmd.Parameters.AddRange(paramArray);
                cmd.Transaction = tran;
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
                            for (int i = 0; i < reader.FieldCount;i++ )
                            {
                                row[i] = reader[i];
                            }
                            ds.Tables[p.ParameterName].Rows.Add(row);
                        }
                        reader.Close();
                    }
                }
                tran.Commit();
                return ds;
            }
            catch
            {
                throw;
            }
        }

        #endregion

        public bool Save(DataTable dt, OracleTransaction tran, out int i)
        {
            try
            {
                OracleDataAdapter oda = new OracleDataAdapter();
                StringBuilder selectSql = new StringBuilder(256);
                selectSql.AppendFormat(" Select * From {0} Where 1!=1 ", dt.TableName);
                OracleCommand selectCommand = new OracleCommand(selectSql.ToString(), Connection);
                selectCommand.Transaction = tran;
                oda.SelectCommand = selectCommand;
                OracleCommandBuilder ocb=new OracleCommandBuilder(oda);
                oda.InsertCommand = ocb.GetInsertCommand();
                oda.UpdateCommand = ocb.GetUpdateCommand();
                i=oda.Update(dt);
                return true;
            }
            catch
            {
                tran.Rollback();
                throw;
            }
        }

        #region 执行语句
        public bool Execute(string sqlString, out int i, params OracleParameter[] Params)
        {
            try
            {
                OracleCommand cmd = new OracleCommand(sqlString, Connection);
                cmd.Parameters.AddRange(Params);
                i = cmd.ExecuteNonQuery();
                return true;
            }
            catch
            {
                throw;
            }
        }

        public bool Execute(string sqlString, OracleTransaction tran,out int i,params OracleParameter[] Params)
        {
            try
            {
                OracleCommand cmd = new OracleCommand(sqlString, Connection);
                cmd.Transaction = tran;
                cmd.Parameters.AddRange(Params);
                i = cmd.ExecuteNonQuery();
                return true;
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
            //关闭连接并释放
            Connection.Close();
            Connection = null;
        }

        #endregion
    }
}
