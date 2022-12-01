<%@ Control Language="C#" ClassName="ThongBao" %>
<script src="Views/Dashboards/Scripts/Constants.js?ver=123"></script>
<script src="JS/Utility.js?ver=123"></script>
<script src="Views/Dashboards/Scripts/ThongBao.js?ver=123" type="text/javascript"></script>
<style type="text/css">
    #ul-thongbao { padding: 5px 0px; }
    .li-thongbao { background: transparent url("/images/icon6.png") no-repeat scroll 7px 8px; line-height: 20px; padding-left: 20px; }
</style>
<div style="border: #ddd 1pt solid;">
    <ul id="ul-thongbao">
    </ul>
</div>
<table class="nobor" style="width: 100%; border: 1px solid #ddd; border-top: none;">
    <tbody>
        <tr>
            <td>
                <div id="Pagination-ThongBao" class="pagination" style="float: right; margin-right: 5px;"></div>
            </td>
        </tr>
    </tbody>
</table>
<div id="DivEditWindow" title="Chi tiết thông báo" style="display: none; height: 300px; width: 400px;">
    <div style="border: #ddd 1pt solid;" id="div-tieude">
        <div class="wigget-title">Tiêu đề</div>
        <div id="div-tieude-content" style="padding: 5px;"></div>
    </div>
    <div id="div-noidung">
        <div class="wigget-title">Nội dung thông báo</div>
        <div id="div-noidung-content" style="padding: 5px;"></div>
    </div>
</div>

