<%@ Page Title="" Language="C#" MasterPageFile="~/Views/BaoCao/BaoCao.Master" AutoEventWireup="true" CodeBehind="baocaotonghoppakntheonguoidungptdv.aspx.cs" Inherits="Website.Views.BaoCao.Popup.baocaotonghoppakntheonguoidungptdv" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<div runat="server" id="baocao" class="reportFont">
    <div id="reportContainer">
         <table border="0" width="100%">
            <tr valign="top">
                <td style="font-weight:bold;font-size:10pt;text-align:center" colspan="3">
                    &nbsp;
                </td>
                <td>                    
                </td>
                <td style="font-weight:bold;font-size:10pt; text-align:center" colspan="3">
                    <span>CỘNG HÒA XÃ HỘI CHỦ NGHĨA VIỆT NAM</span>
                    <br />
                    <span style="text-decoration: underline;">
                        Độc lập - Tự do - Hạnh phúc
                    </span>                    
                </td>
            </tr>
            <tr>
                <td colspan="6"></td>
            </tr>            
        </table>     

        <div style="text-align:center">
            <h1>
                BÁO CÁO TỔNG HỢP SỐ LIỆU PAKN THEO NGƯỜI DÙNG
            </h1>                            
            
            <table border="0" width="100%" >
                <tr>
                    <td colspan="5" style="text-align:center">
                        <asp:Label ID="lblFromDateToDate" runat="server"></asp:Label>
                    </td>                                        
                </tr>                               
            </table>                       
        </div>    

        <br />

        <table class="tbl_style" border="1" style="border-collapse: collapse;">
            <tr >
                <th rowspan="2">
                    STT
                </th>
                <th rowspan="2">
                    Tên truy cập                    
                </th>
                 <th rowspan="2">
                    Tên đầy đủ
                </th>
                <th rowspan="2" >
                    SL tồn kỳ trước
                    <br />[1]
                </th>
                <th colspan="2">
                    Số lượng tiếp nhận
                </th>
                <th colspan="6" >
                    Số lượng đã xử lý
                </th>                    
                <th rowspan="2" >
                    Số lượng tồn đọng
                    <br />[4]
                </th>
                <th rowspan="2">
                    Số lượng tồn đọng quá hạn
                    <br />[4.1]
                </th>               
            </tr>
            <tr>                
                <th>SL tiếp nhận<br />[2]</th>
                <th>SL tạo mới<br />[2.1]</th>
                <th>SL đã xử lý<br />[3] = [3.1] + [3.2] + [3.3] + [3.4]</th>
                <th>SL chuyển ngang hàng<br />[3.1]</th>
                <th>SL chuyển xử lý<br />[3.2]</th>
                <th>SL chuyển phản hồi<br />[3.3]</th>
                <th>SL đã đóng<br />[3.4]</th> 
                <th>Số lượng đã xử lý quá hạn<br />[3.5]</th>               
            </tr>
            <%=sNoiDungBaoCao %>
        </table>

        <table border="0" width="100%">
            <tr>
                <td style="border: 0px;">
                    &nbsp;
                </td>
            </tr>
            <tr valign="top">
                <td style="border: 0px; text-align: center; font-weight: bold" colspan="3">                    
                </td>  
                <td style="text-align: center; font-weight: bold; border: 0px" colspan="3">
                    Người báo cáo
                </td>              
            </tr>
            <tr>
                <td colspan="5" style="height:70px">
                    &nbsp;
                </td>
            </tr>
            <tr>                
                <td align="center" colspan="3">
                    <asp:label ID="lblFullName" runat="server"></asp:label>
                </td>
            </tr>
        </table>

    <script src="/JS/jquery.PrintArea.js_4.js" type="text/javascript"></script>

    <script src="/Views/BaoCao/printCore.js" type="text/javascript"></script>

    </div>
    <div>
        <b><i>Giải thích</i></b>
        <br />[1] : Số lượng khiếu nại tồn tính đến trước ngày thực hiện báo cáo
        <br />[2] : Số lượng khiếu nại tiếp nhận (khiếu nại từ nơi khác chuyển đến + tạo mới) ngoại trừ các khiếu nại tồn đọng từ kỳ trước
        <br />[2.1] : Số lượng khiếu nại do người dùng tạo ra
        <br />[3] = [3.1] + [3.2] + [3.3] + [3.4]: Tổng số khiếu nại đã được xử lý (chuyển ngang hàng/chuyển xử lý/chuyển phản hồi/đóng khiếu nại)
        <br />[3.1] : Số lượng khiếu nại đã được chuyển ngang hàng
        <br />[3.2] : Số lượng khiếu nại đã được chuyển xử lý
        <br />[3.3] : Số lượng khiếu nại đã được chuyển chuyển phản hồi
        <br />[3.4] : Số lượng khiếu nại người dùng đã đóng
        <br />[3.5] : Số lượng khiếu nại đã được xử lý nhưng quá hạn phòng ban quy định
        <br />[4] : Số lượng khiếu nại tồn tính tới thời điểm cuối cùng thực hiện báo cáo
        <br />[4.1] : Số lượng khiếu nại tồn quá hạn tính tới thời điểm cuối cùng thực hiện báo cáo
    </div>
</div>

</asp:Content>
