<%@ Page Language="C#" AutoEventWireup="true" Title="Báo cáo tổng hợp PAKN" CodeBehind="BaoCaoTongHopPAKN.aspx.cs" MasterPageFile="~/Master_Default.master" Inherits="Website.BaoCao.BaoCaoTongHopPAKN" %>

<asp:Content ContentPlaceHolderID="HeaderJs" runat="server">
    <link href="/Css/jquery.datepick.css" rel="stylesheet" type="text/css" />
    <script src="/Js/plugin/jquery.datepick.js" type="text/javascript"></script>
    <script src="/Js/plugin/jquery.datepick-vi.js" type="text/javascript"></script>
    <script src="/Js/jquery.number.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $(".datetime").datepick({ dateFormat: 'dd/mm/yyyy' });
              <%--$('#<%= btnExport.ClientID %>').on({--%>
            $('#btnReport').on({
                click: function (e) {
                    $(".over-screen").removeClass("off").addClass("on");

                    setTimeout(function () {

                        var khuVucId = $("#<%= ddlKhuVuc.ClientID  %> option:selected").val();
                        var fromDate = $("#<%= txtFromDate.ClientID %>").val();
                        var toDate = $("#<%= txtToDate.ClientID %>").val();
                        var typeReport = 1;
                        $("#<%= rdlSelectedExport.ClientID %> input").each(function (a) {
                            if ($(this).is(":checked")) {
                                typeReport = $(this).val();
                            }
                        });
                        // Tiến hành lấy báo cáo
                        $.ajax({
                            async: false,
                            type: "POST",
                            url: "BaoCaoTongHopPAKN.aspx/CalReport",
                            data: "{ khuVucId: " + khuVucId + ", fromDate: '" + fromDate + "', toDate: '" + toDate + "', typeReport: " + typeReport + " }",
                            contentType: "application/json; charset=utf-8",
                            datatype: "json",
                            success: function (data, status, jqXHR) {
                                var reVal = $.parseJSON(data.d);
                                if (reVal.ErrorCode == 1) { // Lấy dữ liệu thành công
                                    // Tính toán số lượng tồn đọng kỳ trước
                                    var isError = false;
                                    var reportId = reVal.ReportId;
                                    var securityCode = reVal.SecurityKey;
                                    var dsTonDongKyTruong = null;
                                    $.ajax({
                                        async: false,
                                        type: "POST",
                                        url: "BaoCaoTongHopPAKN.aspx/CalTonDongKyTruoc",
                                        data: "{reportId: " + reportId + " , securityCode: '" + securityCode + "'}",
                                        contentType: "application/json; charset=utf-8",
                                        datatype: "json",
                                        success: function (data, status, jqXHR) {
                                            var reVal = $.parseJSON(data.d);
                                            if (reVal.ErrorCode == 1) { } // Xử lý dữ liệu thành công
                                            else {
                                                isError = true;
                                                alert(reVal.Message);
                                            }
                                        },
                                        error: function (jqXHR, status, exMsg) {
                                            isError = true;
                                            alert(exMsg);
                                        },
                                        complete: function () { }
                                    });
                                    if (isError == false) {
                                        // Tinh toán tiếp nào, lặp tính toán cho từng đối tác
                                        var objs_list = reVal.DoiTacIds.Value.split(',');
                                        var objs_length = objs_list.length;

                                        $.each(reVal.DoiTacIds.Value.split(','), function (index, value) {
                                            if (console && console.log) console.log("Đội dài: " + objs_length + ", thứ tự: " + index + ", đối tác: " + value);
                                            if (isError == false) {
                                                $.ajax({
                                                    async: false,
                                                    type: "POST",
                                                    url: "BaoCaoTongHopPAKN.aspx/CalEveryPartner",
                                                    data: "{reportId: " + reportId + " , securityCode: '" + securityCode + "', doiTacId: " + value + "}",
                                                    contentType: "application/json; charset=utf-8",
                                                    datatype: "json",
                                                    success: function (data, status, jqXHR) {
                                                        var reVal1 = $.parseJSON(data.d);
                                                        if (reVal1.ErrorCode != 1) {
                                                            isError = true;
                                                            alert(reVal1.Message);
                                                        }
                                                    },
                                                    error: function (jqXHR, status, exMsg) {
                                                        // Nếu xảy ra lỗi, thử lại thêm lần 2
                                                        if (console && console.log) console.log("có lỗi xảy ra, và sẽ thử lại lần nữa DoiTacId = " + value);
                                                        $.ajax({
                                                            async: false,
                                                            type: "POST",
                                                            url: "BaoCaoTongHopPAKN.aspx/CalEveryPartner",
                                                            data: "{reportId: " + reportId + " , securityCode: '" + securityCode + "', doiTacId: " + value + "}",
                                                            contentType: "application/json; charset=utf-8",
                                                            datatype: "json",
                                                            success: function (data, status, jqXHR) {
                                                                var reVal1 = $.parseJSON(data.d);
                                                                if (reVal1.ErrorCode != 1) {
                                                                    isError = true;
                                                                    alert(reVal1.Message);
                                                                }
                                                            },
                                                            error: function (a, b, c) {
                                                                // Nếu lỗi, thử lại lần 3
                                                                $.ajax({
                                                                    async: false,
                                                                    type: "POST",
                                                                    url: "BaoCaoTongHopPAKN.aspx/CalEveryPartner",
                                                                    data: "{reportId: " + reportId + " , securityCode: '" + securityCode + "', doiTacId: " + value + "}",
                                                                    contentType: "application/json; charset=utf-8",
                                                                    datatype: "json",
                                                                    success: function (data, status, jqXHR) {
                                                                        var reVal1 = $.parseJSON(data.d);
                                                                        if (reVal1.ErrorCode != 1) {
                                                                            isError = true;
                                                                            alert(reVal1.Message);
                                                                        }
                                                                    },
                                                                    error: function (a, b, c) {
                                                                        isError = true;
                                                                        alert(c);
                                                                        // Thông báo lỗi
                                                                    }

                                                                });
                                                            }
                                                        });
                                                    },
                                                    complete: function () { }
                                                });
                                            }
                                        })
                                    }
                                    else {
                                        hideOverScreen();
                                        alert("Có lỗi xảy ra, vui lòng kiểm tra lại!");
                                    }

                                    if (!isError) // Không có lỗi => Hoàn thành tính toán báo cáo
                                    {
                                        var objJson = "{'isOK':1, 'reportId': " + reportId + ", 'securityCode': '" + securityCode + "'}";
                                        $("#<%= hdfReportCode.ClientID %>").val(objJson);
                                        $("form:first").submit();
                                    }
                                } else if (reVal.ErrorCode == 0) { // Không tìm thấy danh sách đối tác phù hợp
                                    hideOverScreen();
                                    alert("Dữ liệu không phù hợp, vui lòng kiểm tra lại");
                                }
                            },
                            error: function (jqXHR, status, exMsg) { alert(exMsg); },
                            complete: function () {
                                hideOverScreen(); // Hoàn thành báo cáo                            
                            }
                        });
                    }, 100);
                    e.preventDefault();
                }
            });
        });
        function hideOverScreen() {
            $(".over-screen").removeClass("on").addClass("off");
        }
    </script>

    <%--Format Number--%>
    <script type="text/javascript">
        $(document).ready(function () {
            $(".tbl_style.tblcus td span.number").number(true, 0, ",", ".");
        });
    </script>
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="ContentPlaceHolder_Main">
    <div class="wrapper">
        <table class="tbl_head_report tb2">
            <tr>
                <td class="coltitle">Khu vực</td>
                <td class="colleft1">
                    <div class="selectstyle w200">
                        <div class="bg">
                            <asp:DropDownList runat="server" ID="ddlKhuVuc"></asp:DropDownList>
                        </div>
                    </div>
                </td>
                <td class="coltitle">Định dạng</td>
                <td>
                    <asp:RadioButtonList CssClass="rd-selected" ID="rdlSelectedExport" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow">
                        <asp:ListItem Selected="True" Value="1">Html</asp:ListItem>
                        <asp:ListItem Value="0">Excel</asp:ListItem>
                    </asp:RadioButtonList>
                </td>
            </tr>
            <tr>
                <td class="coltitle">Từ ngày</td>
                <td class="colleft1">
                    <div class="inputstyle w200">
                        <div class="bg">
                            <asp:TextBox ID="txtFromDate" CssClass="datetime" runat="server"></asp:TextBox>
                        </div>
                    </div>
                </td>
                <td class="coltitle">Đến ngày</td>
                <td>
                    <div class="inputstyle w200">
                        <div class="bg">
                            <asp:TextBox ID="txtToDate" CssClass="datetime" runat="server"></asp:TextBox>
                        </div>
                    </div>

                </td>
            </tr>
            <tr>
                <td class="coltitle">&nbsp;</td>
                <td class="colleft1 export">
                    <input type="button" id="btnReport" class="btn_main save" value="Lấy báo cáo" />&nbsp;
                    <asp:Button ID="btnExportExcel" CssClass="btn_main save excel" runat="server" Text="Xuất excel" Visible="false" />
                </td>
                <td class="coltitle">&nbsp;</td>
                <td>&nbsp;</td>
            </tr>
        </table>
        <asp:Panel runat="server" CssClass="rptContent" ID="RptContent">
            <asp:Literal runat="server" ID="liReport"></asp:Literal>
        </asp:Panel>
        <asp:HiddenField ID="hdfReportCode" runat="server" />
    </div>
    <div class="over-screen off">
        <div class="mask"></div>
        <div class="loading">
            <div class="loadAjax">
                <img runat="server" src="~/images/loading.gif" alt="Đang tải ..." />
            </div>
        </div>
    </div>
</asp:Content>
