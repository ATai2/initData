using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using BillBackUpcs.dal;
using log4net;
using PrintForm.model;

namespace PrintForm.dal
{
    /// <summary>
    ///  规则：
    /// 1.所有需要连接的地方，都先做检查
    /// 
    /// </summary>
    public class DbService
    {
        private ILog slog = LogManager.GetLogger(typeof(DbService));
        private ConfigModel configModel;
        private SqlConnection localConn;
        private SqlConnection remoteConn;
        private List<string> mTables;
        private List<string> mRemoteTables;

        public DbService()
        {
            string configPath = "initdata.xml";
            if (!File.Exists(configPath))
            {
                slog.Error("配置文件不存在，请联系管理员");
            }
            configModel = new ConfigModel();
            configModel.Init(configPath);
            SqlHelper.SetConnString(configModel.Local);
            RemoteSqlHelper.SetConnString(configModel.Remote);
//            localConn = SqlHelper.GetConnection();
        }

        /// <summary>
        /// 获得本地连接
        /// </summary>
        /// <param name="constr"></param>
        /// <returns></returns>
        private SqlConnection GetLocalConnection(string constr)
        {
            try
            {
                localConn = SqlHelper.GetConnection();
                localConn.Open();
                slog.Info("获得本地数据库");
            }
            catch (Exception)
            {
                slog.Error("无法得到本地连接，请检查配置");
                Environment.Exit(0);
            }
            return localConn;
        }

        /// <summary>
        /// 获得远程连接
        /// </summary>
        /// <param name="constr"></param>
        /// <returns></returns>
        private SqlConnection GetRemoteConnection(string constr)
        {
            try
            {
                remoteConn = RemoteSqlHelper.GetConnection();
                remoteConn.Open();
                slog.Info("获得远程数据库");
            }
            catch (Exception)
            {
                slog.Error("无法得到远程连接，请检查配置");
                Environment.Exit(0);
            }
            return remoteConn;
        }

        /// <summary>
        /// 校验本地连接
        /// </summary>
        private void CheckLocalConn()
        {
            if (localConn == null)
            {
                GetLocalConnection(configModel.Local);
            }
        }

        /// <summary>
        /// 校验远程连接
        /// </summary>
        private void CheckRemotConn()
        {
            if (remoteConn == null)
            {
                GetRemoteConnection(configModel.Remote);
            }
        }



        /// <summary>
        /// 远程数据库表
        /// </summary>
        public List<string> GetListFromRemoteControllTable()
        {
            CheckRemotConn();
            var sql = "SELECT Name FROM SysObjects Where XType='U' ORDER BY Name";
            mRemoteTables = new List<string>();
//            SqlCommand cmd = new SqlCommand(sql, remoteConn);
            try
            {
                var dr = RemoteSqlHelper.ExecuteReader(RemoteSqlHelper.GetConnection(), CommandType.Text, sql);
                while (dr.Read())
                {
                    mRemoteTables.Add(dr["Name"].ToString());
                }
                dr.Close();
            }
            catch (Exception)
            {
                slog.Error("无法从远程数据库中获得信息！");
            }
            return mRemoteTables;
        }

        public DataTable GetTable(string sql)
        {
            var ds=RemoteSqlHelper.ExecuteDataset(RemoteSqlHelper.GetConnection(),
                CommandType.Text,
                sql);
                return ds?.Tables[0];
        }
   
     
    }
}