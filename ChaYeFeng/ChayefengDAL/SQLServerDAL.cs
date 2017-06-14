using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using System.Data.Common;
using System.Collections;

namespace ChaYeFeng
{
    /// <summary>
    /// sqlserver访问帮助类，可以通过配置ConnectionString来控制访问数据库
    /// </summary>
    public class DBHelper
    {
        private string ConnectionString;
        private SqlTransaction Trans = null;
        private SqlConnection Connection = null;

        /// <summary>
        /// 初始化一个操作配置中设置的数据库的实例
        /// </summary>
        public DBHelper()
            : this(DALConfig.Instance.ConnectionStr)
        {
        }

        /// <summary>
        /// 初始化一个操作自定义的数据库的实例
        /// </summary>
        /// <param name="connString"></param>
        public DBHelper(string connString)
        {
            this.ConnectionString = connString;
        }

        /// <summary>
        /// 与另一个操作实例共享连接和事务
        /// </summary>
        /// <param name="trans"></param>
        public DBHelper(SqlTransaction trans)//事务可以共用
        {
            this.Trans = trans;
            this.Connection = trans.Connection;
            this.ConnectionString = trans.Connection.ConnectionString;
        }

        /// <summary>
        /// 执行SQL返回影响行数
        /// </summary>
        /// <param name="strSQL"></param>
        /// <returns></returns>
        public int ExecuteNonQuery(string strSQL)
        {
            try
            {
                SqlCommand cmd = new SqlCommand(strSQL);
                return this.ExecuteNonQuery(cmd);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// 执行带参数的SQL返回影响行数
        /// </summary>
        /// <param name="strSQL"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public int ExecuteNonQuery(string strSQL, List<SqlParameter> parameters)
        {
            SqlCommand cmd = new SqlCommand(strSQL);
            cmd.CommandType = CommandType.StoredProcedure;

            foreach (SqlParameter parameter in parameters)
            {
                cmd.Parameters.Add(parameter);
            }

            return ExecuteNonQuery(cmd);
        }

        /// <summary>
        /// 执行命令，返回影响行数
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        public int ExecuteNonQuery(SqlCommand cmd)//执行时共享同一个事务
        {
            try
            {
                //记录数据操作语句
                int result;
                if (this.Trans != null)
                {
                    if (this.Trans.Connection == null)
                    {
                        throw new InvalidOperationException("事务已终止，已无法再使用");
                    }

                    if (this.Connection.State == ConnectionState.Closed)
                    {
                        this.Connection.Open();
                    }
                    cmd.Connection = this.Connection;
                    cmd.Transaction = this.Trans;
                    CYFLog.WriteLog(CYFLog.LogFile.SQL, cmd.CommandText);
                    result = cmd.ExecuteNonQuery();
                }
                else
                {
                    using (SqlConnection sqlcon = new SqlConnection(this.ConnectionString))
                    {
                        if (sqlcon.State == ConnectionState.Closed)
                        {
                            sqlcon.Open();
                        }

                        cmd.Connection = sqlcon;
                        CYFLog.WriteLog(CYFLog.LogFile.SQL, cmd.CommandText);
                        result = cmd.ExecuteNonQuery();
                    }
                }
                return result;

            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// 执行SQL并返回第一行第一个值
        /// </summary>
        /// <param name="strSQL"></param>
        /// <returns></returns>
        public object ExecuteScalar(string strSQL)
        {
            try
            {
                SqlCommand cmd = new SqlCommand(strSQL);
                return this.ExecuteScalar(cmd);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// 执行SQL返回DataSet
        /// </summary>
        /// <param name="strSQL"></param>
        /// <returns></returns>
        public DataSet ExecuteSql(string strSQL)
        {
            try
            {
                SqlCommand cmd = new SqlCommand(strSQL);
                return this.ExecuteSql(cmd);
            }
            catch
            {
                throw;
            }

        }

        /// <summary>
        /// 执行命令返回DataSet
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        private DataSet ExecuteSql(SqlCommand cmd)
        {
            try
            {
                DataSet ds = new DataSet();
                //记录数据操作语句
                //new Logger.clsLogText().Log2File(string.Format("{0} {1} \r\n{2}", DateTime.Now.ToString(), "ExecuteSql", cmd.CommandText));
                if (this.Trans != null)
                {
                    if (this.Trans.Connection == null)
                    {
                        throw new InvalidOperationException("事务已终止，已无法再使用");
                    }

                    if (this.Connection.State == ConnectionState.Closed)
                    {
                        this.Connection.Open();
                    }
                    cmd.Connection = this.Connection;
                    cmd.Transaction = this.Trans;
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter da = new SqlDataAdapter();
                    da.SelectCommand = cmd;
                    CYFLog.WriteLog(CYFLog.LogFile.SQL, cmd.CommandText);
                    da.Fill(ds);
                }
                else
                {
                    using (SqlConnection sqlcon = new SqlConnection(this.ConnectionString))
                    {
                        if (sqlcon.State == ConnectionState.Closed)
                        {
                            sqlcon.Open();
                        }

                        cmd.Connection = sqlcon;
                        cmd.CommandType = CommandType.StoredProcedure;
                        SqlDataAdapter da = new SqlDataAdapter();
                        da.SelectCommand = cmd;
                        CYFLog.WriteLog(CYFLog.LogFile.SQL, cmd.CommandText);
                        da.Fill(ds);
                    }
                }
                return ds;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// 执行命令返回第一行第一个值
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        public object ExecuteScalar(SqlCommand cmd)
        {
            try
            {
                //记录数据操作语句
                //new Logger.clsLogText().Log2File(string.Format("{0} {1} \r\n{2}", DateTime.Now.ToString(), "ExecuteScalar", cmd.CommandText));
                object obj = null;
                if (this.Trans != null)
                {
                    if (this.Trans.Connection == null)
                    {
                        throw new InvalidOperationException("事务已终止，已无法再使用");
                    }

                    if (this.Connection.State == ConnectionState.Closed)
                    {
                        this.Connection.Open();
                    }
                    cmd.Connection = this.Connection;
                    cmd.Transaction = this.Trans;
                    CYFLog.WriteLog(CYFLog.LogFile.SQL, cmd.CommandText);
                    obj = cmd.ExecuteScalar();
                }
                else
                {
                    using (SqlConnection sqlcon = new SqlConnection(this.ConnectionString))
                    {
                        if (sqlcon.State == ConnectionState.Closed)
                        {
                            sqlcon.Open();
                        }

                        cmd.Connection = sqlcon;
                        CYFLog.WriteLog(CYFLog.LogFile.SQL, cmd.CommandText);
                        obj = cmd.ExecuteScalar();
                    }
                }
                return obj;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// 执行SQL返回DataTable
        /// </summary>
        /// <param name="strSQL"></param>
        /// <returns></returns>
        public System.Data.DataTable GetTable(string strSQL)
        {
            try
            {
                SqlCommand cmd = new SqlCommand(strSQL);
                return GetTable(cmd);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// 执行命令返回DataTable
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        public System.Data.DataTable GetTable(System.Data.SqlClient.SqlCommand cmd)
        {
            try
            {
                //记录数据操作语句
                //new Logger.clsLogText().Log2File(string.Format("{0} {1} \r\n{2}", DateTime.Now.ToString(), "GetTable", cmd.CommandText));
                DataTable dt = new DataTable();
                if (this.Trans != null)
                {
                    if (this.Trans.Connection == null)
                    {
                        throw new InvalidOperationException("事务已终止，已无法再使用");
                    }

                    if (this.Connection.State == ConnectionState.Closed)
                    {
                        this.Connection.Open();
                    }
                    cmd.Connection = this.Connection;
                    cmd.Transaction = this.Trans;
                    SqlDataAdapter da = new SqlDataAdapter();
                    da.SelectCommand = cmd;
                    CYFLog.WriteLog(CYFLog.LogFile.SQL, cmd.CommandText);
                    da.Fill(dt);
                }
                else
                {

                    using (SqlConnection sqlcon = new SqlConnection(this.ConnectionString))
                    {
                        if (sqlcon.State == ConnectionState.Closed)
                        {
                            sqlcon.Open();
                        }

                        cmd.Connection = sqlcon;
                        SqlDataAdapter da = new SqlDataAdapter();
                        da.SelectCommand = cmd;
                        CYFLog.WriteLog(CYFLog.LogFile.SQL, cmd.CommandText);
                        da.Fill(dt);
                    }
                }

                return dt;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// 执行SQL返回DataSet
        /// </summary>
        /// <param name="strSQL"></param>
        /// <returns></returns>
        public System.Data.DataSet GetDataSet(string strSQL)
        {
            try
            {
                SqlCommand cmd = new SqlCommand(strSQL);
                return GetDataSet(cmd);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// 执行命令返回DataSet
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        public System.Data.DataSet GetDataSet(System.Data.SqlClient.SqlCommand cmd)
        {
            try
            {
                //记录数据操作语句
                //new Logger.clsLogText().Log2File(string.Format("{0} {1} \r\n{2}", DateTime.Now.ToString(), "GetDataSet", cmd.CommandText));
                DataSet ds = new DataSet();

                if (this.Trans != null)
                {
                    if (this.Trans.Connection == null)
                    {
                        throw new InvalidOperationException("事务已终止，已无法再使用");
                    }

                    if (this.Connection.State == ConnectionState.Closed)
                    {
                        this.Connection.Open();
                    }
                    cmd.Connection = this.Connection;
                    cmd.Transaction = this.Trans;
                    SqlDataAdapter da = new SqlDataAdapter();
                    da.SelectCommand = cmd;
                    CYFLog.WriteLog(CYFLog.LogFile.SQL, cmd.CommandText);
                    da.Fill(ds);
                }
                else
                {
                    using (SqlConnection sqlcon = new SqlConnection(this.ConnectionString))
                    {
                        if (sqlcon.State == ConnectionState.Closed)
                        {
                            sqlcon.Open();
                        }

                        cmd.Connection = sqlcon;
                        SqlDataAdapter da = new SqlDataAdapter();
                        da.SelectCommand = cmd;
                        CYFLog.WriteLog(CYFLog.LogFile.SQL, cmd.CommandText);
                        da.Fill(ds);
                    }
                }
                return ds;
            }
            catch
            {
                throw;
            }
        }

        #region 事务相关
        /// <summary>
        /// 初始化一个带操作配置中数据库事务的操作实例
        /// </summary>
        /// <returns></returns>
        public static DBHelper BeginTransaction()
        {
            return BeginTransaction(DALConfig.Instance.ConnectionStr);
        }

        /// <summary>
        /// 初始化一个自定义数据库事务的操作实例
        /// </summary>
        /// <param name="connString"></param>
        /// <returns></returns>
        public static DBHelper BeginTransaction(string connString)
        {
            SqlConnection conn = new SqlConnection(connString);
            conn.Open();
            SqlTransaction trans = conn.BeginTransaction();
            DBHelper helper = new DBHelper(trans);
            return helper;
        }

        /// <summary>
        /// 提交事务
        /// </summary>
        public void Commit()
        {
            if (this.Trans == null)
            {
                throw new NotSupportedException("事务未启用");
            }
            this.Trans.Commit();
        }

        /// <summary>
        /// 回滚事务
        /// </summary>
        public void RollBack()
        {
            if (this.Trans == null)
            {
                throw new NotSupportedException("事务未启用");
            }
            this.Trans.Rollback();
        }
        #endregion

        /// <summary>
        /// 关闭连接
        /// </summary>
        public void Close()
        {
            if (this.Connection != null)
                this.Connection.Close();
        }

        #region IDisposable 成员
        /// <summary>
        /// 释放事务和连接
        /// </summary>
        public void Dispose()
        {
            if (this.Trans != null)
                this.Trans.Dispose();

            if (this.Connection != null)
                this.Connection.Dispose();
        }

        #endregion
    }
}
