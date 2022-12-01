<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ListKNSelectKNHangLoat.ascx.cs"
    Inherits="Website.Views.QLKhieuNai.UserControls.ListKNSelectKNHangLoat" %> 
 <script language ="javascript">
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
    <table class="tbl_style" cellspacing="0" cellpadding="0" style='width: 1830px;'>
        <colgroup>
            <col width="30px">
            <col width="50px">
            <col width="100px">
            <col width="100px">
            <col width="100px">
            <col width="150px">
            <col width="150px">
            <col width="150px">
            <col width="300px">
            <col width="150px">
            <col width="150px">
            <col width="150px">
            <col width="150px">
            <col width="150px">
        </colgroup>
        <thead class="grid-data-thead">
            <th class="thead-colunm">
            <input id ="ckCheckAll" onclick="javascript:SelectAllCheckboxes(this);" type ="checkbox" />
            </th>
            <th class="thead-colunm">
                Xử lý
            </th>
            <th class="thead-colunm">
                Mã PA/KN
            </th>
            <th class="thead-colunm">
                Độ ưu tiên
            </th>
            <th class="thead-colunm">
                Số thuê bao
            </th>
            <th class="thead-colunm">
                Loại khiếu nại
            </th>
            <th class="thead-colunm">
                Lĩnh vực chung
            </th>
            <th class="thead-colunm">
                Lĩnh vực con
            </th>
            <th class="thead-colunm">
                Nội dung
            </th>
            <th class="thead-colunm">
                Người tiếp nhận
            </th>
            <th class="thead-colunm">
                Người xử lý
            </th>
            <th class="thead-colunm">
                Người tiền xử lý
            </th>
            <th class="thead-colunm">
                Ngày tiếp nhận
            </th>
            <th class="thead-colunm-end" style="color: #fff;">
                Ngày quá hạn
            </th>
        </thead>
        <tbody id="grid-data">
            <%--<tr class="rowA">
                <td align="center">
                    <input id ="ck" type ="checkbox" />
                </td>
                <td align="center">
                    <span style="border: 1pt solid #CCC; background: #FF6600; width: 15px; height: 10px;">
                    </span>
                </td>
                <td align="center">
                    MaKN
                </td>
                <td align="center">
                    Độ ưu tiên
                </td>
                <td align="center">
                    <a href="javascript:ShowPoup('MKN:3333-333');" title="Hiển thị thông tin chi tiết khiếu nại">
                        0972 455 822</a>
                </td>
                <td align="center">
                    Loại khiếu nại
                </td>
                <td align="center">
                    Lĩnh vực chung
                </td>
                <td align="center">
                    Lĩnh vực con
                </td>
                <td align="center">
                    Nội dung
                </td>
                <td align="center">
                    Người tiếp nhận
                </td>
                <td align="center">
                    Người xử lý
                </td>
                <td align="center">
                    Người tiền xử lý
                </td>
                <td align="center">
                    Ngày tiếp nhận
                </td>
                <td align="center">
                    Ngày quá hạn
                </td>
            </tr>
            <tr class="rowB">
                <td align="center">
                    <input id ="ckCheck" type ="checkbox" />
                </td>
                <td align="center">
                    1
                </td>
                <td align="center">
                </td>
                <td align="center">
                </td>
                <td align="center">
                </td>
                <td align="center">
                    1
                </td>
                <td align="center">
                </td>
                <td align="center">
                </td>
                <td align="center">
                </td>
                <td align="center">
                    1
                </td>
                <td align="center">
                </td>
                <td align="center">
                </td>
                <td align="center">
                </td>
                <td align="center" class="td-column-end">
                </td>
            </tr>--%>
        </tbody>
    </table>
</div>
<div class="div-clear" style="height: 10px;">
</div>
<div id="Pagination" class="pagination" style="float: right; margin-right: -5px;">
</div>
<div class="div-clear">
</div>
<div id="Searchresult">
</div>
