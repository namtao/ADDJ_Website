<%@ Page Language="C#" MasterPageFile="~/Master_Default.master" AutoEventWireup="true" Inherits="PhanQuyenKN2NSD" CodeBehind="PhanQuyenKN2NSD.aspx.cs" %>

<asp:Content runat="server" ContentPlaceHolderID="HeaderJs">
    <script src="/Content/Chosen/chosen.jquery.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $(".chosen").chosen();
        });
    </script>
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="HeaderCss">
    <link href="/Content/Chosen/chosen.css" rel="stylesheet" />
</asp:Content>
<asp:Content ContentPlaceHolderID="ContentPlaceHolder_Text" runat="Server">
    <asp:UpdatePanel ID="UpdatePnl" runat="server">
        <ContentTemplate>
            <script type="text/javascript">
                function LoadJS() {
                    $(".chosen").chosen(); // Search Chosen

                    // Add multiple select / deselect functionality
                    $("#selectallRead").click(function () {
                        $('.chkAllRead').find("input").attr('checked', this.checked);
                    });
                    // If all checkbox are selected, check the selectall checkbox
                    // And viceversa
                    $(".chkAllRead").click(function () {
                        if ($(".chkAllRead").find("//input[checked='checked']").length == $(".chkAllRead").find("input").length) {
                            $("#selectallRead").attr("checked", "checked");
                        } else {
                            $("#selectallRead").removeAttr("checked");
                        }
                    });
                }
                $(function () {
                    // add multiple select / deselect functionality
                    $("#selectallRead").click(function () {
                        $('.chkAllRead').find("input").attr('checked', this.checked);
                    });
                    // if all checkbox are selected, check the selectall checkbox
                    // and viceversa
                    $(".chkAllRead").click(function () {
                        if ($(".chkAllRead").find("//input[checked='checked']").length == $(".chkAllRead").find("input").length) {
                            $("#selectallRead").attr("checked", "checked");
                        } else {
                            $("#selectallRead").removeAttr("checked");
                        }
                    });
                });
            </script>
            <!-- begin panel nav boot -->
            <div class="nav_btn_bootstrap">
                <ul>
                    <li><a href="javascript:history.back()" class="btn btn-primary"><span class="glyphicon glyphicon-circle-arrow-left" aria-hidden="true"></span> Quay về</a></li>
                    <li>
                        <asp:LinkButton ID="linkbtnPhanQuyen" class="btn btn-primary" runat="server" OnClick="linkbtnPhanQuyen_Click">
                             <span class="glyphicon glyphicon-refresh" aria-hidden="true"></span> Cập nhật
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

                        <table border="0" cellpadding="0" cellspacing="0" width="100%" style="margin: 10px 0px;">
                            <tr>
                                <td style="text-align: right; padding-right: 5px;" colspan="5">
                                    <asp:Literal runat="server" ID="lbMess"></asp:Literal>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 120px; text-align: right; padding-right: 5px;">Chọn người sử dụng</td>
                                <td style="width: 150px;">
                                    <asp:DropDownList ID="ddlUser" runat="server" AutoPostBack="true" CssClass="chosen" OnSelectedIndexChanged="ddlUser_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                                <td style="width: 150px; text-align: right; padding-right: 5px;">Nhóm quyền mặc định</td>
                                <td style="width: 230px;">
                                    <asp:DropDownList Width="200px" ID="ddlPermissionDefault" runat="server" AutoPostBack="true" CssClass="chosen" OnSelectedIndexChanged="ddlPermissionDefault_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                                <td>&nbsp;</td>
                            </tr>
                        </table>
                        <asp:GridView ID="grvView" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered table-hover table-striped table-condensed"
                            DataKeyNames="PermissionId"
                            AllowSorting="True" OnRowDataBound="grvView_RowDataBound" Width="100%">
                            <RowStyle CssClass="rowB" />
                            <AlternatingRowStyle CssClass="rowA" />
                            <Columns>
                                <asp:TemplateField ItemStyle-Width="10%" HeaderText="STT" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="Center"></asp:TemplateField>
                                <asp:BoundField ItemStyle-Width="60%" DataField="Name" HeaderText="Chức năng" ItemStyle-HorizontalAlign="Left"
                                    HeaderStyle-HorizontalAlign="Center" />
                                <asp:TemplateField ItemStyle-Width="10%" HeaderText="Read" ItemStyle-HorizontalAlign="Center">
                                    <HeaderTemplate>
                                        Thực hiện
                                        <input type="checkbox" id="selectallRead" />
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkRead" CssClass="chkAllRead" runat="server" Checked='<%# Convert.ToBoolean(Eval("IsAllow")) %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                        <!-- end body boot -->
                    </div>
                </div>
                <!-- end panel boot -->
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
