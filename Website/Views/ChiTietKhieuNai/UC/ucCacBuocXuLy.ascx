<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucCacBuocXuLy.ascx.cs"  Inherits="Website.Views.KhieuNai.UC.ucCacBuocXuLy" %>




<asp:UpdatePanel ID="UPCacBuocXuLy" runat="server">    
    <ContentTemplate>
       
        <script src="/JS/jquery.textcomplete.js" type="text/javascript"></script>
        <script src="/JS/jquery.atooltip.js?v=1" type="text/javascript"></script>

        <link href="/CSS/atooltip.css" rel="stylesheet" type="text/css" />
    
        <script type="text/javascript">
            var arrWord = <%=WordAutocomplete%>;

            
        </script>       
        
        <div class="nav_btn">
            <ul>
                <li>
                    <asp:LinkButton ID="lbExportExcel" runat="server" OnClick="lbExportExcel_Click"><span class="ex_excel">Xuất Excel</span></asp:LinkButton>                    
                </li>
            </ul>
        </div>
        
        <input id="hidNguoiXuLy" runat="server" ClientIDMode="Static" type="hidden" value="" />

        <asp:GridView ID="gvCacBuocXuLy" runat="server" AutoGenerateColumns="False" CssClass="tbl_style NoPadding" PagerStyle-CssClass=""
            AlternatingRowStyle-CssClass="rowA" RowStyle-CssClass="rowB" ShowFooter="True" AllowPaging="true" PageSize="6" CellPadding="0" CellSpacing="0" BorderWidth="1"
            EditRowStyle-CssClass="" FooterStyle-CssClass="gridFooterRow" OnRowCancelingEdit="gvCacBuocXuLy_RowCancelingEdit" OnPageIndexChanging="gvCacBuocXuLy_PageIndexChanging"
            OnRowCommand="gvCacBuocXuLy_RowCommand" OnRowDataBound="gvCacBuocXuLy_RowDataBound" OnRowDeleting="gvCacBuocXuLy_RowDeleting"
            OnRowEditing="gvCacBuocXuLy_RowEditing" OnRowUpdating="gvCacBuocXuLy_RowUpdating" DataKeyNames="Id" ShowHeaderWhenEmpty="true">
            <Columns>
            
                <asp:TemplateField HeaderText="Nội dung xử lý">
                    <EditItemTemplate>
                        <div class="inputstyle_longlx">
                            <div class="bg">
                                <span class="inputstyle_notblock">
                                    <asp:TextBox ID="txtNoiDung" CausesValidation="false" runat="server" Text='<%# Bind("NoiDung") %>' CssClass="mw clsNoiDungXuLy" Rows="10" Height="100px" TextMode="MultiLine"></asp:TextBox></span>
                            </div>
                        </div>
                        <asp:RequiredFieldValidator ID="rfvNoiDung" ValidationGroup="UpdateCacBuocXuLy" runat="server"
                            ControlToValidate="txtNoiDung" ErrorMessage="Bạn chưa nhập nội dung xử lý" Display="None"
                            ToolTip="Bạn chưa nhập nội dung xử lý" SetFocusOnError="true" ForeColor="Red">*</asp:RequiredFieldValidator>
                    </EditItemTemplate>
                    <FooterTemplate>
                        <div class="inputstyle_longlx">
                            <div class="bg">
                                <span class="inputstyle_notblock">
                                    <asp:TextBox ID="txtNoiDung" CausesValidation="false" runat="server" Rows="10" Height="100px" CssClass="mw clsNoiDungXuLy" TextMode="MultiLine"></asp:TextBox></span>
                            </div>
                        </div>
                        <asp:RequiredFieldValidator ID="rfvNoiDung" ValidationGroup="InsertCacBuocXuLy" runat="server"
                            ControlToValidate="txtNoiDung" ErrorMessage="Bạn chưa nhập nội dung xử lý" Display="None"
                            ToolTip="Bạn chưa nhập nội dung xử lý" SetFocusOnError="true" ForeColor="Red">*</asp:RequiredFieldValidator>
                    </FooterTemplate>
                    <ItemTemplate>                        
                        <span style="white-space: normal;"><%# ((string)Eval("NoiDung")).Replace("\n", "<br/>") %></span>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Người xử lý">
                    
                        <EditItemTemplate>
                            <span><%# Eval("CUser") %></span>
                        </EditItemTemplate>
                        <FooterTemplate>
                        </FooterTemplate>
                    
                    <ItemTemplate>
                        <a href="#" class="normalTip exampleTip" title='<%# Eval("CUser") %>'><%# Eval("CUser") %></a>
                        
                        <%--<span><%# Eval("CUser") %></span>--%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Ngày xử lý">
                    <EditItemTemplate>
                        <span><%# string.Format("{0:d/M/yyyy HH:mm}",DateTime.Now)%></span>
                    </EditItemTemplate>
                    <FooterTemplate>
                        <span><%# string.Format("{0:d/M/yyyy HH:mm}",DateTime.Now)%></span>
                    </FooterTemplate>
                    <ItemTemplate>
                        <span><%# string.Format("{0:d/M/yyyy HH:mm}",Eval("CDate"))%></span>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Thao tác" ShowHeader="False" ItemStyle-HorizontalAlign="Center" FooterStyle-HorizontalAlign="Center" HeaderStyle-Width="120px">
                    <EditItemTemplate>
                    <asp:HiddenField runat="server" ID="hdCUser" Value='<%#Eval("CUser") %>' />
                        <asp:HiddenField runat="server" ID="hdPhongBanXuLyId" Value='<%#Eval("PhongBanXuLyId") %>' />
                        <span>
                            <asp:LinkButton ID="lnkUpdate" runat="server" CausesValidation="True" CommandName="Update"
                                CssClass="mybtn" OnClientClick="return confirm('Bạn có muốn sửa?')" ValidationGroup="UpdateCacBuocXuLy"><span class="save">Cập nhật</span></asp:LinkButton>
                            <asp:ValidationSummary ID="vsUpdateCacBuocXuLy" runat="server" ShowMessageBox="true" ShowSummary="false"
                                ValidationGroup="UpdateCacBuocXuLy" Enabled="true" HeaderText="Lỗi dữ liệu..." />
                            <asp:LinkButton ID="lnkCancel" runat="server" CausesValidation="False" CommandName="Cancel"
                                CssClass="mybtn"><span class="cancel">Hủy</span></asp:LinkButton>
                            <asp:LinkButton ID="lnkDelete" runat="server" CausesValidation="False" CommandName="Delete"
                                CssClass="mybtn" OnClientClick="return confirm('Bạn có muốn xóa?')"><span class="del_file">Xoá</span></asp:LinkButton></span>
                        </span>
                    </EditItemTemplate>
                    <FooterTemplate>                        
                        <span>
                            <asp:LinkButton ID="lnkAdd" runat="server" OnClientClick="return ValidHtml();" CausesValidation="True" CommandName="Insert"
                                ValidationGroup="InsertCacBuocXuLy" CssClass="mybtn"> <span class="nhapdl">Thêm mới</span></asp:LinkButton></span>
                        <asp:ValidationSummary ID="vsInsertCacBuocXuLy" runat="server" ShowMessageBox="true" ShowSummary="false"
                            ValidationGroup="InsertCacBuocXuLy" Enabled="true" HeaderText="Lỗi nhập dữ liệu" />                            
                    </FooterTemplate>
                    <ItemTemplate>
                        <asp:HiddenField runat="server" ID="hdCUser" Value='<%#Eval("CUser") %>' />
                        <asp:HiddenField runat="server" ID="hdPhongBanXuLyId" Value='<%#Eval("PhongBanXuLyId") %>' />
                        <span>
                            <asp:LinkButton ID="lnkEdit" runat="server" CausesValidation="False" CommandName="Edit"
                                CssClass="mybtn"> <span class="edit">Sửa</span></asp:LinkButton>
                            <asp:LinkButton ID="lnkDelete" runat="server" CausesValidation="False" CommandName="Delete"
                                CssClass="mybtn" OnClientClick="return confirm('Bạn có muốn xóa?')"><span class="del_file">Xoá</span></asp:LinkButton></span>
                        
                    </ItemTemplate>
                </asp:TemplateField>

            </Columns>
            <EmptyDataTemplate>
                
                <tr class="gridRow">
                    <td colspan="10"><span>Chưa có dữ liệu...</span>
                    </td>
                </tr>
               
                <tr class="gridFooterRow<%=taomoi?"":" dpn" %>">
                    <td>
                        <div class="inputstyle_longlx">
                            <div class="bg">
                                <span class="inputstyle_notblock">
                                    <asp:TextBox ID="txtNoiDung" runat="server" Rows="10" Height="100px" CssClass="mw clsNoiDungXuLy" TextMode="MultiLine"></asp:TextBox>
                            </span>
                                </div>
                            
                        </div>
                        <asp:RequiredFieldValidator ID="rfvNoiDung" ValidationGroup="InsertCacBuocXuLy" runat="server"
                            ControlToValidate="txtNoiDung" ErrorMessage="Bạn chưa nhập nội dung xử lý" Display="None"
                            ToolTip="Bạn chưa nhập nội dung xử lý" SetFocusOnError="true" ForeColor="Red">*</asp:RequiredFieldValidator>
                    </td>
                    <td></td>
                    <td></td>
                    <td colspan="2" style="text-align: center">
                        <asp:LinkButton ID="lnkAdd" runat="server" ValidationGroup="InsertCacBuocXuLy" CausesValidation="true" CommandName="emptyInsert"
                            CssClass="mybtn"> <span class="nhapdl">Thêm mới</span></asp:LinkButton>
                        <asp:ValidationSummary ID="vsInsertCacBuocXuLy" runat="server" ShowMessageBox="true" ShowSummary="false"
                            ValidationGroup="InsertCacBuocXuLy" Enabled="true" HeaderText="Lỗi nhập dữ liệu" />
                    </td>
                </tr>
            </EmptyDataTemplate>
        </asp:GridView>
        <div style="clear: both;" ></div>
        <script type="text/javascript">
            function ValidHtml()
            {
                $('.clsNoiDungXuLy').each(function () {
                    //console.log($(this).text($(this).val()).html());
                    $(this).val($(this).text($(this).val()).html());
                    //$(this).text($(this).val()).html();
                });
                
                return true;
            }

            function htmlEncode(value) {                
                return $('<div/>').text(value).html();
            }

            function htmlDecode(value) {
                return $('<div/>').html(value).text();
            }

            //function ShowToolTip()
            //{
            //    $('a.normalTip').aToolTip();
            //}

            function AutocompleteText(controlId) {
                //console.log(controlId);
                $('#' + controlId + '').textcomplete([
                       { // tech companies
                           words: arrWord,
                           match: /\b(\w{1,})$/,
                           search: function (term, callback) {
                               //console.log(term);
                               callback($.map(this.words, function (word) {
                                   //console.log(word.indexOf(term));
                                   //console.log(word.indexOf(term) == 0);
                                   return word.indexOf(term) >= 0 ? word : null;
                               }));
                           },
                           index: 1,
                           replace: function (word) {
                               return word + ' ';
                           }
                       }
                ]);

                $('a.normalTip').aToolTip();
            }
            AutocompleteText('ContentPlaceHolder_Main_ContentPlaceHolder_Text_ucChiTietKhieuNai_TabContainer1_TabPanel1_UcCacBuocXuLy_gvCacBuocXuLy_txtNoiDung');
        </script>
    </ContentTemplate>
</asp:UpdatePanel>

