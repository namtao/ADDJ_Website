<%@ Page Title="" Language="C#" MasterPageFile="~/Master_Default.master" AutoEventWireup="true" CodeBehind="mn_HtNguoiDungTuExcel.aspx.cs" Inherits="Website.HeThongHoTro.Manager.mn_HtNguoiDungTuExcel" %>

<%@ Register Assembly="DevExpress.Web.v21.2, Version=21.2.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeaderCss" runat="server">
    <style type="text/css">
        .inputstyle.cus input[type=text] {
            padding: 0 5px;
            width: 200px;
        }

        .btn_style_button {
            padding: 5px 15px;
        }

        .DDPager {
            margin: 10px 0px;
        }

        .tbl_style a {
            color: #034AFC;
        }

        .msg {
            font-size: 12px;
            font-weight: bold;
            color: red;
        }

        div.DDPager {
            border: 1px solid #ccc;
            padding: 5px;
            padding-top: 10px;
        }

        .DDControl {
            text-align: center;
        }

        #mainmenu {
            position: relative;
            z-index: 9999;
        }

        .tbl_style .tooltip {
            color: black;
            cursor: help;
        }
    </style>
    <script type="text/javascript">

        function Uploader_OnUploadStart() {
            btnUpload.SetEnabled(false);
        }
        function Uploader_OnFileUploadComplete(args) {
            var imgSrc;
            if (args.isValid) {
                var date = new Date();
                imgSrc = "/HTHTKT/Uploadcontrol/UploadImages/" + args.callbackData + "?dx=" + date.getTime();
            }
            //alert(imgSrc);
            $('#nameFile').text(imgSrc);
            //$('#previewImage').attr("src", imgSrc);
            //getPreviewImageElement().src = imgSrc;

            gvReadExcel.PerformCallback();
        }
        function Uploader_OnFilesUploadComplete(args) {
            UpdateUploadButton();
        }
        function UpdateUploadButton() {
            btnUpload.SetEnabled(uploader.GetText(0) != "");
        }
        function getPreviewImageElement() {
            return document.getElementById("previewImage");
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderJs" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder_Main" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder_Text" runat="server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="Content" runat="server">
    <!-- begin panel nav boot -->
    <div class="nav_btn_bootstrap">
        <ul>
            <li><a href="javascript:history.back()" class="btn btn-primary"><span class="glyphicon glyphicon-circle-arrow-left" aria-hidden="true"></span>&nbsp;Quay về</a></li>
            <li><a onclick="themMoiLuongXuly()" class="btn btn-primary"><span class="new"><span class="glyphicon glyphicon-plus-sign" aria-hidden="true"></span>&nbsp;Thêm mới</span></a></li>
        </ul>
        
    </div>
    <!-- end panel nav boot -->
    <div class="p8">
        <!-- begin panel boot -->
        <div class="panel panel-default">
            <div class="panel-heading"><span style="font-size: 12pt"></span></div>
            <div class="panel-body" style="border: none">
                <!-- begin body boot -->




                <table cellpadding="0" cellspacing="0">
                    <tr>
                        <td align="left" style="padding-right: 20px; vertical-align: top;">
                            <table cellpadding="0" cellspacing="0">
                                <tr>
                                    <td>
                                        <dx:ASPxLabel ID="lblSelectImage" runat="server" Text="Chọn tệp tin:" AssociatedControlID="uplImage">
                                        </dx:ASPxLabel>
                                    </td>
                                    <td>
                                        &nbsp;</td>
                                    <td>
                                        <dx:ASPxUploadControl ID="uplImage" runat="server" ClientInstanceName="uploader" Theme="Office2010Blue"
                                            ShowProgressPanel="True" Size="35" OnFileUploadComplete="uplImage_FileUploadComplete">
                                            <ClientSideEvents FileUploadComplete="function(s, e) { 
                                    Uploader_OnFileUploadComplete(e); 
                                }"
                                                FilesUploadComplete="function(s, e) { 
                                    Uploader_OnFilesUploadComplete(e); 
                                }"
                                                FileUploadStart="function(s, e) { 
                                    Uploader_OnUploadStart(); 
                                }"
                                                TextChanged="function(s, e) { 
                                    UpdateUploadButton();   
                                }"></ClientSideEvents>
                                            <ValidationSettings MaxFileSize="4194304" AllowedFileExtensions=".xlsx">
                                            </ValidationSettings>
                                            <BrowseButton Text="Duyệt...">
                                            </BrowseButton>
                                        </dx:ASPxUploadControl>
                                    </td>
                                </tr>
                                <tr>
                                    <td></td>
                                    <td>&nbsp;</td>
                                    <td>
                                        <dx:ASPxLabel ID="lblAllowebMimeType" runat="server" Text="Những tập tin được phép có phần mở rộng: xlsx"
                                            Font-Size="8pt" ForeColor="Red" Font-Bold="true">
                                        </dx:ASPxLabel>
                                        <br />
                                        <dx:ASPxLabel ID="lblMaxFileSize" runat="server" Text="Dung lượng lớn nhất mỗi tệp: 4Mb" Font-Size="8pt">
                                        </dx:ASPxLabel>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="3" align="left" style="padding:10px">
                                        <dx:ASPxButton ID="btnUpload" runat="server" AutoPostBack="False" Text="Tải lên" ClientInstanceName="btnUpload"
                                            Width="100px" ClientEnabled="False" Theme="Office2010Blue" >
                                            <ClientSideEvents Click="function(s, e) { 
                                uploader.Upload(); 
                            }" /><Image Url="../../HTHTKT/icons/level_up_16x16.gif"></Image>
                                        </dx:ASPxButton>
                                        <a href="../../HTHTKT/UploadFiles/file_template_import_user.xlsx">Tải file mẫu</a>
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td align="center" class="imagePreviewCell">
                            <%-- <img src="../Content/ImagePreview.gif" id="previewImage" alt="" />--%>
                            <span id="nameFile"></span>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">

                            <dx:ASPxGridView ID="gvReadExcel" ClientInstanceName="gvReadExcel" runat="server" Theme="Office2010Blue"
                                AutoGenerateColumns="false" KeyFieldName="TENTRUYCAP"
                                OnCustomCallback="gvReadExcel_CustomCallback"
                                OnDataBinding="gvReadExcel_DataBinding"
                                OnPageIndexChanged="gvReadExcel_PageIndexChanged"
                                OnHtmlDataCellPrepared="gvReadExcel_HtmlDataCellPrepared" EnablePagingGestures="False">
                                <Columns>
                                    <dx:GridViewDataTextColumn FieldName="TONGCT" Caption="Tổng CTy"></dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn FieldName="KHUVUC" Caption="Khu vực"></dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn FieldName="DONVI" Caption="Đơn vị"></dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn FieldName="BOPHAN" Caption="Bộ phận"></dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn FieldName="CANHAN" Caption="Cá nhân"></dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn FieldName="HOTEN" Caption="Họ và tên"></dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn FieldName="DIENTHOAI" Caption="Số điện thoại"></dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn FieldName="TENTRUYCAP" Caption="Tên truy cập"></dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn FieldName="EMAIL" Caption="Email"></dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn FieldName="QUYEN" Caption="Quyền truy cập"></dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn FieldName="TRANGTHAI" Caption="Trạng thái"></dx:GridViewDataTextColumn>
                                </Columns>
                            </dx:ASPxGridView>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" style="padding:10px">
                            <dx:ASPxButton runat="server" ID="importHT" ClientInstanceName="importHT" Text="Nhập vào hệ thống" Theme="Office2010Blue"
                              OnClick="importHT_Click"
                                > 
                                <Image Url="../../HTHTKT/icons/flag3-add-16x16.gif"></Image>
                            </dx:ASPxButton>
                            <dx:ASPxButton runat="server" ID="lamMoi" ClientInstanceName="lamMoi" Text="Làm mới" Theme="Office2010Blue"
                             OnClick="lamMoi_Click">
                                <Image Url="../../HTHTKT/icons/refresh_16x16.gif"></Image>
                            </dx:ASPxButton>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" style="padding:10px">
                            <dx:ASPxLabel ID="lblStatus" ClientInstanceName="lblStatus" runat="server" Theme="Office2010Blue" Text=""
                                  ForeColor="Red"></dx:ASPxLabel>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <div class="alert alert-warning">
                                <strong>Chú ý:</strong> Bạn hãy nhập theo mẫu excel để nhập vào hệ thống cho chính xác.
                            </div>
                        </td>
                    </tr>
                </table>
                 



            </div>
            <!-- end body boot -->
        </div>
    </div>
    <!-- end panel boot -->
</asp:Content>
