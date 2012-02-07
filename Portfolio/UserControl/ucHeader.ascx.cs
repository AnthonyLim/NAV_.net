using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NAV.Portfolio.UserControl
{
    public partial class ucHeader : System.Web.UI.UserControl
    {

        #region "properties"

        private String strClientID;
        public String propClientID { get { return strClientID; } set { strClientID = value; } }

        private String strPortfolioID;
        public String propPortfolioID { get { return strPortfolioID; } set { strPortfolioID = value; } }

        private String strUserID;
        public String propUserID { get { return strUserID; } set { strUserID = value; } }

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if(!Page.IsPostBack)
            {
                clsPortfolio Portfolio = new clsPortfolio(this.propClientID, this.propPortfolioID, this.propUserID);
                clsClient Client = new clsClient(this.propClientID);

                populate(Portfolio, Client);
            }
        }

        private void populate(clsPortfolio _clsPortfolio, clsClient Client) 
        {
            this.lblValue_ClientName.Text = Client.propForename + " " + Client.propSurname + " (" + Client.propCode + ")";
            this.lblValue_Company.Text = _clsPortfolio.propCompany;
            this.lblValue_PortfolioType.Text = _clsPortfolio.propPortfolioType;
            this.lblValue_Currency.Text = _clsPortfolio.propPortfolioCurrency;
            this.lblValue_AccountNumber.Text = _clsPortfolio.propAccountNumber;
            this.lblValue_PlanStatus.Text = _clsPortfolio.propPlanStatus;
            this.lblValue_StartDate.Text = _clsPortfolio.propPortfolioStartDate.ToString("dd/MM/yyyy");
            this.lblValue_PolicyCategory.Text = _clsPortfolio.propLiquidity;
            this.lblValue_Profile.Text = _clsPortfolio.propRiskProfile;
            this.lblValue_SpecialistInformation.Text = _clsPortfolio.propRetentionTerm;
            this.lblValue_Discretionary.Text = _clsPortfolio.propMFPercent == 0 ? "no" : "yes";
        }
    }
}