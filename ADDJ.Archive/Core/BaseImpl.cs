using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using Lucene.Net.Index;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Documents;
using Lucene.Net.Store;
using Lucene.Net.Search;
using Lucene.Net.QueryParsers;
using Lucene.Net.Highlight;
using System.Text.RegularExpressions;
using System.Threading;
using GQKN.Archive.Core.AILucene;

namespace GQKN.Archive.Core.Provider
{
    public abstract class BaseImpl<T> where T : new()
    {
        protected string ConnectionString;

        protected string IndexLocation;

        public string TableName;

        /// <summary>
        /// Timeout connection by Second default = 30s
        /// </summary>

        protected int MaxFieldLength;

        protected bool IsUseLucene = false;

        protected bool IsUpdateLucene = false;

        private string[] fieldLucene;

        private static object objLock = new object();

        private const string FILE_NAME_LOCK_LUCENE = "write.lock";

        public int ConnectTimeOut = 0;

        protected BaseImpl()
        {
            ConnectionString = Config.ConnectionString;
            SetInfoDerivedClass();
            CheckUseLucene();
        }

        protected BaseImpl(string connectionString)
        {
            this.ConnectionString = connectionString;
            SetInfoDerivedClass();
            CheckUseLucene();
        }

        /// <summary>
        /// Kiểm tra và full index
        /// </summary>
        private void CheckUseLucene()
        {
            if (IsUseLucene)
            {
                //Get FieldSearch
                string s = string.Empty;
                var p = typeof(T).GetProperties();

                List<string> lst = new List<string>();
                foreach (var pitem in p)
                {
                    if (Type.GetTypeCode(pitem.PropertyType) == TypeCode.String)
                    {
                        object[] attCustom = pitem.GetCustomAttributes(typeof(AIFieldAttribute), true);

                        if (attCustom.Length > 0)
                        {
                            var objKey = attCustom[0] as AIFieldAttribute;
                            if (objKey != null)
                            {
                                if (objKey.FieldIndexLucene == Lucene.Net.Documents.Field.Index.ANALYZED
                                    && objKey.FieldStoreLucene == Lucene.Net.Documents.Field.Store.YES)
                                {
                                    if (objKey.IsKoDau) lst.Add(pitem.Name + "_LONGLX");
                                    lst.Add(pitem.Name);
                                }
                            }
                        }
                    }
                }
                fieldLucene = lst.ToArray();

                this.IndexLocation = Path.Combine(Config.PathIndexLucene, TableName) + @"\";

                //Nếu chưa tồn tại thư mục thì tạo
                if (!System.IO.Directory.Exists(this.IndexLocation))
                {
                    System.IO.Directory.CreateDirectory(this.IndexLocation);
                }
                Lucene.Net.Store.Directory dir = FSDirectory.Open(new DirectoryInfo(this.IndexLocation));

                //Nếu chưa index thì index
                if (!IsIndexExists(dir))
                {
                    if (IsLooked(dir))
                    {
                        IndexWriter.Unlock(dir);
                    }
                    Helper.GhiLogs("LuceneIndex", "Start FullIndex: {0}", TableName);
                    FullindexLucene();
                }
                else if (!File.Exists(Path.Combine(Config.PathIndexLucene, string.Format(@"{0}\Fullimport\{1}", TableName, DateTime.Now.ToString("yyyyMMdd")))))
                {
                    if (!dir.FileExists(FILE_NAME_LOCK_LUCENE))
                    {
                        try
                        {
                            Monitor.Enter(Constant.ObjLockFull);
                            Helper.GhiLogs("LuceneIndex", "Start FullImport: {0}", TableName);
                            this.FullindexLucene();
                        }
                        finally
                        {
                            Monitor.Exit(Constant.ObjLockFull);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// set một số thuộc tính chung của class kế thừa: table name, id name trong db
        /// </summary>
        protected abstract void SetInfoDerivedClass();

        #region Utility
        //----------
        /// <summary>
        /// convert attribute trước khi insert,update vào db, nếu null thì trả về DBNull.Value
        /// </summary>
        /// <param name="atrr"></param>
        /// <returns></returns>
        protected object AttrToDb(object atrr)
        {
            return atrr ?? DBNull.Value;
        }

        //----------
        /// <summary>
        /// lấy giá trị từ db ra, trả về null nếu là dbnull
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="colName"></param>
        /// <returns></returns>
        protected object AttrFromDb(IDataReader reader, string colName)
        {
            return reader[reader.GetOrdinal(colName)] == DBNull.Value ? null : reader[reader.GetOrdinal(colName)];
        }

        //----------
        /// <summary>
        /// convert 1 reader sang 1 item. reader phải read() trước khi truyền vào
        /// </summary>
        /// <param name="reader"></param>
        /// <returns>trả về null nếu exception</returns>
        public virtual T ConvertReaderToData(IDataReader reader)
        {
            T item = new T();
            string colName = string.Empty;
            try
            {
                for (var i = 0; i < reader.FieldCount; i++)
                {
                    colName = reader.GetName(i);
                    object value = AttrFromDb(reader, colName);
                    if (value != null)
                    {
                        // Fix lỗi chữ hoa, chữ thường khi truy vấn dữ liệu
                        string prColName = colName;
                        PropertyInfo[] prs = typeof(T).GetProperties(); // Danh sách Property
                        foreach (PropertyInfo p in prs)
                        {
                            if (p.Name.ToLower() == prColName.ToLower()) // Trùng nhau
                            {
                                typeof(T).GetProperty(p.Name).SetValue(item, value, null);
                                break;
                            }
                        }
                        #region Phiên bản gốc (không dùng)
                        //if (typeof(T).GetProperty(colName) != null)
                        //    typeof(T).GetProperty(colName).SetValue(item, value, null); 
                        #endregion
                    }
                }
            }
            catch (Exception ex) { Utility.LogEvent("Column name: " + colName, ex); throw ex; }

            return item;
        }


        //----------
        /// <summary>
        /// Convert 1 reader sang list item. reader không cần read() trước khi truyền vào
        /// </summary>
        public virtual List<T> ConvertReaderToList(IDataReader reader)
        {
            List<T> list = new List<T>();

            if (reader != null)
            {
                while (reader.Read())
                {
                    list.Add(ConvertReaderToData(reader));
                }
            }

            return list;
        }

        /// <summary>
        /// Convert Datatable sang List item.
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public virtual List<T> ToCollection(DataTable dt)
        {

            List<T> lst = new System.Collections.Generic.List<T>();
            Type tClass = typeof(T);
            PropertyInfo[] pClass = tClass.GetProperties();
            List<DataColumn> dc = dt.Columns.Cast<DataColumn>().ToList();
            T cn;
            foreach (DataRow item in dt.Rows)
            {
                cn = (T)Activator.CreateInstance(tClass);
                foreach (PropertyInfo pc in pClass)
                {
                    // Can comment try catch block. 
                    try
                    {
                        DataColumn d = dc.Find(c => c.ColumnName == pc.Name);
                        if (d != null)
                            pc.SetValue(cn, item[pc.Name], null);
                    }
                    catch (Exception ex)
                    {
                        Helper.GhiLogs(ex);
                    }
                }
                lst.Add(cn);
            }
            return lst;
        }

        /// <summary>
        /// Convert Datatable sang List item với ItemDecode.
        /// </summary>
        public virtual List<T> ToCollectionDecode(DataTable dt)
        {

            List<T> lst = new System.Collections.Generic.List<T>();
            Type tClass = typeof(T);
            PropertyInfo[] pClass = tClass.GetProperties();
            List<DataColumn> dc = dt.Columns.Cast<DataColumn>().ToList();
            T cn;
            foreach (DataRow item in dt.Rows)
            {
                cn = (T)Activator.CreateInstance(tClass);
                foreach (PropertyInfo pc in pClass)
                {
                    // Can comment try catch block. 
                    try
                    {
                        DataColumn d = dc.Find(c => c.ColumnName == pc.Name);
                        if (d != null)
                        {
                            if (d.DataType == typeof(string))
                                pc.SetValue(cn, System.Web.HttpUtility.HtmlDecode(item[pc.Name].ToString()), null);
                            else
                                pc.SetValue(cn, item[pc.Name], null);
                        }
                    }
                    catch (Exception ex)
                    {
                        Helper.GhiLogs(ex);
                    }
                }
                lst.Add(cn);
            }
            return lst;
        }
        #endregion

        #region Các hàm Add, Update, Delete, GetInfo, GetPage

        //----------
        /// <summary>
        /// insert một item vào csdl, 
        /// </summary>
        /// <param name="item">item cần insert </param>
        /// <returns>trả về item vừa insert vào csld</returns>
        public virtual int Add(T item)
        {
            if (Config.TurnOnLog) Utility.LogEvent("Add " + TableName + Environment.NewLine + Newtonsoft.Json.JsonConvert.SerializeObject(item));

            SqlConnection conn = new SqlConnection(ConnectionString);

            StringBuilder commandText = new StringBuilder("usp_" + TableName + "_Add");
            SqlCommand dsCmd = new SqlCommand(commandText.ToString(), conn) { CommandType = CommandType.StoredProcedure };
            int result = 0;

            try
            {
                object obj;
                Monitor.Enter(objLock);
                conn.Open();
                SqlCommandBuilder.DeriveParameters(dsCmd);
                foreach (SqlParameter parameter in dsCmd.Parameters)
                {
                    string paramName = parameter.ParameterName.Substring(1);
                    PropertyInfo field = typeof(T).GetProperty(paramName);
                    if (field != null)
                    {
                        object value = AttrToDb(field.GetValue(item, null));
                        dsCmd.Parameters[parameter.ParameterName].Value = value;
                    }
                    else
                    {
                        dsCmd.Parameters[parameter.ParameterName].Value = DBNull.Value;
                    }
                }
                //insert item vào db
                obj = dsCmd.ExecuteScalar();


                if (obj != null)
                {
                    result = Convert.ToInt32(obj);
                    if (IsUpdateLucene)
                        AddDoc(result);
                }
            }
            catch (SqlException se)
            {
                Utility.LogEvent("Add " + TableName + ":ERROR SQL Exception:", se);
                throw new Exception(Constant.MESSAGE_SERVER_QUA_TAI, se);
            }
            catch (Exception ex)
            {
                Utility.LogEvent("Add " + TableName + ":ERROR:", ex);
                throw new Exception(Constant.MESSAGE_DU_LIEU_CHUA_HOP_LE, ex);
            }
            finally
            {
                Monitor.Exit(objLock);
                conn.Close();
            }

            return result;
        }

        public virtual void AddArchiveWithBulk(DataRow[] dtInsert, List<string> columnName)
        {
            //var conn = new SqlConnection(ConnectionString);
            //var commandText = new StringBuilder("usp_" + TableName + "_AddArchive");
            //var dsCmd = new SqlCommand(commandText.ToString(), conn) { CommandType = CommandType.StoredProcedure };

            try
            {
                using (SqlBulkCopy bulkCopy = new SqlBulkCopy(ConnectionString, SqlBulkCopyOptions.Default))
                {
                    bulkCopy.DestinationTableName = TableName;

                    if (columnName != null)
                    {
                        foreach (string column in columnName)
                        {
                            bulkCopy.ColumnMappings.Add(column, column);
                        }
                    }

                    //SqlCommandBuilder.DeriveParameters(dsCmd);
                    //foreach (SqlParameter parameter in dsCmd.Parameters)
                    //{
                    //    if (parameter.Direction == ParameterDirection.Input)
                    //    {
                    //        var paramName = parameter.ParameterName.Substring(1);

                    //        //SqlCommandBuilder.DeriveParameters(dsCmd);
                    //        bulkCopy.ColumnMappings.Add(paramName, paramName);
                    //    }
                    //}

                    bulkCopy.BulkCopyTimeout = 600000;
                    bulkCopy.WriteToServer(dtInsert);
                }
            }
            catch (Exception ex)
            {
                Helper.GhiLogs(ex);
                throw ex;
            }
        }

        public virtual void AddArchiveWithBulk(DataTable dtInsert, List<string> param)
        {
            //var conn = new SqlConnection(ConnectionString);
            //var commandText = new StringBuilder("usp_" + TableName + "_AddArchive");
            //var dsCmd = new SqlCommand(commandText.ToString(), conn) { CommandType = CommandType.StoredProcedure };

            try
            {
                using (SqlBulkCopy bulkCopy = new SqlBulkCopy(ConnectionString, SqlBulkCopyOptions.Default))
                {
                    bulkCopy.DestinationTableName = TableName;

                    if (param != null)
                    {
                        foreach (var item in param)
                        {
                            bulkCopy.ColumnMappings.Add(item, item);
                        }
                    }

                    bulkCopy.BulkCopyTimeout = 600000;
                    bulkCopy.WriteToServer(dtInsert);
                }
            }
            catch (SqlException se)
            {
                Utility.LogEvent("Add " + TableName + ":ERROR SQL Exception:", se);
                throw new Exception(Constant.MESSAGE_SERVER_QUA_TAI, se);
            }
            catch (Exception ex)
            {
                Utility.LogEvent("Add " + TableName + ":ERROR:", ex);
                throw new Exception(Constant.MESSAGE_DU_LIEU_CHUA_HOP_LE, ex);
            }
        }

        public virtual void AddArchive(T item)
        {
            if (Config.TurnOnLog) Utility.LogEvent("Add " + TableName + Environment.NewLine + Newtonsoft.Json.JsonConvert.SerializeObject(item));

            SqlConnection conn = new SqlConnection(ConnectionString);

            StringBuilder commandText = new StringBuilder("usp_" + TableName + "_AddArchive");
            SqlCommand dsCmd = new SqlCommand(commandText.ToString(), conn) { CommandType = CommandType.StoredProcedure };

            try
            {
                conn.Open();
                SqlCommandBuilder.DeriveParameters(dsCmd);
                foreach (SqlParameter parameter in dsCmd.Parameters)
                {
                    string paramName = parameter.ParameterName.Substring(1);
                    PropertyInfo field = typeof(T).GetProperty(paramName);
                    if (field != null)
                    {
                        var value = AttrToDb(field.GetValue(item, null));
                        dsCmd.Parameters[parameter.ParameterName].Value = value;
                    }
                    else
                    {
                        dsCmd.Parameters[parameter.ParameterName].Value = DBNull.Value;
                    }
                }
                dsCmd.ExecuteNonQuery();
            }
            catch (SqlException se)
            {
                Utility.LogEvent("Add " + TableName + ":ERROR SQL Exception:", se);
                throw new Exception(Constant.MESSAGE_SERVER_QUA_TAI, se);
            }
            catch (Exception ex)
            {
                Utility.LogEvent("Add " + TableName + ":ERROR:", ex);
                throw new Exception(Constant.MESSAGE_DU_LIEU_CHUA_HOP_LE, ex);
            }
            finally
            {
                conn.Close();
            }
        }

        //----------
        /// <summary>
        /// Update 1 item vào db, trả về số bản ghi bị ảnh hưởng. 
        /// </summary>
        /// <returns>trả về -1 nếu exception</returns>
        public virtual int Update(T item)
        {
            if (Config.TurnOnLog) Utility.LogEvent("Update " + TableName + Environment.NewLine + Newtonsoft.Json.JsonConvert.SerializeObject(item));

            int result = -1;
            var conn = new SqlConnection(ConnectionString);
            var commandText = new StringBuilder("usp_" + TableName + "_Update");
            var dsCmd = new SqlCommand(commandText.ToString(), conn);
            dsCmd.CommandType = CommandType.StoredProcedure;
            try
            {
                conn.Open();
                SqlCommandBuilder.DeriveParameters(dsCmd);
                foreach (SqlParameter parameter in dsCmd.Parameters)
                {
                    if (parameter.Direction == ParameterDirection.Input)
                    {
                        var paramName = parameter.ParameterName.Substring(1);
                        var field = typeof(T).GetProperty(paramName);
                        if (field != null)
                        {
                            var value = AttrToDb(field.GetValue(item, null));
                            dsCmd.Parameters[parameter.ParameterName].Value = value;
                        }
                    }
                }
                // Update item vào db
                result = dsCmd.ExecuteNonQuery();

                if (IsUpdateLucene) UpdateDoc(item);
            }
            catch (SqlException se)
            {
                Utility.LogEvent("Update " + TableName + ":ERROR SQL Exception:", se);
                throw new Exception(Constant.MESSAGE_SERVER_QUA_TAI, se);
            }
            catch (Exception ex)
            {
                Utility.LogEvent("Update " + TableName + ":ERROR:", ex);
                throw new Exception(Constant.MESSAGE_DU_LIEU_CHUA_HOP_LE, ex);
            }
            finally
            {
                conn.Close();
            }

            return result;
        }

        //----------
        /// <summary>
        /// update 1 item vào db, trả về số bản ghi bị ảnh hưởng. 
        /// </summary>
        /// <param name="fieldUpdate">DS truong can update. Exam: ViewCount=ViewCount+1, LDate = getdate()</param>
        /// <param name="whereClause">Dieu kien update. Exam: Id=1</param>
        /// <returns>trả về -1 nếu exception</returns>
        public virtual int UpdateDynamic(string fieldUpate, string whereClause)
        {
            if (Config.TurnOnLog)
                Utility.LogEvent(string.Format("UpdateDynamic {0} \n FieldUpdate:{1} \n WhereClause:{2}", TableName, fieldUpate, whereClause));

            int result = -1;
            if (string.IsNullOrEmpty(fieldUpate.Trim()))
                return result;
            if (string.IsNullOrEmpty(whereClause.Trim()))
                return result;

            SqlConnection conn = new SqlConnection(ConnectionString);
            StringBuilder commandText = new StringBuilder("usp_" + TableName + "_UpdateDynamic");
            SqlCommand dsCmd = new SqlCommand(commandText.ToString(), conn);
            dsCmd.CommandType = CommandType.StoredProcedure;
            dsCmd.Parameters.AddWithValue("@FieldUpdate", AttrToDb(fieldUpate));
            dsCmd.Parameters.AddWithValue("@WhereCondition", AttrToDb(whereClause));

            try
            {
                conn.Open();
                //update item vào db
                result = dsCmd.ExecuteNonQuery();

                if (IsUpdateLucene)
                {
                    List<T> lstUpdate = GetListDynamic("", whereClause, "");
                    UpdateDoc(lstUpdate);
                }
            }
            catch (SqlException se)
            {
                Utility.LogEvent("UpdateDynamic " + TableName + ":ERROR SQL Exception:", se);
                throw new Exception(Constant.MESSAGE_SERVER_QUA_TAI, se);
            }
            catch (Exception ex)
            {
                Utility.LogEvent("UpdateDynamic " + TableName + ":ERROR:", ex);
                throw new Exception(Constant.MESSAGE_DU_LIEU_CHUA_HOP_LE, ex);
            }
            finally
            {
                conn.Close();
            }

            return result;
        }

        //----------
        /// <summary>
        /// xóa item được truyền vào. chỉ cần giá trị của id. 
        /// </summary>
        /// <param name="item"></param>
        /// <returns>trả về số bản ghi bị xóa. -1 là exception</returns>
        public virtual int Delete(T item)
        {
            if (Config.TurnOnLog) Utility.LogEvent(string.Format("Delete {0} \n {1}", TableName, Newtonsoft.Json.JsonConvert.SerializeObject(item)));

            int result = 0;
            SqlConnection conn = new SqlConnection(ConnectionString);
            try
            {
                var commandText = new StringBuilder("usp_" + TableName + "_Delete");
                var dsCmd = new SqlCommand(commandText.ToString(), conn);
                dsCmd.CommandType = CommandType.StoredProcedure;
                conn.Open();
                SqlCommandBuilder.DeriveParameters(dsCmd);
                foreach (SqlParameter parameter in dsCmd.Parameters)
                {
                    if (parameter.Direction == ParameterDirection.Input)
                    {
                        var paramName = parameter.ParameterName.Substring(1);
                        var field = typeof(T).GetProperty(paramName);
                        if (field != null)
                        {
                            var value = AttrToDb(field.GetValue(item, null));
                            dsCmd.Parameters[parameter.ParameterName].Value = value;
                        }
                    }
                }
                result = dsCmd.ExecuteNonQuery();

                if (IsUpdateLucene) DeleteDoc(item);
            }
            catch (SqlException se)
            {
                Utility.LogEvent("Delete " + TableName + ":ERROR SQL Exception:", se);
                throw new Exception(Constant.MESSAGE_SERVER_QUA_TAI, se);
            }
            catch (Exception ex)
            {
                Utility.LogEvent("Delete " + TableName + ":ERROR:", ex);
                throw new Exception(Constant.MESSAGE_DU_LIEU_CHUA_HOP_LE, ex);
            }
            finally
            {
                conn.Close();
            }

            return result;
        }

        //----------
        /// <summary>
        /// xóa item được truyền vào. chỉ cần giá trị của id. 
        /// </summary>
        /// <returns>trả về số bản ghi bị xóa. -1 là exception</returns>
        public virtual int Delete(int id)
        {
            if (Config.TurnOnLog) Utility.LogEvent(string.Format("Delete {0} \n Id:{1}", TableName, id));

            int result = 0;
            SqlConnection conn = new SqlConnection(ConnectionString);
            try
            {
                var commandText = new StringBuilder("usp_" + TableName + "_Delete");
                var dsCmd = new SqlCommand(commandText.ToString(), conn);
                dsCmd.CommandType = CommandType.StoredProcedure;
                dsCmd.Parameters.AddWithValue("@Id", AttrToDb(id));
                conn.Open();

                result = dsCmd.ExecuteNonQuery();

                if (IsUpdateLucene) DeleteDoc(id);
            }
            catch (SqlException se)
            {
                Utility.LogEvent("Delete " + TableName + ":ERROR SQL Exception:", se);
                throw new Exception(Constant.MESSAGE_SERVER_QUA_TAI, se);
            }
            catch (Exception ex)
            {
                Utility.LogEvent("Delete " + TableName + ":ERROR:", ex);
                throw new Exception(Constant.MESSAGE_DU_LIEU_CHUA_HOP_LE, ex);
            }
            finally
            {
                conn.Close();
            }

            return result;
        }

        //----------
        /// <summary>
        /// xóa item trong db theo tiêu chí truyền vào
        /// </summary>       
        /// <param name="whereClause">điều kiện truy vấn(ví dụ: "Category=1 AND Id=3")</param>
        /// <returns>trả về null nếu exception, 0 item nếu không có dữ liệu</returns>
        public virtual int DeleteDynamic(string whereClause)
        {


            int result = 0;
            var conn = new SqlConnection(ConnectionString);
            var commandText = new StringBuilder("usp_" + TableName + "_DeleteDynamic");
            var dsCmd = new SqlCommand(commandText.ToString(), conn);
            dsCmd.CommandType = CommandType.StoredProcedure;
            dsCmd.Parameters.AddWithValue("@WhereCondition", AttrToDb(whereClause));
            try
            {
                conn.Open();

                if (IsUpdateLucene)
                {
                    List<T> lstUpdate = GetListDynamic("", whereClause, "");
                    DeleteDoc(lstUpdate);
                }

                result = dsCmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Utility.LogEvent("DeleteDynamic " + TableName + ":ERROR:", ex);
                throw ex;
            }
            finally
            {
                conn.Close();
            }

            return result;
        }


        //----------
        /// <summary>
        /// lấy về item theo id. truyền vào objec, chỉ cần giá trị id
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public virtual T GetInfo(T item)
        {
            var result = default(T);
            var conn = new SqlConnection(ConnectionString);
            try
            {
                var commandText = new StringBuilder("usp_" + TableName + "_GetById");
                var dsCmd = new SqlCommand(commandText.ToString(), conn);
                dsCmd.CommandType = CommandType.StoredProcedure;
                conn.Open();
                SqlCommandBuilder.DeriveParameters(dsCmd);
                foreach (SqlParameter parameter in dsCmd.Parameters)
                {
                    if (parameter.Direction == ParameterDirection.Input)
                    {
                        string paramName = parameter.ParameterName.Substring(1);
                        PropertyInfo field = typeof(T).GetProperty(paramName);
                        if (field != null)
                        {
                            object value = AttrToDb(field.GetValue(item, null));
                            dsCmd.Parameters[parameter.ParameterName].Value = value;
                        }
                    }
                }

                SqlDataReader reader = dsCmd.ExecuteReader();
                if (reader.Read()) result = ConvertReaderToData(reader);
            }
            catch (SqlException se)
            {
                Utility.LogEvent("GetInfo " + TableName + ":ERROR SQL Exception:", se);
                throw new Exception(Constant.MESSAGE_SERVER_QUA_TAI, se);
            }
            catch (Exception ex)
            {
                Utility.LogEvent("GetInfo " + TableName + ":ERROR:", ex);
                throw new Exception(Constant.MESSAGE_DU_LIEU_CHUA_HOP_LE, ex);
            }
            finally
            {
                conn.Close();
            }

            return result;
        }

        //----------
        /// <summary>
        /// Lấy về item theo id. truyền vào objec, chỉ cần giá trị id
        /// </summary>
        public virtual T GetInfo(int id)
        {
            T result = default(T);
            SqlConnection conn = new SqlConnection(ConnectionString);
            try
            {
                StringBuilder commandText = new StringBuilder("usp_" + TableName + "_GetById");
                SqlCommand dsCmd = new SqlCommand(commandText.ToString(), conn);
                dsCmd.CommandType = CommandType.StoredProcedure;
                dsCmd.Parameters.AddWithValue("@Id", AttrToDb(id));
                conn.Open();

                SqlDataReader reader = dsCmd.ExecuteReader();
                while (reader.Read())
                {
                    result = ConvertReaderToData(reader);
                    //Console.WriteLine(result);
                }
            }
            catch (SqlException se)
            {
                Utility.LogEvent("GetInfo " + TableName + ":ERROR SQL Exception:", se);
                throw new Exception(Constant.MESSAGE_SERVER_QUA_TAI, se);
            }
            catch (Exception ex)
            {
                Utility.LogEvent("GetInfo " + TableName + ":ERROR:", ex);
                throw new Exception(Constant.MESSAGE_DU_LIEU_CHUA_HOP_LE, ex);
            }
            finally
            {
                conn.Close();
            }

            return result;
        }
        //----------
        /// <summary>
        /// lấy về toàn bộ item trong db
        /// </summary>
        /// <returns>trả về null nếu exception, 0 item nếu không có dữ liệu</returns>
        public virtual List<T> GetList()
        {
            List<T> result = null;
            var conn = new SqlConnection(ConnectionString);
            var commandText = new StringBuilder("usp_" + TableName + "_GetList_All");
            var dsCmd = new SqlCommand(commandText.ToString(), conn);
            dsCmd.CommandType = CommandType.StoredProcedure;

            try
            {
                conn.Open();
                var reader = dsCmd.ExecuteReader();
                result = ConvertReaderToList(reader);
            }
            catch (SqlException se)
            {
                Utility.LogEvent("GetList " + TableName + ":ERROR SQL Exception:", se);
                throw new Exception(Constant.MESSAGE_SERVER_QUA_TAI, se);
            }
            catch (Exception ex)
            {
                Utility.LogEvent("GetList " + TableName + ":ERROR:", ex);
                throw new Exception(Constant.MESSAGE_DU_LIEU_CHUA_HOP_LE, ex);
            }
            finally
            {
                conn.Close();
            }

            return result;
        }

        //----------
        /// <summary>
        /// lấy về toàn bộ item trong db theo tiêu chí truyền vào
        /// </summary>
        /// <param name="selectQuery">Các trường cần hiển thị ( Ví dụ: "Id,Name,Desc" )</param>
        /// <param name="whereClause">điều kiện truy vấn(ví dụ: "Category=1 AND Id=3")</param>
        /// <param name="orderBy">điều kiện sắp xếp(ví dụ: "CategoryID DESC,Name ASC")</param>
        /// <returns>trả về null nếu exception, 0 item nếu không có dữ liệu</returns>
        public virtual DataTable GetListDynamicToTable(string selectQuery, string whereClause, string orderBy)
        {
            DataTable result = null;
            var conn = new SqlConnection(ConnectionString);
            var commandText = new StringBuilder("usp_" + TableName + "_SelectDynamic");
            var dsCmd = new SqlCommand(commandText.ToString(), conn);
            dsCmd.CommandType = CommandType.StoredProcedure;
            dsCmd.Parameters.AddWithValue("@SelectQuery", AttrToDb(selectQuery));
            dsCmd.Parameters.AddWithValue("@WhereCondition", AttrToDb(whereClause));
            dsCmd.Parameters.AddWithValue("@OrderByExpression", AttrToDb(orderBy));
            try
            {
                conn.Open();
                SqlDataAdapter adapter = new SqlDataAdapter(dsCmd);
                var ds = new DataSet();
                adapter.Fill(ds);

                result = ds.Tables[0];
            }
            catch (SqlException se)
            {
                string mess = "SelectClause: " + selectQuery + Environment.NewLine;
                mess += "WhereClause:" + whereClause + Environment.NewLine;
                Utility.LogEvent(mess + "GetListDynamic " + TableName + ":ERROR SQL Exception:", se);
                throw new Exception(Constant.MESSAGE_SERVER_QUA_TAI, se);
            }
            catch (Exception ex)
            {
                string mess = "SelectClause: " + selectQuery + Environment.NewLine;
                mess += "WhereClause:" + whereClause + Environment.NewLine;
                Utility.LogEvent(mess + "GetListDynamic " + TableName + ":ERROR:", ex);
                throw new Exception(Constant.MESSAGE_DU_LIEU_CHUA_HOP_LE, ex);
            }
            finally
            {
                conn.Close();
            }

            return result;
        }

        //----------
        /// <summary>
        /// lấy về toàn bộ item trong db theo tiêu chí truyền vào
        /// </summary>
        /// <param name="selectQuery">Các trường cần hiển thị ( Ví dụ: "Id,Name,Desc" )</param>
        /// <param name="whereClause">điều kiện truy vấn(ví dụ: "Category=1 AND Id=3")</param>
        /// <param name="orderBy">điều kiện sắp xếp(ví dụ: "CategoryID DESC,Name ASC")</param>
        /// <returns>trả về null nếu exception, 0 item nếu không có dữ liệu</returns>
        public virtual List<T> GetListDynamic(string selectQuery, string whereClause, string orderBy)
        {
            List<T> result = null;
            SqlConnection conn = new SqlConnection(ConnectionString);
            StringBuilder commandText = new StringBuilder("usp_" + TableName + "_SelectDynamic");
            SqlCommand dsCmd = new SqlCommand(commandText.ToString(), conn);
            dsCmd.CommandType = CommandType.StoredProcedure;
            dsCmd.Parameters.AddWithValue("@SelectQuery", AttrToDb(selectQuery));
            dsCmd.Parameters.AddWithValue("@WhereCondition", AttrToDb(whereClause));
            dsCmd.Parameters.AddWithValue("@OrderByExpression", AttrToDb(orderBy));
            try
            {
                // Kết nối dài hơn
                if (ConnectTimeOut > 0) dsCmd.CommandTimeout = ConnectTimeOut;

                conn.Open();
                SqlDataReader reader = dsCmd.ExecuteReader();
                result = ConvertReaderToList(reader);
            }
            catch (SqlException se)
            {
                string mess = "SelectClause: " + selectQuery + Environment.NewLine;
                mess += "WhereClause:" + whereClause + Environment.NewLine;
                Utility.LogEvent(mess + "GetListDynamic " + TableName + ":ERROR SQL Exception:", se);
                throw new Exception(Constant.MESSAGE_SERVER_QUA_TAI, se);
            }
            catch (Exception ex)
            {
                string mess = "SelectClause: " + selectQuery + Environment.NewLine;
                mess += "WhereClause:" + whereClause + Environment.NewLine;
                Utility.LogEvent(mess + "GetListDynamic " + TableName + ":ERROR:", ex);
                throw new Exception(Constant.MESSAGE_DU_LIEU_CHUA_HOP_LE, ex);
            }
            finally
            {
                conn.Close();
            }

            return result;
        }

        //----------
        /// <summary>
        /// lấy về toàn bộ item trong db theo tiêu chí truyền vào
        /// </summary>
        /// <param name="selectQuery">Các trường cần hiển thị ( Ví dụ: "Id,Name,Desc" )</param>
        /// <param name="joinClause">Điều kiện join. Mặc định bảng đầu tiên có Alias là a ( Ví dụ: left join Province b on a.ProvinceId = b.Id )</param>
        /// <param name="whereClause">điều kiện truy vấn(ví dụ: "Category=1 AND Id=3")</param>
        /// <param name="orderBy">điều kiện sắp xếp(ví dụ: "CategoryID DESC,Name ASC")</param>
        /// <returns>trả về null nếu exception, 0 item nếu không có dữ liệu</returns>
        public virtual List<T> GetListDynamicJoin(string selectQuery, string joinClause, string whereClause, string orderBy)
        {
            List<T> result = null;
            var conn = new SqlConnection(ConnectionString);
            var commandText = new StringBuilder("usp_" + TableName + "_SelectDynamicJoin");
            var dsCmd = new SqlCommand(commandText.ToString(), conn);
            dsCmd.CommandType = CommandType.StoredProcedure;
            dsCmd.Parameters.AddWithValue("@SelectQuery", AttrToDb(selectQuery));
            dsCmd.Parameters.AddWithValue("@JoinQuery", AttrToDb(joinClause));
            dsCmd.Parameters.AddWithValue("@WhereCondition", AttrToDb(whereClause));
            dsCmd.Parameters.AddWithValue("@OrderByExpression", AttrToDb(orderBy));
            try
            {
                conn.Open();
                var reader = dsCmd.ExecuteReader();
                result = ConvertReaderToList(reader);
            }
            catch (SqlException se)
            {
                string mess = "SelectClause: " + selectQuery + Environment.NewLine;
                mess = "JoinClause: " + joinClause + Environment.NewLine;
                mess += "WhereClause:" + whereClause + Environment.NewLine;
                Utility.LogEvent(mess + "GetListDynamicJoin " + TableName + ":ERROR SQL Exception:", se);
                throw new Exception(Constant.MESSAGE_SERVER_QUA_TAI, se);
            }
            catch (Exception ex)
            {
                string mess = "SelectClause: " + selectQuery + Environment.NewLine;
                mess = "JoinClause: " + joinClause + Environment.NewLine;
                mess += "WhereClause:" + whereClause + Environment.NewLine;
                Utility.LogEvent(mess + "GetListDynamicJoin " + TableName + ":ERROR:", ex);
                throw new Exception(Constant.MESSAGE_DU_LIEU_CHUA_HOP_LE, ex);
            }
            finally
            {
                conn.Close();
            }

            return result;
        }

        /// <summary>
        /// lấy về list item đã phân trang trên server theo tiêu chí truyền vào
        /// </summary>
        /// <param name="selectQuery">Các trường cần hiển thị ( Ví dụ: "Id,Name,Desc" )</param>
        /// <param name="whereClause">điều kiện truy vấn(ví dụ: "Category=1 AND Id=3")</param>
        /// <param name="orderBy">điều kiện sắp xếp(ví dụ: "CategoryID DESC,Name ASC")</param>
        /// <param name="pageIndex">số thứ tự trang</param>
        /// <param name="pageSize">số bản ghi trên 1 trang</param>
        /// <param name="totalRowCount">tổng số bản ghi thỏa mãn điều kiện filter</param>
        /// <returns></returns>
        public List<T> GetPaged(string selectQuery, string whereClause, string orderBy, int pageIndex, int pageSize, ref int totalRowCount)
        {
            List<T> result = null;
            totalRowCount = 0;
            var conn = new SqlConnection(ConnectionString);
            var commandText = "usp_" + TableName + "_GetPaged";
            var dsCmd = new SqlCommand(commandText, conn) { CommandType = CommandType.StoredProcedure };
            dsCmd.Parameters.AddWithValue("@SelectQuery", AttrToDb(selectQuery));
            dsCmd.Parameters.AddWithValue("@WhereCondition", AttrToDb(whereClause));
            dsCmd.Parameters.AddWithValue("@OrderByExpression", AttrToDb(orderBy));
            dsCmd.Parameters.AddWithValue("@PageIndex", AttrToDb(pageIndex));
            dsCmd.Parameters.AddWithValue("@PageSize", AttrToDb(pageSize));
            dsCmd.Parameters.Add("@TotalRecord", SqlDbType.Int);
            dsCmd.Parameters["@TotalRecord"].Direction = ParameterDirection.Output;

            try
            {
                conn.Open();
                SqlDataAdapter adapter = new SqlDataAdapter(dsCmd);
                DataSet ds = new DataSet();
                adapter.Fill(ds);

                result = ToCollection(ds.Tables[0]);
                totalRowCount = int.Parse(dsCmd.Parameters["@TotalRecord"].Value.ToString());

            }
            catch (SqlException se)
            {
                string mess = "SelectClause: " + selectQuery + Environment.NewLine;
                mess += "WhereClause:" + whereClause + Environment.NewLine;
                Utility.LogEvent(mess + "GetPaged " + TableName + ":ERROR SQL Exception:", se);
                throw new Exception(Constant.MESSAGE_SERVER_QUA_TAI, se);
            }
            catch (Exception ex)
            {
                string mess = "SelectClause: " + selectQuery + Environment.NewLine;
                mess += "WhereClause:" + whereClause + Environment.NewLine;
                Utility.LogEvent(mess + "GetPaged " + TableName + ":ERROR:", ex);
                throw new Exception(Constant.MESSAGE_DU_LIEU_CHUA_HOP_LE, ex);
            }
            finally
            {
                conn.Close();
            }

            return result;
        }

        /// <summary>
        /// lấy về list item (decode) đã phân trang trên server theo tiêu chí truyền vào
        /// </summary>
        /// <param name="selectQuery">Các trường cần hiển thị ( Ví dụ: "Id,Name,Desc" )</param>
        /// <param name="whereClause">điều kiện truy vấn(ví dụ: "Category=1 AND Id=3")</param>
        /// <param name="orderBy">điều kiện sắp xếp(ví dụ: "CategoryID DESC,Name ASC")</param>
        /// <param name="pageIndex">số thứ tự trang</param>
        /// <param name="pageSize">số bản ghi trên 1 trang</param>
        /// <param name="totalRowCount">tổng số bản ghi thỏa mãn điều kiện filter</param>
        /// <returns></returns>
        public List<T> GetPageDecode(string selectQuery, string whereClause, string orderBy, int pageIndex, int pageSize, ref int totalRowCount)
        {
            List<T> result = null;
            totalRowCount = 0;
            var conn = new SqlConnection(ConnectionString);
            var commandText = "usp_" + TableName + "_GetPaged";
            var dsCmd = new SqlCommand(commandText, conn) { CommandType = CommandType.StoredProcedure };
            dsCmd.Parameters.AddWithValue("@SelectQuery", AttrToDb(selectQuery));
            dsCmd.Parameters.AddWithValue("@WhereCondition", AttrToDb(whereClause));
            dsCmd.Parameters.AddWithValue("@OrderByExpression", AttrToDb(orderBy));
            dsCmd.Parameters.AddWithValue("@PageIndex", AttrToDb(pageIndex));
            dsCmd.Parameters.AddWithValue("@PageSize", AttrToDb(pageSize));
            dsCmd.Parameters.Add("@TotalRecord", SqlDbType.Int);
            dsCmd.Parameters["@TotalRecord"].Direction = ParameterDirection.Output;

            try
            {
                conn.Open();
                SqlDataAdapter adapter = new SqlDataAdapter(dsCmd);
                DataSet ds = new DataSet();
                adapter.Fill(ds);

                result = ToCollectionDecode(ds.Tables[0]);
                totalRowCount = int.Parse(dsCmd.Parameters["@TotalRecord"].Value.ToString());

            }
            catch (SqlException se)
            {
                string mess = "SelectClause: " + selectQuery + Environment.NewLine;
                mess += "WhereClause:" + whereClause + Environment.NewLine;
                Utility.LogEvent(mess + "GetPaged " + TableName + ":ERROR SQL Exception:", se);
                throw new Exception(Constant.MESSAGE_SERVER_QUA_TAI, se);
            }
            catch (Exception ex)
            {
                string mess = "SelectClause: " + selectQuery + Environment.NewLine;
                mess += "WhereClause:" + whereClause + Environment.NewLine;
                Utility.LogEvent(mess + "GetPaged " + TableName + ":ERROR:", ex);
                throw new Exception(Constant.MESSAGE_DU_LIEU_CHUA_HOP_LE, ex);
            }
            finally
            {
                conn.Close();
            }

            return result;
        }

        /// <summary>
        /// lấy về list item đã phân trang trên server theo tiêu chí truyền vào
        /// </summary>
        /// <param name="selectQuery">Các trường cần hiển thị ( Ví dụ: "Id,Name,Desc" )</param>
        /// <param name="joinClause">Bảng và điều kiện join ( Ví dụ: "LEFT JOIN PhongBan b on a.PhongBanId = b.Id"</param>
        /// <param name="whereClause">điều kiện truy vấn(ví dụ: "Category=1 AND Id=3")</param>
        /// <param name="orderBy">điều kiện sắp xếp(ví dụ: "CategoryID DESC,Name ASC")</param>
        /// <param name="pageIndex">số thứ tự trang</param>
        /// <param name="pageSize">số bản ghi trên 1 trang</param>
        /// <param name="totalRowCount">tổng số bản ghi thỏa mãn điều kiện filter</param>
        /// <returns></returns>
        public List<T> GetPagedJoin(string selectQuery, string joinClause, string whereClause, string orderBy, int pageIndex, int pageSize, ref int totalRowCount)
        {
            List<T> result = null;
            totalRowCount = 0;
            var conn = new SqlConnection(ConnectionString);
            var commandText = "usp_" + TableName + "_GetPagedJoin";
            var dsCmd = new SqlCommand(commandText, conn) { CommandType = CommandType.StoredProcedure };
            dsCmd.Parameters.AddWithValue("@SelectQuery", AttrToDb(selectQuery));
            dsCmd.Parameters.AddWithValue("@JoinClause", AttrToDb(joinClause));
            dsCmd.Parameters.AddWithValue("@WhereCondition", AttrToDb(whereClause));
            dsCmd.Parameters.AddWithValue("@OrderByExpression", AttrToDb(orderBy));
            dsCmd.Parameters.AddWithValue("@PageIndex", AttrToDb(pageIndex));
            dsCmd.Parameters.AddWithValue("@PageSize", AttrToDb(pageSize));
            dsCmd.Parameters.Add("@TotalRecord", SqlDbType.Int);
            dsCmd.Parameters["@TotalRecord"].Direction = ParameterDirection.Output;

            try
            {
                conn.Open();
                SqlDataAdapter adapter = new SqlDataAdapter(dsCmd);
                DataSet ds = new DataSet();
                adapter.Fill(ds);

                result = ToCollection(ds.Tables[0]);
                totalRowCount = int.Parse(dsCmd.Parameters["@TotalRecord"].Value.ToString());

            }
            catch (SqlException se)
            {
                string mess = "SelectClause: " + selectQuery + Environment.NewLine;
                mess = "JoinClause: " + joinClause + Environment.NewLine;
                mess += "WhereClause:" + whereClause + Environment.NewLine;
                Utility.LogEvent(mess + "GetPagedJoin " + TableName + ":ERROR SQL Exception:", se);
                throw new Exception(Constant.MESSAGE_SERVER_QUA_TAI, se);
            }
            catch (Exception ex)
            {
                string mess = "SelectClause: " + selectQuery + Environment.NewLine;
                mess = "JoinClause: " + joinClause + Environment.NewLine;
                mess += "WhereClause:" + whereClause + Environment.NewLine;
                Utility.LogEvent(mess + "GetPagedJoin " + TableName + ":ERROR:", ex);
                throw new Exception(Constant.MESSAGE_DU_LIEU_CHUA_HOP_LE, ex);
            }
            finally
            {
                conn.Close();
            }

            return result;
        }
        #endregion

        #region Excute
        public virtual int ExecuteNonQuery(string command, SqlParameter[] sqlparam)
        {
            int result = 0;
            var conn = new SqlConnection(ConnectionString);
            var commandText = new StringBuilder(command);
            var dsCmd = new SqlCommand(commandText.ToString(), conn);
            dsCmd.CommandType = CommandType.StoredProcedure;
            if (sqlparam != null)
                dsCmd.Parameters.AddRange(sqlparam);
            try
            {
                conn.Open();
                result = dsCmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                string mess = "Store: " + command;
                Utility.LogEvent(mess, ex);
            }
            finally
            {
                conn.Close();
            }

            return result;
        }

        public virtual int ExecuteNonQueryNoStore(string command)
        {
            int result = 0;
            var conn = new SqlConnection(ConnectionString);
            SqlCommand dsCmd = new SqlCommand();
            dsCmd.CommandText = command;
            dsCmd.CommandType = CommandType.Text;
            dsCmd.Connection = conn;

            try
            {
                Monitor.Enter(objLock);

                conn.Open();
                result = dsCmd.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                string mess = "Store: " + dsCmd.CommandText;
                Utility.LogEvent(mess, ex);
                throw ex;
            }
            finally
            {
                Monitor.Exit(objLock);
                conn.Close();
            }

            return result;
        }
        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created date : 15/12/2014
        /// Todo : Thêm đối commandType cho phép người dùng có thể dùng các loại commandType khác, không nhất thiết là Stored
        /// </summary>
        /// <param name="command"></param>
        /// <param name="commandType"></param>
        /// <param name="sqlparam"></param>
        /// <returns></returns>
        public virtual int ExecuteNonQuery(string command, CommandType commandType, SqlParameter[] sqlparam)
        {
            int result = 0;
            SqlConnection conn = new SqlConnection(ConnectionString);
            StringBuilder commandText = new StringBuilder(command);
            SqlCommand dsCmd = new SqlCommand(commandText.ToString(), conn);
            dsCmd.CommandType = commandType;
            if (sqlparam != null)
                dsCmd.Parameters.AddRange(sqlparam);
            try
            {
                conn.Open();
                result = dsCmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
            }

            return result;
        }

        public virtual int ExecuteNonQuery(SqlCommand dsCmd)
        {
            int result = 0;
            var conn = new SqlConnection(ConnectionString);
            dsCmd.Connection = conn;

            try
            {
                Monitor.Enter(objLock);

                conn.Open();
                result = dsCmd.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                string mess = "Store: " + dsCmd.CommandText;
                Utility.LogEvent(mess, ex);
                throw ex;
            }
            finally
            {
                Monitor.Exit(objLock);
                conn.Close();
            }

            return result;
        }



        public virtual List<T> ExecuteQuery(string command, SqlParameter[] sqlparam)
        {
            List<T> result = null;
            var conn = new SqlConnection(ConnectionString);
            var commandText = new StringBuilder(command);
            var dsCmd = new SqlCommand(commandText.ToString(), conn);
            dsCmd.CommandType = CommandType.StoredProcedure;
            if (sqlparam != null)
                dsCmd.Parameters.AddRange(sqlparam);
            try
            {
                conn.Open();
                var reader = dsCmd.ExecuteReader();
                result = ConvertReaderToList(reader);
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
            }
            finally
            {
                conn.Close();
            }

            return result;
        }

        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created date : 07/05/2014
        /// Todo : Cho phép thực thi sql từ stored hoặc câu lệnh trực tiếp
        /// </summary>
        /// <param name="command"></param>
        /// <param name="commandType"></param>
        /// <param name="sqlparam"></param>
        /// <returns></returns>
        public virtual List<T> ExecuteQuery(string command, CommandType commandType, SqlParameter[] sqlparam)
        {
            List<T> result = null;
            var conn = new SqlConnection(ConnectionString);
            var commandText = new StringBuilder(command);
            var dsCmd = new SqlCommand(commandText.ToString(), conn);
            dsCmd.CommandType = commandType;
            if (sqlparam != null)
                dsCmd.Parameters.AddRange(sqlparam);
            try
            {
                conn.Open();
                var reader = dsCmd.ExecuteReader();
                result = ConvertReaderToList(reader);
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
            }
            finally
            {
                conn.Close();
            }

            return result;
        }

        public virtual DataSet ExecuteQueryToDataSet(string command, SqlParameter[] sqlparam)
        {
            DataSet result = null;
            var conn = new SqlConnection(ConnectionString);
            var commandText = new StringBuilder(command);
            var dsCmd = new SqlCommand(commandText.ToString(), conn);
            dsCmd.CommandType = CommandType.StoredProcedure;
            if (sqlparam != null)
                dsCmd.Parameters.AddRange(sqlparam);
            try
            {
                conn.Open();

                SqlDataAdapter adapter = new SqlDataAdapter(dsCmd);
                result = new DataSet();
                adapter.Fill(result);
            }
            catch (Exception ex)
            {
                Utility.LogEvent("ExecuteQueryToDataSet: " + command, ex);
            }
            finally
            {
                conn.Close();
            }

            return result;
        }

        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created date : 05/06/2014
        /// Todo : Thực hiện truy vấn tùy theo dạng commandType truyền vào (stored hay text)
        /// </summary>
        /// <param name="command"></param>
        /// <param name="commandType"></param>
        /// <param name="sqlparam"></param>
        /// <returns></returns>
        public virtual DataSet ExecuteQueryToDataSet(string command, CommandType commandType, SqlParameter[] sqlparam)
        {
            DataSet result = null;
            var conn = new SqlConnection(ConnectionString);
            var commandText = new StringBuilder(command);
            var dsCmd = new SqlCommand(commandText.ToString(), conn);
            dsCmd.CommandType = commandType;
            if (sqlparam != null)
                dsCmd.Parameters.AddRange(sqlparam);
            try
            {
                conn.Open();

                SqlDataAdapter adapter = new SqlDataAdapter(dsCmd);
                result = new DataSet();
                adapter.Fill(result);
            }
            catch (Exception ex)
            {
                Utility.LogEvent("ExecuteQueryToDataSet: " + command, ex);
            }
            finally
            {
                conn.Close();
            }

            return result;
        }

        public virtual DataSet ExecuteQueryToDataSet(SqlCommand command)
        {
            DataSet result = null;
            var conn = new SqlConnection(ConnectionString);
            command.Connection = conn;
            try
            {
                conn.Open();

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                result = new DataSet();
                adapter.Fill(result);
            }
            catch (Exception ex)
            {
                Utility.LogEvent("ExecuteQueryToDataSet: " + command.CommandText, ex);
            }
            finally
            {
                conn.Close();
            }

            return result;
        }

        public virtual object ExecuteScalar(string command, SqlParameter[] sqlparam)
        {
            object result = null;
            var conn = new SqlConnection(ConnectionString);
            var commandText = new StringBuilder(command);
            var dsCmd = new SqlCommand(commandText.ToString(), conn);
            dsCmd.CommandType = CommandType.StoredProcedure;
            if (sqlparam != null)
                dsCmd.Parameters.AddRange(sqlparam);
            try
            {
                conn.Open();
                result = dsCmd.ExecuteScalar();
            }
            catch (Exception ex)
            {
                Utility.LogEvent("ExecuteScalar: " + dsCmd.CommandText, ex);
            }
            finally
            {
                conn.Close();
            }

            return result;
        }

        public virtual object ExecuteScalar(SqlCommand sqlCmd)
        {
            object result = null;
            var conn = new SqlConnection(ConnectionString);
            sqlCmd.Connection = conn;

            try
            {
                conn.Open();
                result = sqlCmd.ExecuteScalar();
            }
            catch (Exception ex)
            {
                Utility.LogEvent("ExecuteScalar: " + sqlCmd.CommandText, ex);
            }
            finally
            {
                conn.Close();
            }

            return result;
        }

        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created date : 17/07/2015
        /// </summary>
        /// <param name="command"></param>
        /// <param name="commandType"></param>
        /// <param name="sqlparam"></param>
        /// <returns></returns>
        public virtual object ExecuteScalar(string command, CommandType commandType, SqlParameter[] sqlparam)
        {
            object result = null;
            var conn = new SqlConnection(ConnectionString);
            var commandText = new StringBuilder(command);
            var dsCmd = new SqlCommand(commandText.ToString(), conn);
            dsCmd.CommandType = commandType;
            if (sqlparam != null)
                dsCmd.Parameters.AddRange(sqlparam);
            try
            {
                conn.Open();
                result = dsCmd.ExecuteScalar();
            }
            catch (Exception ex)
            {
                Utility.LogEvent("ExecuteScalar: " + dsCmd.CommandText, ex);
            }
            finally
            {
                conn.Close();
            }

            return result;
        }
        #endregion

        #region Other
        public DataTable GetColumnSchema()
        {
            SqlConnection conn = new SqlConnection(ConnectionString);

            string commandText = string.Format("SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME='{0}'", this.TableName);
            SqlCommand dsCmd = new SqlCommand(commandText, conn) { CommandType = CommandType.Text };

            try
            {
                conn.Open();
                SqlDataAdapter adapter = new SqlDataAdapter(dsCmd);
                DataSet ds = new DataSet();
                adapter.Fill(ds);

                return ds.Tables[0];
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
            }
        }
        #endregion

        #region Lucene
        public virtual void FullindexLucene()
        {
            IndexWriter writer = null;
            SqlConnection conn = null;
            FileStream fs = null;
            try
            {
                Lucene.Net.Store.Directory dir = FSDirectory.Open(new DirectoryInfo(this.IndexLocation));
                var maxFieldLeng = new IndexWriter.MaxFieldLength(this.MaxFieldLength);
                writer = new IndexWriter(dir, new StandardAnalyzer(Lucene.Net.Util.Version.LUCENE_29), true, maxFieldLeng);

                conn = new SqlConnection(ConnectionString);
                StringBuilder commandText = new StringBuilder("usp_" + TableName + "_GetList_All");
                SqlCommand dsCmd = new SqlCommand(commandText.ToString(), conn);
                dsCmd.CommandType = CommandType.StoredProcedure;

                conn.Open();
                SqlDataReader reader = dsCmd.ExecuteReader();

                IndexListToLucene(writer, reader);

                if (!System.IO.Directory.Exists(Path.Combine(Config.PathIndexLucene, string.Format(@"{0}\Fullimport", TableName))))
                {
                    System.IO.Directory.CreateDirectory(Path.Combine(Config.PathIndexLucene, string.Format(@"{0}\Fullimport", TableName)));
                }

                fs = File.Create(Path.Combine(Config.PathIndexLucene, string.Format(@"{0}\Fullimport\{1}", TableName, DateTime.Now.ToString("yyyyMMdd"))));
            }
            catch (Exception ex)
            {
                Utility.LogEvent("FullindexLucene " + TableName, ex);
            }
            finally
            {
                if (conn != null)
                    conn.Close();
                if (writer != null)
                {
                    writer.Optimize();
                    writer.Close();
                }
                if (fs != null)
                    fs.Close();
            }
        }

        public int CountDoc()
        {
            int result = 0;
            try
            {
                Lucene.Net.Store.Directory dir = FSDirectory.Open(new DirectoryInfo(this.IndexLocation));
                var searcher = new Lucene.Net.Search.IndexSearcher(dir, true);

                result = searcher.MaxDoc();
            }
            catch (Exception ex)
            {
                Utility.LogEvent("CountDoc error", ex);
            }
            return result;
        }

        public bool AddDoc(int id)
        {
            SqlConnection conn = new SqlConnection(ConnectionString);
            bool flag = false;
            IndexWriter writer = null;
            try
            {
                Lucene.Net.Store.Directory dir = FSDirectory.Open(new DirectoryInfo(this.IndexLocation));
                var maxFieldLeng = new IndexWriter.MaxFieldLength(this.MaxFieldLength);
                writer = new IndexWriter(dir, new StandardAnalyzer(Lucene.Net.Util.Version.LUCENE_29), false, maxFieldLeng);

                var commandText = new StringBuilder("usp_" + TableName + "_GetById");
                var dsCmd = new SqlCommand(commandText.ToString(), conn);
                dsCmd.CommandType = CommandType.StoredProcedure;
                dsCmd.Parameters.AddWithValue("@Id", AttrToDb(id));
                conn.Open();

                var reader = dsCmd.ExecuteReader();
                while (reader.Read())
                {
                    IndexDocument(writer, reader);
                }
                writer.Commit();
                flag = true;
            }
            catch (Exception ex)
            {
                Utility.LogEvent("AddDoc " + TableName, ex);
            }
            finally
            {
                conn.Close();
                if (writer != null)
                {
                    writer.Close();
                }
            }

            return flag;
        }

        public bool AddDoc(T item)
        {
            bool flag = false;
            IndexWriter writer = null;
            try
            {
                Lucene.Net.Store.Directory dir = FSDirectory.Open(new DirectoryInfo(this.IndexLocation));
                var maxFieldLeng = new IndexWriter.MaxFieldLength(this.MaxFieldLength);
                writer = new IndexWriter(dir, new StandardAnalyzer(Lucene.Net.Util.Version.LUCENE_29), false, maxFieldLeng);

                Document doc = ConvertItemToDocument(item);

                writer.AddDocument(doc);
                writer.Commit();
                flag = true;
            }
            catch (Exception ex)
            {
                Utility.LogEvent("AddDoc " + TableName, ex);
            }
            finally
            {
                if (writer != null)
                {
                    writer.Close();
                }
            }

            return flag;
        }

        public bool UpdateDoc(int id)
        {
            SqlConnection conn = new SqlConnection(ConnectionString);
            bool flag = false;
            IndexWriter writer = null;
            try
            {
                Monitor.Enter(Constant.ObjLockFull);
                Lucene.Net.Store.Directory dir = FSDirectory.Open(new DirectoryInfo(this.IndexLocation));
                var maxFieldLeng = new IndexWriter.MaxFieldLength(this.MaxFieldLength);
                writer = new IndexWriter(dir, new StandardAnalyzer(Lucene.Net.Util.Version.LUCENE_29), false, maxFieldLeng);

                var oParserDel = new Lucene.Net.QueryParsers.QueryParser(Lucene.Net.Util.Version.LUCENE_29, "Id", new StandardAnalyzer(Lucene.Net.Util.Version.LUCENE_29));
                var queryDel = oParserDel.Parse(id.ToString());
                writer.DeleteDocuments(queryDel);

                StringBuilder commandText = new StringBuilder("usp_" + TableName + "_GetById");
                SqlCommand dsCmd = new SqlCommand(commandText.ToString(), conn);
                dsCmd.CommandType = CommandType.StoredProcedure;
                dsCmd.Parameters.AddWithValue("@Id", AttrToDb(id));
                conn.Open();

                SqlDataReader reader = dsCmd.ExecuteReader();
                while (reader.Read())
                {
                    IndexDocument(writer, reader);
                }
                writer.Commit();
                flag = true;
            }
            catch (Exception ex)
            {
                Utility.LogEvent("UpdateDoc " + TableName, ex);
            }
            finally
            {
                if (conn != null)
                    conn.Close();
                if (writer != null)
                {
                    writer.Close();
                }

                Monitor.Exit(Constant.ObjLockFull);
            }
            return flag;
        }

        public bool UpdateDoc(T item)
        {
            bool flag = false;
            IndexWriter writer = null;
            try
            {
                Lucene.Net.Store.Directory dir = FSDirectory.Open(new DirectoryInfo(this.IndexLocation));
                var maxFieldLeng = new IndexWriter.MaxFieldLength(this.MaxFieldLength);
                writer = new IndexWriter(dir, new StandardAnalyzer(Lucene.Net.Util.Version.LUCENE_29), false, maxFieldLeng);

                string keyValue = string.Empty;
                string fieldName = string.Empty;
                var doc = ConvertItemToDocument(item);
                var p = typeof(T).GetProperties();
                foreach (var pItem in p)
                {
                    object[] attCustom = pItem.GetCustomAttributes(typeof(AIFieldAttribute), true);
                    if (attCustom.Length > 0)
                    {
                        var objKey = attCustom[0] as AIFieldUnikeyAttribute;
                        if (objKey != null)
                        {
                            fieldName = objKey.FieldName;
                            keyValue = pItem.GetValue(item, null).ToString();
                            break;
                        }
                    }
                }
                if (!string.IsNullOrEmpty(keyValue))
                {
                    writer.UpdateDocument(new Term(fieldName, keyValue), doc);
                    writer.Commit();
                    flag = true;
                }
            }
            catch (Exception ex)
            {
                Utility.LogEvent("UpdateDoc " + TableName, ex);
            }
            finally
            {
                if (writer != null)
                {
                    writer.Close();
                }
            }

            return flag;
        }

        public bool UpdateDoc(List<T> lst)
        {
            bool flag = false;
            IndexWriter writer = null;
            try
            {
                Monitor.Enter(Constant.ObjLockFull);
                Lucene.Net.Store.Directory dir = FSDirectory.Open(new DirectoryInfo(this.IndexLocation));
                var maxFieldLeng = new IndexWriter.MaxFieldLength(this.MaxFieldLength);
                writer = new IndexWriter(dir, new StandardAnalyzer(Lucene.Net.Util.Version.LUCENE_29), false, maxFieldLeng);

                string keyValue = string.Empty;
                string fieldName = string.Empty;
                foreach (T item in lst)
                {
                    keyValue = string.Empty;

                    Document doc = ConvertItemToDocument(item);
                    PropertyInfo[] p = typeof(T).GetProperties();
                    foreach (var pItem in p)
                    {
                        object[] attCustom = pItem.GetCustomAttributes(typeof(AIFieldAttribute), true);
                        if (attCustom.Length > 0)
                        {
                            var objKey = attCustom[0] as AIFieldUnikeyAttribute;
                            if (objKey != null)
                            {
                                fieldName = objKey.FieldName;
                                keyValue = pItem.GetValue(item, null).ToString();
                                break;
                            }
                        }
                    }
                    if (!string.IsNullOrEmpty(keyValue))
                    {
                        writer.UpdateDocument(new Term(fieldName, keyValue), doc);
                    }
                }
                writer.Commit();
                flag = true;
            }
            catch (Exception ex)
            {
                Utility.LogEvent("UpdateListDoc " + TableName, ex);
            }
            finally
            {
                if (writer != null)
                {
                    writer.Close();
                }

                Monitor.Exit(Constant.ObjLockFull);
            }

            return flag;
        }

        public bool UpdateDoc(int id, T item)
        {
            bool flag = false;
            IndexWriter writer = null;
            try
            {
                Lucene.Net.Store.Directory dir = FSDirectory.Open(new DirectoryInfo(this.IndexLocation));
                var maxFieldLeng = new IndexWriter.MaxFieldLength(this.MaxFieldLength);
                writer = new IndexWriter(dir, new StandardAnalyzer(Lucene.Net.Util.Version.LUCENE_29), false, maxFieldLeng);

                Document doc = ConvertItemToDocument(item);

                writer.UpdateDocument(new Term("Id", id.ToString()), doc);
                writer.Commit();
                flag = true;
            }
            catch (Exception ex)
            {
                Utility.LogEvent("UpdateDoc " + TableName, ex);
            }
            finally
            {
                if (writer != null)
                {
                    writer.Close();
                }
            }

            return flag;
        }

        public bool DeleteDoc(T item)
        {
            bool flag = false;
            IndexWriter writer = null;
            try
            {
                Lucene.Net.Store.Directory dir = FSDirectory.Open(new DirectoryInfo(this.IndexLocation));
                var maxFieldLeng = new IndexWriter.MaxFieldLength(this.MaxFieldLength);
                writer = new IndexWriter(dir, new StandardAnalyzer(Lucene.Net.Util.Version.LUCENE_29), false, maxFieldLeng);

                string keyValue = string.Empty;
                string fieldName = string.Empty;
                Document doc = ConvertItemToDocument(item);
                PropertyInfo[] p = typeof(T).GetProperties();
                foreach (var pItem in p)
                {
                    object[] attCustom = pItem.GetCustomAttributes(typeof(AIFieldAttribute), true);
                    if (attCustom.Length > 0)
                    {
                        AIFieldUnikeyAttribute objKey = attCustom[0] as AIFieldUnikeyAttribute;
                        if (objKey != null)
                        {
                            fieldName = objKey.FieldName;
                            keyValue = pItem.GetValue(item, null).ToString();
                            break;
                        }
                    }
                }
                if (!string.IsNullOrEmpty(keyValue))
                {
                    var oParserDel = new Lucene.Net.QueryParsers.QueryParser(Lucene.Net.Util.Version.LUCENE_29, fieldName, new StandardAnalyzer(Lucene.Net.Util.Version.LUCENE_29));
                    Query queryDel = oParserDel.Parse(keyValue);
                    writer.DeleteDocuments(queryDel);
                    writer.Commit();
                    flag = true;
                }
            }
            catch (Exception ex)
            {
                Utility.LogEvent("DeleteDoc " + TableName, ex);
            }
            finally
            {
                if (writer != null)
                {
                    writer.Close();
                }
            }

            return flag;
        }

        public bool DeleteDoc(List<T> lst)
        {
            bool flag = false;
            IndexWriter writer = null;
            try
            {
                Lucene.Net.Store.Directory dir = FSDirectory.Open(new DirectoryInfo(this.IndexLocation));
                var maxFieldLeng = new IndexWriter.MaxFieldLength(this.MaxFieldLength);
                writer = new IndexWriter(dir, new StandardAnalyzer(Lucene.Net.Util.Version.LUCENE_29), false, maxFieldLeng);

                string keyValue = string.Empty;
                string fieldName = string.Empty;
                foreach (T item in lst)
                {
                    keyValue = string.Empty;
                    var doc = ConvertItemToDocument(item);
                    var p = typeof(T).GetProperties();
                    foreach (var pItem in p)
                    {
                        var attCustom = pItem.GetCustomAttributes(typeof(AIFieldAttribute), true);
                        if (attCustom.Length > 0)
                        {
                            var objKey = attCustom[0] as AIFieldUnikeyAttribute;
                            if (objKey != null)
                            {
                                fieldName = objKey.FieldName;
                                keyValue = pItem.GetValue(item, null).ToString();
                                break;
                            }
                        }
                    }
                    if (!string.IsNullOrEmpty(keyValue))
                    {
                        var oParserDel = new Lucene.Net.QueryParsers.QueryParser(Lucene.Net.Util.Version.LUCENE_29, fieldName, new StandardAnalyzer(Lucene.Net.Util.Version.LUCENE_29));
                        var queryDel = oParserDel.Parse(keyValue);
                        writer.DeleteDocuments(queryDel);
                    }
                }
                writer.Commit();
                flag = true;
            }
            catch (Exception ex)
            {
                Utility.LogEvent("DeleteDoc " + TableName, ex);
            }
            finally
            {
                if (writer != null)
                {
                    writer.Close();
                }
            }

            return flag;
        }

        public bool DeleteDoc(int id)
        {
            bool flag = false;
            IndexWriter writer = null;
            try
            {
                Lucene.Net.Store.Directory dir = FSDirectory.Open(new DirectoryInfo(this.IndexLocation));
                var maxFieldLeng = new IndexWriter.MaxFieldLength(this.MaxFieldLength);
                writer = new IndexWriter(dir, new StandardAnalyzer(Lucene.Net.Util.Version.LUCENE_29), false, maxFieldLeng);

                var oParserDel = new Lucene.Net.QueryParsers.QueryParser(Lucene.Net.Util.Version.LUCENE_29, "Id", new StandardAnalyzer(Lucene.Net.Util.Version.LUCENE_29));
                var queryDel = oParserDel.Parse(id.ToString());
                writer.DeleteDocuments(queryDel);
                writer.Commit();
                flag = true;
            }
            catch (Exception ex)
            {
                Utility.LogEvent("DeleteDoc " + TableName, ex);
            }
            finally
            {
                if (writer != null)
                {
                    writer.Close();
                }
            }

            return flag;
        }

        public T GetDoc(int id)
        {
            T result = new T();
            try
            {
                Lucene.Net.Store.Directory dir = FSDirectory.Open(new DirectoryInfo(this.IndexLocation));
                var searcher = new Lucene.Net.Search.IndexSearcher(dir, true);

                var oParser = new Lucene.Net.QueryParsers.QueryParser(Lucene.Net.Util.Version.LUCENE_29, "Id", new StandardAnalyzer(Lucene.Net.Util.Version.LUCENE_29));
                Query query = oParser.Parse(id.ToString());

                TopDocs topDocs = searcher.Search(query, null, 1);
                if (topDocs != null && topDocs.totalHits > 0)
                {
                    result = ConvertDocumentToItem(searcher.Doc(topDocs.scoreDocs[0].doc));
                }

            }
            catch (Exception ex)
            {
                Utility.LogEvent("GetDoc error", ex);
            }
            return result;
        }

        public List<T> Search(Query query)
        {
            return Search(query, null, null, 0, false);
        }

        public List<T> Search(Query query, bool hightlight)
        {
            return Search(query, null, null, 0, hightlight);
        }

        public List<T> Search(Query query, int top)
        {
            return Search(query, null, null, top, false);
        }

        public List<T> Search(string keyword)
        {
            Lucene.Net.QueryParsers.MultiFieldQueryParser parser = new MultiFieldQueryParser(Lucene.Net.Util.Version.LUCENE_29, fieldLucene, new StandardAnalyzer(Lucene.Net.Util.Version.LUCENE_29));
            parser.SetDefaultOperator(QueryParser.Operator.OR);
            Query q = parser.Parse(keyword);
            return Search(q, null, null, 20, false);
        }

        public List<T> Search(string keyword, bool hightlight)
        {
            Lucene.Net.QueryParsers.MultiFieldQueryParser parser = new MultiFieldQueryParser(Lucene.Net.Util.Version.LUCENE_29, fieldLucene, new StandardAnalyzer(Lucene.Net.Util.Version.LUCENE_29));
            parser.SetDefaultOperator(QueryParser.Operator.OR);
            Query q = parser.Parse(keyword);
            return Search(q, null, null, 20, hightlight);
        }

        public List<T> Search(string keyword, int top)
        {
            Lucene.Net.QueryParsers.MultiFieldQueryParser parser = new MultiFieldQueryParser(Lucene.Net.Util.Version.LUCENE_29, fieldLucene, new StandardAnalyzer(Lucene.Net.Util.Version.LUCENE_29));
            parser.SetDefaultOperator(QueryParser.Operator.OR);
            Query q = parser.Parse(keyword);
            return Search(q, null, null, top, false);
        }

        public List<T> Search(string keyword, int top, bool hightlight)
        {
            Lucene.Net.QueryParsers.MultiFieldQueryParser parser = new MultiFieldQueryParser(Lucene.Net.Util.Version.LUCENE_29, fieldLucene, new StandardAnalyzer(Lucene.Net.Util.Version.LUCENE_29));
            parser.SetDefaultOperator(QueryParser.Operator.OR);
            Query q = parser.Parse(keyword);
            return Search(q, null, null, top, hightlight);
        }

        public List<T> Search(Query query, Filter filter)
        {
            return Search(query, filter, null, 0, false);
        }

        public List<T> Search(Query query, Filter filter, Sort sort)
        {
            return Search(query, filter, sort, 0, false);
        }

        public List<T> Search(Query query, Filter filter, Sort sort, bool hightlight)
        {
            return Search(query, filter, sort, 0, false);
        }

        /// <summary>
        /// Hàm search
        /// </summary>
        /// <param name="query">Câu query. Ex:
        /// <para>Lấy 1 trường: Id:1</para>
        /// <para>Điều kiện 'và': Id:1 && Status:2</para>
        /// <para>Điều kiện 'hoặc': Id:1 || Status:1</para>
        /// </param>
        /// <param name="filter">Điều kiện lọc: HightLight...</param>
        /// <param name="sort">Sắp xếp</param>
        /// <param name="top">Số bản ghi lấy</param>
        /// <returns></returns>
        public List<T> Search(Query query, Filter filter, Sort sort, int top, bool hightlight)
        {
            List<T> result = null;
            try
            {
                Lucene.Net.Store.Directory dir = FSDirectory.Open(new DirectoryInfo(this.IndexLocation));
                var searcher = new Lucene.Net.Search.IndexSearcher(dir, true);

                if (top == 0)
                    top = 100;

                Hits hits = searcher.Search(query, filter, sort);

                Lucene.Net.Analysis.Standard.StandardAnalyzer analyzer = new Lucene.Net.Analysis.Standard.StandardAnalyzer(Lucene.Net.Util.Version.LUCENE_29);
                // code highlighting
                Formatter formatter = new Lucene.Net.Highlight.SimpleHTMLFormatter("<em>", "</em>");
                Lucene.Net.Highlight.SimpleFragmenter fragmenter = new Lucene.Net.Highlight.SimpleFragmenter(500);
                Lucene.Net.Highlight.QueryScorer scorer = new Lucene.Net.Highlight.QueryScorer(query);
                Highlighter highlighter = new Lucene.Net.Highlight.Highlighter(formatter, scorer);
                highlighter.SetTextFragmenter(fragmenter);

                //for (int i = 0; i < hits.Length(); i++)
                //{
                //    Lucene.Net.Documents.Document doc = hits.Doc(i);
                //    Lucene.Net.Analysis.TokenStream stream = analyzer.TokenStream("", new StringReader(doc.Get("text")));
                //    string highlightedText = highlighter.GetBestFragments(stream, doc.Get("text"), 1, "...");
                //}

                if (hits != null)
                {
                    result = new List<T>();
                    if (hightlight)
                    {
                        for (int i = 0; i < top && i < hits.Length(); i++)
                        {
                            Document oDoc = hits.Doc(i);
                            result.Add(ConvertDocumentToItemHightLight(oDoc, highlighter, analyzer));
                        }
                    }
                    else
                    {
                        for (int i = 0; i < top && i < hits.Length(); i++)
                        {
                            Document oDoc = hits.Doc(i);
                            result.Add(ConvertDocumentToItem(oDoc));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Utility.LogEvent("Search error " + TableName, ex);
            }
            return result;
        }

        /// <summary>
        /// Hàm search và phân trang
        /// </summary>
        /// <param name="keyword">Từ khóa tìm kiếm</param>
        /// <param name="pageIndex">Trang bắt đầu từ 1</param>
        /// <param name="pageSize">Số bản ghi trên trang: Default Config.RecordPerPage</param>
        /// <param name="totalRecord">Tổng số bản ghi tìm kiếm được</param>
        /// <returns></returns>
        public List<T> GetPage(string keyword, int pageIndex, int pageSize, ref int totalRecord)
        {
            Lucene.Net.QueryParsers.MultiFieldQueryParser parser = new MultiFieldQueryParser(Lucene.Net.Util.Version.LUCENE_29, fieldLucene, new StandardAnalyzer(Lucene.Net.Util.Version.LUCENE_29));
            parser.SetDefaultOperator(QueryParser.Operator.OR);
            Query q = parser.Parse(keyword);
            return GetPage(q, null, null, pageIndex, pageSize, ref totalRecord);
        }

        /// <summary>
        /// Hàm phân trang
        /// </summary>
        /// <param name="query">Câu query. Ex:
        /// <para>Lấy 1 trường: Id:1</para>
        /// <para>Điều kiện 'và': Id:1 && Status:2</para>
        /// <para>Điều kiện 'hoặc': Id:1 || Status:1</para>
        /// </param>
        /// <param name="filter">Điều kiện lọc: HightLight...</param>
        /// <param name="sort">Sắp xếp</param>
        /// <param name="pageIndex">Trang bắt đầu từ 1</param>
        /// <param name="pageSize">Số bản ghi trên trang: Default Config.RecordPerPage</param>
        /// <param name="totalRecord">Tổng số bản ghi tìm kiếm được</param>
        /// <returns></returns>
        public List<T> GetPage(Query query, Filter filter, Sort sort, int pageIndex, int pageSize, ref int totalRecord)
        {
            List<T> result = null;
            try
            {
                Lucene.Net.Store.Directory dir = FSDirectory.Open(new DirectoryInfo(this.IndexLocation));
                IndexSearcher searcher = new IndexSearcher(dir, true);

                Hits hits = searcher.Search(query, filter, sort);
                if (hits != null)
                {
                    totalRecord = hits.Length();

                    if (pageIndex == 0)
                        pageIndex = 1;
                    if (pageSize == 0)
                        pageSize = Config.RecordPerPage;

                    int first = (pageIndex - 1) * pageSize, last = pageSize + first;

                    result = new List<T>();
                    for (int i = first; i < last && i < totalRecord; i++)
                    {
                        Document oDoc = hits.Doc(i);
                        result.Add(ConvertDocumentToItem(oDoc));
                    }
                }
            }
            catch (Exception ex)
            {
                Utility.LogEvent("GetPage Lucene " + TableName, ex);
            }
            return result;
        }

        public List<T> MoreLikeThis(string text, int top)
        {
            List<T> result = null;
            IndexSearcher searcher = null;
            IndexReader reader = null;
            try
            {
                Lucene.Net.Store.Directory dir = FSDirectory.Open(new DirectoryInfo(this.IndexLocation));
                searcher = new IndexSearcher(dir, true);

                var oParser = new QueryParser(Lucene.Net.Util.Version.LUCENE_29, "Id", new StandardAnalyzer(Lucene.Net.Util.Version.LUCENE_29));
                Query query = oParser.Parse("102");

                TopDocs hitsTemp = searcher.Search(query, null, 1);
                int docNum = hitsTemp.scoreDocs[0].doc;

                reader = IndexReader.Open(dir, true);

                //Similarity.Net.SimilarityQueries sq = new Similarity.Net.SimilarityQueries();

                Similarity.Net.MoreLikeThis mlt = new Similarity.Net.MoreLikeThis(reader);
                mlt.SetFieldNames(fieldLucene);
                mlt.SetMinTermFreq(1);
                mlt.SetMinDocFreq(1);

                //Stream stream;

                //convert string to stream
                byte[] byteArray = Encoding.UTF8.GetBytes(text);
                MemoryStream stream = new MemoryStream(byteArray);
                Query q = mlt.Like(docNum);

                Hits hits = searcher.Search(q);

                if (hits != null)
                {
                    result = new List<T>();

                    for (int i = 0; i < top && i < hits.Length(); i++)
                    {
                        Document oDoc = hits.Doc(i);
                        result.Add(ConvertDocumentToItem(oDoc));
                    }
                }
            }
            catch (Exception ex)
            {
                Utility.LogEvent("Search error ", ex);
            }
            finally
            {
                if (searcher != null)
                {
                    searcher.Close();
                }
                if (reader != null)
                {
                    reader.Close();
                }
            }

            return result;
        }

        public List<T> Similarity(string text, int top)
        {
            List<T> result = null;
            Lucene.Net.Search.IndexSearcher searcher = null;
            IndexReader reader = null;
            try
            {
                Lucene.Net.Store.Directory dir = FSDirectory.Open(new DirectoryInfo(this.IndexLocation));
                searcher = new IndexSearcher(dir, true);

                var oParser = new QueryParser(Lucene.Net.Util.Version.LUCENE_29, "Id", new StandardAnalyzer(Lucene.Net.Util.Version.LUCENE_29));
                Query query = oParser.Parse("102");

                TopDocs hitsTemp = searcher.Search(query, null, 1);
                int docNum = hitsTemp.scoreDocs[0].doc;

                reader = IndexReader.Open(dir, true);


                Similarity.Net.MoreLikeThis mlt = new Similarity.Net.MoreLikeThis(reader);
                mlt.SetFieldNames(fieldLucene);
                mlt.SetMinTermFreq(1);
                mlt.SetMinDocFreq(1);

                //Stream stream;

                //convert string to stream
                byte[] byteArray = Encoding.UTF8.GetBytes(text);
                MemoryStream stream = new MemoryStream(byteArray);
                Query q = mlt.Like(docNum);

                Hits hits = searcher.Search(q);

                if (hits != null)
                {
                    result = new List<T>();

                    for (int i = 0; i < top && i < hits.Length(); i++)
                    {
                        Document oDoc = hits.Doc(i);
                        result.Add(ConvertDocumentToItem(oDoc));
                    }
                }
            }
            catch (Exception ex)
            {
                Utility.LogEvent("Search error ", ex);
            }
            finally
            {
                if (searcher != null)
                {
                    searcher.Close();
                }
                if (reader != null)
                {
                    reader.Close();
                }
            }

            return result;
        }

        #region Utility Lucene
        private bool IsIndexExists(Lucene.Net.Store.Directory dir)
        {
            return IndexReader.IndexExists(dir);
        }

        private bool IsLooked(Lucene.Net.Store.Directory dir)
        {
            return IndexWriter.IsLocked(dir);
        }

        private void IndexListToLucene(IndexWriter writer, IDataReader reader)
        {
            if (reader != null)
            {
                while (reader.Read())
                {
                    IndexDocument(writer, reader);
                }
            }
        }

        private void IndexDocument(IndexWriter writer, IDataReader reader)
        {
            string colName = string.Empty;
            try
            {
                Document doc = new Document();
                for (var i = 0; i < reader.FieldCount; i++)
                {
                    colName = reader.GetName(i);
                    object valueCheckNull = AttrFromDb(reader, colName);
                    if (valueCheckNull == null)
                        continue;

                    var value = valueCheckNull.ToString();
                    value = System.Web.HttpUtility.HtmlDecode(value);
                    value = RemoveTagComment(value);
                    value = RemoveAllTag(value);
                    if (value != null)
                    {
                        PropertyInfo p = typeof(T).GetProperty(colName);
                        if (p == null)
                            continue;

                        object[] attCustom = p.GetCustomAttributes(typeof(AIFieldAttribute), true);
                        if (attCustom.Length > 0)
                        {
                            var objKey = attCustom[0] as AIFieldAttribute;
                            if (objKey != null)
                            {
                                doc.Add(new Field(colName, value.ToString(), objKey.FieldStoreLucene, objKey.FieldIndexLucene));
                                if (objKey.IsKoDau)
                                {
                                    string keyKoDau = colName + "_LONGLX";
                                    string valueKoDau = ConvertToKhongDau(value.ToString());
                                    doc.Add(new Field(keyKoDau, valueKoDau, objKey.FieldStoreLucene, objKey.FieldIndexLucene));
                                }
                            }
                        }
                    }
                }

                writer.AddDocument(doc);
            }
            catch (Exception ex) { Utility.LogEvent("Lucene index document error Column name: " + colName, ex); throw ex; }
        }

        private string RemoveAllTag(string str)
        {
            string strReturn = str.Replace("TEXT:", "");
            List<string> listTag = GetTag(str);
            if (listTag != null && listTag.Count > 0)
                foreach (string item in listTag)
                {
                    if (item.ToLower().Contains("<br"))
                        strReturn = strReturn.Replace(item, Environment.NewLine);
                    else if (item.ToLower().Contains("<p>"))
                        strReturn = strReturn.Replace(item, Environment.NewLine);
                    else if (item.ToLower().Contains("</p>"))
                        strReturn = strReturn.Replace(item, Environment.NewLine);
                    else if (strReturn.Contains(item))
                        strReturn = strReturn.Replace(item, "");
                }
            strReturn = strReturn.Replace(Environment.NewLine + Environment.NewLine + Environment.NewLine, Environment.NewLine + Environment.NewLine);
            return strReturn.Trim();
        }

        private List<string> GetTag(string str)
        {
            string strRegex = "<(?<tag>.*?)>";
            Match mt = (new Regex(strRegex)).Match(str);
            List<string> listTag = new List<string>();
            while (mt.Success)
            {
                listTag.Add(mt.Value);
                mt = mt.NextMatch();
            }
            if (listTag.Count > 0)
                return listTag;
            else
                return null;
        }

        private string RemoveTagComment(string str)
        {
            if (str != null && str.Trim() != string.Empty)
            {
                string strReturn = str;
                string strRegex = "<!--(?<tag>.*?)-->|<script.*[^CR]*?</script>|<SCRIPT.*[^CR]*?</SCRIPT>";
                Match mt = (new Regex(strRegex)).Match(str);
                while (mt.Success)
                {
                    strReturn = strReturn.Replace(mt.Value, "");
                    mt = mt.NextMatch();
                }
                return strReturn.Trim();
            }
            return string.Empty;
        }

        private void IndexDocumentFromEntity(IndexWriter writer, T item)
        {
            string colName = string.Empty;
            try
            {
                Document doc = new Document();

                PropertyInfo[] p = typeof(T).GetProperties();
                foreach (var pItem in p)
                {
                    object[] attCustom = pItem.GetCustomAttributes(typeof(AIFieldAttribute), true);
                    if (attCustom.Length > 0)
                    {
                        AIFieldAttribute objKey = attCustom[0] as AIFieldAttribute;
                        if (objKey != null)
                        {
                            string value = pItem.GetValue(item, null).ToString();
                            Field f = new Field(pItem.Name, pItem.GetValue(item, null).ToString(), objKey.FieldStoreLucene, objKey.FieldIndexLucene);
                            doc.Add(f);
                            if (objKey.IsKoDau)
                            {
                                string keyKoDau = colName + "_LONGLX";
                                string valueKoDau = ConvertToKhongDau(value.ToString());
                                doc.Add(new Field(keyKoDau, valueKoDau, objKey.FieldStoreLucene, objKey.FieldIndexLucene));
                            }
                        }
                    }
                }
                writer.AddDocument(doc);
            }
            catch (Exception ex) { Utility.LogEvent("Lucene index document error Column name: " + colName, ex); throw ex; }
        }

        private Document ConvertItemToDocument(T item)
        {
            string colName = string.Empty;
            Document doc = null;
            try
            {
                doc = new Document();

                PropertyInfo[] p = typeof(T).GetProperties();
                foreach (var pItem in p)
                {
                    object[] attCustom = pItem.GetCustomAttributes(typeof(AIFieldAttribute), true);

                    if (attCustom.Length > 0)
                    {
                        var objKey = attCustom[0] as AIFieldAttribute;
                        if (objKey != null)
                        {
                            string value = pItem.GetValue(item, null).ToString();
                            Field f = new Field(pItem.Name, value, objKey.FieldStoreLucene, objKey.FieldIndexLucene);
                            doc.Add(f);
                            if (objKey.IsKoDau)
                            {
                                string keyKoDau = colName + "_LONGLX";
                                string valueKoDau = ConvertToKhongDau(value.ToString());
                                doc.Add(new Field(keyKoDau, valueKoDau, objKey.FieldStoreLucene, objKey.FieldIndexLucene));
                            }
                        }
                    }
                }
            }
            catch (Exception ex) { Utility.LogEvent("Lucene error ConvertItemToDocument: " + colName, ex); throw ex; }
            return doc;
        }

        private T ConvertDocumentToItem(Document doc)
        {
            var item = new T();
            string colName = string.Empty;
            try
            {
                PropertyInfo[] properties = typeof(T).GetProperties();

                foreach (var p in properties)
                {
                    Field f = doc.GetField(p.Name);

                    if (f != null)
                    {
                        switch (Type.GetTypeCode(p.PropertyType))
                        {
                            case TypeCode.Boolean:
                                typeof(T).GetProperty(p.Name).SetValue(item, Convert.ToBoolean(f.StringValue()), null);
                                break;
                            case TypeCode.Byte:
                                typeof(T).GetProperty(p.Name).SetValue(item, Convert.ToByte(f.StringValue()), null);
                                break;
                            case TypeCode.Char:
                                typeof(T).GetProperty(p.Name).SetValue(item, Convert.ToChar(f.StringValue()), null);
                                break;
                            case TypeCode.DateTime:
                                typeof(T).GetProperty(p.Name).SetValue(item, Convert.ToDateTime(f.StringValue()), null);
                                break;
                            case TypeCode.Decimal:
                                typeof(T).GetProperty(p.Name).SetValue(item, Convert.ToDecimal(f.StringValue()), null);
                                break;
                            case TypeCode.Double:
                                typeof(T).GetProperty(p.Name).SetValue(item, Convert.ToDouble(f.StringValue()), null);
                                break;
                            case TypeCode.Int16:
                                typeof(T).GetProperty(p.Name).SetValue(item, Convert.ToInt16(f.StringValue()), null);
                                break;
                            case TypeCode.Int32:
                                typeof(T).GetProperty(p.Name).SetValue(item, Convert.ToInt32(f.StringValue()), null);
                                break;
                            case TypeCode.Int64:
                                typeof(T).GetProperty(p.Name).SetValue(item, Convert.ToInt64(f.StringValue()), null);
                                break;
                            case TypeCode.String:
                                typeof(T).GetProperty(p.Name).SetValue(item, f.StringValue(), null);
                                break;
                            default:
                                typeof(T).GetProperty(p.Name).SetValue(item, f.StringValue(), null);
                                break;
                        }
                    }
                }
            }
            catch (Exception ex) { Utility.LogEvent("Lucene convert to Entity error Column name: " + colName, ex); throw ex; }

            return item;
        }

        private T ConvertDocumentToItemHightLight(Document doc, Highlighter highlighter, Lucene.Net.Analysis.Standard.StandardAnalyzer analyzer)
        {
            T item = new T();
            string colName = string.Empty;
            try
            {
                PropertyInfo[] properties = typeof(T).GetProperties();
                foreach (PropertyInfo p in properties)
                {
                    Field f = doc.GetField(p.Name);

                    if (f != null)
                    {
                        string value = f.StringValue();
                        if (fieldLucene.Contains(p.Name))
                        {
                            object[] attCustom = p.GetCustomAttributes(typeof(AIFieldAttribute), true);

                            if (attCustom.Length > 0)
                            {
                                AIFieldAttribute objKey = attCustom[0] as AIFieldAttribute;
                                if (objKey != null)
                                {
                                    if (objKey.IsKoDau)
                                    {
                                        Lucene.Net.Analysis.TokenStream stream = analyzer.TokenStream("", new StringReader(doc.Get(p.Name + "_LONGLX")));
                                        string[] t = highlighter.GetBestFragments(stream, doc.Get(p.Name + "_LONGLX"), 10);
                                        string valueHightlight = highlighter.GetBestFragments(stream, doc.Get(p.Name + "_LONGLX"), 10, "");
                                        if (!string.IsNullOrEmpty(valueHightlight))
                                        {
                                            int indexStart = valueHightlight.IndexOf("<em>");
                                            int indexEnd = 0;

                                            while (indexStart != -1)
                                            {
                                                indexEnd = valueHightlight.IndexOf("</em>", indexStart);
                                                value = value.Insert(indexStart, "<em>");
                                                value = value.Insert(indexEnd, "</em>");
                                                indexStart = valueHightlight.IndexOf("<em>", indexStart + 1);
                                            }
                                        }
                                        else
                                            value = valueHightlight;
                                    }
                                    else
                                    {
                                        Lucene.Net.Analysis.TokenStream stream = analyzer.TokenStream("", new StringReader(doc.Get(p.Name)));
                                        value = highlighter.GetBestFragments(stream, doc.Get(p.Name), 1, "...");
                                        if (string.IsNullOrEmpty(value))
                                            value = f.StringValue();
                                    }
                                }
                            }
                        }
                        switch (Type.GetTypeCode(p.PropertyType))
                        {
                            case TypeCode.Boolean:
                                typeof(T).GetProperty(p.Name).SetValue(item, Convert.ToBoolean(value), null);
                                break;
                            case TypeCode.Byte:
                                typeof(T).GetProperty(p.Name).SetValue(item, Convert.ToByte(value), null);
                                break;
                            case TypeCode.Char:
                                typeof(T).GetProperty(p.Name).SetValue(item, Convert.ToChar(value), null);
                                break;
                            case TypeCode.DateTime:
                                typeof(T).GetProperty(p.Name).SetValue(item, Convert.ToDateTime(value), null);
                                break;
                            case TypeCode.Decimal:
                                typeof(T).GetProperty(p.Name).SetValue(item, Convert.ToDecimal(value), null);
                                break;
                            case TypeCode.Double:
                                typeof(T).GetProperty(p.Name).SetValue(item, Convert.ToDouble(value), null);
                                break;
                            case TypeCode.Int16:
                                typeof(T).GetProperty(p.Name).SetValue(item, Convert.ToInt16(value), null);
                                break;
                            case TypeCode.Int32:
                                typeof(T).GetProperty(p.Name).SetValue(item, Convert.ToInt32(value), null);
                                break;
                            case TypeCode.Int64:
                                typeof(T).GetProperty(p.Name).SetValue(item, Convert.ToInt64(value), null);
                                break;
                            case TypeCode.String:
                                typeof(T).GetProperty(p.Name).SetValue(item, value, null);
                                break;
                            default:
                                typeof(T).GetProperty(p.Name).SetValue(item, value, null);
                                break;
                        }
                    }
                }
            }
            catch (Exception ex) { Utility.LogEvent("Lucene convert to Entity error Column name: " + colName, ex); throw ex; }

            return item;
        }

        public string ConvertToKhongDau(string chucodau)
        {
            const string FindText = "áàảãạâấầẩẫậăắằẳẵặđéèẻẽẹêếềểễệíìỉĩịóòỏõọôốồổỗộơớờởỡợúùủũụưứừửữựýỳỷỹỵÁÀẢÃẠÂẤẦẨẪẬĂẮẰẲẴẶĐÉÈẺẼẸÊẾỀỂỄỆÍÌỈĨỊÓÒỎÕỌÔỐỒỔỖỘƠỚỜỞỠỢÚÙỦŨỤƯỨỪỬỮỰÝỲỶỸỴ";
            const string ReplText = "aaaaaaaaaaaaaaaaadeeeeeeeeeeeiiiiiooooooooooooooooouuuuuuuuuuuyyyyyAAAAAAAAAAAAAAAAADEEEEEEEEEEEIIIIIOOOOOOOOOOOOOOOOOUUUUUUUUUUUYYYYY";
            int index = -1;
            char[] arrChar = FindText.ToCharArray();
            while ((index = chucodau.IndexOfAny(arrChar)) != -1)
            {
                int index2 = FindText.IndexOf(chucodau[index]);
                chucodau = chucodau.Replace(chucodau[index], ReplText[index2]);
            }
            return chucodau;
        }
        #endregion
        #endregion
    }
}
