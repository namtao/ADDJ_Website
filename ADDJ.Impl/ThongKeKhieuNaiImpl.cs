using ADDJ.Core;
using ADDJ.Core.Provider;
using ADDJ.Entity;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace ADDJ.Impl
{
    public class ThongKeKhieuNaiImpl: BaseImpl<ThongKeKhieuNaiEntity>
    {
        protected override void SetInfoDerivedClass()
        {
            TableName = "";
        }

        public int CountThongKeKhieuNai_TrangChu(int PhongBanId)
        {
            SqlParameter[] sqlParam = {
                                          new SqlParameter("PhongBanId", PhongBanId)                                         
                                      };

            var obj = this.ExecuteScalar("usp_KhieuNai_CountThongKeKhieuNai_TrangChu", sqlParam);
            if (obj != null)
            {
                return ConvertUtility.ToInt32(obj, -1);
            }
            return -1;
        }

        public List<ThongKeKhieuNaiEntity> GetThongKeKhieuNai(int PhongBanId)
        {
            SqlParameter[] sqlParam = {
                                          new SqlParameter("PhongBanId", PhongBanId)   
                                      };
            var lst = this.ExecuteQuery("usp_KhieuNai_GetThongKeKhieuNai", sqlParam);
            return lst;
        }
    }
}
