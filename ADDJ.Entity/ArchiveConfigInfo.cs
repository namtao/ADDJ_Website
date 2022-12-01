using System;

namespace ADDJ.Entity
{
    /// <summary>
    /// Class Mapping table ArchiveConfig in Databasse
    /// </summary>
    /// <author>Lê Xuân Long</author>
    /// <date>07/10/2013</date>

    [Serializable]
    public class ArchiveConfigInfo
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int NamLuuTru { get; set; }
        public string ServerName { get; set; }
        public string DatabaseName { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string ConnectionString { get; set; }
        public string PathFileSystem { get; set; }
        public string PathUrlFile { get; set; }
        public bool IsCurrent { get; set; }


        public ArchiveConfigInfo()
        {
            Id = 0;
            Name = string.Empty;
            Description = string.Empty;
            NamLuuTru = 0;
            ServerName = string.Empty;
            DatabaseName = string.Empty;
            Username = string.Empty;
            Password = string.Empty;
            ConnectionString = string.Empty;
            PathFileSystem = string.Empty;
            PathUrlFile = string.Empty;
            IsCurrent = false;

        }

    }
}