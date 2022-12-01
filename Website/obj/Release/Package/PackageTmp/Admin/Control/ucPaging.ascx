<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucPaging.ascx.cs" Inherits="VTN_QLTS.AppCode.ucPaging" %>

<div class="DDPager">
    <span style="float: left">

        <asp:ImageButton AlternateText="Trang đầu tiên" ToolTip="Trang đầu tiên" ID="ImageButtonFirst"
            runat="server" ImageUrl="~/admin/Images/PgFirst.gif" Width="8" Height="9" />
        &nbsp;
        <asp:ImageButton AlternateText="Trang trước" ToolTip="Trang trước" ID="ImageButtonPrev"
            runat="server" ImageUrl="~/admin/Images/PgPrev.gif" Width="5" Height="9" />
        &nbsp;
        <asp:Label ID="LabelPage" runat="server" Text="Trang " AssociatedControlID="TextBoxPage" />
        <asp:TextBox ID="TextBoxPage" runat="server" Columns="5" AutoPostBack="true" OnTextChanged="TextBoxPage_TextChanged"
            Width="20px" CssClass="DDControl" />
        /
        <asp:Label ID="LabelNumberOfPages" runat="server" />
        &nbsp;
        <asp:ImageButton AlternateText="Trang tiếp theo" ToolTip="Trang tiếp theo" ID="ImageButtonNext"
            runat="server" ImageUrl="~/admin/Images/PgNext.gif" Width="5" Height="9" />
        &nbsp;
        <asp:ImageButton AlternateText="Trang cuối cùng" ToolTip="Trang cuối cùng" ID="ImageButtonLast"
            runat="server" ImageUrl="~/admin/Images/PgLast.gif" Width="8" Height="9" />

    </span>

    <span style="float: right">
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
        /&nbsp Tổng số: 10
    </span>
</div>
