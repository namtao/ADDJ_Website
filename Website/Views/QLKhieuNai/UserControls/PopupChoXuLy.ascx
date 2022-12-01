<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PopupChoXuLy.ascx.cs" Inherits="Website.Views.QLKhieuNai.UserControls.PopupChoXuLy" %>

<%-- Chuyển xử lý khiếu nại ParentPage: QuanLyKhieuNai.aspx --%>
<div id="divPoupTiepNhanKN" class="popup_process pop_tiepnhan" style="display: none;">
    <div class="pop_title">
        <h3 id="H7" style="float: left; color: #fff; font-weight: bold;">Tiếp nhận khiếu nại
        </h3>
        <span style="float: right;"><a href="javascript:closePopup('divPoupTiepNhanKN');">
            <img src="/Images/x.png" />
        </a></span>
    </div>
    <div id="div5">
        <div class="nav_btn" style='background: none;'>
            <div class="popup_Body">
                <div class="infoBox">
                    Bạn có muốn tiếp nhận khiếu nại được chọn ?
                </div>
                <div class="vide"></div>
                <div class="pop_pro_up">
                    <ul>
                        <li style="float: right;"><a href="javascript:ClosePoup();"><span class="notapply">Hủy</span></a></li>
                        <li style="float: right;"><a href="javascript:fnTiepNhanKN();"><span class="apply">Đồng ý</span></a></li>
                    </ul>
                </div>
            </div>
        </div>
    </div>
    <div class="vide"></div>
</div>

<div id="divPoupChuyenXuLyAuTo" class="popup_process pop_chuyenxulyAuto" style="display: none;">
    <div class="pop_title">
        <h3 id="H5" style="float: left; color: #fff; font-weight: bold;">Chuyển xử lý khiếu nại</h3>
        <span style="float: right;"><a href="javascript:ClosePoupChuyenXuLyAuTo();" title="Đóng cửa sổ">
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
                </div>--%>
                 Nội dung xử lý <span class="color-red">(*)</span>
                <div class="inputstyle" style="padding: 10px 0px;">
                    <div class="bg">
                        <textarea rows="5" style="height: 40px; width: 100%; padding: 3px;" id="txtNoiDungXuLyChuyenXuLyAuTo"></textarea>
                    </div>
                </div>
                <div class="vide"></div>
                <div class="pop_pro_up">
                    <ul style="background-color: #d1e8fa">
                        <li style="float: right;"><a href="javascript:ClosePoupChuyenXuLyAuTo();" title="Hủy bỏ xử lý"><span class="notapply">Hủy</span></a></li>
                        <li style="float: right;"><a href="javascript:ChuyenXuLyAuto();"><span class="apply">Đồng ý</span></a></li>
                    </ul>
                </div>
            </div>
        </div>
    </div>
    <div class="vide"></div>
</div>

<div id="divPoupChuyenPhanHoi" class="pop_chuyen_phan_hoi" style="display: none;">
    <div style="border-bottom: 1px solid #2A4A50; background: #4D709A; padding: 10px; height: 25px;">
        <h3 id="H2" style="float: left; color: #fff; font-weight: bold;">Chuyển phản hồi            
        </h3>
        <span style="float: right;"><a href="javascript:ClosePoup();">
            <img src="/Images/x.png" />
        </a></span>
    </div>
    <div id="div3">
        <div class="nav_btn" style='background: none;'>
            <div class="infoBox">
                Khiếu nại sẽ được chuyển đến người chuyển khiếu nại cho bạn
            </div>
            <div style="margin-top: 10px; height: 105px; background: none;">
                <span style="font-size: 13px; font-weight: bold;">Nội dung xử lý <strong style="color: Red">(*)</strong></span>
                <div class="inputstyle">
                    <div class="bg">
                        <textarea rows="5" style="height: 60px; width: 100%; padding: 3px" maxlength="500" id="txtNoteChuyenPhanHoi"></textarea>
                    </div>
                </div>
            </div>
            <div class="vide"></div>
            <div class="pop_pro_up">
                <ul>
                    <li style="float: right;"><a href="javascript:ClosePoup();"><span class="notapply">Hủy
                    </span></a></li>
                    <li style="float: right;"><a href="javascript:ChuyenPhanHoi();"><span class="apply">Đồng ý </span></a></li>
                </ul>
            </div>
        </div>
    </div>
    <div style="clear: both; height: 1px;">
    </div>
</div>

<div id="divPoupPhongBan" style="width: auto; height: auto; background: #fff; margin: 0 auto; z-index: 1000; position: fixed; top: 10%; left: 30%; right: 30%; border: 1px solid #4D709A; border-radius: 5px 5px 5px 5px; box-shadow: 0 1px 3px #999999 inset; display: none;">
    <div style="border-bottom: 1px solid #2A4A50; background: #4D709A; padding: 10px; height: 25px;">
        <h3 id="H1" style="float: left; color: #fff; font-weight: bold;">Chọn phòng ban cần chuyển đến
        </h3>
        <span style="float: right;"><a href="javascript:ClosePoup();">
            <img src="/Images/x.png" />
        </a></span>
    </div>
    <div id="div2">
        <div class="nav_btn" style='background: none;'>
            <div style="margin-top: 10px; margin-left: 10px; height: 185px; border-top: 1px solid #CCC; border-bottom: 1px solid #CCC; background: none; overflow-y: scroll;">
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
            <div style="margin-top: 10px; height: 105px; background: none;">
                <span style="font-size: 13px; font-weight: bold;">Nội dung xử lý <strong style="color: Red">(*)</strong></span>
                <div class="inputstyle">
                    <div class="bg">
                        <textarea rows="5" style="height: 60px; width: 99%; padding: 5px;" maxlength="500" id="txtNoteChuyenXuLy"></textarea>
                    </div>
                </div>
            </div>
            <div class="vide"></div>
            <div class="pop_pro_up">
                <ul>
                    <li style="float: right;"><a href="javascript:ClosePoup();"><span class="notapply">Hủy</span></a></li>
                    <li style="float: right;"><a href="javascript:ChuyenXuLy();"><span class="apply">Đồng ý</span></a></li>
                </ul>
            </div>
        </div>
    </div>
    <div style="clear: both; height: 1px;">
    </div>
</div>

<div id="divPoupChuyenNgangHang" class="pop_chuyen_gang_hang" style="width: auto; height: auto; background: #fff; margin: 0 auto; z-index: 1000; position: fixed; top: 20%; left: 30%; right: 30%; border: 1px solid #4D709A; border-radius: 5px 5px 5px 5px; box-shadow: 0 1px 3px #999999 inset; display: none;">
    <div style="border-bottom: 1px solid #2A4A50; background: #4D709A; padding: 10px; height: 25px;">
        <h3 id="H3" style="float: left; color: #fff; font-weight: bold;">Chuyển ngang hàng
        </h3>
        <span style="float: right;"><a href="javascript:ClosePoup();">
            <img src="/Images/x.png" />
        </a></span>
    </div>
    <div id="div1">
        <div class="nav_btn" style='background: none;'>
            <div class="infoBox">
                <%-- Khiếu nại sẽ được chuyển ngang hàng ra phòng ban tiếp nhận --%>
                Chuyển ngang hàng tới
            </div>
            <div class="selectstyle">
                <div class="bg">
                    <asp:DropDownList Width="100%" runat="server" ID="ddlUserNgangHang">
                    </asp:DropDownList>
                </div>
            </div>
            <div style="margin-top: 10px; height: 100px; background: none;">
                <span style="font-size: 13px; font-weight: bold;">Nội dung xử lý <strong style="color: Red">(*)</strong></span>
                <div class="inputstyle">
                    <div class="bg">
                        <textarea rows="5" style="height: 50px; width: 100%; padding: 3px;" id="txtNoteChuyenNgangHang"></textarea>

                    </div>
                </div>
            </div>
            <div class="vide"></div>
            <div class="pop_pro_up">
                <ul>
                    <li style="float: right;"><a href="javascript:ClosePoup();"><span class="notapply">Hủy
                    </span></a></li>
                    <li style="float: right;"><a href="javascript:ChuyenNgangHang();"><span class="apply">Đồng ý </span></a></li>
                </ul>
            </div>
        </div>
    </div>
    <div style="clear: both; height: 1px;">
    </div>
</div>

<div id="divPoupDongKhieuNai" style="width: auto; height: 350px; background: #fff; margin: 0 auto; z-index: 1000; position: fixed; top: 15%; left: 30%; right: 30%; border: 1px solid #4D709A; border-radius: 5px 5px 5px 5px; box-shadow: 0 1px 3px #999999 inset; display: none;">
    <div style="border-bottom: 1px solid #2A4A50; background: #4D709A; padding: 10px; height: 25px;">
        <h3 id="H4" style="float: left; color: #fff; font-weight: bold;">Đóng khiếu nại
        </h3>
        <span style="float: right;"><a href="javascript:ClosePoup();">
            <img src="/Images/x.png" />
        </a></span>
    </div>
    <div id="div4">
        <div class="nav_btn" style="background-color: white;">
            <div class="wp">
                <div style="font-size: 12px; font-weight: bold; line-height: 150%;">Bạn vui lòng chọn nguyên nhân lỗi, chi tiết lỗi, độ hài lòng của khách hàng và nhập nội dung xử lý trước khi đóng khiếu nại:</div>
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <table border="0" cellpadding="1" cellspacing="0" width="100%">
                            <tr>
                                <td style="width: 135px;">Nguyên nhân lỗi
                                </td>
                                <td>
                                    <div class="selectstyle">
                                        <div class="bg">
                                            <asp:DropDownList ID="ddlNguyenNhanLoi" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlNguyenNhanLoi_SelectedIndexChanged"></asp:DropDownList>
                                        </div>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td>Chi tiết lỗi
                                </td>
                                <td>
                                    <div class="selectstyle">
                                        <div class="bg">
                                            <asp:DropDownList ID="ddlChiTietLoi" runat="server"></asp:DropDownList>
                                        </div>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td>Độ hài lòng
                                </td>
                                <td>
                                    <div class="selectstyle">
                                        <div class="bg">
                                            <select id="slDoHaiLong" name="slDoHaiLong">
                                                <option value="-1" selected="selected">Chọn độ hài lòng của khách hàng</option>
                                                <option value="0">Rất hài lòng</option>
                                                <option value="1">Hài lòng</option>
                                                <option value="2">Không hài lòng</option>
                                                <option value="3">KH phản ứng gay gắt</option>
                                                <option value="4">Không liên lạc được KH</option>
                                                <option value="5">Ý kiến khác</option>
                                            </select>
                                        </div>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <span style="font-size: 13px;">Nội dung xử lý </span>
                                </td>
                                <td>
                                    <textarea rows="6" cols="50" style="width: 98%; height: 70px;" maxlength="500" id="txtNoteDongKhieuNai"></textarea>
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <div class="vide"></div>
            <div class="pop_pro_up" style="margin-top: 20px">
                <ul>
                    <li style="float: right;"><a href="javascript:ClosePoup();"><span class="notapply">Hủy</span></a></li>
                    <li style="float: right;"><a href="javascript:DongKhieuNai();"><span class="apply">Đồng ý</span></a></li>
                </ul>
            </div>
        </div>
    </div>
</div>


