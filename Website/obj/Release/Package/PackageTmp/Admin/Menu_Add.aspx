<%@ Page Language="C#" MasterPageFile="~/Master_Default.master" AutoEventWireup="true" Inherits="Website.Admin.Menu_Add" CodeBehind="Menu_Add.aspx.cs" %>

<asp:Content runat="server" ContentPlaceHolderID="HeaderJs">
    <script type="text/javascript">
        function showByType(typeId) // Hiển thị lựa chọn theo typeId
        {
            if (typeId == 1) {
                $(".tbl_cus tr").show();
                $(".sp").hide();
            }
            else {
                $(".tbl_cus tr").hide();
                $(".sp").show();
                $(".choice").show();
                $(".cmd").show();
            }
        }
        $(document).ready(function () {
            <asp:Literal Id="liJs" runat="server"></asp:Literal >
                showByType(typeId);

            $("#<%=ddlMenuType.ClientID %>").change(function () {
                var typeId = $("#<%=ddlMenuType.ClientID %>").val();
                showByType(typeId);

                $.ajax({
                    beforeSend: function () { },
                    type: "POST",
                    dataType: "text",
                    url: "/admin/Ajax/SendMessageHander.ashx",
                    data: { type: "ChangeParentMenu", pareintId: $("#<%=ddlParrent.ClientID %>").val() },
                    success: function (text) {
                        if (text != "0") {
                            $("#<%=txtSTT.ClientID %>").val(text);
                        }
                    },
                    error: function () {
                    }
                });
            });

            $(".openWindow").click(function (e) {
                try {
                    common.loadding();

                    window.$linkClick = this;
                    var num = $(this).attr("number");
                    if (typeof (num) == "undefined") $(this).attr("number", 1);
                    else $(this).attr("number", Number(num) + 1)

                    var link = $(this).attr("href");
                    if (typeof (window.dsMenuPopup) === 'undefined' || window.dsMenuPopup == null) {
                        var newNum = Number($(this).attr("number"));

                        // Chống click liên tục
                        if (newNum == 1) {
                            var w = window.open(link, "Open menu!", "width=990,height=540,resizable=yes,toolbar=no,menubar=no,location=no,status=no,scrollbars=yes,top=30")
                            w.focus();
                            window.dsMenuPopup = w;

                            w.onbeforeunload = function (e) {
                                common.unLoading();
                                window.dsMenuPopup = null;
                                $(window.$linkClick).attr("number", 0);
                            }

                            // Popup mở thành công
                            w.onload = function (e) {
                                common.unLoading();
                            }
                        }
                    }
                    else {
                        window.dsMenuPopup.focus();
                        common.unLoading();
                    }
                }
                catch (e) {
                    if (console && console.log) console.log(obj);
                    common.unLoading();
                    alert(e.message);
                }
                e.preventDefault();
            });
            $(window).on({
                "getMenuItem": function (e) {
                    console.log(e);
                    $("#<%= txtParentId.ClientID %> ").val(e.value[0].Name);
                    $("#<%= hdfParentId.ClientID %>").val(e.value[0].Id);
                    window.dsMenuPopup.close();
                },
                "closeMenuItem": function (e) {
                    window.dsMenuPopup.close();
                }
            });
        });
    </script>
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="HeaderCss">
    <style type="text/css">
        /*.tbl_cus td { padding: 5px; }*/
        .tbl_cus .inputstyle input {
            padding-left: 5px;
            padding-right: 5px;
        }

        .col1 {
            width: 120px;
        }

        .chkcheck label {
            padding-left: 7px;
        }
    </style>
</asp:Content>
<asp:Content ContentPlaceHolderID="Content" runat="Server">
    <!-- begin panel nav boot -->
    <div class="nav_btn_bootstrap">
        <ul>
            <li><a href="javascript:history.back()" class="btn btn-primary"><span class="glyphicon glyphicon-circle-arrow-left" aria-hidden="true"></span> Quay về</a></li>
            <li>
                <asp:LinkButton ID="linkbtnSubmit" class="btn btn-primary" runat="server" OnClick="linkbtnSubmit_Click">
                    <span class="glyphicon glyphicon-floppy-saved" aria-hidden="true"></span> Cập nhật
                </asp:LinkButton>
            </li>
        </ul>
    </div>
    <!-- end panel nav boot -->
    <div class="p8">
        <!-- begin panel boot -->
        <div class="panel panel-default">
            <div class="panel-heading"><span style="font-size: 12pt">Lọc tìm kiếm</span></div>
            <div class="panel-body" style="border: none">
                <!-- begin body boot -->

                <div id="tableMenu">
                    <table border="0" class="tblstyle tbl_cus" cellpadding="0" cellspacing="0" width="100%">
                        <tr>
                            <td class="col1" style="text-align: right; padding-right: 10px">&nbsp;</td>
                            <td style="text-align: left">
                                <asp:Label ID="lblMsg" runat="server" Text="" ForeColor="Red"></asp:Label>
                            </td>
                        </tr>
                        <tr class="choice">
                            <td style="width: 100px; text-align: right; padding-right: 10px">Loại menu
                            </td>
                            <td style="text-align: left">
                                <div class="selectstyle">
                                    <div class="bg">
                                        <asp:DropDownList ID="ddlMenuType" runat="server" Width="120px">
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: right; padding-right: 10px">Chọn cha
                            </td>
                            <td style="text-align: left">
                                <div class="selectstyle">
                                    <div class="bg">
                                        <asp:DropDownList ID="ddlParrent" runat="server" Width="255px">
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </td>
                        </tr>

                        <tr class="none">
                            <td style="text-align: right; padding-right: 10px">&nbsp;</td>
                            <td style="text-align: left">
                                <div class="inputstyle" style="width: 350px">
                                    <div class="bg">
                                        <asp:HiddenField ID="hdfParentId" Value="0" runat="server" />
                                        <asp:TextBox ID="txtParentId" runat="server" Width="245px"></asp:TextBox>
                                        <a class="openWindow" href="Popup/DanhSachMenu.aspx">[Chọn]</a>
                                    </div>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 100px; text-align: right; padding-right: 10px">Tên
                            </td>
                            <td style="text-align: left">
                                <div class="inputstyle">
                                    <div class="bg">
                                        <asp:TextBox ID="txtName" runat="server" MaxLength="100" Width="445px"></asp:TextBox>
                                    </div>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: right; padding-right: 10px">Tên 2
                            </td>
                            <td style="text-align: left">
                                <div class="inputstyle">
                                    <div class="bg">
                                        <asp:TextBox ID="txtName2" runat="server" MaxLength="100" Width="445px"></asp:TextBox>
                                    </div>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: right; padding-right: 10px">Tên 3
                            </td>
                            <td style="text-align: left">
                                <div class="inputstyle">
                                    <div class="bg">
                                        <asp:TextBox ID="txtName3" runat="server" MaxLength="100" Width="445px"></asp:TextBox>
                                    </div>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: right; padding-right: 10px">Thứ tự
                            </td>
                            <td style="text-align: left">
                                <div class="inputstyle">
                                    <div class="bg">
                                        <asp:TextBox ID="txtSTT" runat="server" Width="50px"></asp:TextBox>
                                    </div>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: right; padding-right: 10px; vertical-align: top; padding-top: 5px;">Đường dẫn
                            </td>
                            <td style="text-align: left">
                                <asp:UpdatePanel runat="server">
                                    <ContentTemplate>
                                        <div class="selectstyle">
                                            <div class="bg">
                                                <asp:DropDownList ID="ddlLink" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlLink_Changed" Width="455px">
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                        <div class="inputstyle">
                                            <div class="bg">
                                                <asp:TextBox ID="txtLink" runat="server" Width="445px"></asp:TextBox>
                                            </div>
                                        </div>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: right; padding-right: 10px">Hiển thị</td>
                            <td style="text-align: left; padding-top: 5px;">
                                <asp:CheckBox ID="chkDisplay" Checked="true" CssClass="chkcheck" runat="server" />
                            </td>
                        </tr>
                        <tr class="sp">
                            <td style="text-align: right; padding-right: 10px">Dấu ngăn
                            </td>
                            <td style="text-align: left; padding-top: 5px;">
                                <div class="inputstyle">
                                    <div class="bg">
                                        <asp:TextBox ID="txtNameSeparator" Text="---------------" runat="server" MaxLength="100" Width="445px"></asp:TextBox>
                                    </div>
                                </div>
                            </td>
                        </tr>
                        <tr class="sp">
                            <td style="text-align: right; padding-right: 10px">Xếp bên dưới Menu
                            </td>
                            <td style="text-align: left; padding-top: 5px;">
                                <div class="selectstyle">
                                    <div class="bg">
                                        <asp:DropDownList ID="ddlMenu" runat="server" Width="455px">
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </td>
                        </tr>
                    
                    </table>
                </div>
                <!-- end body boot -->
            </div>
        </div>
        <!-- end panel boot -->
    </div>
</asp:Content>
