<%@ Page Language="C#" MasterPageFile="~/Master_Default.master" AutoEventWireup="true" Inherits="admin_doiTac_manager" CodeBehind="DoiTac_Manager.aspx.cs" %>

<asp:Content runat="server" ContentPlaceHolderID="HeaderCss">
    <style type="text/css">
        .msg {
            padding: 10px 0px;
        }

        .tbl_style a {
            color: #034afc;
        }
    </style>
</asp:Content>
<asp:Content ContentPlaceHolderID="Content" runat="Server">
    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <!-- begin panel nav boot -->
            <div class="nav_btn_bootstrap">
                <ul>
                    <li><a href="javascript:history.back()" class="btn btn-primary"><span class="glyphicon glyphicon-circle-arrow-left" aria-hidden="true"></span>Quay về</a></li>
                </ul>
            </div>
            <div class="p8">
                <!-- begin panel boot -->
                <div class="panel panel-default">
                    <div class="panel-heading"><span style="font-size: 12pt">Lọc tìm kiếm</span></div>
                    <div class="panel-body" style="border: none">
                        <!-- begin body boot -->
                        <table width="100%" cellpadding="0" cellspacing="0" border="0">
                            <tr>
                                <td style="text-align: left; width: 200px;">
                                    <div class="selectstyle">
                                        <div class="bg">
                                            <asp:DropDownList ID="ddlDoiTac" runat="server">
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                </td>
                                <td style="width: 200px;">
                                    <div class="selectstyle">
                                        <div class="bg">
                                            <asp:DropDownList ID="ddlFilter" runat="server">
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                </td>
                                <td style="text-align: left; width: 160px;">
                                    <div class="inputstyle">
                                        <div class="bg">
                                            <asp:TextBox ID="txtKeyword" runat="server" Style="width: 150px;" placeholder="Tìm kiếm từ khóa"> </asp:TextBox>
                                        </div>
                                    </div>

                                </td>
                                <td style="text-align: left; width: 300px;">
                                    <asp:Button ID="btFilter" CssClass="newbtn grad filter" runat="server" Text="Lọc" OnClick="btFilter_Click" />
                                    <asp:Button ID="btClearFilter" OnClick="btClearFilter_Click" runat="server" CssClass="newbtn grad rmfilter" Text="Bỏ lọc" />
                                </td>
                                <td>
                                    <div class="selectstyle" style="float: right; width: 200px;">
                                        <div class="bg">
                                            <asp:DropDownList runat="server" ID="ddlLevel">
                                                <asp:ListItem Text="-- Tất cả --" Value="32"></asp:ListItem>
                                                <asp:ListItem Text="Cấp 1" Value="1"></asp:ListItem>
                                                <asp:ListItem Text="Cấp 2" Value="2"></asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                </td>
                            </tr>
                        </table>
                        <div class="msg">
                            <asp:Literal ID="ltThongBao" runat="server"></asp:Literal>
                        </div>
                        <asp:GridView ID="GrvView" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered table-hover table-striped table-condensed" AllowPaging="False" DataKeyNames="Id" AllowSorting="True" OnRowDataBound="GrvView_RowDataBound" OnPageIndexChanging="GrvView_PageIndexChanging" Width="100%">
                            <RowStyle CssClass="rowB" />
                            <AlternatingRowStyle CssClass="rowA" />
                            <Columns>
                                <asp:TemplateField ItemStyle-Width="5%" HeaderText="STT" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Center" Width="5%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Mã đối tác">
                                    <ItemTemplate>
                                        <%# Website.AppCode.Common.GiveEName(Eval("MaDoiTac"), (int)Eval("Level"), "|—") %>
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Tên đối tác">
                                    <ItemTemplate>
                                        <%# Website.AppCode.Common.GiveEName(Eval("TenDoiTac"), (int)Eval("Level"), "|—") %>
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Sắp xếp">
                                    <ItemTemplate>
                                        <span><%# Eval("Sort") %></span>
                                        <div style="float: right;">
                                            <asp:LinkButton CommandArgument='<%# Eval("Id").ToString() + ";Up" %>' OnClick="Unnamed_Click" runat="server" Text="Lên"></asp:LinkButton>
                                            -
                                    <asp:LinkButton CommandArgument='<%# Eval("Id").ToString() + ";Down" %>' runat="server" Text="Xuống" OnClick="Unnamed_Click"></asp:LinkButton>
                                        </div>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <%--<asp:BoundField DataField="DienThoai" HeaderText="DienThoai" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Center" />
                                    <asp:BoundField DataField="MaSoThue" HeaderText="MaSoThue" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Center" />--%>

                                <asp:BoundField DataField="DiaChi" HeaderText="Địa chỉ" NullDisplayText="-" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Center">
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Fax" HeaderText="Fax" NullDisplayText="-" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Center">
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <%--<asp:BoundField DataField="Website" HeaderText="Website" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Center" />--%>
                                <asp:BoundField DataField="NguoiDaiDien" HeaderText="Người đại diện" NullDisplayText="-" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Center">
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:BoundField DataField="ChucVu" HeaderText="Chức vụ" NullDisplayText="-" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Center">
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:BoundField DataField="GhiChu" HeaderText="Ghi chú" NullDisplayText="-" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Center">
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:BoundField>
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
