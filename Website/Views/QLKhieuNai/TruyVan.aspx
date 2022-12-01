<%@ Page Title="" Language="C#" MasterPageFile="~/AdminNotAJAX.Master" AutoEventWireup="true" CodeBehind="TruyVan.aspx.cs" Inherits="Website.Views.QLKhieuNai.TruyVan" %>

<%@ Register Src="~/Views/QLKhieuNai/UserControls/KNTruyVan.ascx" TagName="KNTruyVan" TagPrefix="KNTruyVan" %>
<asp:Content runat="server" ContentPlaceHolderID="HeaderCss">
    <link href="/CSS/autocomplete.css" rel="stylesheet" type="text/css" />
    <link href="/CSS/flexigrid.pack.css" rel="stylesheet" />
    <style type="text/css">
        #grid-data td.nowrap { white-space: nowrap; }
        .autocomplete-suggestions { border: 1px solid #999; background: #FFF; cursor: default; overflow: auto; -webkit-box-shadow: 1px 4px 3px rgba(50, 50, 50, 0.64); -moz-box-shadow: 1px 4px 3px rgba(50, 50, 50, 0.64); box-shadow: 1px 4px 3px rgba(50, 50, 50, 0.64); }
        .autocomplete-suggestion { padding: 2px 5px; white-space: nowrap; overflow: hidden; }
        .autocomplete-selected { background: #F0F0F0; }
        .autocomplete-suggestions strong { font-weight: normal; color: #3399FF; }
        #aToolTip { position: absolute; display: none; z-index: 50000; }
        #aToolTip .aToolTipContent { position: relative; margin: 0; padding: 0; }
        .defaultTheme { border: 2px solid #ccc; background: #fff; color: #000; margin: 0; padding: 6px 12px; -moz-border-radius: 12px 12px 12px 0; -webkit-border-radius: 12px 12px 12px 0; -khtml-border-radius: 12px 12px 12px 0; border-radius: 12px 12px 12px 0; -moz-box-shadow: 2px 2px 5px #111; /* for Firefox 3.5+ */ -webkit-box-shadow: 2px 2px 5px #111; /* for Safari and Chrome */ box-shadow: 2px 2px 5px #111; /* for Safari and Chrome */ }
        .defaultTheme #aToolTipCloseBtn { display: block; height: 18px; width: 18px; background: url(../images/closeBtn.png) no-repeat; text-indent: -9999px; outline: none; position: absolute; top: -20px; right: -30px; margin: 2px; padding: 4px; }
        .divOpacity img { height: auto !important; }
    </style>
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="HeaderJs">
    <%= string.Format("<script src=\"{0}?Ver={1}\" type=\"text/javascript\"></script>", ResolveClientUrl("~/Js/jquery.autocomplete.js"), Website.AppCode.Common.Ver) %>
    <%= string.Format("<script src=\"{0}?Ver={1}\" type=\"text/javascript\"></script>", ResolveClientUrl("~/Js/jquery.atooltip.js"), Website.AppCode.Common.Ver) %>
    <%= string.Format("<script src=\"{0}?Ver={1}\" type=\"text/javascript\"></script>", ResolveClientUrl("~/Js/jquery.pagination.js"), Website.AppCode.Common.Ver) %>
    <%= string.Format("<script src=\"{0}?Ver={1}\" type=\"text/javascript\"></script>", ResolveClientUrl("~/Views/QLKhieuNai/Js/jsTruyVan.js"), Website.AppCode.Common.Ver) %>
    <%= string.Format("<script src=\"{0}?Ver={1}\" type=\"text/javascript\"></script>", ResolveClientUrl("~/JS/flexigrid.js"), Website.AppCode.Common.Ver) %>
    <script type="text/javascript">
        var strConfigColumn = [<%= strColumnConfig %>];
    </script>
    <script type="text/javascript">
        $(document).ready(function () {
            $('.textbox').keyup(function () {
                this.value = this.value.replace(/[\W-_]/g, '');
            });
        });
    </script>
</asp:Content>
<asp:Content ContentPlaceHolderID="ContentPlaceHolder_Text" runat="server">
    <asp:PlaceHolder ID="placeHolder" runat="server"></asp:PlaceHolder>
    <input id="hidtinhthanhid" runat="server" clientidmode="Static" type="hidden" value="" />
    <input id="hidloaikhieunai0id" runat="server" clientidmode="Static" type="hidden" value="" />
    <input id="hidloaikhieunai1id" runat="server" clientidmode="Static" type="hidden" value="" />
    <input id="hidloaikhieunai2id" runat="server" clientidmode="Static" type="hidden" value="" />
    <div class="over-screen off">
        <div class="mask"></div>
        <div class="loading">
            <div class="loadAjax">
                <img runat="server" src="~/images/loading.gif" alt="Đang tải ..." />
            </div>
        </div>
    </div>
</asp:Content>
