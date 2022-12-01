<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucTimKiemGiaiPhap.ascx.cs" Inherits="Website.Views.KhieuNai.UC.ucTimKiemGiaiPhap" %>

<asp:UpdatePanel ID="UPTimKiemGiaiPhap" runat="server">
    <ContentTemplate>
        <asp:GridView ID="gvTimKiemGiaiPhap" runat="server" AutoGenerateColumns="False" CssClass="tbl_style NoPadding" PagerStyle-CssClass=""
            AlternatingRowStyle-CssClass="rowA" RowStyle-CssClass="rowB" ShowFooter="True" AllowPaging="true" PageSize="6" CellPadding="5"
            EditRowStyle-CssClass="" FooterStyle-CssClass="gridFooterRow" OnRowCancelingEdit="gvTimKiemGiaiPhap_RowCancelingEdit" OnPageIndexChanging="gvTimKiemGiaiPhap_PageIndexChanging"
            OnRowCommand="gvTimKiemGiaiPhap_RowCommand" OnRowDataBound="gvTimKiemGiaiPhap_RowDataBound" OnRowDeleting="gvTimKiemGiaiPhap_RowDeleting"
            OnRowEditing="gvTimKiemGiaiPhap_RowEditing" OnRowUpdating="gvTimKiemGiaiPhap_RowUpdating" DataKeyNames="Id,CUser" ShowHeaderWhenEmpty="true">
            <Columns>
                <asp:TemplateField HeaderText="Tên">
                    <EditItemTemplate>
                        <div class="inputstyle_longlx">
                            <div class="bg">
                                <span>
                                    <asp:TextBox ID="txtName" runat="server" Text='<%# Bind("Name") %>' CssClass="mw" TextMode="MultiLine"></asp:TextBox></span>
                            </div>
                        </div>
                        <asp:RequiredFieldValidator ID="rfvName" ValidationGroup="Update" runat="server"
                            ControlToValidate="txtName" ErrorMessage="Bạn chưa nhập tên" Display="None"
                            ToolTip="Bạn chưa nhập tên" SetFocusOnError="true" ForeColor="Red">*</asp:RequiredFieldValidator>
                    </EditItemTemplate>
                    <FooterTemplate>
                        <div class="inputstyle_longlx">
                            <div class="bg">
                                <span>
                                    <asp:TextBox ID="txtName" runat="server" CssClass="mw" TextMode="MultiLine"></asp:TextBox></span>
                            </div>
                        </div>
                        <asp:RequiredFieldValidator ID="rfvNoiDung" ValidationGroup="Insert" runat="server"
                            ControlToValidate="txtName" ErrorMessage="Bạn chưa nhập nội dung xử lý" Display="None"
                            ToolTip="Bạn chưa nhập nội dung xử lý" SetFocusOnError="true" ForeColor="Red">*</asp:RequiredFieldValidator>
                    </FooterTemplate>
                    <ItemTemplate>
                        <span><%# Eval("Name")%></span>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="FAQ">
                    <EditItemTemplate>
                        <div class="inputstyle_longlx">
                            <div class="bg">
                                <span>
                                    <asp:TextBox ID="txtFAQ" runat="server" Text='<%# Bind("FAQ") %>' CssClass="mw" TextMode="MultiLine"></asp:TextBox></span>
                            </div>
                        </div>
                        <asp:RequiredFieldValidator ID="rfvFAQ" ValidationGroup="Update" runat="server"
                            ControlToValidate="txtFAQ" ErrorMessage="Bạn chưa nhập FAQ" Display="None"
                            ToolTip="Bạn chưa nhập FAQ" SetFocusOnError="true" ForeColor="Red">*</asp:RequiredFieldValidator>
                    </EditItemTemplate>
                    <FooterTemplate>
                        <div class="inputstyle_longlx">
                            <div class="bg">
                                <span>
                                    <asp:TextBox ID="txtFAQ" runat="server" CssClass="mw" TextMode="MultiLine"></asp:TextBox></span>
                            </div>
                        </div>
                        <asp:RequiredFieldValidator ID="rfvFAQ" ValidationGroup="Insert" runat="server"
                            ControlToValidate="txtFAQ" ErrorMessage="Bạn chưa nhập FAQ" Display="None"
                            ToolTip="Bạn chưa nhập FAQ" SetFocusOnError="true" ForeColor="Red">*</asp:RequiredFieldValidator>
                    </FooterTemplate>
                    <ItemTemplate>
                        <span><%# Eval("FAQ")%></span>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Mô tả">
                    <EditItemTemplate>
                        <div class="inputstyle_longlx">
                            <div class="bg">
                                <span>
                                    <asp:TextBox ID="txtMoTa" runat="server" Text='<%# Bind("MoTa") %>' CssClass="mw" TextMode="MultiLine"></asp:TextBox></span>
                            </div>
                        </div>
                        <asp:RequiredFieldValidator ID="rfvMoTa" ValidationGroup="Update" runat="server"
                            ControlToValidate="txtMoTa" ErrorMessage="Bạn chưa nhập mô tả" Display="None"
                            ToolTip="Bạn chưa nhập mô tả" SetFocusOnError="true" ForeColor="Red">*</asp:RequiredFieldValidator>
                    </EditItemTemplate>
                    <FooterTemplate>
                        <div class="inputstyle_longlx">
                            <div class="bg">
                                <span>
                                    <asp:TextBox ID="txtMoTa" runat="server" CssClass="mw" TextMode="MultiLine"></asp:TextBox></span>
                            </div>
                        </div>
                        <asp:RequiredFieldValidator ID="rfvMoTa" ValidationGroup="Insert" runat="server"
                            ControlToValidate="txtMoTa" ErrorMessage="Bạn chưa nhập mô tả" Display="None"
                            ToolTip="Bạn chưa nhập mô tả" SetFocusOnError="true" ForeColor="Red">*</asp:RequiredFieldValidator>
                    </FooterTemplate>
                    <ItemTemplate>
                        <span><%# Eval("MoTa") %></span>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Ghi chú">
                    <EditItemTemplate>
                        <div class="inputstyle_longlx">
                            <div class="bg">
                                <span>
                                    <asp:TextBox ID="txtComments" runat="server" Text='<%# Bind("Comments") %>' CssClass="mw" TextMode="MultiLine"></asp:TextBox></span>
                            </div>
                        </div>
                    </EditItemTemplate>
                    <FooterTemplate>
                        <div class="inputstyle_longlx">
                            <div class="bg">
                                <span>
                                    <asp:TextBox ID="txtComments" runat="server" CssClass="mw" TextMode="MultiLine"></asp:TextBox></span>
                            </div>
                        </div>
                    </FooterTemplate>
                    <ItemTemplate>
                        <span><%# Eval("Comments")%></span>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Thao tác" ShowHeader="False" ItemStyle-HorizontalAlign="Center" FooterStyle-HorizontalAlign="Center">
                    <EditItemTemplate>
                        <span>
                            <asp:LinkButton ID="lnkUpdate" runat="server" CausesValidation="True" CommandName="Update"
                                CssClass="mybtn" OnClientClick="return confirm('Bạn có muốn sửa?')" ValidationGroup="Update"><span class="save">Cập nhật</span></asp:LinkButton>
                            <asp:ValidationSummary ID="vsUpdate" runat="server" ShowMessageBox="true" ShowSummary="false"
                                ValidationGroup="Update" Enabled="true" HeaderText="Lỗi dữ liệu..." />
                            <asp:LinkButton ID="lnkCancel" runat="server" CausesValidation="False" CommandName="Cancel"
                                CssClass="mybtn"><span class="cancel">Hủy</span></asp:LinkButton>
                            <asp:LinkButton ID="lnkDelete" runat="server" CausesValidation="False" CommandName="Delete"
                                CssClass="mybtn" OnClientClick="return confirm('Bạn có muốn xóa?')"><span class="del_file">Xoá</span></asp:LinkButton>
                        </span>
                    </EditItemTemplate>
                    <FooterTemplate>
                        <span>
                            <asp:LinkButton ID="lnkAdd" runat="server" CausesValidation="True" CommandName="Insert"
                                ValidationGroup="Insert" CssClass="mybtn"> <span class="nhapdl">Thêm mới</span></asp:LinkButton></span>
                        <asp:ValidationSummary ID="vsInsert" runat="server" ShowMessageBox="true" ShowSummary="false"
                            ValidationGroup="Insert" Enabled="true" HeaderText="Lỗi nhập dữ liệu" />
                    </FooterTemplate>
                    <ItemTemplate>
                        <span>
                            <asp:LinkButton ID="lnkEdit" runat="server" CausesValidation="False" CommandName="Edit"
                                CssClass="mybtn"> <span class="edit">Sửa</span></asp:LinkButton>
                            <asp:LinkButton ID="lnkDelete" runat="server" CausesValidation="False" CommandName="Delete"
                                CssClass="mybtn" OnClientClick="return confirm('Bạn có muốn xóa?')"><span class="del_file">Xoá</span></asp:LinkButton>
                        </span>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
            <EmptyDataTemplate>
                <tr class="gridRow">
                    <td colspan="8"><span>Chưa có dữ liệu...</span>
                    </td>
                </tr>
                <tr class="gridFooterRow">
                    <td>
                        <div class="inputstyle_longlx">
                            <div class="bg">
                                <span>
                                    <asp:TextBox ID="txtName" runat="server" CssClass="mw" TextMode="MultiLine"></asp:TextBox></span>
                            </div>
                        </div>
                        <asp:RequiredFieldValidator ID="rfvtName" ValidationGroup="Insert" runat="server"
                            ControlToValidate="txtName" ErrorMessage="Bạn chưa nhập tên" Display="None"
                            ToolTip="Bạn chưa nhập tên" SetFocusOnError="true" ForeColor="Red">*</asp:RequiredFieldValidator>
                    </td>
                    <td>
                        <div class="inputstyle_longlx">
                            <div class="bg">
                                <span>
                                    <asp:TextBox ID="txtFAQ" runat="server" CssClass="mw" TextMode="MultiLine"></asp:TextBox></span>
                            </div>
                        </div>
                        <asp:RequiredFieldValidator ID="rfvFAQ" ValidationGroup="Insert" runat="server"
                            ControlToValidate="txtFAQ" ErrorMessage="Bạn chưa nhập FAQ" Display="None"
                            ToolTip="Bạn chưa nhập FAQ" SetFocusOnError="true" ForeColor="Red">*</asp:RequiredFieldValidator>
                    </td>
                    <td>
                        <div class="inputstyle_longlx">
                            <div class="bg">
                                <span>
                                    <asp:TextBox ID="txtMoTa" runat="server" CssClass="mw" TextMode="MultiLine"></asp:TextBox></span>
                            </div>
                        </div>
                        <asp:RequiredFieldValidator ID="rfvMoTa" ValidationGroup="Insert" runat="server"
                            ControlToValidate="txtMoTa" ErrorMessage="Bạn chưa nhập mô tả" Display="None"
                            ToolTip="Bạn chưa nhập mô tả" SetFocusOnError="true" ForeColor="Red">*</asp:RequiredFieldValidator>
                    </td>
                    <td>
                        <div class="inputstyle_longlx">
                            <div class="bg">
                                <span>
                                    <asp:TextBox ID="txtComments" runat="server" CssClass="mw" TextMode="MultiLine"></asp:TextBox></span>
                            </div>
                        </div>
                    </td>
                    <td colspan="2" style="text-align: center">
                        <asp:LinkButton ID="lnkAdd" runat="server" ValidationGroup="Insert" CausesValidation="true" CommandName="emptyInsert"
                            CssClass="mybtn"> <span class="nhapdl">Thêm mới</span></asp:LinkButton>
                        <asp:ValidationSummary ID="vsInsert" runat="server" ShowMessageBox="true" ShowSummary="false"
                            ValidationGroup="Insert" Enabled="true" HeaderText="Lỗi nhập dữ liệu" />
                    </td>
                </tr>
            </EmptyDataTemplate>
        </asp:GridView>
    </ContentTemplate>
</asp:UpdatePanel>
