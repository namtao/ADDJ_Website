using ADDJ.Core;
using ADDJ.Core.Provider;
using ADDJ.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace ADDJ.Impl
{
    public class NguoiSuDung_GioiHanGiamTruImpl : BaseImpl<NguoiSuDung_GioiHanGiamTruInfo>
    {
        protected override void SetInfoDerivedClass()
        {
            TableName = "NguoiSuDung_GioiHanGiamTru";           
        }
        
        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created date : 21/11/2015
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public DataSet GetInfoGioiHanGiamTruOfUser(int userId)
        {
            DataSet dsLogin = null;
            try
            {
                string sql = string.Format(@"SELECT DoiTac.* FROM DoiTac
                                            INNER JOIN PhongBan ON DoiTac.Id = PhongBan.DoiTacId
                                            INNER JOIN PhongBan_User ON PhongBan.Id = PhongBan_User.PhongBanId
                                            WHERE PhongBan_User.NguoiSuDungId = @UserId;
                                            SELECT * FROM NguoiSuDung_GioiHanGiamTru WHERE UserId=@UserId AND IsDeleted=0;");
                SqlParameter[] param = { new SqlParameter("@UserId", userId) };
                dsLogin = this.ExecuteQueryToDataSet(sql, CommandType.Text, param);
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex.Message);
            }

            return dsLogin;

        }
    }
}
