using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NAV.Admin
{
    public partial class EmailSignedConfirmation_List : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request["__EVENTTARGET"] == "ctl00$btnBack_Classic") { Response.Redirect("https://" + Request.ServerVariables["HTTP_HOST"] + Session["SourcePage"]); }

            if (!Page.IsPostBack)
            {
                gvList.DataSource = clsCompany.getCompanyInsurance();
                gvList.DataBind();
            }
            else
            {
                if (Request["cbox"] == null || Request["status"] == null) { Response.End(); }
                bool isChecked = Request["status"] == "checked" ? true : false;
                int CompanyID = int.Parse(Request["cbox"]);
                clsCompany comp = new clsCompany(CompanyID);
                comp.setConfirmationStatus = isChecked;
            }
        }
    }
}
