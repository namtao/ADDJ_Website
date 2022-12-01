using System;
using System.Web.UI.WebControls;
using AIVietNam.Core;
using AIVietNam.GQKN.Entity;
using Website.AppCode;
using AIVietNam.Admin;

public partial class admin_thongBao_manager : PageBase
{
	protected void Page_Load(object sender, EventArgs e)
	{
		LoginAdmin.IsLoginAdmin();

		if (!UserRightImpl.CheckRightAdminnistrator_NoCache().UserRead)
		{
			Response.Redirect(Config.PathNotRight, false);
			return;
		}

		if (!IsPostBack)
		{
			grvView.PageSize = Config.RecordPerPage;
			BindGrid();
		}
	}
	private void BindGrid()
	{
		try
		{
			var thongBaoObj = ServiceFactory.GetInstanceThongBao().GetList();
            for (int i = 0; i < thongBaoObj.Count; i++)
		    {
		        ThongBaoInfo thongBaoInfo = new ThongBaoInfo();
		        thongBaoInfo = thongBaoObj[i];
                thongBaoInfo.NoiDung = System.Web.HttpUtility.HtmlDecode(thongBaoInfo.NoiDung.ToString());
		    }
			if (thongBaoObj != null && thongBaoObj.Count > 0)
			{
				ltThongBao.Text = "<font color='red'>Có " + thongBaoObj.Count + " ThongBao được tìm thấy.</font>";
				grvView.DataSource = thongBaoObj;
				grvView.DataBind();
			}
			else
			{
				ltThongBao.Text = "<font color='red'>Không có Log được tìm thấy.</font>";
grvView.DataSource = null;
grvView.DataBind();
			}

		}
		catch(Exception ex)
		{
			Utility.LogEvent(ex);
			Response.Redirect(Config.PathError, false);
			return;
		}
	}


	private void RowDataBound(GridViewRowEventArgs e)
	{
		if (e.Row.DataItemIndex != -1)
		{
			//if (e.Row.RowIndex % 2 != 0)
			//{
			//	e.Row.Attributes.Add("onmouseover", "this.style.backgroundColor='#f5eeb8'");
			//	e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor='#DEEAF3'");
			//}			else			{
			//	e.Row.Attributes.Add("onmouseover", "this.style.backgroundColor='#f5eeb8'");
			//	e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor='#ffffff'");
			//}
			e.Row.Cells[0].Text = (e.Row.DataItemIndex + 1).ToString();
			e.Row.Cells[1].Text = "<a href='thongBao_add.aspx?ID=" + grvView.DataKeys[e.Row.RowIndex].Value + "'>" + e.Row.Cells[1].Text + "</a>";
		}
	}

	private void PageIndexChanging(GridViewPageEventArgs e)
	{
		grvView.PageIndex = e.NewPageIndex;
			BindGrid();
	}

	protected void grvView_RowDataBound(object sender, GridViewRowEventArgs e)
	{
		try
		{
			RowDataBound(e);
		}
		catch(Exception ex)
		{
			Utility.LogEvent(ex);
			Response.Redirect(Config.PathError, false);
			return;
		}
	}

	protected void grvView_PageIndexChanging(object sender, GridViewPageEventArgs e)
	{
		PageIndexChanging(e);
	}
 

    protected void linkbtnThemMoi_Click(object sender, EventArgs e)
    {
        Response.Redirect("thongBao_add.aspx", false);
    }

    protected void linkbtnDelete_Click(object sender, EventArgs e)
    {
        if (!UserRightImpl.CheckRightAdminnistrator_NoCache().UserDelete)
        {
            Response.Redirect(Config.PathNotRight, false);
            return;
        }

        try
        {
            int i = 0;
            var obj = ServiceFactory.GetInstanceThongBao();
            foreach (GridViewRow row in grvView.Rows)
            {
                var status = (CheckBox)row.FindControl("cbSelectAll");
                if (status.Checked)
                {
                    int ID = int.Parse(grvView.DataKeys[i].Value.ToString());
                    obj.Delete(ID);
                }
                i++;
            }
        }
        catch (Exception ex)
        {
            Utility.LogEvent(ex);
            Response.Redirect(Config.PathError, false);
            return;
        }
        BindGrid();
    }
}

