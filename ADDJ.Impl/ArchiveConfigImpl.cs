using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using ADDJ.Entity;
using ADDJ.Core;
using ADDJ.Core.Provider;

namespace ADDJ.Impl
{
    /// <summary>
    /// Class Insert, Update, Delete, Get dữ liệu của ArchiveConfig
    /// </summary>
    /// <author>Lê Xuân Long</author>
    /// <date>04/10/2013</date>

    public class ArchiveConfigImpl : BaseImpl<ArchiveConfigInfo>
    {
        protected override void SetInfoDerivedClass()
        {
            TableName = "ArchiveConfig";
        }

        private static Dictionary<int, ArchiveConfigInfo> _ListArchive;
        public static Dictionary<int, ArchiveConfigInfo> ListArchive
        {
            get
            {
                if (_ListArchive == null)
                {
                    var lst = new ArchiveConfigImpl().GetList();
                    if (lst != null && lst.Count > 0)
                    {
                        _ListArchive = new Dictionary<int, ArchiveConfigInfo>();

                        foreach (var item in lst)
                        {
                            _ListArchive.Add(item.Id, item);
                        }
                    }
                }
                return _ListArchive;
            }
            set
            {
                _ListArchive = value;
            }
        }

        public string BuildConnectionString(string server, string database, string username, string password)
        {
            string strConnection = "SERVER={0};UID={1};Password={2};database={3}";
            strConnection = string.Format(strConnection, server, username, password, database);

            return strConnection;
        }

        public bool CheckValidConnectionString(string connection)
        {
            SqlConnection conn = null;
            try
            {
                conn = new SqlConnection(connection);
                conn.Open();
                return true;
            }
            catch { return false; }
            finally
            {
                if (conn != null)
                    conn.Close();
            }

        }

        public static bool CheckExistsDataArchive(int archiveId)
        {
            try
            {
                if (ListArchive.ContainsKey(archiveId))
                {
                    var conn = ListArchive[archiveId].ConnectionString;

                    var lstCheck = new KhieuNaiImpl(conn).GetListDynamic("top 1 Id", "", "");

                    if (lstCheck != null && lstCheck.Count > 0)
                        return false;
                    return true;
                }
                return true;
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return false;
            }
        }

        #region Function

        #endregion
    }
}
