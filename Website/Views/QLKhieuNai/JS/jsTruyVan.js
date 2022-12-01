dataTruyVan = { items: [] };

//Lọc
var isLoadFlex = false;
var number = 0;
var keyTotal = 0;
var keyGetHTML = 0;
var keyExcel = 0;

function fnGetDataJson() {
    var tieuDe = $("#DropParam option:selected").text();
    //var kieuDuLieu = $("#DropParam").val();
    var arr = $("#DropParam").val().split("#");
    var kieuDuLieu = arr[0];
    var tenTruong = arr[1];
    var tinhThanhId = $("#hidtinhthanhid").val();
    var loaikhieunai0id = $("#hidloaikhieunai0id").val();
    var loaikhieunai1id = $("#hidloaikhieunai1id").val();
    var loaikhieunai2id = $("#hidloaikhieunai2id").val();
    var pheptoan = $("#DropPhepToan").val();
    var giaTri = '';
    var dataJson = '{"object_list":[';
    dataTruyVan.items.push({ TenTruong: tenTruong, TieuDe: tieuDe, KieuDuLieu: kieuDuLieu, PhepToan: pheptoan, GiaTri: giaTri, TinhThanhId: tinhThanhId, Loaikhieunai0id: loaikhieunai0id, Loaikhieunai1id: loaikhieunai1id, Loaikhieunai2id: loaikhieunai2id });
    $.each(dataTruyVan.items, function (i, item) {
        dataJson += '{ "TenTruong":' + '"' + item.TenTruong + '"' + ',' + '"TieuDe":' + '"' + item.TieuDe + '"' + ',' + '"KieuDuLieu":' + '"' + item.KieuDuLieu + '"' + ',' + '"PhepToan":'
            + '"' + item.PhepToan + '"' + ',' + '"GiaTri":' + '"' + item.GiaTri + '"' + '"TinhThanhId":' + '"' + item.TinhThanhId + '"' + '"Loaikhieunai0id":' + '"' + item.Loaikhieunai0id + '"' + '"Loaikhieunai1id":' + '"' + item.Loaikhieunai1id + '"' + '"Loaikhieunai2id":' + '"' + item.Loaikhieunai2id + '"' + '}' + ',';
    });
    dataJson += ']}'
    return dataJson.replace(",]}", "]}");
}

//Lọc
function fnTruyVan() {

    if (dataTruyVan.items.length > 0) {
        $("#rowListData").css("display", "block");
        var optInit = getOptionsFromForm();
        var dataJson = fnGetDataJson();
        $.post('/Views/QLKhieuNai/Handler/HandlerTruyVan.ashx?key=2' + '&startPageIndex=1&pageSize=' + pageSize, { data: dataJson },
            function (totalRecords) {
                if (totalRecords != '') {
                    if (totalRecords == 0) {
                        $("#Pagination").pagination(0, optInit);
                    }
                    else {
                        $("#Pagination").pagination(totalRecords, optInit);
                    }
                    $("#divTotalRecords").html('Tổng số bản ghi:' + " <span style=\"color: #FF0000;\">(" + addCommas(totalRecords) + ")</span>");
                }

            });
    } else {
        $("#rowListData").css("display", "none");
        MessageAlert.AlertNormal("Vui lòng chọn tham số tìm kiếm");
    }
}

function fnSetSizeDiv() {
    var d = $('body').innerWidth() - 56;
    var h = screen.height;
    $("#divScroll").css("width", d);
    $(".divOpacity").css("height", h);

}

function pageselectCallback(page_index) {
    var curentPages = page_index + 1;
    var param = [
           { name: 'pageSize', value: pageSize },
           { name: 'startPageIndex', value: 1 }];

    // Lấy dữ liệu
    var dataJson = fnGetDataJson();

    // Trạng thái chưa load Flex
    if (isLoadFlex) {
        var param = [
            { name: 'pageSize', value: pageSize },
            { name: 'startPageIndex', value: curentPages },
        { name: 'JSONParam', value: dataJson }];
        $('.flex_KNChoXuLy').flexOptions({ params: param }).flexReload();
    }
    else {
        var urlQuery = '/Views/QLKhieuNai/Handler/HandlerTruyVan.ashx?key=1';
        $(".flex_KNChoXuLy").flexigrid({
            url: urlQuery,
            params: param,
            dataType: 'json',
            colModel: strConfigColumn,
            sortname: "LDate",
            sortorder: "desc",
            useStatusBar: true,
            pagestat: "",
            rp: 500,
            callFunctionAfterReload: function () {
                $('a.normalTip').aToolTip();
            },
            callFunctionUpdateColumn: function (before, after) {
                updateColumnFlex('TruyVan', before, after);
            },
            width: "auto",
            height: 316,
            useUpdateCol: true
        });

        isLoadFlex = true;
    }
    //return false;
}


function UpdateColumn(before, after) {
    var param = fnGetUrlParameter('ctrl');

    $.ajax({
        type: "GET",
        url: "/Views/QLKhieuNai/XMLFiles/HandlerUpdateXML.ashx",
        data: {
            ctrl: param,
            before: before,
            after: after
        },
        contentType: "application/json; charset=utf-8",
        dataType: "text",
        success: function (data) {
        }
    });
}

function updateColumnFlex(tabName, before, after) {
    $.ajax({
        type: "GET",
        url: "/Views/QLKhieuNai/XMLFiles/HandlerUpdateXML.ashx",
        data: {
            ctrl: tabName,
            before: before,
            after: after
        },
        contentType: "application/json; charset=utf-8",
        dataType: "text",
        success: function (data, a, b) {
            console.log(data);
        },
        error: function (a, b, c) {
            if (console && console.log) { console.log(c); }
        }
    });
}

function getOptionsFromForm() {
    //var opt = { callback: pageselectCallback };
    $("input:text").each(function () {
        opt[this.name] = this.className.match(/numeric/) ? parseInt(this.value) : this.value;
    });
    return opt;
}