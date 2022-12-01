<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="KNDaGuiDi.ascx.cs" Inherits="Website.Views.QLKhieuNai.UserControls.KNDaGuiDi" %>
<%@ Register Src="~/Views/QLKhieuNai/UserControls/UcTopMyKhieuNai.ascx" TagName="UcTopMyKhieuNai"
    TagPrefix="uc" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="UcFillterMyKhieuNai.ascx" TagName="UcFillterMyKhieuNai" TagPrefix="uc1" %>
<uc:UcTopMyKhieuNai ID="UcTopMyKhieuNai1" runat="server" />
<link href="/CSS/jquery.datepick.css" rel="stylesheet" type="text/css" />
<script src="/JS/plugin/jquery.datepick.js" type="text/javascript"></script>
<script src="/JS/plugin/jquery.datepick-vi.js" type="text/javascript"></script>
<script type="text/javascript">

    $(document).ready(function () {

        keyTotal = 1;
        keyGetHTML = 1;
        keyExcel = 1;

        pageSize = $('#DropPageSize').val();
        fnSetSizeDiv();

    });
</script>


<asp:UpdatePanel ID="UpdatePanel2" runat="server">
    <ContentTemplate>
        <div class="nav_btn" style='border-top: 0px'>
            <ul>
                <li style="float: left;"><a href="javascript:history.back()"><span class="back">Quay
                    về</span></a></li>
                <li id="btnChuyenXuLy" style="float: left;"><a href="javascript:ShowPoupPhongBan()"><span class="move_file">Chuyển xử lý</span></a></li>
                <li runat="server" id="liThemMoiKN"><a href="javascript:parent.fnAddNewKN();"><span
                    class="new">Thêm mới</span> </a></li>
                <li style="float: left;"><a href="../QLKhieuNai/SoTheoDoi.aspx"><span class="move_file">Vào sổ theo dõi</span></a></li>
            </ul>
            <div class="div-clear">
            </div>
        </div>
        <div class="p8">
            <table width="100%" cellspacing="0" cellpadding="0" border="0">
                <tbody>
                    <tr valign="top">
                        <td style="height: 5px"></td>
                    </tr>
                    <tr>
                        <td>


                            <uc1:UcFillterMyKhieuNai ID="UcFillterMyKhieuNai1" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td style="height: 10px"></td>
                    </tr>
                    <tr>
                        <td>
                            <div id="divNote" style="width: 100%; float: left; margin-top: 5px;">
                                <p style="border: 1pt solid #CCC; background: red; width: 22px; height: 13px; float: left;">
                                </p>
                                <span style="color: #4D709A; font-size: 12px; font-weight: bold; float: left; padding-left: 5px; padding-right: 5px;">Chờ xử lý</span>
                                <p style="border: 1pt solid #CCC; background: yellow; width: 22px; height: 13px; float: left;">
                                </p>
                                <span style="color: #4D709A; font-size: 12px; font-weight: bold; float: left; padding-left: 5px; padding-right: 5px;">Đang xử lý</span>
                                <p style="border: 1pt solid #CCC; background: #0095CC; width: 22px; height: 13px; float: left;">
                                </p>
                                <span style="color: #4D709A; font-size: 12px; font-weight: bold; float: left; padding-left: 5px; padding-right: 5px;">Chờ đóng</span>

                                <p style="border: 1pt solid #CCC; background: #FF8000; width: 22px; height: 13px; float: left;"></p>
                                <span style="color: #4D709A; font-size: 12px; font-weight: bold; float: left; padding-left: 5px; padding-right: 5px;">KN BP khác chuyển về</span>

                                <p style="border: 1pt solid #CCC; background: #999; width: 22px; height: 13px; float: left;"></p>
                                <span style="color: #4D709A; font-size: 12px; font-weight: bold; float: left; padding-left: 5px; padding-right: 5px;">KN quá hạn</span>

                                <p style="border: 1pt solid #CCC; background: green; width: 22px; height: 13px; float: left;">
                                </p>
                                <span style="color: #4D709A; font-size: 12px; font-weight: bold; float: left; padding-left: 5px; padding-right: 5px;">Đã đóng</span>
                                <br />
                            </div>
                            <div id="Pagination" class="pagination" style="float: right; margin-right: -5px;">
                            </div>
                            <div id="PageSize" class="pagination" style="float: right;">
                                <div class="selectstyle">
                                    <div class="bg" style="margin: -7px; margin-right: 10px; margin-left: 10px;">
                                        <select id="DropPageSize" onchange="javascript:fnDropPageSizeChange();" style="width: 60px;">
                                            <option value="10" selected="selected">10</option>
                                            <option value="20">20</option>
                                            <option value="50">50</option>
                                            <option value="100">100</option>
                                            <option value="500">500</option>
                                        </select>
                                    </div>
                                </div>
                            </div>
                            <div id="divTotalRecords" style="width: 150px; float: right; margin-top: 5px; text-align: right;">
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td>

                            <div class="div-clear" style="height: 10px;">
                            </div>
                        </td>
                    </tr>


                </tbody>
            </table>
            <table class="nobor">
                <tr>
                    <td>
                        <div id="divScroll" style="height: 370px; width: 100%;">
                            <table class="flex_KNChoXuLy" style="display: none"></table>
                        </div>
                    </td>
                </tr>
            </table>
        </div>

        <div id="divPoupPhongBan" style="width: auto; height: auto; background: #fff; margin: 0 auto; z-index: 200; position: fixed; top: 15%; left: 30%; right: 30%; border: 1px solid #4D709A; border-radius: 5px 5px 5px 5px; box-shadow: 0 1px 3px #999999 inset; display: none;">
            <div style="border-bottom: 1px solid #2A4A50; background: #4D709A; padding: 10px; height: 25px;">
                <h3 id="H1" style="float: left; color: #fff; font-weight: bold;">Chọn phòng ban cần chuyển đến
                </h3>
                <span style="float: right;"><a href="javascript:ClosePoup();">
                    <img src="/Images/x.png" />
                </a></span>
            </div>
            <div id="div2" style="">
                <div class="nav_btn" style='background: none;'>
                    <div style="margin-top: 10px; margin-left: 10px; height: 200px; border-top: 1px solid #CCC; border-bottom: 1px solid #CCC; background: none; overflow-y: scroll;">
                        <asp:Repeater ID="rptListData" runat="server">
                            <HeaderTemplate>
                                <table cellspacing="1" class="tbl_style">
                                    <thead>
                                        <tr>
                                            <th class="title" width="30">STT
                                            </th>
                                            <th class="title" align="left">Tên phòng ban
                                            </th>
                                            <th class="title" width="50">Chọn
                                            </th>
                                        </tr>
                                    </thead>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <tbody>
                                    <tr>
                                        <td style="padding-left: 6px;">
                                            <%# (Container.ItemIndex) + 1%>
                                        </td>
                                        <td align="left">
                                            <%# DataBinder.Eval(Container.DataItem, "Name")%>
                                        </td>
                                        <td align="center">
                                            <input type="radio" id='rad<%# DataBinder.Eval(Container.DataItem, "Id")%>' onclick="LoadUserInPhongBan(<%# DataBinder.Eval(Container.DataItem, "Id")%>)" name="SelectPhongBan"
                                                value="<%# DataBinder.Eval(Container.DataItem, "Id")%>">
                                        </td>
                                    </tr>
                                </tbody>
                            </ItemTemplate>
                            <FooterTemplate>
                                </table>
                            </FooterTemplate>
                        </asp:Repeater>
                    </div>
                    <div id="dvUserInPhongBan" runat="server">
                        <span style="font-size: 13px; font-weight: bold;">Chọn user</span>
                        <div class="selectstyle">
                            <div class="bg">
                                <select id="ddlUserInPhongBan">
                                </select>
                            </div>
                        </div>
                    </div>
                    <div style="margin-top: 10px; height: 150px; background: none;">
                        <span style="font-size: 13px; font-weight: bold;">Nội dung xử lý <strong style="color: Red">(*)</strong></span>
                        <div class="inputstyle">
                            <div class="bg">
                                <textarea rows="5" height="40px;" maxlength="500" id="txtNoteChuyenXuLy"></textarea>

                            </div>
                        </div>
                    </div>
                    <div style="clear: both; height: 1px; border-bottom: 1px solid #CCC; margin-bottom: 5px;">
                    </div>
                    <ul>
                        <li style="float: right;"><a href="javascript:ClosePoup();"><span class="notapply">Hủy
                        </span></a></li>
                        <li style="float: right;"><a href="javascript:ChuyenXuLy();"><span class="apply">Đồng
                    ý </span></a></li>
                    </ul>
                </div>
            </div>
            <div style="clear: both; height: 1px;">
            </div>
        </div>

        <div id="divPoupChuyenXuLyAuTo" style="width: auto; height: auto; background: #fff; margin: 0 auto; z-index: 1000; position: fixed; top: 15%; left: 30%; right: 30%; border: 1px solid #4D709A; border-radius: 5px 5px 5px 5px; box-shadow: 0 1px 3px #999999 inset; display: none;">
            <div style="border-bottom: 1px solid #2A4A50; background: #4D709A; padding: 10px; height: 25px;">
                <h3 id="H5" style="float: left; color: #fff; font-weight: bold;">Chuyển xử lý khiếu nại
                </h3>
                <span style="float: right;"><a href="javascript:ClosePoupChuyenXuLyAuTo();">
                    <img src="/Images/x.png" />
                </a></span>
            </div>
            <div id="div6">
                <div class="nav_btn" style='background: none;'>
                    <div class="selectstyle">
                        <div class="bg">
                            <select id="ddlTuDongDinhTuyenAndPhongBanCungDoiTac" onchange="javascript:fnLoadUserByPhongBanId('ddlTuDongDinhTuyenAndPhongBanCungDoiTac','ddlUserInPhongBan_divPoupChuyenXuLyAuTo')">
                            </select>
                        </div>
                    </div>
                    <div id="dvUserInPhongBan_divPoupChuyenXuLyAuTo" runat="server">
                        Chọn user
                        <div class="selectstyle">
                            <div class="bg">
                                <select id="ddlUserInPhongBan_divPoupChuyenXuLyAuTo">
                                </select>
                            </div>
                        </div>
                    </div>
                    <div class="popup_Body">
                        <%--<div class="infoBox">
                            Khiếu nại sẽ được tự động định tuyến đến phòng ban xử lý
                        </div>
                        <br />--%>
                        Nội dung xử lý
                        <div class="inputstyle">
                            <div class="bg">
                                <textarea rows="5" id="txtNoiDungXuLyChuyenXuLyAuTo" style="height: 40px; width: 99%;"></textarea>
                            </div>
                        </div>

                        <div style="clear: both; height: 1px; border-bottom: 1px solid #CCC; margin-bottom: 10px;">
                        </div>
                        <ul>
                            <li style="float: right;"><a href="javascript:ClosePoupChuyenXuLyAuTo();"><span class="notapply">Hủy</span></a></li>
                            <li style="float: right;"><a href="javascript:ChuyenXuLyAuto();"><span class="apply">Đồng ý</span></a></li>
                        </ul>
                    </div>
                </div>
            </div>
            <div style="clear: both; height: 1px;">
            </div>
        </div>
        <div id="divOpacity" class="divOpacity" style="opacity: .4; -moz-opacity: 0.4; filter: alpha(opacity=70); background: #000; width: 100%; position: fixed; left: 0; top: -80px; display: none; z-index: 100;">
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
