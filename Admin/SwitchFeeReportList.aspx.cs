using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NAV.Admin
{
    public partial class SwitchFeeReportList : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                ((NAV)this.Page.Master).FindControl("btnBack_Classic").Visible = false;

                ViewState["IFA"] = Request.QueryString["IFA"];
                ViewState["SDate"] = Request.QueryString["SDate"];
                ViewState["EDate"] = Request.QueryString["EDate"];
                string strSwitches = Request.QueryString["SID"].ToString();
                int[] intSwitches = Array.ConvertAll(strSwitches.Split(','), s => int.Parse(s));
                List<clsSwitch> oSwitchList = clsSwitch.getSwitchList(intSwitches);
                populateSwitchList(oSwitchList);
            }
        }
        protected void btnPrevious_Click(object sender, EventArgs e)
        {
            Response.Redirect(string.Format("SwitchFeeReport.aspx?IFA={0}&SDate={1}&EDate={2}", ViewState["IFA"].ToString(), ViewState["SDate"].ToString(), ViewState["EDate"].ToString()));
        }
        private void populateSwitchList(List<clsSwitch> oSwitchList)
        {
            gvSwitchListReport.DataSource = oSwitchList;
            gvSwitchListReport.DataBind();
        }
        //private List<clsSwitch> getSwitchList()
        //{

        //}
    }
}
