<%@ Page Title="" Language="C#" MasterPageFile="~/Master_Default.master" AutoEventWireup="true" CodeBehind="mn_TraCuuThongTinYCHT.aspx.cs" Inherits="Website.HeThongHoTro.Manager.mn_TraCuuThongTinYCHT" %>

<%@ Register Assembly="DevExpress.Web.v21.2, Version=21.2.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxTreeList.v21.2, Version=21.2.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxTreeList" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxHtmlEditor.v21.2, Version=21.2.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxHtmlEditor" TagPrefix="dx" %>



<%@ Register assembly="DevExpress.Web.ASPxSpellChecker.v21.2, Version=21.2.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxSpellChecker" tagprefix="dx" %>



<asp:Content ID="Content1" ContentPlaceHolderID="HeaderCss" runat="server">
    <style type="text/css">
        #divAllUsers
    /*{
        width:720px;
        color: grey;
        font-family: sans-serif;
    }*/
    .PageLink
    {
        text-decoration: none;
        color: #2C70B6;
        cursor: pointer;
        outline: 0 none !important;
    }
    .PerformanceHeader
    {
        font-size: small;
        font-weight: bold;
        text-align:center;
        height:28px;
        overflow: hidden;
        position: relative;
        line-height: 26px;
        background-repeat: repeat-x;
        background-image: linear-gradient(to bottom, #F1F1F1 1%,#A2A2A2 100%);
    }
    .trUser:hover
    {
        font-weight:bold;
    }
    .trUser
    {
        font-weight:500;
    }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderJs" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder_Main" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder_Text" runat="server">
    <script type="text/javascript">
        function ChonDonVi(s, e) {
            var tendv = '';
            var key = treelist_donvi_cc.GetFocusedNodeKey();
            treelist_donvi_cc.GetNodeValues(key, "linhvuc", function (value) {
                LoaiHoTro.SetText(value);
                tenDonVi = value;
                LoaiHoTro.SetKeyValue(key);
                LoaiHoTro.HideDropDown();
                //cmb_nhanvien_gsbh.PerformCallback(DropDownEdit.GetKeyValue());
            });
            // gán id loai hỗ trợ cho hidden
            LoaiHTKTID.Set('LoaiHTKTID', key);
            //noiChuyenDen.PerformCallback();
        }
        var postponedCallbackRequired = false;
        //Chọn hệ thống hỗ trợ kỹ thuật
        function OnListBoxIndexChanged(s, e) {
            debugger;
            IDLoaiHoTro.Set('hidden_value', s.GetValue());
            IDLoaiHoTro2.Set('hidden_value2', 1);
            isReshresd.Set('res_value', 1);
            //treelist_donvi_cc.PerformCallback('refresh');
            if (CallbackPanel.InCallback())
                postponedCallbackRequired = true;
            else
                CallbackPanel.PerformCallback();
            // gán giá trị id cho hidden
            ThongHTKTID.Set('ThongHTKTID', s.GetValue());

            //XuLyLayIdLuongHoTro(s.GetValue(), NoiChuyenDenID.Get('iddonvitao'));
        }
        function OnEndCallback(s, e) {
            if (postponedCallbackRequired) {
                CallbackPanel.PerformCallback();
                postponedCallbackRequired = false;
            }
        }


        function chonDonViAdd(s, e) {

            var tendv = '';
            var key = treeListDanhSachDonViAdd.GetFocusedNodeKey();
            treeListDanhSachDonViAdd.GetNodeValues(key, "TENDONVI", function (value) {
                ddlChonDonViAdd.SetText(value);
                ddlChonDonViAdd.SetKeyValue(key);
                ddlChonDonViAdd.HideDropDown();
            });
            treeListDanhSachDonViAdd.GetNodeValues(key, "ID", function (value) {
                HiddenField.Set('ID_DONVI_ADD', value);
            });
        }


        function loadBaoCao(sothuebao, hethong, linhvuc, mucdosuco, tungay, denngay, iddonvi, nguoidung,mayeucau) {
            $.ajax({
                type: 'POST',
                url: '/HeThongHoTro/Services/ws_baoCao.asmx/bc_traCuuThongTinTongHop',
                data: "{ sothuebao: '" + sothuebao + "',hethong: '" + hethong + "',linhvuc: '" + linhvuc + "',mucdosuco: '" + mucdosuco + "',tungay: '" + tungay + "',denngay: '" + denngay + "',iddonvi: '" + iddonvi + "',nguoidung: '" + nguoidung + "',mayeucau: '" + mayeucau + "' }",
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                beforeSend: function () {
                    loadingdata.Show();
                },
                success: function (jsonData) {
                    var objret = JSON.parse(jsonData.d);
                    $("#noiDungTraCuu tbody").html("");
                    var table = '';
                    var tr;
                    for (var i = 0; i < objret.length; i++) {
                        tr = $('<tr/>');
                        tr.append("<td>" + objret[i].ID + "</td>");                      
                        //tr.append("<td>" + objret[i].MADONVI + "</td>");
                        tr.append("<td>" + objret[i].SODIENTHOAI + "</td>");
                        tr.append("<td>" + objret[i].MA_YEUCAU + "</td>");
                        tr.append("<td>" + objret[i].NOIDUNG_YEUCAU + "</td>");
                        tr.append("<td>" + objret[i].NGAYTAO + "</td>");
                        tr.append("<td>" + objret[i].NGUOITAO + "</td>");
                        tr.append("<td>" + objret[i].TENDAYDU + "</td>");
                        tr.append("<td>" + objret[i].TENDONVI + "</td>");
                        tr.append("<td>" + objret[i].TENHETHONG + "</td>");
                        tr.append("<td>" + objret[i].MA_HETHONG + "</td>");
                        tr.append("<td>" + objret[i].LINHVUC + "</td>");
                        tr.append("<td>" + objret[i].MA_LINHVUC + "</td>");
                        tr.append("<td>" + objret[i].TENMUCDO + "</td>");
                        tr.append("<td>" + objret[i].TEN_LUONG + "</td>");
                        tr.append("<td>" + objret[i].DONVIPHOIHOP + "</td>");
                        tr.append("<td><a onclick=\"xemLuongXuLyYC('" + objret[i].ID + "')\" style=\"cursor: pointer;\"><img src='../../HTHTKT/icons/view_16x16.gif' alt='Smiley face'> Xem</a></td>");
                        $("#noiDungTraCuu tbody").append(tr); 
                    }
                    //if (val2 == 1) {
                    //    aspxPopupTimelineXuLyHoTro.ShowAtPos(100, 100);
                    //    loadDonViChuyenDenTiepTheoByIDYeuCauHoTro(val);
                    //} else if (val2 == 2) {
                    //    aspxPopupTimelineXuLyHoTroNodeCuoi.ShowAtPos(100, 100);
                    //}
                    if (objret.length > 0) {
                        $('#hienThiTongSoBanGhi').show();
                        $('#tongSoBanGhi').html(objret.length);
                    }
                    loadingdata.Hide();
                },
                error: function () {
                    loadingdata.Hide();
                    alert('có lỗi xảy ra khi lấy dữ liệu tra cứu.');
                }
            });
        }
        function checkValid() {
            if (txtTuNgay.GetText() == null || txtTuNgay.GetText() == '') {
                alert('Bạn phải nhập từ ngày!');
                return false;
            }
            if (txtDenNgay.GetText() == null || txtDenNgay.GetText() == '') {
                alert('Bạn phải nhập đến ngày!');
                return false;
            }
            //if (ddlChonDonViAdd.GetValue() == null || ddlChonDonViAdd.GetValue() == '' || ddlChonDonViAdd.GetValue() == 0) {
            //    alert('Bạn phải chọn đơn vị!');
            //    return false;
            //}
            return true;
        }




        
        function BindUserData(PageIndex, PageSize, ImageID,sothuebao, hethong, linhvuc, mucdosuco, tungay, denngay, iddonvi, nguoidung,mayeucau) {
            var StartIndex = ((PageIndex - 1) * PageSize) + 1 - 1;
            var SortingOrder = "";

            switch (ImageID) {
                case "imgIdUp":
                    SortingOrder = "ORDER BY User_id ASC";
                    break;
                case "imgIdDown":
                    SortingOrder = "ORDER BY User_id DESC";
                    break;
                case "imgNameUp":
                    SortingOrder = "ORDER BY Name ASC";
                    break;
                case "imgNameDown":
                    SortingOrder = "ORDER BY Name DESC";
                    break;
                case "imgCityUp":
                    SortingOrder = "ORDER BY City ASC";
                    break;
                case "imgCityDown":
                    SortingOrder = "ORDER BY City DESC";
                    break;
                case "imgEmailUp":
                    SortingOrder = "ORDER BY email ASC";
                    break;
                case "imgEmailDown":
                    SortingOrder = "ORDER BY email DESC";
                    break;
                default:
                    SortingOrder = ImageID;
                    break;
            }

            if (SortingOrder != "") {
                document.getElementById('hdnSortingOrder').value = SortingOrder;
            }
            else {
                SortingOrder = "ORDER BY User_id ASC";
            }

            $.ajax({
                type: "POST",
                url: '/HeThongHoTro/Services/ws_baoCao.asmx/bc_traCuuThongTinTongHop2',
                data: "{StartIndex:'" + (PageIndex-1) + "',PageSize:'" + PageSize + "',SortingOrder:'" + SortingOrder + "',sothuebao:'" + sothuebao + "',hethong: '" + hethong + "',linhvuc: '" + linhvuc + "',mucdosuco: '" + mucdosuco + "',tungay: '" + tungay + "',denngay: '" + denngay + "',iddonvi: '" + iddonvi + "',nguoidung: '" + nguoidung + "',mayeucau: '" + mayeucau + "' }",
                contentType: "application/json;charset=utf-8",
                dataType: "json",
                success: function (jsonData) {
                    $('#divAllUsers').empty();
                    $('#divAllUsers').append("<thead><tr>" +
                        "<th>Id</th>" +
                        "<th>Số điện thoại</th>" +
                        "<th>Mã yêu cầu</th>" +
                        "<th>Nội dung y/c</th>" +

                        "<th>Ngày tạo</th>" +
                        "<th>Người tạo</th>" +
                        "<th>Tên đầy đủ</th>" +
                        "<th>Đơn vị tạo</th>" +


                        "<th>Tên hệ thống</th>" +
                        "<th>Mã hệ thống</th>" +
                        "<th>Linhc vực</th>" +
                        "<th>Mã lĩnh vực</th>" +


                        "<th>Mức độ yêu cầu</th>" +
                        "<th>Luồng xử lý</th>" +
                        "<th>Đơn vị phối hợp</th>" +
                        "<th>#</th>" +



                        "</tr></thead>");

                    var objret = JSON.parse(jsonData.d);


                    if (objret.length > 0) {
                        var LastData = objret.length - 1;
                        for (var i = 0; i < objret.length; i++) {
                            $('#divAllUsers').append("<tr>" +
                                "<td>" + objret[i].ID + "</td>" +
                                "<td>" + objret[i].SODIENTHOAI + "</td>" +
                                "<td>" + objret[i].MA_YEUCAU + "</td>" +
                                "<td>" + objret[i].NOIDUNG_YEUCAU + "</td>" +
                                "<td>" + objret[i].NGAYTAO + "</td>" +
                                "<td>" + objret[i].NGUOITAO + "</td>" +
                                "<td>" + objret[i].TENDAYDU + "</td>" +
                                "<td>" + objret[i].TENDONVI + "</td>" +
                                "<td>" + objret[i].TENHETHONG + "</td>" +
                                "<td>" + objret[i].MA_HETHONG + "</td>" +
                                "<td>" + objret[i].LINHVUC + "</td>" +
                                "<td>" + objret[i].MA_LINHVUC + "</td>" +
                                "<td>" + objret[i].TENMUCDO + "</td>" +
                                "<td>" + objret[i].TEN_LUONG + "</td>" +
                                "<td>" + objret[i].DONVIPHOIHOP + "</td>" +
                                "<td><a onclick=\"xemLuongXuLyYC('" + objret[i].ID + "')\" style=\"cursor: pointer;\"><img src='../../HTHTKT/icons/view_16x16.gif' alt='Smiley face'> Xem</a></td>" +
                                "</tr>");
                        }
                        document.getElementById('hdnEndingIndex').value = objret[LastData].CountData;
                        document.getElementById('hdnStartingIndex').value = objret[0].CountData;
                        document.getElementById('hdnLastPage').value = objret[0].NumberOfPage;
                        document.getElementById('hdnCurrentPage').value = PageIndex;

                        var AppendDiv = "<tr><td colspan=16><table><tr><td colspan=3>Hiển thị từ <div class=TradesResult>" + objret[0].CountData + "</div> đến <div class=TradesResult>" + objret[LastData].CountData + "</div> trong tổng số <div class=TradesResult>" + objret[LastData].ToltalRecords + "</div> bản ghi </td>" +
                            "<td colspan=1>Hiển thị <select id=ddlPageSize onchange=BindUserData(1,this.value,document.getElementById('hdnSortingOrder').value,'"+sothuebao+"','"+ hethong+"','"+ linhvuc+"','"+ mucdosuco+"','"+ tungay+"','"+ denngay+"','"+ iddonvi+"','"+ nguoidung+"','"+mayeucau+"');><option value=2>2</option><option value=5>5</option><option value=10>10</option><option value=25>25</option><option value=50>50</option><option value=100>100</option></select> bản ghi </td>" +
                            "<td colspan=12><table><tr>" +
                            "<td><a class=PageLink id=LinkFirst onclick=BindUserData(1,document.getElementById('ddlPageSize').options[document.getElementById('ddlPageSize').selectedIndex].value,document.getElementById('hdnSortingOrder').value,'"+sothuebao+"','"+ hethong+"','"+ linhvuc+"','"+ mucdosuco+"','"+ tungay+"','"+ denngay+"','"+ iddonvi+"','"+ nguoidung+"','"+mayeucau+"'); style=cursor:pointer;>Đầu</a><div id=divFirst style=display:none;color:silver;>Đầu</div></td>" +
                            "<td><div style=padding-left:10px;display:inline;></div></td>" +
                            "<td><a class=PageLink id=LinkPrev onclick=BindUserData(parseInt(document.getElementById('hdnCurrentPage').value)-1,document.getElementById('ddlPageSize').options[document.getElementById('ddlPageSize').selectedIndex].value,document.getElementById('hdnSortingOrder').value,'"+sothuebao+"','"+ hethong+"','"+ linhvuc+"','"+ mucdosuco+"','"+ tungay+"','"+ denngay+"','"+ iddonvi+"','"+ nguoidung+"','"+mayeucau+"'); style=cursor:pointer;>trước</a><div id=divPrev style=display:none;color:silver;>trước</div></td>" +
                            "<td><div style=padding-left:10px;display:inline;></div></td><td>";

                        if (parseInt(objret[0].NumberOfPage) - PageIndex >= 5) {
                            if (parseInt(PageIndex) - 5 >= 0) {
                                for (var j = parseInt(PageIndex) - 5 + 1; j < parseInt(PageIndex) + 5; j++) {
                                    AppendDiv += "<a class=PageLink id=" + j + " onclick=BindUserData(this.id,document.getElementById('ddlPageSize').options[document.getElementById('ddlPageSize').selectedIndex].value,document.getElementById('hdnSortingOrder').value,'" + sothuebao + "','" + hethong + "','" + linhvuc + "','" + mucdosuco + "','" + tungay + "','" + denngay + "','" + iddonvi + "','" + nguoidung + "','" + mayeucau + "');>   " + j + "   </a><div class=divCurrentPage id=divCurrentPage" + j + " style=display:none;color:silver;>" + j + "</div>";
                                }
                            }
                            else {
                                for (var j = 1; j < parseInt(PageIndex) + 10; j++) {
                                    AppendDiv += "<a class=PageLink id=" + j + " onclick=BindUserData(this.id,document.getElementById('ddlPageSize').options[document.getElementById('ddlPageSize').selectedIndex].value,document.getElementById('hdnSortingOrder').value,'" + sothuebao + "','" + hethong + "','" + linhvuc + "','" + mucdosuco + "','" + tungay + "','" + denngay + "','" + iddonvi + "','" + nguoidung + "','" + mayeucau + "');>   " + j + "   </a><div class=divCurrentPage id=divCurrentPage" + j + " style=display:none;color:silver;>" + j + "</div>";
                                }
                            }
                        }
                        else {

                            var sl_in_page = parseInt(objret[0].NumberOfPage) + 10 - PageIndex;
                            if (parseInt(objret[0].NumberOfPage) - 10 > 0) {
                                for (var j = parseInt(objret[0].NumberOfPage) - 10; j <= parseInt(objret[0].NumberOfPage); j++) {
                                    AppendDiv += "<a class=PageLink id=" + j + " onclick=BindUserData(this.id,document.getElementById('ddlPageSize').options[document.getElementById('ddlPageSize').selectedIndex].value,document.getElementById('hdnSortingOrder').value,'" + sothuebao + "','" + hethong + "','" + linhvuc + "','" + mucdosuco + "','" + tungay + "','" + denngay + "','" + iddonvi + "','" + nguoidung + "','" + mayeucau + "');>   " + j + "   </a><div class=divCurrentPage id=divCurrentPage" + j + " style=display:none;color:silver;>" + j + "</div>";
                                }
                            }
                            else {
                                for (var j = 1; j <= parseInt(objret[0].NumberOfPage); j++) {
                                    AppendDiv += "<a class=PageLink id=" + j + " onclick=BindUserData(this.id,document.getElementById('ddlPageSize').options[document.getElementById('ddlPageSize').selectedIndex].value,document.getElementById('hdnSortingOrder').value,'" + sothuebao + "','" + hethong + "','" + linhvuc + "','" + mucdosuco + "','" + tungay + "','" + denngay + "','" + iddonvi + "','" + nguoidung + "','" + mayeucau + "');>   " + j + "   </a><div class=divCurrentPage id=divCurrentPage" + j + " style=display:none;color:silver;>" + j + "</div>";
                                }
                            }
                        }


                        AppendDiv += "</td><td><div style=padding-left:10px;display:inline;></div></td>" +
                            "<td><a id=LinkNext class=PageLink onclick=BindUserData(parseInt(document.getElementById('hdnCurrentPage').value)+1,document.getElementById('ddlPageSize').options[document.getElementById('ddlPageSize').selectedIndex].value,document.getElementById('hdnSortingOrder').value,'"+sothuebao+"','"+ hethong+"','"+ linhvuc+"','"+ mucdosuco+"','"+ tungay+"','"+ denngay+"','"+ iddonvi+"','"+ nguoidung+"','"+mayeucau+"'); style=cursor:pointer;>tiếp</a><div id=divNext style=display:none;color:silver;>tiếp</div></td>" +
                            "<td><div style=padding-left:10px;display:inline;></div></td>" +
                            "<td><a class=PageLink id=LinkLast onclick=BindUserData(parseInt(document.getElementById('hdnLastPage').value),document.getElementById('ddlPageSize').options[document.getElementById('ddlPageSize').selectedIndex].value,document.getElementById('hdnSortingOrder').value,'"+sothuebao+"','"+ hethong+"','"+ linhvuc+"','"+ mucdosuco+"','"+ tungay+"','"+ denngay+"','"+ iddonvi+"','"+ nguoidung+"','"+mayeucau+"'); style=cursor:pointer;>Cuối</a><div id=divLast style=display:none;color:silver;>Cuối</div></td>" +
                            "</tr></table></td></tr></table></td></tr>";

                        $('#divAllUsers').append(AppendDiv);
                        if (objret[0].NumberOfPage == 1) {
                            document.getElementById('LinkLast').style.display = 'none';
                            document.getElementById('LinkNext').style.display = 'none';
                            document.getElementById('LinkFirst').style.display = 'none';
                            document.getElementById('LinkPrev').style.display = 'none';
                            document.getElementById('divLast').style.display = 'inline';
                            document.getElementById('divNext').style.display = 'inline';
                            document.getElementById('divFirst').style.display = 'inline';
                            document.getElementById('divPrev').style.display = 'inline';
                        }
                        if (PageIndex == objret[0].NumberOfPage) {
                            document.getElementById('LinkLast').style.display = 'none';
                            document.getElementById('divLast').style.display = 'inline';
                            document.getElementById('LinkNext').style.display = 'none';
                            document.getElementById('divNext').style.display = 'inline';
                        }
                        if (PageIndex < objret[0].NumberOfPage && PageIndex == 1) {
                            document.getElementById('LinkFirst').style.display = 'none';
                            document.getElementById('divFirst').style.display = 'inline';
                            document.getElementById('LinkPrev').style.display = 'none';
                            document.getElementById('divPrev').style.display = 'inline';
                        }
                        document.getElementById('ddlPageSize').value = PageSize;
                        $('#divCurrentPage' + PageIndex).css({ 'display': 'inline', 'font-weight': 'bolder' });
                        $('#' + PageIndex).css({ 'display': 'none' });
                        $('.TradesResult').css({ 'color': 'green', 'font-weight': 'bolder', 'display': 'inline' });
                    }
                },
                error: function (result) {

                }
            });
        }




        function baocao() {
            if (!checkValid())
                return;


            var sothuebao = txtSoThueBao.GetText();
            var hethong = cboChonHeThongHoTro.GetValue();
            var linhvuc = LoaiHoTro.GetKeyValue();
            var mucdosuco = cboMucDoSuCo.GetValue();
            var tungay = txtTuNgay.GetText();
            var denngay = txtDenNgay.GetText();
            var iddonvi = ddlChonDonViAdd.GetKeyValue();
            var nguoidung = txtTenNguoiDung.GetText();
            var mayeucau = txtMaYeuCau.GetText();
            //loadBaoCao(sothuebao, hethong, linhvuc, mucdosuco, tungay, denngay, iddonvi, nguoidung,mayeucau);
            BindUserData(1, 5,1,sothuebao, hethong, linhvuc, mucdosuco, tungay, denngay, iddonvi, nguoidung,mayeucau);
        }
        function xuatExcel() {
            $('#divAllUsers').tableExport({ type: 'excel', escape: 'false' });
        }

        function xemLuongXuLyYC(val) {
            loadToanBoThongTinYeuCauHoTroTheoID(val);
            loadTimelineHoTro(val);
        }

        function loadToanBoThongTinYeuCauHoTroTheoID(val) {
            debugger;
            $.ajax({
                type: 'POST',
                url: '/HeThongHoTro/Services/ws_thongTinHoTro.asmx/loadToanBoThongTinYeuCauHoTroTheoID',
                data: "{ idchitietxlhotro: " + val + " }",
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                success: function (jsonData) {
                    var objret = JSON.parse(jsonData.d);
                    lblSoDienThoaiYC.SetText(objret.SODIENTHOAI);
                    lblMucDoYeuCauYC.SetText(objret.TENMUCDO);
                    lblTenLuongHoTroHTYC.SetText(objret.TEN_LUONG);
                    lblMoTaLuongHoTroHTYC.SetText(objret.MOTA);
                    lblTenHeThongHoTroKyThuatHTYC.SetText(objret.TENHETHONG);
                    lblNoiDungYeuCauHoTroGocHTYC.SetText(objret.NOIDUNG_YEUCAU);
                    lblLinhVucConHTYC.SetText(objret.LINHVUC);
                },
                error: function () {
                }
            });
        }

        function loadTimelineHoTro(val) {
            $.ajax({
                type: 'POST',
                url: '/HeThongHoTro/Services/ws_thongTinHoTro.asmx/loadTimeLineXuLyHT',
                data: "{ idyeucauhotro: " + val + " }",
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                beforeSend: function () {
                    loadingdata.Show();
                },
                success: function (jsonData) {
                    var objret = JSON.parse(jsonData.d);
                    $("#timeLine tbody").html("");
                    var table = '';
                    var tr;
                    for (var i = 0; i < objret.length; i++) {
                        tr = $('<tr/>');
                        tr.append("<td>" + objret[i].NGAYXULY + "</td>");
                        tr.append("<td>" + objret[i].ID_NODE_LUONG_HOTRO + "</td>");
                        tr.append("<td>" + objret[i].NOIDUNGXULY + "</td>");
                        tr.append("<td>" + objret[i].ID_DONVI_TO + "</td>");
                        tr.append("<td>" + objret[i].DONVIXULY + "</td>");
                        tr.append("<td><a onclick=\"xemThongTinChiTietDaPhuongTien('" + objret[i].ID + "')\" style=\"cursor: pointer;\"><img src='../../HTHTKT/icons/view_16x16.gif' alt='Smiley face'> Xem</a></td>");
                        tr.append("<td><a onclick=\"loadFileAttach('" + objret[i].ID + "')\" style=\"cursor: pointer;\"><img src='../../HTHTKT/icons/file_16x16.gif' alt='Smiley face'> File</a></td>");
                        $("#timeLine tbody").append(tr);
                    }
                      aspxPopupTimelineXuLyHoTro.ShowAtPos(100, 100);
                    loadingdata.Hide();
                },
                error: function () {
                    loadingdata.Hide();
                    alert('có lỗi xảy ra khi lấy timeline luồng xử lý, vui lòng thử lại sau!');
                }
            });
        }

        function xemThongTinChiTietDaPhuongTien(val) {
            //alert(val);
            //htmlXemNoiDungXLChiTiet.SetHtml('<img src="/HTHTKT/UploadFiles/Images/2017-05-09_10-38-27(4).png" alt=""/>');
            $.ajax({
                type: 'POST',
                url: '/HeThongHoTro/Services/ws_xemThongTin.asmx/thongTinChiTietXuLyDaPhuongTien',
                data: "{ id: " + val + " }",
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                success: function (jsonData) {
                    var objret = JSON.parse(jsonData.d);
                    htmlXemNoiDungXLChiTiet.SetHtml(objret[0].NOIDUNGXLCHITIET);
                },
                error: function () {
                }
            });
            popupXemNoiDungDaPhuongTien.Show();
        }

        function loadFileAttach(val) {
                    popupDanhSachFileAttach.Show();

                    $.ajax({
                        type: 'POST',
                        url: '/HeThongHoTro/Services/ws_taptindinhkem.asmx/danhSachFileAttach',
                        data: "{ id_xuly_ycht: " + val + " }",
                        contentType: 'application/json; charset=utf-8',
                        dataType: 'json',
                        beforeSend: function () {
                            loadingdata.Show();
                        },
                        success: function (jsonData) {
                            var objret = JSON.parse(jsonData.d);
                            $("#dsFileAttach tbody").html("");
                            var table = '';
                            var tr;
                            for (var i = 0; i < objret.length; i++) {
                                tr = $('<tr/>');
                                tr.append("<td><strong>" + (i + 1) + "<strong></td>");
                                tr.append("<td><strong>" + objret[i].TENFILE + "<strong></td>");
                                tr.append("<td>" + objret[i].NGAYTAO + "</td>");
                                tr.append("<td><a href=\"/HTHTKT/UploadAttachFiles/" + objret[i].TENFILETAIVE + "\" style=\"cursor: pointer;\"><img src='../../HTHTKT/icons/check-in_16x16.gif' alt='Smiley face'> Tải về</a></td>");
                                $("#dsFileAttach tbody").append(tr);
                            }
                            loadingdata.Hide();
                        },
                        error: function () {
                            loadingdata.Hide();
                            alert('có lỗi xảy ra khi lấy thông tin file attach');
                        }
                    });
                }


    </script>
    
    <div style="padding: 10px;">
        <div class="container-fluid">
            <div class="row">
                <div class="col-md-12">
                    <div class="panel panel-primary">
                        <div class="panel-heading">Tra cứu thông tin yêu cầu hỗ trợ</div>
                        <div class="panel-body">
                            <table class="table_baocao" width="100%">
                                <tr>
                                    <td>Số thuê bao
                                    </td>
                                    <td>
                                        <dx:ASPxTextBox ID="txtSoThueBao" runat="server" ClientInstanceName="txtSoThueBao" Theme="Office2010Blue">
                                        </dx:ASPxTextBox>
                                    </td>
                                    <td>Hệ thống
                                    </td>
                                    <td>
                                        <dx:ASPxComboBox ID="cboChonHeThongHoTro" ClientInstanceName="cboChonHeThongHoTro" runat="server" ValueType="System.String" Theme="Office2010Blue"
                                            AutoPostBack="false">
                                            <ClientSideEvents SelectedIndexChanged="OnListBoxIndexChanged" />
                                        </dx:ASPxComboBox>
                                    </td>
                                    <td>Lĩnh vực
                                    </td>
                                    <td>
                                        <dx:ASPxCallbackPanel ID="ASPxCallbackPanel1" runat="server" Width="200px"
                                            ClientInstanceName="CallbackPanel">
                                            <ClientSideEvents EndCallback="OnEndCallback"></ClientSideEvents>
                                            <PanelCollection>
                                                <dx:PanelContent ID="PanelContent3" runat="server">
                                                    <dx:ASPxDropDownEdit ID="LoaiHoTro" runat="server" Theme="Office2010Blue" ClientInstanceName="LoaiHoTro" AnimationType="None" Width="100%">
                                                        <DropDownWindowTemplate>
                                                            <div>
                                                                <dx:ASPxTreeList ID="treelist_donvi_cc" runat="server" Width="350px" ClientInstanceName="treelist_donvi_cc"
                                                                    Theme="Aqua" Font-Names="arial" Font-Size="12px" KeyFieldName="id"
                                                                    OnVirtualModeCreateChildren="treelist_donvi_cc_VirtualModeCreateChildren"
                                                                    OnVirtualModeNodeCreating="treelist_donvi_cc_VirtualModeNodeCreating"
                                                                    OnCustomCallback="treelist_donvi_cc_OnCustomCallback" AutoGenerateColumns="False">
                                                                    <SettingsBehavior AllowFocusedNode="true" FocusNodeOnExpandButtonClick="false" FocusNodeOnLoad="false" />
                                                                    <Settings HorizontalScrollBarMode="Visible" ShowTreeLines="true" SuppressOuterGridLines="true" VerticalScrollBarMode="Visible" />
                                                                    <ClientSideEvents FocusedNodeChanged="ChonDonVi" NodeExpanding="function(s, e) {
	                                                                      IDLoaiHoTro2.Set('hidden_value2', 0);
                                                                    }" />
                                                                    <Columns>
                                                                        <dx:TreeListTextColumn FieldName="linhvuc" Caption="Lĩnh vực" Width="350px">
                                                                        </dx:TreeListTextColumn>
                                                                    </Columns>
                                                                </dx:ASPxTreeList>
                                                            </div>
                                                        </DropDownWindowTemplate>
                                                    </dx:ASPxDropDownEdit>
                                                </dx:PanelContent>
                                            </PanelCollection>
                                        </dx:ASPxCallbackPanel>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Mức độ sự cố
                                    </td>
                                    <td>
                                        <dx:ASPxComboBox ID="cboMucDoSuCo" runat="server" ClientInstanceName="cboMucDoSuCo" Theme="Office2010Blue">
                                        </dx:ASPxComboBox>
                                    </td>

                                    <td>Từ ngày
                                    </td>
                                    <td>
                                        <dx:ASPxDateEdit ID="ASPxDateEdit1" DisplayFormatString="dd/MM/yyyy" EditFormatString="dd/MM/yyyy" ClientInstanceName="txtTuNgay" runat="server" Theme="Office2010Silver">
                                        </dx:ASPxDateEdit>
                                    </td>
                                    <td>Đến ngày
                                    </td>
                                    <td>
                                        <dx:ASPxDateEdit ID="ASPxDateEdit2" DisplayFormatString="dd/MM/yyyy" EditFormatString="dd/MM/yyyy" ClientInstanceName="txtDenNgay" runat="server" Theme="Office2010Silver">
                                        </dx:ASPxDateEdit>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Đơn vị&nbsp;</td>
                                    <td>
                                        <dx:ASPxDropDownEdit ID="ddlChonDonViAdd" runat="server" Theme="Office2010Blue"
                                            ClientInstanceName="ddlChonDonViAdd" AnimationType="None" Width="100%">
                                            <DropDownWindowTemplate>
                                                <div style="height: 200px; overflow: scroll;">
                                                    <dx:ASPxTreeList runat="server" ID="treeListDanhSachDonViAdd" AutoGenerateColumns="False"
                                                        ClientInstanceName="treeListDanhSachDonViAdd" Width="350px" EnableTheming="True"
                                                        KeyFieldName="ID"
                                                        OnCustomCallback="treeListDanhSachDonVi_CustomCallback"
                                                        OnHtmlDataCellPrepared="treeListDanhSachDonVi_HtmlDataCellPrepared"
                                                        OnVirtualModeCreateChildren="treeListDanhSachDonVi_VirtualModeCreateChildren"
                                                        OnVirtualModeNodeCreated="treeListDanhSachDonVi_VirtualModeNodeCreated"
                                                        OnVirtualModeNodeCreating="treeListDanhSachDonVi_VirtualModeNodeCreating"
                                                        Theme="Aqua" EnablePagingGestures="False">
                                                        <SettingsBehavior AllowFocusedNode="true" FocusNodeOnExpandButtonClick="false" FocusNodeOnLoad="false" />
                                                        <Columns>
                                                            <dx:TreeListTextColumn Caption="ID" FieldName="ID" VisibleIndex="2" Visible="false">
                                                            </dx:TreeListTextColumn>
                                                            <dx:TreeListTextColumn Caption="Mã đơn vị" FieldName="MADONVI" VisibleIndex="1">
                                                            </dx:TreeListTextColumn>
                                                            <dx:TreeListTextColumn Caption="Tên đơn vị" FieldName="TENDONVI" VisibleIndex="0" CellStyle-HorizontalAlign="Left">
                                                            </dx:TreeListTextColumn>
                                                        </Columns>
                                                        <ClientSideEvents FocusedNodeChanged="chonDonViAdd" />
                                                    </dx:ASPxTreeList>
                                                </div>
                                            </DropDownWindowTemplate>
                                        </dx:ASPxDropDownEdit>
                                    </td>
                                    <td>Người dùng</td>
                                    <td>
                                        <dx:ASPxTextBox ID="txtTenNguoiDung" runat="server" ClientInstanceName="txtTenNguoiDung" Theme="Office2010Blue" Width="170px">
                                        </dx:ASPxTextBox>
                                    </td>
                                    <td>Mã yêu cầu</td>
                                    <td><dx:ASPxTextBox ID="txtMaYeuCau" runat="server" ClientInstanceName="txtMaYeuCau" Theme="Office2010Blue" Width="170px">
                                        </dx:ASPxTextBox></td>
                                </tr>
                                <tr>
                                    <td>&nbsp;</td>
                                    <td>
                                        <dx:ASPxLoadingPanel ID="ASPxLoadingPanel1" ClientInstanceName="loadingdata" runat="server" Modal="True" Text="Đang tải&amp;hellip;" Theme="Office2010Silver">
                                        </dx:ASPxLoadingPanel>
                                    </td>
                                    <td>
                                        <a onclick="baocao()" class="btn btn-primary"><i class="fa fa-pie-chart" aria-hidden="true"></i>Tra cứu</a>
                                        <a onclick="xuatExcel()" class="btn btn-success"><i class="fa fa-file-excel-o" aria-hidden="true"></i>Xuất Excel</a>
                                    </td>
                                    <td>&nbsp;</td>
                                    <td></td>
                                    <td></td>
                                </tr>
                            </table>
                        </div>
                        <div style="padding: 5px; display: none" id="hienThiTongSoBanGhi">Có tổng số: <span class="badge"><span id="tongSoBanGhi"></span></span></div>
                        <div class="table-responsive" style="height: 500px">
                            <table id="noiDungTraCuu" class="table table-hover table-bordered table-striped" style="display:none">
                                <thead>
                                    <tr>
                                        <th>Id</th>
                                        <%--<th>Mã đơn vị</th>--%>
                                        <th>Số điện thoại</th>
                                        <th>Mã yêu cầu</th>
                                        <th>Nội dung y/c</th>
                                        <th>Ngày tạo</th>
                                        <th>Người tạo</th>
                                        <th>Tên đầy đủ</th>
                                        <th>Đơn vị tạo</th>
                                        <th>Tên hệ thống</th>
                                        <th>Mã hệ thống</th>
                                        <th>Linhc vực</th>
                                        <th>Mã lĩnh vực</th>
                                        <th>Mức độ yêu cầu</th>
                                        <th>Luồng xử lý</th>
                                        <th>Đơn vị phối hợp</th>
                                        <th>#</th>
                                    </tr>
                                </thead>
                                <tbody></tbody>
                            </table>



                            <asp:HiddenField ID="hdnStartingIndex" ClientIDMode="Static" runat="server" />
                            <asp:HiddenField ID="hdnEndingIndex" ClientIDMode="Static" runat="server" />
                            <asp:HiddenField ID="hdnCurrentPage" ClientIDMode="Static" runat="server" />
                            <asp:HiddenField ID="hdnLastPage" ClientIDMode="Static" runat="server" />
                            <asp:HiddenField ID="hdnSortingOrder" ClientIDMode="Static" runat="server" />
                            <table id="divAllUsers" class="table table-hover table-bordered table-striped">
                            </table>

                        </div>
                        <%-- <div class="panel-footer"></div>--%>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <dx:ASPxPopupControl ID="aspxPopupTimelineXuLyHoTro" runat="server" CloseAction="CloseButton" HeaderText="Thông tin xử lý hỗ trơ" Height="192px" Theme="Office2010Silver" Width="800px"
        ClientInstanceName="aspxPopupTimelineXuLyHoTro" AllowDragging="True" Modal="True" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" AllowResize="True" ShowCollapseButton="True" ShowMaximizeButton="True" ShowPinButton="True">
        <ContentCollection>
            <dx:PopupControlContentControl runat="server">
                <table style="width: 100%;" class="tblKhaiBaoHT">
                    <tr>
                        <td class="labelhienthi">Thông tin luồng xử lý:</td>
                        <td>&nbsp;
                                <dx:ASPxLabel ID="lblTenLuongHoTroHTYC" ClientInstanceName="lblTenLuongHoTroHTYC" runat="server" Text="lblTenLuongHoTro">
                                </dx:ASPxLabel>
                        </td>
                        <td>&nbsp;</td>
                    </tr>
                    <tr>
                        <td class="labelhienthi">Mô tả luồng xử lý:</td>
                        <td>&nbsp;
                                    <dx:ASPxLabel ID="lblMoTaLuongHoTroHTYC" runat="server" ClientInstanceName="lblMoTaLuongHoTroHTYC" Text="lblMoTaLuongHoTroHTYC">
                                    </dx:ASPxLabel>
                        </td>
                        <td>&nbsp;</td>
                    </tr>
                    <tr>
                        <td class="labelhienthi">Hệ thống hỗ trợ kỹ thuật:</td>
                        <td>&nbsp;
                                    <dx:ASPxLabel ID="lblTenHeThongHoTroKyThuatHTYC" runat="server" ClientInstanceName="lblTenHeThongHoTroKyThuatHTYC" Text="lblTenHeThongHoTroKyThuatHTYC">
                                    </dx:ASPxLabel>
                        </td>
                        <td>&nbsp;</td>
                    </tr>
                    <tr>
                        <td class="labelhienthi">Lĩnh vực:</td>
                        <td>&nbsp;
                                    <dx:ASPxLabel ID="lblLinhVucConHTYC" runat="server" ClientInstanceName="lblLinhVucConHTYC" Text="lblLinhVucConHTYC">
                                    </dx:ASPxLabel>
                        </td>
                        <td>&nbsp;</td>
                    </tr>
                    <tr>
                        <td class="labelhienthi">Số điện thoại:</td>
                        <td>&nbsp;
                        <dx:ASPxLabel ID="lblSoDienThoaiYC" runat="server" ClientInstanceName="lblSoDienThoaiYC" Text="ASPxLabel">
                        </dx:ASPxLabel>
                        </td>
                        <td>&nbsp;</td>
                    </tr>
                    <tr>
                        <td class="labelhienthi">Mức độ yêu cầu:</td>
                        <td>
                            <dx:ASPxLabel ID="lblMucDoYeuCauYC" runat="server" ClientInstanceName="lblMucDoYeuCauYC" Text="ASPxLabel">
                            </dx:ASPxLabel>
                        </td>
                        <td>&nbsp;</td>
                    </tr>
                    <tr>
                        <td class="labelhienthi">Nội dung yêu cầu</td>
                        <td>&nbsp;
                                <dx:ASPxLabel ID="lblNoiDungYeuCauHoTroGocHTYC" ClientInstanceName="lblNoiDungYeuCauHoTroGocHTYC" runat="server" Text="lblNoiDungYeuCauHoTroGocHTYC">
                                </dx:ASPxLabel>
                        </td>
                        <td>&nbsp;</td>
                    </tr>
                    <tr>
                        <td class="labelhienthi" style="color: darkred">Dòng sự kiện xử lý</td>
                        <td>&nbsp;</td>
                        <td>&nbsp;</td>
                    </tr>
                    <tr>
                        <td colspan="3">
                            <div class="table-responsive" style="height: 200px">
                                <table id="timeLine" class="table table-hover table-striped">
                                    <%--header-fixed--%>
                                    <thead>
                                        <tr>
                                            <th>Ngày</th>
                                            <th>idnode </th>
                                            <th>Nội dung y/c</th>
                                            <th>id dv</th>
                                            <th>Đơn vị xử lý</th>
                                            <th>Xem chi tiết</th>
                                            <th>#</th>
                                        </tr>
                                    </thead>
                                    <tbody></tbody>
                                </table>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td>&nbsp;</td>
                        <td>&nbsp;</td>
                        <td>&nbsp;</td>
                    </tr>
                    <tr>
                        <td></td>
                        <td>&nbsp;</td>
                        <td>
                            <dx:ASPxButton ID="ASPxButton2" runat="server" AutoPostBack="False" Text="Đóng" Theme="Office2010Blue">
                                <ClientSideEvents Click="function(s, e) {
	                                        aspxPopupTimelineXuLyHoTro.Hide();
                                        }" />
                                <Image Url="~/HTHTKT/icons/delete_16x16.gif">
                                </Image>
                            </dx:ASPxButton>
                        </td>
                    </tr>
                </table>
            </dx:PopupControlContentControl>
        </ContentCollection>
    </dx:ASPxPopupControl>

                <br />
            <dx:ASPxPopupControl ID="popupXemNoiDungDaPhuongTien" runat="server" ClientInstanceName="popupXemNoiDungDaPhuongTien"
                HeaderText="Nội dung chi tiết" Theme="Office2010Blue"
                Width="500px" AllowDragging="True" CloseAction="CloseButton" Modal="True"
                PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" AllowResize="True"
                ShowCollapseButton="True" ShowMaximizeButton="True" ShowPinButton="True">
                <ContentCollection>
                    <dx:PopupControlContentControl runat="server">
                        <div>
                            <div class="container" style="width: 670px">
                                <div class="row">
                                    <div class="col-md-12">
                                        <dx:ASPxHtmlEditor ID="htmlXemNoiDungXLChiTiet" runat="server" Width="645px" Height="360px" Theme="Office2010Blue" ClientInstanceName="htmlXemNoiDungXLChiTiet">
                                            <SettingsHtmlEditing AllowHTML5MediaElements="true" AllowObjectAndEmbedElements="true" AllowYouTubeVideoIFrames="true" />
                                            <Toolbars>
                                                <dx:HtmlEditorToolbar Name="Toolbar">
                                                    <Items>
                                                        <dx:ToolbarFullscreenButton BeginGroup="true">
                                                        </dx:ToolbarFullscreenButton>
                                                    </Items>
                                                </dx:HtmlEditorToolbar>
                                            </Toolbars>

                                        </dx:ASPxHtmlEditor>

                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-10">&nbsp;</div>
                                    <div class="col-md-2" style="padding-top: 10px;">
                                        <dx:ASPxButton ID="btnDongXem" runat="server" AutoPostBack="false" Text="Đóng" Theme="Office2010Blue">
                                            <ClientSideEvents Click="function(s, e) {
	                                            popupXemNoiDungDaPhuongTien.Hide();
                                            }" />
                                            <Image Url="~/HTHTKT/icons/delete_16x16.gif"></Image>
                                        </dx:ASPxButton>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </dx:PopupControlContentControl>
                </ContentCollection>
            </dx:ASPxPopupControl>



    <dx:ASPxPopupControl ID="popupDanhSachFileAttach" runat="server" ClientInstanceName="popupDanhSachFileAttach" HeaderText="Danh sách tệp tin đính kèm" Height="300px" Theme="Office2010Blue" Width="500px" AllowDragging="True" CloseAction="CloseButton" Modal="True" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter">
    <ContentCollection>
        <dx:PopupControlContentControl runat="server">
            <table style="width: 100%;">
                <tr>
                    <td>&nbsp;</td>
                    <td>&nbsp;</td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td colspan="3">
                        <div class="table-responsive" style="height: 200px">
                            <table id="dsFileAttach" class="table table-hover table-striped">
                                <%--header-fixed--%>
                                <thead>
                                    <tr>
                                        <th>STT</th>
                                        <th>Tên tập tin</th>
                                        <th>Ngày cập nhật</th>
                                        <th>Tải về</th>
                                    </tr>
                                </thead>
                                <tbody></tbody>
                            </table>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                    <td>&nbsp;</td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                    <td>&nbsp;</td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                    <td>&nbsp;</td>
                    <td style="text-align: right">
                        <dx:ASPxButton ID="ASPxButton63334" runat="server" Text="Đóng" Theme="Office2010Blue" AutoPostBack="False">
                            <ClientSideEvents Click="function(s, e) {
	popupDanhSachFileAttach.Hide();
}" />
                            <Image Url="~/HTHTKT/icons/delete_16x16.gif">
                            </Image>
                        </dx:ASPxButton>
                    </td>
                </tr>
            </table>
        </dx:PopupControlContentControl>
    </ContentCollection>
</dx:ASPxPopupControl>



    <dx:ASPxHiddenField ID="ASPxHiddenField1" runat="server" ClientInstanceName="IDLoaiHoTro">
    </dx:ASPxHiddenField>
    <dx:ASPxHiddenField ID="ASPxHiddenField3" runat="server" ClientInstanceName="IDLoaiHoTro2">
    </dx:ASPxHiddenField>
    <dx:ASPxHiddenField ID="ASPxHiddenField2" runat="server" ClientInstanceName="isReshresd">
    </dx:ASPxHiddenField>
    <dx:ASPxHiddenField ID="ThongHTKTID" runat="server" ClientInstanceName="ThongHTKTID">
    </dx:ASPxHiddenField>
    <dx:ASPxHiddenField ID="LoaiHTKTID" runat="server" ClientInstanceName="LoaiHTKTID">
    </dx:ASPxHiddenField>
    <dx:ASPxHiddenField ID="NoiChuyenDenID" runat="server" ClientInstanceName="NoiChuyenDenID">
    </dx:ASPxHiddenField>
    <dx:ASPxHiddenField ID="aspxHiddenIDLuongHoTro" runat="server" ClientInstanceName="aspxHiddenIDLuongHoTro">
    </dx:ASPxHiddenField>

    <dx:ASPxHiddenField runat="server" ID="HiddenField" ClientInstanceName="HiddenField"></dx:ASPxHiddenField>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="Content" runat="server">
</asp:Content>
