﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Net.NetworkInformation;
using System.Text;
using BillUpConsole.controllers;
using BillUpConsole.models;
using BillUpConsole.tools;
using log4net;

namespace BillUpConsole.dal
{
    /// <summary>
    ///  规则：
    /// 1.所有需要连接的地方，都先做检查
    /// 
    /// </summary>
    public class DbSqlHelper : IOperator
    {
        private ILog slog = LogManager.GetLogger(typeof(DbSqlHelper));
        private ConfigModel configModel;
        private SqlConnection localConn;
        private SqlConnection remoteConn;
        private List<string> mTables;
        private List<string> mRemoteTables;

        public DbSqlHelper()
        {
            var str = System.AppDomain.CurrentDomain.BaseDirectory;
            string configPath = str + "initdata.xml";
//            slog.Error(configPath);
//            slog.Info(configPath);
            if (!File.Exists(configPath))
            {
                slog.Error("配置文件不存在，请联系管理员");
            }
            configModel = new ConfigModel();
            configModel.Init(configPath);
            var con = SqlHelper.GetConnection();
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
        /// 本地数据库表
        /// </summary>
        public void GetListFromLocalControllTable()
        {
            CheckLocalConn();
            var controlTable = "ty_FillTableList"; //放入配置文件中
            var sql = "select sTableName from " + controlTable;
            mTables = new List<string>();
            try
            {
                var dr = SqlHelper.ExecuteReader(localConn, CommandType.Text, sql);
                if (!dr.HasRows)
                {
                    Environment.Exit(0);
                }
                while (dr.Read())
                {
                    var name=dr["sTableName"].ToString();
                    if (!mTables.Contains(name))
                    {
                         mTables.Add(name);
                    }
                }
                dr.Close();
            }
            catch (Exception)
            {
                slog.Error("无法从控制变中获得迁移表信息！");
            }
        }

        /// <summary>
        /// 远程数据库表
        /// </summary>
        public void GetListFromRemoteControllTable()
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
                    var name = dr["sTableName"].ToString();
                    if (!mRemoteTables.Contains(name))
                    {
                        mRemoteTables.Add(dr["Name"].ToString());
                    }
                }
                dr.Close();
            }
            catch (Exception)
            {
//                slog.Error("无法从远程数据库中获得信息！");
            }
        }


        public void InitTable()
        {
            slog.Info("检查数据表一致性……");
            GetListFromLocalControllTable();
            GetListFromRemoteControllTable();
            mTables.ForEach(ValidateTable);
        }

        /// <summary>
        /// 根据表名，验证在远程数据库中是否存在，不存在，新建表；
        /// 存在校验表结构，相同；不同旧表重命名，创建新表
        /// </summary>
        /// <param name="tableName"></param>
        private void ValidateTable(string tableName)
        {
            if (!mRemoteTables.Contains(tableName))
            {
                slog.Info("正在创建差异数据表：" + tableName);
                CreatTableByName(tableName);
                //slog.Info("正在创建差异数据表：" + tableName);
            }
            else
            {
//                校验表结构是否一致
                ValidateTableStructure(tableName);
            }
        }

        /// <summary>
        /// 验证表结构是否一致  不一致重命名并新建表
        /// 暂时只算字段是否存在
        /// </summary>
        /// <param name="tableName"></param>
        private void ValidateTableStructure(string tableName)
        {
            //conntection
            var localTable = GetTableStructures(tableName, localConn);
            var remoteTable = GetTableStructures(tableName, remoteConn);
            bool needRename = false;
            List<string> localfields = new List<string>();
            List<string> remotefields = new List<string>();
            foreach (var tableStructure in remoteTable)
            {
                remotefields.Add(tableStructure.Name);
            }

            foreach (var tableStructure in localTable)
            {
                if (!remotefields.Contains(tableStructure.Name))
                {
                    needRename = true;
                    break;
                }
            }

            if (needRename)
            {
                try
                {
                    var newName = tableName + "_" + DateTime.Now.ToString("yyyy_MM_d_hh_mm_ss_ffff");
                    string sql = "EXEC sp_rename '" + tableName + "','" + newName + "'";
//                    var cmd = new SqlCommand(sql, remoteConn);
//                    cmd.ExecuteNonQuery();
                    var result = RemoteSqlHelper.ExecuteNonQuery(RemoteSqlHelper.GetConnection(), CommandType.Text, sql);
                    var insertSql = "insert into ty_FillTableList (sTableName) values('" + newName + "')";
                    RemoteSqlHelper.ExecuteNonQuery(RemoteSqlHelper.GetConnection(), CommandType.Text, insertSql);
                    slog.Info("表" + tableName + "重命名成功！");
                }
                catch (Exception)
                {
                    slog.Error("表" + tableName + "重命名失败！");
                }
                CreatTableByName(tableName);
            }
        }

        /// <summary>
        /// 根据表名创建数据表
        /// </summary>
        /// <param name="tableName"></param>
        private void CreatTableByName(string tableName)
        {
            CheckLocalConn();
            CheckRemotConn();
//            获得本地数据表结构
            var fields = GetTableStructures(tableName, localConn);
            //根据得到的表结构，生成并创建表  CREATE TABLE [dbo].[NewTable] (//[a] varchar(255) NOT NULL,//[e] int NULL,//[r] text NULL,//PRIMARY KEY([a])//)////GO
            StringBuilder sb = new StringBuilder("create table ")
                .Append(tableName + " ( ");
            string pm = "";
            for (int index = 0; index < fields.Count; index++)
            {
                var tableStructure = fields[index];
                sb.Append(tableStructure.Name);
                switch (tableStructure.SystemTypeName)
                {
                    case "bigint":
                        sb.Append(" " + tableStructure.SystemTypeName);
                        break;
                    case "varchar":
                        var value = (Convert.ToInt64(tableStructure.Maxlength)) > 0
                            ? tableStructure.Maxlength.ToString()
                            : "max";
                        sb.Append(" " + tableStructure.SystemTypeName.Trim() + "(" + value + ") ");
                        break;
                    case "int":
                        sb.Append(" " + tableStructure.SystemTypeName);
                        break;
                }
                sb.Append((!tableStructure.IsNullable) ? " NULL" : " NOT NULL");
                sb.Append(",");
                if (tableStructure.IsPk)
                {
                    pm = "PRIMARY KEY (" + tableStructure.Name + ")";
                }
            }
//            添加额外的字段   目录迁移地址；医院id；操作时间；是否打印
            sb.Append("tiffdest varchar(255) NULL, ")
                .Append("hisid varchar(128) NULL,")
                .Append("backupdate datetime default(getDate()),")
                .Append("isprint int NOT NULL  default(0),");
            sb.Append(pm);
            sb.Append(")");

//            SqlCommand creatCommand = new SqlCommand(sb.ToString(), remoteConn);
            try
            {
                var createTableResult = RemoteSqlHelper.ExecuteNonQuery(RemoteSqlHelper.GetConnection(),
                    CommandType.Text, sb.ToString());
                if (createTableResult > 0)
                {
                    slog.Info("创建表" + tableName + "成功！");
                }
            }
            catch (Exception)
            {
               // slog.Error("创建表" + tableName + "失败！");
            }
        }

        /// <summary>
        /// 根据表名获得表结构
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="conn">数据库连接</param>
        /// <returns></returns>
        private List<TableStructure> GetTableStructures(string tableName, SqlConnection conn)
        {
            string sql =
                "SELECT  sys.columns.name,   \n" +
                "(SELECT name from sys.types where sys.types.system_type_id=sys.columns .system_type_id) as system_type_name,  \n" +
                " sys.columns.max_length,   \n" +
                "sys.columns.is_nullable ,  \n" +
                "  'is_pk'=     \n" +
                "(select --c.id, c.name  ,k.colid ,k.keyno    \n" +
                "case when count(*)> 0 then 'true' else 'false' end  \n" +
                " from sysindexes i  \n" +
                " join sysindexkeys k on i.id = k.id and i.indid = k.indid  \n" +
                " join sysobjects o on i.id = o.id  \n" +
                " join syscolumns c on i.id=c.id and k.colid = c.colid  \n" +
                " where o.xtype = 'U'  \n" +
                " and exists(select 1 from sysobjects where  xtype = 'PK'  and name = i.name)  \n" +
                "and c.name=sys.columns  .name and c.id=sys.columns  .object_id   \n" +
                ")  \n" +
                "from  sys.tables ,sys.columns    \n" +
                "where  sys.tables.name='" + tableName + "' and  sys.tables.object_id =sys.columns .object_id   ";


//                " SELECT  sys.columns.name,  (SELECT name from sys.types where sys.types.system_type_id = sys.columns.system_type_id) as system_type_name,sys.columns.max_length,   sys.columns.is_nullable ,  'is_pk' =(select--c.id, c.name  ,k.colid ,k.keyno case when count(*) > 0 then 'true' else 'false' end from sysindexes i join sysindexkeys k on i.id = k.id and i.indid = k.indid join sysobjects o on i.id = o.id join syscolumns c on i.id = c.id and k.colid = c.colid where o.xtype = 'U' and exists(select 1 from sysobjects where xtype = 'PK'  and name = i.name) and c.name = sys.columns.name and c.id = sys.columns.object_id )  from sys.tables ,sys.columns where  sys.tables.name = '" +
//                tableName + "' and sys.tables.object_id = sys.columns.object_id ";

            var cmd = new SqlCommand(sql, conn);
            var fields = new List<TableStructure>();
            try
            {
                var rd = cmd.ExecuteReader();
                while (rd.Read())
                {
                    var ts = new TableStructure
                    {
                        Name = rd[0].ToString(),
                        SystemTypeName = rd[1].ToString(),
                        Maxlength = int.Parse(rd[2].ToString()),
                        IsNullable = bool.Parse(rd[4].ToString()),
                        IsPk = bool.Parse(rd[4].ToString())
                    };
                    fields.Add(ts);
                }
                rd.Close();
            }
            catch (Exception)
            {
            }
            return fields;
        }

        /// <summary>
        /// 获得可查询到的tiff文件
        /// </summary>
        /// <returns></returns>
        private List<TiffBean> GetTiffFiles()
        {
            string targetDir = "";
            string sql = "select sVoucherKey, sVoucherNo, sFilePathName from ty_VoucherFile";
            var list = new List<TiffBean>();
            try
            {
//                var rd = new SqlCommand(sql, localConn).ExecuteReader();
                var rd = SqlHelper.ExecuteReader(SqlHelper.GetConnection(), CommandType.Text, sql);
                while (rd.Read())
                {
                    var bean = new TiffBean
                    {
                        SVoucherKey = rd["sVoucherKey"].ToString(),
                        SUoucherNo = rd["sVoucherNo"].ToString(),
                        SFilePahtName = rd["sFilePathName"].ToString(),
                        Destination = targetDir + "\\" + DateTime.Now.Date +
                                      rd["sFilePathName"].ToString().Substring(2)
                    };
                    list.Add(bean);
                }
            }
            catch (Exception)
            {
                slog.Error("查询tiff文件出错");
            }
            return list;
        }

        /// <summary>
        /// 移动文件， 后期更改为ftp
        /// </summary>
        /// <param name="src">源目录</param>
        /// <param name="des">目标目录</param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        private bool MoveTiffFile(string src, string des, string tableName)
        {
            if (String.IsNullOrEmpty(src.Trim()) && String.IsNullOrEmpty(des.Trim()))
            {
                return false;
            }
            if (!File.Exists(src))
            {
                slog.Error("文件：" + src + "不存在");
                return false;
            }
            if (File.Exists(des))
            {
                File.Delete(des);
            }
            try
            {
                var f = new FileInfo(des);
                var dir = f.DirectoryName;
                if (!Directory.Exists(dir))
                {
                    if (dir != null)
                    {
                        var sdf = Directory.CreateDirectory(dir);
                    }
                }
                File.Move(src, des);
                slog.Debug("文件：" + src + "移动成功");
                return true;
            }
            catch (Exception exception)
            {
                slog.Error(exception.Message);
                slog.Error("表：" + tableName + "    文件：" + src + "移动失败");
                return false;
            }
        }

        private void CopyTableData(string tableName)
        {
            slog.Info("进行表数据复制");
            slog.Info("表名：" + tableName);
            slog.Info("-----------------------------------------");
            //获得表结构
            var tablestruct = GetTableStructures(tableName, localConn);

            #region 获得主键和主键类型

            var pk = "";
            var pktype = "";
            string tiffPath = null;
            foreach (var tableStructure in tablestruct)
            {
                if (!tableStructure.IsPk) continue;
                pk = tableStructure.Name;
                pktype = tableStructure.SystemTypeName;
                break;
            }

            #endregion

            string sql = "select * from " + tableName;

            var checkTable =
                "SELECT  * FROM dbo.SysObjects WHERE ID = object_id(N'[" + tableName +
                "]') AND OBJECTPROPERTY(ID, 'IsTable') = 1";
            var sqlDataReader = SqlHelper.ExecuteReader(SqlHelper.GetConnection(), CommandType.Text,
                checkTable);
            bool isTableExist = false;
            while (sqlDataReader.Read())
            {
                isTableExist = true;
            }

            if (!isTableExist)
            {
                slog.Error(tableName + "查无此表！！！");
                return;
            }

            var ds = SqlHelper.ExecuteDataset(localConn, CommandType.Text, sql);

            if (ds.Tables.Count < 1)
            {
                slog.Info("表" + tableName + "中查无数据！");
                return;
            }


            var rows = ds.Tables[0].Rows;

            if (rows.Count > 0)
            {
                StringBuilder insertSql = null;
                foreach (DataRow dataRow in rows)
                {
                    //远程库中是否有数据，有旧数据删除；无继续!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
//                    var delRemote="select from "+tableName+" where "

                    // 查找tiff文件，如果有插入操作直接写入；没有为空
                    var sVoucherNo = dataRow["sVoucherNo"];
                    var tiffsql = "select sFilePathName from ty_VoucherFile where sVoucherNo='" + sVoucherNo + "'";
//                    SqlCommand tiffCommand = new SqlCommand(tiffsql, localConn);
                    try
                    {
                        var result = SqlHelper.ExecuteReader(localConn, CommandType.Text, tiffsql);
                        tiffPath = "";
//                        var result = tiffCommand.ExecuteReader();
                        if (result.Read())
                        {
                            tiffPath = result["sFilePathName"].ToString();
                        }
                        else
                        {
                            slog.Error("sVoucherNo='" + sVoucherNo + "查无数据");
                        }
                        result.Close();
                    }
                    catch (Exception ex)
                    {
                        slog.Error(ex.Message);
                    }


                    insertSql = new StringBuilder("insert into " + tableName + "(");
                    foreach (var tableStructure in tablestruct)
                    {
                        insertSql.Append(tableStructure.Name).Append(",");
                    }
                    insertSql.Append(Constant.TIFF_DEST + "," + Constant.HIS_ID +
                                     ") values (");

                    for (int i = 0; i < tablestruct.Count; i++)
                    {
                        var memberInfo = dataRow[i].ToString();
                        var type = dataRow[i].GetType().ToString();
                        switch (type)
                        {
                            case "System.Int64":
                                insertSql.Append(dataRow[i]);
                                break;
                            case "System.String":
                                var str = dataRow[i].ToString();
                                if (String.IsNullOrEmpty(str))
                                {
                                    insertSql.Append("NULL");
                                }
                                else
                                {
                                    insertSql.Append("'").Append(str).Append("'");
                                }

                                break;
                            case "System.DBNull":
                                insertSql.Append("NULL");
                                break;
                        }
                        insertSql.Append(",");
                    }

                    //有tiff路径，则进行tiff移动。 如果报错，词条记录不删除   
                    //修改，插入成功后，移动文件，更新数据；
                    var isNullOrEmpty = string.IsNullOrEmpty(tiffPath);
                    bool moveFlag = false;
                    var path = "";
                    string dest;
                    if (isNullOrEmpty) dest = "NULL";
                    else
                    {
                        path = configModel.Pic + '\\' + tiffPath.Substring(tiffPath.LastIndexOf('\\') + 1);
                        dest = "'" + path + "'";
                        moveFlag = MoveTiffFile(tiffPath, path, tableName);
                    }

                    if (!moveFlag)
                    {
                        dest = "NULL";
                    }
                    insertSql.Append(dest + ",'" + configModel.Hisid + "' )");
                    try
                    {
//                        var insertCmd = new SqlCommand(insertSql.ToString(), remoteConn);
                        var insertResult = -1;
//                        insertResult = insertCmd.ExecuteNonQuery();
                        insertResult = RemoteSqlHelper.ExecuteNonQuery(remoteConn, CommandType.Text,
                            insertSql.ToString());
                        if (insertResult != -1)
                        {
                            slog.Debug("向表" + tableName + "中添加数据成功：" + insertSql.ToString());

                            //更新表数据

                            //新表数据添加成功，删除旧表数据
                            var value = "";
                            if (pktype.Equals("int") || pktype.Equals("bigint"))
                            {
                                value = dataRow[pk].ToString();
                            }
                            else if (pktype.Equals("varchar"))
                            {
                                value = "'" + dataRow[pk] + "'";
                            }
                            var delsql = "delete from " + tableName + " where " + pk + "=" + value;
//                            var delcmd = SqlHelper.ExecuteNonQuery(localConn,CommandType.Text,delsql);
//                            var delcmd = new SqlCommand(delsql, localConn);
                            try
                            {
//                                    if (moveFlag)
//                                    {
                                var delresult = SqlHelper.ExecuteNonQuery(localConn, CommandType.Text, delsql);
                                if (delresult != -1)
                                {
                                    slog.Debug("主键为：" + value + "旧表数据删除成功");
                                }
//                                    }
                            }
                            catch (SqlException e)
                            {
                                slog.Error("主键为：" + value + "删除本地表中数据失败");
                                slog.Error(e.Message);
                            }
                        }
                        else
                        {
                            slog.Error("向表" + tableName + "中添加数据失败：" + insertSql.ToString());
                        }
                    }
                    catch (Exception)
                    {
                        slog.Error("向表" + tableName + "中添加数据失败：" + insertSql.ToString());
//                        localTransaction.Rollback();
//                        remoteTransaction.Rollback();
                    }
                }
            }
            else
            {
                slog.Info("数据表" + tableName + "中查无数据！");
            }
        }

        /// <summary>
        /// 检查本机是否联网
        /// </summary>
        public bool CheckNetWork()
        {
            //mTables.ForEach();
            string ip = configModel.Remoteip;
            Ping ping = new Ping();
            var reply = ping.Send(ip);
            if (reply != null && reply.Status == IPStatus.Success)
            {
                slog.Info("本地主机与远程服务器网络连接成功，开始数据操作……");
                return true;
            }
            else
            {
                slog.Error("本地主机与远程服务器网络连接失败！");
                return false;
            }
        }

//        /// <summary>
//        /// 废弃
//        /// </summary>
//        public void CheckTableUpdate()
//        {
//        }


        public void DataTransfer()
        {
            mTables.ForEach(CopyTableData);
        }

//
//        public void TiffMove()
//        {
////            foreach (var bean in GetTiffFiles())
////            {
////                MoveTiffFile(bean.SFilePahtName, bean.Destination);
////            }
//        }

        /// <summary>
        /// 关闭数据库连接
        /// </summary>
        public void Close()
        {
            try
            {
                localConn?.Close();
                localConn = null;
                slog.Info("本地数据库关闭成功！");
            }
            catch (Exception)
            {
                slog.Error("本地数据库关闭失败！");
            }

            try
            {
                remoteConn?.Close();
                remoteConn = null;
                slog.Info("远程数据库关闭成功！");
            }
            catch (Exception)
            {
                slog.Error("远程数据库关闭失败！");
            }
        }

        /// <summary>
        /// tablelist的表数据复制
        /// </summary>
        /// <param name="tableName"></param>
        private void CopyOriginTableData(string tableName)
        {
            slog.Info("进行表数据复制");
            slog.Info("表名：" + tableName);
            slog.Info("-----------------------------------------");
            //获得表结构
            var tablestruct = GetTableStructures(tableName, localConn);

            #region 获得主键和主键类型

            var pk = "";
            var pktype = "";
            foreach (var tableStructure in tablestruct)
            {
                if (!tableStructure.IsPk) continue;
                pk = tableStructure.Name;
                pktype = tableStructure.SystemTypeName;
                break;
            }

            #endregion

            string sql = "select * from " + tableName;
            
            List<string> key = new List<string>();
            List<string> valueList = new List<string>();

            var ds = SqlHelper.ExecuteDataset(localConn, CommandType.Text, sql);
            if (ds.Tables.Count < 1)
            {
                slog.Info("表" + tableName + "中查无数据！");
                return;
            }


            var rows = ds.Tables[0].Rows;

            if (rows.Count > 0)
            {
               
                StringBuilder insertSql = null;
                foreach (DataRow dataRow in rows)
                { var checksql = new StringBuilder("select count(*) from ").Append(tableName);
                    insertSql = new StringBuilder("insert into " + tableName + "(");
                    for (int index = 0; index < tablestruct.Count; index++)
                    {
                        var tableStructure = tablestruct[index];
                        key.Add(tableStructure.Name);
                        insertSql.Append(tableStructure.Name);
                        if (index != tablestruct.Count - 1)
                        {
                            insertSql.Append(",");
                        }
                    }
                    insertSql.Append(") values (");
                    for (int i = 0; i < tablestruct.Count; i++)
                    {
                        var memberInfo = dataRow[i].ToString();
                        var type = dataRow[i].GetType().ToString();
                        switch (type)
                        {
                            case "System.Int32":
                                insertSql.Append(dataRow[i]);
                                valueList.Add(dataRow[i].ToString());
                                break;
                            case "System.Int64":
                                insertSql.Append(dataRow[i]);
                                valueList.Add(dataRow[i].ToString());
                                break;
                            case "System.String":
                                var str = dataRow[i].ToString();
                                if (String.IsNullOrEmpty(str))
                                {
                                    insertSql.Append("NULL");
                                    valueList.Add("NULL");
                                }
                                else
                                {
                                    insertSql.Append("'").Append(str).Append("'");
                                    valueList.Add("'" + str + "'");
                                }
                                break;
                            case "System.DBNull":
                                insertSql.Append("NULL");
                                valueList.Add("NULL");
                                break;
                        }
                        if (i != tablestruct.Count - 1)
                        {
                            insertSql.Append(",");
                        }
                        else
                        {
                            insertSql.Append(")");
                        }
                    }
                    checksql.Append(" where ");
                    checksql.Append(key[4]).Append("=").Append(valueList[4]);
//                    checksql.Append(" and ");
//                    checksql.Append(key[2]).Append("=").Append(valueList[2]);

                    try
                    {
                        var insertResult = -1;
                        insertResult = RemoteSqlHelper.ExecuteNonQuery(remoteConn, CommandType.Text,
                            insertSql.ToString());
                        if (insertResult != -1)
                        {
                            var result = RemoteSqlHelper.ExecuteScalar(RemoteSqlHelper.GetConnection(),
                                             CommandType.Text,
                                             checksql.ToString()) !=null;

                            if (result)
                            {
                                slog.Debug("向表" + tableName + "中添加数据成功：" + insertSql.ToString());
                            }
                        }
                        else
                        {
                            slog.Error("向表" + tableName + "中添加数据失败：" + insertSql.ToString());
                        }
                    }
                    catch (Exception)
                    {
                        slog.Error("向表" + tableName + "中添加数据失败：" + insertSql.ToString());
                    }
                }
            }
            else
            {
                slog.Info("数据表" + tableName + "中查无数据！");
            }
        }

        /// <summary>
        /// 复制FillTableList
        /// </summary>
        public void CopyTableList()
        {
            CheckLocalConn();
            CheckRemotConn();
            //检查远程库中是否存在数据表
            string tableName = "ty_FillTableList";
            var checkTable =
                "SELECT  * FROM dbo.SysObjects WHERE ID = object_id(N'[ty_FillTableList]') AND OBJECTPROPERTY(ID, 'IsTable') = 1";
            var sqlDataReader = RemoteSqlHelper.ExecuteReader(RemoteSqlHelper.GetConnection(), CommandType.Text,
                checkTable);
            bool isTableExist = false;
            while (sqlDataReader.Read())
            {
                isTableExist = true;
            }

//            如果表不存在，创建表，添加数据；存在,检查列表个数，记录没有，添加；
            if (!isTableExist)
            {
                //            获得本地数据表结构
                var fields = GetTableStructures(tableName, localConn);
                //根据得到的表结构，生成并创建表  CREATE TABLE [dbo].[NewTable] (//[a] varchar(255) NOT NULL,//[e] int NULL,//[r] text NULL,//PRIMARY KEY([a])//)////GO
                StringBuilder sb = new StringBuilder("create table ")
                    .Append(tableName + " ( ");
                string pm = "";
                for (int index = 0; index < fields.Count; index++)
                {
                    var tableStructure = fields[index];
                    sb.Append(tableStructure.Name);
                    switch (tableStructure.SystemTypeName)
                    {
                        case "bigint":
                            sb.Append(" " + tableStructure.SystemTypeName);
                            break;
                        case "varchar":
                            var value = (Convert.ToInt64(tableStructure.Maxlength)) > 0
                                ? tableStructure.Maxlength.ToString()
                                : "max";
                            sb.Append(" " + tableStructure.SystemTypeName.Trim() + "(" + value + ") ");
                            break;
                        case "int":
                            sb.Append(" " + tableStructure.SystemTypeName);
                            break;
                    }
                    sb.Append((!tableStructure.IsNullable) ? " NULL" : " NOT NULL");
                    if (index != fields.Count - 1)
                    {
                        sb.Append(",");
                    }
                }
                sb.Append(")");
                try
                {
                    var createTableResult = RemoteSqlHelper.ExecuteNonQuery(RemoteSqlHelper.GetConnection(),
                        CommandType.Text, sb.ToString());
                    slog.Info("创建表" + tableName + "成功！");
                    CopyOriginTableData(tableName);
                }
                catch (Exception)
                {
                  //  slog.Error("创建表" + tableName + "失败！");
                }
            }
            else
            {
                //有表，数据比对，插入数据行
                CompareOriginTableData(tableName);
            }
        }

        /// <summary>
        /// 比较表结构的不同
        /// </summary>
        /// <param name="tableName"></param>
        private void CompareOriginTableData(string tableName)
        {
            var sql = "select * from " + tableName;
            var localset = SqlHelper.ExecuteDataset(SqlHelper.GetConnection(), CommandType.Text, sql);
            var adapter = new SqlDataAdapter(sql, RemoteSqlHelper.GetConnection());
            DataTable remoteTable = new DataTable();
            adapter.Fill(remoteTable);

            var locatTable = localset.Tables[0];
            List<string> nameList = new List<string>();
            foreach (DataRow str in remoteTable.Rows)
            {
                nameList.Add(str["sTableName"].ToString());
            }
            StringBuilder insertSql = null;
            foreach (DataRow local in locatTable.Rows)
            {
                var name = local["sTableName"].ToString();
                if (!nameList.Contains(name))
                {
                    insertSql = new StringBuilder("insert into " + tableName + "(");
                    for (int index = 0; index < remoteTable.Columns.Count; index++)
                    {
                        insertSql.Append(remoteTable.Columns[index].ColumnName);
                        if (index != remoteTable.Columns.Count - 1)
                        {
                            insertSql.Append(",");
                        }
                    }
                    insertSql.Append(") values (");
                    for (int i = 0; i < remoteTable.Columns.Count; i++)
                    {
                        var type = local[i].GetType().ToString();
                        switch (type)
                        {
                            case "System.Int32":
                                insertSql.Append(local[i]);
                                break;
                            case "System.Int64":
                                insertSql.Append(local[i]);
                                break;
                            case "System.String":
                                var str = local[i].ToString();
                                if (String.IsNullOrEmpty(str))
                                {
                                    insertSql.Append("NULL");
                                }
                                else
                                {
                                    insertSql.Append("'").Append(str).Append("'");
                                }

                                break;
                            case "System.DBNull":
                                insertSql.Append("NULL");
                                break;
                        }
                        if (i != remoteTable.Columns.Count - 1)
                        {
                            insertSql.Append(",");
                        }
                        else
                        {
                            insertSql.Append(")");
                        }
                    }

                    var insertResult = -1;
                    try
                    {
                        insertResult = RemoteSqlHelper.ExecuteNonQuery(remoteConn, CommandType.Text,
                            insertSql.ToString());
                        slog.Debug("向表" + tableName + "中添加数据成功：" + insertSql.ToString());
                    }
                    catch (Exception)
                    {
                        slog.Error("向表" + tableName + "中添加数据失败：" + insertSql.ToString());
                    }
                }
            }
        }
    }
}