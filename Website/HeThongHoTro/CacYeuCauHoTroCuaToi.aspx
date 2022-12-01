<%@ Page Title="" Language="C#" MasterPageFile="~/AdminNotAJAX.Master" AutoEventWireup="true" CodeBehind="CacYeuCauHoTroCuaToi.aspx.cs" Inherits="Website.HeThongHoTro.CacYeuCauHoTroCuaToi" %>
<%@ Register assembly="DevExpress.Web.v21.2, Version=21.2.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeaderCss" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderJs" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder_Text" runat="server">
      <div style="padding:20px;">
    <dx:ASPxRoundPanel ID="ASPxRoundPanel1" runat="server" HeaderText="Danh sách các yêu cầu hỗ trợ của tôi" ShowCollapseButton="true" Width="100%" Theme="Office2010Silver">
        <PanelCollection>
            <dx:PanelContent runat="server">
                <script type="text/javascript">
                    $(function () {
                        DanhSachHoTroCuaToi.PerformCallback();
                    });
                </script>
                <dx:ASPxGridView ID="ASPxGridView1" runat="server" AutoGenerateColumns="False"
                     ClientInstanceName="DanhSachHoTroCuaToi" 
                    EnableTheming="True" 
                    OnCustomCallback="ASPxGridView1_CustomCallback" 
                    OnDataBinding="ASPxGridView1_DataBinding"
                     OnPageIndexChanged="ASPxGridView1_PageIndexChanged"
                     Theme="Office2010Blue" 
                    OnCustomButtonInitialize="ASPxGridView1_CustomButtonInitialize">
                    <SettingsPager PageSize="5">
                    </SettingsPager>
                    <Columns>
                        <dx:GridViewDataTextColumn Caption="#" FieldName="ID" ShowInCustomizationForm="True" VisibleIndex="0">
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Số ĐT" FieldName="SODIENTHOAI" ShowInCustomizationForm="True" VisibleIndex="3">
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Mã y/c" FieldName="MA_YEUCAU" ShowInCustomizationForm="True" VisibleIndex="4">
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Nội dung" FieldName="NOIDUNG_YEUCAU" ShowInCustomizationForm="True" VisibleIndex="5">
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Ngày tạo" FieldName="NGAYTAO" ShowInCustomizationForm="True" VisibleIndex="6">
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Người tạo" FieldName="NGUOITAO" ShowInCustomizationForm="True" VisibleIndex="7">
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Tên người tạo" FieldName="TENDAYDU" ShowInCustomizationForm="True" VisibleIndex="8">
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Đơn vị tạo" FieldName="TENDONVI" ShowInCustomizationForm="True" VisibleIndex="1">
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Mã đơn vị" FieldName="MADONVI" ShowInCustomizationForm="True" VisibleIndex="2">
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Hệ thống" FieldName="TENHETHONG" ShowInCustomizationForm="True" VisibleIndex="8">
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Mã hệ thống" FieldName="MA_HETHONG" ShowInCustomizationForm="True" VisibleIndex="8">
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Lĩnh vực" FieldName="LINHVUC" ShowInCustomizationForm="True" VisibleIndex="8">
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Mã lĩnh vực" FieldName="MA_LINHVUC" ShowInCustomizationForm="True" VisibleIndex="8">
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Mức độ" FieldName="TENMUCDO" ShowInCustomizationForm="True" VisibleIndex="8">
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Luồng" FieldName="TEN_LUONG" ShowInCustomizationForm="True" VisibleIndex="8">
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Đơn vị phối hợp" FieldName="DONVIPHOIHOP" ShowInCustomizationForm="True" VisibleIndex="8">
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewCommandColumn ShowInCustomizationForm="True" VisibleIndex="9">
                            <CustomButtons>
                                <dx:GridViewCommandColumnCustomButton ID="Xem" Text="Xem">
                                </dx:GridViewCommandColumnCustomButton>
                            </CustomButtons>
                        </dx:GridViewCommandColumn>
                    </Columns>
                </dx:ASPxGridView>
                <dx:ASPxHiddenField ID="hiddenIDHoTroChiTietXuLy" runat="server" ClientInstanceName="hiddenIDHoTroChiTietXuLy">
                </dx:ASPxHiddenField>
            </dx:PanelContent>
        </PanelCollection>
    </dx:ASPxRoundPanel>
          </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="Content" runat="server">
</asp:Content>
