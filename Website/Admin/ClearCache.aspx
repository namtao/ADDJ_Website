<%@ Page Title="" Language="C#" MasterPageFile="~/Master_Default.master" AutoEventWireup="true"
    CodeBehind="ClearCache.aspx.cs" Inherits="Website.admin.ClearCache" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder_Text" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <!-- begin panel nav boot -->
            <div class="nav_btn_bootstrap">
                <ul>
                    <ul>
                        <li><a href="javascript:history.back()" class="btn btn-primary"><span class="glyphicon glyphicon-circle-arrow-left" aria-hidden="true"></span> Quay về</a></li>
                    </ul>
            </div>
            <!-- end panel nav boot -->
            <div class="p8">
                <!-- begin panel boot -->
                <div class="panel panel-default">
                    <div class="panel-heading"><span style="font-size: 12pt"> &nbsp;</span></div>
                    <div class="panel-body" style="border: none">
                        <!-- begin body boot -->
                        <table border="0">
                            <tr>
                                <td style="height: 30px;"></td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:LinkButton ID="linkbtnClearAllCache" CssClass="btn btn-primary" runat="server" OnClick="linkbtnClearAllCache_Click">
                                         <span class="glyphicon glyphicon-refresh" aria-hidden="true"></span> Refresh toàn bộ cache
                                    </asp:LinkButton>
                                </td>
                            </tr>
                            <tr>
                                <td style="height: 30px;"></td>
                            </tr>
                            <tr style="display:none">
                                <td>
                                    <asp:Button ID="btDeltaImportKhieuNai" runat="server" CssClass="btn_style_button" Text="Delta-import Khiếu nại" OnClick="btDeltaImportKhieuNai_Click" />
                                </td>
                                <td>
                                    <asp:Button ID="btFullImportKhieuNai" runat="server" CssClass="btn_style_button" Text="Full-import Khiếu nại" OnClick="btFullImportKhieuNai_Click" />
                                </td>
                            </tr>
                            <tr>
                                <td style="height: 30px;"></td>
                            </tr>
                            <tr style="display:none">
                                <td>
                                    <asp:Button ID="btDeltaImportActivity" runat="server" CssClass="btn_style_button" Text="Delta-import Activity" OnClick="btDeltaImportActivity_Click" />
                                </td>
                                <td>
                                    <asp:Button ID="btFullImportActivity" runat="server" CssClass="btn_style_button" Text="Full-import Activity" OnClick="btFullImportActivity_Click" />
                                </td>
                            </tr>
                            <tr>
                                <td style="height: 30px;"></td>
                            </tr>
                            <tr style="display:none">
                                <td>
                                    <asp:Button ID="btDeltaImportSoTien" runat="server" CssClass="btn_style_button" Text="Delta-import số tiền" OnClick="btDeltaImportSoTien_Click" />
                                </td>
                                <td>
                                    <asp:Button ID="btFullImportSoTien" runat="server" CssClass="btn_style_button" Text="Full-import số tiền" OnClick="btFullImportSoTien_Click" />
                                </td>
                            </tr>
                            <tr>
                                <td style="height: 30px;"></td>
                            </tr>
                            <tr style="display:none">
                                <td>
                                    <asp:Button ID="btDeltaImportKQXL" runat="server" CssClass="btn_style_button" Text="Delta-import kết quả xử lý" OnClick="btDeltaImportKQXL_Click" />
                                </td>
                                <td>
                                    <asp:Button ID="btFullImportKQXL" runat="server" CssClass="btn_style_button" Text="Full-import kết quả xử lý" OnClick="btFullImportKQXL_Click" />
                                </td>
                            </tr>
                            <tr>
                                <td style="height: 30px;"></td>
                            </tr>
                        </table>
                        <!-- end body boot -->
                    </div>
                </div>
                <!-- end panel boot -->
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdateProgress ID="UpdateProgress1" runat="server" DisplayAfter="0"
        DynamicLayout="true">
        <ProgressTemplate>
            <div style="position: absolute; visibility: visible; border: none; z-index: 10000; top: 0px; left: 0px; width: 100%; height: 100%; background: #999; opacity: 0.4;">
                <div style="position: relative; min-height: 300px">
                    <asp:Image ID="Image1" runat="server" ImageUrl="~/images/loader.gif" CssClass="loading_updatepanel" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>
</asp:Content>
