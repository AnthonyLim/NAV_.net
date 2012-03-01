    using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;

namespace NAV
{
    public class clsSwitch
    {
        private SqlConnection con = new clsSystem_DBConnection(clsSystem_DBConnection.strConnectionString.NavIntegrationDB).propConnection;

        #region properties

        private int intSwitchID;
        public int propSwitchID { get { return intSwitchID; } set { intSwitchID = value; }}

        private string strModelGroupID;
        public string propModelGroupID { get { return strModelGroupID; } set { strModelGroupID = value; } }

        private string strModelPortfolioID;
        public string propModelPortfolioID { get { return strModelPortfolioID; } set { strModelPortfolioID = value; } }

        private string strPortfolioID;
        public string propPortfolioID { get { return strPortfolioID; } set { strPortfolioID = value; } }

	    private string strClientID;
        public string propClientID { get { return strClientID; } set { strClientID = value; }}

	    private Int16 sintStatus;
        public Int16 propStatus { get { return sintStatus; } set { sintStatus = value; }}

        private string strStatus = "Draft";
        public string propStatusString { get { return strStatus; } set { strStatus = value; } }

	    private DateTime dtDate_Created;
        public DateTime propDate_Created { get { return dtDate_Created; } set { dtDate_Created = value; }}
            
        private string strCreated_By;
        public string propCreated_By { get { return strCreated_By; } set { strCreated_By = value; }}

        private string strDescription = string.Empty;
        public string propDescription { get { return strDescription; } set { strDescription = value; } }

        private clsClient oClient;
        public clsClient propSwitchClient { get { return oClient; } set { oClient = value; } }

        private clsPortfolio oPortfolio;
        public clsPortfolio propPortfolio { get { return oPortfolio; } set { oPortfolio = value; } }

        private List<clsSwitchDetails> listSwitchDetails;
        public List<clsSwitchDetails> propSwitchDetails {get { return listSwitchDetails; } set { listSwitchDetails = value; }}

        #endregion

        public clsSwitch(clsPortfolio Portfolio, string strUserID)
        {
            getSwitchInfo(Portfolio, strUserID);
        }

        public clsSwitch(int intSwitchID)
        {
            getHeaderInfo(intSwitchID);
        }

        public clsSwitch() { }

        private void getHeaderInfo(int intSwitchID)
        {
            SqlCommand cmd = new SqlCommand();
            SqlDataReader dr;
            con.Open();
            cmd.Connection = con;
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "[SWITCH_HeaderGet]";
           
            cmd.Parameters.Add("@param_intSwitchID", System.Data.SqlDbType.Int).Value = intSwitchID;

            dr = cmd.ExecuteReader();

            if (dr.HasRows)
            {

                while (dr.Read())
                {

                    this.propClientID = dr["ClientID"].ToString();
                    this.propDate_Created = DateTime.Parse(dr["Date_Created"].ToString());
                    this.propPortfolioID = dr["PortfolioID"].ToString();
                    this.propStatus = short.Parse(dr["Status"].ToString());
                    this.propStatusString = getSwitchStringStatus(this.propStatus);
                    this.propSwitchID = int.Parse(dr["SwitchID"].ToString());
                    this.propCreated_By = dr["Created_By"].ToString();
                    this.propDescription = dr["Description"].ToString();
                    this.propPortfolio = new clsPortfolio(propClientID, propPortfolioID);
                }
            }

            dr.Close();
            con.Close();
            cmd.Dispose();
            con.Dispose();
        }

        private void getSwitchInfo(clsPortfolio Portfolio, string strUserID)
        { 
            SqlCommand cmd = new SqlCommand();
            SqlDataReader dr;
            con.Open();
            cmd.Connection = con;
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "[SWITCH_HeaderGet]";

            cmd.Parameters.Add("@param_strPortfolioID", System.Data.SqlDbType.NVarChar).Value = Portfolio.propPortfolioID;
            cmd.Parameters.Add("@param_strClientID", System.Data.SqlDbType.NVarChar).Value = Portfolio.propClientID;
                                   
            dr = cmd.ExecuteReader();

            if (dr.HasRows)
            {

                while (dr.Read())
                {

                    this.propClientID = dr["ClientID"].ToString();
                    this.propDate_Created = DateTime.Parse(dr["Date_Created"].ToString());
                    this.propPortfolioID = dr["PortfolioID"].ToString();
                    this.propStatus = short.Parse(dr["Status"].ToString());
                    this.propStatusString = getSwitchStringStatus(this.propStatus);
                    this.propSwitchID = int.Parse(dr["SwitchID"].ToString());
                    this.propSwitchDetails = getSwitchDetails(strUserID, Portfolio, this.propSwitchID);
                    this.propPortfolio = new clsPortfolio(propClientID, propPortfolioID);
                    if (propStatus == (int)enumSwitchStatus.Declined_Client)
                    {
                        foreach (clsSwitchDetails Details in this.propSwitchDetails)
                        {
                            clsSwitchDetails.removeSwitchDetails(Details.propSwitchDetailsID);
                        }
                        this.propSwitchDetails = clsSwitchDetails.replicatePortfolioDetails(Portfolio.propPortfolioDetails, propSwitchID);
                    }
                  
                    this.propCreated_By = dr["Created_By"].ToString();
                    this.propDescription = dr["Description"].ToString();
                }
            }
            else {
                this.propSwitchID = 0;
                this.propPortfolioID = Portfolio.propPortfolioID;
                this.propClientID = Portfolio.propClientID;
                this.propStatus = (short)enumSwitchStatus.Saved;
                this.propStatusString = getSwitchStringStatus(this.propStatus);
                this.propCreated_By = strUserID;
                this.propSwitchDetails = clsSwitchDetails.replicatePortfolioDetails(Portfolio);
                this.propPortfolio = new clsPortfolio(propClientID, propPortfolioID);
            }

            dr.Close();
            con.Close();
            cmd.Dispose();
            //con.Dispose();
        }

        private List<clsSwitchDetails> getSwitchDetails(string strUserID, clsPortfolio Portfolio, int intSwitchID)
        {
                        
            SqlConnection con1 = new clsSystem_DBConnection(clsSystem_DBConnection.strConnectionString.NavIntegrationDB).propConnection;
            List<clsSwitchDetails> listSwitchDetails = new List<clsSwitchDetails>();

            SqlCommand cmd = new SqlCommand();
            SqlDataReader dr1;

            con1.Open();
            cmd.Connection = con1;
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "[SWITCH_DetailsGet]";

            cmd.Parameters.Add("@param_intSwitchID", System.Data.SqlDbType.Int).Value = intSwitchID;

            dr1 = cmd.ExecuteReader();

            float fTotalAllocation = 0;

            while (dr1.Read())
            {

                clsSwitchDetails newClsSwitchDetails = new clsSwitchDetails();

                newClsSwitchDetails.propAllocation = float.Parse(Math.Round(double.Parse(dr1["Allocation"].ToString()), 2).ToString());
                newClsSwitchDetails.propCreated_By = dr1["Created_By"].ToString();
                newClsSwitchDetails.propDate_Created = DateTime.Parse(dr1["Date_Created"].ToString());
                newClsSwitchDetails.propDate_LastUpdate = DateTime.Parse(dr1["Date_LastUpdate"].ToString());
                newClsSwitchDetails.propFund = new clsFund(int.Parse(dr1["FundID"].ToString()));
                newClsSwitchDetails.propFundID = int.Parse(dr1["FundID"].ToString());
                newClsSwitchDetails.propSwitchDetailsID = int.Parse(dr1["SwitchDetailsID"].ToString());
                newClsSwitchDetails.propSwitchID = int.Parse(dr1["SwitchID"].ToString());
                newClsSwitchDetails.propUpdated_By = dr1["Updated_By"].ToString();
                newClsSwitchDetails.propIsDeletable = dr1["isDeletable"].ToString().Equals("1") ? true : false;

                if (Portfolio.propClient.propCurrency != newClsSwitchDetails.propFund.propCurrency)
                {
                    newClsSwitchDetails.propUnits = clsSwitchDetails.computeUnits(newClsSwitchDetails.propAllocation,
                                                                                  float.Parse(Math.Round(double.Parse(Portfolio.propPortfolioDetails[0].propTotalCurrentValueClient.ToString()), 0).ToString()),
                                                                                  clsCurrency.convertToClientCurrency(this.propClientID, newClsSwitchDetails.propFund.propPrice, newClsSwitchDetails.propFund.propCurrency));
                }
                else
                {
                    newClsSwitchDetails.propUnits = clsSwitchDetails.computeUnits(newClsSwitchDetails.propAllocation,
                                                                                  float.Parse(Math.Round(double.Parse(Portfolio.propPortfolioDetails[0].propTotalCurrentValueClient.ToString()), 0).ToString()),
                                                                                  newClsSwitchDetails.propFund.propPrice);
                }


                newClsSwitchDetails.propCurrencyMultiplier = clsCurrency.getCurrencyMultiplier(this.propClientID, newClsSwitchDetails.propFund.propCurrency);
                //newClsSwitchDetails.propValue = clsSwitchDetails.computeValue(newClsSwitchDetails.propAllocation, Portfolio.propPortfolioDetails[0].propTotalCurrentValueClient);
                newClsSwitchDetails.propTotalValue = float.Parse(Math.Round(double.Parse(Portfolio.propPortfolioDetails[0].propTotalCurrentValueClient.ToString()), 0).ToString());
                newClsSwitchDetails.propValue = clsSwitchDetails.computeValue(newClsSwitchDetails.propAllocation, newClsSwitchDetails.propTotalValue);


                fTotalAllocation = fTotalAllocation + newClsSwitchDetails.propAllocation;
                newClsSwitchDetails.propTotalAllocation = fTotalAllocation;

                listSwitchDetails.Add(newClsSwitchDetails);

            }
            con1.Close();
            cmd.Dispose();
            con1.Dispose();

            return listSwitchDetails;
        }

        public enum enumSwitchStatus 
        {
            //DO NOT MODIFY the ORDER! <Draft, Saved, Proposed, Amended, Declined_IFA, Declined_Client, Approved>

            Draft,
            Saved,
            Proposed,
            Amended,
            Declined_IFA,
            Declined_Client,
            Approved,
            Locked,
            Request_ForDiscussion,
            Cancelled,
            Completed
        }

        public static string getSwitchStringStatus(Int16 int16Status)
        {
            //DO NOT MODIFY the ORDER! <Draft, Saved, Proposed, Amended, Declined_IFA, Declined_Client, Approved>

            string SwitchStringStatus = "UNDEFINED";

            switch (int16Status)
            {
                case 0:
                    SwitchStringStatus = enumSwitchStatus.Draft.ToString();
                    break;
                case 1:
                    SwitchStringStatus = enumSwitchStatus.Saved.ToString();
                    break;
                case 2:
                    SwitchStringStatus = enumSwitchStatus.Proposed.ToString();
                    break;
                case 3:
                    SwitchStringStatus = enumSwitchStatus.Amended.ToString();
                    break;
                case 4:
                    SwitchStringStatus = enumSwitchStatus.Declined_IFA.ToString();
                    break;
                case 5:
                    SwitchStringStatus = enumSwitchStatus.Declined_Client.ToString();
                    break;
                case 6:
                    SwitchStringStatus = enumSwitchStatus.Approved.ToString();
                    break;
                case 7:
                    SwitchStringStatus = enumSwitchStatus.Locked.ToString();
                    break;
                case 8:
                    SwitchStringStatus = "Request for discussion";
                    break;
                case 9:
                    SwitchStringStatus = enumSwitchStatus.Cancelled.ToString();                        
                    break;
                case 10:
                    SwitchStringStatus = enumSwitchStatus.Completed.ToString();
                    break;
                default:
                    SwitchStringStatus = enumSwitchStatus.Draft.ToString();
                    break;
            }

            return SwitchStringStatus;
        }
        public static int insertCustomizedSwitchHeaderWithModel(int intIFA_ID, string strClientID, string strPortfolioID, string strUserID, enumSwitchStatus SwitchStatus, Nullable<int> intSwitchID, string strModelGroupID, string strModelPortfolioID)
        {

            SqlConnection con = new clsSystem_DBConnection(clsSystem_DBConnection.strConnectionString.NavIntegrationDB).propConnection;
            SqlCommand cmd = new SqlCommand();

            con.Open();
            cmd.Connection = con;
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "[SWITCH_Customized_HeaderInsert]";

            cmd.Parameters.Add("@param_intIFA_ID", System.Data.SqlDbType.Int).Value = intIFA_ID;
            cmd.Parameters.Add("@param_strClientID", System.Data.SqlDbType.NVarChar).Value = strClientID;
            cmd.Parameters.Add("@param_strPortfolioID", System.Data.SqlDbType.NVarChar).Value = strPortfolioID;
            cmd.Parameters.Add("@param_intStatus", System.Data.SqlDbType.SmallInt).Value = SwitchStatus;
            cmd.Parameters.Add("@param_strCreated_By", System.Data.SqlDbType.NVarChar).Value = strUserID;
            cmd.Parameters.Add("@param_intSwitchID", System.Data.SqlDbType.Int).Value = intSwitchID;
            cmd.Parameters.Add("@param_strModelGroupID", System.Data.SqlDbType.NVarChar).Value = strModelGroupID;
            cmd.Parameters.Add("@param_strModelPortfolioID", System.Data.SqlDbType.NVarChar).Value = strModelPortfolioID;

            return int.Parse(cmd.ExecuteScalar().ToString());

        }
        public static int insertSwitchHeaderWithModel(string strPortfolioID, string strClientID, string strUserID, enumSwitchStatus SwitchStatus, Nullable<int> intSwitchID, string strDescription, Nullable<int> intModelID, string strModelGroupID, string strModelPortfolioID)
        {

            SqlConnection con = new clsSystem_DBConnection(clsSystem_DBConnection.strConnectionString.NavIntegrationDB).propConnection;
            SqlCommand cmd = new SqlCommand();

            con.Open();
            cmd.Connection = con;
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "[SWITCH_HeaderInsert]";

            cmd.Parameters.Add("@param_strPortfolioID", System.Data.SqlDbType.NVarChar).Value = strPortfolioID;
            cmd.Parameters.Add("@param_strClientID", System.Data.SqlDbType.NVarChar).Value = strClientID;
            cmd.Parameters.Add("@param_intStatus", System.Data.SqlDbType.SmallInt).Value = SwitchStatus;
            cmd.Parameters.Add("@param_strCreated_By", System.Data.SqlDbType.NVarChar).Value = strUserID;
            cmd.Parameters.Add("@param_intSwitchID", System.Data.SqlDbType.Int).Value = intSwitchID;
            cmd.Parameters.Add("@param_strDescription", System.Data.SqlDbType.NVarChar).Value = strDescription;
            cmd.Parameters.Add("@param_ModelID", System.Data.SqlDbType.Int).Value = intModelID;
            cmd.Parameters.Add("@param_strModelGroupID", System.Data.SqlDbType.NVarChar).Value = strModelGroupID;
            cmd.Parameters.Add("@param_strModelPortfolioID", System.Data.SqlDbType.NVarChar).Value = strModelPortfolioID;

            return int.Parse(cmd.ExecuteScalar().ToString());

        }
        public static int insertSwitchHeader(string strPortfolioID, string strClientID, string strUserID, enumSwitchStatus SwitchStatus, Nullable<int> intSwitchID, string strDescription)
        {

            SqlConnection con = new clsSystem_DBConnection(clsSystem_DBConnection.strConnectionString.NavIntegrationDB).propConnection;
            SqlCommand cmd = new SqlCommand();

            con.Open();
            cmd.Connection = con;
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "[SWITCH_HeaderInsert]";

            cmd.Parameters.Add("@param_strPortfolioID", System.Data.SqlDbType.NVarChar).Value = strPortfolioID;
            cmd.Parameters.Add("@param_strClientID", System.Data.SqlDbType.NVarChar).Value = strClientID;
            cmd.Parameters.Add("@param_intStatus", System.Data.SqlDbType.SmallInt).Value = SwitchStatus;
            cmd.Parameters.Add("@param_strCreated_By", System.Data.SqlDbType.NVarChar).Value = strUserID;
            cmd.Parameters.Add("@param_intSwitchID", System.Data.SqlDbType.Int).Value = intSwitchID;
            cmd.Parameters.Add("@param_strDescription", System.Data.SqlDbType.NVarChar).Value = strDescription;

            return int.Parse(cmd.ExecuteScalar().ToString());
            
        }

        public static void deleteSwitch(int intSwitchID)
        {            

            SqlConnection con = new clsSystem_DBConnection(clsSystem_DBConnection.strConnectionString.NavIntegrationDB).propConnection;
            SqlCommand cmd = new SqlCommand();

            con.Open();
            cmd.Connection = con;
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "[SWITCH_Delete]";

            cmd.Parameters.Add("@param_SwitchID", System.Data.SqlDbType.Int).Value = intSwitchID;

            cmd.ExecuteNonQuery();

        }
        public static void deleteSwitchDetails(int intSwitchID)
        {

            SqlConnection con = new clsSystem_DBConnection(clsSystem_DBConnection.strConnectionString.NavIntegrationDB).propConnection;
            SqlCommand cmd = new SqlCommand();

            con.Open();
            cmd.Connection = con;
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "[SWITCH_DetailsDeleteAll]";

            cmd.Parameters.Add("@param_SwitchID", System.Data.SqlDbType.Int).Value = intSwitchID;

            cmd.ExecuteNonQuery();
        }
        public static void updateSwitchHeader(int intSwitchID, enumSwitchStatus SwitchStatus)
        {

            SqlConnection con = new clsSystem_DBConnection(clsSystem_DBConnection.strConnectionString.NavIntegrationDB).propConnection;
            SqlCommand cmd = new SqlCommand();

            con.Open();
            cmd.Connection = con;
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "[SWITCH_HeaderUpdate]";

            cmd.Parameters.Add("@param_intSwitchID", System.Data.SqlDbType.Int).Value = intSwitchID;
            cmd.Parameters.Add("@param_Status", System.Data.SqlDbType.SmallInt).Value = SwitchStatus;

            cmd.ExecuteNonQuery();

        }

        public static void updateSwitchHeader(int intSwitchID, enumSwitchStatus SwitchStatus, string strDescription)
        {
            SqlConnection con = new clsSystem_DBConnection(clsSystem_DBConnection.strConnectionString.NavIntegrationDB).propConnection;
            SqlCommand cmd = new SqlCommand();

            con.Open();
            cmd.Connection = con;
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "[SWITCH_HeaderUpdate]";

            cmd.Parameters.Add("@param_intSwitchID", System.Data.SqlDbType.Int).Value = intSwitchID;
            cmd.Parameters.Add("@param_Status", System.Data.SqlDbType.SmallInt).Value = SwitchStatus;
            cmd.Parameters.Add("@param_Description", System.Data.SqlDbType.NVarChar).Value = strDescription;

            cmd.ExecuteNonQuery();

        }

        public static List<clsSwitch> getSwitchList(int intIFA_ID, string strClientName, string strCompany, int intStatus, string strStartDate, string strEndDate)
        {
            List<clsSwitch> oSwitchList = new List<clsSwitch>();

            SqlConnection con = new clsSystem_DBConnection(clsSystem_DBConnection.strConnectionString.NavIntegrationDB).propConnection;
            SqlCommand cmd = new SqlCommand();
            SqlDataReader dr;

            con.Open();
            cmd.Connection = con;
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "[SWITCH_HeaderGetAllByIFA]";

            cmd.Parameters.Add("@param_IFA_ID", System.Data.SqlDbType.Int).Value = intIFA_ID;
            cmd.Parameters.Add("@param_ClientName", System.Data.SqlDbType.NVarChar).Value = strClientName;
            cmd.Parameters.Add("@param_Company", System.Data.SqlDbType.NVarChar).Value = strCompany;
            cmd.Parameters.Add("@param_Status", System.Data.SqlDbType.Int).Value = intStatus;
            cmd.Parameters.Add("@param_StartDate", System.Data.SqlDbType.NVarChar).Value = strStartDate;
            cmd.Parameters.Add("@param_EndDate", System.Data.SqlDbType.NVarChar).Value = strEndDate;
            dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                clsSwitch oSwitch = new clsSwitch();
                oSwitch.propSwitchID = int.Parse(dr["SwitchID"].ToString());
                oSwitch.propPortfolioID = dr["PortfolioID"].ToString();
                oSwitch.propClientID = dr["ClientID"].ToString();
                oSwitch.propStatus = short.Parse(dr["Status"].ToString());
                oSwitch.propStatusString = clsSwitch.getSwitchStringStatus(oSwitch.propStatus);
                oSwitch.propDate_Created = dr["Date_Created"] != System.DBNull.Value ? DateTime.Parse(dr["Date_Created"].ToString()) : DateTime.ParseExact("01/01/1800", "dd/MM/yyyy", null);
                oSwitch.propCreated_By = dr["Created_By"].ToString();
                oSwitch.propDescription = dr["Description"].ToString();
                oSwitch.propSwitchClient = new clsClient(oSwitch.propClientID);
                oSwitch.propPortfolio = new clsPortfolio(oSwitch.propClientID, oSwitch.propPortfolioID);

                oSwitchList.Add(oSwitch);
            }

            con.Close();
            cmd.Dispose();
            con.Dispose();

            return oSwitchList;
        }
        public static List<clsSwitch> getSwitchList(int[] intSwitches)
        {
            List<clsSwitch> oSwitchList = new List<clsSwitch>();
            SqlConnection con = new clsSystem_DBConnection(clsSystem_DBConnection.strConnectionString.NavIntegrationDB).propConnection;


            con.Open();
            for (int i = 0; i < intSwitches.Length; i++)
            {
                SqlCommand cmd = new SqlCommand();
                SqlDataReader dr;

                cmd.Connection = con;
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = "[SWITCH_HeaderGet]";

                cmd.Parameters.Add("@param_intSwitchID", System.Data.SqlDbType.Int).Value = intSwitches[i];

                dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    clsSwitch oSwitch = new clsSwitch();
                    oSwitch.propSwitchID = int.Parse(dr["SwitchID"].ToString());
                    oSwitch.propPortfolioID = dr["PortfolioID"].ToString();
                    oSwitch.propClientID = dr["ClientID"].ToString();
                    oSwitch.propStatus = short.Parse(dr["Status"].ToString());
                    oSwitch.propStatusString = clsSwitch.getSwitchStringStatus(oSwitch.propStatus);
                    oSwitch.propDate_Created = dr["Date_Created"] != System.DBNull.Value ? DateTime.Parse(dr["Date_Created"].ToString()) : DateTime.ParseExact("01/01/1800", "dd/MM/yyyy", null);
                    oSwitch.propCreated_By = dr["Created_By"].ToString();
                    oSwitch.propDescription = dr["Description"].ToString();
                    oSwitch.propSwitchClient = new clsClient(oSwitch.propClientID);
                    oSwitch.propPortfolio = new clsPortfolio(oSwitch.propClientID, oSwitch.propPortfolioID);

                    oSwitchList.Add(oSwitch);
                }
                dr.Close();
                dr.Dispose();
                cmd.Dispose();
            }
            con.Close();
            con.Dispose();

            return oSwitchList;
        }
        public void getSwitchInfoFromTemp(clsPortfolio Portfolio, string strUserID, int intIFA_ID)
        {
            SqlCommand cmd = new SqlCommand();
            SqlDataReader dr;
            con.Open();
            cmd.Connection = con;
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "[SWITCH_HeaderTempGet]";

            cmd.Parameters.Add("@param_intIFA_ID", System.Data.SqlDbType.NVarChar).Value = intIFA_ID;
            cmd.Parameters.Add("@param_ModelGroupID", System.Data.SqlDbType.NVarChar).Value = this.strModelGroupID;
            cmd.Parameters.Add("@param_ModelPortfolioID", System.Data.SqlDbType.NVarChar).Value = this.strModelPortfolioID;
            cmd.Parameters.Add("@param_strPortfolioID", System.Data.SqlDbType.NVarChar).Value = Portfolio.propPortfolioID;
            cmd.Parameters.Add("@param_strClientID", System.Data.SqlDbType.NVarChar).Value = Portfolio.propClientID;

            dr = cmd.ExecuteReader();

            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    this.propClientID = dr["ClientID"].ToString();
                    this.propDate_Created = DateTime.Parse(dr["Date_Created"].ToString());
                    this.propPortfolioID = dr["PortfolioID"].ToString();
                    this.propStatus = short.Parse(dr["Status"].ToString());
                    this.propStatusString = getSwitchStringStatus(this.propStatus);
                    this.propSwitchID = int.Parse(dr["SwitchID"].ToString());
                    if (propStatus == (int)enumSwitchStatus.Declined_Client)
                    {
                        this.propSwitchDetails = clsSwitchDetails.replicatePortfolioDetails(Portfolio);
                    }
                    else
                    {
                        this.propSwitchDetails = getSwitchDetailsFromTemp(Portfolio, strUserID, strPortfolioID);
                    }
                    this.propCreated_By = dr["Created_By"].ToString();
                    this.propDescription = dr["Description"].ToString();
                }
            }
            else
            {
                this.propSwitchID = 0;
                this.propPortfolioID = Portfolio.propPortfolioID;
                this.propClientID = Portfolio.propClientID;
                this.propStatus = (short)enumSwitchStatus.Saved;
                this.propStatusString = getSwitchStringStatus(this.propStatus);
                this.propCreated_By = strUserID;
                this.propSwitchDetails = clsSwitchDetails.replicatePortfolioDetails(Portfolio);
            }

            dr.Close();
            con.Close();
            cmd.Dispose();
            //con.Dispose();
        }
        //SwitchHeaderTemp
        public List<clsSwitchDetails> getSwitchDetailsFromTemp(clsPortfolio _clsPortfolio, string strClientID, string strPortfolioID)
        {
            SqlConnection con1 = new clsSystem_DBConnection(clsSystem_DBConnection.strConnectionString.NavIntegrationDB).propConnection;
            List<clsSwitchDetails> listSwitchDetails = new List<clsSwitchDetails>();

            SqlCommand cmd = new SqlCommand();
            SqlDataReader dr1;

            con1.Open();
            cmd.Connection = con1;
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "[SWITCH_DetailsTempGet]";

            cmd.Parameters.Add("@param_strClientID", System.Data.SqlDbType.NVarChar).Value = strClientID;
            cmd.Parameters.Add("@param_strPortfolioID", System.Data.SqlDbType.NVarChar).Value = strPortfolioID;

            dr1 = cmd.ExecuteReader();

            float fTotalAllocation = 0;

            while (dr1.Read())
            {

                clsSwitchDetails newClsSwitchDetails = new clsSwitchDetails();

                newClsSwitchDetails.propAllocation = float.Parse(Math.Round(double.Parse(dr1["Allocation"].ToString()), 2).ToString());
                newClsSwitchDetails.propCreated_By = dr1["Created_By"].ToString();
                newClsSwitchDetails.propDate_Created = DateTime.Parse(dr1["Date_Created"].ToString());
                newClsSwitchDetails.propDate_LastUpdate = DateTime.Parse(dr1["Date_LastUpdate"].ToString());
                newClsSwitchDetails.propFund = new clsFund(int.Parse(dr1["FundID"].ToString()));
                newClsSwitchDetails.propFundID = int.Parse(dr1["FundID"].ToString());
                newClsSwitchDetails.propSwitchDetailsID = int.Parse(dr1["SwitchDetailsID"].ToString());
                newClsSwitchDetails.propSwitchID = int.Parse(dr1["SwitchID"].ToString());
                newClsSwitchDetails.propUpdated_By = dr1["Updated_By"].ToString();
                newClsSwitchDetails.propIsDeletable = dr1["isDeletable"].ToString().Equals("1") ? true : false;

                //if (Portfolio.propPortfolioDetails[0].propClientCurrency != newClsSwitchDetails.propFund.propCurrency)
                if (_clsPortfolio.propClient.propCurrency != newClsSwitchDetails.propFund.propCurrency)
                {
                    newClsSwitchDetails.propUnits = clsSwitchDetails.computeUnits(newClsSwitchDetails.propAllocation,
                                                                                  float.Parse(Math.Round(double.Parse(_clsPortfolio.propPortfolioDetails[0].propTotalCurrentValueClient.ToString()), 0).ToString()),
                                                                                  clsCurrency.convertToClientCurrency(_clsPortfolio.propClientID, newClsSwitchDetails.propFund.propPrice, newClsSwitchDetails.propFund.propCurrency));
                }
                else
                {
                    newClsSwitchDetails.propUnits = clsSwitchDetails.computeUnits(newClsSwitchDetails.propAllocation,
                                                                                  float.Parse(Math.Round(double.Parse(_clsPortfolio.propPortfolioDetails[0].propTotalCurrentValueClient.ToString()), 0).ToString()),
                                                                                  newClsSwitchDetails.propFund.propPrice);
                }


                newClsSwitchDetails.propCurrencyMultiplier = clsCurrency.getCurrencyMultiplier(_clsPortfolio.propClientID, newClsSwitchDetails.propFund.propCurrency);
                //newClsSwitchDetails.propValue = clsSwitchDetails.computeValue(newClsSwitchDetails.propAllocation, Portfolio.propPortfolioDetails[0].propTotalCurrentValueClient);
                newClsSwitchDetails.propTotalValue = float.Parse(Math.Round(double.Parse(_clsPortfolio.propPortfolioDetails[0].propTotalCurrentValueClient.ToString()), 0).ToString());
                newClsSwitchDetails.propValue = clsSwitchDetails.computeValue(newClsSwitchDetails.propAllocation, newClsSwitchDetails.propTotalValue);


                fTotalAllocation = fTotalAllocation + newClsSwitchDetails.propAllocation;
                newClsSwitchDetails.propTotalAllocation = fTotalAllocation;

                listSwitchDetails.Add(newClsSwitchDetails);

            }
            con1.Close();
            cmd.Dispose();
            con1.Dispose();

            return listSwitchDetails;
        }
        public List<clsSwitchDetails> replicateModelPortfolioDetails(clsPortfolio _clsPortfolio, string strClientID, string strPortfolioID)
        {
            SqlConnection con1 = new clsSystem_DBConnection(clsSystem_DBConnection.strConnectionString.NavIntegrationDB).propConnection;
            List<clsSwitchDetails> listSwitchDetails = new List<clsSwitchDetails>();

            SqlCommand cmd = new SqlCommand();
            SqlDataReader dr1;

            con1.Open();
            cmd.Connection = con1;
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "[SWITCH_PortfolioDetailsGet]";

            cmd.Parameters.Add("@param_strClientID", System.Data.SqlDbType.NVarChar).Value = strClientID;
            cmd.Parameters.Add("@param_strPortfolioID", System.Data.SqlDbType.NVarChar).Value = strPortfolioID;

            dr1 = cmd.ExecuteReader();

            float fTotalAllocation = 0;

            while (dr1.Read())
            {

                clsSwitchDetails newClsSwitchDetails = new clsSwitchDetails();

                newClsSwitchDetails.propAllocation = float.Parse(Math.Round(double.Parse(dr1["Allocation"].ToString()), 2).ToString());
                newClsSwitchDetails.propCreated_By = dr1["Created_By"].ToString();
                newClsSwitchDetails.propDate_Created = DateTime.Parse(dr1["Date_Created"].ToString());
                newClsSwitchDetails.propDate_LastUpdate = DateTime.Parse(dr1["Date_LastUpdate"].ToString());
                newClsSwitchDetails.propFund = new clsFund(int.Parse(dr1["FundID"].ToString()));
                newClsSwitchDetails.propFundID = int.Parse(dr1["FundID"].ToString());
                newClsSwitchDetails.propSwitchDetailsID = int.Parse(dr1["SwitchDetailsID"].ToString());
                newClsSwitchDetails.propSwitchID = int.Parse(dr1["SwitchID"].ToString());
                newClsSwitchDetails.propUpdated_By = dr1["Updated_By"].ToString();
                newClsSwitchDetails.propIsDeletable = dr1["isDeletable"].ToString().Equals("1") ? true : false;

                //if (Portfolio.propPortfolioDetails[0].propClientCurrency != newClsSwitchDetails.propFund.propCurrency)
                if (_clsPortfolio.propClient.propCurrency != newClsSwitchDetails.propFund.propCurrency)
                {
                    newClsSwitchDetails.propUnits = clsSwitchDetails.computeUnits(newClsSwitchDetails.propAllocation,
                                                                                  float.Parse(Math.Round(double.Parse(_clsPortfolio.propPortfolioDetails[0].propTotalCurrentValueClient.ToString()), 0).ToString()),
                                                                                  clsCurrency.convertToClientCurrency(_clsPortfolio.propClientID, newClsSwitchDetails.propFund.propPrice, newClsSwitchDetails.propFund.propCurrency));
                }
                else
                {
                    newClsSwitchDetails.propUnits = clsSwitchDetails.computeUnits(newClsSwitchDetails.propAllocation,
                                                                                  float.Parse(Math.Round(double.Parse(_clsPortfolio.propPortfolioDetails[0].propTotalCurrentValueClient.ToString()), 0).ToString()),
                                                                                  newClsSwitchDetails.propFund.propPrice);
                }


                newClsSwitchDetails.propCurrencyMultiplier = clsCurrency.getCurrencyMultiplier(_clsPortfolio.propClientID, newClsSwitchDetails.propFund.propCurrency);
                //newClsSwitchDetails.propValue = clsSwitchDetails.computeValue(newClsSwitchDetails.propAllocation, Portfolio.propPortfolioDetails[0].propTotalCurrentValueClient);
                newClsSwitchDetails.propTotalValue = float.Parse(Math.Round(double.Parse(_clsPortfolio.propPortfolioDetails[0].propTotalCurrentValueClient.ToString()), 0).ToString());
                newClsSwitchDetails.propValue = clsSwitchDetails.computeValue(newClsSwitchDetails.propAllocation, newClsSwitchDetails.propTotalValue);


                fTotalAllocation = fTotalAllocation + newClsSwitchDetails.propAllocation;
                newClsSwitchDetails.propTotalAllocation = fTotalAllocation;

                listSwitchDetails.Add(newClsSwitchDetails);

            }
            con1.Close();
            cmd.Dispose();
            con1.Dispose();

            return listSwitchDetails;
        }
    }
}
