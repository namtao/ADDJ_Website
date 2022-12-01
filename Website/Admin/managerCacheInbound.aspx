<%@ Page Title="" Language="C#" MasterPageFile="~/Master_Default.master" AutoEventWireup="true" CodeBehind="ManagerCacheInbound.aspx.cs" Inherits="Website.admin.managerCacheInbound" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder_Main" runat="server">
    <!-- begin panel nav boot -->
    <div class="nav_btn_bootstrap">
        <ul>
            <li><a href="javascript:history.back()" class="btn btn-primary"><span class="glyphicon glyphicon-circle-arrow-left" aria-hidden="true"></span>Quay về</a></li>
        </ul>
    </div>
    <!-- end panel nav boot -->
    <div class="p8">
        <!-- begin panel boot -->
        <div class="panel panel-default">
            <div class="panel-heading"><span style="font-size: 12pt">Lọc tìm kiếm</span></div>
            <div class="panel-body" style="border: none">
                <!-- begin body boot -->
                <table border="0" style="width: 100%">
                    <tr>
                        <td style="width: 100px; text-align: right; padding-right: 5px;">Key</td>
                        <td style="width: 120px;">
                            <asp:TextBox ID="txtKey" runat="server"></asp:TextBox>
                        </td>
                        <td style="text-align: left">
                            <asp:Button ID="btGetKey" OnClick="btGetKey_Click" runat="server" Text="Get Key" />
                            <asp:Button ID="btGetListKey" OnClick="btGetListKey_Click" runat="server" Text="Get List Key" />
                            <asp:Button ID="btRemoveKey" OnClick="btRemoveKey_Click" runat="server" Text="Remove Key" />
                            <asp:Button ID="btCountKey" OnClick="btCountKey_Click" runat="server" Text="Count Key" />

                            <asp:Button ID="btRemoveAllKey" OnClick="btRemoveAllKey_Click" runat="server" Text="Remove All Key" />
                        </td>
                    </tr>

                    <tr>
                        <td colspan="3">
                            <asp:TextBox ID="txtResult" TextMode="MultiLine" Width="100%" Rows="15" ReadOnly="true" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                </table>
                <!-- end body boot -->
            </div>
        </div>
        <!-- end panel boot -->
    </div>
</asp:Content>
