<%@ Page Title="" Language="C#" MasterPageFile="~/master_default.master" AutoEventWireup="true"
    CodeBehind="SoTheoDoi.aspx.cs" Inherits="Website.Views.QLKhieuNai.SoTheoDoi" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder_Main" runat="server">
    <%--Su dung Autocomplate--%>   
    <link href="/CSS/autocomplete.css" rel="stylesheet" type="text/css" />
    <script src="/JS/jquery.autocomplete.js" type="text/javascript"></script>
    <%--Su dung Autocomplate--%>
    <style type="text/css">
        #grid-data td.nowrap{white-space:nowrap;}
        .autocomplete-suggestions
        {
            border: 1px solid #999;
            background: #FFF;
            cursor: default;
            overflow: auto;
            -webkit-box-shadow: 1px 4px 3px rgba(50, 50, 50, 0.64);
            -moz-box-shadow: 1px 4px 3px rgba(50, 50, 50, 0.64);
            box-shadow: 1px 4px 3px rgba(50, 50, 50, 0.64);
        }
        .autocomplete-suggestion
        {
            padding: 2px 5px;
            white-space: nowrap;
            overflow: hidden;
        }
        .autocomplete-selected
        {
            background: #F0F0F0;
        }
        .autocomplete-suggestions strong
        {
            font-weight: normal;
            color: #3399FF;
        }
    </style>
    <script language="javascript">
        $(document).ready(function () {
            $('.typeNumber').keyup(function () {
                //this.value = this.value.replace(/[^0-9]/g, '');
                //this.value = this.value.replace(/[\W-_]/g, ''); // chỉ cho nhập chữ cái la tinh và số
                this.value = this.value.replace(/[^0-9\.]/g, ''); //Chi cho nhap so
            });
//            $(".typeNumber").keydown(function (event) {
//                if (event.keyCode == 46 || event.keyCode == 8 || event.keyCode == 9 || event.keyCode == 27 || event.keyCode == 13 || (event.keyCode == 65 && event.ctrlKey == true) || (event.keyCode >= 35 && event.keyCode <= 39)) {
//                    return;
//                }
//                else if (event.keyCode == 109) {
//                    return true;
//                }
//                else {
//                    if (event.shiftKey || (event.keyCode < 48 || event.keyCode > 57) && (event.keyCode < 96 || event.keyCode > 105)) {
//                        event.preventDefault();
//                    }
//                }
//            });
        });        
       
    </script>
    <asp:PlaceHolder ID="placeHolder" runat="server"></asp:PlaceHolder>
</asp:Content>
