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
    public partial class phongBanVNPT_Cap3_add : PageBase
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
            var lstPhongBan = ServiceFactory.GetInstancePhongBan().GetListDynamic("Id,ParentId,Name,Cap", "(Cap = 1 OR Cap = 2) AND DoiTacId=" + UserInfo.DoiTacId, "Cap");

            foreach (var itemC1 in lstPhongBan.Where(t => t.Cap == 1))
            {
                foreach (var itemC2 in lstPhongBan.Where(t => t.ParentId == itemC1.Id))
                {
                    ddlPhongBancha.Items.Add(new OptionDropDownList.OptionGroupItem(itemC2.Id.ToString(), itemC2.Name, itemC1.Name));
                }
                //ListItem item2 = new ListItem(itemC1.Name, itemC1.Id.ToString());
                //item2.Attributes["OptionGroup"] = "Mammals";
                //ddlPhongBanDinhTuyen.Items.Add(item2);
            }
            //ddlPhongBanDinhTuyen.DataSource = lstPhongBan;
            //ddlPhongBanDinhTuyen.DataValueField = "Id";
            //ddlPhongBanDinhTuyen.DataTextField = "Name";
            //ddlPhongBanDinhTuyen.DataBind();

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
                    ddlPhongBancha.SelectedValue = item.ParentId.ToString();
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
                            item.ParentId = Convert.ToInt32(ddlPhongBancha.SelectedValue);

                            item.LoaiPhongBanId = ConstFixCode.PHONG_BAN_TIEP_NHAN;

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
                                //item.Sort = Convert.ToInt32(txtSort.Text);
                            }
                            else
                            {
                                var itemSort = ServiceFactory.GetInstancePhongBan().GetInfo(Convert.ToInt32(ddlPhongBancha.SelectedValue));
                                if (itemSort != null)
                                {
                                    item.Sort = itemSort.Sort;
                                    item.Cap = itemSort.Cap + 1;
                                }
                                else
                                    throw new Exception("Phòng ban cha không tồn tại.");
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
                        item.ParentId = Convert.ToInt32(ddlPhongBancha.SelectedValue);

                        item.LoaiPhongBanId = ConstFixCode.PHONG_BAN_TIEP_NHAN;

                        item.KhuVucId = UserInfo.KhuVucId;
                        item.DoiTacId = UserInfo.DoiTacId;

                        item.Name = txtName.Text.Trim();
                        item.Description = txtDescription.Text.Trim();


                        item.IsDinhTuyenKN = false;
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
                            //item.Sort = Convert.ToInt32(txtSort.Text);
                        }
                        else
                        {
                            var itemSort = ServiceFactory.GetInstancePhongBan().GetInfo(Convert.ToInt32(ddlPhongBancha.SelectedValue));
                            if (itemSort != null)
                            {
                                item.Sort = itemSort.Sort;
                                item.Cap = itemSort.Cap + 1;
                            }
                            else
                                throw new Exception("Phòng ban cha không tồn tại.");
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