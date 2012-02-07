using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NAV.Scheme.UserControl
{
    public partial class ucContributions : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            String strClientID = Session[clsSystem_Session.strSession.clientID.ToString()].ToString();
            String strSchemeID = Session[clsSystem_Session.strSession.tempschemeid.ToString()].ToString();

            populate(strClientID, strSchemeID);
        }

        private void populate(String strClientID, String strSchemeID)
        { 
            this.gvContributions.DataSource = clsScheme.clsContribution.getContributions(new clsScheme(strClientID, strSchemeID));
            this.gvContributions.DataBind();
        }

        protected string CheckNull(object objGrid)
        {
            if (object.ReferenceEquals(objGrid, DBNull.Value) || objGrid.ToString().Equals(""))
            {
                return "no data";
            }
            else
            {
                if (objGrid.ToString().Equals("01/01/1800") || objGrid.ToString().Equals(""))
                {
                    return "no data";
                }
                else
                {
                    return objGrid.ToString();
                }
            }
        }
    }
}