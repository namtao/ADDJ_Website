<%@ Page Language="C#" MasterPageFile="~/Master_Default.master" AutoEventWireup="true"
    Inherits="admin_nguoiSuDung_add" Title="Thêm mới người sử dung" CodeBehind="NguoiSuDung_Add.aspx.cs" %>

<asp:Content ContentPlaceHolderID="ContentPlaceHolder_Text" runat="Server">
    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <!-- begin panel nav boot -->
            <div class="nav_btn_bootstrap">
                <ul>
                    <li>
                        <a href="javascript:history.back()" class="btn btn-primary">
                            <span class="glyphicon glyphicon-circle-arrow-left" aria-hidden="true"></span> Quay về
                        </a>
                    </li>
                    <li>
                        <asp:LinkButton ID="linkbtnSubmit" class="btn btn-primary" runat="server" OnClick="linkbtnSubmit_Click">
                            <span class="glyphicon glyphicon-plus-sign" aria-hidden="true"></span> Cập nhật
                        </asp:LinkButton>
                    </li>
                </ul>
            </div>
             <!-- end panel nav boot -->
            <div class="p8">
                <!-- begin panel boot -->
                <div class="panel panel-default">
                    <div class="panel-heading"><span style="font-size: 12pt">Lọc tìm kiếm</span></div>
                    <div class="panel-body" style="border: none">
                        <!-- begin body boot -->

                        <table class="nobor nsd_add">
                            <tr>
                                <td class="colname">Tên truy cập
                                </td>
                                <td class="colval">
                                    <div class="inputstyle">
                                        <div class="bg">
                                            <asp:TextBox ID="txtTenTruyCap" runat="server" ReadOnly="true" Text="" MaxLength="50"></asp:TextBox>
                                        </div>
                                    </div>
                                </td>
                                <td class="colname">Tên đầy đủ
                                </td>
                                <td class="colval">
                                    <div class="inputstyle">
                                        <div class="bg">
                                            <asp:TextBox ID="txtTenDayDu" runat="server" Text="" ReadOnly="true" MaxLength="200"></asp:TextBox>
                                        </div>
                                    </div>
                                </td>
                                <td class="colval">&nbsp;</td>
                            </tr>
                            <tr>
                                <td class="colname">Ngày sinh
                                </td>
                                <td>
                                    <div class="inputstyle">
                                        <div class="bg">
                                            <asp:TextBox ID="txtNgaySinh" runat="server" Text="" ReadOnly="true"></asp:TextBox>
                                        </div>
                                    </div>
                                </td>
                                <td class="colname">Địa chỉ
                                </td>
                                <td>
                                    <div class="inputstyle">
                                        <div class="bg">
                                            <asp:TextBox ID="txtDiaChi" runat="server" Text="" ReadOnly="true" MaxLength="500"></asp:TextBox>
                                        </div>
                                    </div>
                                </td>
                                <td>&nbsp;</td>
                            </tr>
                            <tr>
                                <td class="colname">Số di động
                                </td>
                                <td>
                                    <div class="inputstyle">
                                        <div class="bg">
                                            <asp:TextBox ID="txtDiDong" runat="server" Text="" ReadOnly="true" MaxLength="20"></asp:TextBox>
                                        </div>
                                    </div>
                                </td>
                                <td class="colname">Giới tính
                                </td>
                                <td align="left">
                                    <asp:RadioButton ID="rdNam" runat="server" Enabled="false" Text="Nam" GroupName="rdGioiTinh" />
                                    &nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:RadioButton ID="rdNu" runat="server" Enabled="false" Text="Nữ" GroupName="rdGioiTinh" />
                                </td>
                                <td align="left">&nbsp;</td>
                            </tr>
                            <tr>
                                <td class="colname">Số cố định
                                </td>
                                <td>
                                    <div class="inputstyle">
                                        <div class="bg">
                                            <asp:TextBox ID="txtCoDinh" runat="server" ReadOnly="true" Text="" MaxLength="20"></asp:TextBox>
                                        </div>
                                    </div>
                                </td>
                                <td class="colname">Công ty
                                </td>
                                <td>
                                    <div class="inputstyle">
                                        <div class="bg">
                                            <asp:TextBox ID="txtCongTy" runat="server" ReadOnly="true" Text="" MaxLength="500"></asp:TextBox>
                                        </div>
                                    </div>
                                </td>
                                <td>&nbsp;</td>
                            </tr>
                            <tr>
                                <td class="colname">Email
                                </td>
                                <td>
                                    <div class="inputstyle">
                                        <div class="bg">
                                            <asp:TextBox ID="txtEmail" runat="server" ReadOnly="true" Text="" MaxLength="200"></asp:TextBox>
                                        </div>
                                    </div>
                                </td>
                                <td class="colname">Địa chỉ công ty
                                </td>
                                <td>
                                    <div class="inputstyle">
                                        <div class="bg">
                                            <asp:TextBox ID="txtDiaChiCongTy" runat="server" ReadOnly="true" Text="" MaxLength="500"></asp:TextBox>
                                        </div>
                                    </div>
                                </td>
                                <td>&nbsp;</td>
                            </tr>
                            <tr>
                                <td class="colname">Số fax
                                </td>
                                <td>
                                    <div class="inputstyle">
                                        <div class="bg">
                                            <asp:TextBox ID="txtFaxCongTy" runat="server" ReadOnly="true" Text="" MaxLength="20"></asp:TextBox>
                                        </div>
                                    </div>
                                </td>
                                <td class="colname">Điện thoại công ty
                                </td>
                                <td>
                                    <div class="inputstyle">
                                        <div class="bg">
                                            <asp:TextBox ID="txtDienThoaiCongTy" runat="server" ReadOnly="true" Text="" MaxLength="20"></asp:TextBox>
                                        </div>
                                    </div>
                                </td>
                                <td>&nbsp;</td>
                            </tr>
                            <tr>
                                <td class="colname">Trạng thái
                                </td>
                                <td align="left">
                                    <asp:RadioButton ID="rdBinhThuong" runat="server" Enabled="false" Text="Sử dụng" GroupName="rdTrangThai" />
                                    &nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:RadioButton ID="rdKhoa" runat="server" Enabled="false" Text="Không sử dụng" GroupName="rdTrangThai" />
                                </td>
                                <td class="colname"></td>
                                <td></td>
                                <td>&nbsp;</td>
                            </tr>
                            <tr>
                                <td class="colname">Nhóm người dùng
                                </td>
                                <td align="left">
                                    <div class="selectstyle">
                                        <div class="bg">
                                            <asp:DropDownList ID="ddlNhomNguoiDung" runat="server">
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                </td>
                                <td class="colname aligntop">
                                    <div style="display: none;">Nhóm người dùng hệ thống (Chưa hoạt động)</div>
                                </td>
                                <td colspan="2">
                                    <div style="display: none;">
                                        <asp:CheckBoxList ID="chklstGroup" RepeatColumns="2" CssClass="chk_nsd" runat="server" CellPadding="0" CellSpacing="0" RepeatDirection="Horizontal" RepeatLayout="Flow">
                                        </asp:CheckBoxList>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td class="colname">Khu vực </td>
                                <td align="left">
                                    <div class="selectstyle">
                                        <div class="bg">
                                            <asp:DropDownList ID="ddlKhuVuc" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlKhuVuc_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                </td>
                                <td class="colname">Đối tác </td>
                                <td>
                                    <div class="selectstyle">
                                        <div class="bg">
                                            <asp:DropDownList ID="ddlDoiTac" runat="server">
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                </td>
                                <td>&nbsp;</td>
                            </tr>
                        </table>
                        <!-- end body boot -->
                    </div>
                </div>
                <!-- end panel boot -->
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
