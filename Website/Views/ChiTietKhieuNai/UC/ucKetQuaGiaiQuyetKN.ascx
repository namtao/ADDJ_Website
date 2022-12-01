<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucKetQuaGiaiQuyetKN.ascx.cs" Inherits="Website.Views.KhieuNai.UC.ucKetQuaGiaiQuyetKN" %>
<asp:UpdatePanel ID="UPQuaGiaiQuyetKN" runat="server">
    <ContentTemplate>
        <asp:GridView ID="gvKetQuaGiaiQuyetKN" runat="server" AutoGenerateColumns="False" CssClass="tbl_style NoPadding" PagerStyle-CssClass=""
            AlternatingRowStyle-CssClass="rowA" RowStyle-CssClass="rowB" ShowFooter="True" AllowPaging="true" PageSize="6"
            EditRowStyle-CssClass="" FooterStyle-CssClass="gridFooterRow" OnRowCancelingEdit="gvKetQuaGiaiQuyetKN_RowCancelingEdit" OnPageIndexChanging="gvKetQuaGiaiQuyetKN_PageIndexChanging"
            OnRowCommand="gvKetQuaGiaiQuyetKN_RowCommand" OnRowDataBound="gvKetQuaGiaiQuyetKN_RowDataBound" OnRowDeleting="gvKetQuaGiaiQuyetKN_RowDeleting"
            OnRowEditing="gvKetQuaGiaiQuyetKN_RowEditing" OnRowUpdating="gvKetQuaGiaiQuyetKN_RowUpdating" DataKeyNames="Id" ShowHeaderWhenEmpty="true">
            <Columns>
                <asp:TemplateField HeaderText="Ngày tạo" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="70px">
                    <EditItemTemplate>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <%# string.Format("{0:d/M/yyyy HH:mm}",Eval("CDate"))%>
                    </ItemTemplate>
                    <FooterTemplate>
                    </FooterTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Người tạo" ItemStyle-Width="80px">
                    <EditItemTemplate>
                        <%# Eval("CUser")%>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <%# Eval("CUser")%>
                    </ItemTemplate>
                    <FooterTemplate>
                    </FooterTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Phòng ban" ItemStyle-Width="80px">
                    <EditItemTemplate>
                        <%# Eval("PhongBanXuLyName")%>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <%# Eval("PhongBanXuLyName")%>
                    </ItemTemplate>
                    <FooterTemplate>
                    </FooterTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Phân tích số liệu" ItemStyle-HorizontalAlign="Center" FooterStyle-HorizontalAlign="Center">
                    <HeaderTemplate>
                        <table border="0" style="width: 100%; border-collapse: collapse" cellspacing="0">
                            <tr>
                                <td style="width: 20%; text-align: center">CSL
                                </td>
                                <td style="width: 20%; text-align: center">IR
                                </td>
                                <td style="text-align: center">DV khác
                                </td>
                            </tr>
                        </table>
                    </HeaderTemplate>
                    <EditItemTemplate>
                        <table border="0" class="nobor" style="width: 100%; border-collapse: collapse" cellspacing="0">
                            <tr>
                                <td style="width: 20%; text-align: center">
                                    <asp:CheckBox ID="cbIsCSL" runat="server" Checked='<%# Bind("IsCSL") %>' />
                                </td>
                                <td style="width: 20%; text-align: center">
                                    <asp:CheckBox ID="cbIsIR" runat="server" Checked='<%# Bind("PTSoLieu_IR") %>' />
                                </td>
                                <td style="text-align: center">
                                    <div class="inputstyle_longlx" style="margin-left: 5px">
                                        <div class="bg">
                                            <asp:TextBox ID="txtPTSL_Khac" runat="server" CssClass="mw" Text='<%# Bind("PTSoLieu_Khac") %>' TextMode="MultiLine"></asp:TextBox>
                                        </div>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </EditItemTemplate>
                    <FooterTemplate>
                        <table border="0" class="nobor" style="width: 100%; border-collapse: collapse" cellspacing="0">
                            <tr>
                                <td style="width: 20%; text-align: center">
                                    <asp:CheckBox ID="cbIsCSL" runat="server" Checked='<%# Bind("IsCSL") %>' />
                                </td>
                                <td style="width: 20%; text-align: center">
                                    <asp:CheckBox ID="cbIsIR" runat="server" Checked='<%# Bind("PTSoLieu_IR") %>' />
                                </td>
                                <td style="text-align: center">
                                    <div class="inputstyle_longlx" style="margin-left: 5px">
                                        <div class="bg">
                                            <asp:TextBox ID="txtPTSL_Khac" runat="server" CssClass="mw" Text='<%# Bind("PTSoLieu_Khac") %>' TextMode="MultiLine"></asp:TextBox>
                                        </div>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </FooterTemplate>
                    <ItemTemplate>
                        <table border="0" class="nobor" style="width: 100%; border-collapse: collapse" cellspacing="0">
                            <tr>
                                <td style="width: 20%; text-align: center">
                                    <asp:Label ID="lbIsCSL" runat="server" Text='<%# Bind("IsCSL") %>'></asp:Label>
                                </td>
                                <td style="width: 20%; text-align: center">
                                    <asp:Label ID="lbPTSoLieu_IR" runat="server" Text='<%# Bind("PTSoLieu_IR") %>'></asp:Label>
                                </td>
                                <td style="text-align: center"><%# Eval("PTSoLieu_Khac") %>
                                </td>
                            </tr>
                        </table>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Số hiệu công văn">
                    <HeaderTemplate>
                        <table border="0" style="width: 100%; border-collapse: collapse" cellspacing="0">
                            <tr>
                                <td style="width: 20%; text-align: center">CCT
                                </td>
                                <td style="text-align: center">SHCV
                                </td>
                            </tr>
                        </table>
                    </HeaderTemplate>
                    <EditItemTemplate>
                        <table border="0" class="nobor" style="width: 100%; border-collapse: collapse;" cellspacing="0">
                            <tr>
                                <td style="width: 20%; text-align: center">
                                    <asp:CheckBox ID="cbIsCCT" runat="server" Checked='<%# Bind("IsCCT") %>' />
                                </td>
                                <td style="text-align: center">
                                    <div class="inputstyle_longlx">
                                        <div class="bg">
                                            <asp:TextBox ID="txtSHCV" runat="server" Text='<%# Bind("SHCV") %>' CssClass="mw" TextMode="MultiLine"></asp:TextBox>
                                        </div>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </EditItemTemplate>
                    <FooterTemplate>
                        <table border="0" class="nobor" style="width: 100%; border-collapse: collapse;" cellspacing="0">
                            <tr>
                                <td style="width: 20%; text-align: center">
                                    <asp:CheckBox ID="cbIsCCT" runat="server" Checked='<%# Bind("IsCCT") %>' />
                                </td>
                                <td style="text-align: center">
                                    <div class="inputstyle_longlx">
                                        <div class="bg">
                                            <asp:TextBox ID="txtSHCV" runat="server" Text='<%# Bind("SHCV") %>' CssClass="mw" TextMode="MultiLine"></asp:TextBox>
                                        </div>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </FooterTemplate>
                    <ItemTemplate>
                        <table border="0" class="nobor" style="width: 100%; border-collapse: collapse;" cellspacing="0">
                            <tr>
                                <td style="width: 20%; text-align: center">
                                    <asp:Label ID="lbIsCCT" runat="server" Text='<%# Bind("IsCCT") %>'></asp:Label>
                                </td>
                                <td style="text-align: center">
                                    <%# Eval("SHCV") %>'
                                </td>
                            </tr>
                        </table>
                    </ItemTemplate>
                </asp:TemplateField>
                
                <asp:TemplateField HeaderText="Nội dung xử lý">
                    <EditItemTemplate>
                        <div class="inputstyle_longlx">
                            <div class="bg">

                                <asp:TextBox ID="txtNoiDungXuLy" runat="server" Rows="10" Height="100px" Text='<%# Bind("NoiDungXuLy") %>' CssClass="mw" TextMode="MultiLine"></asp:TextBox>
                            </div>
                        </div>
                    </EditItemTemplate>
                    <FooterTemplate>
                        <div class="inputstyle_longlx">
                            <div class="bg">

                                <asp:TextBox ID="txtNoiDungXuLy" runat="server" Rows="10" Height="100px" Text='<%# Bind("NoiDungXuLy") %>' CssClass="mw" TextMode="MultiLine"></asp:TextBox>
                            </div>
                        </div>
                    </FooterTemplate>
                    <ItemTemplate>
                        <%# Eval("NoiDungXuLy")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Kết quả xử lý">
                    <EditItemTemplate>
                        <div class="inputstyle_longlx">
                            <div class="bg">
                                <asp:TextBox ID="txtKetQuaXuLy" runat="server" Rows="10" Height="100px" Text='<%# Bind("KetQuaXuLy") %>' CssClass="mw" TextMode="MultiLine"></asp:TextBox>
                            </div>
                        </div>
                    </EditItemTemplate>
                    <FooterTemplate>
                        <div class="inputstyle_longlx">
                            <div class="bg">
                                <asp:TextBox ID="txtKetQuaXuLy" runat="server" Rows="10" Height="100px" Text='<%# Bind("KetQuaXuLy") %>' CssClass="mw" TextMode="MultiLine"></asp:TextBox>
                            </div>
                        </div>
                    </FooterTemplate>
                    <ItemTemplate>
                        <%# Eval("KetQuaXuLy")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Thao tác" ShowHeader="False" ItemStyle-HorizontalAlign="Center" FooterStyle-HorizontalAlign="Center">
                    <EditItemTemplate>
                        <asp:HiddenField runat="server" ID="hdCUser" Value='<%#Eval("CUser") %>' />
                        <span>
                            <asp:LinkButton ID="lnkUpdate" runat="server" CausesValidation="True" CommandName="Update"
                                CssClass="mybtn" OnClientClick="return confirm('Bạn có muốn sửa?')" ValidationGroup="UpdateKetQuaGiaiQuyetKN"><span class="save">Cập nhật</span></asp:LinkButton>
                            <asp:ValidationSummary ID="vsUpdateKetQuaGiaiQuyetKN" runat="server" ShowMessageBox="true" ShowSummary="false"
                                ValidationGroup="Update" Enabled="true" HeaderText="Lỗi dữ liệu..." />
                            <asp:LinkButton ID="lnkCancel" runat="server" CausesValidation="False" CommandName="Cancel"
                                CssClass="mybtn"><span class="cancel">Hủy</span></asp:LinkButton>
                            <asp:LinkButton ID="lnkDelete" runat="server" CausesValidation="False" CommandName="Delete"
                                CssClass="mybtn" OnClientClick="return confirm('Bạn có muốn xóa?')"><span class="del_file">Xoá</span></asp:LinkButton>
                        </span>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:HiddenField runat="server" ID="hdCUser" Value='<%#Eval("CUser") %>' />
                        <asp:HiddenField runat="server" ID="hdPhongBanXuLyId" Value='<%#Eval("PhongBanXuLyId") %>' />
                        <span>
                            <asp:LinkButton ID="lnkEdit" runat="server" CausesValidation="False" CommandName="Edit"
                                CssClass="mybtn"> <span class="edit">Sửa</span></asp:LinkButton>
                            <asp:LinkButton ID="lnkDelete" runat="server" CausesValidation="False" CommandName="Delete"
                                CssClass="mybtn" OnClientClick="return confirm('Bạn có muốn xóa?')"><span class="del_file">Xoá</span></asp:LinkButton>
                        </span>
                    </ItemTemplate>
                    <FooterTemplate>
                        <asp:LinkButton ID="lnkAdd" runat="server" ValidationGroup="InsertKetQuaGiaiQuyetKN" CausesValidation="true" CommandName="Insert"
                            CssClass="mybtn"> <span class="nhapdl">Thêm mới</span></asp:LinkButton>
                        <asp:ValidationSummary ID="vsInsertKetQuaGiaiQuyetKN" runat="server" ShowMessageBox="true" ShowSummary="false"
                            ValidationGroup="InsertKetQuaGiaiQuyetKN" Enabled="true" HeaderText="Lỗi nhập dữ liệu" />
                    </FooterTemplate>
                    <HeaderStyle />
                </asp:TemplateField>
            </Columns>
            <EmptyDataTemplate>

                <tr class="gridRow">
                    <td colspan="10">Chưa có dữ liệu...
                    </td>
                </tr>

                <tr class="gridFooterRow<%=taomoi?"":" dpn" %>">
                    <td style="width: 70px"></td>
                    <td style="width: 80px"></td>
                    <td style="width: 80px"></td>
                    <td style="text-align: center">
                        <table class="nobor" border="0" style="width: 100%; border-collapse: collapse" cellspacing="0">
                            <tr>
                                <td style="width: 20%; text-align: center">
                                    <asp:CheckBox ID="cbIsCSL" runat="server" />
                                </td>
                                <td style="width: 20%; text-align: center">
                                    <asp:CheckBox ID="cbIsIR" runat="server" />
                                </td>
                                <td style="text-align: center">
                                    <div class="inputstyle_longlx" style="margin-left: 5px">
                                        <div class="bg">
                                            <asp:TextBox ID="txtPTSL_Khac" runat="server" CssClass="mw" TextMode="MultiLine" />
                                        </div>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td>
                        <table border="0" class="nobor" style="width: 100%; border-collapse: collapse;" cellspacing="0">
                            <tr>
                                <td style="width: 20%; text-align: center">
                                    <asp:CheckBox ID="cbIsCCT" runat="server" />
                                </td>
                                <td style="text-align: center">
                                    <div class="inputstyle_longlx">
                                        <div class="bg">
                                            <asp:TextBox ID="txtSHCV" runat="server" CssClass="mw" TextMode="MultiLine"></asp:TextBox>
                                        </div>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </td>
                   
                    <td>
                        <div class="inputstyle_longlx">
                            <div class="bg">
                                <asp:TextBox ID="txtNoiDungXuLy" runat="server" Rows="10" Height="100px" CssClass="mw" TextMode="MultiLine"></asp:TextBox>
                            </div>
                        </div>
                    </td>
                    <td>
                        <div class="inputstyle_longlx">
                            <div class="bg">
                                <asp:TextBox ID="txtKetQuaXuLy" runat="server" Rows="10" Height="100px" CssClass="mw" TextMode="MultiLine"></asp:TextBox>
                            </div>
                        </div>
                    </td>
                    <td colspan="2" style="text-align: center">
                        <asp:LinkButton ID="lnkAdd" runat="server" ValidationGroup="InsertKetQuaGiaiQuyetKN" CausesValidation="true" CommandName="emptyInsert"
                            CssClass="mybtn"> <span class="nhapdl">Thêm mới</span></asp:LinkButton>
                        <asp:ValidationSummary ID="vsInsertKetQuaGiaiQuyetKN" runat="server" ShowMessageBox="true" ShowSummary="false"
                            ValidationGroup="InsertKetQuaGiaiQuyetKN" Enabled="true" HeaderText="Lỗi nhập dữ liệu" />
                    </td>
                </tr>
            </EmptyDataTemplate>
        </asp:GridView>
    </ContentTemplate>
</asp:UpdatePanel>
