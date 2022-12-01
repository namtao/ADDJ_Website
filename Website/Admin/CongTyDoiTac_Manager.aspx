<%@ Page Language="C#" MasterPageFile="~/Master_Default.master" AutoEventWireup="true" Inherits="admin_congTyDoiTac_manager" CodeBehind="CongTyDoiTac_Manager.aspx.cs" %>

<asp:Content runat="server" ContentPlaceHolderID="HeaderCss">
    <style type="text/css">
        .partner-search {
            margin: 10px 0px;
        }

            .partner-search .input-text {
                float: left;
                width: 250px;
                margin-right: 10px;
            }

            .partner-search .bnt {
                float: left;
                margin-right: 10px;
            }

            .partner-search .vide {
                clear: both;
                height: 0%;
            }

            .partner-search .msg {
                font-weight: bold;
            }

                .partner-search .msg p {
                    padding-top: 10px;
                }

        .tbl_style th {
            line-height: 175%;
            padding: 7px 0px;
        }
    </style>
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="HeaderJs">
</asp:Content>
<asp:Content ContentPlaceHolderID="Content" runat="Server">
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
            <asp:Literal Id="liJs" runat="server"></asp:Literal >
        });
    </script>
    <!-- begin panel nav boot -->
    <div class="nav_btn_bootstrap">
        <ul>
            <li><a href="javascript:history.back()" class="btn btn-primary"><span class="glyphicon glyphicon-circle-arrow-left" aria-hidden="true"></span> Quay về</a></li>
            <li>
                <asp:LinkButton ID="linkbtnThemMoi"  class="btn btn-primary" runat="server" OnClick="linkbtnThemMoi_Click">
                    <span class="glyphicon glyphicon-plus-sign" aria-hidden="true"></span> Thêm mới
                </asp:LinkButton>
            </li>
            <li>
                <asp:LinkButton ID="linkbtnXoa"  class="btn btn-danger" runat="server" OnClientClick="javascript:{return confirm('Bạn thực sự chắc chắn?');}" OnClick="linkbtnXoa_Click">
                    <span class="glyphicon glyphicon-remove-sign" aria-hidden="true"></span> Không dùng
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
                <asp:Panel runat="server" DefaultButton="btnSearch" CssClass="partner-search">
                    <div class="input-text">
                        <div class="inputstyle">
                            <div class="bg">
                                <asp:TextBox placeholder="Tìm kiếm" ID="txtKeySearch" runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <%--                <asp:RequiredFieldValidator ValidationGroup="A1" Display="None" SetFocusOnError="true" ControlToValidate="txtKeySearch" runat="server" ErrorMessage="Vui lòng nhập từ khóa"></asp:RequiredFieldValidator>--%>
                    </div>
                    <div class="form-btn">
                        <asp:Button CssClass="btn_main search" ValidationGroup="A1" ID="btnSearch" runat="server" Text="Tìm kiếm" />
                    </div>
                    <div class="msg">
                        <p>
                            <asp:Literal ID="ltThongBao" runat="server"></asp:Literal>
                        </p>
                    </div>
                    <div class="vide"></div>
                    <asp:ValidationSummary ValidationGroup="A1" ShowMessageBox="true" ShowSummary="false" runat="server" />
                </asp:Panel>
                <asp:GridView ID="GrvView" runat="server" Width="100%" AutoGenerateColumns="False" CssClass="table table-bordered table-hover table-striped table-condensed" DataKeyNames="Id" OnRowDataBound="GrvView_RowDataBound">
                    <RowStyle CssClass="rowB" />
                    <AlternatingRowStyle CssClass="rowA" />
                    <Columns>
                        <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="STT" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="5%">
                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Center" Width="5%"></ItemStyle>
                        </asp:TemplateField>
                        <asp:BoundField DataField="Ten" HeaderStyle-HorizontalAlign="Center" HeaderText="Tên CP" ItemStyle-HorizontalAlign="Left">
                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Left"></ItemStyle>
                        </asp:BoundField>
                        <asp:BoundField DataField="DiaChi" HeaderStyle-HorizontalAlign="Center" HeaderText="Địa chỉ" NullDisplayText="-" ItemStyle-HorizontalAlign="Left">
                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Left"></ItemStyle>
                        </asp:BoundField>
                        <asp:BoundField DataField="DienThoai_Fax" HeaderStyle-HorizontalAlign="Center" HeaderText="Điện thoại" NullDisplayText="-" ItemStyle-HorizontalAlign="Left">
                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Left"></ItemStyle>
                        </asp:BoundField>
                        <asp:BoundField DataField="HoTroKhachHang" HeaderStyle-HorizontalAlign="Center" HeaderText="Hỗ trợ khách hàng" NullDisplayText="-">
                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                        </asp:BoundField>
                        <asp:TemplateField HeaderText="Trạng thái" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:CheckBox runat="server" Checked='<%# ((int)Eval("TrangThai")) > 0 %>' Enabled="false" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField ItemStyle-HorizontalAlign="center">
                            <HeaderTemplate>
                                <input type="checkbox" id="selectall" />Chọn
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:CheckBox ID="cbSelectAll" runat="server" CssClass="case" />
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                <asp:AspNetPager ID="Pager" CssClass="wp-pager" PageSize="30" runat="server"
                    PagingButtonSpacing="0" CurrentPageButtonClass="selected" PagingButtonsClass="page"
                    CustomInfoClass="" CustomInfoSectionWidth="" FirstLastButtonsClass="page" MoreButtonsClass="page"
                    PrevNextButtonsClass="page" ShowPageIndexBox="Auto" FirstPageText="|<<" LastPageText=">>|"
                    NextPageText=">>" PrevPageText="<<" ShowFirstLast="True" ShowCustomInfoSection="Never" UrlPageIndexName="Page"
                    SubmitButtonText="Nhảy tới" ShowDisabledButtons="False" ShowBoxThreshold="2" InvalidPageIndexErrorMessage="Số trang phải là một số"
                    PageIndexOutOfRangeErrorMessage="Số trang vượt mức" HorizontalAlign="Right" SubmitButtonClass="SaveButton">
                </asp:AspNetPager>
                <!-- end body boot -->
            </div>
        </div>
        <!-- end panel boot -->
    </div>
</asp:Content>


