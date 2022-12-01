using System;
using System.Collections.Generic;
using Microsoft.Practices.ServiceLocation;
using SolrNet;
using SolrNet.Commands.Parameters;
using ADDJ.Entity;
using ADDJ.Core;

namespace ADDJ.Impl
{
    public class KhieuNai_TruyVanImpl
    {
        #region "Declare URL Solr"

        //const string URL_SOLR = "http://192.168.0.210:9080/solr/db/";
        private static Int32 total = 0;

        #endregion

        #region "Search Class"

        private string GetListByDonViTrucThuoc(int DoiTacId)
        {
            string strValue = "";
            DoiTacImpl _DoiTacImpl = new DoiTacImpl();
            List<DoiTacInfo> lst = _DoiTacImpl.GetListByDonViTrucThuoc(DoiTacId);
            if (lst.Count > 0)
            {
                strValue += DoiTacId.ToString() + "#";
                foreach (DoiTacInfo info in lst)
                {
                    strValue += GetListByDonViTrucThuocChild(info.Id);
                }
            }
            else
            {
                strValue += strValue += DoiTacId.ToString() + "#";
            }
            return strValue;
        }

        public string GetListByDonViTrucThuocChild(Int32 sParent)
        {
            string strValue = "";
            DoiTacImpl _DoiTacImpl = new DoiTacImpl();
            List<DoiTacInfo> lst = _DoiTacImpl.GetListByDonViTrucThuoc(sParent);
            if (lst.Count > 0)
            {
                strValue += sParent.ToString() + "#";
                foreach (DoiTacInfo info in lst)
                {
                    strValue += GetListByDonViTrucThuocChild(info.Id);
                }
            }
            else
            {
                strValue += sParent.ToString() + "#";
            }
            return strValue;
        }

        private List<KhieuNai_TruyVanInfo> ConvertList(List<KhieuNai_TruyVanInfo> listTruyVan)
        {
            List<KhieuNai_TruyVanInfo> list = new List<KhieuNai_TruyVanInfo>();
            if (listTruyVan.Count > 0)
            {
                foreach (var info in listTruyVan)
                {
                    KhieuNai_TruyVanInfo item = new KhieuNai_TruyVanInfo();
                    if (info.KieuDuLieu == "date")
                    {
                        item.TenTruong = info.TenTruong + "Sort";
                        item.PhepToan = info.PhepToan;
                        item.KieuDuLieu = info.KieuDuLieu;
                        if (info.GiaTri.IndexOf("#") == -1)
                        {
                            var arrItems = info.GiaTri.Split('/');
                            item.GiaTri = arrItems[2] + arrItems[1] + arrItems[0];
                        }
                        else
                        {
                            var arrItems = info.GiaTri.Split('#');
                            string item1 = arrItems[0];
                            string item2 = arrItems[1];
                            var arrItem1 = item1.Split('/');
                            var arrItem2 = item2.Split('/');
                            item.GiaTri = arrItem1[2] + arrItem1[1] + arrItem1[0] + "#" + arrItem2[2] + arrItem2[1] + arrItem2[0];

                        }
                    }
                    else
                    {
                        item.TenTruong = info.TenTruong;
                        item.PhepToan = info.PhepToan;
                        item.KieuDuLieu = info.KieuDuLieu;
                        if (info.GiaTri.IndexOf(",") == -1)
                        {
                            if (info.GiaTri.IndexOf("-") == -1)
                            {
                                item.GiaTri = info.GiaTri;
                            }
                            else
                            {
                                item.GiaTri = info.GiaTri.Split('-')[0];
                            }
                        }
                        else
                        {
                            var arrItems = info.GiaTri.Split(',');
                            int temp = 0;
                            foreach (var items in arrItems)
                            {
                                temp++;
                                if (!string.IsNullOrEmpty(items))
                                {
                                    if (items.IndexOf("-") == -1)
                                    {
                                        if (temp == arrItems.Length)
                                        {
                                            item.GiaTri += items;
                                        }
                                        else
                                        {
                                            item.GiaTri += items + ",";
                                        }
                                    }
                                    else
                                    {
                                        if (temp == arrItems.Length)
                                        {
                                            item.GiaTri += items.Split('-')[0];
                                        }
                                        else
                                        {
                                            item.GiaTri += items.Split('-')[0] + ",";
                                        }
                                    }
                                }
                            }
                        }
                    }
                    list.Add(item);
                }
            }
            return list;
        }

        private List<ISolrQuery> ListSolrQuery(QueryOptions queryOption, List<KhieuNai_TruyVanInfo> list, Int32 DoiTacId)
        {
            List<KhieuNai_TruyVanInfo> listTruyVan = ConvertList(list);
            List<ISolrQuery> lstFilter = new List<ISolrQuery>();
            //Sắp xếp
            List<SortOrder> lstOrder = new List<SortOrder>();
            lstOrder.Add(new SortOrder("Id", Order.ASC));
            queryOption.OrderBy = lstOrder;

            // Biến hasAddFilterDoiTac : trường hợp DoiTacId != 0 thì phải xác định xem trong phần điều kiện tham số tìm kiếm của người dùng
            //                          đã có DoiTacId hoặc DoiTacXuLyId hay chưa ? Nếu chưa có thì phải thêm vào phần filter để kết quả
            //                          lọc tìm kiếm chỉ hiển thị ra các khiếu nại thuộc đối tác (hoặc các đối tác trực thuộc)
            bool hasAddFilterDoiTac = true;
            foreach (KhieuNai_TruyVanInfo info in listTruyVan)
            {
                if (DoiTacId != 0 && hasAddFilterDoiTac)
                {
                    if (info.TenTruong == "DoiTacId" || info.TenTruong == "DoiTacXuLyId")
                    {
                        hasAddFilterDoiTac = false;
                    }
                }

                switch (info.PhepToan)
                {
                    //case "LIKE":
                    //    lstFilter.Add(new SolrQuery(info.TenTruong + ":*" + info.GiaTri + "*"));
                    //    break;
                    case "=":
                        lstFilter.Add(new SolrQuery(info.TenTruong + ":(" + info.GiaTri.Replace(",", " ") + ")"));
                        break;
                    case ">=":
                        lstFilter.Add(new SolrQuery(info.TenTruong + ":[" + info.GiaTri + " TO *]"));
                        break;
                    case "<=":
                        lstFilter.Add(new SolrQuery(info.TenTruong + ":[* TO " + info.GiaTri + "]"));
                        break;
                    case "AND":
                        lstFilter.Add(new SolrQuery(info.TenTruong + ":" + info.GiaTri));
                        break;
                    case "OR":
                        lstFilter.Add(new SolrQuery(info.TenTruong + ":(" + info.GiaTri.Split(',')[0] + " OR " + info.GiaTri.Split(',')[1] + ")"));
                        break;
                    case "IN":
                        lstFilter.Add(new SolrQuery(info.TenTruong + ":(" + info.GiaTri.Replace(",", " ") + ")"));
                        break;
                    case "NOT IN":
                        lstFilter.Add(new SolrQuery("-" + info.TenTruong + ":(" + info.GiaTri.Replace(",", " ") + ")"));
                        break;
                    case "BETWEEN":
                        lstFilter.Add(new SolrQuery(info.TenTruong + ":[" + info.GiaTri.Split('#')[0] + " TO " + info.GiaTri.Split('#')[1] + "]"));
                        break;

                }
            }

            // Trường hợp chỉ được tìm khiếu nại theo đối tác cụ thể và trong danh sách tham số tìm kiếm người dùng không chọn DoiTac hoặc DoiTacXuLyId thì
            // tự động thêm vào danh sách tham số tìm kiếm (Filter)
            if (DoiTacId != 0 && hasAddFilterDoiTac)
            {
                string lstDoiTacId = GetListByDonViTrucThuoc(DoiTacId);
                lstDoiTacId = lstDoiTacId.Substring(0, lstDoiTacId.Length - 1);

                string whereClauseDoiTac = string.Format("DoiTacId : ({0}) OR DoiTacXuLyId: ({0})", lstDoiTacId.Replace("#", " "));

                //lstFilter.Add(new SolrQuery("DoiTacId:(" + lstDoiTacId.Replace("#", " ") + ") OR DoiTacXuLyId : ()"));                
                lstFilter.Add(new SolrQuery(whereClauseDoiTac));
            }

            return lstFilter;
        }

        public List<KhieuNaiSolrInfo> QueryKhieuNaiFromSolr(string URL_SOLR, string keyword, List<KhieuNai_TruyVanInfo> listTruyVan, Int32 DoitacId, int page, int limit, ref int totalRecord)
        {
            try
            {
                if (string.IsNullOrEmpty(keyword))
                {
                    keyword = "*:*";
                }
                string select = "Id,MaKhieuNai" +
                               ",KhuVucId" +
                               ",DoiTacId" +
                               ",PhongBanTiepNhanId" +
                               ",PhongBanXuLyId" +
                               ",LoaiKhieuNaiId" +
                               ",LinhVucChungId" +
                               ",LinhVucConId" +
                               ",LoaiKhieuNai" +
                               ",LinhVucChung" +
                               ",LinhVucCon" +
                               ",DoUuTien" +
                               ",SoThueBao" +
                               ",MaTinhId" +
                               ",MaTinh" +
                               ",MaQuanId" +
                               ",MaQuan" +
                               ",HoTenLienHe" +
                               ",DiaChiLienHe" +
                               ",SDTLienHe" +
                               ",DiaDiemXayRa" +
                               ",ThoiGianXayRa" +
                               ",NoiDungPA" +
                               ",NoiDungCanHoTro" +
                               ",TrangThai" +
                               ",IsChuyenBoPhan" +
                               ",NguoiTiepNhan" +
                               ",HTTiepNhan" +
                               ",NgayTiepNhan" +
                               ",NgayTiepNhanSort" +
                               ",NguoiTienXuLyCap1" +
                               ",NguoiTienXuLyCap2" +
                               ",NguoiTienXuLyCap3" +
                               ",NguoiXuLy" +
                               ",NgayQuaHan" +
                               ",NgayQuaHanSort" +
                               ",NgayCanhBao" +
                               ",NgayCanhBaoSort" +
                               ",NgayChuyenPhongBan" +
                               ",NgayCanhBaoPhongBanXuLy" +
                               ",NgayQuaHanPhongBanXuLy" +
                               ",NgayTraLoiKN" +
                               ",NgayTraLoiKNSort" +
                               ",NgayDongKN" +
                               ",NgayDongKNSort" +
                               ",KQXuLy_SHCV" +
                               ",KQXuLy_CCT" +
                               ",KQXuLy_CSL" +
                               ",KQXuLy_PTSL_IR" +
                               ",KQXuLy_PTSL_Khac" +
                               ",KetQuaXuLy" +
                               ",NoiDungXuLy" +
                               ",NoiDungXuLyDongKN" +
                               ",GhiChu" +
                               ",KNHangLoat" +
                               ",SoTienKhauTru_TKC" +
                               ",SoTienKhauTru_KM1" +
                               ",SoTienKhauTru_KM2" +
                               ",SoTienKhauTru_KM3" +
                               ",SoTienKhauTru_KM4" +
                               ",SoTienKhauTru_KM5" +
                               ",IsLuuKhieuNai" +
                               ",CDate" +
                               ",CUser" +
                               ",LDate" +
                               ",LUser" +
                               ",CallCount" +
                               ",DoHaiLong" +
                               ", ArchiveId";
                List<KhieuNaiSolrInfo> list = new List<KhieuNaiSolrInfo>();
                QueryOptions queryOption = new QueryOptions();

                queryOption.Start = page * limit;
                queryOption.Rows = limit;

                //Lấy ra những trường nào
                Dictionary<string, string> extraParam = new Dictionary<string, string>();
                extraParam.Add("fl", select);
                queryOption.ExtraParams = extraParam;


                //Sắp xếp
                //List<SortOrder> lstOrder = new List<SortOrder>();
                //lstOrder.Add(new SortOrder("Id", Order.ASC));
                //queryOption.OrderBy = lstOrder;

                //Where điều kiện
                //List<ISolrQuery> lstFilter = new List<ISolrQuery>();
                //Lấy ngày tháng trong khoảng
                //lstFilter.Add(new SolrQuery("CDate:[2013-09-10T23:59:59.999Z TO *]"));

                //Lấy ĐK AND OR
                //lstFilter.Add(new SolrQuery("Id:(3 OR 4) AND LinhVucChungId:1"));

                //Id trong khoảng [30-60]: Cái này yêu cầu số lượng string phải bằng nhau :(
                //lstFilter.Add(new SolrQuery("Id:[30 TO 60]"));

                //Id không trong khoảng [30-60]: Cái này yêu cầu số lượng string phải bằng nhau :(
                //lstFilter.Add(new SolrQuery("-Id:[30 TO 60]"));

                //Id = 3 4 5
                //lstFilter.Add(new SolrQuery("Id:(3 4 5)"));

                //Id != 3 4 5
                //lstFilter.Add(new SolrQuery("-Id:(3 4 5)"));

                queryOption.FilterQueries = ListSolrQuery(queryOption, listTruyVan, DoitacId);
                SolrQuery solrQuery = new SolrQuery(keyword);

                SolrQueryResults<KhieuNaiSolrInfo> results = QuerySolrBase<KhieuNaiSolrInfo>.QuerySolr(URL_SOLR, solrQuery, queryOption);

                if (results.Count > 0)
                {
                    //Result
                    totalRecord = results.NumFound;
                    total = results.NumFound;
                    int countRecord = results.Count;
                    for (int i = 0; i < countRecord; i++)
                    {
                        list.Add(results[i]);
                    }
                }

                return list;
            }
            catch (Exception ex)
            {
                Helper.GhiLogs(ex);
                return null;
            }

        }

        public int QueryKhieuNaiFromSolrTotalRecord(string URL_SOLR, string keyword, List<KhieuNai_TruyVanInfo> listTruyVan, Int32 DoitacId, int page, int limit)
        {
            try
            {
                if (string.IsNullOrEmpty(keyword))
                {
                    keyword = "*:*";
                }
                string select = "Id";

                var queryOption = new QueryOptions();

                queryOption.Start = page * limit;
                queryOption.Rows = limit;

                //Lấy ra những trường nào
                var extraParam = new Dictionary<string, string>();
                extraParam.Add("fl", select);
                queryOption.ExtraParams = extraParam;
                queryOption.FilterQueries = ListSolrQuery(queryOption, listTruyVan, DoitacId);
                var solrQuery = new SolrQuery(keyword);

                var results = QuerySolrBase<KhieuNaiSolrInfo>.QuerySolr(URL_SOLR, solrQuery, queryOption);

                if (results.Count > 0)
                {
                    return results.NumFound;
                }

                return 0;
            }
            catch (Exception ex)
            {
                Helper.GhiLogs(ex);
                return 0;
            }

        }

        #endregion
    }
}
