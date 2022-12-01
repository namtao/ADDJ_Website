<%@ Page Language="C#" MasterPageFile="~/admin/adminNotAJAX.master" AutoEventWireup="true"
    Inherits="admin_admin_manager" Title="Untitled Page" CodeBehind="admin_manager.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder_Text" runat="server">


    <script language="javascript">
        $(function() {
            $("#selectall").click(function() {
                $('.case').find("input").attr('checked', this.checked);
            });
            $(".case").click(function() {
            
            if ($(".case").find("//input[checked='checked']").length == $(".case").find("input").length) {
                    $("#selectall").attr("checked", "checked");
                } else {
                    $("#selectall").removeAttr("checked");
                }
            });
        });
    </script>

    <table width="100%" cellpadding="0" cellspacing="0" border="0">
        <tr valign="top">
            <td>
                <table cellpadding="1" cellspacing="1" border="0" class="text">
                    <tr>
                        <td>
                        </td>
                        <td>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td style="height: 10px">
            </td>
        </tr>
        <tr>
            <td>
                <table cellpadding="0" style="width: 600px" cellspacing="0">
                    <tr>
                        <td align="center" style="width: 100px; text-align: left">
                            <asp:Button ID="Button2" runat="server" Width="80px" CssClass="button" Text=" Thêm mới " OnClick="btThemMoi_Click"
                                />
                        </td>
                        <td align="right" style="width: 500px; text-align: right">
                            <asp:Button ID="Button3" runat="server" CssClass="button" Text=" Cập nhật " OnClick="btUpdate_Click"
                                Width="70px" />
                            <asp:Button ID="Button1" runat="server" CssClass="button" Text="Xóa" OnClick="btDelete_Click"
                                Width="40px" OnClientClick="javascript:{ return confirm('Bạn có muốn xóa user được chọn?');}" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td style="height: 10px">
            </td>
        </tr>
        <tr valign="top">
            <td style="text-align: center">
                <asp:GridView ID="grvView" runat="server" AutoGenerateColumns="False" DataKeyNames="id"
                    BackColor="White" BorderColor="#16538C" BorderStyle="Solid" BorderWidth="1px"
                    CellPadding="3" GridLines="Both" AllowPaging="True" PageSize="50" AllowSorting="True"
                    OnRowDataBound="grvView_RowDataBound" OnPageIndexChanging="grvView_PageIndexChanging">
                    <RowStyle BackColor="White" ForeColor="Black" />
                    <EditRowStyle BackColor="#999999" />
                    <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                    <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Right" />
                    <HeaderStyle BackColor="#2360A4" Font-Bold="True" ForeColor="White" />
                    <AlternatingRowStyle BackColor="#DEEAF3" ForeColor="Black" />
                    <Columns>
                        <asp:TemplateField ItemStyle-Width="50px" HeaderText="STT" HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-HorizontalAlign="Center"></asp:TemplateField>
                        <asp:BoundField ItemStyle-Width="200px" DataField="Username" HeaderText="Tên đăng nhập"
                            ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Center" />
                        <asp:BoundField DataField="FullName" HeaderText="Tên đầy đủ" HeaderStyle-HorizontalAlign="Center" />
                        <asp:TemplateField ItemStyle-Width="125px" HeaderText="Cho phép đăng nhập">
                            <ItemTemplate>
                                <asp:CheckBox ID="IsLogin" runat="server" Checked='<%# Convert.ToBoolean(Eval("IsLogin")) %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                Chọn
                                <input type="checkbox" id="selectall" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:CheckBox ID="cbSelectAll" CssClass="case" runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </td>
        </tr>
        <tr>
            <td style="height: 10px">
            </td>
        </tr>
        <tr>
            <td>
                <table cellpadding="0" cellspacing="0">
                    <tr>
                        <td align="center" style="width: 100px; text-align: left">
                            <asp:Button ID="btThemMoi" runat="server" CssClass="button" Text=" Thêm mới " OnClick="btThemMoi_Click"
                                Width="80px" />
                        </td>
                        <td align="right" style="width: 500px; text-align: right">
                            <asp:Button ID="btUpdate" runat="server" CssClass="button" Text=" Cập nhật " OnClick="btUpdate_Click"
                                Width="70px" />
                            <asp:Button ID="btDelete" runat="server" CssClass="button" Text="Xóa" OnClick="btDelete_Click"
                                Width="40px" OnClientClick="javascript:{ return confirm('Bạn có muốn xóa user được chọn?');}" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
            </td>
        </tr>
    </table>
</asp:Content>
