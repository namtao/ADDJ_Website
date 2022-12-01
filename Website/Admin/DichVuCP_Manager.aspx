<%@ Page Language="C#" MasterPageFile="~/Master_Default.master" AutoEventWireup="true"
    Inherits="Website.admin.DichVuCP_Manager" Title="DichVuCP Management" CodeBehind="DichVuCP_Manager.aspx.cs" %>


<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder_Text" runat="Server">
    <script src="Scripts/DichVuCP.js" type="text/javascript"></script>

    <!-- begin panel nav boot -->
    <div class="nav_btn_bootstrap">
        <asp:UpdatePanel ID="UpdatePanel3" runat="server">
            <ContentTemplate>
                <ul>
                    <li><a href="javascript:history.back()" class="btn btn-primary"><span class="glyphicon glyphicon-circle-arrow-left" aria-hidden="true"></span> Quay về</a></li>
                    <li>
                        <asp:LinkButton ID="linkbtnAdd" class="btn btn-primary" OnClientClick="DichVuCP.List().ShowEditModal()" runat="server" >
                             <span class="glyphicon glyphicon-plus-sign" aria-hidden="true"></span> Thêm mới
                        </asp:LinkButton>
                        <li>
                            <asp:LinkButton ID="linkbtnDelete" CssClass="btn btn-danger"   OnClientClick="javascript:{return confirm('Bạn có muốn xóa dịch vụ CP được chọn?');}" runat="server" OnClick="linkbtnDelete_Click">
                                  <span class="glyphicon glyphicon-remove-sign" aria-hidden="true"></span> Xóa
                            </asp:LinkButton>
                        </li>
                </ul>
            </ContentTemplate>
        </asp:UpdatePanel>
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
                            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                <ContentTemplate>
                                    <table style="border: 1px solid #d2d2d2; border-collapse: collapse; width: 100%">
                                        <tr>
                                            <td bgcolor="#f0f0f0" style="text-align: left">
                                                <h3 style="color: #3c78b5; line-height: 30px; padding-left: 15px;">Lọc tìm kiếm</h3>
                                            </td>
                                        </tr>
                                        <tr style="background: #fffff0">
                                            <td>
                                                <table border="0" width="100%">
                                                    <tr>
                                                        <td style="text-align: right;">
                                                            <strong>Mã dịch vụ:</strong>
                                                        </td>
                                                        <td style="text-align: left">
                                                            <div class="inputstyle">
                                                                <div class="bg">
                                                                    <asp:TextBox ID="txtNameFilter" runat="server"></asp:TextBox>
                                                                </div>
                                                            </div>
                                                        </td>
                                                        <td style="text-align: left">
                                                            <asp:Button ID="btFilter" runat="server" Text="Lọc" CssClass="btn_style_button" OnClick="btFilter_Click" />
                                                        </td>
                                                        <td style="text-align: right" width="50%">
                                                            <asp:Button ID="btClearFilter" OnClick="btClearFilter_Click" runat="server" CssClass="button_clear_filter"
                                                                Text="Xóa điều kiện lọc" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                </ContentTemplate>
                            </asp:UpdatePanel>
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
                            <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                <ContentTemplate>
                                    <asp:GridView ID="grvView" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered table-hover table-striped table-condensed"
                                        DataKeyNames="ID" AllowPaging="False" AllowSorting="True" OnRowDataBound="grvView_RowDataBound">
                                        <RowStyle CssClass="rowB" />
                                        <AlternatingRowStyle CssClass="rowA" />
                                        <Columns>
                                            <asp:TemplateField ItemStyle-Width="5%" HeaderText="STT" HeaderStyle-HorizontalAlign="Center"
                                                ItemStyle-HorizontalAlign="Center"></asp:TemplateField>
                                            <asp:BoundField DataField="MaDichVu" HeaderText="Mã dịch vụ" ItemStyle-HorizontalAlign="Left"
                                                HeaderStyle-HorizontalAlign="Center" />
                                            <asp:BoundField DataField="NgayBatDau" HeaderText="Ngày bắt đầu" ItemStyle-HorizontalAlign="Left"
                                                HeaderStyle-HorizontalAlign="Center" />
                                            <asp:BoundField DataField="NgayKetThuc" HeaderText="Ngày kết thúc" ItemStyle-HorizontalAlign="Left"
                                                HeaderStyle-HorizontalAlign="Center" />
                                            <asp:TemplateField HeaderText="Deactive">
                                                <ItemTemplate>
                                                    <input type="checkbox" name="chkDeactive" value="<%#Eval("Deactive") %>" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="GhiChu" HeaderText="Ghi chú" ItemStyle-HorizontalAlign="Left"
                                                HeaderStyle-HorizontalAlign="Center" />
                                            <asp:TemplateField HeaderText="Hành động">
                                                <ItemTemplate>
                                                    <a href="#" onclick="DichVuCP.List().ShowEditModal('<%#Eval("Id") %>')">Sửa</a>
                                                </ItemTemplate>
                                            </asp:TemplateField>
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
                                    <!-- 06/04/2016-->
                                    <!--Start Phân trang AJAX-->
                                    <div class="DDPager">
                                        <span style="float: left">
                                            <asp:ImageButton AlternateText="Trang đầu tiên" ToolTip="Trang đầu tiên" ID="ImageButtonFirst"
                                                runat="server" ImageUrl="~/admin/Images/PgFirst.gif" Width="8" Height="9" OnClick="ImageButtonFirst_Click" />
                                            &nbsp;
                                    <asp:ImageButton AlternateText="Trang trước" ToolTip="Trang trước" ID="ImageButtonPrev"
                                        runat="server" ImageUrl="~/admin/Images/PgPrev.gif" Width="5" Height="9" OnClick="ImageButtonPrev_Click" />
                                            &nbsp;
                                    <asp:Label ID="LabelPage" runat="server" Text="Trang " AssociatedControlID="TextBoxPage" />
                                            <asp:TextBox ID="TextBoxPage" runat="server" Columns="5" AutoPostBack="true" OnTextChanged="TextBoxPage_TextChanged"
                                                Width="20px" CssClass="DDControl" />
                                            /
                                    <asp:Label ID="LabelNumberOfPages" runat="server" />
                                            &nbsp;
                                    <asp:ImageButton AlternateText="Trang tiếp theo" ToolTip="Trang tiếp theo" ID="ImageButtonNext"
                                        runat="server" ImageUrl="~/admin/Images/PgNext.gif" Width="5" Height="9" OnClick="ImageButtonNext_Click" />
                                            &nbsp;
                                    <asp:ImageButton AlternateText="Trang cuối cùng" ToolTip="Trang cuối cùng" ID="ImageButtonLast"
                                        runat="server" ImageUrl="~/admin/Images/PgLast.gif" Width="8" Height="9" OnClick="ImageButtonLast_Click" />
                                        </span><span style="float: right">
                                            <asp:Label ID="LabelRows" runat="server" Text="Kết quả trên 1 trang:" AssociatedControlID="DropDownListPageSize" />
                                            <asp:DropDownList ID="DropDownListPageSize" runat="server" AutoPostBack="true" CssClass="DDControl"
                                                OnSelectedIndexChanged="DropDownListPageSize_SelectedIndexChanged">
                                                <asp:ListItem Value="10" />
                                                <asp:ListItem Value="15" />
                                                <asp:ListItem Value="20" />
                                                <asp:ListItem Value="30" />
                                                <asp:ListItem Value="40" />
                                                <asp:ListItem Value="50" />
                                                <asp:ListItem Value="100" />
                                            </asp:DropDownList>
                                            /&nbsp Tổng số:
                                    <asp:Literal ID="ltTongSoBanGhi" runat="server"></asp:Literal>
                                        </span>
                                    </div>
                                    <!--End Phân trang AJAX-->
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="btFilter" EventName="Click" />
                                    <asp:AsyncPostBackTrigger ControlID="btClearFilter" EventName="Click" />
                                    <asp:AsyncPostBackTrigger ControlID="linkbtnAdd" EventName="Click" />
                                </Triggers>
                            </asp:UpdatePanel>

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
    <asp:Button ID="ButtonEdit" runat="server" Text="Edit dịch vụ CP" Style="display: none" />
    <cc1:ModalPopupExtender ID="ModalPopupExtender2" BackgroundCssClass="ModalPopupBG"
        runat="server" CancelControlID="btnCancelEdit" OkControlID="btnOkayEdit" TargetControlID="ButtonEdit"
        PopupControlID="DivEditWindow" OnOkScript="DichVuCP.List().EditOkayScript();"
        BehaviorID="EditModalPopup">
    </cc1:ModalPopupExtender>
    <div class="popup_Buttons" style="display: none">
        <input id="btnOkayEdit" value="Done" type="button" />
        <input id="btnCancelEdit" value="Cancel" type="button" />
    </div>
    <div id="DivEditWindow" style="display: none;" class="popup_Container">
        <iframe id="IframeEdit" src="" frameborder="0" height="320" scrolling="no"></iframe>
    </div>
</asp:Content>
