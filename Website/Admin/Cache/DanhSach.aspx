<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DanhSach.aspx.cs" MasterPageFile="~/Master_Default.master" Inherits="Website.Admin.Cache.DanhSach" %>

<asp:Content runat="server" ContentPlaceHolderID="HeaderCss">
    <style type="text/css">
        body .tbl_style.tblcus.width1 .colstt { width: 45px; text-align: center; }
        body .tbl_style.tblcus.width1 .colkey { width: 50%; }
    </style>
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="ContentPlaceHolder_Main">
    <div class="p10">
        <asp:GridView runat="server" ID="GrvViews" CssClass="table table-bordered table-hover table-striped table-condensed" AutoGenerateColumns="false" Width="100%">
            <Columns>
                <asp:TemplateField HeaderText="STT" HeaderStyle-CssClass="colstt" ItemStyle-CssClass="colstt">
                    <ItemTemplate>
                        <%# Container.DataItemIndex + 1 %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Khóa" HeaderStyle-CssClass="colkey" ItemStyle-CssClass="colkey">
                    <ItemTemplate>
                        <%# Eval("Key") %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Kiểu">
                    <ItemTemplate>
                        <%# Eval("TypeName") %>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
        <div style="height: 10px;"></div>
        <asp:GridView runat="server" ID="GrvViewCus" CssClass="tbl_style tblcus width1" AutoGenerateColumns="false" Width="100%">
            <Columns>
                <asp:TemplateField HeaderText="STT" HeaderStyle-CssClass="colstt" ItemStyle-CssClass="colstt">
                    <ItemTemplate>
                        <%# Container.DataItemIndex + 1 %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Khóa" HeaderStyle-CssClass="colkey" ItemStyle-CssClass="colkey">
                    <ItemTemplate>
                        <%# Eval("Key") %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Kiểu">
                    <ItemTemplate>
                        <%# Eval("TypeName") %>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>
</asp:Content>
