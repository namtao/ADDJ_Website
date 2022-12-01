<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UcTopMyKhieuNai.ascx.cs"
    Inherits="Website.Views.QLKhieuNai.UserControls.UcTopMyKhieuNai" %>


<div id="dvTop">
    <ul class="dvqlkhieunai-top">
        <li class="liItem"><a id="tab2" class="" href="/Views/QLKhieuNai/MyKhieuNai.aspx?ctrl=tab2-KNPhanHoi">KN phản hồi <span id="totalKNPhanHoi"> (<asp:Literal runat="server" ID="ltKNPhanHoi"></asp:Literal>) </span></a></li>
        <li class="liItem"><a id="tab1" href="/Views/QLKhieuNai/MyKhieuNai.aspx?ctrl=tab1-KNDaGuiDi">KN đã tạo <span id="totalKNDaGuiDi">(<asp:Literal runat="server" ID="ltKNDaGui"></asp:Literal>) </span></a></li>
        
        
    </ul>
</div>
<script language="javascript">
    var pageSize = 10;
    function fnGetUrlParameter(sParam) {
        var values = '';
        var sPageURL = window.location.search.substring(1);
        var sURLVariables = sPageURL.split('&');
        for (var i = 0; i < sURLVariables.length; i++) {
            var sParameterName = sURLVariables[i].split('=');
            if (sParameterName[0] == sParam) {
                values = sParameterName[1];
            }
        }

        return values;
    }

    function fnGetTotalKhieuNai(key, tag) {
        //$.getJSON('/Views/QLKhieuNai/Handler/Handler.ashx?key=' + key + '&IsCache=' + cache, '', function (totalRecords) {
        //    if (totalRecords != '') {
        //        $('#' + tag).html("(" + totalRecords + ")");
        //    }
        //});
        $.ajax({
            type: "GET",
            url: "/Views/QLKhieuNai/Handler/Handler.ashx?key=" + key,
            contentType: "application/json; charset=utf-8",
            dataType: "text",
            success: function (data) {
                if (data != "-1") {
                    var arrData = data.split('|');
                    if (arrData.length == 8) {
                        $('#totalKNDaGuiDi').html("(" + arrData[0].substring(1) + ")");
                        $('#totalKNPhanHoi').html("(" + arrData[1] + ")");
                        
                    }
                }
            }
        });
    }
    $(document).ready(function () {
        var param = fnGetUrlParameter('ctrl');
        if (param != '') {
            var arr = param.split("-");
            $("a").removeClass("liItem-select");
            $('#' + arr[0]).addClass("liItem-select");
        } else {
            $("a").removeClass("liItem-select");
            $('#tab1').addClass("liItem-select");
        }

        var typeSearch = fnGetUrlParameter('TypeSearch');
        if (typeSearch == "-1") {
            
            $("#tab1").attr("href", $("#tab1").attr("href") + "&TypeSearch=-1");
            $("#tab2").attr("href", $("#tab2").attr("href") + "&TypeSearch=-1");
            
        }

        //if (param == "tab1-KNDaGuiDi") {
        //    fnGetTotalKhieuNai(1, "totalKNDaGuiDi");
        //}
        //else if (param == "tab2-KNPhanHoi") {
        //    fnGetTotalKhieuNai(2, "totalKNPhanHoi");
        //}
        

    });


</script>

