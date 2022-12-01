using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Website.HTHTKT;

namespace Website.HeThongHoTro.Actions
{
    public class ActionProcess
    {
        public static void GhiLog(Exception ex, string error_code)
        {
            try
            {
                using (var ctx = new ADDJContext())
                {
                    var stringSql = string.Format(@"INSERT INTO dbo.HT_LOG
                                                    (
                                                      ERROR_CODE
                                                     ,MESSAGE
                                                     ,FULL_MESSAGE
                                                     ,DATE_CREATE
                                                    )
                                                    VALUES
                                                    (
                                                      N'{0}' -- ERROR_CODE - nvarchar(500)
                                                     ,N'{1}' -- MESSAGE - nvarchar(500)
                                                     ,N'{2}' -- FULL_MESSAGE - nvarchar(4000)
                                                     ,getdate()
                                                    )", error_code != null ? error_code.Replace("'", "\"") : "", ex.Message != null ? ex.Message.ToString().Replace("'", "\"") : "", ex.InnerException != null ? ex.InnerException.ToString().Replace("'", "\"") : "");
                    ctx.Database.ExecuteSqlCommand(stringSql);
                }
            }
            catch (Exception eex)
            {

            }
        }
    }
}