<%@ Page Language="C#" MasterPageFile="~/Master_Default.Master" AutoEventWireup="true" Inherits="ConfigurationTime" CodeBehind="ConfigurationTime.aspx.cs" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content runat="server" ContentPlaceHolderID="HeaderCss">
    <style type="text/css">
        .nobor td {
            padding: 0px;
        }
    </style>
</asp:Content>
<asp:Content ContentPlaceHolderID="Content" runat="Server">
    <asp:UpdatePanel ID="UpdatePane" runat="server">
        <ContentTemplate>
            <!-- begin panel nav boot -->
            <div class="nav_btn_bootstrap">
                <ul>
                    <li><a href="javascript:history.back()" class="btn btn-primary"><span class="glyphicon glyphicon-circle-arrow-left" aria-hidden="true"></span> Quay về</a></li>
                    <li>
                        <asp:LinkButton ID="linkbtnCapNhat" class="btn btn-primary" runat="server" OnClick="linkbtnCapNhat_Click">
                            <span class="glyphicon glyphicon-floppy-saved" aria-hidden="true"></span> Cập nhật
                        </asp:LinkButton>
                    </li>
                </ul>
            </div>
            <!-- end panel nav boot -->
            <div class="p8">
                <!-- begin panel boot -->
                <div class="panel panel-default">
                    <div class="panel-heading"><span style="font-size: 12pt">Cấu hình</span></div>
                    <div class="panel-body" style="border: none">
                        <!-- begin body boot -->
                        <table width="100%" cellpadding="0" cellspacing="0" border="0">
                            <tr valign="top">
                                <td style="height: 5px"></td>
                            </tr>
                            <tr>
                                <td style="height: 10px"></td>
                            </tr>
                            <tr>
                                <td>
                                    <table class="nobor" width="100%">
                                        <tr>
                                            <td style="width: 200px">Ngày bắt đầu tuần:
                                            </td>
                                            <td style="width: 100px">
                                                <div class="selectstyle">
                                                    <div class="bg">
                                                        <asp:DropDownList ID="ddlNgayBatDauTuan" runat="server">
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </td>
                                            <td>&nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td>Tổng số ngày làm việc trên tuần:
                                            </td>
                                            <td>
                                                <div class="inputstyle">
                                                    <div class="bg">
                                                        <asp:TextBox ID="txtTongSoNgayLamViec" Width="100px" runat="server"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </td>
                                            <td>&nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td class="auto-style1">Giờ bắt đầu
                                            </td>
                                            <td class="auto-style1">
                                                <div class="selectstyle">
                                                    <div class="bg">
                                                        <asp:DropDownList ID="ddlGioBatDau" runat="server">
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </td>
                                            <td class="auto-style1"></td>
                                        </tr>
                                        <tr>
                                            <td>Giờ kết thúc
                                            </td>
                                            <td>
                                                <div class="selectstyle">
                                                    <div class="bg">
                                                        <asp:DropDownList ID="ddlGioKetThuc" runat="server">
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </td>
                                            <td>&nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td><strong>Danh sách ngày lễ của năm </strong></td>
                                            <td>
                                                <div class="selectstyle">
                                                    <div class="bg">
                                                        <asp:DropDownList ID="ddlNam" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlNam_SelectedIndexChanged">
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </td>
                                            <td>&nbsp;</td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td>&nbsp;</td>
                            </tr>
                            <tr>
                                <td style="height: 10px"></td>
                            </tr>
                            <tr valign="top">
                                <td style="text-align: center">
                                    <asp:GridView ID="grvView" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered table-hover table-striped table-condensed" PagerStyle-CssClass=""
                                        AlternatingRowStyle-CssClass="rowA" RowStyle-CssClass="rowB" ShowFooter="True" CellPadding="0" CellSpacing="0" BorderWidth="1"
                                        EditRowStyle-CssClass="" FooterStyle-CssClass="gridFooterRow" OnRowCancelingEdit="grvView_RowCancelingEdit"
                                        OnRowCommand="grvView_RowCommand" OnRowDataBound="grvView_RowDataBound" OnRowDeleting="grvView_RowDeleting"
                                        OnRowEditing="grvView_RowEditing" OnRowUpdating="grvView_RowUpdating" DataKeyNames="NgayThang" ShowHeaderWhenEmpty="true" Width="100%">
                                        <AlternatingRowStyle CssClass="rowA" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="Tên ngày lễ">
                                                <EditItemTemplate>
                                                    <div class="inputstyle_longlx">
                                                        <div class="bg">
                                                            <span>
                                                                <asp:TextBox ID="txtName" runat="server" Text='<%# Bind("Name") %>' CssClass="mw"></asp:TextBox></span>
                                                        </div>
                                                    </div>
                                                    <asp:RequiredFieldValidator ID="rfvNoiDung" ValidationGroup="UpdateCacBuocXuLy" runat="server"
                                                        ControlToValidate="txtName" ErrorMessage="Bạn chưa nhập tên ngày lễ" Display="None"
                                                        ToolTip="Bạn chưa nhập tên ngày lễ" SetFocusOnError="true" ForeColor="Red">*</asp:RequiredFieldValidator>
                                                </EditItemTemplate>
                                                <FooterTemplate>
                                                    <div class="inputstyle_longlx">
                                                        <div class="bg">
                                                            <span>
                                                                <asp:TextBox ID="txtName" runat="server" CssClass="mw"></asp:TextBox></span>
                                                        </div>
                                                    </div>
                                                    <asp:RequiredFieldValidator ID="rfvNoiDung" ValidationGroup="InsertCacBuocXuLy" runat="server"
                                                        ControlToValidate="txtName" ErrorMessage="Bạn chưa nhập tên ngày lễ" Display="None"
                                                        ToolTip="Bạn chưa nhập tên ngày lễ" SetFocusOnError="true" ForeColor="Red">*</asp:RequiredFieldValidator>
                                                </FooterTemplate>
                                                <ItemTemplate>
                                                    <span><%# Eval("Name")%></span>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Ngày tháng">
                                                <EditItemTemplate>
                                                    <div class="inputstyle_longlx">
                                                        <div class="bg">
                                                            <span>
                                                                <asp:TextBox ID="txtNgayThang" runat="server" Text='<%# Bind("NgayThang") %>' CssClass="mw"></asp:TextBox>
                                                                <cc1:CalendarExtender ID="calendarNgayThang" Format="dd/MM/yyyy" runat="server" TargetControlID="txtNgayThang"></cc1:CalendarExtender>
                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ValidationGroup="InsertCacBuocXuLy" runat="server"
                                                                    ControlToValidate="txtNgayThang" ErrorMessage="Bạn chưa chọn ngày tháng" Display="None"
                                                                    ToolTip="Bạn chưa chọn ngày tháng" SetFocusOnError="true" ForeColor="Red">*</asp:RequiredFieldValidator>
                                                            </span>
                                                        </div>
                                                    </div>
                                                </EditItemTemplate>
                                                <FooterTemplate>
                                                    <div class="inputstyle_longlx">
                                                        <div class="bg">
                                                            <span>
                                                                <asp:TextBox ID="txtNgayThang" runat="server" Text='<%# Bind("NgayThang") %>' CssClass="mw"></asp:TextBox>
                                                                <cc1:CalendarExtender ID="calendarNgayThang" Format="dd/MM/yyyy" runat="server" TargetControlID="txtNgayThang"></cc1:CalendarExtender>
                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ValidationGroup="InsertCacBuocXuLy" runat="server"
                                                                    ControlToValidate="txtNgayThang" ErrorMessage="Bạn chưa chọn ngày tháng" Display="None"
                                                                    ToolTip="Bạn chưa chọn ngày tháng" SetFocusOnError="true" ForeColor="Red">*</asp:RequiredFieldValidator>
                                                            </span>
                                                        </div>
                                                    </div>
                                                </FooterTemplate>
                                                <ItemTemplate>
                                                    <span><%# Bind_NgayThang(Eval("NgayThang")) %></span>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Thao tác" ShowHeader="False" ItemStyle-HorizontalAlign="Center" FooterStyle-HorizontalAlign="Center">
                                                <EditItemTemplate>
                                                    <span>
                                                        <asp:LinkButton ID="lnkUpdate" runat="server" CausesValidation="True" CommandName="Update"
                                                            CssClass="mybtn" OnClientClick="return confirm('Bạn có muốn sửa?')" ValidationGroup="UpdateCacBuocXuLy"><span class="save">Cập nhật</span></asp:LinkButton>
                                                        <asp:ValidationSummary ID="vsUpdateCacBuocXuLy" runat="server" ShowMessageBox="true" ShowSummary="false"
                                                            ValidationGroup="UpdateCacBuocXuLy" Enabled="true" HeaderText="Lỗi dữ liệu..." />
                                                        <asp:LinkButton ID="lnkCancel" runat="server" CausesValidation="False" CommandName="Cancel"
                                                            CssClass="mybtn"><span class="cancel">Hủy</span></asp:LinkButton>
                                                        <asp:LinkButton ID="lnkDelete" runat="server" CausesValidation="False" CommandName="Delete"
                                                            CssClass="mybtn" OnClientClick="return confirm('Bạn có muốn xóa?')"><span class="del_file">Xoá</span></asp:LinkButton></span>
                                                    </span>
                                                </EditItemTemplate>
                                                <FooterTemplate>
                                                    <span>
                                                        <asp:LinkButton ID="lnkAdd" runat="server" CausesValidation="True" CommandName="Insert"
                                                            ValidationGroup="InsertCacBuocXuLy" CssClass="mybtn"> <span class="nhapdl">Thêm mới</span></asp:LinkButton></span>
                                                    <asp:ValidationSummary ID="vsInsertCacBuocXuLy" runat="server" ShowMessageBox="true" ShowSummary="false"
                                                        ValidationGroup="InsertCacBuocXuLy" Enabled="true" HeaderText="Lỗi nhập dữ liệu" />
                                                </FooterTemplate>
                                                <ItemTemplate>
                                                    <span>
                                                        <asp:LinkButton ID="lnkEdit" runat="server" CausesValidation="False" CommandName="Edit"
                                                            CssClass="mybtn"> <span class="edit">Sửa</span></asp:LinkButton>
                                                        <asp:LinkButton ID="lnkDelete" runat="server" CausesValidation="False" CommandName="Delete"
                                                            CssClass="mybtn" OnClientClick="return confirm('Bạn có muốn xóa?')"><span class="del_file">Xoá</span></asp:LinkButton></span>
                                                </ItemTemplate>
                                                <FooterStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                        </Columns>
                                        <EmptyDataTemplate>
                                            <tr class="gridRow">
                                                <td colspan="10"><span>Chưa có dữ liệu...</span>
                                                </td>
                                            </tr>
                                            <tr class="gridFooterRow">
                                                <td>
                                                    <div class="inputstyle_longlx">
                                                        <div class="bg">
                                                            <span>
                                                                <asp:TextBox ID="txtName" runat="server" CssClass="mw"></asp:TextBox></span>
                                                        </div>
                                                    </div>
                                                    <asp:RequiredFieldValidator ID="rfvNoiDung" ValidationGroup="InsertCacBuocXuLy" runat="server"
                                                        ControlToValidate="txtName" ErrorMessage="Bạn chưa nhập tên ngày lễ" Display="None"
                                                        ToolTip="Bạn chưa nhập tên ngày lễ" SetFocusOnError="true" ForeColor="Red">*</asp:RequiredFieldValidator>
                                                </td>
                                                <td>
                                                    <div class="inputstyle_longlx">
                                                        <div class="bg">
                                                            <span>
                                                                <asp:TextBox ID="txtNgayThang" runat="server" Text='<%# Bind("NgayThang") %>' CssClass="mw"></asp:TextBox>
                                                                <cc1:CalendarExtender ID="calendarNgayThang" runat="server" Format="dd/MM/yyyy" TargetControlID="txtNgayThang"></cc1:CalendarExtender>
                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ValidationGroup="InsertCacBuocXuLy" runat="server"
                                                                    ControlToValidate="txtNgayThang" ErrorMessage="Bạn chưa chọn ngày tháng" Display="None"
                                                                    ToolTip="Bạn chưa chọn ngày tháng" SetFocusOnError="true" ForeColor="Red">*</asp:RequiredFieldValidator>
                                                            </span>
                                                        </div>
                                                    </div>
                                                </td>
                                                <td colspan="2" style="text-align: center">
                                                    <asp:LinkButton ID="lnkAdd" runat="server" ValidationGroup="InsertCacBuocXuLy" CausesValidation="true" CommandName="emptyInsert"
                                                        CssClass="mybtn"> <span class="nhapdl">Thêm mới</span></asp:LinkButton>
                                                    <asp:ValidationSummary ID="vsInsertCacBuocXuLy" runat="server" ShowMessageBox="true" ShowSummary="false"
                                                        ValidationGroup="InsertCacBuocXuLy" Enabled="true" HeaderText="Lỗi nhập dữ liệu" />
                                                </td>
                                            </tr>
                                        </EmptyDataTemplate>
                                        <FooterStyle CssClass="gridFooterRow" />
                                        <RowStyle CssClass="rowB" />
                                    </asp:GridView>
                                    <br />
                                </td>
                            </tr>
                            <tr>
                                <td style="height: 200px;"></td>
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


