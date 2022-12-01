<%@ Page Title="" Language="C#" MasterPageFile="~/adminNotAJAX.master" AutoEventWireup="true"
    CodeBehind="ThongTinKhachHang.aspx.cs" Inherits="Website.admin.ThongTinKhachHang" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>

<asp:Content ContentPlaceHolderID="ContentPlaceHolder_Text" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div id="rightarea">
                <div id="tracuuthongtinkhachhang">
                    <div class="head">
                        <h3>Tra cứu thông tin khách hàng</h3>
                    </div>
                    <div class="body">
                        <table class="nobor">
                            <tr>
                                <td width="66%" valign="top">
                                    <div class="boxOB">
                                        <div class="topbox">
                                            THÔNG TIN THUÊ BAO
                                        </div>
                                        <div class="midbox">
                                            <table class="nobor">
                                                <tr>
                                                    <td width="110px">Số thuê bao
                                                    </td>
                                                    <td width="15px">
                                                        <input id="txtDauSo" value="84" type="text" style="width: 15px" readonly="readonly" />
                                                    </td>
                                                    <td>
                                                        <input id="txtThueBao" type="text" style="width: 135px" />
                                                    </td>
                                                    <td width="55px" align="right">MSIN
                                                    </td>
                                                    <td>
                                                        <input id="txtMSIN" value="0" readonly="readonly" type="text" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>Loại thuê bao
                                                    </td>
                                                    <td colspan="2">
                                                        <input type="text" id="txtLoaiTB" readonly="readonly" class='mw' style="width: 158px" />
                                                    </td>
                                                    <td width="65px" align="right">
                                                        <label for="chkGoiDi" class="fr">
                                                            <input type="checkbox" id="chkGoiDi" disabled="disabled" />
                                                            Gọi đi</label>
                                                    </td>
                                                    <td>
                                                        <label for="chkGoiDen" class="fl ml10">
                                                            <input type="checkbox" id="chkGoiDen" disabled="disabled" />
                                                            Gọi đến</label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>Tỉnh
                                                    </td>
                                                    <td colspan="2">
                                                        <input type="text" id="txtTinh" readonly="readonly" class='mw' style="width: 158px" />
                                                    </td>
                                                    <td align="right">Ngày KH
                                                    </td>
                                                    <td>
                                                        <input type="text" id="txtNgayKH" readonly="readonly" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>Mã KH
                                                    </td>
                                                    <td colspan="2">
                                                        <input type="text" class='mw' id="txtMaKH" readonly="readonly" style="width: 158px" />
                                                    </td>
                                                    <td align="right">Mã CQ
                                                    </td>
                                                    <td>
                                                        <input type="text" id="txtMaCQ" readonly="readonly" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>Tên thuê bao
                                                    </td>
                                                    <td colspan="2">
                                                        <input type="text" class='mw' id="txtTB" readonly="readonly" style="width: 158px" />
                                                    </td>
                                                    <td align="right" class='nowarp'>Ngày sinh
                                                    </td>
                                                    <td>
                                                        <input type="text" id="txtNgaySinh" readonly="readonly" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>Số GT
                                                    </td>
                                                    <td colspan="2">
                                                        <input type="text" class='mw' id="txtSoGT" readonly="readonly" style="width: 158px" />
                                                    </td>
                                                    <td align="right">Ngày cấp
                                                    </td>
                                                    <td>
                                                        <input type="text" id="txtNoiCap" readonly="readonly" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>Số PIN/PUK
                                                    </td>
                                                    <td colspan="2">
                                                        <input type="text" style="width: 40%" id="txtPIN" readonly="readonly" />&nbsp;&nbsp;
                                                        <input type="text" style="width: 40%;" id="txtPUK" readonly="readonly" />
                                                    </td>
                                                    <td align="right">Số PIN2/PUK2
                                                    </td>
                                                    <td>
                                                        <input type="text" style="width: 40%" id="txtPIN2" readonly="readonly" />&nbsp;&nbsp;
                                                        <input type="text" style="width: 40%;" id="txtPUK2" readonly="readonly" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class='nowarp'>Đối tượng
                                                    </td>
                                                    <td colspan="4">
                                                        <input type="text" class='mw' id="txtDoiTuong" readonly="readonly" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class='nowarp'>Địa chỉ chứng từ
                                                    </td>
                                                    <td colspan="4">
                                                        <input type="text" class='mw' id="txtDiaChiChungTu" readonly="readonly" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>Địa chỉ thanh toán
                                                    </td>
                                                    <td colspan="4">
                                                        <input type="text" id="txtDiaChiThanhToan" readonly="readonly" class='mw' />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>ĐC thường trú
                                                    </td>
                                                    <td colspan="4">
                                                        <input type="text" id="txtDiaChiThuongTru" readonly="readonly" class='mw' />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>Tài khoản chính
                                                    </td>
                                                    <td colspan="2">
                                                        <input type="text" id="txtTKC" readonly="readonly" class='mw' style="width: 158px" />
                                                    </td>
                                                    <td align="right" class='nowarp'>Hạn sử dụng
                                                    </td>
                                                    <td>
                                                        <input type="text" id="txtHSD" readonly="readonly" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>Tài khoản KM
                                                    </td>
                                                    <td colspan="2">
                                                        <input type="text" id="txtKM" readonly="readonly" class='mw' style="width: 158px" />
                                                    </td>
                                                    <td align="right" class='nowarp'>Tài khoản KM1
                                                    </td>
                                                    <td>
                                                        <input type="text" id="txtKM1" readonly="readonly" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>Tài khoản KM2
                                                    </td>
                                                    <td colspan="2">
                                                        <input type="text" id="txtKM2" readonly="readonly" class='mw' style="width: 158px" />
                                                    </td>
                                                    <td align="right" class='nowarp'>Data
                                                    </td>
                                                    <td>
                                                        <input type="text" id="txtData" readonly="readonly" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="5" align="right">
                                                        <div class="col1">
                                                            &nbsp;
                                                        </div>
                                                        <div class="col8">
                                                            <input type="button" id="LamLai" value="Làm lại" class="btnie6 dpn" />
                                                            <input type="button" id="TiepThi" value="Up Sell - Cross Sell" disabled="disabled"
                                                                class="btnie6" />
                                                        </div>
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                    </div>
                                </td>
                                <td width="34%" valign="top">
                                    <div class="boxOB">
                                        <div class="box3" id="tblCacDichVu">
                                            <table class="myTbl superborder">
                                                <thead>
                                                    <tr>
                                                        <th width='30px'>&nbsp;
                                                        </th>
                                                        <th width='65px' title="Mã dịch vụ">Mã DV
                                                        </th>
                                                        <th title="Tên dịch vụ">Tên DV
                                                        </th>
                                                    </tr>
                                                </thead>
                                                <tbody id="lstService">
                                                </tbody>
                                            </table>
                                        </div>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <cc2:TabContainer ID="TabContainer1" runat="server" CssClass="Tab" ActiveTabIndex="1" AutoPostBack="True"
                                        OnActiveTabChanged="TabContainer1_ActiveTabChanged">
                                        <cc2:TabPanel runat="server" HeaderText="TabPanel1" ID="TabPanel1">
                                            <HeaderTemplate>
                                                Lịch sử<br />
                                                khiếu nại
                                            </HeaderTemplate>
                                            <ContentTemplate>
                                                fsfsêf
                                                    few
                                                    f
                                                    ewfè<br />
                                                ewfè<br />
                                                ewfè<br />
                                                ewfè<br />
                                                ewfè<br />
                                                ewfè<br />
                                            </ContentTemplate>
                                        </cc2:TabPanel>
                                        <cc2:TabPanel runat="server" HeaderText="TabPanel1" ID="TabPanel2">
                                            <HeaderTemplate>
                                                Lịch sử<br />
                                                thuê bao
                                            </HeaderTemplate>
                                            <ContentTemplate>
                                            </ContentTemplate>
                                        </cc2:TabPanel>
                                        <cc2:TabPanel runat="server" HeaderText="TabPanel1" ID="TabPanel3">
                                            <HeaderTemplate>
                                                Lịch sử<br />
                                                nạp thẻ
                                            </HeaderTemplate>
                                            <ContentTemplate>
                                            </ContentTemplate>
                                        </cc2:TabPanel>
                                        <cc2:TabPanel runat="server" HeaderText="TabPanel1" ID="TabPanel4">
                                            <HeaderTemplate>
                                                Lịch sử khiếu nại
                                            </HeaderTemplate>
                                            <ContentTemplate>
                                                fsfsêf
                                                    few
                                                    f
                                                    ewfè<br />
                                                ewfè<br />
                                                ewfè<br />
                                                ewfè<br />
                                                ewfè<br />
                                                ewfè<br />
                                            </ContentTemplate>
                                        </cc2:TabPanel>
                                        <cc2:TabPanel runat="server" HeaderText="TabPanel1" ID="TabPanel5">
                                            <HeaderTemplate>
                                                Lịch sử thuê bao
                                            </HeaderTemplate>
                                            <ContentTemplate>
                                            </ContentTemplate>
                                        </cc2:TabPanel>
                                        <cc2:TabPanel runat="server" HeaderText="TabPanel1" ID="TabPanel6">
                                            <HeaderTemplate>
                                                Lịch sử nạp thẻ
                                            </HeaderTemplate>
                                            <ContentTemplate>
                                            </ContentTemplate>
                                        </cc2:TabPanel>
                                        <cc2:TabPanel runat="server" HeaderText="TabPanel1" ID="TabPanel7">
                                            <HeaderTemplate>
                                                Lịch sử khiếu nại
                                            </HeaderTemplate>
                                            <ContentTemplate>
                                                fsfsêf
                                                    few
                                                    f
                                                    ewfè<br />
                                                ewfè<br />
                                                ewfè<br />
                                                ewfè<br />
                                                ewfè<br />
                                                ewfè<br />
                                            </ContentTemplate>
                                        </cc2:TabPanel>
                                        <cc2:TabPanel runat="server" HeaderText="TabPanel1" ID="TabPanel8">
                                            <HeaderTemplate>
                                                Lịch sử thuê bao
                                            </HeaderTemplate>
                                            <ContentTemplate>
                                            </ContentTemplate>
                                        </cc2:TabPanel>
                                        <cc2:TabPanel runat="server" HeaderText="TabPanel1" ID="TabPanel9">
                                            <HeaderTemplate>
                                                Lịch sử nạp thẻ
                                            </HeaderTemplate>
                                            <ContentTemplate>
                                            </ContentTemplate>
                                        </cc2:TabPanel>
                                        <cc2:TabPanel runat="server" HeaderText="TabPanel1" ID="TabPanel10">
                                            <HeaderTemplate>
                                                Lịch sử khiếu nại
                                            </HeaderTemplate>
                                            <ContentTemplate>
                                                fsfsêf
                                                    few
                                                    f
                                                    ewfè<br />
                                                ewfè<br />
                                                ewfè<br />
                                                ewfè<br />
                                                ewfè<br />
                                                ewfè<br />
                                            </ContentTemplate>
                                        </cc2:TabPanel>
                                        <cc2:TabPanel runat="server" HeaderText="TabPanel1" ID="TabPanel11">
                                            <HeaderTemplate>
                                                Lịch sử thuê bao
                                            </HeaderTemplate>
                                            <ContentTemplate>
                                            </ContentTemplate>
                                        </cc2:TabPanel>
                                        <cc2:TabPanel runat="server" HeaderText="TabPanel1" ID="TabPanel12">
                                            <HeaderTemplate>
                                                Lịch sử nạp thẻ
                                            </HeaderTemplate>
                                            <ContentTemplate>
                                            </ContentTemplate>
                                        </cc2:TabPanel>
                                    </cc2:TabContainer>
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
