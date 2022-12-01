using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.SqlClient;
using ADDJ.Entity;
using ADDJ.Core;
using ADDJ.Core.Provider;
using System.Linq;
using System.Web;
using System.Text;
using System.IO;
using Lucene.Net.QueryParsers;
using Lucene.Net.Analysis.Standard;

namespace ADDJ.Impl
{
    /// <summary>
    /// Class Insert, Update, Delete, Get dữ liệu của DoiTac
    /// </summary>
    /// <author>Lê Xuân Long</author>
    /// <date>02/08/2013</date>

    public class DoiTacImpl : BaseImpl<DoiTacInfo>
    {
        public DoiTacImpl()
            : base()
        {

        }

        /// <summary>
        /// Author: MarkNguyen
        /// Created on: 19/02/2014 11:31
        /// Description: Initializes a new instance of the <see cref="DoiTacImpl"/> class.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        public DoiTacImpl(string connectionString)
            : base(connectionString)
        {

        }

        protected override void SetInfoDerivedClass()
        {
            TableName = "DoiTac";
            IndexLocation = Path.Combine(Config.PathIndexLucene, TableName) + @"\";
            MaxFieldLength = 10;
            IsUseLucene = true;
            IsUpdateLucene = true;
        }

        public List<DoiTacInfo> Suggestion(string query)
        {
            string[] fields = new string[] { "Id", "TenDoiTac" };

            var parser = new MultiFieldQueryParser(Lucene.Net.Util.Version.LUCENE_29, fields, new StandardAnalyzer(Lucene.Net.Util.Version.LUCENE_29));
            parser.SetAllowLeadingWildcard(true);
            parser.SetDefaultOperator(QueryParser.Operator.OR);

            query = query + "*";

            var q = parser.Parse(query);

            return this.Search(q, null, null, 10, false);
        }

        public List<DoiTacInfo> SuggestionGetAllList(string query)
        {
            string[] fields = new string[] { "Id", "TenDoiTac" };

            var parser = new MultiFieldQueryParser(Lucene.Net.Util.Version.LUCENE_29, fields, new StandardAnalyzer(Lucene.Net.Util.Version.LUCENE_29));
            parser.SetAllowLeadingWildcard(true);
            parser.SetDefaultOperator(QueryParser.Operator.OR);

            query = query + "*";

            var q = parser.Parse(query);

            List<DoiTacInfo> list = this.Search(q, null, null, int.MaxValue, false);
            var newList = list.OrderBy(x => x.TenDoiTac).ToList();
            return newList;
        }

        public int add_ccos(DoiTacInfo item)
        {
            var commandText = new StringBuilder("usp_" + TableName + "_Add_CCOS");
            //var dsCmd = new SqlCommand();
            var result = 0;
            try
            {
                //conn.Open();
                SqlParameter[] param = {
                                        new SqlParameter("@Id",item.Id),
                                        new SqlParameter("@DonViTrucThuoc",item.DonViTrucThuoc),
                                        new SqlParameter("@ProvinceId",item.ProvinceId),
                                        new SqlParameter("@ProvinceHuyenId",item.ProvinceHuyenId),
                                        new SqlParameter("@MaDoiTac",item.MaDoiTac),
                                        new SqlParameter("@TenDoiTac",item.TenDoiTac),
                                        new SqlParameter("@DienThoai",item.DienThoai),
                                        new SqlParameter("@MaSoThue",item.MaSoThue),
                                        new SqlParameter("@Fax",item.Fax),
                                        new SqlParameter("@DiaChi",item.DiaChi),
                                        new SqlParameter("@Website",item.Website),
                                        new SqlParameter("@NguoiDaiDien",item.NguoiDaiDien),
                                        new SqlParameter("@ChucVu",item.ChucVu),
                                        new SqlParameter("@LoaiGiayTo", item.LoaiGiayTo),
                                        new SqlParameter("@NgayCap",item.NgayCap),
                                        new SqlParameter("@NoiCap",item.NoiCap),
                                        new SqlParameter("@SoGiayTo",item.SoGiayTo),
                                        new SqlParameter("@SoHopDong",item.SoHopDong),
                                        new SqlParameter("@NgayHopDong", item.NgayHopDong),
                                        new SqlParameter("@GhiChu",item.GhiChu),
                                        new SqlParameter("@Sort",item.Sort),
                                        new SqlParameter("@LUser",item.LUser),
                                    };


                //insert item vào db
                var obj = base.ExecuteScalar(commandText.ToString(), param);
                if (obj != null)
                {
                    result = Convert.ToInt32(obj);
                }
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
            }

            return result;
        }


        public int update_ccos(DoiTacInfo item)
        {
            var commandText = new StringBuilder("usp_" + TableName + "_Update_CCOS");
            //var dsCmd = new SqlCommand();
            var result = 0;
            try
            {
                //conn.Open();
                SqlParameter[] param = {
                                        new SqlParameter("@Id",item.Id),
                                        new SqlParameter("@DonViTrucThuoc",item.DonViTrucThuoc),
                                        new SqlParameter("@ProvinceId",item.ProvinceId),
                                        new SqlParameter("@ProvinceHuyenId",item.ProvinceHuyenId),
                                        new SqlParameter("@MaDoiTac",item.MaDoiTac),
                                        new SqlParameter("@TenDoiTac",item.TenDoiTac),
                                        new SqlParameter("@DienThoai",item.DienThoai),
                                        new SqlParameter("@MaSoThue",item.MaSoThue),
                                        new SqlParameter("@Fax",item.Fax),
                                        new SqlParameter("@DiaChi",item.DiaChi),
                                        new SqlParameter("@Website",item.Website),
                                        new SqlParameter("@NguoiDaiDien",item.NguoiDaiDien),
                                        new SqlParameter("@ChucVu",item.ChucVu),
                                        new SqlParameter("@LoaiGiayTo", item.LoaiGiayTo),
                                        new SqlParameter("@NgayCap",item.NgayCap),
                                        new SqlParameter("@NoiCap",item.NoiCap),
                                        new SqlParameter("@SoGiayTo",item.SoGiayTo),
                                        new SqlParameter("@SoHopDong",item.SoHopDong),
                                        new SqlParameter("@NgayHopDong", item.NgayHopDong),
                                        new SqlParameter("@GhiChu",item.GhiChu),
                                        new SqlParameter("@Sort",item.Sort),
                                        new SqlParameter("@LUser",item.LUser),
                                    };


                //insert item vào db
                var obj = base.ExecuteNonQuery(commandText.ToString(), param);
                if (obj != null)
                {
                    result = Convert.ToInt32(obj);
                }
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
            }

            return result;
        }

        public int delete_ccos(int Id)
        {
            var commandText = new StringBuilder("usp_" + TableName + "_Delete_CCOS");
            //var dsCmd = new SqlCommand();
            var result = 0;
            try
            {
                //conn.Open();
                SqlParameter[] param = {
                                        new SqlParameter("@Id",Id),
                                    };


                //insert item vào db
                var obj = base.ExecuteNonQuery(commandText.ToString(), param);
                if (obj != null)
                {
                    result = Convert.ToInt32(obj);
                }
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
            }

            return result;
        }

        private static List<DoiTacInfo> _lstDoiTac;

        public static List<DoiTacInfo> ListDoiTac
        {
            get
            {
                if (_lstDoiTac == null)
                    _lstDoiTac = new DoiTacImpl().GetList();
                return _lstDoiTac;
            }
            set
            {
                _lstDoiTac = value;
            }
        }

        #region Function 

        public List<DoiTacInfo> GetListByDonViTrucThuoc(int _donViTrucThuoc)
        {
            List<DoiTacInfo> list = null;
            SqlParameter[] param = {
                                        new SqlParameter("@DonViTrucThuoc",_donViTrucThuoc),
                                    };
            try
            {
                list = ExecuteQuery("usp_DoiTac_GetListByDonViTrucThuoc", param);
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
            }
            return list;

        }

        public static List<DoiTacInfo> BuildTree(List<DoiTacInfo> lst, int pId, int cap, string replaceSpace)
        {
            List<DoiTacInfo> lstRet = new List<DoiTacInfo>();
            var lstPar = lst.Where(t => t.DonViTrucThuoc == pId).ToList<DoiTacInfo>();
            string strSpace = string.Empty;

            for (int i = 0; i < cap; i++)
                strSpace += replaceSpace;

            foreach (var item in lstPar)
            {
                item.MaDoiTac = HttpUtility.HtmlDecode(strSpace + item.MaDoiTac);
                lstRet.Add(item);
                var q = BuildTree(lst, item.Id, cap + 1, replaceSpace);
                if (q.Count > 0)
                {
                    foreach (var i in q)
                    {
                        i.MaDoiTac = i.MaDoiTac;
                        lstRet.Add(i);
                    }
                }
            }
            return lstRet;
        }

        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created date : 07/04/2014
        /// Todo : Lấy toàn bộ tree (con, cháu, ...) của doiTacId truyền vào
        /// </summary>
        /// <param name="donViTrucThuocId"></param>
        /// <returns></returns>
        public List<DoiTacInfo> GetTreeByDonViTrucThuoc(int doiTacId)
        {
            List<DoiTacInfo> list = null;
            SqlParameter[] param = {
                                        new SqlParameter("@DoiTacId", doiTacId),
                                    };
            try
            {
                list = ExecuteQuery("usp_DoiTac_GetTreeByDonViTrucThuoc", param);
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
            }
            return list;
        }

        public DoiTacInfo GetVNPTTByTruongDaiDienId(int truongDaiDienId, List<DoiTacInfo> listDoiTac)
        {
            DoiTacInfo objResult = null;
            switch (truongDaiDienId)
            {
                case 10119: // Bắc Giang
                    objResult = listDoiTac.Find(delegate (DoiTacInfo obj) { return obj.Id == 10035; });
                    break;
                case 10120: // Bắc Ninh
                    objResult = listDoiTac.Find(delegate (DoiTacInfo obj) { return obj.Id == 10036; });
                    break;
                case 10121: // Bắc Cạn
                    objResult = listDoiTac.Find(delegate (DoiTacInfo obj) { return obj.Id == 10037; });
                    break;
                case 10122: // Cao Bằng
                    objResult = listDoiTac.Find(delegate (DoiTacInfo obj) { return obj.Id == 10038; });
                    break;
                case 10123: // Điện Biên
                    objResult = listDoiTac.Find(delegate (DoiTacInfo obj) { return obj.Id == 10039; });
                    break;
                case 10124: // Lai Châu
                    objResult = listDoiTac.Find(delegate (DoiTacInfo obj) { return obj.Id == 10040; });
                    break;
                case 10125: // Hà Nam
                    objResult = listDoiTac.Find(delegate (DoiTacInfo obj) { return obj.Id == 10041; });
                    break;
                case 10127: // Hà Tĩnh
                    objResult = listDoiTac.Find(delegate (DoiTacInfo obj) { return obj.Id == 10043; });
                    break;
                case 10128: // Hải Dương
                    objResult = listDoiTac.Find(delegate (DoiTacInfo obj) { return obj.Id == 10044; });
                    break;
                case 10129: // Hải Phòng
                    objResult = listDoiTac.Find(delegate (DoiTacInfo obj) { return obj.Id == 10045; });
                    break;
                case 10130: // Hòa Bình
                    objResult = listDoiTac.Find(delegate (DoiTacInfo obj) { return obj.Id == 10046; });
                    break;
                case 10131: // Hưng Yên
                    objResult = listDoiTac.Find(delegate (DoiTacInfo obj) { return obj.Id == 10047; });
                    break;
                case 10132: // Lạng Sơn
                    objResult = listDoiTac.Find(delegate (DoiTacInfo obj) { return obj.Id == 10048; });
                    break;
                case 10133: // Lào Cai
                    objResult = listDoiTac.Find(delegate (DoiTacInfo obj) { return obj.Id == 10049; });
                    break;
                case 10134: // Nam Định
                    objResult = listDoiTac.Find(delegate (DoiTacInfo obj) { return obj.Id == 10050; });
                    break;
                case 10135: // Nghệ An
                    objResult = listDoiTac.Find(delegate (DoiTacInfo obj) { return obj.Id == 10051; });
                    break;
                case 10136: // Ninh Bình
                    objResult = listDoiTac.Find(delegate (DoiTacInfo obj) { return obj.Id == 10052; });
                    break;
                case 10137: // Quảng Ninh
                    objResult = listDoiTac.Find(delegate (DoiTacInfo obj) { return obj.Id == 10053; });
                    break;
                case 10138: // Phú Thọ
                    objResult = listDoiTac.Find(delegate (DoiTacInfo obj) { return obj.Id == 10054; });
                    break;
                case 10139: // Sơn La
                    objResult = listDoiTac.Find(delegate (DoiTacInfo obj) { return obj.Id == 10055; });
                    break;
                case 10140: // Thanh Hóa
                    objResult = listDoiTac.Find(delegate (DoiTacInfo obj) { return obj.Id == 10056; });
                    break;
                case 10141: // Thái Bình
                    objResult = listDoiTac.Find(delegate (DoiTacInfo obj) { return obj.Id == 10057; });
                    break;
                case 10142: // Thái Nguyên
                    objResult = listDoiTac.Find(delegate (DoiTacInfo obj) { return obj.Id == 10058; });
                    break;
                case 10143: // Tuyên Quang
                    objResult = listDoiTac.Find(delegate (DoiTacInfo obj) { return obj.Id == 10059; });
                    break;
                case 10144: // Vĩnh Phúc
                    objResult = listDoiTac.Find(delegate (DoiTacInfo obj) { return obj.Id == 10060; });
                    break;
                case 10145: // Yên Bái
                    objResult = listDoiTac.Find(delegate (DoiTacInfo obj) { return obj.Id == 10061; });
                    break;
                case 10147: // An Giang
                    objResult = listDoiTac.Find(delegate (DoiTacInfo obj) { return obj.Id == 10077; });
                    break;
                case 10148: // Bình Dương
                    objResult = listDoiTac.Find(delegate (DoiTacInfo obj) { return obj.Id == 10078; });
                    break;
                case 10149:
                    objResult = listDoiTac.Find(delegate (DoiTacInfo obj) { return obj.Id == 10079; });
                    break;
                case 10150:
                    objResult = listDoiTac.Find(delegate (DoiTacInfo obj) { return obj.Id == 10080; });
                    break;
                case 10151:
                    objResult = listDoiTac.Find(delegate (DoiTacInfo obj) { return obj.Id == 10081; });
                    break;
                case 10152:
                    objResult = listDoiTac.Find(delegate (DoiTacInfo obj) { return obj.Id == 10082; });
                    break;
                case 10153:
                    objResult = listDoiTac.Find(delegate (DoiTacInfo obj) { return obj.Id == 10083; });
                    break;
                case 10154:
                    objResult = listDoiTac.Find(delegate (DoiTacInfo obj) { return obj.Id == 10084; });
                    break;
                case 10155:
                    objResult = listDoiTac.Find(delegate (DoiTacInfo obj) { return obj.Id == 10085; });
                    break;
                case 10156: // Đồng Tháp
                    objResult = listDoiTac.Find(delegate (DoiTacInfo obj) { return obj.Id == 10086; });
                    break;
                case 10158: // Kiên Giang
                    objResult = listDoiTac.Find(delegate (DoiTacInfo obj) { return obj.Id == 10088; });
                    break;
                case 10159: // Long An
                    objResult = listDoiTac.Find(delegate (DoiTacInfo obj) { return obj.Id == 10089; });
                    break;
                case 10160: // Lâm Đồng
                    objResult = listDoiTac.Find(delegate (DoiTacInfo obj) { return obj.Id == 10090; });
                    break;
                case 10161:
                    objResult = listDoiTac.Find(delegate (DoiTacInfo obj) { return obj.Id == 10091; });
                    break;
                case 10162:
                    objResult = listDoiTac.Find(delegate (DoiTacInfo obj) { return obj.Id == 10092; });
                    break;
                case 10163:
                    objResult = listDoiTac.Find(delegate (DoiTacInfo obj) { return obj.Id == 10093; });
                    break;
                case 10164:
                    objResult = listDoiTac.Find(delegate (DoiTacInfo obj) { return obj.Id == 10094; });
                    break;
                case 10165:
                    objResult = listDoiTac.Find(delegate (DoiTacInfo obj) { return obj.Id == 10095; });
                    break;
                case 10166:
                    objResult = listDoiTac.Find(delegate (DoiTacInfo obj) { return obj.Id == 10096; });
                    break;
                case 10167:
                    objResult = listDoiTac.Find(delegate (DoiTacInfo obj) { return obj.Id == 10097; });
                    break;
                case 10168:
                    objResult = listDoiTac.Find(delegate (DoiTacInfo obj) { return obj.Id == 10098; });
                    break;
                case 10169:
                    objResult = listDoiTac.Find(delegate (DoiTacInfo obj) { return obj.Id == 10099; });
                    break;
                case 10171: // Quảng Bình
                    objResult = listDoiTac.Find(delegate (DoiTacInfo obj) { return obj.Id == 10063; });
                    break;
                case 10172: // Quảng Trị
                    objResult = listDoiTac.Find(delegate (DoiTacInfo obj) { return obj.Id == 10064; });
                    break;
                case 10173: // Huế
                    objResult = listDoiTac.Find(delegate (DoiTacInfo obj) { return obj.Id == 10065; });
                    break;
                case 10175: // Quảng Nam
                    objResult = listDoiTac.Find(delegate (DoiTacInfo obj) { return obj.Id == 10067; });
                    break;
                case 10176:
                    objResult = listDoiTac.Find(delegate (DoiTacInfo obj) { return obj.Id == 10068; });
                    break;
                case 10177:
                    objResult = listDoiTac.Find(delegate (DoiTacInfo obj) { return obj.Id == 10069; });
                    break;
                case 10178:
                    objResult = listDoiTac.Find(delegate (DoiTacInfo obj) { return obj.Id == 10070; });
                    break;
                case 10179:
                    objResult = listDoiTac.Find(delegate (DoiTacInfo obj) { return obj.Id == 10071; });
                    break;
                case 10180:
                    objResult = listDoiTac.Find(delegate (DoiTacInfo obj) { return obj.Id == 10072; });
                    break;
                case 10181:
                    objResult = listDoiTac.Find(delegate (DoiTacInfo obj) { return obj.Id == 10073; });
                    break;
                case 10182:
                    objResult = listDoiTac.Find(delegate (DoiTacInfo obj) { return obj.Id == 10074; });
                    break;
                case 10183: // Đăk Nông
                    objResult = listDoiTac.Find(delegate (DoiTacInfo obj) { return obj.Id == 10075; });
                    break;
            }

            return objResult;
        }

        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created date : 07/05/2014
        /// Todo : Lấy ra tất cả các phòng ban của tất cả các đối tác trực thuộc (của giá trị trường) DonViTrucThuocChoBaoCao
        /// </summary>
        /// <param name="parentDoiTacId"></param>
        /// <returns></returns>
        public List<DoiTacInfo> GetAllDoiTacOfDonViTrucThuocChoBaoCao(int parentDoiTacId = -1)
        {
            List<DoiTacInfo> list = null;
            SqlParameter[] param = {
                                        new SqlParameter("@ParentDoiTacId", parentDoiTacId),
                                    };

            string sql = @"  WITH EntityChildren AS
                            (
	                            SELECT *, 1 AS Level  FROM DoiTac WHERE Id = @ParentDoiTacId
	                            UNION ALL
	                            SELECT e.*, Level + 1 FROM DoiTac e INNER JOIN EntityChildren e2 on e.DonViTrucThuocChoBaoCao = e2.Id
                            )
                            SELECT *
                            FROM EntityChildren	                            
                            ORDER By DonViTrucThuocChoBaoCao ASC, TenDoiTac ASC";

            list = ExecuteQuery(sql, CommandType.Text, param);

            return list;
        }

        public List<DoiTacInfo> GetListHierarchy()
        {
            List<DoiTacInfo> listResult = new List<DoiTacInfo>();
            List<DoiTacInfo> listDoiTacAll = this.GetAllDoiTacOfDonViTrucThuocChoBaoCao(DoiTacInfo.DoiTacIdValue.VNP);
            if (listDoiTacAll == null || listDoiTacAll.Count == 0) return null;

            listResult.Add(listDoiTacAll[0]);
            AddDoiTacToList(listResult[0], listDoiTacAll, ref listResult);

            return listResult;
        }

        private void AddDoiTacToList(DoiTacInfo doiTacInfo, List<DoiTacInfo> listDoiTacAll, ref List<DoiTacInfo> listResult)
        {
            for (int i = 0; i < listDoiTacAll.Count; i++)
            {
                if (listDoiTacAll[i].DonViTrucThuoc == doiTacInfo.Id)
                {
                    listResult.Add(listDoiTacAll[i]);
                    this.AddDoiTacToList(listDoiTacAll[i], listDoiTacAll, ref listResult);
                }
            }
        }

        #endregion
    }
}
