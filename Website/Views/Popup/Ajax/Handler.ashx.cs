using AIVietNam.Core;
using AIVietNam.GQKN.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using Website.AppCode;
using Website.Components.Info;

namespace Website.Views.Popup.Ajax
{
    /// <summary>
    /// Summary description for Handler
    /// </summary>
    public class Handler : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            try
            {
                string type = context.Request.QueryString["Type"];
                if (!string.IsNullOrEmpty(type))
                {
                    if (type.EqualsIgnorCase("ThemMoiKhieuNai"))
                    {
                        string command = context.Request["Command"];
                        if (command.EqualsIgnorCase("TimKiem"))
                        {
                            #region Tìm kiếm của autocomplete
                            string keyword = context.Request["keyword"];

                            ResponseInfo objs = new ResponseInfo() { Code = 1, Message = "Thành công", Data = GetData(keyword) };
                            context.Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(objs));
                            #endregion
                        }
                        else if (command.EqualsIgnorCase("LayDanhSachLoaiKN"))// Lấy danh sách loại khiếu nại
                        {
                            #region Lấy danh sách Loại khiếu nại, cấp 1
                            StringBuilder sb = new StringBuilder();
                            List<LoaiKhieuNaiInfo> lst = ServiceFactory.GetInstanceLoaiKhieuNai().GetListDynamic("Id, Name", "Cap = 1 AND (Status = 1 OR Status = 2)", "Sort");
                            sb.Append("<option value='0' title='-- Loại khiếu nại --'>-- Loại khiếu nại --</option>");
                            if (lst != null && lst.Count > 0)
                            {
                                foreach (LoaiKhieuNaiInfo info in lst)
                                {
                                    sb.AppendFormat("<option value='{0}' title='{1}'>{1}</option>", info.Id, info.Name);
                                }
                            }

                            ResponseInfo objs = new ResponseInfo() { Code = 1, Message = "Thành công", Data = sb.ToString() };
                            context.Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(objs));
                            #endregion
                        }
                        else
                        {
                            ResponseInfo objs = new ResponseInfo() { Code = -1, Message = "Vui lòng cung cấp tham số lệnh" };
                            context.Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(objs));
                        }
                    }
                    else
                    {
                        ResponseInfo objs = new ResponseInfo() { Code = -1, Message = "Tham số không hợp lệ" };
                        context.Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(objs));
                    }
                }
                else
                {
                    ResponseInfo objs = new ResponseInfo() { Code = -1, Message = "Chưa cung cấp tham số 'type'" };
                    context.Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(objs));
                }
            }
            catch (Exception ex)
            {
                ResponseInfo objs = new ResponseInfo() { Code = -1, Message = "Có lỗi xảy ra: " + ex.Message, Data = ex.StackTrace };
                context.Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(objs));
            }
            context.Response.End();
        }

        public object GetData(string keyword)
        {
            DataTable tbl = SqlHelper.ExecuteDataset(Config.ConnectionString, "LoaiKhieuNai_TimKiemTheoTen", keyword).Tables[0];
            return tbl.AsEnumerable().Select(v => new
            {
                rowNumber = v.Field<object>("RowNumber"),
                id = v.Field<object>("Id"),
                name = v.Field<object>("Name"),
                type = v.Field<object>("Type"), // 1: Loại Khiếu nại, 2 Lĩnh vực chung, 3 Lĩnh vực con
                parentId = v.Field<object>("ParentId"),
                parentName = v.Field<object>("ParentName"),
                loaiKhieuNaiId = v.Field<object>("LoaiKhieuNaiId"),
                tenLoaiKhieuNai = v.Field<object>("TenLoaiKhieuNai")
            });
        }

        public bool IsReusable
        {
            get { return false; }
        }
    }
}