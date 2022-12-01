<%@ Page Title="" Language="C#" MasterPageFile="~/AdminNotAJAX.Master" AutoEventWireup="true" CodeBehind="BaoCaoThongKeDKT.aspx.cs" Inherits="Website.Views.BaoCao.BaoCaoThongKeDKT" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder_Text" runat="server">    

<style type="text/css">
    .treeView td
    {
        vertical-align:top;
    }
</style>

<link href="/CSS/autocomplete.css" rel="stylesheet" type="text/css" />
    <script src="/JS/jquery.autocomplete.js" type="text/javascript"></script>
    <script src="/JS/plugin/date.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            var tvLoaiKhieuNai_ReportType2 = <%=tvLoaiKhieuNai_ReportType2.ClientID %>;
            $("div[id $= " + tvLoaiKhieuNai_ReportType2 + "] input[type=checkbox]").click(function () {
                if ($('#ContentPlaceHolder_Main_ContentPlaceHolder_Text_chkAutoCheckChildren').attr('checked')) {                
                    $(this).closest("table").next("div").find("input[type=checkbox]").attr("checked", this.checked);
                }
                
            });

            var tvLoaiKhieuNai_ReportType4 = <%=tvLoaiKhieuNai_ReportType4.ClientID %>;
            $("div[id $= " + tvLoaiKhieuNai_ReportType4 + "] input[type=checkbox]").click(function () {
                $(this).closest("table").next("div").find("input[type=checkbox]").attr("checked", this.checked);
            });

            jsBaoCaoThongKeOnLoad('');
        });  
    </script>
    
    <div id="rightarea">
        <div id="baocaokhieunai">
           <%-- <div class="head">
                <h3>
                    Báo cáo khiếu nại
                </h3>
            </div>--%>
            <div class="head">
                &nbsp;
            </div>
            <div class="body">
                <%--<center>
                    <h4>
                        BÁO CÁO KHIẾU NẠI
                    </h4>
                </center>--%>

                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>

                    <%--style="border:1pt solid #ccc; text-align:center; width:150px; height:70px"--%>
                    <div style='overflow:hidden; height:100%'>
                    <div class='col_left_pagebaocao'>                        
                        <div class='menuhaitac'>
                        <ul>
                               <li>
                                    <h4>Báo cáo tổ GQKN</h4>
                                    <ul>
                                        <li><asp:LinkButton ID="lbReport11" runat="server" CommandArgument="bc11" 
                                                onclick="lbReport_Click">Báo cáo chi tiết giảm trừ cước dịch vụ trả trước</asp:LinkButton>                                
                                        </li>
                                        <li><asp:LinkButton ID="lbReport81" runat="server" CommandArgument="bc81" 
                                                onclick="lbReport_Click">Báo cáo chi tiết giảm trừ cước dịch vụ trả sau</asp:LinkButton>                                
                                        </li>
                                        <li>
                                            <asp:LinkButton ID="lbReport21" runat="server" CommandArgument="bc21" onclick="lbReport_Click">Báo cáo tổng hợp giảm trừ cước DV GTGT theo CP</asp:LinkButton>                                
                                        </li>
                                        <li>
                                            <asp:LinkButton ID="lbReport41" runat="server" CommandArgument="bc41" onclick="lbReport_Click">Báo cáo chi tiết PPS</asp:LinkButton>                                
                                        </li>
                                        <li>
                                            <asp:LinkButton ID="lbReport51" runat="server" CommandArgument="bc51" onclick="lbReport_Click">Báo cáo chi tiết POST</asp:LinkButton>                                
                                        </li>
                                        <li>
                                            <asp:LinkButton ID="lbReport61" runat="server" CommandArgument="bc61" onclick="lbReport_Click">Báo cáo theo loại khiếu nại</asp:LinkButton>                                
                                        </li>
                                    </ul>
                                </li>
                                <li>
                                    <h4>Báo cáo tổ XLNV</h4>
                                    <ul>
                                        <li>
                                            <asp:LinkButton ID="lbReport31" runat="server" CommandArgument="bc31" onclick="lbReport_Click">Báo cáo tổng hợp theo loại khiếu nại</asp:LinkButton>                                
                                        </li>                                        
                                        <li>
                                            <asp:LinkButton ID="lbReport_XLNV_BaoCaoKhoiLuongCongViec" runat="server" CommandArgument="bc_XLNV_BaoCaoKhoiLuongCongViec" onclick="lbReport_Click">Báo cáo khối lượng công việc</asp:LinkButton>                                
                                        </li>
                                   </ul>
                                </li>
                                <li>
                                    <h4>Báo cáo tổ KS</h4>
                                    <ul>
                                         <li>
                                            <asp:LinkButton ID="lbReport_KS_BaoCaoTongHopKN" runat="server" CommandArgument="bc_KS_BaoCaoTongHopKN" onclick="lbReport_Click">Báo cáo tổng hợp theo loại khiếu nại</asp:LinkButton>                                
                                        </li>   
                                    </ul>
                                </li>  
                                <li>
                                    <h4>Báo cáo tổ OB</h4>
                                    <ul>
                                         <li>
                                            <asp:LinkButton ID="lbReport_OB_BaoCaoTongHopKN" runat="server" CommandArgument="bc_OB_BaoCaoTongHopKN" onclick="lbReport_Click">Báo cáo tổng hợp theo loại khiếu nại</asp:LinkButton>                                
                                        </li>   
                                    </ul>
                                </li>      
                                <li>
                                    <h4>Báo cáo tổ trưởng tổ KTV </h4>
                                    <ul>
                                         <li>
                                            <asp:LinkButton ID="lbReport_TTKTV_BaoCaoSoLuongKNPhanHoiVeKTV" runat="server" CommandArgument="bc_TTKTV_BaoCaoSoLuongKNPhanHoiVeKTV" onclick="lbReportUC_Click">Báo cáo số lượng KN bị phản hồi về của KTV</asp:LinkButton>                                
                                        </li>   
                                        <li>
                                            <asp:LinkButton ID="lbReport_TTKTV_BaoCaoSoLuongKNQuaHanCuaKTV" runat="server" CommandArgument="bc_TTKTV_BaoCaoSoLuongKNQuaHanCuaKTV" onclick="lbReportUC_Click">Báo cáo khiếu nại quá hạn của KTV</asp:LinkButton>                                
                                        </li>   
                                    </ul>
                                </li>                             
                                <li>
                                    <h4>Báo cáo dạng biểu đồ</h4>
                                    <ul>
                                        <li>
                                            <asp:LinkButton ID="lbReportBieuDo_TheoLinhVucCon" runat="server" CommandArgument="bcBieuDo_TheoLinhVucCon" onclick="lbReport_Click">Theo lĩnh vực con </asp:LinkButton>                                
                                        </li>                                        
                                    </ul>
                                </li>
                            </ul>                             
                        </div>
                    </div>
                    <div class='col_right_pagebaocao'>                           
                       <asp:Label runat="server" ID="lblReportType" Visible="false"></asp:Label>                          

                       <%-- <table class="tb1" style="display:none">                                        
                            <tr>
                                <td>
                                    Chọn báo cáo
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlReportType" runat="server" style="width: 400px" 
                                        onselectedindexchanged="ddlReportType_SelectedIndexChanged" AutoPostBack="true">
                                        <asp:ListItem Value="-1" Selected="True">--Chọn báo cáo--</asp:ListItem>
                                        <asp:ListItem value="bc11">Báo cáo chi tiết giảm trừ cước dịch vụ trả trước</asp:ListItem>
                                        <asp:ListItem value="bc81">Báo cáo chi tiết giảm trừ cước dịch vụ trả sau</asp:ListItem>
                                        <asp:ListItem value="bc21">Báo cáo tổng hợp giảm trừ cước DV GTGT theo CP</asp:ListItem>
                                        <asp:ListItem value="bc31">Báo cáo tổng hợp theo loại khiếu nại</asp:ListItem>
                                        <asp:ListItem value="bc71">Báo cáo chi tiết khiếu nại</asp:ListItem>
                                        <asp:ListItem value="bc41">Báo cáo chi tiết PPS</asp:ListItem>
                                        <asp:ListItem value="bc51">Báo cáo chi tiết POST</asp:ListItem>
                                        <asp:ListItem value="bc61">Báo cáo theo loại khiếu nại</asp:ListItem>                                           
                                        <asp:ListItem value="bcBieuDo_TheoLinhVucCon">Báo cáo dạng biểu đồ theo lĩnh vực con</asp:ListItem>                                                                            
                                    </asp:DropDownList>                                                                                                        
                                </td>
                            </tr>
                        </table>--%>

                        <div style="text-align:center">
                            <asp:Panel ID="pnlContainer" runat="server">
                            </asp:Panel>
                        </div>
                                                
                        <!-- pnReportType1 -->
                        <asp:Panel ID="pnReportType1" runat="server" Visible='false'>
                            <table class="tb2" width="100%">                       
                                <tr>
                                    <th colspan="4">
                                        <asp:Label ID="lblTittle" runat="server" ></asp:Label>                                        
                                    </th>
                                </tr>
                                <tr>
                                    <td >
                                        <asp:Label ID="lblKhuVuc_ReportType1" runat="server">Khu vực</asp:Label>
                                    </td>
                                    <td>                                       
                                        <asp:DropDownList ID="ddlKhuVuc_ReportType1" runat="server" 
                                            DataTextField ="TenDoiTac" DataValueField="Id"
                                            AutoPostBack="true" 
                                            onselectedindexchanged="ddlKhuVuc_ReportType1_SelectedIndexChanged">                                                                                       
                                        </asp:DropDownList>                                                                         
                                    </td>  
                                    <td >
                                        <asp:Label ID="lblPhongBan_DoiTac_ReportType1" runat="server">Phòng</asp:Label>
                                    </td>
                                    <td >
                                        <asp:DropDownList ID="ddlPhongBan_ReportType1" runat="server" DataTextField="Name" DataValueField="Id">
                                        </asp:DropDownList>     
                                        
                                        <asp:DropDownList ID="ddlDoiTac_ReportType1" runat="server" Visible="false"
                                             DataTextField ="TenDoiTac" DataValueField="Id">
                                        </asp:DropDownList>                                          
                                    </td>                                    
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
                                    <td >
                                        Loại khiếu nại
                                    </td>
                                    <td colspan="3">
                                        <asp:DropDownList ID="ddlLoaiKhieuNai" runat="server" DataTextField="Name" 
                                            DataValueField="Id" AutoPostBack="true" 
                                            onselectedindexchanged="ddlLoaiKhieuNai_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>                            
                                </tr>
                                <tr>
                                    <td>
                                        Lĩnh vực chung
                                    </td>
                                    <td >
                                        <asp:DropDownList ID="ddlLinhVucChung" runat="server" DataTextField="Name" 
                                            DataValueField="Id" AutoPostBack="true" 
                                            onselectedindexchanged="ddlLinhVucChung_SelectedIndexChanged" Width="250px">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        Lĩnh vực con
                                    </td>
                                    <td >
                                        <asp:DropDownList ID="ddlLinhVucCon" runat="server" DataTextField="Name" DataValueField="Id" Width="250px">
                                        </asp:DropDownList>
                                    </td>
                                </tr>                                                           
                                <tr>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td colspan="3">                               
                                        <asp:RadioButtonList ID="rblLoaiBaoCao_ReportType1" runat="server" CssClass="surrounded" RepeatDirection="Horizontal">
                                            <asp:ListItem Value="html" Selected="True">HTML</asp:ListItem>
                                            <asp:ListItem Value="excel">EXCEL</asp:ListItem>
                                        </asp:RadioButtonList>
                                    </td>
                                </tr>
                                <tr>
                                    <th colspan="4">
                                         <asp:LinkButton ID="lbReport_ReportType1" runat="server" 
                                            CssClass="poplight btn" onclick="lbReport_ReportType1_Click" >Báo cáo</asp:LinkButton>                                          
                                    </th>
                                </tr>
                            </table>
                        </asp:Panel>

                        <!-- pnReportType2 -->
                        <asp:Panel ID="pnReportType2" runat="server" Visible="false">
                            <table class="tb2" width="100%">                       
                                <tr>
                                    <th colspan="4">                                
                                        <asp:Label ID="lblTitleReportType2" runat="server" ></asp:Label>                          
                                    </th>
                                </tr>
                                <tr>                                    
                                    <td >
                                        Khu vực
                                    </td>
                                    <td >                                                                                
                                        <asp:DropDownList ID="ddlKhuVuc_ReportType2" runat="server" 
                                            DataTextField ="TenDoiTac" DataValueField="Id"                                     
                                            onselectedindexchanged="ddlKhuVuc_ReportType2_SelectedIndexChanged" 
                                            AutoPostBack="True">
                                        </asp:DropDownList>

                                        <asp:Label ID="lblKhuVucId_ReportType2" runat="server" Visible="false"></asp:Label>
                                        <asp:Label ID="lblTenKhuVuc_ReportType2" runat="server" Visible="false"></asp:Label>
                                    </td>        
                                    <td >
                                        Phòng
                                    </td>
                                    <td >                                        
                                        <asp:DropDownList ID="ddlPhongBan_ReportType2" runat="server" 
                                            DataTextField ="Name" DataValueField="Id">
                                        </asp:DropDownList>
                                    </td>                           
                                </tr>
                                <tr>
                                    <td>
                                        Từ ngày
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtFromDate_BaoCaoTongHopTheoKhieuNai" runat="server" CssClass="fromdate"></asp:TextBox>                                        
                                    </td>
                                    <td>
                                        đến ngày
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtToDate_BaoCaoTongHopTheoKhieuNai" runat="server" CssClass="todate"></asp:TextBox>                                       
                                    </td>
                                </tr>
                                <tr valign="top">
                                    <td>
                                        <asp:Label ID="lblDoiTac" runat="server" Text="Đối tác"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:CheckBoxList ID="cblDoiTac" runat="server" DataTextField="TenDoiTac" DataValueField="Id">
                                        </asp:CheckBoxList>                                       
                                    </td>
                                    <td >
                                        Loại khiếu nại
                                    </td>
                                    <td>                               
                                        <span style="padding-left:12px"><asp:CheckBox ID="chkAutoCheckChildren" runat="server" Text="Tự động check/uncheck các mục con" /></span>
                                        <asp:TreeView ID="tvLoaiKhieuNai_ReportType2" runat="server" CssClass="treeView" 
                                            ShowCheckBoxes="All" NodeWrap="True" Width="300px">
                                        </asp:TreeView>
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
                                        <asp:LinkButton ID="btnReport_ReportType2" runat="server" 
                                            CssClass="poplight btn" onclick="btnReport_ReportType2_Click">Báo cáo</asp:LinkButton>                                                                        
                                    </th>
                                </tr>
                            </table>

                        </asp:Panel>                                                                  
                        
                        <!-- pnReportType4
                            Báo cáo dạng biểu đồ
                        -->
                        <asp:Panel ID="pnReportType4" runat="server" Visible="false">
                            <table class="tb2" width="100%">                       
                                <tr>
                                    <th colspan="4">                                
                                        <asp:Label ID="lblTitleReportType4" runat="server" ></asp:Label>                          
                                    </th>
                                </tr>                              
                                <tr>
                                    <td>
                                        Từ ngày
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtFromDate1_ReportType4" runat="server" CssClass="fromdate"></asp:TextBox>                                        
                                    </td>
                                    <td>
                                        đến ngày
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtToDate1_ReportType4" runat="server" CssClass="todate"></asp:TextBox>                                        
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Từ ngày
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtFromDate2_ReportType4" runat="server" CssClass="fromdate"></asp:TextBox>                                        
                                    </td>
                                    <td>
                                        đến ngày
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtToDate2_ReportType4" runat="server" CssClass="todate"></asp:TextBox>                                        
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="4">
                                        <asp:RadioButtonList ID="rblReportType_ReportType4" runat="server" RepeatDirection="Horizontal">
                                            <asp:ListItem Value="1">Thống kê theo loại khiếu nại</asp:ListItem>
                                            <asp:ListItem Value="2">Thống kê theo lĩnh vực chung</asp:ListItem>
                                            <asp:ListItem Value="3" Selected="True">Thống kê theo lĩnh vực con</asp:ListItem>
                                        </asp:RadioButtonList>
                                    </td>
                                </tr>
                                <tr valign="top">                                    
                                    <td >
                                        Loại khiếu nại
                                    </td>
                                    <td colspan="3">                               
                                        <asp:TreeView ID="tvLoaiKhieuNai_ReportType4" runat="server" ShowCheckBoxes="All">
                                        </asp:TreeView>
                                    </td>                            
                                </tr>                        
                                <tr>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td colspan="3">
                                         <asp:RadioButtonList ID="rblLoaiBaoCao_ReportType4" runat="server" CssClass="surrounded" RepeatDirection="Horizontal">
                                            <asp:ListItem Value="html" Selected="True">HTML</asp:ListItem>
                                            <asp:ListItem Value="excel">EXCEL</asp:ListItem>
                                        </asp:RadioButtonList>
                                    </td>
                                </tr>
                                <tr>
                                    <th colspan="4">
                                        <asp:LinkButton ID="lbReport_ReportType4" runat="server" 
                                            CssClass="poplight btn" onclick="lbReport_ReportType4_Click">Báo cáo</asp:LinkButton>                                                                      
                                    </th>
                                </tr>
                            </table>

                        </asp:Panel>         
                    </div>
                    </div>
                    </ContentTemplate>
                </asp:UpdatePanel>                               
                        
            </div>   
        </div>    
    </div>
    <link href="/CSS/jquery.datepick.css" rel="stylesheet" type="text/css" />
    <script src="/JS/plugin/jquery.datepick.js" type="text/javascript"></script>
    <script src="/JS/plugin/jquery.datepick-vi.js" type="text/javascript"></script>
    <script src="/Views/BaoCao/baocao.js?v=1" type="text/javascript"></script>

    <style type="text/css" media="screen">
        .hiddenDiv
        {
            display: none;
        }
        .visibleDiv
        {
            display: block;
        }
    </style>


</asp:Content>
