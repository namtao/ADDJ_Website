using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using ADDJ.Entity;
using ADDJ.Core;
using ADDJ.Core.Provider;
using System.Transactions;

namespace ADDJ.Impl
{
	/// <summary>
    /// Class Insert, Update, Delete, Get dữ liệu của LoaiPhongBan_ThoiGianXuLyKhieuNai
    /// </summary>
    /// <author>Lê Xuân Long</author>
    /// <date>19/08/2013</date>
	
	public class LoaiPhongBan_ThoiGianXuLyKhieuNaiImpl : BaseImpl<LoaiPhongBan_ThoiGianXuLyKhieuNaiInfo>
    {
        protected override void SetInfoDerivedClass()
        {
            TableName = "LoaiPhongBan_ThoiGianXuLyKhieuNai";
        }

        private static List<LoaiPhongBan_ThoiGianXuLyKhieuNaiInfo> _ListThoiGianXuLyPhongBan;
        public static List<LoaiPhongBan_ThoiGianXuLyKhieuNaiInfo> ListThoiGianXuLyPhongBan
        {
            get
            {
                if (_ListThoiGianXuLyPhongBan == null)
                    _ListThoiGianXuLyPhongBan = new LoaiPhongBan_ThoiGianXuLyKhieuNaiImpl().GetList();
                return _ListThoiGianXuLyPhongBan;
            }
            set { _ListThoiGianXuLyPhongBan = value; }
        }

		#region Function 
		
        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created date : 13/09/2014
        /// </summary>
        /// <returns></returns>
        /// <param name="loaiPhongBanId"></param>
        /// <param name="thoiGianXuLy"></param>
        /// <param name="loaiDuLieu"></param>
        /// <returns>
        ///     = 1 : Thành công
        ///     != 1 : Không thành công
        /// </returns>
        public int ResetTime(int loaiPhongBanId, string thoiGianXuLyMacDinh, string loaiDuLieu)
        {            
            string updateClause = string.Empty;
            string whereClause = string.Empty;
           
            if (loaiDuLieu == "1")
            {
                updateClause = string.Format("ThoiGianCanhBao={0}, ThoiGianUocTinh={0}", thoiGianXuLyMacDinh);
                whereClause = string.Format("LoaiPhongBanId={0}", loaiPhongBanId);

                this.UpdateDynamic(updateClause, whereClause);
            }
            else if (loaiDuLieu == "2")
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    try
                    {
                        for (int i = 0; i < LoaiKhieuNaiImpl.ListLoaiKhieuNai.Count; i++)
                        {
                            string thoiGianXuLyKhieuNai = LoaiKhieuNaiImpl.ListLoaiKhieuNai[i].ThoiGianUocTinh;
                            string day = string.Empty;
                            string hour = string.Empty;

                            if (thoiGianXuLyKhieuNai.ToLower().Contains("d"))
                            {
                                day = thoiGianXuLyKhieuNai.Substring(0, thoiGianXuLyKhieuNai.IndexOf('d'));
                            }

                            if (thoiGianXuLyKhieuNai.ToLower().Contains("h"))
                            {
                                thoiGianXuLyKhieuNai = thoiGianXuLyKhieuNai.Substring(0, thoiGianXuLyKhieuNai.IndexOf('d') + 1);
                                hour = thoiGianXuLyKhieuNai.Substring(0, thoiGianXuLyKhieuNai.IndexOf('h'));
                            }

                            int totalHour = ConvertUtility.ToInt32(day, 0) * 24 + ConvertUtility.ToInt32(hour, 0);

                            int percent = ConvertUtility.ToInt32(thoiGianXuLyMacDinh, 0);

                            int retHour = totalHour * percent / 100;

                            string result = string.Empty;
                            if (retHour >= 24)
                            {
                                if(retHour % 24 == 0)
                                {
                                    result = string.Format("{0}d", retHour / 24);
                                }
                                else
                                {
                                    result = string.Format("{0}d{1}h", retHour / 24, retHour % 24);
                                }
                                
                            }
                            else
                            {
                                result = string.Format("{0}h", retHour);
                            }

                            updateClause = string.Format("ThoiGianCanhBao='{0}', ThoiGianUocTinh='{0}'", result);
                            whereClause = string.Format("LoaiPhongBanId={0} AND LoaiKhieuNaiId = {1}", loaiPhongBanId, LoaiKhieuNaiImpl.ListLoaiKhieuNai[i].Id);
                            this.UpdateDynamic(updateClause, whereClause);
                            
                        } // end for (int i = 0; i < LoaiKhieuNaiImpl.ListLoaiKhieuNai.Count; i++)

                        scope.Complete();
                    }
                    catch
                    {
                        return -1;
                    }
                }
                    
            } // end else if (loaiDuLieu == "2")           
            
            return 1;
        }

		#endregion
    }
}
