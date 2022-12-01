using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Website.AppCode;
using AIVietNam.Core;
using SolrNet;
using SolrNet.Commands.Parameters;
using AIVietNam.GQKN.Entity;
using AIVietNam.GQKN.Impl;
using Aspose.Cells;
using System.IO;
using AIVietNam.Admin;
using System.Globalization;
using Website.Components;

namespace Website.BaoCao
{
    public partial class PopupDetail : Page
    {
        // Tên phục vụ kiểm tra xuất excel cho từng loại báo cáo
        private string ReportType
        {
            get { return ViewState["ReportType"] != null ? (string)ViewState["ReportType"] : string.Empty; }
            set { ViewState.Add("ReportType", value); }
        }
        private string ReportName
        {
            get { return ViewState["ReportName"] != null ? (string)ViewState["ReportName"] : string.Empty; }
            set { ViewState.Add("ReportName", value); }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            LoginAdmin.IsLoginAdmin();
            AdminInfo UserInfo = LoginAdmin.AdminLogin();

            liJs.Text = string.Empty;
            btnExportExel.Click += BtnExportExel_Click;
            btnExportExcel2.Click += BtnExportExel_Click;

            if (!IsPostBack)
            {
                CustomMessage ret = new CustomMessage();

                string type = Request.QueryString["Type"];
                if (!string.IsNullOrEmpty(type))
                {
                    type = type.ToLower();

                    // Báo cáo dịch vụ GTGT cho tập đoàn
                    if (type == "BaoCaoDichVuGTGTChoTapDoan".ToLower())
                    {
                        this.ReportType = "BaoCaoDichVuGTGTChoTapDoan";
                        ret = BaoCaoDichVuGTGTChoTapDoan();
                        if (ret.Code == CustomCode.OK)
                        {
                            PnlMain.Visible = true;
                            GrvViewTongHopPAKN.DataSource = ret.DataEx;
                            GrvViewTongHopPAKN.DataBind();

                            liTitle.Text = string.Format("Danh sách chi tiết khiếu nại");
                            liTime.Text = string.Format("Thời gian báo cáo tháng {0} năm {1}", Request.QueryString["Thang"], Request.QueryString["Nam"]);
                            liMess.Text = string.Format("Tìm thấy tổng số <span>{0}</span> khiếu nại", ret.Number);
                        }
                        else
                        {
                            liTitle.Text = ret.Message;
                        }

                    }
                    // Báo cáo tổng hợp PAKN
                    else if (type == "BaoCaoTongHopPAKN".ToLower())
                    {

                    }
                    // Báo cáo tổng hợp VNPT-NET
                    else if (type.ToLower() == "BaoCaoTongHopVNPTNET".ToLower())
                    {
                        ReportType = "BaoCaoTongHopVNPTNET";
                        ReportName = Request.QueryString["ReportName"];

                        ret = BaoCaoTongHopVNPTNET();
                        if (ret.Code == CustomCode.OK) // Lấy dữ liệu thành công
                        {
                            liTitle.Text = string.Format("Danh sách chi tiết khiếu nại");
                            liTime.Text = string.Format("Thời gian báo cáo từ {0} đến {1}", HttpUtility.UrlDecode(Request.QueryString["FromDate"]), HttpUtility.UrlDecode(Request.QueryString["ToDate"]));

                            if (ReportName.ToLower() == "SLTonDongQuaHan".ToLower()) liMess.Text = string.Format("Tìm thấy tổng số <span>{0}</span> khiếu nại tồn đọng quá hạn", ret.Number);
                            else if (ReportName.ToLower() == "SLTonDong".ToLower()) liMess.Text = string.Format("Tìm thấy tổng số <span>{0}</span> khiếu nại tồn đọng", ret.Number);
                            else if (ReportName.ToLower() == "SLTiepNhan".ToLower()) liMess.Text = string.Format("Tìm thấy tổng số <span>{0}</span> khiếu nại được tiếp nhận", ret.Number);
                            else if (ReportName.ToLower() == "SLTonDongKyTruoc".ToLower()) liMess.Text = string.Format("Tìm thấy tổng số <span>{0}</span> khiếu nại tồn đọng kỳ trước", ret.Number);
                            else if (ReportName.ToLower() == "SLDaXuLyTiepNhan".ToLower()) liMess.Text = string.Format("Tìm thấy tổng số <span>{0}</span> khiếu nại tiếp nhận đã xử lý", ret.Number);

                            else if (ReportName.ToLower() == "SLDaXuLyLuyKe".ToLower()) liMess.Text = string.Format("Tìm thấy tổng số <span>{0}</span> khiếu nại đã xử lý lũy kế", ret.Number);
                            else if (ReportName.ToLower() == "SLQuaHanDaXuLy".ToLower()) liMess.Text = string.Format("Tìm thấy tổng số <span>{0}</span> khiếu nại quá hạn đã xử lý", ret.Number);


                            PnlMain.Visible = true;
                            if (ret.Number == 0) btnExportExel.Visible = btnExportExcel2.Visible = false;

                            RenderGridViewTemplate(GrvDanhSach, ret.DataTemplate as List<CustomTemplate>);
                            GrvDanhSach.DataSource = ret.DataEx;
                            GrvDanhSach.DataBind();
                        }
                        else
                        {
                            liTitle.Text = ret.Message;
                        }
                    }
                }
            }
        }
        private CustomMessage BaoCaoTongHopVNPTNET()
        {
            CustomMessage retValue = new CustomMessage();

            int doiTacId = 0;
            if (int.TryParse(Request.QueryString["DoiTacId"], out doiTacId))
            {
                string strFromDate = Request.QueryString["FromDate"];
                if (!string.IsNullOrEmpty(strFromDate))
                {
                    string strToDate = Request.QueryString["ToDate"];
                    if (!string.IsNullOrEmpty(strToDate))
                    {
                        try
                        {
                            DateTime fromDate = DateTime.Now;
                            DateTime toDate = DateTime.Now;
                            bool isDateTime = true;
                            try
                            {
                                // Kiểm tra ngày tháng có đúng cú pháp
                                fromDate = Convert.ToDateTime(strFromDate, new CultureInfo("vi-VN"));
                                toDate = Convert.ToDateTime(strToDate, new CultureInfo("vi-VN"));
                            }
                            catch (Exception ex)
                            {
                                Helper.GhiLogs(ex);
                                isDateTime = false;
                            }
                            if (isDateTime)
                            {
                                #region "Xử lý dữ liệu"
                                // Có đủ đối tác, ngày đầu, ngày cuối báo cáo hợp lệ
                                string reportName = Request.QueryString["ReportName"];

                                if (!string.IsNullOrEmpty(reportName))
                                {

                                    if (reportName.ToLower() == "SLTonDongKyTruoc".ToLower())
                                    {
                                        #region "Số lượng tồn đọng kỳ trước"
                                        //  List<KhieuNai_ReportInfo> lst = LayKhieuNaiTonDongKyTruoc(doiTacId, fromDate, toDate);
                                        List<KhieuNai_ReportInfo> lst = new BaoCaoPAKNImpl().LayKhieuNaiTonDongKyTruoc(CapBaoCaoEnum.KhuVuc, doiTacId, fromDate, toDate);
                                        if (lst != null) // Có dữ liệu
                                        {

                                            retValue.Number = lst.Count;
                                            retValue.Data = lst;
                                            retValue.DataEx = lst.OrderBy(v => v.NgayTiepNhan).Select((v, Index) => new
                                            {
                                                STT = Index + 1,
                                                Id = v.Id,
                                                KhieuNaiId = v.KhieuNaiId.ToString("PA-0000000000"),
                                                // ChiTietLoiId = v.ChiTietLoiId,
                                                // ChiTietLoi = GetChiTietLoi(v.ChiTietLoiId),
                                                SoThueBao = v.SoThueBao,
                                                NguoiTiepNhan = v.NguoiXuLy,
                                                TenPhongBanXuLy = v.TenPhongBanXuLy,
                                                // LinhVucCon = v.LinhVucCon,
                                                // NoiDungPA = LimitExcelContent(v.NoiDungPA),
                                                // NoiDungXuLy = LimitExcelContent(v.NoiDungXuLyDongKN),
                                                NgayTiepNhan = v.NgayTiepNhan.ToString("dd/MM/yyyy HH:mm"),
                                                NgayDongKN = v.NgayDongKN.ToString("dd/MM/yyyy HH:mm"),
                                                NgayQuaHan = v.NgayQuaHan.ToString("dd/MM/yyyy HH:mm"),
                                                NgayXuLy = v.LDate.ToString("dd/MM/yyyy HH:mm")
                                            });

                                            List<CustomTemplate> lstTempates = new List<CustomTemplate>();
                                            lstTempates.Add(new CustomTemplate() { STT = 1, Name = "STT", DataField = "STT" });
                                            lstTempates.Add(new CustomTemplate() { STT = 2, Name = "Acitivity Id", DataField = "Id" });
                                            lstTempates.Add(new CustomTemplate() { STT = 3, Name = "Mã phản ánh", DataField = "KhieuNaiId" });
                                            lstTempates.Add(new CustomTemplate() { STT = 4, Name = "Số thuê bao", DataField = "SoThueBao" });

                                            lstTempates.Add(new CustomTemplate() { STT = 5, Name = "Ngày tiếp nhận", DataField = "NgayTiepNhan" });
                                            lstTempates.Add(new CustomTemplate() { STT = 6, Name = "Người tiếp nhận", DataField = "NguoiTiepNhan" });
                                            lstTempates.Add(new CustomTemplate() { STT = 7, Name = "Phong ban tiếp nhận", DataField = "TenPhongBanXuLy" });

                                            lstTempates.Add(new CustomTemplate() { STT = 8, Name = "Ngày chuyển phản ánh", DataField = "NgayXuLy" });
                                            lstTempates.Add(new CustomTemplate() { STT = 9, Name = "Ngày hết hạn", DataField = "NgayQuaHan" });
                                            retValue.DataTemplate = lstTempates;
                                        }
                                        retValue.Code = CustomCode.OK;
                                        retValue.Message = string.Format("Thành công");
                                        #endregion
                                    }
                                    else if (reportName.ToLower() == "SLTiepNhan".ToLower())
                                    {
                                        #region "Số lượng tiếp nhận"
                                        List<KhieuNai_ReportInfo> listKhieuNaiInfo = new BaoCaoPAKNImpl().LayKhieuNaiTiepNhan(CapBaoCaoEnum.KhuVuc, doiTacId, fromDate, toDate); // LayKhieuNaiTiepNhan(doiTacId, fromDate, toDate);
                                        if (listKhieuNaiInfo != null) // Có dữ liệu
                                        {
                                            retValue.Number = listKhieuNaiInfo.Count;
                                            retValue.Data = listKhieuNaiInfo;
                                            retValue.DataEx = listKhieuNaiInfo.OrderBy(v => v.NgayTiepNhan).Select((v, Index) => new
                                            {
                                                STT = Index + 1,
                                                Id = v.Id,
                                                KhieuNaiId = v.KhieuNaiId.ToString("PA-0000000000"),
                                                // ChiTietLoiId = v.ChiTietLoiId,
                                                // ChiTietLoi = GetChiTietLoi(v.ChiTietLoiId),
                                                SoThueBao = v.SoThueBao,
                                                NguoiTiepNhan = v.NguoiXuLy,
                                                TenPhongBanXuLy = v.TenPhongBanXuLy,
                                                // LinhVucCon = v.LinhVucCon,
                                                // NoiDungPA = LimitExcelContent(v.NoiDungPA),
                                                // NoiDungXuLy = LimitExcelContent(v.NoiDungXuLyDongKN),
                                                NgayTiepNhan = v.NgayTiepNhan.ToString("dd/MM/yyyy HH:mm"),
                                                NgayDongKN = v.NgayDongKN.ToString("dd/MM/yyyy HH:mm"),
                                                NgayQuaHan = v.NgayQuaHan.ToString("dd/MM/yyyy HH:mm"),
                                                NgayXuLy = v.LDate.ToString("dd/MM/yyyy HH:mm")
                                            });

                                            List<CustomTemplate> lstTempates = new List<CustomTemplate>();
                                            lstTempates.Add(new CustomTemplate() { STT = 1, Name = "STT", DataField = "STT" });
                                            lstTempates.Add(new CustomTemplate() { STT = 2, Name = "Acitivity Id", DataField = "Id" });

                                            lstTempates.Add(new CustomTemplate() { STT = 3, Name = "Mã phản ánh", DataField = "KhieuNaiId" });
                                            lstTempates.Add(new CustomTemplate() { STT = 4, Name = "Số thuê bao", DataField = "SoThueBao" });
                                            lstTempates.Add(new CustomTemplate() { STT = 5, Name = "Ngày tiếp nhận", DataField = "NgayTiepNhan" });

                                            lstTempates.Add(new CustomTemplate() { STT = 6, Name = "Người tiếp nhận", DataField = "NguoiTiepNhan" });
                                            lstTempates.Add(new CustomTemplate() { STT = 7, Name = "Phòng ban tiếp nhận", DataField = "TenPhongBanXuLy" });

                                            lstTempates.Add(new CustomTemplate() { STT = 8, Name = "Ngày chuyển phản ánh", DataField = "NgayXuLy" });
                                            lstTempates.Add(new CustomTemplate() { STT = 9, Name = "Ngày hết hạn", DataField = "NgayQuaHan" });
                                            retValue.DataTemplate = lstTempates;

                                        }
                                        retValue.Code = CustomCode.OK;
                                        retValue.Message = string.Format("Thành công");
                                        #endregion
                                    }
                                    else if (reportName.ToLower() == "SLDaXuLyTiepNhan".ToLower())
                                    {
                                        #region "SL đã xử lý (tiếp nhận)"
                                        List<KhieuNai_ReportInfo> listKhieuNaiInfo = new BaoCaoPAKNImpl().LayKhieuNaiDaXuLy_TiepNhan(CapBaoCaoEnum.KhuVuc, doiTacId, fromDate, toDate);
                                        if (listKhieuNaiInfo != null) // Có dữ liệu
                                        {

                                            retValue.Number = listKhieuNaiInfo.Count;
                                            retValue.Data = listKhieuNaiInfo;
                                            retValue.DataEx = listKhieuNaiInfo.OrderBy(v => v.NgayTiepNhan).Select((v, Index) => new
                                            {
                                                STT = Index + 1,
                                                Id = v.Id,
                                                KhieuNaiId = v.KhieuNaiId.ToString("PA-0000000000"),
                                                // ChiTietLoiId = v.ChiTietLoiId,
                                                // ChiTietLoi = GetChiTietLoi(v.ChiTietLoiId),
                                                SoThueBao = v.SoThueBao,
                                                NguoiTiepNhan = v.NguoiXuLy,
                                                TenPhongBanXuLy = v.TenPhongBanXuLy,
                                                // LinhVucCon = v.LinhVucCon,
                                                // NoiDungPA = LimitExcelContent(v.NoiDungPA),
                                                // NoiDungXuLy = LimitExcelContent(v.NoiDungXuLyDongKN),
                                                NgayTiepNhan = v.NgayTiepNhan.ToString("dd/MM/yyyy HH:mm"),
                                                NgayDongKN = v.NgayDongKN.ToString("dd/MM/yyyy HH:mm"),
                                                NgayQuaHan = v.NgayQuaHan.ToString("dd/MM/yyyy HH:mm"),
                                                NgayXuLy = v.LDate.ToString("dd/MM/yyyy HH:mm")
                                            });


                                            List<CustomTemplate> lstTempates = new List<CustomTemplate>();
                                            lstTempates.Add(new CustomTemplate() { STT = 1, Name = "STT", DataField = "STT" });
                                            lstTempates.Add(new CustomTemplate() { STT = 2, Name = "Acitivity Id", DataField = "Id" });

                                            lstTempates.Add(new CustomTemplate() { STT = 3, Name = "Mã phản ánh", DataField = "KhieuNaiId" });
                                            lstTempates.Add(new CustomTemplate() { STT = 4, Name = "Số thuê bao", DataField = "SoThueBao" });

                                            lstTempates.Add(new CustomTemplate() { STT = 5, Name = "Người tiếp nhận", DataField = "NguoiTiepNhan" });
                                            lstTempates.Add(new CustomTemplate() { STT = 6, Name = "Phòng ban tiếp nhận", DataField = "TenPhongBanXuLy" });

                                            lstTempates.Add(new CustomTemplate() { STT = 7, Name = "Ngày tiếp nhận", DataField = "NgayTiepNhan" });

                                            lstTempates.Add(new CustomTemplate() { STT = 8, Name = "Ngày chuyển phản ánh", DataField = "NgayXuLy" });
                                            lstTempates.Add(new CustomTemplate() { STT = 9, Name = "Ngày hết hạn", DataField = "NgayQuaHan" });
                                            retValue.DataTemplate = lstTempates;

                                        }
                                        retValue.Code = CustomCode.OK;
                                        retValue.Message = string.Format("Thành công");
                                        #endregion
                                    }

                                    else if (reportName.ToLower() == "SLDaXuLyLuyKe".ToLower())
                                    {
                                        #region "SL đã xử lý (lũy kế)"
                                        List<KhieuNai_ReportInfo> listKhieuNaiInfo = new BaoCaoPAKNImpl().LayKhieuNaiDaXuLy_LuyKe(CapBaoCaoEnum.KhuVuc, doiTacId, fromDate, toDate);
                                        if (listKhieuNaiInfo != null) // Có dữ liệu
                                        {

                                            retValue.Number = listKhieuNaiInfo.Count;
                                            retValue.Data = listKhieuNaiInfo;
                                            retValue.DataEx = listKhieuNaiInfo.OrderBy(v => v.NgayTiepNhan).Select((v, Index) => new
                                            {
                                                STT = Index + 1,
                                                Id = v.Id,
                                                KhieuNaiId = v.KhieuNaiId.ToString("PA-0000000000"),
                                                // ChiTietLoiId = v.ChiTietLoiId,
                                                // ChiTietLoi = GetChiTietLoi(v.ChiTietLoiId),
                                                SoThueBao = v.SoThueBao,
                                                NguoiTiepNhan = v.NguoiXuLy,
                                                TenPhongBanXuLy = v.TenPhongBanXuLy,
                                                // LinhVucCon = v.LinhVucCon,
                                                // NoiDungPA = LimitExcelContent(v.NoiDungPA),
                                                // NoiDungXuLy = LimitExcelContent(v.NoiDungXuLyDongKN),
                                                NgayTiepNhan = v.NgayTiepNhan.ToString("dd/MM/yyyy HH:mm"),
                                                NgayDongKN = v.NgayDongKN.ToString("dd/MM/yyyy HH:mm"),
                                                NgayQuaHan = v.NgayQuaHan.ToString("dd/MM/yyyy HH:mm"),
                                                NgayXuLy = v.LDate.ToString("dd/MM/yyyy HH:mm")
                                            });


                                            List<CustomTemplate> lstTempates = new List<CustomTemplate>();
                                            lstTempates.Add(new CustomTemplate() { STT = 1, Name = "STT", DataField = "STT" });
                                            lstTempates.Add(new CustomTemplate() { STT = 2, Name = "Acitivity Id", DataField = "Id" });

                                            lstTempates.Add(new CustomTemplate() { STT = 3, Name = "Mã phản ánh", DataField = "KhieuNaiId" });
                                            lstTempates.Add(new CustomTemplate() { STT = 4, Name = "Số thuê bao", DataField = "SoThueBao" });

                                            lstTempates.Add(new CustomTemplate() { STT = 5, Name = "Người tiếp nhận", DataField = "NguoiTiepNhan" });
                                            lstTempates.Add(new CustomTemplate() { STT = 6, Name = "Phòng ban tiếp nhận", DataField = "TenPhongBanXuLy" });

                                            lstTempates.Add(new CustomTemplate() { STT = 7, Name = "Ngày tiếp nhận", DataField = "NgayTiepNhan" });

                                            lstTempates.Add(new CustomTemplate() { STT = 8, Name = "Ngày chuyển phản ánh", DataField = "NgayXuLy" });
                                            lstTempates.Add(new CustomTemplate() { STT = 9, Name = "Ngày hết hạn", DataField = "NgayQuaHan" });
                                            retValue.DataTemplate = lstTempates;

                                        }
                                        retValue.Code = CustomCode.OK;
                                        retValue.Message = string.Format("Thành công");
                                        #endregion
                                    }
                                    else if (reportName.ToLower() == "SLQuaHanDaXuLy".ToLower())
                                    {
                                        #region "SL quá hạn đã xử lý"
                                        List<KhieuNai_ReportInfo> listKhieuNaiInfo = new BaoCaoPAKNImpl().LayKhieuNaiDaXuLy_QuaHan(CapBaoCaoEnum.KhuVuc, doiTacId, fromDate, toDate);
                                        if (listKhieuNaiInfo != null) // Có dữ liệu
                                        {

                                            retValue.Number = listKhieuNaiInfo.Count;
                                            retValue.Data = listKhieuNaiInfo;
                                            retValue.DataEx = listKhieuNaiInfo.OrderBy(v => v.NgayTiepNhan).Select((v, Index) => new
                                            {
                                                STT = Index + 1,
                                                Id = v.Id,
                                                KhieuNaiId = v.KhieuNaiId.ToString("PA-0000000000"),
                                                // ChiTietLoiId = v.ChiTietLoiId,
                                                // ChiTietLoi = GetChiTietLoi(v.ChiTietLoiId),
                                                SoThueBao = v.SoThueBao,
                                                NguoiTiepNhan = v.NguoiXuLy,
                                                TenPhongBanXuLy = v.TenPhongBanXuLy,
                                                // LinhVucCon = v.LinhVucCon,
                                                // NoiDungPA = LimitExcelContent(v.NoiDungPA),
                                                // NoiDungXuLy = LimitExcelContent(v.NoiDungXuLyDongKN),
                                                NgayTiepNhan = v.NgayTiepNhan.ToString("dd/MM/yyyy HH:mm"),
                                                NgayDongKN = v.NgayDongKN.ToString("dd/MM/yyyy HH:mm"),
                                                NgayQuaHan = v.NgayQuaHan.ToString("dd/MM/yyyy HH:mm"),
                                                NgayXuLy = v.LDate.ToString("dd/MM/yyyy HH:mm")
                                            });


                                            List<CustomTemplate> lstTempates = new List<CustomTemplate>();
                                            lstTempates.Add(new CustomTemplate() { STT = 1, Name = "STT", DataField = "STT" });
                                            lstTempates.Add(new CustomTemplate() { STT = 2, Name = "Acitivity Id", DataField = "Id" });

                                            lstTempates.Add(new CustomTemplate() { STT = 3, Name = "Mã phản ánh", DataField = "KhieuNaiId" });
                                            lstTempates.Add(new CustomTemplate() { STT = 4, Name = "Số thuê bao", DataField = "SoThueBao" });

                                            lstTempates.Add(new CustomTemplate() { STT = 5, Name = "Người tiếp nhận", DataField = "NguoiTiepNhan" });
                                            lstTempates.Add(new CustomTemplate() { STT = 6, Name = "Phòng ban tiếp nhận", DataField = "TenPhongBanXuLy" });

                                            lstTempates.Add(new CustomTemplate() { STT = 7, Name = "Ngày tiếp nhận", DataField = "NgayTiepNhan" });

                                            lstTempates.Add(new CustomTemplate() { STT = 8, Name = "Ngày chuyển phản ánh", DataField = "NgayXuLy" });
                                            lstTempates.Add(new CustomTemplate() { STT = 9, Name = "Ngày hết hạn", DataField = "NgayQuaHan" });
                                            retValue.DataTemplate = lstTempates;

                                        }
                                        retValue.Code = CustomCode.OK;
                                        retValue.Message = string.Format("Thành công");
                                        #endregion
                                    }
                                    else if (reportName.ToLower() == "SLTonDongQuaHan".ToLower())
                                    {
                                        #region "Số lượng tồn đọng quá hạn"
                                        List<KhieuNai_ReportInfo> objData = new BaoCaoPAKNImpl().LayKhieuNaiTonDongQuaHan(CapBaoCaoEnum.KhuVuc, doiTacId, toDate);

                                        if (objData != null) // Có dữ liệu
                                        {

                                            retValue.Number = objData.Count;
                                            retValue.Data = objData;
                                            retValue.DataEx = objData.OrderBy(v => v.NgayTiepNhan).Select((v, Index) => new
                                            {
                                                STT = Index + 1,
                                                Id = v.Id,
                                                KhieuNaiId = v.KhieuNaiId.ToString("PA-0000000000"),
                                                // ChiTietLoiId = v.ChiTietLoiId,
                                                // ChiTietLoi = GetChiTietLoi(v.ChiTietLoiId),
                                                SoThueBao = v.SoThueBao,
                                                NguoiTiepNhan = v.NguoiXuLy,
                                                TenPhongBanXuLy = v.TenPhongBanXuLy,
                                                // LinhVucCon = v.LinhVucCon,
                                                // NoiDungPA = LimitExcelContent(v.NoiDungPA),
                                                // NoiDungXuLy = LimitExcelContent(v.NoiDungXuLyDongKN),
                                                NgayTiepNhan = v.NgayTiepNhan.ToString("dd/MM/yyyy HH:mm"),
                                                NgayDongKN = v.NgayDongKN.ToString("dd/MM/yyyy HH:mm"),
                                                NgayQuaHan = v.NgayQuaHan.ToString("dd/MM/yyyy HH:mm"),
                                                NgayXuLy = (!v.IsCurrent ? v.LDate.ToString("dd/MM/yyyy HH:mm") : null)
                                            });


                                            List<CustomTemplate> lstTempates = new List<CustomTemplate>();
                                            lstTempates.Add(new CustomTemplate() { STT = 1, Name = "STT", DataField = "STT" });
                                            lstTempates.Add(new CustomTemplate() { STT = 2, Name = "Acitivity Id", DataField = "Id" });

                                            lstTempates.Add(new CustomTemplate() { STT = 3, Name = "Mã phản ánh", DataField = "KhieuNaiId" });
                                            lstTempates.Add(new CustomTemplate() { STT = 4, Name = "Số thuê bao", DataField = "SoThueBao" });

                                            lstTempates.Add(new CustomTemplate() { STT = 5, Name = "Người tiếp nhận", DataField = "NguoiTiepNhan" });
                                            lstTempates.Add(new CustomTemplate() { STT = 6, Name = "Phòng ban tiếp nhận", DataField = "TenPhongBanXuLy" });

                                            lstTempates.Add(new CustomTemplate() { STT = 7, Name = "Ngày tiếp nhận", DataField = "NgayTiepNhan" });

                                            lstTempates.Add(new CustomTemplate() { STT = 8, Name = "Ngày chuyển phản ánh", DataField = "NgayXuLy" });
                                            lstTempates.Add(new CustomTemplate() { STT = 9, Name = "Ngày hết hạn", DataField = "NgayQuaHan" });
                                            retValue.DataTemplate = lstTempates;

                                        }
                                        retValue.Code = CustomCode.OK;
                                        retValue.Message = string.Format("Thành công");
                                        #endregion

                                    }
                                    else if (reportName.ToLower() == "SLTonDong".ToLower())
                                    {
                                        #region "Số lượng tồn đọng"
                                        List<KhieuNai_ReportInfo> listKhieuNaiInfo = new BaoCaoPAKNImpl().LayKhieuNaiTonDong(CapBaoCaoEnum.KhuVuc, doiTacId, toDate);

                                        // Số lượng tồn đọng
                                        if (listKhieuNaiInfo != null) // Có dữ liệu
                                        {
                                            retValue.Number = listKhieuNaiInfo.Count;
                                            retValue.Data = listKhieuNaiInfo;
                                            retValue.DataEx = listKhieuNaiInfo.OrderBy(v => v.NgayTiepNhan).Select((v, Index) => new
                                            {
                                                STT = Index + 1,
                                                Id = v.Id,
                                                KhieuNaiId = v.KhieuNaiId.ToString("PA-0000000000"),
                                                // ChiTietLoiId = v.ChiTietLoiId,
                                                // ChiTietLoi = GetChiTietLoi(v.ChiTietLoiId),
                                                SoThueBao = v.SoThueBao,
                                                NguoiTiepNhan = v.NguoiXuLy,
                                                TenPhongBanXuLy = v.TenPhongBanXuLy,
                                                // LinhVucCon = v.LinhVucCon,
                                                // NoiDungPA = LimitExcelContent(v.NoiDungPA),
                                                // NoiDungXuLy = LimitExcelContent(v.NoiDungXuLyDongKN),
                                                NgayTiepNhan = v.NgayTiepNhan.ToString("dd/MM/yyyy HH:mm"),
                                                NgayDongKN = v.NgayDongKN.ToString("dd/MM/yyyy HH:mm"),
                                                NgayQuaHan = v.NgayQuaHan.ToString("dd/MM/yyyy HH:mm"),
                                                NgayXuLy = v.IsCurrent ? null : v.LDate.ToString("dd/MM/yyyy HH:mm")
                                            });

                                            List<CustomTemplate> lstTempates = new List<CustomTemplate>();
                                            lstTempates.Add(new CustomTemplate() { STT = 1, Name = "STT", DataField = "STT" });
                                            lstTempates.Add(new CustomTemplate() { STT = 2, Name = "Acitivity Id", DataField = "Id" });

                                            lstTempates.Add(new CustomTemplate() { STT = 3, Name = "Mã phản ánh", DataField = "KhieuNaiId" });
                                            lstTempates.Add(new CustomTemplate() { STT = 4, Name = "Số thuê bao", DataField = "SoThueBao" });

                                            lstTempates.Add(new CustomTemplate() { STT = 5, Name = "Người tiếp nhận", DataField = "NguoiTiepNhan" });
                                            lstTempates.Add(new CustomTemplate() { STT = 6, Name = "Phòng ban tiếp nhận", DataField = "TenPhongBanXuLy" });

                                            lstTempates.Add(new CustomTemplate() { STT = 7, Name = "Ngày tiếp nhận", DataField = "NgayTiepNhan" });

                                            lstTempates.Add(new CustomTemplate() { STT = 8, Name = "Ngày chuyển phản ánh", DataField = "NgayXuLy" });
                                            lstTempates.Add(new CustomTemplate() { STT = 9, Name = "Ngày hết hạn", DataField = "NgayQuaHan" });
                                            retValue.DataTemplate = lstTempates;
                                        }
                                        retValue.Code = CustomCode.OK;
                                        retValue.Message = string.Format("Thành công");
                                        #endregion
                                    }
                                    else
                                    {
                                        retValue.Code = CustomCode.DuLieuKhongHopLe;
                                        retValue.Message = "Dữ liệu không hợp lệ, chưa cung cấp đủ thông tin báo cáo";
                                    }
                                }
                                else
                                {
                                    retValue.Code = CustomCode.DuLieuKhongHopLe;
                                    retValue.Message = "Dữ liệu không hợp lệ, chưa cung cấp đủ thông tin báo cáo";
                                }
                                #endregion
                            }
                            else
                            {
                                retValue.Code = CustomCode.DuLieuKhongHopLe;
                                retValue.Message = "Ngày tháng dữ liệu không hợp lệ, vui lòng xem lại";
                            }
                        }
                        catch (Exception ex)
                        {
                            Helper.GhiLogs("LogNet", ex);
                            retValue.Code = CustomCode.KhongXacDinh;
                            retValue.Message = "Có lỗi xảy ra, vui lòng thử lại sau";
                        }
                    }
                    else
                    {
                        retValue.Code = CustomCode.DuLieuKhongHopLe;
                        retValue.Message = "Dữ liệu không hợp lệ, do chưa cung cấp điểm cuối báo cáo";
                    }
                }
                else
                {
                    retValue.Code = CustomCode.DuLieuKhongHopLe;
                    retValue.Message = "Dữ liệu không hợp lệ, do chưa cung cấp điểm đầu báo cáo";
                }
            }
            else
            {
                retValue.Code = CustomCode.DuLieuKhongHopLe;
                retValue.Message = "Dữ liệu không hợp lệ, do chưa cung cấp thông tin đối tác";
            }
            return retValue;
        }
        private CustomMessage BaoCaoDichVuGTGTChoTapDoan()
        {
            CustomMessage retValue = new CustomMessage();

            string linhVucConIds = Request.QueryString["Ids"];
            string sCode = Request.QueryString["SCode"]; // Mã bảo mật phiên làm việc

            if (!string.IsNullOrEmpty(linhVucConIds)) // Có giá trị
            {
                string[] ids = linhVucConIds.Split(new string[] { ", ", ",", ":", ";" }, StringSplitOptions.RemoveEmptyEntries);
                List<int> dsLinhVucCon = new List<int>();
                foreach (string id in ids)
                {
                    int linhVucConId = 0;
                    if (int.TryParse(id, out linhVucConId))
                    {
                        dsLinhVucCon.Add(linhVucConId); // Đưa vào danh sách
                    }
                }
                if (dsLinhVucCon.Count > 0)  // Có lĩnh vực con cần thể hiện trên báo cáo
                {
                    // Kiểm tra ngay tháng
                    int month = 0;
                    int year = 0;
                    int.TryParse(Request.QueryString["Thang"], out month);
                    int.TryParse(Request.QueryString["Nam"], out year);
                    if (month > 0 && month <= 12 && year >= 2000 && year <= 2050) // Dữ liệu ngày tháng hợp lệ
                    {
                        string keyCache = string.Format("BaoCaoDichVuGTGTChoTapDoan{0}{1}{2}", year, month, string.Join("-", dsLinhVucCon));

                        // Cache data trong vòng 15p
                        SolrQueryResults<KhieuNai_ReportInfo> listKhieuNaiInfo = Cache.Data<SolrQueryResults<KhieuNai_ReportInfo>>(keyCache, (15 * 60), () =>
                               {

                                   string dsLoiKn = string.Empty;
                                   // Chỉ tháng 5, 6 năm 2016
                                   if ((year == 2016) && (month == 5 || month == 6))
                                   {
                                       string dsLoiKNCu = "8, 9 ,10 ,11 ,12 ,13 ,14 ,16 ,18 ,19 ,20 ,21 ,22 ,23 ,24 ,25 ,26 ,27 ,28 ,29 ,30 ,31 ,32 ,34 ,35 ,48 ,49 ,50 ,51 ,52 ,53 ,55 ,57 ,59 ,60 ,61 ,62 ,63 ,64 ,65 ,66 ,68 ,70 ,73 ,76 ,78 ,80 ,81 ,83 ,84 ,85 ,86 ,87 ,89 ,90 ,91 ,92 ,94 ,96 ,97 ,99 ,100 ,102 ,106 ,107 ,108 ,109 ,110 ,114 ,117 ,118 ,119 ,127 ,128 ,130 ,136 ,137 ,144 ,145 ,150 ,152 ,155 ,158 ,160 ,161 ,163 ,164 ,165 ,166 ,170 ,172 ,175 ,176 ,178";
                                       string[] dsLoiKNCuTemp = dsLoiKNCu.Split(',');
                                       List<string> newDS = new List<string>();
                                       foreach (string tmp in dsLoiKNCuTemp) newDS.Add(tmp.Trim());
                                       dsLoiKn = string.Format("AND ChiTietLoiId: ({0})", string.Join(" ", newDS));
                                   }
                                   else // Những tháng còn lại lấy trong Database
                                   {
                                       //List<LoiKhieuNaiInfo> lstLoiKN = new LoiKhieuNaiImpl().GetListDynamic(string.Empty, "Loai = 2 AND HoatDong = 1 AND Cap = 2", string.Empty);
                                       //if (lstLoiKN != null && lstLoiKN.Count > 0)
                                       //{
                                       //    foreach (LoiKhieuNaiInfo item in lstLoiKN)
                                       //    {
                                       //        dsLoiKn += item.Id + " ";
                                       //    }
                                       //    dsLoiKn = string.Format("AND ChiTietLoiId: ({0})", dsLoiKn);
                                       //}


                                       if (year <= 2016 && month <= 9) // <= 2016-09-31
                                       {
                                           List<LoiKhieuNaiInfo> lstLoiKN = new LoiKhieuNaiImpl().GetListDynamic(string.Empty, "Loai = 2 AND HoatDong = 1 AND Cap = 2", string.Empty);
                                           if (lstLoiKN != null && lstLoiKN.Count > 0)
                                           {
                                               foreach (LoiKhieuNaiInfo item in lstLoiKN)
                                               {
                                                   dsLoiKn += item.Id + " ";
                                               }
                                               dsLoiKn = string.Format("AND ChiTietLoiId: ({0})", dsLoiKn);
                                           }
                                       }
                                       else // Từ tháng 10 năm 2016
                                       {
                                           DateTime dtBaoCao = new DateTime(year, month, 1).EndOfMonth();


                                           // Điều kiện chọn ra Khiếu lại có nguyên nhân là lỗi
                                           string whereClause = string.Format("Loai >= 2 AND HoatDong = 1 AND Cap = 2 AND TuNgay <= {0} AND DenNgay >= {1}", dtBaoCao.StartOfMonth().ToString("yyyyMMdd"), dtBaoCao.EndOfMonth().ToString("yyyyMMdd"));

                                           Helper.GhiLogs(whereClause);

                                           List<LoiKhieuNaiInfo> lstLoiKN = new LoiKhieuNaiImpl().GetListDynamic(string.Empty, whereClause, string.Empty);
                                           if (lstLoiKN != null && lstLoiKN.Count > 0)
                                           {
                                               foreach (LoiKhieuNaiInfo item in lstLoiKN)
                                               {
                                                   dsLoiKn += item.Id + " ";
                                               }
                                               dsLoiKn = string.Format("AND ChiTietLoiId: ({0})", dsLoiKn);
                                           }
                                       }


                                   }

                                   DateTime fromDate = new DateTime(year, month, 1).StartOfMonth();
                                   DateTime toDate = new DateTime(year, month, 1).EndOfMonth();

                                   string whereSolr = string.Empty;
                                   whereSolr += string.Format("NgayDongKNSort:[{0} TO {1}]", fromDate.ToString("yyyyMMdd"), toDate.ToString("yyyyMMdd"));
                                   whereSolr += string.Format(" {0}", dsLoiKn);
                                   whereSolr += string.Format(" AND LinhVucConId: ({0})", string.Join(" ", dsLinhVucCon));

                                   SolrQuery solrQuery = new SolrQuery(whereSolr);
                                   QueryOptions qoDaXuLy = new QueryOptions();
                                   Dictionary<string, string> extraParamDaXuLy = new Dictionary<string, string>();
                                   extraParamDaXuLy.Add("fl", @"Id, SoThueBao, LinhVucCon, NgayTiepNhan, NoiDungPA, NoiDungXuLy, NgayDongKN, NoiDungXuLyDongKN, ChiTietLoiId, NguoiXuLy, DoiTacXuLyId, NoiDungXuLyDongKN");
                                   qoDaXuLy.ExtraParams = extraParamDaXuLy;
                                   qoDaXuLy.Start = 0;
                                   qoDaXuLy.Rows = int.MaxValue;

                                   FacetParameters facetParam = new FacetParameters();
                                   SolrFacetFieldQuery sffq = new SolrFacetFieldQuery("LinhVucConId");
                                   facetParam.Queries.Add(sffq);
                                   qoDaXuLy.Facet = facetParam;
                                   return QuerySolrBase<KhieuNai_ReportInfo>.QuerySolr(string.Concat(Config.ServerSolr, "GQKN"), solrQuery, qoDaXuLy);
                               });


                        retValue.Code = CustomCode.OK;
                        retValue.Message = "Thành công";
                        retValue.Number = listKhieuNaiInfo.Count;

                        retValue.Data = listKhieuNaiInfo;
                        retValue.DataEx = listKhieuNaiInfo.Select((v, Index) => new
                        {
                            STT = Index + 1,
                            Id = v.Id,
                            ChiTietLoiId = v.ChiTietLoiId,
                            ChiTietLoi = GetChiTietLoi(v.ChiTietLoiId),
                            SoThueBao = v.SoThueBao,
                            LinhVucCon = v.LinhVucCon,
                            NoiDungPA = LimitExcelContent(v.NoiDungPA),
                            NoiDungXuLy = LimitExcelContent(v.NoiDungXuLyDongKN),
                            NgayTiepNhan = v.NgayTiepNhan,
                            NgayDongKN = v.NgayDongKN,
                            NoiDungXuLyDongKN = v.NoiDungXuLyDongKN,
                            NguoiXuLy = v.NguoiXuLy,
                            DoiTacXuLyId = GetDoiTacXuLyId(v.DoiTacXuLyId)
                        });
                    }
                    else
                    {
                        retValue.Code = CustomCode.DuLieuKhongHopLe;
                        retValue.Message = "Dữ liệu tháng hoặc năm không hợp lệ";
                    }
                }
                else
                {
                    retValue.Code = CustomCode.DuLieuKhongHopLe;
                    retValue.Message = "Giá trị lĩnh vực con không hợp lệ";
                }
            }
            else
            {
                retValue.Code = CustomCode.DuLieuKhongHopLe;
                retValue.Message = "Giá trị lĩnh vực con không hợp lệ";
            }
            return retValue;
        }
        private void BtnExportExel_Click(object sender, EventArgs e)
        {
            // Báo cáo BaoCaoDichVuGTGTChoTapDoan
            if (ReportType.ToLower() == "BaoCaoDichVuGTGTChoTapDoan".ToLower())
            {
                CustomMessage ret = BaoCaoDichVuGTGTChoTapDoan();
                if (ret.Code == CustomCode.OK) // Thành công
                {
                    SolrQueryResults<KhieuNai_ReportInfo> data = ret.Data as SolrQueryResults<KhieuNai_ReportInfo>;

                    // Xuất Excel
                    string fileNameTemp = "DanhSachKhieuNai_BaoCaoDichVuGTGTChoTapDoan.xlsx";
                    string pathFile = Server.MapPath("~/ExportExcel/Template/" + fileNameTemp);

                    WorkbookDesigner designer = new WorkbookDesigner();
                    LoadOptions loadOptions = new LoadOptions(LoadFormat.Xlsx);
                    designer.Workbook = new Workbook(pathFile, loadOptions);

                    // DateTime startDate = DateTime.Parse(txtFromDate.Text, new CultureInfo("vi-VN")).StartOfDay();
                    // DateTime endDate = DateTime.Parse(txtToDate.Text, new CultureInfo("vi-VN")).EndOfDay();
                    string tieuDe = string.Format("Danh sách chi tiết khiếu nại");
                    string thoiGianBaoCao = string.Format("Thời gian báo cáo tháng {0} năm {1}", Request.QueryString["Thang"], Request.QueryString["Nam"]);

                    designer.SetDataSource("TieuDe", tieuDe);
                    designer.SetDataSource("ThoiGianBaoCao", thoiGianBaoCao);

                    // Chỉnh sửa dữ liệu
                    var tempData = data.Select((v, Index) => new
                    {
                        STT = Index + 1,
                        Id = v.Id,
                        ChiTietLoiId = v.ChiTietLoiId,
                        ChiTietLoi = GetChiTietLoi(v.ChiTietLoiId),
                        SoThueBao = v.SoThueBao,
                        LinhVucCon = v.LinhVucCon,
                        NoiDungPA = LimitExcelContent(v.NoiDungPA),
                        NoiDungXuLy = LimitExcelContent(v.NoiDungXuLyDongKN),
                        NgayTiepNhan = v.NgayTiepNhan,
                        NgayDongKN = v.NgayDongKN,
                        NoiDungXuLyDongKN = v.NoiDungXuLyDongKN,
                        NguoiXuLy = v.NguoiXuLy,
                        DoiTacXuLyId = GetDoiTacXuLyId(v.DoiTacXuLyId)
                    });

                    designer.SetDataSource("TongSoKhieuNai", string.Format("Tìm thấy tổng số {0} khiếu nại", tempData.Count()));

                    designer.SetDataSource("DanhSachChiTiet", tempData.ToList());
                    designer.Process();

                    string exportPath = HttpContext.Current.Server.MapPath("~/ExportExcel/Temp/Data/");
                    if (!Directory.Exists(exportPath)) Directory.CreateDirectory(exportPath);

                    string newFileName = string.Format("DanhSachKhieuNai_BaoCaoDichVuGTGTChoTapDoan_{0:yyMMddHHmmss}.xlsx", DateTime.Now);
                    string fullFileName = Path.Combine(exportPath, newFileName);

                    designer.Workbook.Save(fullFileName, SaveFormat.Xlsx);

                    //Set the appropriate ContentType.
                    Response.ContentType = "application/vnd.ms-excel";

                    string header = string.Format("attachment; filename={0}", newFileName);
                    Response.AddHeader("Content-Disposition", header);
                    //Get the physical path to the file.
                    //Write the file directly to the HTTP content output stream.
                    Response.WriteFile(fullFileName);
                    Response.Flush();

                }
            }
        }
        private string LimitExcelContent(string content)
        {
            if (content != null && (content.Length > Constant.LIMIT_CHARACTER_EXCEL_CELL))
            {
                return content.Substring(0, Constant.LIMIT_CHARACTER_EXCEL_CELL - 5) + " ...";
            }
            else
            {
                return content;
            }
        }
        private string GetChiTietLoi(int chiTietLoiId)
        {
            string cacheKeyChiTietLoi = "ChiTietLoi_" + chiTietLoiId;

            LoiKhieuNaiInfo info = Cache.Data<LoiKhieuNaiInfo>(cacheKeyChiTietLoi, 9000, () =>
            {
                return new LoiKhieuNaiImpl().GetInfo(chiTietLoiId);
            });
            return info != null ? info.TenLoi : "Không xác định";
        }

        private string GetDoiTacXuLyId(int doiTacXuLyId)
        {
            string cacheKeyChiTietLoi = "DoiTacXuLyId_" + doiTacXuLyId;

            DoiTacInfo info = Cache.Data<DoiTacInfo>(cacheKeyChiTietLoi, 9000, () =>
            {
                return new DoiTacImpl().GetInfo(doiTacXuLyId);
            });
            return info != null ? info.TenDoiTac : "Không xác định";
        }

        private void RenderGridViewTemplate(GridView grvView, List<CustomTemplate> templates)
        {
            if (templates != null && templates.Count > 0)
            {
                grvView.Columns.Clear();

                foreach (CustomTemplate column in templates.OrderBy(v => v.STT))
                {
                    BoundField temp = new BoundField();
                    temp.HeaderText = column.Name;
                    temp.DataField = column.DataField;

                    grvView.Columns.Add(temp);
                }
            }
        }
    }
}