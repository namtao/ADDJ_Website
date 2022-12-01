<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="uc_ReportType4_VNP.ascx.cs" Inherits="Website.Views.BaoCao.UC.uc_ReportType4_VNP" %>

 <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table class="tb2" width="100%">                       
                <tr>
                    <th colspan="4">                                
                        <asp:Label ID="lblTitle" runat="server" ></asp:Label>  
                        <asp:Label ID="lblReportType" runat="server" Visible ="false" ></asp:Label>     
                        <asp:Label ID="lblDoiTacId" runat="server" Visible ="false"  ></asp:Label>     
                        <asp:Label ID="lblPhongBanXuLyId" runat="server" Visible ="false"  ></asp:Label>     
                        <asp:Label ID="lblIsFirstLoai" runat="server" Visible ="false"  ></asp:Label>                               
                    </th>
                </tr>                                     
                <tr>
                    <td>
                        Từ ngày
                    </td>
                    <td>
                        <asp:TextBox ID="txtFromDate" runat="server" CssClass="fromdate"></asp:TextBox>                                        
                    </td>
                    <td>
                        đến ngày
                    </td>
                    <td>
                        <asp:TextBox ID="txtToDate" runat="server" CssClass="todate"></asp:TextBox>                                        
                    </td>
                </tr>                              
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td colspan="3">
                            <asp:RadioButtonList ID="rblLoaiBaoCao" runat="server" CssClass="surrounded" RepeatDirection="Horizontal">
                            <asp:ListItem Value="html" Selected="True">HTML</asp:ListItem>
                            <asp:ListItem Value="excel">EXCEL</asp:ListItem>
                        </asp:RadioButtonList>
                    </td>
                </tr>
                <tr>
                    <th colspan="4">
                        <asp:LinkButton ID="lbReport" runat="server" 
                            CssClass="poplight btn" OnClick="lbReport_Click">Báo cáo</asp:LinkButton>                                                                      
                    </th>
                </tr>
            </table>

        </ContentTemplate>
    </asp:UpdatePanel>

<asp:UpdatePanel ID="UpdatePanel2" runat="server">
    <ContentTemplate>
        <asp:GridView ID="XmlGridView" runat="server" AutoGenerateColumns="false"
            Height="247px" Width="795px" BackColor="White" BorderColor="#999999"
            BorderStyle="None" BorderWidth="1px" CellPadding="3"
            GridLines="Vertical" ShowFooter="true"
            OnRowCancelingEdit="XmlGridView_RowCancelingEdit" OnRowDeleting="XmlGridView_RowDeleting"
            OnRowEditing="XmlGridView_RowEditing" OnRowUpdating="XmlGridView_RowUpdating">
            <AlternatingRowStyle BackColor="#DCDCDC" />
                <Columns>
                    <asp:TemplateField HeaderText="Số thứ tự" HeaderStyle-Width="70" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:Label ID="lblSoTT" runat="server" Text='<%# Bind("SoTT")%>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txtEditSoTT" runat="server" Text='<%# Bind("SoTT")%>'></asp:TextBox>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="txtSoTT" runat="server"></asp:TextBox>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Tên đầy đủ">
                        <ItemTemplate>  
                            <asp:Label ID="lblTenDayDu" runat="server" Text='<%# Bind("TenDayDu") %>'></asp:Label>
                        </ItemTemplate>                        
                        <EditItemTemplate>
                            <asp:TextBox ID="txtEditTenDayDu" runat="server" Text='<%# Bind("TenDayDu") %>'></asp:TextBox>
                        </EditItemTemplate>
                        <FooterTemplate>    
                            <asp:TextBox ID="txtTenDayDu" runat="server"></asp:TextBox>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Tên truy cập">
                        <ItemTemplate>
                            <asp:Label ID="lblTenTruyCap" runat="server" Text='<%# Bind("TenTruyCap") %>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txtEditTenTruyCap" runat="server" Text='<%# Bind("TenTruyCap") %>'></asp:TextBox>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="txtTenTruyCap" runat="server"></asp:TextBox>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Hoạt động" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:Label ID="lblIsHoatDong" runat="server" Text='<%# Bind("IsHoatDong") %>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:CheckBox ID="chkEditIsHoatDong" runat="server" />
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:CheckBox ID="chkIsHoatDong" runat="server" Checked='<%# Bind("IsHoatDong") %>'></asp:CheckBox>
                        </FooterTemplate>
                    </asp:TemplateField>                    
                    <asp:TemplateField HeaderText="Hành động">
                        <ItemTemplate>
                            <asp:Button ID="btnEdit" runat="server" CommandName="Edit" Text="Sửa" />
                            <asp:Button ID="btnDelete" runat="server" CommandName="Delete" Text="Xóa" OnClientClick="return confirm('Bạn có chắc chắn muốn xóa bản ghi này không ?');" />
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:Button ID="bthUpdate" runat="server" CommandName="Update" Text="Cập nhật" />
                            <asp:Button ID="btnCancel" runat="server" CommandName="Cancel" Text="Hủy bỏ" />
                        </EditItemTemplate>        
                        <FooterTemplate>
                            <asp:Button ID="btnInsert" runat="server" Text="Thêm mới" OnClick="btnInsert_Click" />
                        </FooterTemplate>
                    </asp:TemplateField>
                </Columns>
            <FooterStyle BackColor="#CCCCCC" ForeColor="Black" />
            <HeaderStyle BackColor="#000084" Font-Bold="True" ForeColor="White" />
            <PagerStyle BackColor="#999999" ForeColor="Black" HorizontalAlign="Center" />
            <RowStyle BackColor="#EEEEEE" ForeColor="Black" />
            <SelectedRowStyle BackColor="#008A8C" Font-Bold="True" ForeColor="White" />
            <SortedAscendingCellStyle BackColor="#F1F1F1" />
            <SortedAscendingHeaderStyle BackColor="#0000A9" />
            <SortedDescendingCellStyle BackColor="#CAC9C9" />
            <SortedDescendingHeaderStyle BackColor="#000065" />
        </asp:GridView>
    </ContentTemplate>
</asp:UpdatePanel>
