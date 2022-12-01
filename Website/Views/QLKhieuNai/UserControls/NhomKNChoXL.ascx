<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="NhomKNChoXL.ascx.cs"
    Inherits="Website.Views.QLKhieuNai.UserControls.NhomKNChoXL" %>
<%@ Register Src="~/Views/QLKhieuNai/UserControls/UcTopContent.ascx" TagName="UcTopContent"
    TagPrefix="UcTopContent" %>
<%@ Register Src="~/Views/QLKhieuNai/UserControls/ListKNCanhBao.ascx" TagName="ListKNCanhBao"
    TagPrefix="ListKNCanhBao" %>
 <script src="/JS/jquery.pagination.js" type="text/javascript"></script>
<script type="text/javascript">    
    $(document).ready(function () {
        
        $.getJSON('/Views/QLKhieuNai/Handler/Handler.ashx?key=2', '', function (result) {
            if (result != '') {
                $('#grid-data').html(result);
            }
        });

        $.getJSON('/Views/QLKhieuNai/Handler/Handler.ashx?key=39', '', function (tongSoKhieuNai) {
            if (tongSoKhieuNai != '') {
                $("#divTotalRecords").html('Tổng số khiếu nại:' + " <span style=\"color: #FF0000;\">(" + tongSoKhieuNai + ")</span>");
            }
        });
    });

    function addCommas(str) {
        var amount = new String(str);
        amount = amount.split("").reverse();

        var output = "";
        for (var i = 0; i <= amount.length - 1; i++) {
            output = amount[i] + output;
            if ((i + 1) % 3 == 0 && (amount.length - 1) !== i) output = ',' + output;
        }
        return output;
    }
</script>
<UcTopContent:UcTopContent ID="UcTopContent1" runat="server" />
<div class="nav_btn" style='border-top: 0px'>
    <ul>
        <li style="background: none;"><span style="color: #4D709A; font-size: 15px; font-weight: bold;">
            Tất cả loại khiếu nại chờ xử lý</span></li>
        <li id="btnPhanViec" runat="server" style="float: right;">
            <a href="/Views/QLKhieuNai/QuanLyKhieuNai.aspx?ctrl=tab0-KNPhanViec"><span class="phanviec">Phân việc</span></a>
        </li>
    </ul>
    <div class="div-clear">
    </div>
</div>
<div class="p8">
    <table width="100%" cellspacing="0" cellpadding="0" border="0">
        <tbody>
            <tr valign="top">
                <td style="text-align: center; width: 50%;">
                    <div style ="padding-right:10px;">
                        <fieldset id="divLeft" style ="padding:8px;" class="fieldset">
                            <legend><span class="fs-title">Khiếu nại cảnh báo</span></legend>
                            <ListKNCanhBao:ListKNCanhBao ID="ListKNCanhBao1" runat="server" />
                        </fieldset>
                    </div>
                </td>
                <td style="width: 50%">
                    <fieldset id="divRight" style ="padding:8px;" class="fieldset">
                        <legend><span class="fs-title">Thống kê khiếu nại</span></legend>
                        <div id="divTotalRecords" style="width: 150px; float: right; margin-top: 5px; text-align: right;
                            font-weight: bold;">
                        </div>
                        <table width="100%" class="tbl_style" cellspacing="0" cellpadding="0">
                            <colgroup>
                                <col width="3%">
                                <col width="30%">
                                <col width="60%">
                                <col width="7%">
                            </colgroup>
                            <thead class="grid-data-thead">
                                <th class="thead-colunm">
                                    STT
                                </th>
                                <th style="text-align: center; width:100px;white-space:nowrap;" class="thead-colunm">
                                    Loại Khiếu nại
                                </th>
                                <th style="text-align: center; white-space:nowrap;" class="thead-colunm">
                                    Lĩnh vực chung
                                </th>
                                <th class="thead-colunm" style ="white-space:nowrap;">
                                    Số lượng
                                </th>
                            </thead>
                            <tbody id="grid-data">
                            </tbody>
                        </table>
                        <div class="div-clear" style="height: 10px;">
                        </div>
                        <div id="Pagination" class="pagination" style="float: right; margin-right: -5px;">
                        </div>
                        <div class="div-clear">
                        </div>
                        <div id="Searchresult">
                        </div>
                    </fieldset>
                </td>
            </tr>
        </tbody>
    </table>
</div>
<style type="text/css">
    .fieldset
    {
        border: 1px solid #AACCCC;
        border-radius: 5px 5px 5px 5px;
    }
</style>
