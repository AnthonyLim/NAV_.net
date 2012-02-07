using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NAV.Scheme.UserControl
{
    public partial class ucHeader : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if( !Page.IsPostBack){

                String strClientID = Session[clsSystem_Session.strSession.clientID.ToString()].ToString();
                String strSchemeID = Session[clsSystem_Session.strSession.tempschemeid.ToString()].ToString();

                populate(strClientID, strSchemeID);
            }

        }

        private void populate(String strClientID, String strSchemeID)
        {
            clsScheme Scheme = new clsScheme(strClientID, strSchemeID);

            this.lblValue_AccountNumber.Text = Scheme.propAccountNumber;
            this.lblValue_ClientName.Text = Scheme.propClient.propForename + " " + Scheme.propClient.propSurname;
            this.lblValue_Company.Text = Scheme.propCompany.propCompany;
            this.lblValue_Currency.Text = Scheme.propSchemeCurrency;
            this.lblValue_Discretionary.Text = Scheme.propMFPercent == 0 ? "no" : "yes";
            this.lblValue_MaturityDate.Text = Scheme.propMaturityDate.ToString("dd/MM/yyyy") == "01/01/1800" ? "" : Scheme.propMaturityDate.ToString("dd/MM/yyyy");
            this.lblValue_PlanStatus.Text = Scheme.propPlanStatus.ToString();
            this.lblValue_PolicyCategory.Text = Scheme.propLiquidity.ToString();
            this.lblValue_PortfolioType.Text = Scheme.propPortfolioType.ToString();
            this.lblValue_Profile.Text = Scheme.propRiskProfile.ToString();
            this.lblValue_SchemeCurrency.Text = Scheme.propSchemeCurrency.ToString();
            this.lblValue_SpecialistInformation.Text = Scheme.propRetentionTerm.ToString();
            this.lblValue_StartDate.Text = Scheme.propStartDate.ToString("dd/MM/yyyy");
            this.lblValue_TotalContribution.Text = Scheme.propContributionTotal.ToString("n0");
            this.lblValue_GainLossValue.Text = "(" + Math.Abs(float.Parse(Scheme.propGainLoss.ToString())).ToString("n0") + ")";
            this.lblValue_GainLossPercent.Text = "(" + Math.Abs(float.Parse(Scheme.propGainLossPercent.ToString())).ToString("n2") + " %)"; ;

            this.lblValue_GainLossValue.ForeColor = Scheme.propGainLoss < 0 ?  System.Drawing.Color.Red : System.Drawing.Color.Green;
            this.lblValue_GainLossPercent.ForeColor = Scheme.propGainLossPercent < 0 ? System.Drawing.Color.Red : System.Drawing.Color.Green;

        }
    }
}