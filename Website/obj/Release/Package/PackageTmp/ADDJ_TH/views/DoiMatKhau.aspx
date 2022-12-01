<%@ Page Title="" Language="C#" MasterPageFile="~/AdminNotAJAX.Master" AutoEventWireup="true" CodeBehind="DoiMatKhau.aspx.cs" Inherits="Website.ADDJ_TH.views.DoiMatKhau" %>

<%@ Register Assembly="DevExpress.Web.v21.2, Version=21.2.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeaderCss" runat="server">
    <style type="text/css">
        .formLayout {
            max-width: 500px;
            margin: auto;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderJs" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder_Text" runat="server">
    <div>
        <!-- begin panel nav boot -->
        <div class="nav_btn_bootstrap">
            <ul>
                <li><a href="javascript:history.back()" class="btn btn-primary"><span class="glyphicon glyphicon-circle-arrow-left" aria-hidden="true"></span>&nbsp;Quay về</a></li>
            </ul>
        </div>
        <div style="width: 100%; margin: 0 auto;">
            <dx:ASPxRoundPanel ID="ASPxRoundPanel1" runat="server" ShowHeader="false" ShowCollapseButton="true"
                Theme="Moderno" View="GroupBox" Width="100%" ContentPaddings-PaddingTop="0px">
                <ContentPaddings PaddingTop="0px" />
                <PanelCollection>
                    <dx:PanelContent runat="server">
                        <dx:ASPxFormLayout runat="server" CssClass="formLayout" RequiredMarkDisplayMode="Auto">
                            <Items>
                                <dx:LayoutGroup Caption="Thay đổi mật khẩu" ColumnCount="3" GroupBoxDecoration="HeadingLine"
                                    UseDefaultPaddings="false" SettingsItemCaptions-Location="Top">
                                    <GroupBoxStyle>
                                        <Caption Font-Bold="true" Font-Size="12"></Caption>
                                    </GroupBoxStyle>
                                    <Items>
                                        <dx:LayoutItem Caption="Mật khẩu" HelpText="Nhập mật khẩu mới">
                                            <LayoutItemNestedControlCollection>
                                                <dx:LayoutItemNestedControlContainer>
                                                    <dx:ASPxTextBox ID="npsw" runat="server" Password="True" ClientInstanceName="npsw">
                                                        <ClientSideEvents Validation="function(s, e) {e.isValid = (s.GetText().length > 0)}" />
                                                        <ValidationSettings ErrorDisplayMode="ImageWithTooltip" ErrorText="Độ dài mật khẩu ít nhất là 1 ký tự">
                                                        </ValidationSettings>
                                                    </dx:ASPxTextBox>
                                                </dx:LayoutItemNestedControlContainer>
                                            </LayoutItemNestedControlCollection>
                                        </dx:LayoutItem>
                                        <dx:LayoutItem Caption="Nhập lại mật khẩu" HelpText="Nhập lại mật khẩu mới">
                                            <LayoutItemNestedControlCollection>
                                                <dx:LayoutItemNestedControlContainer>
                                                    <dx:ASPxTextBox ID="cnpsw" runat="server" Password="True" ClientInstanceName="cnpsw">
                                                        <ClientSideEvents Validation="function(s, e) {e.isValid = (s.GetText() == npsw.GetText());}" />
                                                        <ValidationSettings ErrorDisplayMode="ImageWithTooltip" ErrorText="Mật khẩu không khớp">
                                                        </ValidationSettings>
                                                    </dx:ASPxTextBox>
                                                </dx:LayoutItemNestedControlContainer>
                                            </LayoutItemNestedControlCollection>
                                        </dx:LayoutItem>
                                        <dx:LayoutItem ShowCaption="false" RequiredMarkDisplayMode="Hidden" HorizontalAlign="Right" Width="100">
                                            <LayoutItemNestedControlCollection>
                                                <dx:LayoutItemNestedControlContainer>
                                                    <dx:ASPxButton ID="btnTimKiem" Width="100px" runat="server" Text="Cập nhật" Theme="Aqua" ClientInstanceName="btnTimKiem" OnClick="btnTimKiem_Click">
                                                        <Image IconID="businessobjects_bo_user_svg_16x16">
                                                        </Image>
                                                    </dx:ASPxButton>
                                                </dx:LayoutItemNestedControlContainer>
                                            </LayoutItemNestedControlCollection>
                                        </dx:LayoutItem>
                                    </Items>
                                    <SettingsItemHelpTexts Position="Bottom"></SettingsItemHelpTexts>
                                </dx:LayoutGroup>
                            </Items>
                        </dx:ASPxFormLayout>
                        <dx:ASPxLabel ID="ASPxLabel1" runat="server" Text=""></dx:ASPxLabel>
                    </dx:PanelContent>
                </PanelCollection>
            </dx:ASPxRoundPanel>
            <br />
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="Content" runat="server">
</asp:Content>
