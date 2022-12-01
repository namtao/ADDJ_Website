<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SoTheoDoiEdit.ascx.cs"
    Inherits="Website.Views.QLKhieuNai.UserControls.SoTheoDoiEdit" %>
<style type="text/css">
    #divChiTietKN-Title h1
    {
        font-size: 14px;
        font-weight: bold;
        padding-left: 10px;
        padding-right: 10px;
        padding-bottom: 5px;
        border-bottom: 1px solid #CCC;
        line-height: 33px;
        margin-bottom: 10px;
    }
    #divChiTietKN-Info table
    {
        border: 1px solid #ccc;
        background: #F2F2F2;
    }
    #divChiTietKN-Info table tr
    {
        line-height: 35px;
    }
    #divChiTietKN-Info table tr td
    {
        white-space: nowrap;
        padding-left: 10px;
    }
    #divChiTietKN-Info table tr td.cls-title
    {
        white-space: nowrap;
        padding-left: 10px;
        width: 110px;
    }
    #divChiTietKN-Info table tr td.cls-date
    {
        white-space: nowrap;
        padding-left: 10px;
        width: 150px;
    }
    #divChiTietKN-Info .info
    {
        font-size: 12px;
        font-weight: bold;
        color: #4D709A;
        padding-right: 10px;
    }
</style>
<script language="javascript">
    function CallServerMethod() {
        var values = "<%= GetValue()%>";
        
    }
    function SetValues(maKhieuNai) {
        $("#<%=HiddenField1.ClientID %>").val(maKhieuNai);
        document.getElementById("<%=btnUpload.ClientID %>").click();
    }
    function DeleteFile(fileId) {
        var r = confirm("Bạn muốn xóa file này ?");
        if (r == true) {
            $.getJSON('/Views/Ajax/Ajax.ashx?type=rf&id=' + fileId, '', function (result) {
                $('#fileId-' + fileId).remove();
                fnLocKhieuNai();

            });
        }


    }
    function CreateFileUpload() {
        $("#createFileUpload").append("<input name=\"uploadedfile\" type=\"file\" accept=\".jpg, .png, .pdf, .doc, .docx, .xls, .xlsx, .rar, .zip, .7z, .ppt, .pptx\" style=\"width: 350px;\" onchange=\"return validateFileExtension(this);\" /><br />");
        
    }
    function RemoveFileUpload() {
        $('#createFileUpload input:last').remove();
        $('#createFileUpload br:last').remove();

    }
    function validateFileExtension(fld) {
        if (!/(\.jpg|\.png|\.pdf|\.doc|\.docx|\.xls|\.xlsx|\.rar|\.zip|\.7z|\.ppt|\.pptx)$/i.test(fld.value)) {
            fld.value = '';            
            fld.focus();
            MessageAlert.AlertNormal('Chỉ được phép định dạng file sau !<br />.jpg, .png, .pdf, .doc, .docx, .xls, .xlsx, .rar, .zip, .7z, .ppt, .pptx');
            return false;
        }
        return true;
    } 
</script>
<div id="divChiTietKN-Info" style="">
    
    <table cellpadding="0" cellspacing="0" width="100%" style="white-space: nowrap; font-size: 12px;background:none;border:none;">
        <tr>
            <td colspan="4" align="center">
                <span id="message-error" style="display: none; color: Red;"></span>
            </td>
        </tr>
        <tr>
            <td class="cls-title">
                <strong>Số thuê bao:<span style="color: Red;">(*)</span></strong>
            </td>
            <td>
                <div class="inputstyle">
                    <div class="bg">
                        <input type="text" id="DauSo" maxlength="11" value="84" style="width: 30px; float: left;
                            text-align: center;" readonly="readonly" />
                        <input type="text" id="SoThueBao" maxlength="11" value="0972455822" style="width: 200px;
                            float: left;" class="typeNumber" />
                        <a href="javascript:TraCuuThongTin();" style="padding: 5px 10px 5px 10px; color: #fff;
                            background: #4D709A; height: 20px; margin-left: 10px; border-radius: 3px 3px 3px 3px;">
                            ...</a>
                    </div>
                </div>
            </td>
            <td class="cls-title">
                <strong>Ngày nhận:<span style="color: Red;">(*)</span></strong>
            </td>
            <td class="cls-date">
                <div class="inputstyle">
                    <div class="bg">
                        <input type="text" id="NgayTiepNhan" class="" />
                    </div>
                </div>
            </td>
        </tr>
        <tr>
            <td class="cls-title">
                <strong>Tên khách hàng:<span style="color: Red;">(*)</span></strong>
            </td>
            <td colspan="3">
                <div class="inputstyle">
                    <div class="bg">
                        <input type="text" id="HoTenLienHe" value="Trần Văn Test" class="" />
                    </div>
                </div>
            </td>
        </tr>
        <tr>
            <td class="cls-title">
                <strong>Đia chỉ liên hệ:<span style="color: Red;">(*)</span></strong>
            </td>
            <td colspan="3">
                <div class="inputstyle">
                    <div class="bg">
                        <input type="text" id="DiaChiLienHe" value="Ha Noi-My Dinh-Từ Liêm" class="" />
                    </div>
                </div>
            </td>
        </tr>
        <tr>
            <td class="cls-title">
                <strong>Chọn khiếu nại:<span style="color: Red;">(*)</span></strong>
            </td>
            <td colspan="3">
                <div class="selectstyle">
                    <div class="bg">
                        <select id="DropLoaiKhieuNai" onchange="javascript:fnDropLoaiKhieuNaiChange();" style="width: 200px;">
                        </select>
                        <select id="DropLinhVucChung" onchange="javascript:fnDropLinhVucChungChange();" style="width: 200px;">
                        </select>
                        <select id="DropLinhVucCon" style="width: 200px;">
                        </select>
                    </div>
                </div>
            </td>
        </tr>
        <tr>
            <td class="cls-title" valign ="top">
                <strong>Giấy tờ kèm theo:</strong>
            </td>
            <td>
                <div class="inputstyle">
                    <div class="bg">
                        <%--<asp:FileUpload ID="FileUploadJquery" runat="server" accept=".jpg, .png, .pdf, .doc, .docx, .xls, .xlsx, .rar, .zip, .7z, .ppt, .pptx" />--%>
                        <input name="uploadedfile" type="file" accept=".jpg, .png, .pdf, .doc, .docx, .xls, .xlsx, .rar, .zip, .7z, .ppt, .pptx" style="width: 350px;float:left;" onchange="return validateFileExtension(this)" />
                        <a href ="javascript:CreateFileUpload();" style =" float: left; height: 20px; margin-top: 10px; padding-left: 10px; padding-right: 10px; width: 20px;">
                            <img src ="/images/icons/plus2.gif" />
                        </a>
                        <a href ="javascript:RemoveFileUpload();" style =" float: left; height: 20px; margin-top: 10px; padding-right: 10px; width: 20px;">
                            <img src ="/images/icons/minus2.gif" />
                        </a>
                        <div style ="clear:both;height:1px;"></div>
                        <div id="createFileUpload">
                            
                        </div>
                        <div id="divListFile" style="line-height: 15px;">
                        </div>
                        <%--<img src ="/images/icons/del_file.png" />--%>
                    </div>
                </div>
            </td>
            <td class="cls-title" valign ="top">
                <strong>Ngày hẹn trả lời:<span style="color: Red;">(*)</span></strong>
            </td>
            <td class="cls-title" valign ="top">
                <div class="inputstyle">
                    <div class="bg">
                        <input type="text" id="NgayTraLoiKN" class="" />
                    </div>
                </div>
            </td>
        </tr>
        <tr>
            <td class="cls-title" valign ="top">
                <strong>Nội dung khiếu nại:<span style="color: Red;">(*)</span></strong>
            </td>
            <td colspan="3">
                <div class="inputstyle">
                    <div class="bg">
                        <textarea name="test" rows="3" id="NoiDungPA">Ha Noi-My Dinh-Từ Liêm</textarea>
                    </div>
                </div>
            </td>
        </tr>
        <tr>
            <td class="cls-title" valign ="top">
                <strong>Trình tự xử lý:</strong>
            </td>
            <td colspan="3">
                <div class="inputstyle">
                    <div class="bg">
                        <textarea name="test" rows="3" id="NoiDungXuLy">Ha Noi-My Dinh-Từ Liêm</textarea>
                    </div>
                </div>
            </td>
        </tr>
        <tr>
            <td class="cls-title" valign ="top">
                <strong>Kết quả trả lời:</strong>
            </td>
            <td colspan="3">
                <div class="inputstyle">
                    <div class="bg">
                        <textarea name="test" rows="3" id="KetQuaXuLy">Ha Noi-My Dinh-Từ Liêm</textarea>
                    </div>
                </div>
            </td>
        </tr>
        <tr>
            <td class="cls-title" valign ="top">
                <strong>Ghi Chú:</strong>
            </td>
            <td colspan="3">
                <div class="inputstyle">
                    <div class="bg">
                        <textarea name="test" rows="3" id="GhiChu">Ha Noi-My Dinh-Từ Liêm</textarea>
                    </div>
                </div>
            </td>
        </tr>
        <tr>
            <td colspan="4" align="right">
                <div class="nav_btn" style='background: none;'>
                    <ul>
                        <li style="float: right;"><a href="javascript:ClosePoup();"><span class="notapply">Hủy
                        </span></a></li>
                        <li style="float: right;"><a href="javascript:fnUpdateSoTheoDoi();"><span class="apply">
                            Đồng ý </span></a></li>
                    </ul>
                </div>
                <asp:HiddenField ID="HiddenField1" runat="server" />
                <div style="display: none;">
                    <asp:Button ID="btnUpload" Visible="true" runat="server" Text="Button" OnClick="btnUpload_Click" />
                </div>
            </td>
        </tr>
    </table>
</div>
