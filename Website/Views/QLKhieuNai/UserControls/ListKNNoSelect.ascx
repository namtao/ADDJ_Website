<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ListKNNoSelect.ascx.cs"
    Inherits="Website.Views.QLKhieuNai.UserControls.ListKNNoSelect" %>
<script language="javascript">
    function SelectAllCheckboxes(spanChk) {
        //var listID = '';
        var oItem = spanChk.children;
        var theBox = (spanChk.type == "checkbox") ?
            spanChk : spanChk.children.item[0];
        xState = theBox.checked;

        elm = theBox.form.elements;
        //console.log(elm);
        for (i = 0; i < elm.length; i++) {
            if (elm[i].type == "checkbox" &&
                     elm[i].id != theBox.id) {
                if (elm[i].checked != xState) {
                    //                     if (xState == true) {
                    //                         listID += elm[i].value + "#";
                    //                     }
                    elm[i].click();

                }

            }
        }

    }
</script>
<div id="divScroll" style='overflow-x: scroll; height: auto;'>
    <table class="tbl_style" cellspacing="0" cellpadding="0" style='width: 2150px;'>

        <thead class="grid-data-thead">
            <%--<th class="thead-colunm">
                <input id="ckCheckAll" onclick="javascript:SelectAllCheckboxes(this);" type="checkbox" />
            </th>--%>
            <th class="thead-colunm" style="white-space: nowrap; padding-left: 5px; padding-right: 5px;">STT
            </th>
            <th class="thead-colunm" style="white-space: nowrap; padding-left: 5px; padding-right: 5px;">Trạng thái
            </th>
            <th class="thead-colunm" style="white-space: nowrap; padding-left: 5px; padding-right: 5px; width: 100px;">Mã PA/KN
            </th>
            <th class="thead-colunm" style="white-space: nowrap; padding-left: 5px; padding-right: 5px; width: 100px;">Độ ưu tiên
            </th>
            <th class="thead-colunm" style="white-space: nowrap; padding-left: 5px; padding-right: 5px; width: 100px;">Số thuê bao
            </th>
            <th class="thead-colunm" style="white-space: nowrap; padding-left: 5px; padding-right: 5px;">Loại khiếu nại
            </th>
            <th class="thead-colunm" style="white-space: nowrap; padding-left: 5px; padding-right: 5px;">Lĩnh vực chung
            </th>
            <th class="thead-colunm" style="white-space: nowrap; padding-left: 5px; padding-right: 5px;">Lĩnh vực con
            </th>

            <th class="thead-colunm" style="white-space: nowrap; padding-left: 5px; padding-right: 5px; width: 150px;">Phòng ban xử lý
            </th>
            <th class="thead-colunm" style="white-space: nowrap; padding-left: 5px; padding-right: 5px;">Người tiếp nhận
            </th>
            <th class="thead-colunm" style="white-space: nowrap; padding-left: 5px; padding-right: 5px;">Người xử lý
            </th>

            <th class="thead-colunm" style="white-space: nowrap; padding-left: 5px; padding-right: 5px;">Ngày tiếp nhận
            </th>
            <th class="thead-colunm-end" style="color: #fff; white-space: nowrap; padding-left: 5px; padding-right: 5px;">Ngày quá hạn
            </th>
            <th class="thead-colunm" style="padding-left: 5px;">Nội dung
            </th>
        </thead>
        <tbody id="grid-data">
        </tbody>
    </table>
</div>


