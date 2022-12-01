﻿<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ListKNSelect.ascx.cs"
    Inherits="Website.Views.QLKhieuNai.UserControls.ListKNSelect" %>
<script language="javascript">
    function SelectAllCheckboxes(spanChk) {

        var oItem = spanChk.children;
        var theBox = (spanChk.type == "checkbox") ?
                            spanChk : spanChk.children.item[0];
        xState = theBox.checked;

        elm = $("#grid-data .checkbox-item");        
        for (i = 0; i < elm.length; i++) {
            if (elm[i].type == "checkbox" &&
                                elm[i].id != theBox.id) {
                if (elm[i].checked != xState) {

                    elm[i].click();

                }

            }
        }

    }
</script>
<div id="divScroll" style='overflow-x: scroll; height: auto;'>
    <table class="tbl_style" cellspacing="0" cellpadding="0" style='width: 2180px;'>
        <thead class="grid-data-thead">
            <th class="thead-colunm" style ="white-space:nowrap;padding-left:5px;padding-right:5px;">
              <%--  <a href="javascript:fnExportExcel();">
                    <img src="/images/icons/xuatexcel.png" />
                </a>--%>
                <input id="ckCheckAll" onclick="javascript:SelectAllCheckboxes(this);" type="checkbox" />
            </th>
            <th class="thead-colunm" style ="white-space:nowrap;padding-left:5px;padding-right:5px;">
                STT
            </th>
            <th class="thead-colunm" style ="white-space:nowrap;padding-left:5px;padding-right:5px;">
                Trạng thái
            </th>
            <th class="thead-colunm" style ="white-space:nowrap;padding-left:5px;padding-right:5px;width:100px;">
                Mã PA/KN
            </th>
            <th class="thead-colunm" style ="white-space:nowrap;padding-left:5px;padding-right:5px;width:100px;">
                Độ ưu tiên
            </th>
            <th class="thead-colunm" style ="white-space:nowrap;padding-left:5px;padding-right:5px;width:100px;">
                Số thuê bao
            </th>
            <th class="thead-colunm" style ="white-space:nowrap;padding-left:5px;padding-right:5px;">
                Loại khiếu nại
            </th>
            <th class="thead-colunm" style ="white-space:nowrap;padding-left:5px;padding-right:5px;">
                Lĩnh vực chung
            </th>
            <th class="thead-colunm" style ="white-space:nowrap;padding-left:5px;padding-right:5px;">
                Lĩnh vực con
            </th>           
            <th class="thead-colunm" style ="white-space:nowrap;padding-left:5px;padding-right:5px;">
                Người tiếp nhận
            </th>
            <th class="thead-colunm" style ="white-space:nowrap;padding-left:5px;padding-right:5px;">
                Người tiền xử lý
            </th> 
            <th class="thead-colunm" style ="white-space:nowrap;padding-left:5px;padding-right:5px;">
                Người được phản hồi 
            </th> 
            <th class="thead-colunm" style ="white-space:nowrap;padding-left:5px;padding-right:5px;">
                Người xử lý
            </th>  
            <th class="thead-colunm" style ="white-space:nowrap;padding-left:5px;padding-right:5px;">
                Phân việc
            </th>            
            <th class="thead-colunm" style ="white-space:nowrap;padding-left:5px;padding-right:5px;">
                Ngày tiếp nhận
            </th>
            <th class="thead-colunm-end" style="color: #fff;white-space:nowrap;padding-left:5px;padding-right:5px;">
                Ngày quá hạn PB
            </th>
             <th class="thead-colunm" style="padding-left: 5px;">
                Nội dung
            </th>
        </thead>
        <tbody id="grid-data">
        </tbody>
    </table>
</div>
