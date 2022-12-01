<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="KNPhanHoi.ascx.cs" Inherits="Website.Views.QLKhieuNai.UserControls.KNPhanHoi" %>
<%@ Register Src="~/Views/QLKhieuNai/UserControls/UcTopMyKhieuNai.ascx" TagName="UcTopMyKhieuNai"
    TagPrefix="uc" %>
<%@ Register Src="UcFillterMyKhieuNai.ascx" TagName="UcFillterMyKhieuNai" TagPrefix="uc1" %>
<uc:UcTopMyKhieuNai ID="UcTopMyKhieuNai1" runat="server" />
<script src="/JS/jquery.atooltip.js" type="text/javascript"></script>
<link href="/CSS/jquery.datepick.css" rel="stylesheet" type="text/css" />
<script src="/JS/plugin/jquery.datepick.js" type="text/javascript"></script>
<script src="/JS/plugin/jquery.datepick-vi.js" type="text/javascript"></script>
<script type="text/javascript">

    $(document).ready(function () {

        keyTotal = 2;
        keyGetHTML = 2;
        keyExcel = 2;

        pageSize = $('#DropPageSize').val();
        fnSetSizeDiv();

    });
</script>

<asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
        <div class="nav_btn" style='border-top: 0px'>
            <ul>
                <li style="float: left;"><a href="javascript:history.back()"><span class="back">Quay
                    về</span></a></li>
            </ul>
            <div class="div-clear">
            </div>
        </div>
        <div class="p8">
            <table width="100%" cellspacing="0" cellpadding="0" border="0">
                <tbody>
                    <tr valign="top">
                    </tr>
                    <tr>
                        <td>
                            <uc1:UcFillterMyKhieuNai ID="UcFillterMyKhieuNai1" runat="server" />
                        </td>
                    </tr>
                    <tr><td>&nbsp;</td></tr>
                    <tr>
                        <td>
                            <div id="Pagination" class="pagination" style="float: right; margin-right: -5px;">
                            </div>
                            <div id="PageSize" class="pagination" style="float: right;">
                                <div class="selectstyle">
                                    <div class="bg" style="margin: -7px; margin-right: 10px; margin-left: 10px;">
                                        <select id="DropPageSize" onchange="javascript:fnDropPageSizeChange();" style="width: 60px;">
                                            <option value="10" selected="selected">10</option>
                                            <option value="20">20</option>
                                            <option value="50">50</option>
                                            <option value="100">100</option>
                                            <option value="500">500</option>
                                        </select>
                                    </div>
                                </div>
                            </div>
                            <div id="divTotalRecords" style="width: 150px; float: right; margin-top: 5px; text-align: right;">
                            </div>
                        </td>
                    </tr>

                    <tr>
                        <td>

                            <div class="div-clear" style="height: 10px;">
                            </div>
                        </td>
                    </tr>

                </tbody>
            </table>
            <table class="nobor">
                <tr>
                    <td>
                        <div id="divScroll" style="height: 370px; width: 100%;">
                            <table class="flex_KNChoXuLy" style="display: none"></table>
                        </div>
                    </td>
                </tr>
            </table>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
