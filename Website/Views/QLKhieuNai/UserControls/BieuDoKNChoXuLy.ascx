<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="BieuDoKNChoXuLy.ascx.cs" Inherits="Website.Views.QLKhieuNai.UserControls.BieuDoKNChoXuLy" %>
<%@ Register Src="~/Views/QLKhieuNai/UserControls/UcTopContent.ascx" TagName="UcTopContent"
    TagPrefix="UcTopContent" %>

<UcTopContent:UcTopContent ID="UcTopContent1" runat="server" />
<div class="nav_btn" style='border-top: 0px'>
    <ul>
        <li style="background: none;"><span style="color: #4D709A; font-size: 15px; font-weight: bold;">
            Biểu đồ khiếu nại chờ xử lý</span></li>
        <li id="btnBack" style="float: right;">
            <%--<a href="javascript:fnPhanViec();"><span class="phanviec">Phân việc</span></a>--%>
        </li>
    </ul>
    <div class="div-clear">
    </div>
</div>
<div style="text-align:center">
        <asp:Chart ID="chartSoLuongKhieuNaiTiepNhan" runat="server" Width="1020" Height="500">
            <Titles> 
                <asp:Title Text="BIỂU ĐỒ SỐ LƯỢNG KHIẾU NẠI CHỜ XỬ LÝ" 
                    Font="Tahoma, 12pt, style=Bold"></asp:Title> 
            </Titles>         
            <Series>     
                <asp:Series Name="KNChoXuLy"></asp:Series>       
            </Series>
            <ChartAreas>
                <asp:ChartArea Name="ChartArea1">
                    <axisx linecolor="64, 64, 64, 64" IsLabelAutoFit="True" ArrowStyle="Triangle">
						<labelstyle IsStaggered="True" />
						<majorgrid linecolor="64, 64, 64, 64" />
					</axisx>
                </asp:ChartArea>
            </ChartAreas>
        </asp:Chart>
    </div>