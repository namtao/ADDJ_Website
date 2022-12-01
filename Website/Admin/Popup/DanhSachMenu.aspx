<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DanhSachMenu.aspx.cs" Inherits="Website.DanhSachMenu" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Danh sách menu</title>
    <%= string.Format("<link href=\"{0}?ver={1}\" type=\"text/css\" rel=\"stylesheet\" />", ResolveClientUrl("~/css/reset.css"), Website.AppCode.Common.Ver)  %>
    <%= string.Format("<link href=\"{0}?ver={1}\" type=\"text/css\" rel=\"stylesheet\" />", ResolveClientUrl("~/css/style.css"), Website.AppCode.Common.Ver)  %>
    <%= string.Format("<link href=\"{0}?ver={1}\" type=\"text/css\" rel=\"stylesheet\" />", ResolveClientUrl("~/css/style.cus.css"), Website.AppCode.Common.Ver)  %>
    <%= string.Format("<script src=\"{0}?ver={1}\" type=\"text/javascript\"></script>", ResolveClientUrl("~/JS/jquery-1.8.3.min.js"), Website.AppCode.Common.Ver)  %>
    <style type="text/css">
        .p8 { padding-bottom: 40px; position: relative; z-index: 10; }
        .gr-command { position: fixed; width: 100%; bottom: 0px; left: 0px; height: 35px; background-color: white; }
        .gr-command .inner { padding: 5px; }
    </style>

    <script type="text/javascript">
        $(document).ready(function () {
            // Xử lý lựa chọn
            var lst = $("#<%: hdfMenu.ClientID %>").val();
            if (lst != "") {
                var fullValue = [];
                var idsSelected = lst.split(",")
                $(".chkItem > input").each(function () {
                    var id = $(this).attr("id");
                    for (index = 0; index < idsSelected.length; index++) {
                        if (id == idsSelected[index]) {
                            $(this).prop("checked", true);
                            break;
                        }
                    }
                });
            }
            $('.chkItem > input').click(function () {
                $('.chkItem > input').not(this).prop('checked', false);
                var checkId = $(this).attr("id");
                $('#<%: hdfMenu.ClientID %>').val(checkId);
            });


            $(".chkItem > input").on("click", function () {
                var lst = [];
                var strList = $('#<%: hdfMenu.ClientID %>').val();
                if (strList != "") lst = ($('#<%: hdfMenu.ClientID %>').val()).split(",");
                var isCheck = $(this).prop("checked"); // Giá trị kiểm tra check
                var checkId = $(this).attr("id"); // Id được check
                var isExits = false;
                var valIndex = -1;
                if (lst.length > 0) {
                    for (index = 0; index < lst.length; index++) {
                        if (lst[index] == checkId) {
                            valIndex = index;
                            isExits = true;
                            break;
                        }
                    }
                }
                if (isCheck && !isExits) {
                    lst.push(checkId);
                }
                if (!isCheck && isExits) {
                    lst.splice(valIndex, 1);
                }
                $('#<%: hdfMenu.ClientID %>').val(lst.join(","))
            });

        });
        // Xử lý checked với id
        $(document).ready(function () {
            $("#<%: btnChapNhan.ClientID %>").on("click", function (e) {
                e.preventDefault();
                // Kiểm tra được mở từ nơi khác
                if (window.opener != null) {
                    var items = $("#<%: hdfMenu.ClientID %>").val();
                    if (items != "") {
                        $.ajax({
                            async: false,
                            url: 'Ajax/HandData.ashx?Type=1&MenuIds=' + items,
                            success: function (data, textStatus, jqXHR) {
                                var dataObj = $.parseJSON(data)
                                if (dataObj.Code == 1) {

                                    var objs = new Array();
                                    for (var i = 0; i < dataObj.Number; i++) {
                                        var person = { Id: dataObj.Data[i].Id, Name: dataObj.Data[i].Name, Link: dataObj.Data[i].Link };
                                        objs.push(person);
                                    }
                                    var windowOpen = window.opener
                                    windowOpen.$('body').trigger({
                                        type: "getMenuItem",
                                        value: objs
                                    });
                                    e.preventDefault();
                                }
                                else {
                                    if (console && console.log) console.log(data);
                                    alert(dataObj.Message);
                                }
                            },
                            error: function (jqXHR, textStatus, errorThrown) {
                                if (console && console.log) console.log(textStatus);
                            },
                            complete: function (xhr, textStatus) {
                                // setTimeout(function () { $.unblockUI(); }, 250);
                                // if (console && console.log) console.log("Hoàn thành");
                            }
                        });
                    }
                    else // Trường hợp loại bỏ tất [Không lựa chọn]
                    {
                        try {
                            alert("Bạn đã không chọn dữ liệu");
                            window.parent.$('body').trigger({
                                type: "CloseMenu"
                            });
                            e.preventDefault();
                        }
                        catch (err) {
                            alert("Form chỉ phù hợp dạng dữ liệu lựa chọn, dạng iframe, vui lòng kiểm tra lại");
                            if (console && console.log) console.log(err.message);
                        }

                    }
                }
                else {
                    alert('Không hợp lệ, cửa số chỉ dùng cho popup');
                }
            });
            $("#btnHuyBo").click(function (e) {
                e.preventDefault();
                if (window.opener != null) {
                    var windowOpen = window.opener
                    windowOpen.$('body').trigger({
                        type: "closeMenuItem"
                    });
                } else {
                    alert('Không hợp lệ, cửa số chỉ dùng cho popup');
                }
            });
        });
    </script>
</head>
<body>
    <form runat="server">
        <div class="p8">
            <table class="tbstyle" border="0" cellpadding="5" cellspacing="0" width="100%">
                <%--<tr>
                    <td style="width: 100px">
                        <label>Tìm người dùng</label></td>
                    <td style="text-align: left">
                        <asp:Panel runat="server" DefaultButton="btFilter">
                            <div class="inputstyle" style="display: inline-block;">
                                <div class="bg">
                                    <asp:TextBox ID="txtTenDangNhap" runat="server"></asp:TextBox>
                                </div>
                            </div>
                            <div style="display: inline-block">
                                <asp:LinkButton ID="btFilter" runat="server" CssClass="btn_control btnsearch" OnClick="btnSearch_Click" Text=""><span>Lọc</span></asp:LinkButton>
                                <asp:LinkButton runat="server" CssClass="btn_control btnrmfilter mgleft5" ID="btnReset" OnClick="btnReset_Click" Text=""><span>Bỏ lọc</span></asp:LinkButton>
                            </div>
                        </asp:Panel>
                    </td>
                    <td></td>
                </tr>--%>
                <tr>
                    <td>
                        <div class="selected" style="padding-top: 0px;">
                            <div style="float: left; padding-top: 20px;">
                                <asp:Literal runat="server" ID="ltrMsg"></asp:Literal>
                            </div>
                            <div class="selectstyle" style="width: 250px; display: inline-block; float: right;">
                                <div class="bg">
                                    <asp:DropDownList ID="cboParentId" runat="server">
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="selectstyle" style="width: 200px; display: inline-block; float: right;">
                                <div class="bg">
                                    <asp:DropDownList ID="cboLevel" runat="server">
                                        <asp:ListItem Text="- Chọn cấp -" Value="0"></asp:ListItem>
                                        <asp:ListItem Text="Cấp 1" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="Cấp 2" Value="2"></asp:ListItem>
                                        <asp:ListItem Text="Cấp 3" Value="3"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div style="clear: both"></div>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:GridView ID="grvView" runat="server" AutoGenerateColumns="False" CssClass="tbl_style" Width="100%"
                            DataKeyNames="ID" AllowPaging="True" AllowSorting="True" OnRowDataBound="grvView_RowDataBound" OnPageIndexChanging="grvView_PageIndexChanging">
                            <RowStyle CssClass="rowB" />
                            <AlternatingRowStyle CssClass="rowA" />
                            <Columns>
                                <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-Width="5%">
                                    <HeaderTemplate>
                                        Chọn
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <span class="chkItem">
                                            <input type="checkbox" id="<%# Eval("Id") %>" attrid="<%# Eval("Id") %>" attrtenmenu="<%# Eval("Name") %>" />
                                        </span>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="Name" HeaderText="Tên Menu" ItemStyle-HorizontalAlign="Left"
                                    HeaderStyle-HorizontalAlign="Center">
                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                    <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="Link" HeaderText="Liên kết" ItemStyle-HorizontalAlign="Left"
                                    HeaderStyle-HorizontalAlign="Center">
                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                    <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                </asp:BoundField>
                            </Columns>
                            <EmptyDataTemplate>
                                <div class="empty">Hiện không có menu con !</div>
                            </EmptyDataTemplate>
                        </asp:GridView>

                    </td>
                </tr>
            </table>
            <asp:HiddenField runat="server" ID="hdfMenu" />
            <div class="gr-command">
                <div class="inner">
                    <asp:Button ID="btnChapNhan" CssClass="command" runat="server" Text="Chấp nhận" />
                    <asp:Button ID="btnHuyBo" CssClass="command" runat="server" Text="Hủy bỏ" />
                    <asp:Label ID="lblmsg" runat="server"></asp:Label>
                </div>
            </div>
        </div>
    </form>
</body>
</html>
