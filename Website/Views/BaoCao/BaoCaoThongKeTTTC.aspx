<%@ Page Title="" Language="C#" MasterPageFile="~/AdminNotAJAX.Master" AutoEventWireup="true" CodeBehind="BaoCaoThongKeTTTC.aspx.cs" Inherits="Website.Views.BaoCao.BaoCaoThongKeTTTC" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder_Text" runat="server">    

 <link href="/CSS/autocomplete.css" rel="stylesheet" type="text/css" />
    <script src="/JS/jquery.autocomplete.js" type="text/javascript"></script>
    <script src="/JS/plugin/date.js" type="text/javascript"></script>  
    
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
                <center>
                    <h4>
                        BÁO CÁO KHIẾU NẠI
                    </h4>
                </center>

                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>

                    <%--style="border:1pt solid #ccc; text-align:center; width:150px; height:70px"--%>
                    <div style='overflow:hidden; height:100%'>
                    <div class='col_left_pagebaocao'>                        
                        <div class='menuhaitac'>
                        <ul>                                
                                <li>
                                    <h4>Báo cáo trung tâm tính cước</h4>
                                    <ul>
                                        <li>
                                            <asp:LinkButton ID="lbReportTTTC_TongHopPAKN" runat="server" CommandArgument="bcTTTC_TongHopPAKN" onclick="lbReport_Click">Báo cáo tổng hợp PAKN</asp:LinkButton>                                
                                        </li>
                                        <li>
                                            <asp:LinkButton ID="lbReportTTTC_TongHopPAKNTheoPhongBan" runat="server" CommandArgument="bcTTTC_TongHopPAKNTheoPhongBan" onclick="lbReport_Click">Báo cáo tổng hợp PAKN theo phòng ban</asp:LinkButton>                                
                                        </li>
                                        <li>
                                            <asp:LinkButton ID="lbReportTTTC_TongHopPAKNTheoNguoiDung" runat="server" CommandArgument="bcTTTC_TongHopPAKNTheoNguoiDung" onclick="lbReport_Click">Báo cáo tổng hợp PAKN theo người dùng</asp:LinkButton>                                
                                        </li>
                                        <li>
                                            <asp:LinkButton ID="lbReportTTTC_ChiTietPAKNTheoNguoiDung" runat="server" CommandArgument="bcTTTC_ChiTietPAKNTheoNguoiDung" onclick="lbReport_Click">Báo cáo chi tiết PAKN theo người dùng</asp:LinkButton>                                
                                        </li>
                                    </ul>
                                </li>                               
                            </ul>                             
                        </div>
                    </div>
                    <div class='col_right_pagebaocao'>                           
                        <table class="tb1" style="display:none">                                        
                            <tr>
                                <td>
                                    Chọn báo cáo
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlReportType" runat="server" style="width: 400px" 
                                        onselectedindexchanged="ddlReportType_SelectedIndexChanged" AutoPostBack="true">
                                        <asp:ListItem Value="-1" Selected="True">--Chọn báo cáo--</asp:ListItem>                                        
                                        <asp:ListItem value="bcTTTC_TongHopPAKN">Báo cáo tổng hợp PAKN TTTC</asp:ListItem>
                                        <asp:ListItem value="bcTTTC_TongHopPAKNTheoPhongBan">Báo cáo tổng hợp PAKN theo phòng ban TTTC</asp:ListItem>
                                        <asp:ListItem value="bcTTTC_TongHopPAKNTheoNguoiDung">Báo cáo tổng hợp PAKN theo người dùng TTTC</asp:ListItem>        
                                        <asp:ListItem value="bcTTTC_ChiTietPAKNTheoNguoiDung">Báo cáo chi tiết PAKN theo người dùng TTTC</asp:ListItem>                                                                                                                 
                                    </asp:DropDownList>                                                                                                        
                                </td>
                            </tr>
                        </table>

                        <!-- pnReportType1 -->
                        <asp:Panel ID="pnReportType1" runat="server" Visible='false'>
                            <table class="tb2" width="100%">                       
                                <tr>
                                    <th colspan="4">
                                        <asp:Label ID="lblTittleReportType1" runat="server" ></asp:Label>                                        
                                    </th>
                                </tr>
                                <tr>                                    
                                    <td >
                                        <asp:Label ID="lblPhongBan_ReportType1" runat="server">Phòng</asp:Label>
                                    </td>
                                    <td >
                                        <asp:DropDownList ID="ddlPhongBan_ReportType1" runat="server" DataTextField="Name" DataValueField="Id">
                                        </asp:DropDownList>                                                                                                                           
                                    </td>                                    
                                </tr>
                                <tr>
                                    <td>
                                        Từ ngày
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtFromDate" runat="server" CssClass="fromdate"></asp:TextBox>
                                        <%--<input type="text" value="" id="txtFromDate" class="fromDate" />--%>
                                    </td>
                                    <td>
                                        đến ngày
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtToDate" runat="server" CssClass="todate"></asp:TextBox>
                                        <%--<input type="text" value="" id="txtToDate" class="toDate" />--%>
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
                                        <%--<a id="btnReport" class="poplight btn" href="#"><span>Báo cáo</span></a> --%>
                                        <%--<a href="#1" class="btn"><span>Hủy</span></a>--%>
                                    </th>
                                </tr>
                            </table>
                        </asp:Panel>

                        <!-- pnReportType 3 
                            
                        -->
                        <asp:Panel ID="pnReportType3" runat="server" Visible="false">
                            <table class="tb2" width="100%">                       
                                <tr>
                                    <th colspan="4">                                
                                        <asp:Label ID="lblTitleReportType3" runat="server" ></asp:Label>                          
                                    </th>
                                </tr>                                
                                <tr>
                                    <td>
                                        Từ ngày
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtFromDate_ReportType3" runat="server" CssClass="fromdate"></asp:TextBox>
                                        <%--<input type="text" runat="server" value="" id="txtFromDate_BaoCaoTongHopTheoKhieuNai" />--%>
                                    </td>
                                    <td>
                                        đến ngày
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtToDate_ReportType3" runat="server" CssClass="todate"></asp:TextBox>
                                        <%--<input type="text" runat="server" value="" id="txtToDate_BaoCaoTongHopTheoKhieuNai" />--%>
                                    </td>
                                </tr>
                                <tr valign="top">
                                    <td >
                                        Phòng
                                    </td>
                                    <td >                                        
                                        <asp:DropDownList ID="ddlPhongBan_ReportType3" runat="server" 
                                            DataTextField ="Name" DataValueField="Id" AutoPostBack="true" 
                                            onselectedindexchanged="ddlPhongBan_ReportType3_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>   
                                    <td>
                                        <asp:Label ID="Label2" runat="server" Text="Người dùng"></asp:Label>
                                    </td>
                                    <td>                                        
                                        <asp:CheckBoxList ID="cblNguoiDung" runat="server" DataTextField="TenDayDu" DataValueField="TenTruyCap">
                                        </asp:CheckBoxList>
                                    </td>
                                    <%--<td >
                                        Loại khiếu nại
                                    </td>
                                    <td>                               
                                        <asp:TreeView ID="TreeView1" runat="server" ShowCheckBoxes="All">
                                        </asp:TreeView>
                                    </td>        --%>                    
                                </tr>                        
                                <tr>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td colspan="3">
                                         <asp:RadioButtonList ID="rblLoaiBaoCao_ReportType3" runat="server" CssClass="surrounded" RepeatDirection="Horizontal">
                                            <asp:ListItem Value="html" Selected="True">HTML</asp:ListItem>
                                            <asp:ListItem Value="excel">EXCEL</asp:ListItem>
                                        </asp:RadioButtonList>
                                    </td>
                                </tr>
                                <tr>
                                    <th colspan="4">
                                        <asp:LinkButton ID="btnReport_ReportType3" runat="server" 
                                            CssClass="poplight btn" 
                                            onclick="btnReport_ReportType3_Click">Báo cáo</asp:LinkButton>                                
                                        <%--<a id="btnReport_TongHopTheoKhieuNai" class="poplight btn" href="#"><span>Báo cáo</span></a>--%>
                                         <%--<a href="#1" class="btn"><span>Hủy</span></a>--%>
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
