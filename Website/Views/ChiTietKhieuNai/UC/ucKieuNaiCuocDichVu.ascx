<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucKieuNaiCuocDichVu.ascx.cs"
    Inherits="Website.Views.KhieuNai.UC.ucKieuNaiCuocDichVu" %>


<asp:UpdatePanel ID="UPKieuNaiCuocDichVu" runat="server">
    <ContentTemplate>
        <script type="text/javascript">
            function FormatNumber(obj) {
                var strvalue;
                if (eval(obj))
                    strvalue = eval(obj).value;
                else
                    strvalue = obj;
                var num;
                num = strvalue.toString().replace(/\$|\,|\./g, '');
                if (isNaN(num))
                    num = "";
                sign = (num == (num = Math.abs(num)));
                num = Math.floor(num * 100 + 0.50000000001);
                num = Math.floor(num / 100).toString();
                for (var i = 0; i < Math.floor((num.length - (1 + i)) / 3) ; i++)
                    num = num.substring(0, num.length - (4 * i + 3)) + '.' +
                    num.substring(num.length - (4 * i + 3));
                eval(obj).value = (((sign) ? '' : '-') + num);
            }

            function fnChangeDichVu() {
                $('#' + id).click(function () {
                });
            }

            $(function () {
                $('.txt-SoTien-Edit').blur(function () {
                    FormatNumber($('.txt-SoTien-Edit').val());
                });

            });
        </script>
        <%--<div style="width: 100%; height:37px; display: block;">
        <table class="nobor" style="float: right; width: 350px;">

            <tr>

                <td style="text-align: right; width: 100px;">Lý do giảm trừ: 
                </td>
                <td>
                    <div class="selectstyle_longlx">
                        <div class="bg">
                            <asp:DropDownList ID="ddlLyDoGiamTru" runat="server"></asp:DropDownList>

                            <asp:RequiredFieldValidator ID="rfvLyDoGiamTru" ValidationGroup="InsertKieuNaiCuocDichVu"
                                runat="server" ControlToValidate="ddlLyDoGiamTru" ErrorMessage="Bạn phải chọn lý do giảm trừ"
                                Display="None" ToolTip="Bạn chưa chọn lý do giảm trừ" SetFocusOnError="true"
                                InitialValue="0"
                                ForeColor="Red">*</asp:RequiredFieldValidator>
                        </div>
                    </div>
                </td>
            </tr>
        </table>
        </div>--%>

        <asp:GridView ID="gvKieuNaiCuocDichVu" runat="server" AutoGenerateColumns="False"
            CssClass="tbl_style NoPadding customized" PagerStyle-CssClass="" AlternatingRowStyle-CssClass="rowA"
            RowStyle-CssClass="rowB" ShowFooter="True" AllowPaging="true" PageSize="6" CellPadding="0"
            CellSpacing="0" BorderWidth="1" EditRowStyle-CssClass="" FooterStyle-CssClass="gridFooterRow"
            OnRowCancelingEdit="gvKieuNaiCuocDichVu_RowCancelingEdit" OnPageIndexChanging="gvKieuNaiCuocDichVu_PageIndexChanging"
            OnRowCommand="gvKieuNaiCuocDichVu_RowCommand" OnRowDataBound="gvKieuNaiCuocDichVu_RowDataBound"
            OnRowDeleting="gvKieuNaiCuocDichVu_RowDeleting" OnRowEditing="gvKieuNaiCuocDichVu_RowEditing"
            OnRowUpdating="gvKieuNaiCuocDichVu_RowUpdating" DataKeyNames="Id" ShowHeaderWhenEmpty="true" Width="100%">
            <Columns>
                <asp:TemplateField HeaderText="Số tiền giảm trừ (VNĐ) <br/> (Trả sau không nhập VAT)" ItemStyle-Width="120px" ItemStyle-HorizontalAlign="Right">
                    <EditItemTemplate>
                        <div class="inputstyle_longlx">
                            <div class="bg">
                                <span>
                                    <asp:Label ID="txtSoTien" runat="server" Text='<%# String.Format("{0:N0}",  Eval("SoTien")).Replace(",",".") %>'></asp:Label>
                            </div>
                        </div>
                    </EditItemTemplate>
                    <FooterTemplate>
                        <div class="inputstyle_longlx">
                            <div class="bg">
                                <span>
                                    <asp:TextBox ID="txtSoTien" Width="100%" runat="server"></asp:TextBox></span>
                            </div>
                        </div>

                        <asp:RequiredFieldValidator ID="rfvSoTien" ValidationGroup="InsertKieuNaiCuocDichVu"
                            runat="server" ControlToValidate="txtSoTien" ErrorMessage="Bạn chưa nhập Số tiền giảm trừ"
                            Display="None" ToolTip="Bạn chưa nhập nội dung xử lý" SetFocusOnError="true"
                            ForeColor="Red">*</asp:RequiredFieldValidator>

                        <asp:RegularExpressionValidator ID="reSoTien" runat="server" ControlToValidate="txtSoTien"
                            ErrorMessage="Chỉ nhập số" ToolTip="Chỉ nhập số" Display="None" SetFocusOnError="true"
                            ForeColor="Red" ValidationExpression="[0-9]+(\.[0-9]*){0,}" ValidationGroup="InsertKieuNaiCuocDichVu">*</asp:RegularExpressionValidator>
                    </FooterTemplate>
                    <ItemTemplate>
                        <asp:Label ID="lblSoTien" runat="server" Text='<%# String.Format("{0:N0}",  Eval("SoTien")).Replace(",",".") %>'></asp:Label>
                        <%--<span>
                            <%# String.Format("{0:N0}",  Eval("SoTien")).Replace(",",".") %></span>--%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Tiền chỉnh sửa <br/> (Trả sau không nhập VAT)" ItemStyle-Width="140px" ItemStyle-HorizontalAlign="Right">
                    <EditItemTemplate>
                        <div class="inputstyle_longlx">
                            <div class="bg">
                                <span>
                                    <asp:TextBox ID="txtSoTien_Edit" CssClass="txt-SoTien-Edit" runat="server" Width="100%" Text='<%# String.Format("{0:0}",  Eval("SoTien_Edit")) %>'></asp:TextBox>
                                </span>
                            </div>
                        </div>
                        <asp:RequiredFieldValidator ID="rfvSoTien_Edit" ValidationGroup="UpdateKieuNaiCuocDichVu"
                            runat="server" ControlToValidate="txtSoTien_Edit" ErrorMessage="Bạn chưa nhập số tiền chỉnh sửa"
                            Display="None" ToolTip="Bạn chưa nhập SoTien_Edit" SetFocusOnError="true" ForeColor="Red">*</asp:RequiredFieldValidator>
                        <asp:RegularExpressionValidator ID="reSoTien_Edit" runat="server" ControlToValidate="txtSoTien_Edit"
                            ErrorMessage="Chỉ nhập số" ToolTip="Chỉ nhập số" Display="None" SetFocusOnError="true"
                            ForeColor="Red" ValidationExpression="[0-9]+(\.[0-9]*){0,}" ValidationGroup="UpdateKieuNaiCuocDichVu">*</asp:RegularExpressionValidator>
                    </EditItemTemplate>
                    <FooterTemplate>
                    </FooterTemplate>
                    <ItemTemplate>
                        <asp:Label ID="lblSoTien_Edit" runat="server" Text='<%# String.Format("{0:N0}",  Eval("SoTien_Edit")).Replace(",",".") %>'></asp:Label>
                        <%--<span>
                            <%# String.Format("{0:N0}",  Eval("SoTien_Edit")).Replace(",",".") %></span>--%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Loại tài khoản" ItemStyle-Width="150px">
                    <EditItemTemplate>
                        <div class="selectstyle_longlx">
                            <div class="bg" style="position: relative">
                                <span>
                                    <asp:HiddenField ID="hdLoaiTien" Value='<%# Eval("LoaiTien") %>' runat="server" />
                                    <asp:DropDownList ID="ddlLoaiTien" CssClass="ie7selauto" runat="server" Width="100%">
                                    </asp:DropDownList>
                                </span>
                            </div>
                        </div>
                    </EditItemTemplate>
                    <FooterTemplate>
                        <div class="selectstyle_longlx">
                            <div class="bg" style="position: relative">
                                <span>
                                    <asp:DropDownList ID="ddlLoaiTien" CssClass="ie7selauto" Width="100%" runat="server">
                                    </asp:DropDownList>
                                </span>
                            </div>
                        </div>
                    </FooterTemplate>
                    <ItemTemplate>
                        <span>
                            <%# GetLoaiTien(Eval("LoaiTien")) %></span>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Ghi chú" ItemStyle-Width="150px">
                    <EditItemTemplate>
                        <div class="inputstyle_longlx">
                            <div class="bg">
                                <span>
                                    <asp:TextBox ID="txtGhiChu" runat="server" Width="100%" Text='<%# Bind("GhiChu") %>' CssClass="mw"
                                        TextMode="MultiLine"></asp:TextBox></span>
                            </div>
                        </div>
                    </EditItemTemplate>
                    <FooterTemplate>
                        <div class="inputstyle_longlx">
                            <div class="bg">
                                <span>
                                    <asp:TextBox ID="txtGhiChu" runat="server" CssClass="mw" Width="100%" TextMode="MultiLine"></asp:TextBox></span>
                            </div>
                        </div>
                    </FooterTemplate>
                    <ItemTemplate>
                        <span>
                            <%# Eval("GhiChu")%></span>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Lĩnh vực chung" ItemStyle-Width="150px">
                    <EditItemTemplate>
                        <div class="selectstyle_longlx">
                            <div class="bg">
                                <span class="edit">
                                    <asp:HiddenField ID="hdfLinhVucChungId" Value='<%# Eval("LinhVucChungId") %>' runat="server" />
                                    <asp:DropDownList ID="ddlLinhVucChung" AutoPostBack="true" OnSelectedIndexChanged="ddlLinhVucChungEdit_SelectedIndexChanged" Width="100%" CssClass="ie7selauto" runat="server">
                                    </asp:DropDownList>
                            </div>
                            <asp:RequiredFieldValidator ValidationGroup="UpdateKieuNaiCuocDichVu" runat="server" Display="None" ControlToValidate="ddlLinhVucChung" ErrorMessage="Vui lòng chọn lĩnh vực chung" InitialValue="0"></asp:RequiredFieldValidator>
                        </div>
                    </EditItemTemplate>
                    <FooterTemplate>
                        <div class="selectstyle_longlx">
                            <div class="bg">
                                <span class="footer">
                                    <asp:DropDownList ID="ddlLinhVucChung" AutoPostBack="true" OnSelectedIndexChanged="ddlLinhVucChungFooter_SelectedIndexChanged" Width="100%" CssClass="ie7selauto" runat="server">
                                    </asp:DropDownList>
                            </div>
                        </div>
                        <asp:RequiredFieldValidator ValidationGroup="InsertKieuNaiCuocDichVu" runat="server" Display="None" ControlToValidate="ddlLinhVucChung" ErrorMessage="Vui lòng chọn lĩnh vực chung" InitialValue="0"></asp:RequiredFieldValidator>
                    </FooterTemplate>
                    <ItemTemplate>
                        <span><%# Eval("LinhVucChung")%></span>
                        <span></span>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Lĩnh vực con" ItemStyle-Width="150px">
                    <EditItemTemplate>
                        <div class="selectstyle_longlx">
                            <div class="bg">
                                <span>
                                    <asp:HiddenField ID="hdDichVuId" Value='<%# Eval("LinhVucConId") %>' runat="server" />
                                    <asp:DropDownList ID="ddlDichVuCP" Width="100%" CssClass="ie7selauto" runat="server">
                                    </asp:DropDownList>
                            </div>
                            <asp:RequiredFieldValidator ID="ValidateDichVuCon" ValidationGroup="UpdateKieuNaiCuocDichVu" runat="server" Display="None" ControlToValidate="ddlDichVuCP" ErrorMessage="Vui lòng chọn lĩnh vực con" InitialValue="0"></asp:RequiredFieldValidator>
                        </div>
                    </EditItemTemplate>
                    <FooterTemplate>
                        <div class="selectstyle_longlx">
                            <div class="bg">
                                <span>
                                    <asp:DropDownList ID="ddlDichVuCP" Width="100%" CssClass="ie7selauto" runat="server">
                                    </asp:DropDownList>
                            </div>
                            <asp:RequiredFieldValidator ID="ValidateDichVuCon" ValidationGroup="InsertKieuNaiCuocDichVu" runat="server" Display="None" ControlToValidate="ddlDichVuCP" ErrorMessage="Vui lòng chọn lĩnh vực con" InitialValue="0"></asp:RequiredFieldValidator>
                        </div>
                    </FooterTemplate>
                    <ItemTemplate>
                        <span><%# Eval("MaDichVu")%></span>
                        <span></span>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Trạng thái phê duyệt" ItemStyle-Width="60px">
                    <EditItemTemplate>
                    </EditItemTemplate>
                    <FooterTemplate>
                    </FooterTemplate>
                    <ItemTemplate>
                        <span>
                            <asp:CheckBox ID="chkIsDaBuTien" Enabled="false" runat="server" Checked='<%# Convert.ToBoolean(DataBinder.Eval(Container.DataItem, "IsDaBuTien")) %>' />
                        </span>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Người tạo" ItemStyle-Width="100px">
                    <EditItemTemplate>
                        <span>
                            <%# Eval("CUser")%></span>
                    </EditItemTemplate>
                    <FooterTemplate>
                    </FooterTemplate>
                    <ItemTemplate>
                        <span>
                            <%# Eval("CUser")%></span>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Người sửa" ItemStyle-Width="100px">
                    <EditItemTemplate>
                        <span>
                            <%# BindNguoiSua(Eval("LUser"), Eval("SoTien_Edit"))%></span>
                    </EditItemTemplate>
                    <FooterTemplate>
                    </FooterTemplate>
                    <ItemTemplate>
                        <span>
                            <%# BindNguoiSua(Eval("LUser"), Eval("SoTien_Edit"))%></span>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Ngày Khai báo" ItemStyle-HorizontalAlign="Center">
                    <EditItemTemplate>
                        <span>
                            <%# string.Format("{0:d/M/yyyy HH:mm}",DateTime.Now)%></span>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <span>
                            <%# string.Format("{0:d/M/yyyy HH:mm}",Eval("CDate"))%></span>
                    </ItemTemplate>
                    <FooterTemplate>
                        <span>
                            <%# string.Format("{0:d/M/yyyy HH:mm}",DateTime.Now)%></span>
                    </FooterTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Thao tác" ShowHeader="False" ItemStyle-HorizontalAlign="Center"
                    FooterStyle-HorizontalAlign="Center">
                    <EditItemTemplate>
                        <asp:HiddenField runat="server" ID="hdId" Value='<%#Eval("Id") %>' />
                        <asp:HiddenField runat="server" ID="hdCUser" Value='<%#Eval("CUser") %>' />
                        <span>
                            <asp:LinkButton ID="lnkUpdate" runat="server" CausesValidation="True" CommandName="Update"
                                CssClass="mybtn" OnClientClick="return confirm('Bạn có muốn sửa?')" ValidationGroup="UpdateKieuNaiCuocDichVu"><span class="save">Cập nhật</span></asp:LinkButton>
                            <asp:ValidationSummary ID="vsUpdateKieuNaiCuocDichVu" runat="server" ShowMessageBox="true"
                                ShowSummary="false" ValidationGroup="UpdateKieuNaiCuocDichVu" Enabled="true"
                                HeaderText="Lỗi dữ liệu..." />
                            <asp:LinkButton ID="lnkCancel" runat="server" CausesValidation="False" CommandName="Cancel"
                                CssClass="mybtn"><span class="cancel">Hủy</span></asp:LinkButton>
                            <asp:LinkButton ID="lnkDelete" runat="server" CausesValidation="False" CommandName="Delete"
                                CssClass="mybtn" OnClientClick="return confirm('Bạn có muốn xóa?')"><span class="del_file">Xoá</span></asp:LinkButton>
                        </span>
                    </EditItemTemplate>
                    <FooterTemplate>
                        <asp:HiddenField runat="server" ID="hdId" Value='<%#Eval("Id") %>' />
                        <span>
                            <asp:LinkButton ID="lnkAdd" runat="server" CausesValidation="True" CommandName="Insert" ValidationGroup="InsertKieuNaiCuocDichVu" CssClass="mybtn"> <span class="nhapdl">Thêm mới</span></asp:LinkButton></span>
                        <asp:ValidationSummary ID="vsInsertKieuNaiCuocDichVu" runat="server" ShowMessageBox="true" ShowSummary="false" ValidationGroup="InsertKieuNaiCuocDichVu" Enabled="true" HeaderText="Lỗi nhập dữ liệu" />
                    </FooterTemplate>
                    <ItemTemplate>
                        <asp:HiddenField runat="server" ID="hdCUser" Value='<%#Eval("CUser") %>' />
                        <asp:HiddenField runat="server" ID="hdIsBuTien" Value='<%#Eval("IsDaBuTien") %>' />
                        <asp:HiddenField runat="server" ID="hdId" Value='<%#Eval("Id") %>' />
                        <span>
                            <asp:LinkButton ID="lnkChuaBuTien" runat="server" Text="Chưa bù tiền" CommandArgument='<%#Eval("Id") %>' CausesValidation="False" CommandName="cmdChuaBuTien" CssClass="mybtn"><span class="save">Chưa phê duyệt</span></asp:LinkButton>
                            <asp:LinkButton ID="lnkDaBuTien" runat="server" Text="Đã bù tiền" CommandArgument='<%#Eval("Id") %>' CausesValidation="False" CommandName="cmdDaBuTien" CssClass="mybtn"><span class="del_file">Đã phê duyệt</span></asp:LinkButton>
                            <asp:LinkButton ID="lnkEdit" runat="server" CausesValidation="False" CommandName="Edit" CssClass="mybtn"> <span class="edit">Sửa</span></asp:LinkButton>
                            <asp:LinkButton ID="lnkDelete" runat="server" CausesValidation="False" CommandName="Delete" CssClass="mybtn" OnClientClick="return confirm('Bạn có muốn xóa?')"><span class="del_file">Xoá</span></asp:LinkButton>
                        </span>
                    </ItemTemplate>
                    <HeaderStyle />
                </asp:TemplateField>
            </Columns>
            <EmptyDataTemplate>
                <tr class="gridRow">
                    <td colspan="8">
                        <span>Chưa có dữ liệu...</span>
                    </td>
                </tr>
                <tr class="gridFooterRow<%=taomoi?"":" dpn" %>">
                    <td style="width: 120px;">
                        <div class="inputstyle_longlx">
                            <div class="bg">
                                <span>
                                    <asp:TextBox ID="txtSoTien" Width="100%" runat="server"></asp:TextBox></span>
                            </div>
                        </div>

                        <asp:RequiredFieldValidator ID="rfvtSoTien" ValidationGroup="InsertKieuNaiCuocDichVu"
                            runat="server" ControlToValidate="txtSoTien" ErrorMessage="Bạn chưa nhập Số tiền giảm trừ"
                            Display="None" ToolTip="Bạn chưa nhập tên" SetFocusOnError="true" ForeColor="Red">*</asp:RequiredFieldValidator>
                        <asp:RegularExpressionValidator ID="reSoTien" runat="server" ControlToValidate="txtSoTien"
                            ErrorMessage="Chỉ nhập số" ToolTip="Chỉ nhập số" Display="None" SetFocusOnError="true"
                            ForeColor="Red" ValidationExpression="[0-9]+(\.[0-9]*){0,}" ValidationGroup="InsertKieuNaiCuocDichVu">*</asp:RegularExpressionValidator>
                    </td>
                    <td style="width: 120px;"></td>
                    <td style="width: 150px;">
                        <div class="selectstyle_longlx">
                            <div class="bg" style="position: relative">
                                <span>
                                    <asp:DropDownList ID="ddlLoaiTien" Width="100%" CssClass="ie7selauto" runat="server">
                                    </asp:DropDownList>
                                </span>
                            </div>
                        </div>
                    </td>
                    <td style="width: 150px;">
                        <div class="inputstyle_longlx">
                            <div class="bg">
                                <span>
                                    <asp:TextBox ID="txtGhiChu" Width="100%" runat="server"></asp:TextBox></span>
                            </div>
                        </div>
                    </td>

                    <td style="width: 150px;">
                        <div class="selectstyle_longlx">
                            <div class="bg abc">
                                <span>
                                    <asp:DropDownList ID="ddlLinhVucChung" AutoPostBack="true" OnSelectedIndexChanged="ddlLinhVucChung_SelectedIndexChanged" Width="100%" CssClass="ie7selauto" runat="server">
                                    </asp:DropDownList>
                            </div>
                            <asp:RequiredFieldValidator ValidationGroup="InsertKieuNaiCuocDichVu" runat="server" Display="None" ControlToValidate="ddlLinhVucChung" ErrorMessage="Vui lòng chọn lĩnh vực chung" InitialValue="0"></asp:RequiredFieldValidator>
                        </div>
                    </td>

                    <td style="width: 150px;">
                        <div class="selectstyle_longlx">
                            <div class="bg">
                                <span><%--OnSelectedIndexChanged="ddlDichVuCP_SelectedIndexChanged"--%>
                                    <asp:DropDownList ID="ddlDichVuCP" Width="100%" CssClass="ie7selauto" runat="server">
                                    </asp:DropDownList>
                            </div>
                        </div>
                        <asp:RequiredFieldValidator ID="ValidateDichVuCon" ValidationGroup="InsertKieuNaiCuocDichVu" runat="server" Display="None" ControlToValidate="ddlDichVuCP" ErrorMessage="Vui lòng chọn lĩnh vực con" InitialValue="0"></asp:RequiredFieldValidator>
                    </td>
                    <%--<td style="width: 80px;">
                        <div class="inputstyle_longlx">
                            <div class="bg">
                                <span>
                                    <asp:TextBox ID="txtDauSoCP" runat="server" Width="100%" CssClass="DauSoCP mw"></asp:TextBox></span>
                            </div>
                        </div>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtDauSoCP"
                            ErrorMessage="Đầu số dịch vụ chưa hợp lệ" ToolTip="Đầu số dịch vụ chưa hợp lệ" Display="None" SetFocusOnError="true"
                            ForeColor="Red" ValidationExpression="^[123456789]{1}[0123456789x]{1,}" ValidationGroup="InsertKieuNaiCuocDichVu">*</asp:RegularExpressionValidator>
                    </td>--%>
                    <td style="width: 60px;"></td>
                    <td style="width: 100px;"></td>
                    <td style="width: 100px;"></td>
                    <td colspan="2" style="text-align: center;">
                        <span>
                            <asp:LinkButton ID="lnkAdd" runat="server" ValidationGroup="InsertKieuNaiCuocDichVu" CausesValidation="true" CommandName="emptyInsert" CssClass="mybtn"> <span class="nhapdl">Thêm mới</span></asp:LinkButton></span>
                        <asp:ValidationSummary ID="vsInsertKieuNaiCuocDichVu" runat="server" ShowMessageBox="true" ShowSummary="false" ValidationGroup="InsertKieuNaiCuocDichVu" Enabled="true" HeaderText="Lỗi nhập dữ liệu" />
                    </td>
                </tr>
            </EmptyDataTemplate>
        </asp:GridView>
    </ContentTemplate>
</asp:UpdatePanel>
