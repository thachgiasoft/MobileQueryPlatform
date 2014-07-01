using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace DAL.Interface
{
    public interface IDAL:IDisposable
    {
        #region transaction
        /// <summary>
        /// 开启事务
        /// </summary>
        /// <returns></returns>
        void BeginTran();

        /// <summary>
        /// 提交事务
        /// </summary>
        /// <returns></returns>
        void CommitTran();

        /// <summary>
        /// 回滚事务
        /// </summary>
        /// <returns></returns>
        void RollBackTran();
        #endregion

        #region Query

        /// <summary>
        /// 查询
        /// create by xuehuibo 2014-03-21
        /// </summary>
        /// <param name="sqlString">sql语句</param>
        /// <param name="paramCollection">参数集合</param>
        /// <returns></returns>
        DataTable Select(string sqlString, out int i, params IDbDataParameter[] paramArray);

        IDataReader Select(string sqlString, params IDbDataParameter[] paramArray);

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
        DataSet RunProcedure(string procedureName, params IDbDataParameter[] paramArray);

        #endregion

        #region Insert
        bool Save(DataTable dt, out int i);
        #endregion

        #region execute
        bool Execute(string sqlString, out int i, params IDbDataParameter[] Params);
        #endregion

        #region ParameterCreater
        IDbDataParameter CreateParameter(string paramName,object value);
        IDbDataParameter CreateParameter(string paramName, DbType dbType);
        IDbDataParameter CreateParameter(string paramName, DbType dbType,int size);
        IDbDataParameter CreateParameter(string paramName, DbType dbType,ParameterDirection direction);
        IDbDataParameter CreateParameter(string paramName, DbType dbType,int size ,ParameterDirection direction);
        #endregion

    }
}
