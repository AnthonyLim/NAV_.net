using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NAV
{
    public partial class SessionTestPage : System.Web.UI.Page
    {
        string sourcePage = "portfoliodetails.asp";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session.Contents.Count != 0)
            {
                
                Response.Write(Session.Contents.Count.ToString());
                //Response.Write(Request.Url.ToString());
                foreach (string key in Session.Keys)
                {
                    Response.Write(key + " - " + Session[key].ToString() + "<br />");
                }
            }
            else
            {

                //Response.Redirect("https://localhost:40/report/" + sourcePage);
                //Response.Redirect("https://172.29.20.10:443/report/" + sourcePage);
                Response.Redirect("https://" + Request.ServerVariables["SERVER_NAME"] + ":" + Request.ServerVariables["SERVER_PORT"] + "/report/" + sourcePage);
            }
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            //Response.Redirect("https://localhost:40/report/" + sourcePage);
            //Response.Redirect("https://172.29.20.10:443/report/" + sourcePage);
            Response.Redirect("https://" + Request.ServerVariables["SERVER_NAME"] + ":" + Request.ServerVariables["SERVER_PORT"] + "/report/" + sourcePage);
        }
    }
}