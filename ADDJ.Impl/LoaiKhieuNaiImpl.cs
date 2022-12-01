using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using ADDJ.Entity;
using ADDJ.Core;
using ADDJ.Core.Provider;
using System.Linq;
using System.Web;
using System.IO;
using Lucene.Net.QueryParsers;
using Lucene.Net.Analysis.Standard;

namespace ADDJ.Impl
{
    /// <summary>
    /// Class Insert, Update, Delete, Get dữ liệu của LoaiKhieuNai
    /// </summary>
    /// <author>Lê Xuân Long</author>
    /// <date>02/08/2013</date>

    public class LoaiKhieuNaiImpl : BaseImpl<LoaiKhieuNaiInfo>
    {
        protected override void SetInfoDerivedClass()
        {
            TableName = "LoaiKhieuNai";

            IndexLocation = Path.Combine(Config.PathIndexLucene, TableName) + @"\";
            MaxFieldLength = 10;
            IsUseLucene = true;
            IsUpdateLucene = true;
        }

        public List<LoaiKhieuNaiInfo> Suggestion(string query)
        {
            string[] fields = new string[] { "Name" };

            var parser = new MultiFieldQueryParser(Lucene.Net.Util.Version.LUCENE_29, fields, new StandardAnalyzer(Lucene.Net.Util.Version.LUCENE_29));
            parser.SetAllowLeadingWildcard(true);
            parser.SetDefaultOperator(QueryParser.Operator.OR);

            query = query + "*";
            var q = parser.Parse(query);

            return this.Search(q, null, null, 500, false);
        }

        public List<LoaiKhieuNaiInfo> Suggestion(string query, string lstParentId)
        {
            string[] fields = new string[] { "Id", "Name", "ParentId", "Description", "MaDichVu" };
            string[] strnum = lstParentId.Split(',');
            var parser = new MultiFieldQueryParser(Lucene.Net.Util.Version.LUCENE_29, fields, new StandardAnalyzer(Lucene.Net.Util.Version.LUCENE_29));
            parser.SetAllowLeadingWildcard(true);
            parser.SetDefaultOperator(QueryParser.Operator.OR);
            query = query + "*";
            var q = parser.Parse(query);
            List<LoaiKhieuNaiInfo> list = Search(q, null, null, int.MaxValue, false);
            list = (from a in list where strnum.Contains(a.ParentId.ToString()) select a).Take(10).ToList();
            return list;
        }

        public List<LoaiKhieuNaiInfo> SuggestionGetListByListParent(string query, int getparent, int loaikhieunai0id, int loaikhieunai1id, int loaikhieunai2id)
        {
            string[] fields = new string[] { "Id", "Name", "ParentId", "Description", "MaDichVu" };

            //phuongtt
            int parentId = 0;

            if (loaikhieunai0id > 0)
            {
                parentId = loaikhieunai0id;
            }

            if (loaikhieunai1id > 0)
            {
                parentId = loaikhieunai1id;
            }

            if (loaikhieunai2id > 0)
            {
                parentId = loaikhieunai2id;
            }
            string[] strnum = parentId.ToString().Split(',');
            var parser = new MultiFieldQueryParser(Lucene.Net.Util.Version.LUCENE_29, fields, new StandardAnalyzer(Lucene.Net.Util.Version.LUCENE_29));
            parser.SetAllowLeadingWildcard(true);
            parser.SetDefaultOperator(QueryParser.Operator.OR);
            List<LoaiKhieuNaiInfo> list = new List<LoaiKhieuNaiInfo>();



            if (parentId != 0)
            {
                //query = string.Format("Id:{0} OR ParentId:{0}", lstParentId);
                if (getparent == 1)
                {
                    query = string.Format("ParentId:{0}", parentId);
                }
                else
                {
                    query = string.Format("Id:{0}", parentId);
                }

                var q = parser.Parse(query);
                list = Search(q, null, null, int.MaxValue, false);
            }
            else
            {
                query = query + "*";
                var q = parser.Parse(query);
                list = Search(q, null, null, int.MaxValue, false);
                list = (from a in list where strnum.Contains(a.Cap.ToString()) select a).OrderBy(x => x.Name).ToList();
                list = (from a in list where strnum.Contains(a.ParentId.ToString()) select a).ToList();
            }


            //list = (from a in list where strnum.Contains(a.ParentId.ToString()) select a).ToList();
            return list;
        }

        public List<LoaiKhieuNaiInfo> SuggestionGetListByListID(string query, string lstParentId)
        {
            string[] fields = new string[] { "Id", "Name", "ParentId", "Description", "MaDichVu" };
            string[] strnum = lstParentId.Split(',');
            var parser = new MultiFieldQueryParser(Lucene.Net.Util.Version.LUCENE_29, fields, new StandardAnalyzer(Lucene.Net.Util.Version.LUCENE_29));
            parser.SetAllowLeadingWildcard(true);
            parser.SetDefaultOperator(QueryParser.Operator.OR);
            query = query + "*";
            var q = parser.Parse(query);
            List<LoaiKhieuNaiInfo> list = Search(q, null, null, int.MaxValue, false);
            list = (from a in list where strnum.Contains(a.Id.ToString()) select a).ToList();
            return list;
        }

        private static List<LoaiKhieuNaiInfo> _ListLoaiKhieuNai;
        public static List<LoaiKhieuNaiInfo> ListLoaiKhieuNai
        {
            get
            {
                if (_ListLoaiKhieuNai == null)
                    _ListLoaiKhieuNai = new LoaiKhieuNaiImpl().GetList();
                return _ListLoaiKhieuNai;
            }
            set { _ListLoaiKhieuNai = value; }
        }

        #region Function

        public List<LoaiKhieuNaiInfo> GetListLoaiKhieuNai2Cap()
        {
            List<LoaiKhieuNaiInfo> lstReturn = new List<LoaiKhieuNaiInfo>();

            List<LoaiKhieuNaiInfo> lst = this.GetListDynamic("", "Cap = 1 or Cap = 2", "Sort");
            //if (lst != null && lst.Count > 0)
            //{
            //    foreach (var item in lst)
            //    {
            //        if (item.Cap == 2)
            //            item.Name = HttpUtility.HtmlDecode("&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + item.Name);
            //    }
            //}

            List<LoaiKhieuNaiInfo> listResult = new List<LoaiKhieuNaiInfo>();
            for (int i = 0; i < lst.Count;i++ )
            {
                if(lst[i].Cap == 1)
                {
                    listResult.Add(lst[i]);
                    for(int j=0;j<lst.Count;j++)
                    {
                        if(lst[j].ParentId == lst[i].Id)
                        {
                            lst[j].Name = HttpUtility.HtmlDecode("&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + lst[j].Name);
                            listResult.Add(lst[j]);
                        }
                    }
                }
            }

            return listResult;
        }

        public List<LoaiKhieuNaiInfo> GetListLoaiKhieuNaiSortParent()
        {
            return GetListLoaiKhieuNaiSortParent("&nbsp;&nbsp;&nbsp;&nbsp;", string.Empty, null);
        }

        public List<LoaiKhieuNaiInfo> GetListLoaiKhieuNaiSortParent(string replaceSpace)
        {
            return GetListLoaiKhieuNaiSortParent(replaceSpace, string.Empty, null);
        }

        public List<LoaiKhieuNaiInfo> GetListLoaiKhieuNaiSortParent(string replaceSpace, string whereClause)
        {
            return GetListLoaiKhieuNaiSortParent(replaceSpace, whereClause, null);
        }

        public List<LoaiKhieuNaiInfo> GetListLoaiKhieuNaiSortParentPage(string replaceSpace, string selectClause, string whereClause, int pageIndex, int pageSize, ref int totalRecord)
        {
            List<LoaiKhieuNaiInfo> lstReturn = new List<LoaiKhieuNaiInfo>();

            List<LoaiKhieuNaiInfo> lst = this.GetPaged(selectClause, whereClause, "Sort", pageIndex, pageSize, ref totalRecord);

            if (lst != null && lst.Count > 0)
            {
                string strSpace2 = string.Empty;
                string strSpace3 = string.Empty;
                for (int i = 0; i < 1; i++)
                    strSpace2 += replaceSpace;

                for (int i = 0; i < 2; i++)
                    strSpace3 += replaceSpace;

                foreach (var item in lst)
                {
                    if (item.Cap == 2)
                        item.Name = HttpUtility.HtmlDecode(strSpace2 + item.Name);
                    else if (item.Cap == 3)
                        item.Name = HttpUtility.HtmlDecode(strSpace3 + item.Name);
                }
            }

            return lst;
        }

        public List<LoaiKhieuNaiInfo> GetListLoaiKhieuNaiSortParent(string replaceSpace, string whereClause, LoaiKhieuNaiInfo infoStart)
        {
            List<LoaiKhieuNaiInfo> lstReturn = new List<LoaiKhieuNaiInfo>();

            if (infoStart != null)
                lstReturn.Add(infoStart);

            var lst = this.GetListDynamic("", whereClause, "");

            if (lst != null && lst.Count > 0)
            {
                var capMin = lst.Min(t => t.Cap);
                var pId = lst.Where(t => t.Cap == capMin).First();
                lstReturn = BuildTree(lst, pId.ParentId, 0, replaceSpace);
            }

            return lstReturn;
        }

        private List<LoaiKhieuNaiInfo> BuildTree(List<LoaiKhieuNaiInfo> lst, int pId, int cap, string replaceSpace)
        {
            List<LoaiKhieuNaiInfo> lstRet = new List<LoaiKhieuNaiInfo>();
            var lstPar = lst.Where(t => t.ParentId == pId).OrderBy(t => t.Sort);
            string strSpace = string.Empty;

            for (int i = 0; i < cap; i++)
                strSpace += replaceSpace;

            foreach (var item in lstPar)
            {
                item.Name = HttpUtility.HtmlDecode(strSpace + item.Name);
                lstRet.Add(item);
                var q = BuildTree(lst, item.Id, cap + 1, replaceSpace);
                if (q.Count > 0)
                {
                    foreach (var i in q)
                    {
                        i.Name = i.Name;
                        lstRet.Add(i);
                    }
                }
            }
            return lstRet;
        }

        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created : 20/09/2014
        /// Todo : Lấy ra danh sách loại khiếu nại được sắp xếp theo cây thư mục cha con
        ///        
        /// </summary>       
        /// <returns></returns>
        public List<LoaiKhieuNaiInfo> GetListSortHierarchyByLoaiKhieuNaiId()
        {
            List<LoaiKhieuNaiInfo> listResult = new List<LoaiKhieuNaiInfo>();
            List<LoaiKhieuNaiInfo> listParent = this.GetListDynamic("*", "ParentId=0", "Name ASC");
            if (listParent == null) return null;

            List<LoaiKhieuNaiInfo> listKhieuNaiAll = this.GetList();
            if(listKhieuNaiAll != null)
            {
                for(int i=0;i<listParent.Count;i++)
                {
                    listResult.Add(listParent[i]);

                    for(int j=0;j<listKhieuNaiAll.Count;j++)
                    {
                        if(listParent[i].Id == listKhieuNaiAll[j].ParentId)
                        {
                            listResult.Add(listKhieuNaiAll[j]);

                            for(int k=0;k<listKhieuNaiAll.Count;k++)
                            {
                                if(listKhieuNaiAll[j].Id == listKhieuNaiAll[k].ParentId)
                                {
                                    listResult.Add(listKhieuNaiAll[k]);
                                }
                            } // end for(int k=0;k<listKhieuNaiAll.Count;k++)
                        }
                    } // end  for(int j=0;j<listKhieuNaiAll.Count;j++)
                } // end for(int i=0;i<listParent.Count;i++)
            } // end if(listKhieuNaiAll != null)

            return listResult;
        }

        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created : 15/06/2015
        /// Todo : Lấy ra danh sách loại khiếu nại được sắp xếp theo cây thư mục cha con        
        /// </summary>    
        /// <param name="status">
        ///     = -1 : Lấy toàn bộ các loại khiếu nại
        ///     = 0 : Lấy các loại khiếu nại không có hiệu lực
        ///     = 1 : Lấy các loại khiếu nại đang có hiệu lực
        /// </param>
        /// <returns></returns>
        public List<LoaiKhieuNaiInfo> GetListSortHierarchy(int status)
        {
            List<LoaiKhieuNaiInfo> listResult = new List<LoaiKhieuNaiInfo>();

            string whereStatus = string.Empty;
            if(status >= 0)
            {
                whereStatus = string.Format(" AND Status={0}", whereStatus);
            }
           
            List<LoaiKhieuNaiInfo> listKhieuNaiAll = this.GetListDynamic("*", "ParentId=0" + whereStatus, "Name ASC");
            if (listKhieuNaiAll != null)
            {
                for (int i = 0; i < listKhieuNaiAll.Count; i++)
                {
                    if(listKhieuNaiAll[i].Cap == 1)
                    {
                        listResult.Add(listKhieuNaiAll[i]);

                        for (int j = 0; j < listKhieuNaiAll.Count; j++)
                        {
                            if (listKhieuNaiAll[i].Id == listKhieuNaiAll[j].ParentId)
                            {
                                listResult.Add(listKhieuNaiAll[j]);

                                for (int k = 0; k < listKhieuNaiAll.Count; k++)
                                {
                                    if (listKhieuNaiAll[j].Id == listKhieuNaiAll[k].ParentId)
                                    {
                                        listResult.Add(listKhieuNaiAll[k]);
                                    }
                                } // end for(int k=0;k<listKhieuNaiAll.Count;k++)
                            }
                        } // end  for(int j=0;j<listKhieuNaiAll.Count;j++)
                    } // end if(listKhieuNaiAll[i].Cap == 1)                    
                } // end for(int i=0;i<listParent.Count;i++)
            } // end if(listKhieuNaiAll != null)

            return listResult;
        }

        public List<LoaiKhieuNaiInfo> GetListSortHierarchyByLoaiKhieuNaiId(List<int> listLoaiKhieuNaiId)
        {
            List<LoaiKhieuNaiInfo> listResult = new List<LoaiKhieuNaiInfo>();
            List<LoaiKhieuNaiInfo> listParent = this.GetListDynamic("*", "ParentId=0", "Name ASC");
            if (listParent == null) return null;

            for(int i=0;i<listParent.Count;i++)
            {
                if(!listLoaiKhieuNaiId.Contains(listParent[i].Id))
                {
                    listParent.RemoveAt(i);
                    i--;
                }
            }

            List<LoaiKhieuNaiInfo> listKhieuNaiAll = this.GetList();
            if (listKhieuNaiAll != null)
            {
                for (int i = 0; i < listParent.Count; i++)
                {
                    listResult.Add(listParent[i]);

                    for (int j = 0; j < listKhieuNaiAll.Count; j++)
                    {
                        if (listParent[i].Id == listKhieuNaiAll[j].ParentId)
                        {
                            listResult.Add(listKhieuNaiAll[j]);

                            for (int k = 0; k < listKhieuNaiAll.Count; k++)
                            {
                                if (listKhieuNaiAll[j].Id == listKhieuNaiAll[k].ParentId)
                                {
                                    listResult.Add(listKhieuNaiAll[k]);
                                }
                            } // end for(int k=0;k<listKhieuNaiAll.Count;k++)
                        }
                    } // end  for(int j=0;j<listKhieuNaiAll.Count;j++)
                } // end for(int i=0;i<listParent.Count;i++)
            } // end if(listKhieuNaiAll != null)           

            return listResult;
        }

        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created date : 27/11/2014
        /// Todo : Lấy danh sách loại khiếu nại thuộc đơn vị
        /// </summary>
        /// <param name="doiTacId"></param>
        /// <returns></returns>
        public List<LoaiKhieuNaiInfo> ListLoaiKhieuNaiThuocDonVi(int doiTacId)
        {
            List<LoaiKhieuNaiInfo> lstRet = null;
//            string sql = @"WITH cte AS
//                        (
//                            SELECT *, CAST(0 AS varbinary(max)) AS Level
//                                FROM LoaiKhieuNai
//                                WHERE ThuocDonVi = @DoiTacId
//                                    AND ParentId = 0
//                            UNION ALL
//                            SELECT i.*, Level + CAST(i.Id AS varbinary(max)) AS Level
//                                FROM LoaiKhieuNai i
//                                INNER JOIN cte c ON c.[Id] = i.[ParentID]
//                        )
//
//                        SELECT 
//	                        *
//                        FROM cte
//                        ORDER BY [Level];";

            string sql = @"SELECT * FROM LoaiKhieuNai
                            WHERE ThuocDonVi = @DoiTacId
                            ORDER BY Sort ASC";

            SqlParameter[] param = {
										new SqlParameter("@DoiTacId", doiTacId),
									};

            lstRet = ExecuteQuery(sql, CommandType.Text, param);

            return lstRet;
        }

        #endregion

        #region Nghi-nv
        public LoaiKhieuNaiInfo QLKN_LoaiKhieuNaigetByID(int id)
        {
            LoaiKhieuNaiInfo info = new LoaiKhieuNaiInfo();

            try
            {
                info = this.GetInfo(id);
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
            }
            return info;
        }
        public List<LoaiKhieuNaiInfo> QLKN_LoaiKhieuNai_ByPhongBan(int PhongBanId)
        {
            List<LoaiKhieuNaiInfo> list = null;
            SqlParameter[] param = {
										new SqlParameter("PhongBanId",PhongBanId),
									};
            try
            {
                list = ExecuteQuery("usp_KhieuNai_LoaiKhieuNai_ByPhongBan", param);
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
            }
            return list;

        }
        public DataTable QLKN_LoaiKhieuNai_ByPhongBanId(int PhongBanId)
        {
            int trangThai1 = (int)KhieuNai_TrangThai_Type.Chờ_xử_lý;
            int trangThai2 = (int)KhieuNai_TrangThai_Type.Đang_xử_lý;
            int trangThai3 = (int)KhieuNai_TrangThai_Type.Chờ_đóng;
            SqlParameter[] sqlParam = {
                                          new SqlParameter("PhongBanId", PhongBanId), 
                                          new SqlParameter("TrangThai1", trangThai1), 
                                          new SqlParameter("TrangThai2", trangThai2), 
                                          new SqlParameter("TrangThai3", trangThai3)
                                         
                                      };
            DataSet dt = this.ExecuteQueryToDataSet("usp_KhieuNai_LoaiKhieuNai_ByPhongBanId", sqlParam);
            DataTable tabReturn = dt.Tables[0];
            return tabReturn;
        }

        public DataTable QLKN_LoaiKhieuNai_GetAllWithPadding(int PhongBanId, int StartPageIndex, int PageSize)
        {
            int trangThai1 = (int)KhieuNai_TrangThai_Type.Chờ_xử_lý;
            int trangThai2 = (int)KhieuNai_TrangThai_Type.Đang_xử_lý;
            int trangThai3 = (int)KhieuNai_TrangThai_Type.Chờ_đóng;
            SqlParameter[] sqlParam = {
                                          new SqlParameter("PhongBanId", PhongBanId), 
                                          new SqlParameter("TrangThai1", trangThai1), 
                                          new SqlParameter("TrangThai2", trangThai2), 
                                          new SqlParameter("TrangThai3", trangThai3), 
                                          new SqlParameter("StartPageIndex", StartPageIndex), 
                                          new SqlParameter("PageSize", PageSize) 
                                      };
            DataSet dt = this.ExecuteQueryToDataSet("usp_KhieuNai_LoaiKhieuNai_GetAllWithPadding", sqlParam);
            DataTable tabReturn = dt.Tables[0];
            return tabReturn;
        }
        public int QLKN_LoaiKhieuNai_GetAllWithPadding_TotalRecords(int PhongBanId, int StartPageIndex, int PageSize)
        {
            int trangThai1 = (int)KhieuNai_TrangThai_Type.Chờ_xử_lý;
            int trangThai2 = (int)KhieuNai_TrangThai_Type.Đang_xử_lý;
            int trangThai3 = (int)KhieuNai_TrangThai_Type.Chờ_đóng;
            SqlParameter[] sqlParam = {
                                          new SqlParameter("PhongBanId", PhongBanId), 
                                          new SqlParameter("TrangThai1", trangThai1), 
                                          new SqlParameter("TrangThai2", trangThai2), 
                                          new SqlParameter("TrangThai3", trangThai3), 
                                          new SqlParameter("StartPageIndex", StartPageIndex), 
                                          new SqlParameter("PageSize", PageSize) 
                                      };
            DataSet dt = this.ExecuteQueryToDataSet("usp_KhieuNai_LoaiKhieuNai_GetAllWithPadding", sqlParam);

            int TotalRecords = Convert.ToInt32(dt.Tables[1].Rows[0]["TotalRecords"]);
            return TotalRecords;
        }
        public int QLKN_LoaiKhieuNai_GetAllWithPadding_TongSoKhieuNai(int PhongBanId, int StartPageIndex, int PageSize)
        {
            int trangThai1 = (int)KhieuNai_TrangThai_Type.Chờ_xử_lý;
            int trangThai2 = (int)KhieuNai_TrangThai_Type.Đang_xử_lý;
            int trangThai3 = (int)KhieuNai_TrangThai_Type.Chờ_đóng;
            SqlParameter[] sqlParam = {
                                          new SqlParameter("PhongBanId", PhongBanId), 
                                          new SqlParameter("TrangThai1", trangThai1), 
                                          new SqlParameter("TrangThai2", trangThai2), 
                                          new SqlParameter("TrangThai3", trangThai3), 
                                          new SqlParameter("StartPageIndex", StartPageIndex), 
                                          new SqlParameter("PageSize", PageSize) 
                                      };
            DataSet dt = this.ExecuteQueryToDataSet("usp_KhieuNai_LoaiKhieuNai_GetAllWithPadding", sqlParam);

            int TongSoKhieuNai = Convert.ToInt32(dt.Tables[2].Rows[0]["TongSoKhieuNai"]);
            return TongSoKhieuNai;
        }
        #endregion
        #region hungnv Lấy danh sách loại khiếu nại cha
        /// <summary>
        /// Danh sach loai khieu nai level 1
        /// </summary>
        /// <returns></returns>
        public List<LoaiKhieuNaiInfo> LoaiKhieuNai_GetAllByParams(string selectField, string whereField, string orderBy)
        {
            List<LoaiKhieuNaiInfo> lstLoaiKhieuNai = null;
            lstLoaiKhieuNai = GetListDynamic(selectField, whereField, orderBy);
            return lstLoaiKhieuNai;
        }
        #endregion
    }
}
