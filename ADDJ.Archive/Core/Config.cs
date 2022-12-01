using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace GQKN.Archive.Core
{
    public class Config
    {
        public static bool TurnOnLog { get { return false; } }
        public static bool IsCommitSolr { get { return GetIsCommitSolr(); } }
        public static string ConnectionString { get { return GetConnectionString(); } }
        public static string PathIndexLucene { get { return GetPathIndexLucene(); } }
        public static string ServerSolr { get { return GetServerSolr(); } }
        public static int RecordPerPage { get { return GetRecordPerPage(); } }
        public static int MaxNumberArchive { get { return GetMaxNumberArchive(); } }
        public static int MaxNumberCommit2Solr { get { return GetMaxNumberCommit2Solr(); } }
        private static string GetConnectionString()
        {
            return ConfigurationManager.AppSettings["VinaphoneCCSOBConnectionstring"];
        }
        private static string GetServerSolr()
        {
            return ConfigurationManager.AppSettings["ServerSolr"];
        }
        private static int GetRecordPerPage()
        {
            return Convert.ToInt32(ConfigurationManager.AppSettings["RecordPerPage"]);
        }
        private static int GetMaxNumberArchive()
        {
            return Convert.ToInt32(ConfigurationManager.AppSettings["MaxNumberArchive"]);
        }
        private static int GetMaxNumberCommit2Solr()
        {
            return Convert.ToInt32(ConfigurationManager.AppSettings["MaxNumberCommit2Solr"]);
        }
        private static string GetPathIndexLucene()
        {
            return ConfigurationManager.AppSettings["PathIndexLucene"];
        }
        private static bool GetIsCommitSolr()
        {

            string isCommitSolr = ConfigurationManager.AppSettings["IsCommitSolr"];
            if (!string.IsNullOrEmpty(isCommitSolr)) return isCommitSolr.Equals("1");
            return false;
        }

    }
}
