using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;

namespace NAV
{
    public class clsSwitchScheme_Client
    {
        private SqlConnection con = new clsSystem_DBConnection(clsSystem_DBConnection.strConnectionString.NavIntegrationDB).propConnection;

        #region properties

        private int intSwitchID;
        public int propSwitchID { get { return intSwitchID; } set { intSwitchID = value; } }

        private clsScheme Scheme;
        public clsScheme propScheme { get { return Scheme; } set { Scheme = value; } }

        private clsClient Client;
        public clsClient propClient { get { return Client; } set { Client = value; } }

        private Int16 sintStatus;
        public Int16 propStatus { get { return sintStatus; } set { sintStatus = value; } }

        private string strStatus = "Draft";
        public string propStatusString { get { return strStatus; } set { strStatus = value; } }

        private DateTime dtDate_Created;
        public DateTime propDate_Created { get { return dtDate_Created; } set { dtDate_Created = value; } }

        private string strCreated_By;
        public string propCreated_By { get { return strCreated_By; } set { strCreated_By = value; } }

        private string strDescription = string.Empty;
        public string propDescription { get { return strDescription; } set { strDescription = value; } }

        private List<clsSwitchSchemeDetails_Client> listSwitchDetailsPortfolio;
        public List<clsSwitchSchemeDetails_Client> propSwitchDetailsPortfolio { get { return listSwitchDetailsPortfolio; } set { listSwitchDetailsPortfolio = value; } }

        private List<clsSwitchSchemeDetails_Client> listSwitchDetailsContribution;
        public List<clsSwitchSchemeDetails_Client> propSwitchDetailsContribution { get { return listSwitchDetailsContribution; } set { listSwitchDetailsContribution = value; } }

        #endregion

        public clsSwitchScheme_Client(int intSwitchID)
        {
            getSwitchInfo(intSwitchID);
        }

        private void getSwitchInfo(int intSwitchID)
        {

            SqlCommand cmd = new SqlCommand();
            SqlDataReader dr;
            con.Open();
            cmd.Connection = con;
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "[SWITCHScheme_HeaderGet]";

            cmd.Parameters.Add("@param_intSwitchID", System.Data.SqlDbType.Int).Value = intSwitchID;

            dr = cmd.ExecuteReader();

            while (dr.Read())
            {

                this.propClient = new clsClient(dr["ClientID"].ToString());
                this.propCreated_By = dr["Created_By"].ToString();
                this.propDate_Created = DateTime.Parse(dr["Date_Created"].ToString());
                this.propDescription = dr["Amend_Description"].ToString();
                this.propScheme = new clsScheme(this.propClient.propClientID, dr["SchemeID"].ToString());
                this.propStatus = dr["Amend_Status"] != System.DBNull.Value ? short.Parse(dr["Amend_Status"].ToString()) : (short)0;
                this.propStatusString = clsSwitch.getSwitchStringStatus(this.propStatus);
                this.propSwitchID = int.Parse(dr["SwitchID"].ToString());
                this.propSwitchDetailsPortfolio = clsSwitchSchemeDetails_Client.getSwitchDetails(intSwitchID, false);
                this.propSwitchDetailsContribution = clsSwitchSchemeDetails_Client.getSwitchDetails(intSwitchID, true); 
            }

            dr.Close();
            con.Close();
            cmd.Dispose();
            //con.Dispose();
        }

        public static void updateSwitchHeader(clsSwitch.enumSwitchStatus SwitchStatus, int intSwitchID, string strDescription)
        {

            SqlConnection con = new clsSystem_DBConnection(clsSystem_DBConnection.strConnectionString.NavIntegrationDB).propConnection;
            SqlCommand cmd = new SqlCommand();

            con.Open();
            cmd.Connection = con;
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "[SWITCHSchemeclient_HeaderUpdate]";

            cmd.Parameters.Add("@param_intSwitchID", System.Data.SqlDbType.Int).Value = intSwitchID;
            cmd.Parameters.Add("@param_intStatus", System.Data.SqlDbType.SmallInt).Value = SwitchStatus;
            cmd.Parameters.Add("@param_strDescription", System.Data.SqlDbType.NVarChar).Value = strDescription;

            cmd.ExecuteNonQuery();

        }

        public static void deleteSwitch(int intSwitchID)
        {

            SqlConnection con = new clsSystem_DBConnection(clsSystem_DBConnection.strConnectionString.NavIntegrationDB).propConnection;
            SqlCommand cmd = new SqlCommand();

            con.Open();
            cmd.Connection = con;
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "[SWITCHSchemeClient_Delete]";

            cmd.Parameters.Add("@param_SwitchID", System.Data.SqlDbType.Int).Value = intSwitchID;

            cmd.ExecuteNonQuery();

        }

        public static void declineSwitch(int intSwitchID)
        {
            SqlConnection con = new clsSystem_DBConnection(clsSystem_DBConnection.strConnectionString.NavIntegrationDB).propConnection;
            SqlCommand cmd = new SqlCommand();

            con.Open();
            cmd.Connection = con;
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "[SWITCHSchemeClient_Decline]";

            cmd.Parameters.Add("@param_SwitchID", System.Data.SqlDbType.Int).Value = intSwitchID;

            cmd.ExecuteNonQuery();
        }


        public class clsSwitchSchemeDetails_Client
        {
            #region properties

            private int intSwitchDetailsID;
            public int propSwitchDetailsID { get { return intSwitchDetailsID; } set { intSwitchDetailsID = value; } }

            private clsSwitchScheme SwitchScheme;
            public clsSwitchScheme propSwitchScheme { get { return SwitchScheme; } set { SwitchScheme = value; } }

            private clsFund Fund;
            public clsFund propFund { get { return Fund; } set { Fund = value; } }

            private float fAllocation;
            public float propAllocation { get { return fAllocation; } set { fAllocation = value; } }

            private DateTime dtDate_Created;
            public DateTime propDate_Created { get { return dtDate_Created; } set { dtDate_Created = value; } }

            private string strCreated_By;
            public string propCreated_By { get { return strCreated_By; } set { strCreated_By = value; } }

            private DateTime dtDate_LastUpdate;
            public DateTime propDate_LastUpdate { get { return dtDate_LastUpdate; } set { dtDate_LastUpdate = value; } }

            private string strUpdated_By;
            public string propUpdated_By { get { return strUpdated_By; } set { strUpdated_By = value; } }

            private decimal dUnits;
            public decimal propUnits { get { return dUnits; } set { dUnits = value; } }

            private float fValue;
            public float propValue { get { return fValue; } set { fValue = value; } }

            private float fTotalValue;
            public float propTotalValue { get { return fTotalValue; } set { fTotalValue = value; } }

            private float fTotalAllocation;
            public float propTotalAllocation { get { return fTotalAllocation; } set { fTotalAllocation = value; } }

            private bool isDeletable;
            public bool propIsDeletable { get { return isDeletable; } set { isDeletable = value; } }

            private float fCurrencyMultiplier;
            public float propCurrencyMultiplier { get { return fCurrencyMultiplier; } set { fCurrencyMultiplier = value; } }

            private bool isContribution;
            public bool propIsContribution { get { return isContribution; } set { isContribution = value; } }

            #endregion

            public static List<clsSwitchSchemeDetails_Client> getSwitchDetails(int intSwitchID, Boolean isContribution)
            {

                SqlConnection con1 = new clsSystem_DBConnection(clsSystem_DBConnection.strConnectionString.NavIntegrationDB).propConnection;
                List<clsSwitchSchemeDetails_Client> listSwitchDetails = new List<clsSwitchSchemeDetails_Client>();

                clsSwitchScheme SwitchScheme = new clsSwitchScheme(intSwitchID);
                clsScheme Scheme = new clsScheme(SwitchScheme.propClient.propClientID, SwitchScheme.propScheme.propSchemeID);

                SqlCommand cmd = new SqlCommand();
                SqlDataReader dr1;

                con1.Open();
                cmd.Connection = con1;
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = "[SWITCHSchemeClient_DetailsGet]";

                cmd.Parameters.Add("@param_intSwitchID", System.Data.SqlDbType.Int).Value = intSwitchID;
                cmd.Parameters.Add("@param_isContribution", System.Data.SqlDbType.Int).Value = isContribution;

                dr1 = cmd.ExecuteReader();

                if (!dr1.HasRows)
                {
                    SqlConnection con2 = new clsSystem_DBConnection(clsSystem_DBConnection.strConnectionString.NavIntegrationDB).propConnection;
                    SqlCommand cmd2 = new SqlCommand();
                    cmd2.Connection = con2;
                    cmd2.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd2.CommandText = "[SWITCHScheme_DetailsGet]";
                    cmd2.Parameters.Add("@param_intSwitchID", System.Data.SqlDbType.Int).Value = intSwitchID;
                    cmd2.Parameters.Add("@param_isContribution", System.Data.SqlDbType.Int).Value = isContribution;
                    con2.Open();
                    dr1 = cmd2.ExecuteReader();
                }

                double dPrice;
                float fTotalAllocation = 0;

                while (dr1.Read())
                {

                    dPrice = 0;

                    clsSwitchSchemeDetails_Client newClsSwitchDetails = new clsSwitchSchemeDetails_Client();

                    newClsSwitchDetails.propAllocation = float.Parse(Math.Round(double.Parse(dr1["Allocation"].ToString()), 2).ToString());
                    newClsSwitchDetails.propCreated_By = dr1["Created_By"].ToString();
                    newClsSwitchDetails.propDate_Created = DateTime.Parse(dr1["Date_Created"].ToString());
                    newClsSwitchDetails.propDate_LastUpdate = DateTime.Parse(dr1["Date_LastUpdate"].ToString());
                    newClsSwitchDetails.propFund = new clsFund(int.Parse(dr1["FundID"].ToString()));
                    newClsSwitchDetails.propIsDeletable = dr1["isDeletable"].ToString().Equals("1") ? true : false;
                    newClsSwitchDetails.propSwitchDetailsID = int.Parse(dr1["SwitchDetailsID"].ToString());
                    newClsSwitchDetails.propSwitchScheme = SwitchScheme;
                    newClsSwitchDetails.propUpdated_By = dr1["Updated_By"].ToString();

                    newClsSwitchDetails.propCurrencyMultiplier = clsCurrency.getCurrencyMultiplier(Scheme.propClient.propClientID, newClsSwitchDetails.propFund.propCurrency);

                    if (Scheme.propClient.propCurrency != newClsSwitchDetails.propFund.propCurrency)
                    {
                        double dConvertedValue = clsCurrency.convertToClientCurrency(Scheme.propClient.propClientID, newClsSwitchDetails.propFund.propPrice, newClsSwitchDetails.propFund.propCurrency);
                        int intMarker = dConvertedValue.ToString().IndexOf('.');
                        string strIntegerPart = dConvertedValue.ToString().Substring(0, intMarker);
                        string strDecimalPart = dConvertedValue.ToString().Substring(intMarker, 4);
                        dPrice = Convert.ToDouble(strIntegerPart + strDecimalPart);
                    }
                    else
                    {
                        dPrice = Math.Round(newClsSwitchDetails.propFund.propPrice, 4);
                    }

                    newClsSwitchDetails.propTotalValue = float.Parse(Math.Round(double.Parse(Scheme.propCC_TotalValue.ToString()), 0).ToString());

                    fTotalAllocation = fTotalAllocation + newClsSwitchDetails.propAllocation;
                    newClsSwitchDetails.propTotalAllocation = fTotalAllocation;

                    newClsSwitchDetails.propValue = float.Parse(((Math.Round(newClsSwitchDetails.propAllocation, 2) / 100) * Math.Round(Scheme.propCC_TotalValue, 0)).ToString());
                    newClsSwitchDetails.propUnits = Convert.ToDecimal((((Math.Round(newClsSwitchDetails.propAllocation, 2) / 100) * Math.Round(Scheme.propCC_TotalValue, 0)) / dPrice).ToString());

                    listSwitchDetails.Add(newClsSwitchDetails);

                }
                con1.Close();
                cmd.Dispose();
                con1.Dispose();

                return listSwitchDetails;
            }

            public static void insertSwitchDetails(List<clsSwitchSchemeDetails_Client> listSwitchDetails, string strUserID, int intSwitchID, Boolean isContribution)
            {
                SqlConnection con = new clsSystem_DBConnection(clsSystem_DBConnection.strConnectionString.NavIntegrationDB).propConnection;
                con.Open();

                foreach (clsSwitchSchemeDetails_Client SwitchDetail in listSwitchDetails)
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = con;

                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "[SWITCHSchemeClient_DetailsInsert]";

                    cmd.Parameters.Add("@param_intSwitchID", System.Data.SqlDbType.Int).Value = intSwitchID;
                    cmd.Parameters.Add("@param_intFundID", System.Data.SqlDbType.Int).Value = SwitchDetail.propFund.propFundID;
                    cmd.Parameters.Add("@param_fAllocation", System.Data.SqlDbType.Float).Value = SwitchDetail.propAllocation;
                    cmd.Parameters.Add("@param_strCreated_By", System.Data.SqlDbType.NVarChar).Value = strUserID;
                    cmd.Parameters.Add("@param_strUpdated_By", System.Data.SqlDbType.NVarChar).Value = strUserID;
                    cmd.Parameters.Add("@param_intSwitchDetailsID", System.Data.SqlDbType.Int).Value = SwitchDetail.intSwitchDetailsID;
                    cmd.Parameters.Add("@param_sintIsDeletable", System.Data.SqlDbType.SmallInt).Value = SwitchDetail.propIsDeletable == true ? 1 : 0;
                    cmd.Parameters.Add("@param_sintIsContribution", System.Data.SqlDbType.SmallInt).Value = isContribution == true ? 1 : 0;

                    cmd.ExecuteNonQuery();
                }
            }

            public static List<clsSwitchSchemeDetails_Client> FundChange(int intFundID_old, int intFundID_new, List<clsSwitchSchemeDetails_Client> listSwitchDetails, string strClientID, string strClientCurrency, Boolean isContribution)
            {
                List<clsSwitchSchemeDetails_Client> newListSwitchDetails = new List<clsSwitchSchemeDetails_Client>();
                float fTotalAllocation = 0;
                float fTotalValue = listSwitchDetails[0].propTotalValue;

                foreach (clsSwitchSchemeDetails_Client SwitchDetail in listSwitchDetails)
                {
                    if (SwitchDetail.propFund.propFundID == intFundID_new)
                    {
                        throw new Exception("Fund already exists.");
                    }

                    if (SwitchDetail.propFund.propFundID == intFundID_old)
                    {

                        clsFund FundInfo = new clsFund(intFundID_new);

                        SwitchDetail.propFund = FundInfo;

                        if (strClientCurrency != FundInfo.propCurrency)
                        {
                            SwitchDetail.propUnits = clsSwitchDetails.computeUnits(SwitchDetail.propAllocation, fTotalValue, clsCurrency.convertToClientCurrency(strClientID, FundInfo.propPrice, FundInfo.propCurrency));
                        }
                        else
                        {
                            SwitchDetail.propUnits = clsSwitchDetails.computeUnits(SwitchDetail.propAllocation, fTotalValue, FundInfo.propPrice);
                        }

                        SwitchDetail.propValue = clsSwitchDetails.computeValue(SwitchDetail.propAllocation, fTotalValue);

                        SwitchDetail.propCurrencyMultiplier = clsCurrency.getCurrencyMultiplier(strClientID, FundInfo.propCurrency);
                    }

                    fTotalAllocation = fTotalAllocation + SwitchDetail.propAllocation;

                    SwitchDetail.propTotalAllocation = fTotalAllocation;
                    SwitchDetail.propIsContribution = isContribution;

                    newListSwitchDetails.Add(SwitchDetail);
                }

                return newListSwitchDetails;
            }

            public static List<clsSwitchSchemeDetails_Client> addFund(int intNewFundID, List<clsSwitchSchemeDetails_Client> currentlistSwitchDetails, string strClientID, string strClientCurrency, string strUserID, Boolean isContribution)
            {

                float fTotalAllocation = 0;
                float fTotalValue = currentlistSwitchDetails[0].propTotalValue;

                foreach (clsSwitchSchemeDetails_Client SwitchDetail in currentlistSwitchDetails)
                {
                    fTotalAllocation = fTotalAllocation + SwitchDetail.propAllocation;

                    if (SwitchDetail.propFund.propFundID == intNewFundID)
                    {
                        throw new Exception("Fund already exists.");
                    }
                }

                clsSwitchSchemeDetails_Client NewSwitchDetail = new clsSwitchSchemeDetails_Client();

                NewSwitchDetail.propCreated_By = strUserID;
                NewSwitchDetail.propFund = new clsFund(intNewFundID);
                NewSwitchDetail.propIsDeletable = true;
                NewSwitchDetail.propSwitchDetailsID = 0;
                NewSwitchDetail.propSwitchScheme = NewSwitchDetail.propSwitchScheme;
                NewSwitchDetail.propTotalAllocation = fTotalAllocation;
                NewSwitchDetail.propCurrencyMultiplier = clsCurrency.getCurrencyMultiplier(strClientID, NewSwitchDetail.propFund.propCurrency);
                NewSwitchDetail.propIsContribution = isContribution;

                currentlistSwitchDetails.Add(NewSwitchDetail);

                return currentlistSwitchDetails;

            }

            public static List<clsSwitchSchemeDetails_Client> removeSwitchDetails(int intFundID, List<clsSwitchSchemeDetails_Client> listSwitchDetails)
            {
                List<clsSwitchSchemeDetails_Client> newListSwitchDetails = new List<clsSwitchSchemeDetails_Client>();

                foreach (clsSwitchSchemeDetails_Client SwitchDetail in listSwitchDetails)
                {
                    if (SwitchDetail.propFund.propFundID != intFundID)
                    {
                        newListSwitchDetails.Add(SwitchDetail);
                    }
                }

                return newListSwitchDetails;
            }

            public static void deleteAllDetails(Nullable<int> intSwitchID)
            {
                if (intSwitchID == null) { return; }

                SqlConnection con = new clsSystem_DBConnection(clsSystem_DBConnection.strConnectionString.NavIntegrationDB).propConnection;
                con.Open();

                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;

                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = "[SWITCHSchemeClient_DetailsDeleteAll]";

                cmd.Parameters.Add("@param_SwitchID", System.Data.SqlDbType.Int).Value = intSwitchID;
                cmd.ExecuteNonQuery();

            }
        }

    }
}
