using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NAV.Scheme.UserControl
{
    public partial class ucProposalDisplay : System.Web.UI.UserControl
    {

        #region "properties"

        private enumUserLevel _userLevel;
        public enumUserLevel propUserLevel
        {
            get { return _userLevel; }
            set { _userLevel = value; }
        }

        private int intSwitchID = 0;
        public int propSwitchID {get { return intSwitchID; } set { intSwitchID = value; }}

        private Boolean isDetailsAsGivenParameter = false;
        public Boolean propIsDetailsAsGivenParameter { get { return isDetailsAsGivenParameter; } set { isDetailsAsGivenParameter = value; } }

        private List<clsSwitchDetailsList> SwitchDetailsList = null;
        public List<clsSwitchDetailsList> propSwitchDetailsList {get { return SwitchDetailsList; } set { SwitchDetailsList = value; }}

        private List<clsSwitchDetailsList> SwitchDetailsListContribution = null;
        public List<clsSwitchDetailsList> propSwitchDetailsListContribution { get { return SwitchDetailsListContribution; } set { SwitchDetailsListContribution = value; } }

        private String strStatus = null;
        public String propStatus {get { return strStatus; }set { strStatus = value; }}

        private String strDescription = null;
        public String propDescription {get { return strDescription; } set { strDescription = value; }}

        #endregion

        #region "enums"

        public enum enumUserLevel
        {
            IFA,
            Client
        }

        public enum enumSwitchDetailType
        { 
            //IFASchemeSwitchDetails = clsSwitchScheme.clsSwitchSchemeDetails
            IFASchemeSwitchDetails
        }

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack){
                if (propUserLevel == null){ throw new Exception("Property propUserLevel cannot be null.");}

                switch (propIsDetailsAsGivenParameter)
                {
                    case true:
                        if (propStatus == null) { throw new Exception("Property Status cannot be null."); }

                        populateProposedSwitchText(_userLevel);
                        populateProposedPortfolio(propSwitchDetailsList, propSwitchDetailsListContribution, propStatus, propDescription);
                        break;
                    case false:
                        populate(propUserLevel, propSwitchID);
                        break;
                    default:
                        break;
                }                
            }            
        }

        private void populate(enumUserLevel _userLevel, int intSwitchID)
        {            
            populateProposedSwitchText(_userLevel);
            populateProposedPortfolio(_userLevel, intSwitchID);            
        }

        private void populateProposedSwitchText(enumUserLevel _userLevel)
        {
            switch (_userLevel)
            {
                case enumUserLevel.IFA:
                    this.lblTitle_ProposedSwitch.Text = "Client Proposed Switch";
                    this.lblTitle_ProposedSwitchContribution.Text = "Client Proposed Contribution Allocation Switch";
                    break;
                case enumUserLevel.Client:
                    this.lblTitle_ProposedSwitch.Text = "IFA Proposed Switch";
                    this.lblTitle_ProposedSwitchContribution.Text = "IFA Proposed Contribution Allocation Switch";
                    break;
                default:
                    break;
            }
        }

        private void populateProposedPortfolio(enumUserLevel _userLevel, int intSwitchID)
        {            
            if (intSwitchID==0){return;}

            switch (_userLevel)
            {
                case enumUserLevel.IFA:
                    clsSwitchScheme_Client SwitchSchemeClient = new clsSwitchScheme_Client(intSwitchID);
                    List<clsSwitchScheme_Client.clsSwitchSchemeDetails_Client> ClientSwitchSchemeDetails = SwitchSchemeClient.propSwitchDetailsPortfolio;
                    List<clsSwitchScheme_Client.clsSwitchSchemeDetails_Client> ClientSwitchSchemeDetailsContribution = SwitchSchemeClient.propSwitchDetailsContribution;

                    List<clsSwitchDetailsList> listSwitchDetailsClient = clsSwitchDetailsList.convertSwitchDetails(ClientSwitchSchemeDetails);
                    List<clsSwitchDetailsList> listSwitchDetailsClientContribution = clsSwitchDetailsList.convertSwitchDetails(ClientSwitchSchemeDetailsContribution);

                    this.lblStatus.Text = SwitchSchemeClient.propStatusString;

                    this.gvSwitchDetails.DataSource = listSwitchDetailsClient;
                    this.gvSwitchDetails.DataBind();

                    this.gvSwitchDetailsContribution.DataSource = listSwitchDetailsClientContribution;
                    this.gvSwitchDetailsContribution.DataBind();

                    Label gvSwitchFooterLblTotalAllocationClient = (Label)this.gvSwitchDetails.FooterRow.Cells[4].FindControl("gvSwitchFooterLblTotalAllocation");
                    gvSwitchFooterLblTotalAllocationClient.Text = ClientSwitchSchemeDetails[ClientSwitchSchemeDetails.Count - 1].propTotalAllocation.ToString("n2");

                    Label gvSwitchFooterLblTotalAllocationContributionClient = (Label)this.gvSwitchDetailsContribution.FooterRow.Cells[4].FindControl("gvSwitchFooterLblTotalAllocation");
                    gvSwitchFooterLblTotalAllocationContributionClient.Text = ClientSwitchSchemeDetailsContribution[ClientSwitchSchemeDetailsContribution.Count - 1].propTotalAllocation.ToString("n2");

                    this.txtProposedSwitchDesc.Text = SwitchSchemeClient.propDescription;
                    break;
                case enumUserLevel.Client:

                    clsSwitchScheme SwitchScheme = new clsSwitchScheme(intSwitchID);
                    List<clsSwitchScheme.clsSwitchSchemeDetails> IFASwitchSchemeDetails = SwitchScheme.propSwitchDetails;
                    List<clsSwitchScheme.clsSwitchSchemeDetails> IFASwitchSchemeDetailsContribution = SwitchScheme.propSwitchDetailsContribution;

                    if (IFASwitchSchemeDetails == null) {break;}

                    List<clsSwitchDetailsList> listSwitchDetails = clsSwitchDetailsList.convertSwitchDetails(IFASwitchSchemeDetails);
                    List<clsSwitchDetailsList> listSwitchDetailsContribution = clsSwitchDetailsList.convertSwitchDetails(IFASwitchSchemeDetailsContribution);

                    this.lblStatus.Text = SwitchScheme.propStatusString;

                    this.gvSwitchDetails.DataSource = listSwitchDetails;
                    this.gvSwitchDetails.DataBind();

                    this.gvSwitchDetailsContribution.DataSource = listSwitchDetailsContribution;
                    this.gvSwitchDetailsContribution.DataBind();

                    Label gvSwitchFooterLblTotalAllocation = (Label)this.gvSwitchDetails.FooterRow.Cells[4].FindControl("gvSwitchFooterLblTotalAllocation");
                    gvSwitchFooterLblTotalAllocation.Text = IFASwitchSchemeDetails[IFASwitchSchemeDetails.Count - 1].propTotalAllocation.ToString("n2");

                    Label gvSwitchFooterLblTotalAllocationContribution = (Label)this.gvSwitchDetailsContribution.FooterRow.Cells[4].FindControl("gvSwitchFooterLblTotalAllocation");
                    gvSwitchFooterLblTotalAllocationContribution.Text = IFASwitchSchemeDetailsContribution[IFASwitchSchemeDetailsContribution.Count - 1].propTotalAllocation.ToString("n2");

                    this.txtProposedSwitchDesc.Text = SwitchScheme.propDescription;

                    break;
                default:
                    break;
            }           
        }

        private void populateProposedPortfolio(List<clsSwitchDetailsList> SwitchDetailsList, List<clsSwitchDetailsList> SwitchDetailsListContribution, String strStatus, String strDescription)
        {
            this.lblStatus.Text = strStatus;
            this.txtProposedSwitchDesc.Text = strDescription;

            if (SwitchDetailsList != null)
            {
                this.gvSwitchDetails.DataSource = SwitchDetailsList;
                this.gvSwitchDetails.DataBind();

                Label gvSwitchFooterLblTotalAllocationClient = (Label)this.gvSwitchDetails.FooterRow.Cells[4].FindControl("gvSwitchFooterLblTotalAllocation");
                gvSwitchFooterLblTotalAllocationClient.Text = SwitchDetailsList[SwitchDetailsList.Count - 1].propTotalAllocation.ToString("n2");

            }
            else
            {
                this.lblTitle_ProposedSwitch.Visible = false;
                this.lblTitle_ProposedSwitchContribution.Visible = false;
                //this.table1_container.Visible = false;
                //this.table1_container2.Visible = false;
            }

            if (SwitchDetailsListContribution != null)
            {
                this.gvSwitchDetailsContribution.DataSource = SwitchDetailsListContribution;
                this.gvSwitchDetailsContribution.DataBind();

                Label gvSwitchFooterLblTotalAllocationContributionClient = (Label)this.gvSwitchDetailsContribution.FooterRow.Cells[4].FindControl("gvSwitchFooterLblTotalAllocation");
                gvSwitchFooterLblTotalAllocationContributionClient.Text = SwitchDetailsListContribution[SwitchDetailsListContribution.Count - 1].propTotalAllocation.ToString("n2");
            }
        }

        public class clsSwitchDetailsList
        {

            #region "properties"

            private string strFund;
            public string propFundName { get { return strFund; } set { strFund = value; } }

            private float fAllocation;
            public float propAllocation { get { return fAllocation; } set { fAllocation = value; } }

            private decimal dUnits;
            public decimal propUnits { get { return dUnits; } set { dUnits = value; } }

            private float fValue;
            public float propValue { get { return fValue; } set { fValue = value; } }

            private float fTotalValue;
            public float propTotalValue { get { return fTotalValue; } set { fTotalValue = value; } }

            private float fTotalAllocation;
            public float propTotalAllocation { get { return fTotalAllocation; } set { fTotalAllocation = value; } }

            private float fPrice;
            public float propPrice {get { return fPrice; } set { fPrice = value; }}

            private string strFundCurrency;
            public string propFundCurrency {get { return strFundCurrency; } set { strFundCurrency = value; }}

            #endregion

            public clsSwitchDetailsList() { }

            public static List<clsSwitchDetailsList> convertSwitchDetails(List<clsSwitchScheme.clsSwitchSchemeDetails> IFASwitchSchemeDetails)
            {
                List<clsSwitchDetailsList> newSwitchDetailsList = new List<clsSwitchDetailsList>();

                float fTotalAllocation = 0;

                foreach (clsSwitchScheme.clsSwitchSchemeDetails SwitchSchemeDetails in IFASwitchSchemeDetails)
                {
                    clsSwitchDetailsList SwitchDetail = new clsSwitchDetailsList();

                    fTotalAllocation = fTotalAllocation + SwitchSchemeDetails.propAllocation;

                    SwitchDetail.propAllocation = SwitchSchemeDetails.propAllocation;
                    SwitchDetail.propFundName = SwitchSchemeDetails.propFund.propFundName;
                    SwitchDetail.propTotalAllocation = fTotalAllocation;
                    SwitchDetail.propTotalValue = SwitchSchemeDetails.propTotalValue;
                    SwitchDetail.propUnits = SwitchSchemeDetails.propUnits;
                    SwitchDetail.propValue = SwitchSchemeDetails.propValue;
                    SwitchDetail.propPrice = SwitchSchemeDetails.propFund.propPrice;
                    SwitchDetail.propFundCurrency = SwitchSchemeDetails.propFund.propCurrency;

                    newSwitchDetailsList.Add(SwitchDetail);
                }

                return newSwitchDetailsList;
            }

            public static List<clsSwitchDetailsList> convertSwitchDetails(List<clsSwitchScheme_Client.clsSwitchSchemeDetails_Client> ClientSwitchSchemeDetails)
            {
                List<clsSwitchDetailsList> newSwitchDetailsList = new List<clsSwitchDetailsList>();

                float fTotalAllocation = 0;

                foreach (clsSwitchScheme_Client.clsSwitchSchemeDetails_Client SwitchSchemeDetails in ClientSwitchSchemeDetails)
                {
                    clsSwitchDetailsList SwitchDetail = new clsSwitchDetailsList();

                    fTotalAllocation = fTotalAllocation + SwitchSchemeDetails.propAllocation;

                    SwitchDetail.propAllocation = SwitchSchemeDetails.propAllocation;
                    SwitchDetail.propFundName = SwitchSchemeDetails.propFund.propFundName;
                    SwitchDetail.propTotalAllocation = fTotalAllocation;
                    SwitchDetail.propTotalValue = SwitchSchemeDetails.propTotalValue;
                    SwitchDetail.propUnits = SwitchSchemeDetails.propUnits;
                    SwitchDetail.propValue = SwitchSchemeDetails.propValue;
                    SwitchDetail.propPrice = SwitchSchemeDetails.propFund.propPrice;
                    SwitchDetail.propFundCurrency = SwitchSchemeDetails.propFund.propCurrency;

                    newSwitchDetailsList.Add(SwitchDetail);
                }

                return newSwitchDetailsList;
            }
        }

    }


}