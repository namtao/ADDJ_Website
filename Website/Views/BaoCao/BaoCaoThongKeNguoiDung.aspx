<%@ Page Language="C#" MasterPageFile="~/AdminNotAJAX.Master" AutoEventWireup="true" CodeBehind="BaoCaoThongKeNguoiDung.aspx.cs" Inherits="Website.Views.BaoCao.BaoCaoThongKeNguoiDung" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder_Text" runat="server">    

    <link href="/CSS/autocomplete.css" rel="stylesheet" type="text/css" />
    <script src="/JS/jquery.autocomplete.js" type="text/javascript"></script>
    <script src="/JS/plugin/date.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            var tvLoaiKhieuNai = <%=tvLoaiKhieuNai.ClientID %>
            $("div[id $= " + tvLoaiKhieuNai + "] input[type=checkbox]").click(function () {
                $(this).closest("table").next("div").find("input[type=checkbox]").attr("checked", this.checked);
            });

            jsBaoCaoThongKeOnLoad(tvLoaiKhieuNaiId);
        });
    </script>
    
    <div id="rightarea">
        <div id="baocaokhieunai">          
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
                                    <h4>
                                        Trung tâm phát triển dịch vụ
                                    </h4>
                                    <ul>
                                         <li><asp:LinkButton ID="lbReport_SoLieuPAKNDaXuLy" runat="server" CommandArgument="rptSoLieuPAKNDaXuLy" 
                                                onclick="lbReport_Click">Báo cáo số liệu PAKN đã xử lý</asp:LinkButton>                                
                                        </li>
                                        <li><asp:LinkButton ID="lbReport_TongHopSoLieuPAKNDangTonDong" runat="server" CommandArgument="rptTongHopSoLieuPAKNDangTonDong" 
                                                onclick="lbReport_Click">Báo cáo tổng hợp số liệu PAKN đang tồn đọng</asp:LinkButton>                                
                                        </li>
                                        <li>
                                            <asp:LinkButton ID="lbReport_TongHopSoLieuPAKNDaTiepNhan" runat="server" CommandArgument="rptTongHopSoLieuPAKNDaTiepNhan" onclick="lbReport_Click">Báo cáo tổng hợp số liệu PAKN đã tiếp nhận</asp:LinkButton>                                
                                        </li>
                                        <li>
                                            <asp:LinkButton ID="lbReport_ChiTietPAKNDaTiepNhan" runat="server" CommandArgument="rptChiTietPAKNDaTiepNhan" onclick="lbReport_Click">Báo cáo chi tiết PAKN đã tiếp nhận</asp:LinkButton>                                
                                        </li>
                                    </ul>
                                </li>                               
                            </ul>                             
                        </div>
                    </div>
                    <div class='col_right_pagebaocao'>                           
                        <asp:Label ID="lblReportType" runat="server" Visible="false"></asp:Label>

                        <!-- pnReportType1 -->
                        <asp:Panel ID="pnReportType1" runat="server" Visible="false">
                            <table class="tb2" width="100%">                       
                                <tr>
                                    <th colspan="4">
                                        <asp:Label ID="lblTittle" runat="server" ></asp:Label>                                        
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
                                <tr id="Tr1" runat="server" visible="false">
                                    <td >
                                        Người dùng
                                    </td>
                                    <td colspan="3">                               
                                        <asp:CheckBoxList ID="cblNguoiDung" runat="server">
                                        </asp:CheckBoxList>
                                    </td>     
                                </tr>                      
                                <tr id="Tr2" runat="server" visible="false">
                                    <td >
                                        Loại khiếu nại
                                    </td>
                                    <td colspan="3">                               
                                        <asp:TreeView ID="tvLoaiKhieuNai" runat="server" ShowCheckBoxes="All">
                                        </asp:TreeView>
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
                                         <asp:LinkButton ID="lbShowReport" runat="server" 
                                            CssClass="poplight btn" onclick="lbShowReport_Click">Báo cáo</asp:LinkButton>                                          
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
