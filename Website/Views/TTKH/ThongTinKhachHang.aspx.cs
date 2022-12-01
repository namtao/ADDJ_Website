using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Website.AppCode.Controller;
using AIVietNam.Core;
using AIVietNam.Admin;
using AIVietNam.GQKN.Entity;
using AIVietNam.GQKN.Impl;
using System.Data;
using Website.AppCode;
using System.Text.RegularExpressions;

namespace Website.View
{
    public partial class ThongTinKhachHang : AppCode.PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!BuildPhongBan_Permission.CheckPermission(PermissionSchemes.Thêm_khiếu_nại))
            {
                spThemMoiKN.Visible = false;
            }

            CallJavaScriptInUpdatePanel();

            if (!IsPostBack)
            {
                ViewState["STB"] = "";
                BindGridEmpty();
            }
        }

        private void BindGridEmpty()
        {
            //grvViewLichSuKhieuNai.DataSource = new List<ServiceFromSubinfo>();
            //grvViewLichSuKhieuNai.DataBind();
        }

        private void CallJavaScriptInUpdatePanel()
        {
            txtThueBao.Attributes.Add("onkeypress", "return handleEnter('" + btTraCuu.ClientID + "', event)");
        }

        protected void TabContainer1_ActiveTabChanged(object sender, EventArgs e) { }

        #region Lich Su Khieu Nai

        private void Bind_LichSuKhieuNai(int _pageIndex)
        {
            //            if (ViewState["STB"] == null || ViewState["STB"].ToString().Equals(""))
            //                return;
            //            pageSize = Convert.ToInt32(DropDownListPageSize.SelectedValue);

            //            if (_pageIndex != 0)
            //                pageIndex = _pageIndex;

            //            int totalRecord = 0;

            //            var lst = ServiceFactory.GetInstanceKhieuNai().GetPaged("", "SoThueBao=" + ViewState["STB"].ToString().Substring(2), "LDate DESC", pageIndex, pageSize, ref totalRecord);

            //            grvViewLichSuKhieuNai.DataSource = lst;
            //            grvViewLichSuKhieuNai.DataBind();

            //            TextBoxPage.Text = pageIndex.ToString();
            //            var totalPage = totalRecord / pageSize;
            //            if (totalRecord % pageSize != 0)
            //                totalPage++;
            //            LabelNumberOfPages.Text = totalPage.ToString();

            //            ltTongSoBanGhi.Text = totalRecord.ToString();
            //<<<<<<< .mine

            //            ltDanhSachKhieuNai.Text = @"<table class='tbl_style' cellpadding='0' cellspacing='0' style='border-collapse: collapse;'>
            //                                            <tbody>
            //                                                <tr class='th' align='center' style='color: White; background-color: #2360A4; font-weight: bold;'>
            //                                                    <th align='center' scope='col' style='width: 110px;'>Mã PA/KN
            //                                                    </th>
            //                                                    <th align='center' scope='col' style='width: 70px;'>Trạng thái
            //                                                    </th>
            //                                                    <th align='center' scope='col'>Người tiếp nhận
            //                                                    </th>
            //                                                    <th align='center' scope='col'>Ngày tiếp nhận
            //                                                    </th>
            //                                                    <th align='center' scope='col'>Phòng ban xử lý
            //                                                    </th>
            //                                                    <th align='center' scope='col'>Người xử lý
            //                                                    </th>
            //                                                    <th align='center' scope='col'>Ngày đóng KN
            //                                                    </th>
            //                                                    <th align='center' scope='col'>Nội dung giải quyết
            //                                                    </th>
            //                                                    <th align='center' scope='col'>Nội dung phản ánh
            //                                                    </th>
            //                                                    <th align='center' scope='col'>Loại khiếu nại
            //                                                    </th>
            //                                                    <th align='center' scope='col'>Lĩnh vực chung
            //                                                    </th>
            //                                                    <th align='center' scope='col'>Lĩnh vực con
            //                                                    </th>
            //                                                </tr>                                                                
            //                                        </tbody></table>";
            //=======
            //>>>>>>> .r974
        }

        #endregion

        #region Lich Su Cuoc Goi

        private void Bind_LichSuCuocGoi(int _pageIndex)
        {

            grvViewLichSuCuocGoi.DataSource = null;
            grvViewLichSuCuocGoi.DataBind();
        }

        protected void grvViewLichSuCuocGoi_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                RowDataBound_LichSuCuocGoi(e);
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                Response.Redirect(Config.PathError, false);
            }
        }

        private void RowDataBound_LichSuCuocGoi(GridViewRowEventArgs e)
        {
            if (e.Row.DataItemIndex != -1)
            {
                e.Row.Cells[0].Text = (e.Row.DataItemIndex + 1).ToString();
            }
        }

        #endregion


        #region Lich Su nạp thẻ

        private void Bind_LichSuNapThe()
        {
            //var Impl = new SubinfoImpl();

            //var admin = LoginAdmin.AdminLogin();

            //if (!string.IsNullOrEmpty(ViewState["STB"].ToString()))
            //{
            //    var infoTB = Impl.getPPSCardHistoryByMsisdn("0", ViewState["STB"].ToString(), admin.Username, Utility.GetIP(), "");
            //    if (infoTB != null && infoTB.ErrorID == "0")
            //    {
            //        grvViewLichSuNapThe.DataSource = infoTB.ListPPSCardHistoryByMsisdn;
            //        grvViewLichSuNapThe.DataBind();
            //    }
            //    else
            //    {
            //        grvViewLichSuNapThe.DataSource = new List<PPSCardHistoryByMsisdnDetailFromSubinfo>();
            //        grvViewLichSuNapThe.DataBind();
            //    }
            //}
            //else
            //{
            //    grvViewLichSuNapThe.DataSource = new List<PPSCardHistoryByMsisdnDetailFromSubinfo>();
            //    grvViewLichSuNapThe.DataBind();
            //}
        }

        //protected void grvViewLichSuNapThe_RowDataBound(object sender, GridViewRowEventArgs e)
        //{
        //    try
        //    {
        //        RowDataBound_LichSuNapThe(e);
        //    }
        //    catch (Exception ex)
        //    {
        //        Utility.LogEvent(ex);
        //        Response.Redirect(Config.PathError, false);
        //        return;
        //    }
        //}

        //private void RowDataBound_LichSuNapThe(GridViewRowEventArgs e)
        //{
        //    if (e.Row.DataItemIndex != -1)
        //    {
        //        e.Row.Cells[0].Text = (e.Row.DataItemIndex + 1).ToString();
        //    }
        //}

        #endregion

        #region Kiểm tra seri thẻ nạp
        protected void btKiemTraTheNap_Click(object sender, EventArgs e)
        { }
        #endregion

        #region Lich su 3G
        private void Bind_LichSu3G()
        {
            //if (!string.IsNullOrEmpty(ViewState["STB"].ToString()))
            //{
            //    var Impl = new SubinfoImpl();
            //    var admin = LoginAdmin.AdminLogin();
            //    var lst = Impl.get3GHistory(ViewState["STB"].ToString(), admin.Username, Utility.GetIP(), "", false);
            //    if (lst != null && lst.ErrorID == "0")
            //    {
            //        grvLichSu3G.DataSource = lst.ListHistory3G;
            //        grvLichSu3G.DataBind();
            //    }
            //    else
            //    {
            //        //grvViewLichSuNapThe.DataSource = new List<History3GDetailFromSubinfo>();
            //        //grvViewLichSuNapThe.DataBind();
            //    }
            //}
            //else
            //{
            //    //grvViewLichSuNapThe.DataSource = new List<History3GDetailFromSubinfo>();
            //    //grvViewLichSuNapThe.DataBind();
            //}
        }

        //protected void grvLichSu3G_RowDataBound(object sender, GridViewRowEventArgs e)
        //{
        //    if (e.Row.DataItemIndex != -1)
        //    {
        //        e.Row.Cells[0].Text = (e.Row.DataItemIndex + 1).ToString();
        //    }
        //}
        #endregion

        #region Tra cứu thẻ cào
        private void Bind_TraCuuTheCao()
        {
            //try
            //{
            //    if (!string.IsNullOrEmpty(ViewState["STB"].ToString()))
            //    {
            //        var Impl = new SubinfoImpl();
            //        var admin = LoginAdmin.AdminLogin();
            //        var lst = Impl.getPPSCardInfo(new Random().Next().ToString(), ViewState["STB"].ToString(), admin.Username, Utility.GetIP(), "");
            //        Utility.LogEvent(lst);
            //        if (lst != null && lst.ErrorID == "0")
            //        {
            //            grvTraCuuTheCao.DataSource = lst.ListPPSCardInfo;
            //            grvTraCuuTheCao.DataBind();
            //        }
            //        else
            //        {
            //            grvTraCuuTheCao.DataSource = new List<PPSCardInfoDetailFromSubinfo>();
            //            grvTraCuuTheCao.DataBind();
            //        }
            //    }
            //    else
            //    {
            //        grvTraCuuTheCao.DataSource = new List<PPSCardInfoDetailFromSubinfo>();
            //        grvTraCuuTheCao.DataBind();
            //    }
            //}
            //catch (Exception ex)
            //{
            //    Utility.LogEvent(ex);
            //}
        }

        //protected void grvTraCuuTheCao_RowDataBound(object sender, GridViewRowEventArgs e)
        //{
        //    if (e.Row.DataItemIndex != -1)
        //    {
        //        e.Row.Cells[0].Text = (e.Row.DataItemIndex + 1).ToString();
        //    }
        //}

        #endregion

        #region Lich Su Thue Bao

        private void Bind_LichSuThueBao()
        {
            //var Impl = new SubinfoImpl();

            //var admin = LoginAdmin.AdminLogin();

            //if (!string.IsNullOrEmpty(ViewState["STB"].ToString()))
            //{
            //    var infoTB = Impl.getSubHistory(ViewState["STB"].ToString(), admin.Username, Utility.GetIP(), "Từ thông tin khách hàng", false);
            //    if (infoTB != null && infoTB.ErrorID == "0")
            //    {
            //        grvViewLichSuThueBao.DataSource = infoTB.ListHistory;
            //        grvViewLichSuThueBao.DataBind();
            //    }
            //    else
            //    {
            //        grvViewLichSuThueBao.DataSource = new List<PPSCardHistoryByMsisdnDetailFromSubinfo>();
            //        grvViewLichSuThueBao.DataBind();
            //    }
            //}
            //else
            //{
            //    grvViewLichSuThueBao.DataSource = null;
            //    grvViewLichSuThueBao.DataBind();
            //}
        }

        //protected void grvViewLichSuThueBao_RowDataBound(object sender, GridViewRowEventArgs e)
        //{
        //    try
        //    {
        //        RowDataBound_LichSuThueBao(e);
        //    }
        //    catch (Exception ex)
        //    {
        //        Utility.LogEvent(ex);
        //        Response.Redirect(Config.PathError, false);
        //        return;
        //    }
        //}

        //private void RowDataBound_LichSuThueBao(GridViewRowEventArgs e)
        //{
        //    if (e.Row.DataItemIndex != -1)
        //    {
        //        e.Row.Cells[0].Text = (e.Row.DataItemIndex + 1).ToString();
        //    }
        //}

        #endregion



        #region Tra cứu SMS888
        private void Bind_SMS888()
        {
            //try
            //{
            //    if (!string.IsNullOrEmpty(ViewState["STB"].ToString()))
            //    {
            //        var Impl = new SubinfoImpl();
            //        var admin = LoginAdmin.AdminLogin();
            //        var lst = Impl.getDeliverSMHistory(new Random().Next().ToString(), ViewState["STB"].ToString(), admin.Username, Utility.GetIP(), "");
            //        Utility.LogEvent(lst);
            //        if (lst != null && lst.ErrorID == "0")
            //        {
            //            grvTraCuuSMS888.DataSource = lst.ListDeliverSMHistory;
            //            grvTraCuuSMS888.DataBind();
            //        }
            //        else
            //        {
            //            grvTraCuuSMS888.DataSource = new List<DeliverSMHistoryDetailFromSubinfo>();
            //            grvTraCuuSMS888.DataBind();
            //        }
            //    }
            //    else
            //    {
            //        grvTraCuuSMS888.DataSource = new List<DeliverSMHistoryDetailFromSubinfo>();
            //        grvTraCuuSMS888.DataBind();
            //    }
            //}
            //catch(Exception ex)
            //{
            //    Utility.LogEvent(ex);
            //}
        }

        //protected void grvTraCuuSMS888_RowDataBound(object sender, GridViewRowEventArgs e)
        //{
        //    if (e.Row.DataItemIndex != -1)
        //    {
        //        e.Row.Cells[0].Text = (e.Row.DataItemIndex + 1).ToString();
        //    }
        //}

        #endregion


    }
}