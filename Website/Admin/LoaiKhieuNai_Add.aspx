<%@ Page Language="C#" MasterPageFile="~/Master_Default.master" AutoEventWireup="true" Inherits="Admin_LoaiKhieuNai_Add" CodeBehind="LoaiKhieuNai_Add.aspx.cs" %>

<asp:Content runat="server" ContentPlaceHolderID="HeaderCss">
    <link href="/Content/Chosen/chosen.css" rel="stylesheet" />
    <style type="text/css">
        .loaikn td {
            padding-bottom: 5px;
        }

        .loaikn .colName {
            width: 170px;
            text-align: right;
            padding-right: 10px;
        }

        .loaikn .colVal {
            width: 300px;
        }

            .loaikn .colVal .inputstyle input[type=text], .loaikn .colVal .inputstyle textarea {
                padding-left: 5px;
                padding-right: 5px;
            }

        .loaikn .aui-form example {
            padding-left: 10px;
        }

        span.aselected {
            font-weight: bold;
        }

            span.aselected a {
                padding: 0px 5px;
                color: #008B64;
            }

        span.valCompany {
            display: inline-block;
        }

            span.valCompany.none {
                display: none;
            }
    </style>
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="HeaderJs">
    <script src="/Content/Chosen/chosen.jquery.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $("#chonCongTy").on({
                click: function () {
                    alert("Chức năng đang được xây dựng!");
                }
            });
            $(".chosen").chosen();

            // Quay về
            $(".nav_btn span.back").on({
                click: function (e) {
                    document.location.href = "LoaiKhieuNai_Manager.aspx";
                    e.preventDefault();
                }
            });

        });
        function validForm() {
            // txtName : string
            var name = $("#<%= txtName.ClientID %>");
            if (name.val().trim() == "") {
                name.val("");
                name.focus();
                $.messager.alert('Thông báo', 'Không để trống trường này', 'error', function () {
                    name.focus();
                });
                return false;
            }
            return true;
        }
    </script>
</asp:Content>

<asp:Content ContentPlaceHolderID="Content" runat="Server">
    <!-- begin panel nav boot -->
    <div class="nav_btn_bootstrap">
        <ul>
            <li>
                <a href="javascript:history.back()" class="btn btn-primary"><span class="glyphicon glyphicon-circle-arrow-left" aria-hidden="true"></span> Quay về</a>
            </li>
            <li>
                <asp:LinkButton ID="linkbtnSubmit" class="btn btn-primary" OnClientClick="return validForm();" runat="server">
                    <span class="glyphicon glyphicon-plus-sign" aria-hidden="true"></span> Thêm mới
                </asp:LinkButton>
            </li>
        </ul>
    </div>
    <!-- end panel nav boot -->
    <div class="p8">
        <!-- begin panel boot -->
        <div class="panel panel-default">
            <div class="panel-heading"><span style="font-size: 12pt">Lọc tìm kiếm</span></div>
            <div class="panel-body" style="border: none">
                <!-- begin body boot -->
                <table border="0" cellpadding="0" cellspacing="0" width="100%" class="loaikn">
                    <tr>
                        <td class="colName"></td>
                        <td class="colVal">
                            <asp:Label ID="lblMsg" runat="server" Text="" ForeColor="Red"></asp:Label>
                        </td>
                        <td></td>
                    </tr>
                    <tr>
                        <td class="colName">Nhóm Cha
                        </td>
                        <td class="colVal">
                            <div class="selectstyle">
                                <div class="bg">
                                    <asp:DropDownList ID="ddlParrent" AutoPostBack="true" OnSelectedIndexChanged="ddlParrent_SelectedIndexChanged" runat="server">
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </td>
                        <td></td>
                    </tr>
                    <tr style="display: none">
                        <td class="colName">Nút gốc</td>
                        <td class="colVal">
                            <asp:Label ID="lblRoot" runat="server"></asp:Label>
                        </td>
                        <td>&nbsp;</td>
                    </tr>
                    <tr>
                        <td class="colName">Mã dịch vụ khiếu nại
                        </td>
                        <td class="colVal">
                            <div class="inputstyle">
                                <div class="bg">
                                    <asp:TextBox ID="txtMaDV" runat="server" Text="" MaxLength="50" Width="200px"></asp:TextBox>
                                </div>
                            </div>
                        </td>
                        <td></td>
                    </tr>
                    <tr>
                        <td class="colName">Tên Loại khiếu nại
                        </td>
                        <td class="colVal">
                            <div class="inputstyle">
                                <div class="bg">
                                    <asp:TextBox ID="txtName" runat="server" Text="" MaxLength="200" Width="100%"></asp:TextBox>
                                </div>
                            </div>
                        </td>
                        <td></td>
                    </tr>
                    <tr>
                        <td class="colName aligntop">Mô tả
                        </td>
                        <td class="colVal">
                            <div class="inputstyle">
                                <div class="bg">
                                    <asp:TextBox ID="txtDescription" runat="server" Text="" MaxLength="500" Width="100%" Height="70px" TextMode="MultiLine"></asp:TextBox>
                                </div>
                            </div>
                        </td>
                        <td></td>
                    </tr>
                    <tr>
                        <td class="colName">Thứ tự hiển thị
                        </td>
                        <td class="colVal">
                            <div class="inputstyle">
                                <div class="bg">
                                    <asp:TextBox ID="txtSort" runat="server" Text="0" Width="100px"></asp:TextBox>
                                </div>
                            </div>
                        </td>
                        <td></td>
                    </tr>
                    <tr>
                        <td class="colName">Trạng thái
                        </td>
                        <td class="colVal">
                            <asp:CheckBox ID="chkStatus" runat="server" Checked="true" Text=" Sử dụng" />
                        </td>
                        <td></td>
                    </tr>
                    <tr>
                        <td class="colName">Service code</td>
                        <td class="colVal">
                            <div class="inputstyle">
                                <div class="bg">
                                    <asp:TextBox ID="txtServiceCode" runat="server" Width="100px"></asp:TextBox>
                                    <span class="aui-form example">(Mã lấy dữ liệu doanh thu)</span>
                                </div>
                            </div>
                        </td>
                        <td></td>
                    </tr>
                    <tr>
                        <td class="colName">Thời gian cảnh báo
                        </td>
                        <td class="colVal">
                            <div class="inputstyle">
                                <div class="bg">
                                    <asp:TextBox ID="txtThoiGianCanhBao" runat="server" Text="" Width="100px"></asp:TextBox>
                                    <span class="aui-form example">(Ví dụ: 3d 14h 20m)</span>
                                </div>
                            </div>
                        </td>
                        <td>&nbsp;</td>
                    </tr>
                    <tr>
                        <td class="colName">Thời gian ước tính
                        </td>
                        <td class="colVal">
                            <div class="inputstyle">
                                <div class="bg">
                                    <asp:TextBox ID="txtThoiGianUocTinh" runat="server" Text="" Width="100px"></asp:TextBox>
                                    <span class="aui-form example">(Ví dụ: 8d 17h 25m)</span>
                                </div>
                            </div>
                        </td>
                        <td>&nbsp;</td>
                    </tr>
                    <tr>
                        <td class="colName">Thuộc đơn vị
                        </td>
                        <td class="colVal">
                            <div class="selectstyle">
                                <div class="bg">
                                    <asp:DropDownList CssClass="chosen" ID="ddlThuocDonVi" runat="server">
                                        <asp:ListItem Value="1">VNP</asp:ListItem>
                                        <asp:ListItem Value="10100">TTTC</asp:ListItem>
                                        <asp:ListItem Value="10101">PTDV</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </td>
                        <td></td>
                    </tr>
                    <tr>
                        <td class="colName">Đơn vị quản lý</td>
                        <td class="colVal">
                            <div class="selectstyle">
                                <div class="bg">
                                    <asp:DropDownList CssClass="chosen" ID="ddlDonViQuanLy" runat="server">
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </td>
                        <td>&nbsp;</td>
                    </tr>
                    <tr>
                        <td class="colName">Nhóm khiếu nại
                        </td>
                        <td class="colVal">
                            <div class="selectstyle">
                                <div class="bg">
                                    <asp:DropDownList CssClass="chosen" ID="ddlLoaiKhieuNaiNhom" runat="server">
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </td>
                        <td>&nbsp;</td>
                    </tr>
                    <tr>
                        <td class="colName" style="height: 20px">Đối tác cung cấp</td>
                        <td class="colVal" style="height: 20px">
                            <%--<span class="valCompany none"></span><span class="aselected">[<a id="chonCongTy" href="javascript:void(0)" onclick="return false;">Chọn</a>]</span>--%>
                            <div class="selectstyle">
                                <div class="bg">
                                    <asp:DropDownList CssClass="chosen" ID="ddlCompany" runat="server"></asp:DropDownList>
                                </div>
                            </div>
                        </td>
                        <td>&nbsp;</td>
                    </tr>
                    <tr>
                        <td class="colName">
                            <asp:HiddenField ID="hdfCompany" runat="server" />
                        </td>
                        <td style="text-align: left" class="colVal">
                            <div class="foot_nav_btn">
                                <a href="#1"><i class="save"></i><span>
                                    <asp:Button ID="btSubmit" CssClass="button_eole"
                                        runat="server" Text="Cập nhật" OnClick="btSubmit_Click" OnClientClick="return validForm();" /></span></a>
                                <a href="javascript:history.back()"><i class="cancel"></i><span>Hủy bỏ</span></a>
                            </div>
                        </td>
                        <td>
                            <%--<asp:Label ID="lblLoaiKhieuNaiNhomId" runat="server" Visible="false"></asp:Label>--%>
                            <asp:Label ID="lblParentLoaiKhieuNaiId" runat="server" Visible="false"></asp:Label>
                            <%--<asp:Label ID="lblLoaiKhieuNaiTenNhom" runat="server" Visible="false"></asp:Label>--%>
                        </td>
                    </tr>
                </table>
                <!-- end body boot -->
            </div>
        </div>
        <!-- end panel boot -->
    </div>
</asp:Content>
