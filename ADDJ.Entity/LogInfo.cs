using System;

namespace ADDJ.Log.Entity
{
    /// <summary>
    /// Class Mapping table Log in Databasse
    /// </summary>
    /// <author>Lê Xuân Long</author>
    /// <date>09/11/2012</date>

    [Serializable]
    public class LogInfo
    {
        public Int64 Id { get; set; }
        public int ObjId { get; set; }
        public int ObjType { get; set; }
        public string ObjName { get; set; }
        public string Note { get; set; }
        public string Ip { get; set; }
        public int UserId { get; set; }
        public string Username { get; set; }
        public int Action { get; set; }
        public int DateCreate { get; set; }
        public int TimeCreate { get; set; }


        public LogInfo()
        {
            Id = 0;
            ObjId = 0;
            ObjType = 0;
            ObjName = string.Empty;
            Note = string.Empty;
            Ip = string.Empty;
            Username = string.Empty;
            UserId = 0;
            Action = 0;
            DateCreate = 0;
            TimeCreate = 0;

        }
    }
}