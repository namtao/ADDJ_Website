using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Data.SqlClient;
using ADDJ.Entity;
using ADDJ.Core;
using ADDJ.Core.Provider;
using ADDJ.Admin;
using System.IO;
using Lucene.Net.QueryParsers;
using Lucene.Net.Analysis.Standard;
using System.Globalization;

namespace ADDJ.Impl
{
    /// <summary>
    /// Class Insert, Update, Delete, Get dữ liệu của NguoiSuDung
    /// </summary>
    /// <author>Lê Xuân Long</author>
    /// <date>02/08/2013</date>

    public class NguoiSuDungImpl : BaseImpl<NguoiSuDungInfo>
    {
        protected override void SetInfoDerivedClass()
        {
            TableName = "NguoiSuDung";

            IndexLocation = Path.Combine(Config.PathIndexLucene, TableName) + @"\";
            MaxFieldLength = 10;
            IsUseLucene = true;
            IsUpdateLucene = true;
        }

        #region Autocomplete
        public List<NguoiSuDungInfo> Suggestion(int KhuVucId, int DoiTacId, int NhomNguoiDung, string query)
        {
            string[] fields = new string[] { "TenTruyCap", "TenDayDu" };

            var parser = new MultiFieldQueryParser(Lucene.Net.Util.Version.LUCENE_29, fields, new StandardAnalyzer(Lucene.Net.Util.Version.LUCENE_29));
            parser.SetAllowLeadingWildcard(true);
            parser.SetDefaultOperator(QueryParser.Operator.OR);

            query = query + "* ";
            if (NhomNguoiDung > 2)
            {
                query += " && KhuVucId:" + KhuVucId;
                if (DoiTacId != KhuVucId)
                    query += " && DoiTacId:" + DoiTacId;
            }

            var q = parser.Parse(query);

            return this.Search(q, null, null, 10, false);
        }
        public List<NguoiSuDungInfo> Suggestion(string query)
        {
            string[] fields = new string[] { "TenTruyCap", "TenDayDu" };

            var parser = new MultiFieldQueryParser(Lucene.Net.Util.Version.LUCENE_29, fields, new StandardAnalyzer(Lucene.Net.Util.Version.LUCENE_29));
            parser.SetAllowLeadingWildcard(true);
            parser.SetDefaultOperator(QueryParser.Operator.OR);

            query = query + "* ";

            var q = parser.Parse(query);

            List<NguoiSuDungInfo> list = this.Search(q, null, null, int.MaxValue, false);
            var newList = list.OrderBy(x => x.TenTruyCap).ToList();
            return newList;
        }
       
        #endregion

        #region Function

        public int UpdateStatus(int id, int status)
        {
            string fieldUpdate = string.Format("LDate = getdate(), LUser = '{1}', TrangThai = {0}", status, LoginAdmin.AdminLogin().Username);
            string whereClause = string.Format("Id={0}", id);

            return this.UpdateDynamic(fieldUpdate, whereClause);
        }

        public bool CheckExists(string username)
        {
            try
            {
                SqlParameter[] param = {
										new SqlParameter("@Username",username),
                                   };

                object obj = ExecuteScalar("usp_NguoiSuDung_CheckExists", param);
                if (obj == null)
                    return false;
                return true;
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return false;
            }
        }

        public int GetIdByUsername(string username)
        {
            try
            {
                SqlParameter[] param = {
										new SqlParameter("@Username",username),
                                   };
                object obj = ExecuteScalar("usp_NguoiSuDung_CheckExists", param);
                if (obj == null)
                    return 0;
                return int.Parse(obj.ToString());
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return 0;
            }
        }

        public List<NguoiSuDungInfo> NguoiSuDung_GetInfoNguoiSuDungByTenTruyCap(string _TenTruyCap)
        {
            List<NguoiSuDungInfo> list = null;
            SqlParameter[] param = {
										new SqlParameter("@TenTruyCap",_TenTruyCap),
									};
            try
            {
                list = ExecuteQuery("usp_NguoiSuDung_GetInfoNguoiSuDungByTenTruyCap", param);
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
            }
            return list;

        }

        public List<NguoiSuDungInfo> GetListNguoiSuDungByPhongBanId(int _PhongBanId)
        {
            List<NguoiSuDungInfo> list = null;
            SqlParameter[] param = {
										new SqlParameter("@PhongBanId",_PhongBanId),
									};
            try
            {
                list = ExecuteQuery("usp_NguoiSuDung_GetListNguoiSuDungByPhongBanId", param);
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
            }
            return list;

        }
        public DataTable GetListNguoiSuDungByPhanViecPhongBanId(int _PhongBanId)
        {
            int TrangThai1 = (int)KhieuNai_TrangThai_Type.Chờ_xử_lý;
            int TrangThai2 = (int)KhieuNai_TrangThai_Type.Đang_xử_lý;
            int TrangThai3 = (int)KhieuNai_TrangThai_Type.Chờ_đóng;
            SqlParameter[] sqlParam = {
                                           new SqlParameter("@PhongBanId",_PhongBanId),
                                           new SqlParameter("TrangThai1", TrangThai1),
		                                    new SqlParameter("TrangThai2", TrangThai2),
                                            new SqlParameter("TrangThai3", TrangThai3),
                                      };
            DataSet dt = this.ExecuteQueryToDataSet("usp_NguoiSuDung_GetListNguoiSuDungPhanViec", sqlParam);
            DataTable tabReturn = dt.Tables[0];
            return tabReturn;

        }
        #endregion

        private static void CheckExists(string username, NguoiSuDungImpl objImpl, ref object obj)
        {
            try
            {
                SqlParameter[] param = {
										new SqlParameter("@Username",username),
                                   };
                obj = objImpl.ExecuteScalar("usp_NguoiSuDung_CheckExists", param);
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                //return false;
            }
        }

        private static int GetKhuVuc(int DoiTacId, NguoiSuDungImpl objImpl, ref string DoiTacName)
        {
            int result = -1;
            try
            {
                SqlParameter[] param = {
										new SqlParameter("@DoiTacId",DoiTacId),
                                   };
                DataTable dt = objImpl.ExecuteQueryToDataSet("usp_DoiTac_GetKhuVucByDoiTac", param).Tables[0];
                if (dt.Rows.Count > 0)
                {
                    result = Convert.ToInt32(dt.Rows[0]["DonViTrucThuoc"]);
                    DoiTacName = dt.Rows[0]["TenDoiTac"].ToString();
                }
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                //return false;
            }
            return result;
        }


        public static bool UpdateProfileCrossSell(string username, int doitacId, string fullname, string codinh, string didong, string email, string address, string ngaysinh, byte sex, byte trangthai, string LUser)
        {
            bool flag = false;
            var objImpl = new NguoiSuDungImpl();
            object id = null;
            CheckExists(username, objImpl, ref id);
            string DoiTacName = string.Empty;

            int KhuVucId = GetKhuVuc(doitacId, objImpl, ref DoiTacName);
            if (KhuVucId == -1)
                throw new Exception("Không tìm thấy đối tác có mã:" + doitacId + " trong hệ thống GQKN");

            if (KhuVucId == 0)
                KhuVucId = 1;
            //Neu ton tai thi update
            if (id != null)
            {
                try
                {
                    //var item = new NguoiSuDungInfo();
                    var item = objImpl.GetInfo(Convert.ToInt32(id));
                    if (item == null)
                        return false;
                    //item.Id = Convert.ToInt32(id);
                    //item.TenTruyCap = username;
                    item.DoiTacId = doitacId;
                    item.KhuVucId = KhuVucId;
                    item.TenDoiTac = DoiTacName;
                    item.TenDayDu = fullname;
                    item.CoDinh = codinh;
                    item.DiDong = didong;
                    item.Email = email;
                    item.DiaChi = address;
                    item.NgaySinh = ConvertUtility.ToDateTime(ngaysinh, new CultureInfo("vi-VN"));
                    item.Sex = sex;
                    item.TrangThai = trangthai;
                    item.LUser = LUser;

                    if (objImpl.Update(item) > 0)
                        flag = true;
                }
                catch (Exception ex)
                {
                    Utility.LogEvent("UpdateProfileCrossSell > Update", ex);
                }
            }
            else  //Neu khong thi insert
            {
                try
                {
                    var item = new NguoiSuDungInfo();
                    //var item = objImpl.GetInfo(Convert.ToInt32(id));
                    //item.Id = Convert.ToInt32(id);
                    item.TenTruyCap = username;
                    item.DoiTacId = doitacId;
                    item.KhuVucId = KhuVucId;
                    item.TenDoiTac = DoiTacName;
                    item.TenDayDu = fullname;
                    item.CoDinh = codinh;
                    item.DiDong = didong;
                    item.Email = email;
                    item.DiaChi = address;

                    item.NgaySinh = ConvertUtility.ToDateTime(ngaysinh, new CultureInfo("vi-VN"));
                    item.Sex = sex;
                    item.TrangThai = trangthai;
                    item.CUser = LUser;
                    item.LUser = LUser;

                    //Thong tin mac dinh
                    item.MatKhau = "e10adc3949ba59abbe56e057f20f883e";
                    item.NhomNguoiDung = 5;
                    //item.TrangThai = 1;

                    if (objImpl.Add(item) > 0)
                        flag = true;
                }
                catch (Exception ex)
                {
                    Utility.LogEvent("UpdateProfileCrossSell > Insert", ex);
                }
            }

            return flag;
        }
    }
}
