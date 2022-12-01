<%@ Page Title="" Language="C#" MasterPageFile="~/Master_Default.Master" AutoEventWireup="true" CodeBehind="NguoiSuDung_GioiHanGiamTru.aspx.cs" Inherits="Website.admin.NguoiSuDung_GioiHanGiamTru" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder_Text" runat="server">
    <script type="text/javascript">
        function isNumeric(n) {
            return !isNaN(parseFloat(n)) && isFinite(n);
        }

        function IsValidGioiHanGiamTru()
        {
            var maxGiamTru = $("#ContentPlaceHolder_Main_ContentPlaceHolder_Text_txtGioiHanKhauTruMax").val();
            var minGiamTru = $("#ContentPlaceHolder_Main_ContentPlaceHolder_Text_txtGioiHanKhauTruMin").val();           
            if (!$.isNumeric(maxGiamTru))
            {
                alert('Bạn phải nhập số tiền được giảm trừ tối đa');
                return false;
            }
            if (!$.isNumeric(minGiamTru))
            {
                alert('Bạn phải nhập số tiền giảm trừ tối thiểu');
                return false;
            }
            return true;
        }

        $("[id*=chkHeader]").live("click", function () {
            var chkHeader = $(this);
            var grid = $(this).closest("table");
            $("input[type=checkbox]", grid).each(function () {
                if (chkHeader.is(":checked")) {
                    $(this).attr("checked", "checked");
                    $("td", $(this).closest("tr")).addClass("selected");
                } else {
                    $(this).removeAttr("checked");
                    $("td", $(this).closest("tr")).removeClass("selected");
                }
            });
        });
        $("[id*=chkChon]").live("click", function () {
            var grid = $(this).closest("table");
            var chkHeader = $("[id*=chkHeader]", grid);
            if (!$(this).is(":checked")) {
                $("td", $(this).closest("tr")).removeClass("selected");
                chkHeader.removeAttr("checked");
            } else {
                $("td", $(this).closest("tr")).addClass("selected");
                if ($("[id*=chkChon]", grid).length == $("[id*=chkChon]:checked", grid).length) {
                    chkHeader.attr("checked", "checked");
                }
            }
        });
    </script>

    <div>
        <asp:Label ID="lblErrorMessage" ForeColor="Red" runat="server"></asp:Label>
    </div>
    <table class="tbl_style" width="100%">
        <tr>
            <td valign="top">
                <table class="tbl_style" width="100%">
                    <tr>
                        <td>
                            Đơn vị
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlDonVi" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlDonVi_SelectedIndexChanged"></asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Phòng ban
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlPhongBan" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlPhongBan_SelectedIndexChanged"></asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>Giới hạn khấu trừ max</td>
                        <td>
                            <asp:TextBox ID="txtGioiHanKhauTruMax" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>Giới hạn khấu trừ min</td>
                        <td>
                            <asp:TextBox ID="txtGioiHanKhauTruMin" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:GridView ID="gvUser" CssClass="table table-bordered table-hover table-striped table-condensed" runat="server" DataKeyNames="Id" AutoGenerateColumns="False">
                                <Columns>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <asp:CheckBox ID="chkHeaderUser" runat="server" />
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chkChon" runat="server"></asp:CheckBox>
                                        </ItemTemplate>                                        
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="TenTruyCap" HeaderText="Tên truy cập" />
                                    <asp:BoundField DataField="TenDayDu" HeaderText="Họ tên" />
                                </Columns>
                            </asp:GridView>
                        </td>
                    </tr>
                </table>
            </td>
            <td valign="top" style="text-align:center; padding-top:200px">
                <asp:Button ID="btnAdd" runat="server" CssClass="btn_style_button" Text=">>>" OnClick="btnAdd_Click" OnClientClick="return IsValidGioiHanGiamTru();" />
            </td>
            <td valign="top">
                <table class="tbl_style" width="100%">
                    <tr>
                        <td>
                            <asp:DropDownList ID="ddlDonVi_GioiHanGiamTru" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlDonVi_GioiHanGiamTru_SelectedIndexChanged"></asp:DropDownList>
                            <asp:DropDownList ID="ddlPhongBan_GioiHanGiamTru" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlPhongBan_GioiHanGiamTru_SelectedIndexChanged"></asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Button ID="btnDelete" runat="server" CssClass="btn_style_button" Text="Xóa" OnClientClick="return confirm('Bạn có chắc chắn muốn xóa không ?');" OnClick="btnDelete_Click"/>
                            <asp:Button ID="btnCapNhat" runat="server" CssClass="btn_style_button"  Text="Cập nhật" OnClick="btnCapNhat_Click"/>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:GridView ID="gvUserGioiHanGiamTru" CssClass="table table-bordered table-hover table-striped table-condensed" runat="server" DataKeyNames="Id" 
                                AutoGenerateColumns="False" 
                                AllowPaging="True" OnPageIndexChanging="gvUserGioiHanGiamTru_PageIndexChanging"
                                OnRowCommand="gvUserGioiHanGiamTru_RowCommand">
                                <Columns>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <asp:CheckBox ID="chkHeaderGioiHanGiamTru" runat="server" />
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chkChon" runat="server"></asp:CheckBox>
                                        </ItemTemplate>     
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="TenTruyCap" HeaderText="Tên truy cập" />
                                    <asp:TemplateField HeaderText="Số tiền giảm trừ tối đa">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtMocKhauTruMax" Text='<%#Eval("MocKhauTruMax", "{0:###,###,###}") %>' runat="server"></asp:TextBox>
                                        </ItemTemplate>     
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Số tiền giảm trừ tối thiểu">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtMocKhauTruMin" Text='<%#Eval("MocKhauTruMin", "{0:###,###,###}") %>' runat="server"></asp:TextBox>
                                        </ItemTemplate>     
                                    </asp:TemplateField>
                                   <%-- <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lbDelete" CommandAgrument='<%# Eval("Id") %>' runat="server"
                                                 OnClientClick="return confirm('Bạn có chắc chắn muốn xóa không ?');" >Xóa</asp:LinkButton>
                                        </ItemTemplate>                                        
                                    </asp:TemplateField>--%>
                                </Columns>
                            </asp:GridView>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    
</asp:Content>
