﻿<%@ Page Language="C#" MasterPageFile="~/admin/adminNotAJAX.master" AutoEventWireup="true"
    Inherits="admin_news_manager" Title="Untitled Page" CodeBehind="news_manager.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder_Text" runat="Server">

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
            <td style="height: 5px; text-align: left">
                <asp:Literal ID="ltThongBao" runat="server"></asp:Literal>
            </td>
        </tr>
        <tr>
            <td style="height: 10px">
            </td>
        </tr>
        <tr>
            <td>
                <table cellpadding="0" cellspacing="0" style="width: 600px">                    
                    <tr>
                        <td style="text-align: left; width: 250px">
                            Chọn nhóm tin
                        </td>
                        <td style="text-align: left">
                            <asp:DropDownList ID="ddlCategoryNews" runat="server">
                            </asp:DropDownList>
                        </td>
                    </tr>      
                    <tr><td style="height:5px"></td></tr>
                    <tr>
                        <td style="text-align: left; width: 250px">
                            Tiêu đề
                        </td>
                        <td style="text-align: left">
                            <asp:TextBox ID="txtTitle" Width="450px" runat="server"></asp:TextBox>
                        </td>
                    </tr>   
                    <tr><td style="height:5px"></td></tr>   
                    <tr>
                        <td style="text-align: left; width: 250px">                            
                        </td>
                        <td style="text-align: left">                            
                            <asp:Button ID="Button5" runat="server" Text="Tìm kiếm" Width="100px" CssClass="button" OnClick="btCapNhat_Click" />
                        </td>
                    </tr>                   
                </table>
            </td>
        </tr>
        <tr>
            <td>
                <table cellpadding="0" cellspacing="0" style="width: 100%">
                    <tr>
                        <td align="center" style="width: 50%; text-align: left">
                            <asp:Button ID="Button1" runat="server" CssClass="button" Text="Thêm mới " OnClick="btThemMoi_Click"
                                Width="80px" />
                        </td>
                        <td align="right" style="width: 50%; text-align: right">
                            <asp:Button ID="Button2" runat="server" CssClass="button" Text="Xóa" OnClick="btDelete_Click"
                                Width="50px" OnClientClick="javascript:{return confirm('Bạn có muốn xóa News được chọn?');}" />
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
                <asp:GridView ID="grvView" runat="server" AutoGenerateColumns="False" Width="100%"
                    DataKeyNames="ID" BackColor="White" BorderColor="#16538C" BorderStyle="Solid"
                    BorderWidth="1px" CellPadding="3" GridLines="Both" AllowPaging="True" PageSize="50"
                    AllowSorting="True" OnRowDataBound="grvView_RowDataBound" OnPageIndexChanging="grvView_PageIndexChanging">
                    <RowStyle BackColor="White" ForeColor="Black" />
                    <EditRowStyle BackColor="#999999" />
                    <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                    <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Right" />
                    <HeaderStyle BackColor="#2360A4" Font-Bold="True" ForeColor="White" />
                    <AlternatingRowStyle BackColor="#DEEAF3" ForeColor="Black" />
                    <Columns>
                        <asp:TemplateField ItemStyle-Width="5%" HeaderText="STT" HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-HorizontalAlign="Center"></asp:TemplateField>
                        <asp:BoundField DataField="CategoryName" HeaderText="Chủ đề" ItemStyle-HorizontalAlign="Left"
                            HeaderStyle-HorizontalAlign="Center" />                        
                        <asp:BoundField DataField="Title" HeaderText="Tiêu đề" ItemStyle-HorizontalAlign="Left"
                            HeaderStyle-HorizontalAlign="Center" />
                        <asp:BoundField DataField="ImagePath" HeaderText="Ảnh" ItemStyle-HorizontalAlign="Left"
                            HeaderStyle-HorizontalAlign="Center" />
                        <asp:BoundField DataField="LikeCount" HeaderText="Số like" ItemStyle-HorizontalAlign="Center"
                            HeaderStyle-HorizontalAlign="Center" />
                        <asp:BoundField DataField="LUser" HeaderText="Người sửa cuối" ItemStyle-HorizontalAlign="Left"
                            HeaderStyle-HorizontalAlign="Center" />
                        <asp:BoundField DataField="LDate" HeaderText="Ngày sửa cuối" ItemStyle-HorizontalAlign="Center"
                            DataFormatString="{0:dd/MM/yyyy}" HeaderStyle-HorizontalAlign="Center" />
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
                <table cellpadding="0" cellspacing="0" style="width: 100%">
                    <tr>
                        <td align="center" style="width: 50%; text-align: left">
                            <asp:Button ID="Button3" runat="server" CssClass="button" Text="Thêm mới " OnClick="btThemMoi_Click"
                                Width="80px" />
                        </td>
                        <td align="right" style="width: 50%; text-align: right">
                            <asp:Button ID="Button4" runat="server" CssClass="button" Text="Xóa" OnClick="btDelete_Click"
                                Width="50px" OnClientClick="javascript:{return confirm('Bạn có muốn xóa News được chọn?');}" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td style="height: 10px">
            </td>
        </tr>
    </table>
</asp:Content>