<%@ Page Title="" Language="C#" MasterPageFile="~/master_default.master" AutoEventWireup="true" CodeBehind="MyKhieuNai.aspx.cs" Inherits="Website.Views.QLKhieuNai.MyKhieuNai" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder_Main" runat="server">
    <link href="/CSS/autocomplete.css" rel="stylesheet" type="text/css" />
    <script src="/JS/jquery.autocomplete.js" type="text/javascript"></script>
    <script src="/JS/jquery.atooltip.js" type="text/javascript"></script>
    <script src="/JS/jquery.pagination.js" type="text/javascript"></script>

    <script type="text/javascript">
        var strConfigColumn = [<%=strColumnConfig %>]
    </script>
    <script src="/Views/QLKhieuNai/JS/jsKNCuaToi.js" type="text/javascript"></script>
    <!--CSS cua JS flexgrid-->
    <link href="../../CSS/flexigrid.pack.css" rel="stylesheet" />
    <!--Control cua JS flexgid-->
    <script type="text/javascript" src="../../JS/flexigrid.js"></script>

    <link href="/CSS/jquery.datepick.css" rel="stylesheet" type="text/css" />
    <script src="/JS/plugin/jquery.datepick.js" type="text/javascript"></script>
    <script src="/JS/plugin/jquery.datepick-vi.js" type="text/javascript"></script>

    <script language="javascript" type="text/C#">
        $(document).ready(function () {
            $('.typeNumber').keyup(function () {
                this.value = this.value.replace(/[^0-9\.]/g, '');
            });
        });        

    </script>

    
<asp:PlaceHolder ID="placeHolder" runat="server"></asp:PlaceHolder>

    <style type="text/css">
        #grid-data td.nowrap {
            white-space: nowrap;
        }

        .autocomplete-suggestions {
            border: 1px solid #999;
            background: #FFF;
            cursor: default;
            overflow: auto;
            -webkit-box-shadow: 1px 4px 3px rgba(50, 50, 50, 0.64);
            -moz-box-shadow: 1px 4px 3px rgba(50, 50, 50, 0.64);
            box-shadow: 1px 4px 3px rgba(50, 50, 50, 0.64);
        }
        
        .autocomplete-suggestion {
            padding: 2px 5px;
            white-space: nowrap;
            overflow: hidden;
        }

        .autocomplete-selected {
            background: #F0F0F0;
        }

        .autocomplete-suggestions strong {
            font-weight: normal;
            color: #3399FF;
        }

        #aToolTip {
            position: absolute;
            display: none;
            z-index: 50000;
        }

            #aToolTip .aToolTipContent {
                position: relative;
                margin: 0;
                padding: 0;
            }

        .defaultTheme {
            border: 2px solid #ccc;
            background: #fff;
            color: #000;
            margin: 0;
            padding: 6px 12px;
            -moz-border-radius: 12px 12px 12px 0;
            -webkit-border-radius: 12px 12px 12px 0;
            -khtml-border-radius: 12px 12px 12px 0;
            border-radius: 12px 12px 12px 0;
            -moz-box-shadow: 2px 2px 5px #111; /* for Firefox 3.5+ */
            -webkit-box-shadow: 2px 2px 5px #111; /* for Safari and Chrome */
            box-shadow: 2px 2px 5px #111; /* for Safari and Chrome */
        }

            .defaultTheme #aToolTipCloseBtn {
                display: block;
                height: 18px;
                width: 18px;
                background: url(../images/closeBtn.png) no-repeat;
                text-indent: -9999px;
                outline: none;
                position: absolute;
                top: -20px;
                right: -30px;
                margin: 2px;
                padding: 4px;
            }
    </style>
    <input  type="hidden" id="sortname"/>
    <input  type="hidden" id="sortorder"/>
</asp:Content>
