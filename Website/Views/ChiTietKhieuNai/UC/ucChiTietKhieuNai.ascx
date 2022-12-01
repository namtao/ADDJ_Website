<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucChiTietKhieuNai.ascx.cs" Inherits="Website.Views.KhieuNai.UC.ucChiTietKhieuNai" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc" TagName="CacBuocXuLy" Src="~/Views/ChiTietKhieuNai/UC/ucCacBuocXuLy.ascx" %>
<%@ Register TagPrefix="uc" TagName="TimKiemGiaiPhap" Src="~/Views/ChiTietKhieuNai/UC/ucTimKiemGiaiPhap.ascx" %>
<%@ Register TagPrefix="uc" TagName="LuuVetThayDoi" Src="~/Views/ChiTietKhieuNai/UC/ucLuuVetThayDoi.ascx" %>
<%@ Register TagPrefix="uc" TagName="KieuNaiCuocDichVu" Src="~/Views/ChiTietKhieuNai/UC/ucKieuNaiCuocDichVu.ascx" %>
<%@ Register TagPrefix="uc" TagName="FileDinhKem" Src="~/Views/ChiTietKhieuNai/UC/ucFileDinhKem.ascx" %>
<%@ Register TagPrefix="uc" TagName="KetQuaGiaiQuyetKN" Src="~/Views/ChiTietKhieuNai/UC/ucKetQuaGiaiQuyetKN.ascx" %>
<%@ Register TagPrefix="uc" TagName="QuaTrinhKhieuNai" Src="~/Views/ChiTietKhieuNai/UC/ucQuaTrinhKhieuNai.ascx" %>

<asp:UpdatePanel ID="UPCacBuocXuLy" runat="server">
    <ContentTemplate>
        <div class="p8">
            <cc2:TabContainer ID="TabContainer1" runat="server" CssClass="Tab tabchitietkhieunai" AutoPostBack="false"
                OnActiveTabChanged="TabContainer1_ActiveTabChanged" OnClientActiveTabChanged="LoadAllowBlock">
                <cc2:TabPanel runat="server" HeaderText="TabPanel1" ID="TabPanel1">
                    <HeaderTemplate>
                        Các bước xử lý
                    </HeaderTemplate>
                    <ContentTemplate>
                        <div class="p8">
                            <uc:CacBuocXuLy ID="UcCacBuocXuLy" runat="server" />
                        </div>
                    </ContentTemplate>
                </cc2:TabPanel>
                <cc2:TabPanel runat="server" HeaderText="TabPanel1" ID="TabPanel2">
                    <HeaderTemplate>
                        File đính kèm
                    </HeaderTemplate>
                    <ContentTemplate>
                        <div class="p8">
                            <uc:FileDinhKem ID="UcFileDinhKem" runat="server" />
                        </div>
                    </ContentTemplate>
                </cc2:TabPanel>
                <cc2:TabPanel runat="server" HeaderText="TabPanel1" ID="TabPanel3">
                    <HeaderTemplate>
                        KN cước dịch vụ 
                    </HeaderTemplate>
                    <ContentTemplate>
                        <div class="p8">
                            <uc:KieuNaiCuocDichVu ID="UcKieuNaiCuocDichVu" runat="server" />
                        </div>
                    </ContentTemplate>
                </cc2:TabPanel>
                <cc2:TabPanel runat="server" HeaderText="TabPanel1" ID="TabPanel5">
                    <HeaderTemplate>
                        Kết quả giải quyết KN cước
                    </HeaderTemplate>
                    <ContentTemplate>
                        <div class="p8">
                            <uc:KetQuaGiaiQuyetKN ID="UcKetQuaGiaiQuyetKN" runat="server" />
                        </div>
                    </ContentTemplate>
                </cc2:TabPanel>
                <cc2:TabPanel runat="server" HeaderText="TabPanel1" ID="TabPanel6">
                    <HeaderTemplate>
                        Lưu vết thay đổi
                    </HeaderTemplate>
                    <ContentTemplate>
                        <div class="p8">
                            <uc:LuuVetThayDoi ID="UcLuuVetThayDoi" runat="server" />
                        </div>
                    </ContentTemplate>
                </cc2:TabPanel>
                <cc2:TabPanel runat="server" HeaderText="TabPanel1" ID="TabPanel4">
                    <HeaderTemplate>
                        Quá trình KN
                    </HeaderTemplate>
                    <ContentTemplate>
                        <div class="p8">
                            <uc:QuaTrinhKhieuNai ID="UcQuaTrinhKhieuNai" runat="server" />
                        </div>
                    </ContentTemplate>
                </cc2:TabPanel>
            </cc2:TabContainer>
            <script type="text/javascript">
                function LoadAllowBlock() {
                    __doPostBack("<%= TabContainer1.ClientID %>", '');
                }
            </script>

        </div>
    </ContentTemplate>
</asp:UpdatePanel>
<%--<cc2:TabPanel runat="server" HeaderText="TabPanel1" ID="TabPanel4">
            <HeaderTemplate>
                Tìm kiếm giải pháp
            </HeaderTemplate>
            <ContentTemplate>
                <div class="p8">
                    <uc:TimKiemGiaiPhap ID="UcTimKiemGiaiPhap" runat="server" />
                </div>
            </ContentTemplate>
</cc2:TabPanel>--%>