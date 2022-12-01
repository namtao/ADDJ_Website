<%@ Page Language="C#" MasterPageFile="~/Master_Default.master" AutoEventWireup="true" Inherits="Website.ConfigurationArchive" CodeBehind="ConfigurationArchive.aspx.cs" %>

<asp:Content ContentPlaceHolderID="Content" runat="Server">
    <asp:UpdatePanel ID="UpdatePnl" runat="server">
        <ContentTemplate>
            <script language="javascript" type="text/javascript">
                function LoadJS() {
                    $("#selectall").click(function () {
                        $('.case').find("input").attr('checked', this.checked);
                    });
                    $(".case").click(function () {
                        if ($(".case").find("//input[checked='checked']").length == $(".case").find("input").length) {
                            $("#selectall").attr("checked", "checked");
                        } else {
                            $("#selectall").removeAttr("checked");
                        }
                    });
                }

                $(function () {
                    $("#selectall").click(function () {
                        $('.case').find("input").attr('checked', this.checked);
                    });
                    $(".case").click(function () {
                        if ($(".case").find("//input[checked='checked']").length == $(".case").find("input").length) {
                            $("#selectall").attr("checked", "checked");
                        } else {
                            $("#selectall").removeAttr("checked");
                        }
                    });
                });
            </script>

            <!-- begin panel nav boot -->
            <div class="nav_btn_bootstrap">
                <ul>
                    <li><a href="javascript:history.back()" class="btn btn-primary"><span class="glyphicon glyphicon-circle-arrow-left" aria-hidden="true"></span>Quay về</a></li>
                    <li>
                        <asp:LinkButton ID="linkbtnThemMoi"  CssClass="btn btn-primary"  runat="server" OnClick="linkbtnThemMoi_Click"><span class="glyphicon glyphicon-plus-sign" aria-hidden="true"></span> Thêm mới</asp:LinkButton>
                    </li>
                    <li>
                        <asp:LinkButton ID="linkbtnXoa" runat="server" OnClientClick="javascript:{return confirm('Bạn có muốn xóa đơn vị được chọn?');}" CssClass="btn btn-danger" OnClick="linkbtnXoa_Click">  <span class="glyphicon glyphicon-remove-sign" aria-hidden="true"></span> Xóa</asp:LinkButton>
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

                        <table width="100%" cellpadding="0" cellspacing="0" border="0">
                            <tr style="height: 5px;">
                                <td></td>
                            </tr>
                            <tr>
                                <td style="height: 5px; text-align: left">
                                    <asp:Literal runat="server" ID="ltThongBao"></asp:Literal>
                                </td>
                            </tr>
                            <tr>
                                <td style="height: 10px"></td>
                            </tr>
                            <tr valign="top">
                                <td style="text-align: center">
                                    <asp:GridView ID="grvView" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered table-hover table-striped table-condensed"
                                        DataKeyNames="ID" AllowSorting="True" OnRowDataBound="grvView_RowDataBound">
                                        <RowStyle CssClass="rowB" />
                                        <AlternatingRowStyle CssClass="rowA" />
                                        <Columns>
                                            <asp:TemplateField ItemStyle-Width="5%" HeaderText="STT" HeaderStyle-HorizontalAlign="Center"
                                                ItemStyle-HorizontalAlign="Center"></asp:TemplateField>
                                            <asp:BoundField DataField="NamLuuTru" HeaderText="Năm dữ liệu" ItemStyle-HorizontalAlign="Center"
                                                HeaderStyle-HorizontalAlign="Center" />
                                            <asp:BoundField DataField="Name" HeaderText="Tên archive" ItemStyle-HorizontalAlign="Left"
                                                HeaderStyle-HorizontalAlign="Center" />
                                            <asp:BoundField DataField="Description" HeaderText="Mô tả" ItemStyle-HorizontalAlign="Left"
                                                HeaderStyle-HorizontalAlign="Center" />
                                            <asp:TemplateField>
                                                <HeaderTemplate>
                                                    Chọn
                                            <input type="checkbox" id="selectall" />
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="cbSelectAll" CssClass="case" runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </td>
                            </tr>
                            <tr>
                                <td style="height: 10px"></td>
                            </tr>
                        </table>
                        <!-- end body boot -->
                    </div>
                </div>
                <!-- end panel boot -->
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
