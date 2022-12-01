using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using ADDJ.Core;
using ADDJ.Core.Provider;
using System.Threading;
using System.Web;
using ADDJ.Log.Entity;
using ADDJ.Admin;

namespace ADDJ.Log.Impl
{
    /// <summary>
    /// Class Insert, Update, Delete, Get dữ liệu của Log
    /// </summary>
    /// <author>Lê Xuân Long</author>
    /// <date>09/11/2012</date>

    public class LogImpl : BaseImpl<LogInfo>
    {
        public static LogImpl logObj;
        protected override void SetInfoDerivedClass()
        {
            TableName = "Log";
        }

        #region Function

        public static void Log(int objId, ObjTypeLog type, string objName, string note, ActionLog action)
        {
            AdminInfo userInfo = LoginAdmin.AdminLogin();
            if (userInfo != null)
            {
                Thread thread = new Thread(() =>
                {
                    try
                    {
                        if (logObj == null) logObj = new LogImpl();

                        string ipUser = Utility.GetIP();

                        LogInfo logInfo = new LogInfo();
                        logInfo.ObjId = objId;
                        logInfo.ObjType = (int)type;
                        logInfo.ObjName = objName;
                        logInfo.Note = note;
                        logInfo.Ip = ipUser;
                        logInfo.UserId = userInfo.Id;
                        logInfo.Username = userInfo.Username;
                        logInfo.Action = (int)action;
                        logInfo.DateCreate = Convert.ToInt32(DateTime.Now.ToString("yyyyMMdd"));
                        logInfo.TimeCreate = Convert.ToInt32(DateTime.Now.ToString("HHmm"));

                        logObj.Add(logInfo);
                    }
                    catch (Exception ex)
                    {
                        Utility.LogEvent(ex);
                    }
                });
                thread.Start();
            }
            else
            {
                Utility.LogEvent("Không tìm thấy User tương tác để ghi log");
            }
        }

        public static void Log(int objId, ObjTypeLog type, string objName, ActionLog action)
        {
            Log(objId, type, objName, string.Empty, action);
        }

        public static void Log(int objId, ObjTypeLog type, ActionLog action)
        {
            Log(objId, type, string.Empty, string.Empty, action);
        }

        public static void Log(ObjTypeLog type, ActionLog action)
        {
            Log(0, type, string.Empty, string.Empty, action);
        }

        public static void Log(ObjTypeLog type, ActionLog action, string note)
        {
            Log(0, type, string.Empty, note, action);
        }

        public static void Log(ActionLog action)
        {
            Log(0, ObjTypeLog.System, string.Empty, string.Empty, action);
        }

        #endregion
    }
}