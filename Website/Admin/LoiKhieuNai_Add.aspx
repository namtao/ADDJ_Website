<%@ Page Title="" Language="C#" MasterPageFile="~/Master_Default.Master" AutoEventWireup="true" CodeBehind="LoiKhieuNai_Add.aspx.cs" Inherits="Website.admin.LoiKhieuNai_add" %>

<asp:Content ContentPlaceHolderID="Content" runat="Server">
    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <!-- begin panel nav boot -->
            <div class="nav_btn_bootstrap">
                <ul>
                    <li>
                        <a href="javascript:history.back()" class="btn btn-primary"><span class="glyphicon glyphicon-circle-arrow-left" aria-hidden="true"></span> Quay về</a>
                    </li>
                    <%-- <li><a href="LoiKhieuNai_Manager.aspx"><span class="back">Quay về</span></a></li>
                    --%>
                    <li>
                        <asp:LinkButton ID="linkbtnSubmit" class="btn btn-primary" OnClientClick="return validForm();" runat="server" OnClick="linkbtnSubmit_Click">
                             <span class="glyphicon glyphicon-refresh" aria-hidden="true"></span> Cập nhật
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

                        <table class="nobor">
                            <tr>
                                <td colspan="2" align="left">
                                    <asp:Label ID="lblMsg" runat="server" Text="" ForeColor="Red"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>Ngày áp dụng
                                </td>
                                <td>
                                    <div class="selectstyle">
                                        <div class="bg">
                                            <asp:DropDownList ID="ddlPhienBan" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlPhienBan_SelectedIndexChanged"></asp:DropDownList>
                                        </div>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td>Nguyên nhân lỗi cha
                                </td>
                                <td>
                                    <div class="selectstyle">
                                        <div class="bg">
                                            <asp:DropDownList ID="ddlLoiKhieuNai" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlLoiKhieuNai_SelectedIndexChanged"></asp:DropDownList>
                                        </div>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td>Lỗi khiếu nại
                                </td>
                                <td>
                                    <div class="inputstyle">
                                        <div class="bg">
                                            <asp:TextBox ID="txtTenLoi" runat="server" Text="" MaxLength="500" Width="450px"></asp:TextBox>
                                        </div>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td>Mã lỗi
                                </td>
                                <td>
                                    <div class="inputstyle">
                                        <div class="bg">
                                            <asp:TextBox ID="txtMaLoi" runat="server" Text="" MaxLength="5" Width="155px"></asp:TextBox>
                                        </div>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td>Thứ tự
                                </td>
                                <td>
                                    <div class="inputstyle">
                                        <div class="bg">
                                            <asp:TextBox ID="txtThuTu" runat="server" Text="" Width="155px"></asp:TextBox>
                                        </div>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td>Cấp
                                </td>
                                <td>
                                    <div class="inputstyle">
                                        <div class="bg">
                                            <asp:TextBox ID="txtCap" runat="server" Text="" Width="155px"></asp:TextBox>
                                        </div>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td>Loại
                                </td>
                                <td>
                                    <div class="selectstyle" style="width: 170px">
                                        <div class="bg">
                                            <asp:DropDownList ID="ddlLoai" runat="server">
                                                <asp:ListItem Value="1">Hỗ trợ</asp:ListItem>
                                                <asp:ListItem Value="2">Khiếu nại</asp:ListItem>
                                                <asp:ListItem Value="3">Khiếu nại CP</asp:ListItem>
                                                <asp:ListItem Value="4">Khiếu nại NET</asp:ListItem>
                                                <asp:ListItem Value="5">Khiếu nại VNP</asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td>Hoạt động
                                </td>
                                <td>
                                    <asp:CheckBox ID="chkHoatDong" runat="server" Checked="True" />
                                </td>
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
