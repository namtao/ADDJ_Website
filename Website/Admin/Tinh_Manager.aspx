<%@ Page Language="C#" MasterPageFile="~/Master_Default.master" AutoEventWireup="true" Inherits="admin_tinh_manager" CodeBehind="Tinh_Manager.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder_Text" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <script type="text/javascript">
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
                    <li><a href="javascript:history.back()" class="btn btn-primary"><span class="glyphicon glyphicon-circle-arrow-left" aria-hidden="true"></span> Quay về</a></li>
                    <li>
                        <asp:LinkButton ID="LinkButton2" runat="server" CssClass="btn btn-primary" OnClick="LinkButton2_Click">
                       <span class="glyphicon glyphicon-plus-sign" aria-hidden="true"></span> Thêm mới
                        </asp:LinkButton>
                    </li>
                    <li>
                        <asp:LinkButton ID="LinkButton1" runat="server" OnClientClick="javascript:{return confirm('Bạn có muốn xóa đơn vị được chọn?');}" CssClass="btn btn-danger" OnClick="LinkButton1_Click">
                            <span class="glyphicon glyphicon-remove-sign" aria-hidden="true"></span> Xóa
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

                        <table width="100%" cellpadding="0" cellspacing="0" border="0">
                            <tr valign="top">
                                <td>
                                    <table style="border: 1px solid #d2d2d2; border-collapse: collapse; width: 100%">
                                        <tr style="background: #fffff0">
                                            <td>
                                                <table border="0" width="100%">
                                                    <tr>
                                                        <td style="text-align: right;">
                                                            <strong>Chọn tỉnh:</strong>
                                                        </td>
                                                        <td style="text-align: left;">
                                                            <div class="selectstyle">
                                                                <div class="bg">
                                                                    <asp:DropDownList ID="ddlTinh" runat="server">
                                                                    </asp:DropDownList>
                                                                </div>
                                                            </div>
                                                        </td>
                                                        <td>
                                                            <strong>
                                                                <asp:CheckBox ID="chkTinh" Text="Chỉ hiện tỉnh" runat="server" /></strong>
                                                        </td>
                                                        <td style="text-align: left">
                                                            <asp:LinkButton ID="LinkButton3" runat="server" CssClass="btn btn-primary" OnClick="LinkButton3_Click">
                                                                 <span class="glyphicon glyphicon-search" aria-hidden="true"></span> Tìm kiếm
                                                            </asp:LinkButton>
                                                        </td>
                                                        <td style="text-align: right; padding-right: 5px" width="50%">
                                                            <asp:LinkButton ID="LinkButton4" runat="server" CssClass="btn btn-danger" OnClick="LinkButton4_Click">
                                                                <span class="glyphicon glyphicon-eye-close" aria-hidden="true"></span> Xóa điều kiện lọc
                                                            </asp:LinkButton>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td style="height: 10px"></td>
                            </tr>
                            <tr>
                                <td style="height: 5px; text-align: left">
                                    <asp:Literal ID="ltThongBao" runat="server"></asp:Literal>
                                </td>
                            </tr>
                            <tr>
                                <td style="height: 10px"></td>
                            </tr>
                            <tr valign="top">
                                <td style="text-align: center">
                                    <%--<asp:BoundField DataField="AbbRev" HeaderText="Mã tỉnh" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Center" />--%>
                                    <asp:GridView ID="grvView" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered table-hover table-striped table-condensed"
                                        DataKeyNames="Id" AllowPaging="True"
                                        AllowSorting="True" OnRowDataBound="grvView_RowDataBound" OnPageIndexChanging="grvView_PageIndexChanging">
                                        <RowStyle CssClass="rowB" />
                                        <AlternatingRowStyle CssClass="rowA" />
                                        <PagerStyle BackColor="#4D709A" ForeColor="White" HorizontalAlign="Right" />
                                        <Columns>
                                            <asp:TemplateField ItemStyle-Width="5%" HeaderText="STT" HeaderStyle-HorizontalAlign="Center"
                                                ItemStyle-HorizontalAlign="Center"></asp:TemplateField>
                                            <asp:BoundField DataField="Name" HeaderText="Tên tỉnh" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Center" />
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
            </a>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

