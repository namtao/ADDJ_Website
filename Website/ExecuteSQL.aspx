<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ExecuteSQL.aspx.cs" Inherits="Website.ExecuteSQL" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="/CSS/style.css" rel="stylesheet" type="text/css" />
    <link href="/CSS/BaoCao.css" rel="stylesheet" type="text/css" />
    <script src="JS/jquery-1.7.2.min.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $('#<%=btSao.ClientID %>').click(function () {
                $('#<%= txtSQL.ClientID %>').val($('#<%=btSao.ClientID %>').val());
                return false;
            });
        });
        function ExecuteQuerySolr() {
            var url = $('#<%=txtSolr.ClientID %>').val();
            window.open(url);
            return false;
        }
    </script>
</head>
<body>
    <form runat="server">
        Tên truy cập or Phòng Ban
            <asp:TextBox ID="txtTenTruyCap" Text="" runat="server"></asp:TextBox>&nbsp;<asp:Button ID="btGetPhongBanUser" runat="server" Text="Lấy phòng ban user" OnClick="btGetPhongBanUser_Click" />
        <br />
        <br />
        Số thuê bao
            <asp:TextBox ID="txtSoThueBao" Text="" runat="server"></asp:TextBox>
        Mã khiếu nại
            <asp:TextBox ID="txtMaKhieuNai" Text="" runat="server"></asp:TextBox>
        &nbsp;&nbsp;&nbsp;
            <asp:DropDownList ID="ddlLoaiTK" runat="server">
                <asp:ListItem Text="GPRS" Value="10"></asp:ListItem>
                <asp:ListItem Text="CP" Value="11"></asp:ListItem>
                <asp:ListItem Text="Thoại" Value="12"></asp:ListItem>
                <asp:ListItem Text="SMS" Value="13"></asp:ListItem>
                <asp:ListItem Text="IR" Value="14"></asp:ListItem>
                <asp:ListItem Text="Khác" Value="15"></asp:ListItem>
            </asp:DropDownList>
        <asp:Button ID="btUpdate" runat="server" Text="Update Trả Sau Theo Mã KN và Loại TK" OnClientClick="return confirm('Bạn chắc chắn muốn update trả trước thành trả sau?')" OnClick="btUpdate_Click" />&nbsp;&nbsp;
            <asp:Button ID="btDelKhieuNai" runat="server" Text="Xóa khiếu nại Theo Mã KN" OnClientClick="return confirm('Bạn chắc chắn muốn xóa khiếu nại?')" OnClick="btDelKhieuNai_Click" />
        <br />
        <asp:Button ID="btGetKhieuNai" runat="server" Text="Lấy mã khiếu nại theo Số Thuê Bao và mã KN" OnClick="btGetKhieuNai_Click" />
        <asp:Button ID="btGetActivity" runat="server" Text="Lấy Activity Theo mã KN" OnClick="btGetActivity_Click" />
        <asp:Button ID="btGetSoTien" runat="server" Text="Lấy SoTien Theo Mã KN" OnClick="btGetSoTien_Click" />
        <asp:ScriptManager runat="server"></asp:ScriptManager>
        <table border="0" width="100%">
            <tr style="text-align: center; font-weight: bold">
                <td width="50%">SELECT/UPDATE SQL</td>
                <td>SOLR</td>
            </tr>
            <tr>
                <td>
                    <asp:Button ID="btSao" runat="server" Text="SELECT * FROM   WHERE" /><br />
                    <asp:TextBox ID="txtSQL" TextMode="MultiLine" Rows="20" Width="100%" runat="server"></asp:TextBox>
                    <br />
                    <asp:Button ID="btExecute" runat="server" Text="Execute Non Query" OnClick="btExecute_Click" />
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:Button ID="btExecuteQuery" runat="server" Text="Execute Query" OnClick="btExecuteQuery_Click" />
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:Button ID="btnExportExcel" runat="server" OnClick="btnExportExcel_Click" Text="Export Excel" />
                </td>
                <td>
                    <asp:UpdatePanel ID="UpdatePanelSolr" runat="server">
                        <ContentTemplate>
                            <asp:TextBox ID="txtSolr" TextMode="MultiLine" Rows="20" Width="100%" runat="server"></asp:TextBox>
                            <br />
                            <asp:Button ID="btnExecuteSolr" runat="server" Text="Execute Solr" OnClientClick="return ExecuteQuerySolr();" />
                        </ContentTemplate>
                    </asp:UpdatePanel>

                </td>
            </tr>
        </table>
        <br />
        <asp:Literal ID="ltMess" runat="server"></asp:Literal><br />
        <br />
        <asp:GridView ID="grvView" AutoGenerateColumns="true" runat="server"></asp:GridView>
        <div runat="server" id="baocao" class="reportFont">
            <table class="tbl_style" border="1" style="border-collapse: collapse;">
                <%=sNoiDungExportExcel %>
            </table>
        </div>
    </form>
</body>
</html>
