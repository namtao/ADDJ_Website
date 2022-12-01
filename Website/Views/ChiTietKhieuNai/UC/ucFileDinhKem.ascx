<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucFileDinhKem.ascx.cs" Inherits="Website.Views.KhieuNai.UC.ucFileDinhKem" %>
<script src="/JS/plugin/ajaxfileupload.js" type="text/javascript"></script>
<style>
    .eole hover{background: none}
</style>
<table cellspacing="0" cellpadding="0" border="0" style="border-collapse: collapse;" class="tbl_style dyc">
    <tbody><tr>
        <th scope="col"><span>Người tạo</span>
        </th>
        <th scope="col"><span>Tên file đính kèm</span>
        </th>
        <th scope="col"><span>Loại file</span>
        </th>
        <th scope="col"><span>Dung lượng</span>
        </th>
        <th scope="col" style="width:250px"><span>Ghi chú</span>
        </th>
        <th scope="col"><span>Ngày Up file</span>
        </th>
        <th scope="col"><span>Thao tác</span>
        </th>
    </tr>
    <%=listFile %>
    <%if (Mode != "View" && taomoi)
    {%>
    <tr id="footerGr" class="eole">
    <td></td>
        <td>
            <span>
                <input type="file" accept=".jpg, .png, .pdf, .doc, .docx, .xls, .xlsx, .rar, .zip, .7z, .ppt, .pptx, .txt, .csv, .mp3," id="file1" name="file1" /></span>
                <img id="loading" style="display:none;" src="../../images/loader.gif" />
            <span id="lblMsg" style="color:red"></span>
        </td>
        <td>
            <select id="slFileType" style="width:100px;">
                <option value="1">Khách hàng</option>
                <option value="2">Bảo mật</option>
            </select>
        </td>
        <td></td>
        <td>
            <span>
                <div class="inputstyle_longlx">
                    <div class="bg">
                        <textarea class="mw" id="txtGhiChu" cols="20" rows="2" ></textarea>
                    </div>
                </div>
            </span>
        </td>
        <td>
            <span></span>
        </td>
        <td>
            <span style="text-align:center">
                <a href="javascript:void(0);" onclick="UploadAjax()" class="mybtn"><span class="nhapdl">Thêm mới</span></a>
            </span>
        </td>
    </tr>
    <%} %>
</tbody></table>

<script type="text/javascript" language="javascript">

    var xcm = "<%=xoacuaminh %>";
    var sua = "<%=sua %>";
    var xoa = "<%=xoa %>";
    function UploadAjax() {
        if ($('#file1').val().length == 0) {
            $.messager.alert("Thông báo", "Bạn chưa chọn file");
            return false;
        }
        var value = $('#file1').val();
        
        var accept = $('#file1').attr('accept');
        var extend = value.substring(value.lastIndexOf('.'), value.length).toLowerCase();
        if (accept.indexOf(extend + ',') < 0) {
            $.messager.alert("Thông báo", "File phải có định dạng sau: " + accept.substring(0, accept.lastIndexOf(',')));
            return;
        }

        var ele = $('#footerGr');
        ajaxFileUpload(ele);
    }
    function ajaxFileUpload(ele) {
        $('#loading').show();
        var MaKN = Utility.GetUrlParam("MaKN");
        var ghichu = encodeURIComponent($('#txtGhiChu').val());
        var fType = $('#slFileType').val();

        $.ajaxFileUpload(
		        {
		            url: '../Ajax/HandlerUpload.ashx?MaKN=' + MaKN + '&ghichu=' + ghichu + "&fType=" + fType,
		            secureuri: false,
		            sfileElementId: 'file1',
		            dataType: 'json',
		            success: function (obj) {
                        //this.sub
		                if (obj.ErrorId == 0) {
		                    var delBtn = "";
		                    var editBtn = "";
                            if(xcm == "True")
                            {
                                delBtn = " <a href='javascript:void(0)' onclick=\"DeleteFile('" + obj.Id + "');\" class=\"mybtn\"><span class=\"del_file\">Xoá</span></a>";
		                    }
                            if(sua == "True")
                            {
                                editBtn = "<a href='javascript:void(0)' onclick='EditFile(" + obj.Id + ")' title='Sửa ghi chú' class=\"mybtn\"><span class=\"edit\">Sửa</span></a>";
                            }

                            var html = "<tr id='fRow" + obj.Id + "'><td><span>" + obj.CUser + "</span></td>";
                            html += "<td><div style='word-wrap: break-word; width:200px'><a  style='text-decoration: underline' href='" + obj.URLFile + "' title='Download file' target='_blank'>" + obj.TenFile + "</a></div></td>";
                            if(obj.Status == 1)
                                html += "<td><span>Khách hàng</span></td>";
                            else if (obj.Status == 2)
                                html += "<td><span>Bảo mật</span></td>";
                            else
                                html += "<td><span></span></td>";

                            html += "<td><span>" + obj.KichThuoc + " KB</span></td>";
                            html += "<td><div style='word-wrap: break-word; width:250px'>" + obj.GhiChu + "</div></td>";
                            html += "<td><span>" + obj.LoaiFile + "</span></td>";
		                    html += "<td><span style='text-align:center'>" + editBtn + delBtn + "</div></td>";
		                    html += "</tr>";
		                    $('#nodata').remove();
		                    ele.before(html);
		                    $('#file1').val('');
		                    $('#txtGhiChu').val('');
		                    $.messager.alert("Thông báo", "Bạn đã upload thành công");
		                }
		                else if (obj.ErrorId == "0") {
		                    $('#lblMsg').val("Bạn chưa chọn file!");
		                }
		                else if (obj.ErrorId == "-1") {
		                    $('#lblMsg').val("Bạn chưa đăng nhập!");
		                }
		                $('#loading').hide();
		            },
		            error: function (e) {
		                $('#loading').hide();
		            }
		        })
        return false;
    }


    function DeleteFile(id) {
        $.messager.confirm("Xác nhận","Bạn chắc chắn muốn xóa file này?", function (b) {
            if (b) {
                $.ajax({
                    beforeSend: function () {
                        
                    },
                    type: "POST",
                    dataType: "text",
                    url: "../Ajax/Ajax.ashx",
                    data: { type: "rf", id: id },
                    success: function (text) {
                        if (text == "1") {
                            $('#fRow' + id).slideUp(1000).remove();
                            $.messager.alert("Thông báo", "Bạn xóa file thành công.");
                        }
                        else {
                            $.messager.alert("Thông báo", text);
                        }
                    },
                    error: function () {
                    }
                });
            }
        });
    }
    function UpdateFile(id) {
        $.messager.confirm("Xác nhận", "Bạn chắc chắn muốn cập nhật?", function (b) {
            if (b) {
                var ghichu = $('#editGhiChu').val();
                $.ajax({
                    beforeSend: function () {

                    },
                    type: "POST",
                    dataType: "text",
                    url: "../Ajax/Ajax.ashx",
                    data: { type: "uf", id: id, ghichu: ghichu },
                    success: function (text) {
                        if (text == "1") {
                            var obj = $('#fRow' + id).find('td').eq(3);
                            obj.html('<div style="word-wrap: break-word; width:400px">' + ghichu + '</div>');
                            var objButton = $('#fRow' + id).find('td').eq(5);
                            objButton.html(oldButton);
                        }
                        else {
                            $.messager.alert("Thông báo", "Mời bạn thử lại.");
                        }
                    },
                    error: function () {
                    }
                });
            }
        });
    }
    function EditFile(id) {
        ShowEdit(id);
        preId = id;
//        var obj = $('#fRow' + id).find('td').eq(3);
//        var objButton = $('#fRow' + id).find('td').eq(5);
//        preButton = objButton.html();
//        preVal = obj.html();
    }

    var oldVal;
    var oldButton;
    var preVal;
    var preButton;
    var preId = "0"; ;
    function ShowEdit(id) {
        if (preId != "0") CancelEdit(preId);
        var obj = $('#fRow' + id).find('td').eq(3);
        var oldText = obj.text();
        var objButton = $('#fRow' + id).find('td').eq(5);
        oldButton = objButton.html();
        oldVal = obj.html();
        var editHtml = '<span><div class="inputstyle_longlx"> <div class="bg"><textarea rows="2" cols="20" id="editGhiChu" class="mw">' + oldText + '</textarea></div></div></span><textarea style="display:none" rows="2" cols="20" id="GhiChuTruoc" class="mw">' + oldVal + '</textarea>';
        var buttonHtml = "<span style='text-align:center'><a href='javascript:void(0)' onclick='UpdateFile(" + id + ")' title='Cập nhật' class=\"mybtn\"><span class=\"save\">Cập nhật</span></a> <a href='javascript:void(0)' onclick=\"CancelEdit('" + id + "');\" class=\"mybtn\"><span class=\"cancel\">Hủy</span></a> <a href='javascript:void(0)' onclick=\"DeleteFile('" + id + "');\" class=\"mybtn\"><span class=\"del_file\">Xóa</span></a></span>";
        buttonHtml += '<textarea style="display:none" rows="2" cols="20" id="ButtonTruoc" class="mw">' + oldButton + '</textarea>';
        obj.html(editHtml);
        objButton.html(buttonHtml);
    }
    function CancelEdit(id) {
        var obj = $('#fRow' + id).find('td').eq(3);
        var objButton = $('#fRow' + id).find('td').eq(5);
        obj.html($('#GhiChuTruoc').val());
        objButton.html($('#ButtonTruoc').val());
    }

    </script>