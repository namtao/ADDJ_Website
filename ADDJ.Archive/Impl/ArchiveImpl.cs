using GQKN.Archive.Core;
using GQKN.Archive.Entity;
using GQKN.Archive.Impl;
using Microsoft.Practices.ServiceLocation;
using SolrNet;
using SolrNet.Commands.Parameters;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Transactions;
using System.Windows.Forms;

namespace GQKN.Archive.Impl
{
    /// <summary>
    /// Author : Phi Hoang Hai
    /// Created date : 14/07/2014
    /// Todo : Xây dựng các phương thức thực hiện archive dữ liệu khiếu nại
    /// </summary>
    public class ArchiveImpl
    {
        #region Variable
        private const string SOLR_GQKN = "GQKN";

        public static bool IsContinue = false;
        private string URL_SOLR_GQKN
        {
            get
            {
                return Config.ServerSolr + SOLR_GQKN;
            }
        }

        private System.Windows.Forms.TextBox txtMessage;
        private System.Windows.Forms.Label lblDelete;
        private System.Windows.Forms.Label lblCopy;
        private System.Windows.Forms.Label lblTotal;

        private int COUNT_SOLR_COMIT = 500;

        #endregion

        #region Properties
        private ArchiveConfigInfo _archiveInfo;
        public ArchiveConfigInfo ArchiveInfo
        {
            get
            {
                return this._archiveInfo;
            }
            set
            {
                this._archiveInfo = value;
            }
        }

        private KhieuNaiImpl _KhieuNaiImpl;
        public KhieuNaiImpl ServiceKhieuNai
        {
            get
            {
                if (_KhieuNaiImpl == null)
                    _KhieuNaiImpl = new KhieuNaiImpl();
                return _KhieuNaiImpl;
            }
        }

        private KhieuNai_ActivityImpl _KhieuNaiActivty;
        public KhieuNai_ActivityImpl ServiceKhieuNaiActivty
        {
            get
            {
                if (_KhieuNaiActivty == null)
                    _KhieuNaiActivty = new KhieuNai_ActivityImpl();
                return _KhieuNaiActivty;
            }
        }

        private KhieuNai_BuocXuLyImpl _KhieuNaiBuocXuLy;
        public KhieuNai_BuocXuLyImpl ServiceKhieuNaiBuocXuLy
        {
            get
            {
                if (_KhieuNaiBuocXuLy == null)
                    _KhieuNaiBuocXuLy = new KhieuNai_BuocXuLyImpl();
                return _KhieuNaiBuocXuLy;
            }
        }

        private KhieuNai_FileDinhKemImpl _KhieuNaiFileDinhKem;
        public KhieuNai_FileDinhKemImpl ServiceKhieuNaiFileDinhKem
        {
            get
            {
                if (_KhieuNaiFileDinhKem == null)
                    _KhieuNaiFileDinhKem = new KhieuNai_FileDinhKemImpl();
                return _KhieuNaiFileDinhKem;
            }
        }

        private KhieuNai_KetQuaXuLyImpl _KhieuNaiKetQuaXuLy;
        public KhieuNai_KetQuaXuLyImpl ServiceKhieuNaiKetQuaXuLy
        {
            get
            {
                if (_KhieuNaiKetQuaXuLy == null)
                    _KhieuNaiKetQuaXuLy = new KhieuNai_KetQuaXuLyImpl();
                return _KhieuNaiKetQuaXuLy;
            }
        }

        private KhieuNai_LogImpl _KhieuNaiLog;
        public KhieuNai_LogImpl ServiceKhieuNaiLog
        {
            get
            {
                if (_KhieuNaiLog == null)
                    _KhieuNaiLog = new KhieuNai_LogImpl();
                return _KhieuNaiLog;
            }
        }

        private KhieuNai_SoTienImpl _KhieuNaiSoTien;
        public KhieuNai_SoTienImpl ServiceKhieuNaiSoTien
        {
            get
            {
                if (_KhieuNaiSoTien == null)
                    _KhieuNaiSoTien = new KhieuNai_SoTienImpl();
                return _KhieuNaiSoTien;
            }
        }

        private List<string> _ColumnKhieuNai;
        public List<string> ColumnKhieuNai
        {
            get
            {
                if (_ColumnKhieuNai == null)
                {
                    DataTable obj = ServiceKhieuNai.GetColumnSchema();
                    _ColumnKhieuNai = GetSchema(obj);
                }
                return _ColumnKhieuNai;
            }
        }

        private List<string> _ColumnKhieuNaiActivity;
        public List<string> ColumnKhieuNaiActivity
        {
            get
            {
                if (_ColumnKhieuNaiActivity == null)
                {
                    _ColumnKhieuNaiActivity = GetSchema(ServiceKhieuNaiActivty.GetColumnSchema());
                }
                return _ColumnKhieuNaiActivity;
            }
        }

        private List<string> _ColumnKhieuNaiBuocXuLy;
        public List<string> ColumnKhieuNaiBuocXuLy
        {
            get
            {
                if (_ColumnKhieuNaiBuocXuLy == null)
                {
                    _ColumnKhieuNaiBuocXuLy = GetSchema(ServiceKhieuNaiBuocXuLy.GetColumnSchema());
                }
                return _ColumnKhieuNaiBuocXuLy;
            }
        }

        private List<string> _ColumnKhieuNaiFileDinhKem;
        public List<string> ColumnKhieuNaiFileDinhKem
        {
            get
            {
                if (_ColumnKhieuNaiFileDinhKem == null)
                {
                    _ColumnKhieuNaiFileDinhKem = GetSchema(ServiceKhieuNaiFileDinhKem.GetColumnSchema());
                }
                return _ColumnKhieuNaiFileDinhKem;
            }
        }

        private List<string> _ColumnKhieuNaiKetQuaXuLy;
        public List<string> ColumnKhieuNaiKetQuaXuLy
        {
            get
            {
                if (_ColumnKhieuNaiKetQuaXuLy == null)
                {
                    _ColumnKhieuNaiKetQuaXuLy = GetSchema(ServiceKhieuNaiKetQuaXuLy.GetColumnSchema());
                }
                return _ColumnKhieuNaiKetQuaXuLy;
            }
        }

        private List<string> _ColumnKhieuNaiLog;
        public List<string> ColumnKhieuNaiLog
        {
            get
            {
                if (_ColumnKhieuNaiLog == null)
                {
                    _ColumnKhieuNaiLog = GetSchema(ServiceKhieuNaiLog.GetColumnSchema());
                }
                return _ColumnKhieuNaiLog;
            }
        }

        private List<string> _ColumnKhieuNaiSoTien;
        public List<string> ColumnKhieuNaiSoTien
        {
            get
            {
                if (_ColumnKhieuNaiSoTien == null)
                {
                    _ColumnKhieuNaiSoTien = GetSchema(ServiceKhieuNaiSoTien.GetColumnSchema());
                }
                return _ColumnKhieuNaiSoTien;
            }
        }

        private List<string> GetSchema(DataTable dtSchema)
        {
            List<string> lst = new List<string>();
            if (dtSchema != null && dtSchema.Rows.Count > 0)
            {
                foreach (DataRow dr in dtSchema.Rows)
                {
                    lst.Add(dr["COLUMN_NAME"].ToString());
                }
            }
            return lst;
        }
        #endregion

        #region Properties Archive
        private KhieuNaiImpl _KhieuNaiArchiveImpl;
        public KhieuNaiImpl ServiceKhieuNaiArchive
        {
            get
            {
                if (_KhieuNaiArchiveImpl == null) _KhieuNaiArchiveImpl = new KhieuNaiImpl(this.ArchiveInfo.ConnectionString);
                return _KhieuNaiArchiveImpl;
            }
        }

        private KhieuNai_ActivityImpl _KhieuNaiActivtyArchive;
        public KhieuNai_ActivityImpl ServiceKhieuNaiActivtyArchive
        {
            get
            {
                if (_KhieuNaiActivtyArchive == null)
                    _KhieuNaiActivtyArchive = new KhieuNai_ActivityImpl(this.ArchiveInfo.ConnectionString);
                return _KhieuNaiActivtyArchive;
            }
        }

        private KhieuNai_BuocXuLyImpl _KhieuNaiBuocXuLyArchive;
        public KhieuNai_BuocXuLyImpl ServiceKhieuNaiBuocXuLyArchive
        {
            get
            {
                if (_KhieuNaiBuocXuLyArchive == null)
                    _KhieuNaiBuocXuLyArchive = new KhieuNai_BuocXuLyImpl(this.ArchiveInfo.ConnectionString);
                return _KhieuNaiBuocXuLyArchive;
            }
        }

        private KhieuNai_FileDinhKemImpl _KhieuNaiFileDinhKemArchive;
        public KhieuNai_FileDinhKemImpl ServiceKhieuNaiFileDinhKemArchive
        {
            get
            {
                if (_KhieuNaiFileDinhKemArchive == null)
                    _KhieuNaiFileDinhKemArchive = new KhieuNai_FileDinhKemImpl(this.ArchiveInfo.ConnectionString);
                return _KhieuNaiFileDinhKemArchive;
            }
        }

        private KhieuNai_KetQuaXuLyImpl _KhieuNaiKetQuaXuLyArchive;
        public KhieuNai_KetQuaXuLyImpl ServiceKhieuNaiKetQuaXuLyArchive
        {
            get
            {
                if (_KhieuNaiKetQuaXuLyArchive == null)
                    _KhieuNaiKetQuaXuLyArchive = new KhieuNai_KetQuaXuLyImpl(this.ArchiveInfo.ConnectionString);
                return _KhieuNaiKetQuaXuLyArchive;
            }
        }

        private KhieuNai_LogImpl _KhieuNaiLogArchive;
        public KhieuNai_LogImpl ServiceKhieuNaiLogArchive
        {
            get
            {
                if (_KhieuNaiLogArchive == null)
                    _KhieuNaiLogArchive = new KhieuNai_LogImpl(this.ArchiveInfo.ConnectionString);
                return _KhieuNaiLogArchive;
            }
        }

        private KhieuNai_SoTienImpl _KhieuNaiSoTienArchive;
        public KhieuNai_SoTienImpl ServiceKhieuNaiSoTienArchive
        {
            get
            {
                if (_KhieuNaiSoTienArchive == null)
                    _KhieuNaiSoTienArchive = new KhieuNai_SoTienImpl(this.ArchiveInfo.ConnectionString);
                return _KhieuNaiSoTienArchive;
            }
        }

        private KhieuNai_UpdateArchiveImpl _KhieuNai_UpdateArchiveImpl;

        public KhieuNai_UpdateArchiveImpl ServiceKhieuNaiUpdateArchive
        {
            get
            {
                if (_KhieuNai_UpdateArchiveImpl == null) _KhieuNai_UpdateArchiveImpl = new KhieuNai_UpdateArchiveImpl(this.ArchiveInfo.ConnectionString);
                return _KhieuNai_UpdateArchiveImpl;
            }
        }
        #endregion

        private void ShowText(string str)
        {
            txtMessage.AppendText(string.Format("{0}:{1}{2}", DateTime.Now.ToString(), str, Environment.NewLine));
        }

        public void XuLyLuuTruDuLieu(DateTime fromDate, DateTime toDate, TextBox txtMessage, Label lblDelete, Label lblCopy, Label lblTotal, bool isRepeat2End)
        {
            try
            {
                this.txtMessage = txtMessage;
                this.lblDelete = lblDelete;
                this.lblCopy = lblCopy;
                this.lblTotal = lblTotal;

                // Lấy danh sách khiếu nại đánh dấu trạng thái đã Archive tại DB chính
                // Do việc dừng đột ngột, lỗi, stop tiến trình


                // Lấy tất, chưa kiểm tra tính toán về mặt số lượng => có thể gây lỗi nếu quá lớn
                List<KhieuNaiInfo> listKhieuNaiInfoArchived = ListKhieuNaiArchived();

                lblCopy.Text = 0.ToString();
                lblDelete.Text = 0.ToString();
                lblTotal.Text = 0.ToString();

                lblDelete.Text = listKhieuNaiInfoArchived.Count.ToString();
                ShowText("Tổng số bản ghi cập nhật lại từ archive trước: " + listKhieuNaiInfoArchived.Count);

                if (listKhieuNaiInfoArchived != null && listKhieuNaiInfoArchived.Count > 0)
                {
                    List<string> dsKhieuNaiTruoc = new List<string>();

                    // Kiểm tra Solr đã được cập nhật chưa ? Nếu solr chưa được cập nhật thì cập nhật solr
                    // Xóa khiếu nại ở các bảng (KhieuNai, KhieuNai_Activity,...) của DB chính
                    Dictionary<int, int> dicKhieuNaiIdNeedDelete = new Dictionary<int, int>();
                    for (int i = 0; i < listKhieuNaiInfoArchived.Count; i++)
                    {
                        dicKhieuNaiIdNeedDelete.Add(listKhieuNaiInfoArchived[i].Id, listKhieuNaiInfoArchived[i].ArchiveId);
                        dsKhieuNaiTruoc.Add(string.Join("_", listKhieuNaiInfoArchived[i].ArchiveId, listKhieuNaiInfoArchived[i].Id));
                    }

                    Helper.GhiLogs("DsKNTruocDoConLai", string.Join(", ", dsKhieuNaiTruoc));

                    if (IsContinue)
                    {
                        List<int> listKhieuNaiIdError = DeleteKhieuNaiArchived_v2(dicKhieuNaiIdNeedDelete);
                        if (listKhieuNaiIdError != null && listKhieuNaiIdError.Count > 0)
                        {
                            string errorMesage = string.Empty;
                            for (int i = 0; i < listKhieuNaiIdError.Count; i++)
                            {
                                errorMesage = string.Format("{0}{1}, ", errorMesage, listKhieuNaiIdError[i]);
                            }

                            errorMesage = errorMesage.Trim().TrimEnd(',');
                            errorMesage = string.Format("Các khiếu nại có mã sau không lưu trữ được : {0}", errorMesage);

                            ShowText(errorMesage);
                        }
                    }
                    ShowText("Kết thúc archive trước đó.");
                }


                lblTotal.Text = LayTongSoLuongKhieuNaiCanLuuTru(fromDate, toDate).ToString();

                // Vòng lặp vô hạn
                while (true)
                {
                    if (IsContinue)
                    {

                        // Lấy danh sách khiếu nại cần Archive theo cấu hình tại DB chính
                        DataTable dsKhieuNaiCanLuuTru = LayDanhSachKhieuNaiCanLuuTru(Config.MaxNumberArchive, fromDate, toDate);

                        lblCopy.Text = (dsKhieuNaiCanLuuTru.Rows.Count + Convert.ToInt32(lblCopy.Text)).ToString();

                        if (dsKhieuNaiCanLuuTru != null && dsKhieuNaiCanLuuTru.Rows.Count > 0)
                        {
                            ShowText("Tổng số bản ghi archive: " + dsKhieuNaiCanLuuTru.Rows.Count);

                            // Xử lý lưu trữ
                            CapNhatDanhSachCanLuuTru(dsKhieuNaiCanLuuTru, fromDate, toDate);

                            lblDelete.Text = (dsKhieuNaiCanLuuTru.Rows.Count + Convert.ToInt32(lblDelete.Text)).ToString();
                        }
                        else
                        {
                            // Nếu hết mới thoát
                            break;
                        }
                        // Nếu không lặp cũng thoát
                        if (!isRepeat2End) break;
                    }
                    else // Nếu click nút stop => thoát tiến trình
                    {
                        break;
                    }
                }
                // end if(listKhieuNaiNeedArchive != null && listKhieuNaiNeedArchive.Count > 0)

            }
            catch (Exception ex)
            {
                ShowText(ex.Message);
                ShowText(ex.StackTrace);
                throw ex;
            }
        }
        /// <summary>
        /// Danh sách khiếu nại bị đánh dấu nhưng dứng lại, do cố ý stop
        /// </summary>
        public List<KhieuNaiInfo> ListKhieuNaiArchived()
        {
            List<KhieuNaiInfo> listKhieuNaiInfo = null;
            KhieuNaiImpl khieuNaiImpl = new KhieuNaiImpl();
            khieuNaiImpl.ConnectTimeOut = 1 * 60; // Kết nối trong 1 phút

            listKhieuNaiInfo = khieuNaiImpl.GetListDynamic("*", "ArchiveId > 0", string.Empty);

            return listKhieuNaiInfo;
        }

        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created date : 15/07/2014
        /// Todo : Lấy ra danh sách khiếu nại cần archive
        /// </summary>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <returns></returns>
        public List<KhieuNaiInfo> ListKhieuNaiNeedArchive(DateTime fromDate, DateTime toDate)
        {
            List<KhieuNaiInfo> listKhieuNaiInfo = null;
            KhieuNaiImpl khieuNaiImpl = new KhieuNaiImpl();
            string whereClause = string.Format("NgayTiepNhanSort >={0} AND NgayTiepNhanSort <= {1} AND ArchiveId = 0 AND TrangThai=3", fromDate.ToString("yyyyMMdd"), toDate.ToString("yyyyMMdd"));
            listKhieuNaiInfo = khieuNaiImpl.GetListDynamic("*", whereClause, "Id ASC");
            return listKhieuNaiInfo;
        }

        /// <summary>
        /// Lấy Danh sách KN cần Update cho Solr
        /// </summary>
        /// <param name="topNumber"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <returns></returns>
        public List<KhieuNai_UpdateArchiveInfo> LayDanhSachKhieuNaiArchiveCanUpdate2Solr(int topNumber, DateTime fromDate, DateTime toDate)
        {
            string whereClause = string.Format("NgayTiepNhanSort >={0} AND NgayTiepNhanSort <= {1} AND (TrangThai IS NULL OR TrangThai = 0 OR TrangThai = 1)",
               fromDate.ToString("yyyyMMdd"),
               toDate.ToString("yyyyMMdd"));

            string select = topNumber <= 0 ? string.Format("*") : string.Format("TOP {0} *", topNumber);

            return ServiceKhieuNaiUpdateArchive.GetListDynamic(select, whereClause, string.Empty);
        }

        /// <summary>
        /// Lưu ý việc chuyển KN từ bảng chính ra bản phụ để xử lý
        /// Cần chạy 1 câu Query để thêm dữ liệu từ bảng chính qua
        /// </summary>
        public int LayTongKhieuNaiArchiveCanUpdate2Solr(DateTime fromDate, DateTime toDate)
        {
            string whereClause = string.Format("NgayTiepNhanSort >={0} AND NgayTiepNhanSort <= {1} AND (TrangThai IS NULL OR TrangThai = 0 OR TrangThai = 1)",
                fromDate.ToString("yyyyMMdd"),
                toDate.ToString("yyyyMMdd"));

            string select = string.Format("COUNT(1)");

            var obj = ServiceKhieuNaiUpdateArchive.GetListDynamicToTable(select, whereClause, string.Empty).Rows[0][0];
            return Convert.ToInt32(obj);
        }

        public DataTable LayDanhSachKhieuNaiCanLuuTru(int numberTop, DateTime fromDate, DateTime toDate)
        {
            DataTable listKhieuNaiInfo = null;
            KhieuNaiImpl ctl = new KhieuNaiImpl();

            //string selectTotalWhere = string.Format("NgayTiepNhanSort >={0} AND NgayTiepNhanSort <= {1} AND ArchiveId = 0 AND TrangThai = 3", fromDate.ToString("yyyyMMdd"), toDate.ToString("yyyyMMdd"));
            //DataTable tblSoLuong = ctl.GetListDynamicToTable("COUNT(1)", selectTotalWhere, string.Empty);
            //int soLuong = Convert.ToInt32(tblSoLuong.Rows[0][0]);

            string select = numberTop <= 0 ? "*" : string.Format("TOP {0} *", numberTop);
            string whereClause = string.Format("NgayTiepNhanSort >={0} AND NgayTiepNhanSort <= {1} AND ArchiveId = 0 AND TrangThai = 3", fromDate.ToString("yyyyMMdd"), toDate.ToString("yyyyMMdd"));
            listKhieuNaiInfo = ctl.GetListDynamicToTable(select, whereClause, "Id ASC");
            return listKhieuNaiInfo;
        }

        public int LayTongSoLuongKhieuNaiCanLuuTru(DateTime fromDate, DateTime toDate)
        {
            string select = "COUNT(1)";
            KhieuNaiImpl ctl = new KhieuNaiImpl();
            string whereClause = string.Format("NgayTiepNhanSort >={0} AND NgayTiepNhanSort <= {1} AND ArchiveId = 0 AND TrangThai = 3", fromDate.ToString("yyyyMMdd"), toDate.ToString("yyyyMMdd"));
            object count = ctl.GetListDynamicToTable(select, whereClause, string.Empty).Rows[0][0];
            return Convert.ToInt32(count);
        }

        private List<int> CapNhatDanhSachCanLuuTru(DataTable listKhieuNaiArchived, DateTime fromDate, DateTime toDate)
        {
            List<int> listError = new List<int>();
            int maxToCommitSolr = 500;

            if (this.ArchiveInfo != null) // Tìm thấy được cấu hình
            {
                List<int> listKhieuNaiId = new List<int>();

                // Danh sach se duoc xu ly
                List<DataRow> listBatchKhieuNai = new List<DataRow>();

                for (int i = 0; i < listKhieuNaiArchived.Rows.Count; i++)
                {
                    if (!IsContinue) break;

                    DataRow dr = listKhieuNaiArchived.Rows[i];
                    dr["ArchiveId"] = this.ArchiveInfo.Id;

                    listBatchKhieuNai.Add(dr);
                    listKhieuNaiId.Add((int)dr["Id"]);

                    if (listBatchKhieuNai.Count == maxToCommitSolr)
                    {
                        ShowText(string.Format("Copy {0}/{1} record", (i + 1), listKhieuNaiArchived.Rows.Count));

                        // Tiến hành xử lý dữ liệu trên Database
                        int retResult = CapNhatLuuTru_CacBang_Sql(listBatchKhieuNai, this.ArchiveInfo.Id);

                        // Thành công
                        if (retResult == 1)
                        {
                            // Commit Solr
                            // Quá lâu, không thể thực hiện được
                            if (Config.IsCommitSolr) CapNhatArchiveIdInSolr(listKhieuNaiId, this.ArchiveInfo.Id);

                            // Xóa dữ liệu trên bản chính
                            DeleteKhieuNaiCopy(listKhieuNaiId);
                        }
                        else
                        {
                            for (int j = 0; j < listBatchKhieuNai.Count; j++)
                            {
                                listError.Add((int)listBatchKhieuNai[j]["Id"]);
                            }
                        }

                        listBatchKhieuNai = new List<DataRow>();
                        listKhieuNaiId = new List<int>();
                    }
                } // end for (int i = 0; i < listKhieuNaiArchived.Count;i++ )

                if (listKhieuNaiId != null && listKhieuNaiId.Count > 0)
                {
                    ShowText(string.Format("Copy {0}/{1} record", listKhieuNaiArchived.Rows.Count, listKhieuNaiArchived.Rows.Count));
                    CapNhatLuuTru_CacBang_Sql(listBatchKhieuNai, this.ArchiveInfo.Id);

                    // Quá lâu, không thể thực hiện được
                    if (Config.IsCommitSolr) CapNhatArchiveIdInSolr(listKhieuNaiId, this.ArchiveInfo.Id);

                    // Thử với việc chưa xóa vội
                    DeleteKhieuNaiCopy(listKhieuNaiId);
                }
            }
            else
            {
                for (int i = 0; i < listKhieuNaiArchived.Rows.Count; i++)
                {
                    listError.Add((int)listKhieuNaiArchived.Rows[i]["Id"]);
                }
            }
            return listError;
        }

        private void DeleteKhieuNaiCopy(List<int> lstIdDelete)
        {
            string whereClause = string.Empty;
            for (int i = 0; i < lstIdDelete.Count; i++)
            {
                whereClause = string.Format("{0}{1},", whereClause, lstIdDelete[i]);
            }

            whereClause = whereClause.TrimEnd(',');
            string whereClauseId = string.Format("Id IN ({0})", whereClause);
            string whereClauseKhieuNaiId = string.Format("KhieuNaiId IN ({0})", whereClause);
            ShowText("Start delete after copy");
            using (TransactionScope scope = new TransactionScope())
            {
                try
                {
                    // Delete from KhieuNai_Activity
                    ServiceKhieuNaiActivty.DeleteDynamic(whereClauseKhieuNaiId);

                    // Delete from KhieuNai_BuocXuLy
                    ServiceKhieuNaiBuocXuLy.DeleteDynamic(whereClauseKhieuNaiId);

                    // Delete from KhieuNai_FileDinhKem
                    ServiceKhieuNaiFileDinhKem.DeleteDynamic(whereClauseKhieuNaiId);

                    // Delete from KhieuNai_GiaiPhap
                    //ServiceKhieuNaiActivty.DeleteDynamic(whereClauseSqlDeleteKhieuNaiIdId);

                    //Delete from KhieuNai_KetQuaXuLy
                    ServiceKhieuNaiKetQuaXuLy.DeleteDynamic(whereClauseKhieuNaiId);

                    // Delete from KhieuNai_Log
                    ServiceKhieuNaiLog.DeleteDynamic(whereClauseKhieuNaiId);

                    // Delete from KhieuNai_SoTien
                    ServiceKhieuNaiSoTien.DeleteDynamic(whereClauseKhieuNaiId);

                    // Delete from KhieuNai
                    ServiceKhieuNai.DeleteDynamic(whereClauseId);

                    scope.Complete();
                }
                catch (Exception ex)
                {
                    ShowText(ex.Message);
                    ShowText(ex.StackTrace);
                    throw;
                }
            }
            ShowText("Finish delete after copy");
        }

        /// <summary>
        /// Cập nhật lưu trữ cho các bảng dữ liệu
        /// 
        /// </summary>
        /// <param name="listKhieuNaiInfo">Danh sách dữ liệu</param>
        /// <returns>1: Thành công, -1: Thất bại</returns>
        private int CapNhatLuuTru_CacBang_Sql(List<DataRow> listKhieuNaiInfo, int archiveId)
        {
            if (listKhieuNaiInfo == null || listKhieuNaiInfo.Count == 0) return -1;

            List<string> dsKhieuNaiArchive = new List<string>();

            string whereClause = string.Empty;
            for (int i = 0; i < listKhieuNaiInfo.Count; i++)
            {
                whereClause = string.Format("{0}{1},", whereClause, listKhieuNaiInfo[i]["Id"]);
                dsKhieuNaiArchive.Add(listKhieuNaiInfo[i]["Id"].ToString());
            }
            Helper.GhiLogs("DanhSachKNArchive", string.Format("ArchiveId: {0}, KhieuNaiId: {1}", archiveId, string.Join(", ", dsKhieuNaiArchive)));

            whereClause = whereClause.TrimEnd(',');
            string whereClauseId = string.Format("Id IN ({0})", whereClause);
            string whereClauseKhieuNaiId = string.Format("KhieuNaiId IN ({0})", whereClause);

            ShowText("WHERE CLAUSE : " + whereClauseId);

            DataRow[] lstArrayKhieuNai = listKhieuNaiInfo.ToArray();

            ShowText("Start transaction Copy");
            using (TransactionScope scope = new TransactionScope())
            {
                try
                {
                    // Update lại ArchiveId
                    ServiceKhieuNai.UpdateDynamic("ArchiveId = " + this.ArchiveInfo.Id, whereClauseId);

                    ServiceKhieuNaiArchive.AddArchiveWithBulk(lstArrayKhieuNai, this.ColumnKhieuNai);

                    // Add Activity
                    DataTable listKhieuNaiActivity = ServiceKhieuNaiActivty.GetListDynamicToTable("*", whereClauseKhieuNaiId, "");
                    if (listKhieuNaiActivity != null)
                    {
                        ServiceKhieuNaiActivtyArchive.AddArchiveWithBulk(listKhieuNaiActivity, this.ColumnKhieuNaiActivity);
                    }

                    // Add from KhieuNai_BuocXuLy
                    DataTable listKhieuNaiBuocXuLy = ServiceKhieuNaiBuocXuLy.GetListDynamicToTable("*", whereClauseKhieuNaiId, "");
                    if (listKhieuNaiBuocXuLy != null)
                    {
                        ServiceKhieuNaiBuocXuLyArchive.AddArchiveWithBulk(listKhieuNaiBuocXuLy, this.ColumnKhieuNaiBuocXuLy);
                    }

                    // Add KhieuNai_FileDinhKem
                    DataTable listKhieuNaiFileDinhKem = ServiceKhieuNaiFileDinhKem.GetListDynamicToTable("*", whereClauseKhieuNaiId, "");
                    if (listKhieuNaiFileDinhKem != null)
                    {
                        ServiceKhieuNaiFileDinhKemArchive.AddArchiveWithBulk(listKhieuNaiFileDinhKem, this.ColumnKhieuNaiFileDinhKem);
                    }

                    // Add KhieuNai_KetQuaXuLy
                    DataTable listKhieuNaiKetQuaXuLy = ServiceKhieuNaiKetQuaXuLy.GetListDynamicToTable("*", whereClauseKhieuNaiId, "");
                    if (listKhieuNaiKetQuaXuLy != null)
                    {
                        ServiceKhieuNaiKetQuaXuLyArchive.AddArchiveWithBulk(listKhieuNaiKetQuaXuLy, this.ColumnKhieuNaiKetQuaXuLy);
                    }

                    // Add KhieuNai_Log
                    DataTable listKhieuNaiLog = ServiceKhieuNaiLog.GetListDynamicToTable("*", whereClauseKhieuNaiId, "");
                    if (listKhieuNaiLog != null)
                    {
                        ServiceKhieuNaiLogArchive.AddArchiveWithBulk(listKhieuNaiLog, this.ColumnKhieuNaiLog);
                    }

                    // Add KhieuNai_SoTien
                    DataTable listKhieuNaiSoTien = ServiceKhieuNaiSoTien.GetListDynamicToTable("*", whereClauseKhieuNaiId, "");
                    if (listKhieuNaiSoTien != null)
                    {
                        ServiceKhieuNaiSoTienArchive.AddArchiveWithBulk(listKhieuNaiSoTien, this.ColumnKhieuNaiSoTien);
                    }
                    scope.Complete();
                }
                catch (Exception ex)
                {
                    ShowText(ex.Message);
                    ShowText(ex.StackTrace);
                    throw ex;
                }
            } // end (TransactionScope scope = new TransactionScope())
            ShowText("Finish transaction Copy");

            return 1;
        }

        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created date : 15/07/2014
        /// Todo : Lấy thông tin Archive
        /// </summary>
        /// <param name="namLuuTru"></param>
        /// <returns></returns>
        private ArchiveConfigInfo GetArchiveConfig(int namLuuTru)
        {
            ArchiveConfigImpl archiveConfigImpl = new ArchiveConfigImpl();
            string whereClause = string.Format("NamLuuTru = {0}", namLuuTru);
            List<ArchiveConfigInfo> listArchiveConfigInfo = archiveConfigImpl.GetListDynamic("*", whereClause, "Id ASC");

            return listArchiveConfigInfo[0];
        }

        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created date : 22/07/2014
        /// Todo : Xóa các khiếu nại đã được đánh dấu IsArchived = 1
        ///     (thực hiện xóa theo lô)
        /// </summary>
        /// <param name="listKhieuNaiIdNeedDelete">
        ///     Tkey : KhieuNaiId
        ///     TValue : ArchiveId
        /// </param>
        /// <returns></returns>
        private List<int> DeleteKhieuNaiArchived_v2(Dictionary<int, int> dicKhieuNaiIdNeedDelete)
        {
            List<int> listError = new List<int>();
            int numberBatch = 100;

            Dictionary<int, int> dicBatch = new Dictionary<int, int>();
            int curArchiveId = 0;
            int count = 0;
            foreach (KeyValuePair<int, int> pair in dicKhieuNaiIdNeedDelete)
            {
                count++;
                if (dicBatch.Count == 0)
                {
                    dicBatch.Add(pair.Key, pair.Value);
                    curArchiveId = pair.Value;
                    continue;
                }

                if (dicBatch.Count == numberBatch || curArchiveId != pair.Value)
                {
                    ShowText(string.Format("Delete {0}/{1} record", (count - 1), dicKhieuNaiIdNeedDelete.Count));
                    List<int> listRetError = DeleteCompleteKhieuNaiArchived(dicBatch, curArchiveId);
                    if (listRetError != null && listRetError.Count > 0)
                    {
                        for (int i = 0; i < listRetError.Count; i++)
                        {
                            listError.Add(listRetError[i]);
                        }
                    }
                    dicBatch = new Dictionary<int, int>();
                    dicBatch.Add(pair.Key, pair.Value);
                    curArchiveId = pair.Value;
                } // end if(dicBatch.Count == numberBatch || curArchiveId != pair.Value)  
                else
                {
                    dicBatch.Add(pair.Key, pair.Value);
                }
            } // end for (int i = 0; i < listKhieuNaiIdNeedDelete.Count;i++ )     

            if (dicBatch.Count > 0)
            {
                ShowText(string.Format("Delete {0}/{1} record", dicKhieuNaiIdNeedDelete.Count, dicKhieuNaiIdNeedDelete.Count));
                List<int> listRetError = DeleteCompleteKhieuNaiArchived(dicBatch, curArchiveId);
                if (listRetError != null && listRetError.Count > 0)
                {
                    for (int i = 0; i < listRetError.Count; i++)
                    {
                        listError.Add(listRetError[i]);
                    }
                }
            }

            return listError;
        }

        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created date : 23/07/2014
        /// Todo : Thực hiện xóa các khiếu nại và commit archiveId lên solr
        /// </summary>
        /// <param name="dicBatch"></param>
        /// <returns>
        ///     Trả về danh sách KhieuNaiId không thực hiện xóa được
        /// </returns>
        private List<int> DeleteCompleteKhieuNaiArchived(Dictionary<int, int> dicBatch, int archiveId)
        {
            try
            {
                List<int> listError = new List<int>();

                //QuerySolrBase<KhieuNaiSolrInfo>.QuerySolr(URL_SOLR_GQKN, null, null); // Khởi tạo solr
                //ISolrOperations<KhieuNaiSolrInfo> solr = ServiceLocator.Current.GetInstance<ISolrOperations<KhieuNaiSolrInfo>>();
                //ISolrConnection solrConnection = ServiceLocator.Current.GetInstance<ISolrConnection>();

                string whereClauseId = string.Empty;
                foreach (KeyValuePair<int, int> item in dicBatch)
                {
                    whereClauseId = string.Format("{0}{1},", whereClauseId, item.Key);
                } // end foreach(KeyValuePair<int, int> item in dicBatch)

                whereClauseId = whereClauseId.TrimEnd(',');
                string whereClauseSqlDeleteId = string.Format("Id IN ({0})", whereClauseId);
                string whereClauseSqlDeleteKhieuNaiIdId = string.Format("KhieuNaiId IN ({0})", whereClauseId);
                string whereClauseSolr = string.Format("Id : ({0}) AND ArchiveId : 0", whereClauseId);
                ShowText("Id Delete:" + whereClauseId);

                //List<KhieuNaiSolrInfo> listKhieuNaiSolrInfo = new List<KhieuNaiSolrInfo>();
                //SolrQuery solrQuery = new SolrQuery(whereClauseSolr);
                //QueryOptions qoKhieuNai = new QueryOptions();
                //Dictionary<string, string> extraParamKhieuNai = new Dictionary<string, string>();
                //extraParamKhieuNai.Add("fl", @"Id, ArchiveId");

                //qoKhieuNai.ExtraParams = extraParamKhieuNai;

                //qoKhieuNai.Start = 0;
                //qoKhieuNai.Rows = int.MaxValue;
                //listKhieuNaiSolrInfo = QuerySolrBase<KhieuNaiSolrInfo>.QuerySolr(URL_SOLR_GQKN, solrQuery, qoKhieuNai);


                //if (listKhieuNaiSolrInfo != null && listKhieuNaiSolrInfo.Count > 0)
                //{
                //    CommitSolr(listKhieuNaiSolrInfo.Select(t => t.Id).ToList(), archiveId);
                //}

                ShowText("Start transaction Delete");
                using (TransactionScope scope = new TransactionScope())
                {
                    try
                    {
                        //Delete from KhieuNai_Activity
                        ServiceKhieuNaiActivty.DeleteDynamic(whereClauseSqlDeleteKhieuNaiIdId);

                        //Delete from KhieuNai_BuocXuLy
                        ServiceKhieuNaiBuocXuLy.DeleteDynamic(whereClauseSqlDeleteKhieuNaiIdId);

                        //Delete from KhieuNai_FileDinhKem
                        ServiceKhieuNaiFileDinhKem.DeleteDynamic(whereClauseSqlDeleteKhieuNaiIdId);

                        //Delete from KhieuNai_GiaiPhap
                        //ServiceKhieuNaiActivty.DeleteDynamic(whereClauseSqlDeleteKhieuNaiIdId);

                        //Delete from KhieuNai_KetQuaXuLy
                        ServiceKhieuNaiKetQuaXuLy.DeleteDynamic(whereClauseSqlDeleteKhieuNaiIdId);

                        //Delete from KhieuNai_Log
                        ServiceKhieuNaiLog.DeleteDynamic(whereClauseSqlDeleteKhieuNaiIdId);

                        //Delete from KhieuNai_SoTien
                        ServiceKhieuNaiSoTien.DeleteDynamic(whereClauseSqlDeleteKhieuNaiIdId);

                        //Delete from KhieuNai_Watchers
                        //KhieuNai_WatchersImpl khieuNaiWatchersImpl = new KhieuNai_WatchersImpl();
                        //khieuNaiWatchersImpl.DeleteDynamic(whereClauseSqlDeleteKhieuNaiIdId);

                        //Delete from KhieuNai
                        ServiceKhieuNai.DeleteDynamic(whereClauseSqlDeleteId);

                        scope.Complete();
                    }
                    catch (Exception ex)
                    {
                        ShowText(ex.Message);
                        ShowText(ex.StackTrace);
                        throw;
                    }
                } // end using (TransactionScope scope = new TransactionScope())
                ShowText("Finish transaction Delete");

                return listError;
            }
            catch (Exception ex)
            {
                ShowText(ex.Message);
                ShowText(ex.StackTrace);
                throw ex;
            }
        }

        public void CapNhatKhieuNaiArchive2Solr(DateTime fromDate, DateTime toDate, TextBox txtMessage, Label lblDelete, Label lblCopy, Label lblTotal, bool isRepeat2End)
        {
            try
            {
                this.txtMessage = txtMessage;
                this.lblDelete = lblDelete;
                this.lblCopy = lblCopy;
                this.lblTotal = lblTotal;

                // Tổng số lượng có được theo ngày tháng
                int totalCount = LayTongKhieuNaiArchiveCanUpdate2Solr(fromDate, toDate);

                while (true)
                {
                    if (IsContinue)
                    {
                        List<KhieuNai_UpdateArchiveInfo> listKhieuNaiUpdate = LayDanhSachKhieuNaiArchiveCanUpdate2Solr(Config.MaxNumberCommit2Solr, fromDate, toDate);

                        if (listKhieuNaiUpdate != null && listKhieuNaiUpdate.Count > 0)
                        {
                            ShowText("Số bản ghi cần Commit: " + listKhieuNaiUpdate.Count);

                            lblCopy.Text = (Convert.ToInt32(lblCopy.Text) + listKhieuNaiUpdate.Count).ToString();
                            lblTotal.Text = totalCount.ToString();

                            List<int> dsCanUpdate = new List<int>();
                            foreach (KhieuNai_UpdateArchiveInfo obj in listKhieuNaiUpdate) dsCanUpdate.Add(obj.KhieuNaiId);



                            ShowText(string.Format("Danh sách khiếu nại: ({0})", string.Join(", ", dsCanUpdate)));
                            Helper.GhiLogs("Query4Solr", "Id : ({0})", string.Join(" ", dsCanUpdate));

                            // Đẩy dữ liệu lên Solr
                            ShowText("Bắt đầu thực hiện commit");
                            CapNhatArchiveIdInSolr(dsCanUpdate, this.ArchiveInfo.Id);
                            ShowText("Kết thúc commit solr");

                            ShowText("Bắt đầu đánh dấu cập nhật database");
                            SqlCommand cmd = new SqlCommand(this.ArchiveInfo.ConnectionString);
                            cmd.CommandText = string.Format("UPDATE {0} SET TrangThai = 2 WHERE KhieuNaiId IN ({1})", ServiceKhieuNaiUpdateArchive.TableName, string.Join(",", dsCanUpdate));
                            cmd.CommandType = CommandType.Text;
                            ServiceKhieuNaiUpdateArchive.ExecuteNonQuery(cmd);
                            ShowText("Kết thúc cập nhật dữa liệu");

                        }
                        else // Thoát ra khi không còn
                        {
                            break;
                        }

                        // Không lặp thì thoát
                        if (!isRepeat2End) break;
                    }
                    else
                    {
                        break;
                    }
                    // End IsContinue
                }
                // End while(true)

                ShowText("Finish");
            }
            catch (Exception ex)
            {
                ShowText(ex.Message);
                ShowText(ex.StackTrace);
                throw ex;
            }
        }

        public void CapNhatArchiveIdInSolr(List<int> listKhieuNaiIds, int archiveId)
        {

            ShowText("Số bản ghi update lại solr: " + listKhieuNaiIds.Count);
            ShowText("Start Comit Solr");

            string core = "GQKN";
            string url = string.Format("{0}{1}/update?commit=true", Config.ServerSolr, core);

            HttpWebRequest httpWebRequest = (HttpWebRequest)HttpWebRequest.Create(url);

            List<object> objs = new List<object>();
            foreach (int khieuNaiId in listKhieuNaiIds)
            {
                var objKhieuNai = new
                {
                    Id = khieuNaiId.ToString(),
                    ArchiveId = new
                    {
                        set = archiveId
                    }
                };
                objs.Add(objKhieuNai);
            }

            string data = Newtonsoft.Json.JsonConvert.SerializeObject(objs);

            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";
            using (StreamWriter sw = new StreamWriter(httpWebRequest.GetRequestStream())) sw.Write(data);

            HttpWebResponse httpWebResponse = httpWebRequest.GetResponse() as HttpWebResponse;
            Stream stream = httpWebResponse.GetResponseStream();

            StreamReader responseReader = new StreamReader(httpWebResponse.GetResponseStream());

            // Cũng trả về kiểu Json
            string responseData = responseReader.ReadToEnd();

            Helper.GhiLogs("ResponseUpdateSolr", Newtonsoft.Json.JsonConvert.DeserializeObject(responseData));

            // XmlDocument doc = new XmlDocument();
            // doc.Load(stream);

            ShowText("Finish Comit Solr");
        }

        /// <summary>
        /// Request chờ phản hồi 5 giây
        /// </summary>
        private HttpWebResponse RequestSolr(string url)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            request.KeepAlive = true;
            // request.Timeout = 5 * 1000; // 5 giây
            request.ContentType = "application/x-www-form-urlencoded";
            request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
            request.UserAgent = "Mozilla/5.0 (Windows; U; Windows NT 5.1; en-US; rv:1.8.0.1) Gecko/20060111 Firefox/1.5.0.1";
            request.CookieContainer = new CookieContainer();

            try
            {
                return (HttpWebResponse)request.GetResponse();
            }
            catch (Exception ex)
            {
                ShowText(url);
                ShowText(ex.Message);
                ShowText(ex.StackTrace);
                Helper.GhiLogs(ex);
                throw ex;
            }
        }

        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created date : 15/07/2014
        /// Todo : Lấy khiếu nại từ solr theo khieuNaiId từ DB
        /// </summary>
        /// <param name="khieuNaiId"></param>
        /// <returns></returns>
        private KhieuNaiSolrInfo LayKhieuNaiTuSolr(int khieuNaiId)
        {
            List<KhieuNaiSolrInfo> listKhieuNaiSolrInfo = null;
            string whereClause = string.Format("Id : {0}", khieuNaiId);
            SolrQuery solrQuery = new SolrQuery(whereClause);
            QueryOptions qoKhieuNai = new QueryOptions();
            Dictionary<string, string> extraParamKhieuNai = new Dictionary<string, string>();
            extraParamKhieuNai.Add("fl", @"Id, ArchiveId");

            qoKhieuNai.ExtraParams = extraParamKhieuNai;

            qoKhieuNai.Start = 0;
            qoKhieuNai.Rows = int.MaxValue;
            listKhieuNaiSolrInfo = QuerySolrBase<KhieuNaiSolrInfo>.QuerySolr(URL_SOLR_GQKN, solrQuery, qoKhieuNai);

            return listKhieuNaiSolrInfo[0];
        }

        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created date : 15/07/2014
        /// Todo : Lấy danh sách khiếu nại từ solr theo danh sách KhieuNaiId từ DB
        /// </summary>
        /// <param name="listKhieuNaiId"></param>
        /// <returns></returns>
        private List<KhieuNaiSolrInfo> LayDanhSachKhieuNaiTuSolr(List<int> listKhieuNaiId)
        {
            if (listKhieuNaiId == null || listKhieuNaiId.Count == 0) return null;

            List<KhieuNaiSolrInfo> listKhieuNaiSolrInfo = null;
            string whereClause = string.Empty;// string.Format("Id : {0}", khieuNaiId);
            for (int i = 0; i < listKhieuNaiId.Count; i++)
            {
                whereClause = string.Format("{0}{1} ", whereClause, listKhieuNaiId[i]);
            }

            whereClause = whereClause.TrimEnd();
            whereClause = string.Format("Id : ({0})", whereClause);

            SolrQuery solrQuery = new SolrQuery(whereClause);
            QueryOptions qoKhieuNai = new QueryOptions();
            Dictionary<string, string> extraParamKhieuNai = new Dictionary<string, string>();
            extraParamKhieuNai.Add("fl", @"*");

            qoKhieuNai.ExtraParams = extraParamKhieuNai;

            qoKhieuNai.Start = 0;
            qoKhieuNai.Rows = int.MaxValue;
            listKhieuNaiSolrInfo = QuerySolrBase<KhieuNaiSolrInfo>.QuerySolr(URL_SOLR_GQKN, solrQuery, qoKhieuNai);

            return listKhieuNaiSolrInfo;
        }
    }
}
