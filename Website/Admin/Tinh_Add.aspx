<%@ Page Language="C#" MasterPageFile="~/Master_Default.master" AutoEventWireup="true"
    Inherits="admin_tinh_add"  CodeBehind="Tinh_Add.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder_Text" runat="Server">
    <script language="javascript">
        function validForm() {
            //txtMaTinh : varchar
            <%--var name = $("#<%=txtMaDonVi.ClientID %>");
            if (name.val().trim() == "") {
                name.val("");
                name.focus();
                $.messager.alert('Thông báo', 'Không để trống trường này', 'error', function () {
                    name.focus();
                });
                return false;
            }--%>

            return true;
        }
    </script>
     <!-- begin panel nav boot -->
    <div class="nav_btn_bootstrap">
        <ul>
              <li>
                  <a href="javascript:history.back()" class="btn btn-primary"><span class="glyphicon glyphicon-circle-arrow-left" aria-hidden="true"></span> Quay về</a>

              </li>
            <li>
                <asp:LinkButton ID="linkbtnSubmit" class="btn btn-primary" OnClientClick="return validForm();" runat="server" OnClick="linkbtnSubmit_Click"><span class="glyphicon glyphicon-plus-sign"></span> Cập nhật</asp:LinkButton>
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

                         <table cellpadding="0" cellspacing="0" width="100%">
            <tr>
                <td valign="top" style="Width:200px">
                    <h3>Hướng dẫn nhập thông tin</h3>
                    - Khi không chọn Tỉnh/Thành : Hệ thống sẽ hiểu là tạo/sửa thông tin Tỉnh/Thành phố
                    <br />
                    - Khi chọn Tỉnh/Thành, không chọn Quận/Huyện : Hệ thống sẽ hiểu là tạo/sửa thông tin Quận/Huyện
                    <br />
                    - Khi chọn Tỉnh/Thành và Quận/Huyện : Hệ thống sẽ hiểu là tạo/sửa thông tin Phường/Xã
                </td>
                <td valign="top">
                    <table border="0" cellpadding="0" cellspacing="0" width="100%">
                        <tr>
                            <td style="height: 25px">
                            </td>
                        </tr>
                        <tr>                
                            <td colspan="2" align="left">
                                <asp:Label ID="lblMsg" runat="server" Text="" ForeColor="Red"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="height: 5px">
                            </td>
                        </tr>

                        <tr>
                            <td style="width: 15%; text-align: right;">
                                <strong>Chọn tỉnh:</strong>
                            </td>
                            <td style="width: 20%; text-align: left;">
                                <div class="selectstyle">
                                    <div class="bg">
                                        <asp:DropDownList ID="ddlTinh" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlTinh_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </td>               
                        </tr>
                         <tr>
                            <td style="width: 15%; text-align: right;">
                                <strong>Chọn Quận/Huyện:</strong>
                            </td>
                            <td style="width: 20%; text-align: left;">
                                <div class="selectstyle">
                                    <div class="bg">
                                        <asp:DropDownList ID="ddlQuan" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlQuan_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </td>                
                        </tr>                        
                        <tr>
                            <td style="height: 10px">
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 15%; text-align: right; padding-right: 10px">
                                <asp:Label ID="lblTitleTenDonVi" runat="server" Text="">Tên tỉnh</asp:Label>
                            </td>
                            <td style="width: 20%; text-align: left">
                            <div class="inputstyle">
                                        <div class="bg">
                                <asp:TextBox ID="txtTenDonVi" runat="server" Text="" MaxLength="100" Width="450px"></asp:TextBox>
                                <br />
                                <asp:Label ID="lblNote" runat="server" Text="">Trường hợp nhiều đơn vị thì viết danh sách các đơn vị cách nhau bởi dấu "," (phẩy)</asp:Label>
                                </div></div>
                            </td>                
                        </tr>
                        <tr>
                            <td style="height: 10px">
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 15%; text-align: right; padding-right: 10px">
                               <asp:Label ID="lblTitleMaDonVi" runat="server" Text="">Mã tỉnh</asp:Label>
                            </td>
                            <td style="width: 20%; text-align: left">
                            <div class="inputstyle">
                                        <div class="bg">
                                <asp:TextBox ID="txtMaDonVi" runat="server" Text="" MaxLength="10" Width="450px"></asp:TextBox>
                                </div></div>
                            </td>                
                        </tr>
                        <tr>
                            <td style="height: 10px">
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 15%; text-align: right; padding-right: 10px">
                                Khu vực
                            </td>
                            <td style="width: 20%; text-align: left">
                                <div class="inputstyle">
                                    <div class="bg">
                                    <asp:DropDownList ID="ddlKhuVuc" runat="server">
                                        <asp:ListItem Value="2">1</asp:ListItem>
                                        <asp:ListItem Value="3">2</asp:ListItem>
                                        <asp:ListItem Value="5">3</asp:ListItem>
                                    </asp:DropDownList>
                                    </div></div>                                
                            </td>                
                        </tr>
                        <%--<tr>
                             <td style="width: 15%; text-align: right; padding-right: 10px">
                                Trạng thái : 
                            </td>
                            <td style="width: 20%; text-align: left">
                                <div class="inputstyle">
                                    <div class="bg">
                                        <asp:CheckBox ID="chkTrangThai" runat="server" />
                                    </div></div>
                            </td>                
                        </tr>--%>
                        <tr>
                            <td style="height: 10px">
                            </td>
                        </tr>
                        <tr>
                            <td>
                            </td>
                            <td style="text-align: left">
                              <%--  <div class="foot_nav_btn">
                                    <a href="#1"><i class="save">&nbsp;</i><span><asp:Button ID="btSubmit" CssClass="button_eole"
                                        runat="server" Text="Cập nhật" OnClick="btSubmit_Click" OnClientClick="return validForm();" /></span></a>
                                    <a href="javascript:history.back()"><i class="cancel">&nbsp;</i><span>Hủy bỏ</span></a>
                                </div>--%>
                            </td>                
                        </tr>
                        <tr>
                            <td style="height: 10px">
                            </td>
                        </tr>
                        <tr>
                            <td style="height: 25px">
                            </td>
                        </tr>
                    </table>
                </td>
                <td>
                    &nbsp;
                </td>
                <td valign="top" style="width:300px">
                    <h3><asp:Label ID="lblDanhSachDonVi" runat="server" Text=""></asp:Label></h3>
                    
                    <asp:GridView ID="gvDonVi" CssClass="table table-bordered table-hover table-striped table-condensed" runat="server" AutoGenerateColumns="False" CellPadding="4" ForeColor="#333333" GridLines="None">
                        <AlternatingRowStyle BackColor="White" />
                        <Columns>
                            <asp:BoundField DataField ="Name" HeaderText="Tên" />
                        </Columns>
                        <EditRowStyle BackColor="#2461BF" />
                        <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                        <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                        <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                        <RowStyle BackColor="#EFF3FB" />
                        <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                        <SortedAscendingCellStyle BackColor="#F5F7FB" />
                        <SortedAscendingHeaderStyle BackColor="#6D95E1" />
                        <SortedDescendingCellStyle BackColor="#E9EBEF" />
                        <SortedDescendingHeaderStyle BackColor="#4870BE" />
                    </asp:GridView>
                </td>
            </tr>
        </table>
                        <!-- end body boot -->
                    </div>
                </div>
                <!-- end panel boot -->
    </div>
</asp:Content>
