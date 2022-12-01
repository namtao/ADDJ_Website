<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucFileDinhKem_Thieu.ascx.cs" Inherits="Website.Views.KhieuNai.UC.ucFileDinhKem_Thieu" %>



<asp:Label ID="lblMsg" runat="server" Text="" ForeColor="Red"></asp:Label>
<asp:GridView ID="gvFileDinhKem" runat="server" AutoGenerateColumns="False" CssClass="tbl_style" PagerStyle-CssClass=""
    AlternatingRowStyle-CssClass="rowA" RowStyle-CssClass="rowB" ShowFooter="True" AllowPaging="true" PageSize="6" CellPadding="0" CellSpacing="0" BorderWidth="1"
    EditRowStyle-CssClass="" FooterStyle-CssClass="gridFooterRow" OnRowCancelingEdit="gvFileDinhKem_RowCancelingEdit" OnPageIndexChanging="gvFileDinhKem_PageIndexChanging"
    OnRowCommand="gvFileDinhKem_RowCommand" OnRowDataBound="gvFileDinhKem_RowDataBound" OnRowDeleting="gvFileDinhKem_RowDeleting"
    OnRowEditing="gvFileDinhKem_RowEditing" OnRowUpdating="gvFileDinhKem_RowUpdating" DataKeyNames="Id">
    <Columns>
        <asp:TemplateField HeaderText="Tên file đính kèm">
            <EditItemTemplate>
                <span><%# Eval("TenFile") %></span>
            </EditItemTemplate>
            <FooterTemplate>
                <span>
                    <asp:FileUpload ID="FileUploadJquery" runat="server" accept="jpg|png|pdf|doc|docx|xls|xlsx|rar|zip|7z" /></span>
                <asp:RequiredFieldValidator ID="rfvFileUploadJquery" ControlToValidate="FileUploadJquery" runat="server" ErrorMessage="RequiredFieldValidator"></asp:RequiredFieldValidator>
            </FooterTemplate>
            <ItemTemplate>
                <span><a href='<%# AIVietNam.Core.Config.PathUploadFile + Eval("URLFile") %>' title="tải về"><%# Eval("TenFile") %></a></span>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Dung lượng">
            <EditItemTemplate>
                <span><%# string.Format("{0:N0} KB", Eval("KichThuoc")) %></span>
            </EditItemTemplate>
            <FooterTemplate>
            </FooterTemplate>
            <ItemTemplate>
                <span><%# string.Format("{0:N0} KB", Eval("KichThuoc")) %></span>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Ghi chú">
            <EditItemTemplate>
                <span>
                    <asp:TextBox ID="txtGhiChu" runat="server" Text='<%# Bind("GhiChu") %>' CssClass="mw" TextMode="MultiLine"></asp:TextBox></span>
            </EditItemTemplate>
            <FooterTemplate>
                <span>
                    <asp:TextBox ID="txtGhiChu" runat="server" CssClass="mw" TextMode="MultiLine"></asp:TextBox></span>
            </FooterTemplate>
            <ItemTemplate>
                <span><%# Eval("GhiChu")%></span>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Người xử lý">
            <EditItemTemplate>
                <span><%# Eval("CUser")%></span>
            </EditItemTemplate>
            <FooterTemplate>
            </FooterTemplate>
            <ItemTemplate>
                <span><%# Eval("CUser")%></span>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Ngày Khai báo">
            <EditItemTemplate>
                <span><%# string.Format("{0:d/M/yyyy HH:mm}",DateTime.Now)%></span>
            </EditItemTemplate>
            <ItemTemplate>
                <span><%# string.Format("{0:d/M/yyyy HH:mm}",Eval("CDate"))%></span>
            </ItemTemplate>
            <FooterTemplate>
                <span><%# string.Format("{0:d/M/yyyy HH:mm}",DateTime.Now)%></span>
            </FooterTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Thao tác" ShowHeader="False">
            <EditItemTemplate>
                <span>
                    <asp:LinkButton ID="lnkUpdate" runat="server" CausesValidation="True" CommandName="Update"
                        Text="Cập nhật | " OnClientClick="return confirm('Bạn có muốn sửa?')" ValidationGroup="UpdateFileDinhKem"></asp:LinkButton>
                    <asp:ValidationSummary ID="vsUpdateFileDinhKem" runat="server" ShowMessageBox="true" ShowSummary="false"
                        ValidationGroup="UpdateFileDinhKem" Enabled="true" HeaderText="Lỗi dữ liệu..." />
                    <asp:LinkButton ID="lnkCancel" runat="server" CausesValidation="False" CommandName="Cancel"
                        Text="Hủy"></asp:LinkButton></span>
            </EditItemTemplate>
            <FooterTemplate>
                <span>
                    <asp:LinkButton ID="lnkAdd" runat="server" CausesValidation="True" CommandName="Insert"
                        ValidationGroup="InsertFileDinhKem" Text="Thêm mới"></asp:LinkButton>
                    <asp:ValidationSummary ID="vsInsertFileDinhKem" runat="server" ShowMessageBox="true" ShowSummary="false"
                        ValidationGroup="InsertFileDinhKem" Enabled="true" HeaderText="Lỗi nhập dữ liệu" />
                </span>
            </FooterTemplate>
            <ItemTemplate>
                <span>
                    <asp:LinkButton ID="lnkEdit" runat="server" CausesValidation="False" CommandName="Edit"
                        Text="Sửa"></asp:LinkButton></span>
            </ItemTemplate>
            <HeaderStyle />
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Xóa" ShowHeader="False">
            <ItemTemplate>
                <span>
                    <asp:LinkButton ID="lnkDelete" runat="server" CausesValidation="False" CommandName="Delete"
                        Text="Xóa" OnClientClick="return confirm('Bạn có muốn xóa?')"></asp:LinkButton></span>
            </ItemTemplate>
            <HeaderStyle />
        </asp:TemplateField>
    </Columns>
    <EmptyDataTemplate>
        <table class="tbl_style" cellspacing="0" cellpadding="0" border="0" style="border-collapse: collapse;">
            <tr>
                <th scope="col"><span>Tên file đính kèm</span>
                </th>
                <th scope="col"><span>Dung lượng</span>
                </th>
                <th scope="col"><span>Ghi chú</span>
                </th>
                <th scope="col"><span>Ngày Khai báo</span>
                </th>
                <th scope="col"><span>Sửa</span>
                </th>
                <th scope="col"><span>Xóa</span>
                </th>
            </tr>
            <tr class="gridRow">
                <td colspan="8"><span>Chưa có dữ liệu...</span>
                </td>
            </tr>
            <tr class="gridFooterRow">
                <td>
                    <span>
                        <asp:FileUpload ID="FileUploadJquery" runat="server" accept="jpg|png|pdf|doc|docx|xls|xlsx|rar|zip|7z" /></span>
                </td>
                <td></td>
                <td>
                    <span>
                        <asp:TextBox ID="txtGhiChu" runat="server" CssClass="mw" TextMode="MultiLine"></asp:TextBox></span>
                </td>
                <td></td>
                <td>
                    <span><%# string.Format("{0:d/M/yyyy HH:mm}",DateTime.Now)%></span>
                </td>
                <td colspan="2">
                    <span>
                        <asp:LinkButton ID="lnkAdd" runat="server" ValidationGroup="InsertFileDinhKem" CausesValidation="true" CommandName="emptyInsert"
                            Text="Thêm mới"></asp:LinkButton>
                        <asp:ValidationSummary ID="vsInsertFileDinhKem" runat="server" ShowMessageBox="true" ShowSummary="false"
                            ValidationGroup="InsertFileDinhKem" Enabled="true" HeaderText="Lỗi nhập dữ liệu" />
                    </span>
                </td>
            </tr>
        </table>
    </EmptyDataTemplate>
</asp:GridView>

