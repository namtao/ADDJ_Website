<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ListKNSoTheoDoi.ascx.cs"
    Inherits="Website.Views.QLKhieuNai.UserControls.ListKNSoTheoDoi" %>

<div id="divScroll" style='overflow-x: scroll; height: auto;'>
    <table class="tbl_style" cellspacing="0" cellpadding="0" style='width: 2200px;'>
        <thead class="grid-data-thead">
           
            <th class="thead-colunm">
                STT
            </th>    
            <th style ="white-space:nowrap;" class="thead-colunm">
                Mã khiếu nại
            </th>       
            <th style ="white-space:nowrap;" class="thead-colunm">
                Ngày tiếp nhận
            </th>
            <th style ="white-space:nowrap;" class="thead-colunm">
                Người tiếp nhận
            </th>
            <th style ="white-space:nowrap;" class="thead-colunm">
                Số thuê bao
            </th>
            <th  style ="padding-left:5px;white-space:nowrap;" class="thead-colunm">
                Tên khách hàng
            </th>
            <th   style ="padding-left:5px;white-space:nowrap;" class="thead-colunm">
                ND khiếu nại
            </th>
            <th   style ="padding-left:5px;white-space:nowrap;"class="thead-colunm">
                Giấy tờ kèm theo
            </th>
            <th   style ="padding-left:5px;white-space:nowrap;" class="thead-colunm">
                Trình tự xử lý
            </th>
            <th  class="thead-colunm" style ="padding-left:5px;white-space:nowrap;">
                Chuyển bộ phận khác (nếu có)
            </th>
            <th style ="white-space:nowrap;" class="thead-colunm">
                Ngày hẹn trả lời
            </th>
            <th   style ="padding-left:5px;white-space:nowrap;" class="thead-colunm">
                Kết quả giải quyêt
            </th>
            <th   style ="padding-left:5px;white-space:nowrap;" class="thead-colunm">
                Ghi chú
            </th>
            
        </thead>        
        <tbody id="grid-data">
        </tbody>
    </table>
</div>


