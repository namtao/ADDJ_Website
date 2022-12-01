<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PopupDetail.aspx.cs" Inherits="Website.BaoCao.PopupDetail" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Tổng hợp chi tiết</title>
    <%= string.Format("<link href=\"{0}?ver={1}\" type=\"text/css\" rel=\"stylesheet\" />", ResolveClientUrl("~/css/style.report.css"), Website.AppCode.Common.Ver)  %>
    <%= string.Format("<script src=\"{0}?ver={1}\" type=\"text/javascript\"></script>", ResolveClientUrl("~/Js/jquery-1.7.2.min.js"), Website.AppCode.Common.Ver)  %>
    <style type="text/css">
        .lim { margin-top: 9px; }
        .inner { width: 100%; overflow-y: no-display; overflow-x: scroll; }
    </style>
    <script type="text/javascript">
        $(document).ready(function () {
            <asp:Literal runat="server" Id="liJs"></asp:Literal>
        });
    </script>
</head>
<body>
    <form runat="server">
        <% if (LoginAdmin.AdminLogin().Username.ToLower() == "Administrator".ToLower())
            { %>
        <div style="margin: 10px; text-align: right">
            <a href="<%= Request.RawUrl %>">Thử lại</a>
        </div>
        <% } %>
        <div class="wrapper">
            <div class="header">
                <asp:Literal runat="server" ID="liTitle" Text="Danh sách khiếu nại"></asp:Literal>
            </div>
            <div class="time">
                <asp:Literal runat="server" ID="liTime"></asp:Literal>
            </div>

            <asp:Panel runat="server" ID="PnlMain" Visible="false">
                <div class="mes">
                    <div class="lim fl">
                        <asp:Literal runat="server" ID="liMess"></asp:Literal>
                    </div>
                    <asp:Button runat="server" CssClass="btn fr" ID="btnExportExel" Text="Xuất Excel" />
                    <div class="vide"></div>
                </div>
                <div class="main">
                    <div class="inner">
                        <asp:GridView runat="server" ID="GrvDanhSach" AutoGenerateColumns="false" Width="100%"></asp:GridView>

                        <%-- Gridview cho Báo Cáo Tổng Hợp PAKN --%>
                        <asp:GridView runat="server" ID="GrvViewTongHopPAKN" AutoGenerateColumns="False" Width="100%">
                            <Columns>
                                <asp:BoundField HeaderText="STT" DataField="STT" />
                                <asp:BoundField HeaderText="Mã khiếu nại" DataField="Id" />
                                <asp:BoundField HeaderText="Số thuê bao" DataField="SoThueBao" />
                                <asp:BoundField HeaderText="Lĩnh vực con" DataField="LinhVucCon" />
                                <asp:BoundField HeaderText="Nội dung PA" DataField="NoiDungPA" />
                                <asp:BoundField HeaderText="Nội dung xử lý" DataField="NoiDungXuLy" />
                                <asp:BoundField HeaderText="Đối tác xử lý" DataField="DoiTacXuLyId" />
                                <asp:BoundField HeaderText="Người xử lý" DataField="NguoiXuLy" />
                                <asp:BoundField HeaderText="Nội dung GQ" DataField="NoiDungXuLyDongKN" />
                                <asp:BoundField HeaderText="Chi tiết lỗi" DataField="ChiTietLoi" />
                                <asp:TemplateField HeaderText="Ngày tiếp nhận">
                                    <ItemTemplate>
                                        <%# ((DateTime)Eval("NgayTiepNhan")).ToString("dd/MM/yyy HH:MM") %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Ngày đóng KN">
                                    <ItemTemplate>
                                        <%# ((DateTime)Eval("NgayDongKN")).ToString("dd/MM/yyy HH:MM") %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
                <div class="mes2">
                    <asp:Button runat="server" ID="btnExportExcel2" Text="Xuất Excel" CssClass="btn fr" />
                    <div class="vide"></div>
                </div>
            </asp:Panel>
        </div>
    </form>
</body>
</html>


