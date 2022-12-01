<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="BieuDoKNCuaPhongBanTheoThoiGian.ascx.cs" Inherits="Website.Views.Dashboards.BieuDoKNCuaPhongBanTheoThoiGian" %>

<script type="text/javascript">
    function pointTiepNhanClick(slTiepNhan, date) {
        alert(slTiepNhan, date);
    }
    function pointDongClick(slDong, date) {
        alert(slDong, date);
    }
</script>
<asp:Label CssClass="pn-msg" ID="lblMessage" runat="server" Text="Hiện chưa có số lượng thống kê"></asp:Label>

<asp:Chart ID="chartKhieuNai" runat="server" Width="500">
    <Legends>
        <asp:Legend LegendStyle="Row" IsTextAutoFit="False" DockedToChartArea="ChartArea1" Docking="Bottom" IsDockedInsideChartArea="False" Name="Default" BackColor="Transparent" Alignment="Center"></asp:Legend>
    </Legends>
    <Series>
        <asp:Series Name="SeriesKhieuNaiTiepNhan" ChartType="Line" MarkerStyle="Circle"
            ShadowColor="Black" BorderColor="180, 26, 59, 105" Color="220, 65, 140, 240"
            LegendText="Số lượng khiếu nại tiếp nhận">
        </asp:Series>
        <asp:Series Name="SeriesKhieuNaiDong" ChartType="Line" MarkerStyle="Diamond"
            ShadowColor="Black" BorderColor="180, 26, 59, 105" Color="220, 224, 64, 10"
            LegendText="Số lượng khiếu nại đã đóng">
        </asp:Series>
    </Series>
    <ChartAreas>
        <asp:ChartArea Name="ChartArea1">
        </asp:ChartArea>
    </ChartAreas>
</asp:Chart>
