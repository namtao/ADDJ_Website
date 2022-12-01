<%@ Page Title="" Language="C#" MasterPageFile="~/AdminNotAJAX.Master" AutoEventWireup="true" CodeBehind="TraCuuHoSoADDJ.aspx.cs" Inherits="Website.ADDJ_TH.views.TraCuuHoSoADDJ" %>


<%@ Register Assembly="DevExpress.Web.v21.2, Version=21.2.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeaderCss" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderJs" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder_Text" runat="server">
    <div style="padding: 20px;">
        <div>
            <div style="width: 100%; margin: 0 auto;">
                <dx:ASPxGridView ID="grvCustomer" runat="server" AutoGenerateColumns="False" KeyFieldName="id"
                    Width="100%" Theme="Aqua">
                    <SettingsPager Visible="False">
                    </SettingsPager>
                    <Border BorderWidth="1px" BorderStyle="Solid"></Border>
                    <Columns>
                        <dx:GridViewDataTextColumn FieldName="id" VisibleIndex="1" Caption="ID">
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn FieldName="hopso" VisibleIndex="2" Caption="Hộp số">
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn FieldName="hoso_so" VisibleIndex="3" Caption="Hồ sơ số">
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn FieldName="trichyeunoidung" VisibleIndex="4" Caption="Trích yếu nội dung">
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn FieldName="soto" VisibleIndex="5" Caption="Số tờ">
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn FieldName="thoigian" VisibleIndex="6" Caption="Thời gian">
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn FieldName="nam" VisibleIndex="7" Caption="Năm">
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn FieldName="mlhs" VisibleIndex="8" Caption="MLHS">
                        </dx:GridViewDataTextColumn>
                    </Columns>
                </dx:ASPxGridView>
                <div style="width: 100%; height: 30px; border: solid 1px #999; border-top: none;">
                    <div style="float: right;">
                        <dx:ASPxPager ID="ASPxPager1" ItemCount="3" ItemsPerPage="1" runat="server" NumericButtonCount="5"
                            CurrentPageNumberFormat="{0}" OnPageIndexChanged="ASPxPager1_PageIndexChanged" Theme="Glass">
                            <LastPageButton Visible="True">
                            </LastPageButton>
                            <AllButton Text="Tất cả">
                            </AllButton>
                            <FirstPageButton Visible="True">
                            </FirstPageButton>
                            <Summary Position="Inside" Text="Trang {0} của {1} " />
                            <CurrentPageNumberStyle BackColor="#FFFF99" ForeColor="Red">
                                <Paddings PaddingLeft="5" PaddingRight="5" PaddingTop="2" PaddingBottom="2" />
                                <Border BorderColor="#CC0000" BorderStyle="Solid" BorderWidth="1px" />
                            </CurrentPageNumberStyle>
                        </dx:ASPxPager>
                    </div>
                </div>
            </div>
        </div>
    </div>

</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="Content" runat="server">
</asp:Content>
