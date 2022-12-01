using AIVietNam.Admin;
using AIVietNam.Core;
using AIVietNam.GQKN.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Website.AppCode;

namespace Website.admin
{
    public partial class phongBanVNPT_Cap1_add : PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                lblMsg.Text = "";

                BindDropDownlist();
                if (Request.QueryString["ID"] != null && Request.QueryString["ID"] != string.Empty)
                {
                    EditData();
                }
            }
        }

        private void BindDropDownlist()
        {

            //foreach (byte i in Enum.GetValues(typeof(KhieuNai_HTTiepNhan_Type)))
            //{
            //    ddlHTTiepNhan.Items.Add(new ListItem(Enum.GetName(typeof(KhieuNai_HTTiepNhan_Type), i).Replace("_", " "), i.ToString()));
            //}
        }

        private void EditData()
        {
            try
            {
                var obj = ServiceFactory.GetInstancePhongBan();
                PhongBanInfo item = obj.GetInfo(int.Parse(Request.QueryString["ID"]));
                if (item == null)
                {
                    Utility.LogEvent("Function EditData phongBan_add get NullId " + Request.QueryString["ID"], System.Diagnostics.EventLogEntryType.Warning);
                    Response.Redirect(Config.PathError, false);
                    return;
                }
                else
                {

                    txtName.Text = item.Name.ToString();
                    txtDescription.Text = item.Description.ToString();
                    //chkIsChuyenTiepKN.Checked = item.IsChuyenTiepKN;
                    //ddlHTTiepNhan.SelectedValue = item.DefaultHTTN.ToString();
                }
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                Response.Redirect(Config.PathError, false);
                return;
            }
        }
 
        protected void linkbtnSubmit_Click(object sender, EventArgs e)
        {
            if (!UserRightImpl.CheckRightAdminnistrator_NoCache().UserEdit)
            {
                Response.Redirect(Config.PathNotRight, false);
                return;
            }

            try
            {
                var obj = ServiceFactory.GetInstancePhongBan();
                if (Request.QueryString["ID"] != null && Request.QueryString["ID"] != string.Empty)
                {
                    try
                    {
                        int idEdit = int.Parse(Request.QueryString["ID"]);
                        PhongBanInfo item = obj.GetInfo(idEdit);

                        if (item == null)
                        {
                            Utility.LogEvent("Function phongBan_add Edit Khong tim thay ban ghi nao voi Id" + Request.QueryString["ID"], System.Diagnostics.EventLogEntryType.Warning);
                            Response.Redirect(Config.PathError, false);
                            return;
                        }

                        try
                        {
                            item.ParentId = 0;


                            item.LoaiPhongBanId = ConstFixCode.PHONG_BAN_XU_LY;

                            item.KhuVucId = UserInfo.KhuVucId;
                            item.DoiTacId = UserInfo.DoiTacId;

                            item.Name = txtName.Text.Trim();
                            item.Description = txtDescription.Text.Trim();


                            item.IsDinhTuyenKN = true;
                            item.DefaultHTTN = (byte)KhieuNai_HTTiepNhan_Type.Điểm_giao_dịch;
                            item.IsChuyenTiepKN = false;


                            var lstSort = ServiceFactory.GetInstancePhongBan().GetListDynamic("Sort", "ParentId=0 and DoiTacId=" + UserInfo.DoiTacId, "");
                            if (lstSort != null && lstSort.Count > 0)
                            {
                                item.Sort = lstSort[0].Sort;
                            }
                            else
                                item.Sort = 0;
                            item.Cap = 1;


                            item.LUser = UserInfo.Username;
                        }
                        catch
                        {
                            lblMsg.Text = "Dữ liệu không hợp lệ";
                            return;
                        }

                        var strPhongBanDen = string.Empty;
                        if (item.ParentId == 0)
                        {
                            if (item.KhuVucId == 2)
                            {
                                strPhongBanDen = "[53,54,55,56,58,134]";
                            }
                            else if (item.KhuVucId == 3)
                            { strPhongBanDen = "[62,63,64,65,72,217]"; }
                            else if (item.KhuVucId == 5)
                            { strPhongBanDen = "[67,68,69,70,73,188]"; }
                            else
                            {
                                lblMsg.Text = "Không tìm thấy danh sách phòng ban Vinaphone gửi khiếu nại.";
                                return;
                            }
                        }
                        else
                        {
                            strPhongBanDen = string.Format("[{0}]", item.ParentId);
                        }

                        var checkExists = ServiceFactory.GetInstancePhongBan2PhongBan().GetListByPhongBanId(item.Id);
                        if (checkExists != null && checkExists.Any())
                        {
                            var itemUpdatePhongban2Phongban = checkExists[0];
                            itemUpdatePhongban2Phongban.PhongBanDen = strPhongBanDen;
                            ServiceFactory.GetInstancePhongBan2PhongBan().Update(itemUpdatePhongban2Phongban);
                            obj.Update(item);
                        }
                        else
                        {

                            ServiceFactory.GetInstancePhongBan2PhongBan().Add(new PhongBan2PhongBanInfo() { PhongBanId = item.Id, PhongBanDen = strPhongBanDen });
                            obj.Update(item);
                        }
                    }
                    catch (Exception ex)
                    {
                        Utility.LogEvent(ex);
                        Response.Redirect(Config.PathError, false);
                        return;
                    }
                }
                else
                {
                    var item = new PhongBanInfo();

                    try
                    {
                        item.ParentId = 0;


                        item.LoaiPhongBanId = ConstFixCode.PHONG_BAN_XU_LY;

                        item.KhuVucId = UserInfo.KhuVucId;
                        item.DoiTacId = UserInfo.DoiTacId;

                        item.Name = txtName.Text.Trim();
                        item.Description = txtDescription.Text.Trim();


                        item.IsDinhTuyenKN = true;
                        item.DefaultHTTN = (byte)KhieuNai_HTTiepNhan_Type.Điểm_giao_dịch;
                        item.IsChuyenTiepKN = false;

                        if (item.ParentId == 0)
                        {
                            var lstSort = ServiceFactory.GetInstancePhongBan().GetListDynamic("Sort", "ParentId=0 and DoiTacId=" + UserInfo.DoiTacId, "");
                            if (lstSort != null && lstSort.Count > 0)
                            {
                                item.Sort = lstSort[0].Sort;
                            }
                            else
                                item.Sort = 0;
                            item.Cap = 1;

                        }


                        item.LUser = UserInfo.Username;
                    }
                    catch
                    {
                        lblMsg.Text = "Dữ liệu không hợp lệ";
                        return;
                    }


                    var strPhongBanDen = string.Empty;
                    if (item.ParentId == 0)
                    {
                        if (item.KhuVucId == 2)
                        {
                            strPhongBanDen = "[53,54,55,56,58,134]";
                        }
                        else if (item.KhuVucId == 3)
                        { strPhongBanDen = "[62,63,64,65,72,217]"; }
                        else if (item.KhuVucId == 5)
                        { strPhongBanDen = "[67,68,69,70,73,188]"; }
                        else
                        {
                            lblMsg.Text = "Không tìm thấy danh sách phòng ban Vinaphone gửi khiếu nại.";
                            return;
                        }
                    }
                    else
                    {
                        strPhongBanDen = string.Format("[{0}]", item.ParentId);
                    }
                    item.Id = obj.Add(item);
                    if (item.Id > 0)
                        ServiceFactory.GetInstancePhongBan2PhongBan().Add(new PhongBan2PhongBanInfo() { PhongBanId = item.Id, PhongBanDen = strPhongBanDen });

                }

                string url = "phongBanVNPT_manager.aspx";
                if (Request.QueryString["ReturnUrl"] != null && !Request.QueryString["ReturnUrl"].ToString().Equals(""))
                    url = HttpUtility.UrlDecode(Request.QueryString["ReturnUrl"]);

                Response.Redirect(url, false);
            }
            catch (System.Data.SqlClient.SqlException se)
            {
                lblMsg.Text = "Tên phòng ban đã tồn tại.";
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                Response.Redirect(Config.PathError, false);
                return;
            }
        }
    }
}