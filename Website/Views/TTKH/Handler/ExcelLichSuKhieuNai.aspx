<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ExcelLichSuKhieuNai.aspx.cs" Inherits="Website.Views.TTKH.Handler.ExcelLichSuKhieuNai" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div id="baocao" runat="server">
            <table width="2000px" cellpadding="0" cellspacing="0" border="0">
                <tr>
                    <td style="text-align: center">
                        <h1>Lịch sử khiếu nại</h1>
                    </td>
                </tr>
                <tr><td style="text-align: center">
                    Số thuê bao: <asp:Literal ID="ltSTB" runat="server"></asp:Literal>

                    </td></tr>
                <tr><td style="text-align: center">
                    Thời gian: <asp:Literal ID="ltTime" runat="server"></asp:Literal>

                    </td></tr>
                <tr>
                    <tr><td style="text-align: center; height:5px;">

                    </td></tr>
                    <td>
                        <asp:Repeater ID="grvViewLichSuKhieuNai" runat="server">
                            <HeaderTemplate>
                                <table class="tbl_style" cellpadding="0" cellspacing="0" style="border-collapse: collapse;">
                                    <tbody>
                                        <tr class="th" align="center" style="color: White; background-color: #2360A4; font-weight: bold;">
                                            <th align="center" scope="col">Mã PA/KN
                                            </th>
                                            <th align="center" scope="col">Trạng thái
                                            </th>
                                            <th align="center" scope="col">Loại khiếu nại
                                            </th>
                                            <th align="center" scope="col">Lĩnh vực chung
                                            </th>
                                            <th align="center" scope="col">Lĩnh vực con
                                            </th>
                                            <th align="center" scope="col">Người tiếp nhận
                                            </th>
                                            <th align="center" scope="col">Ngày tiếp nhận
                                            </th>
                                            <th align="center" scope="col">Số điện thoại liên hệ
                                            </th>
                                            <th align="center" scope="col">Người xử lý
                                            </th>
                                            <th align="center" scope="col">Ngày đóng KN
                                            </th>
                                            <th align="center" scope="col">Nội dung giải quyết
                                            </th>
                                            <th align="center" scope="col">Nội dung phản ánh
                                            </th>
                                        </tr>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <tr>
                                    <td align="left">
                                        <%# BindMaKN(DataBinder.Eval(Container.DataItem, "Id"))%>
                                    </td>
                                    <td align="center" style="width: 10%;">
                                        <%# BindTinhTrangXuLy(DataBinder.Eval(Container.DataItem, "TrangThai"))%>
                                    </td>
                                    <td align="left">
                                        <%# DataBinder.Eval(Container.DataItem, "LoaiKhieuNai")%>
                                    </td>
                                    <td align="left">
                                        <%# DataBinder.Eval(Container.DataItem, "LinhVucChung")%>
                                    </td>
                                    <td align="left">
                                        <%# DataBinder.Eval(Container.DataItem, "LinhVucCon")%>
                                    </td>
                                    <td align="left">
                                        <%# DataBinder.Eval(Container.DataItem, "NguoiTiepNhan")%>
                                    </td>
                                    <td align="center">
                                        <%# DataBinder.Eval(Container.DataItem, "NgayTiepNhan")%>
                                    </td>
                                    <td align="left">
                                        <%# DataBinder.Eval(Container.DataItem, "SDTLienHe")%>
                                    </td>
                                    <td align="left">
                                        <%# DataBinder.Eval(Container.DataItem, "NguoiXuLy")%>
                                    </td>
                                    <td align="center">
                                        <%# BindNgayDong(DataBinder.Eval(Container.DataItem, "TrangThai"), DataBinder.Eval(Container.DataItem, "NgayDongKN"))%>                                   
                                    </td>
                                    <td align="left">
                                        <%# DataBinder.Eval(Container.DataItem, "NoiDungXuLyDongKN")%>
                                    </td>
                                    <td align="left">
                                        <%# DataBinder.Eval(Container.DataItem, "NoiDungPA")%>
                                    </td>
                                </tr>
                            </ItemTemplate>
                            <AlternatingItemTemplate>
                                <tr class="rowA">
                                    <td align="left">
                                        <%# BindMaKN(DataBinder.Eval(Container.DataItem, "Id"))%>
                                    </td>
                                    <td align="center" style="width: 10%;">
                                        <%# BindTinhTrangXuLy(DataBinder.Eval(Container.DataItem, "TrangThai"))%>
                                    </td>
                                    <td align="left">
                                        <%# DataBinder.Eval(Container.DataItem, "LoaiKhieuNai")%>
                                    </td>
                                    <td align="left">
                                        <%# DataBinder.Eval(Container.DataItem, "LinhVucChung")%>
                                    </td>
                                    <td align="left">
                                        <%# DataBinder.Eval(Container.DataItem, "LinhVucCon")%>
                                    </td>
                                    <td align="left">
                                        <%# DataBinder.Eval(Container.DataItem, "NguoiTiepNhan")%>
                                    </td>
                                    <td align="center">
                                        <%# DataBinder.Eval(Container.DataItem, "NgayTiepNhan")%>
                                    </td>
                                     <td align="left">
                                        <%# DataBinder.Eval(Container.DataItem, "SDTLienHe")%>
                                    </td>
                                    <td align="left">
                                        <%# DataBinder.Eval(Container.DataItem, "NguoiXuLy")%>
                                    </td>
                                    <td align="center">
                                        <%# BindNgayDong(DataBinder.Eval(Container.DataItem, "TrangThai"), DataBinder.Eval(Container.DataItem, "NgayDongKN"))%>                                   
                                    </td>
                                    <td align="left">
                                        <%# DataBinder.Eval(Container.DataItem, "NoiDungXuLyDongKN")%>
                                    </td>
                                    <td align="left">
                                        <%# DataBinder.Eval(Container.DataItem, "NoiDungPA")%>
                                    </td>
                                </tr>
                            </AlternatingItemTemplate>
                            <FooterTemplate>
                                </table>
                            </FooterTemplate>
                        </asp:Repeater>
                    </td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>
