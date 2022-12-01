<%@ Page Language="C#" MasterPageFile="~/admin/adminNotAJAX.master" AutoEventWireup="true" Inherits="admin_AddAdminToGroup" Title="Untitled Page" Codebehind="AddAdminToGroup.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder_Text" Runat="Server">

<SCRIPT language="javascript">
$(function(){ 
    // add multiple select / deselect functionality
    $("#selectallRead").click(function () {
          $('.chkAllRead').find("input").attr('checked', this.checked);
    });
 
    // if all checkbox are selected, check the selectall checkbox
    // and viceversa
    $(".chkAllRead").click(function(){
 
        if($(".chkAllRead").find("//input[checked='checked']").length == $(".chkAllRead").find("input").length) {
            $("#selectallRead").attr("checked", "checked");
        } else {
            $("#selectallRead").removeAttr("checked");
        }
 
    });    
});
</SCRIPT>
    <table border="0" cellpadding="0" cellspacing="0" width="100%">
        <tr>
            <td style="height:20px">                
            </td>
        </tr>           
       <tr>        
        <td style="text-align:left; width:25%">
           Chọn nhóm:  
            <asp:DropDownList  AutoPostBack="true" ID="ddlUser" runat="server" 
                onselectedindexchanged="ddlUser_SelectedIndexChanged">
            </asp:DropDownList>
        </td>
        <td style="text-align: right; width:25%">            
        </td>  
        <td style="width:50%"></td>      
       </tr>
       <tr><td style="height:10px"></td></tr>
       <tr><td colspan="2" style="text-align: left; color:Red">
           <asp:Literal ID="lbMess" runat="server" Text=""></asp:Literal>           
       </td></tr>
        <tr>            
            <td colspan="2">
            <asp:GridView ID="grvView" runat="server" AutoGenerateColumns="False"
                    DataKeyNames="id" BackColor="White" BorderColor="#16538C" BorderStyle="Solid"
                    BorderWidth="1px" CellPadding="3" GridLines="Both" AllowPaging="True" PageSize="50"
                    AllowSorting="True" OnRowDataBound="grvView_RowDataBound" OnPageIndexChanging="grvView_PageIndexChanging" Width="550px">
                    <RowStyle BackColor="White" ForeColor="Black" />
                    <EditRowStyle BackColor="#999999" />
                    <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                    <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Right" />
                    <HeaderStyle BackColor="#2360A4" Font-Bold="True" ForeColor="White" />
                    <AlternatingRowStyle BackColor="#DEEAF3" ForeColor="Black" />
                    <Columns>
                        <asp:TemplateField ItemStyle-Width="5%" HeaderText="STT" HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-HorizontalAlign="Center"></asp:TemplateField>
                        <asp:BoundField DataField="Username" HeaderText="Tên đăng nhập" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Center" />                        
                        <asp:TemplateField HeaderText="Read">
                        <HeaderTemplate>
                            Chọn                        
                            <input type="checkbox" id="selectallRead"/>
                        </HeaderTemplate>
                        <ItemTemplate>
                                <asp:CheckBox ID="chkRead" CssClass="chkAllRead" runat="server" />
                        </ItemTemplate>                     
                        </asp:TemplateField>                              
                    </Columns>                    
                </asp:GridView>             
            </td>                        
            <td></td>
        </tr>
        <tr><td style="height:10px"></td></tr>
        <tr><td colspan="2" style="text-align:right">
            <asp:Button ID="btPhanQuyen" CssClass="button" runat="server" Text="Cập nhật" 
                onclick="btPhanQuyen_Click" />
        </td>
            <td></td>
        </tr>
        <tr><td style="height:20px"></td></tr>
    </table>

</asp:Content>

