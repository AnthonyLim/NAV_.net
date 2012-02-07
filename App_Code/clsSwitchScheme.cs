using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;

namespace NAV
{
    public class clsSwitchScheme
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

        private List<clsSwitchSchemeDetails> listSwitchDetails;
        public List<clsSwitchSchemeDetails> propSwitchDetails { get { return listSwitchDetails; } set { listSwitchDetails = value; } }

        private List<clsSwitchSchemeDetails> listSwitchDetailsContribution;
        public List<clsSwitchSchemeDetails> propSwitchDetailsContribution { get { return listSwitchDetailsContribution; } set { listSwitchDetailsContribution = value; } }

        #endregion

        #region "constructors"

        public clsSwitchScheme() { }

        public clsSwitchScheme(int intSwitchID)
        {
            getSwitchScheme(null, null, intSwitchID);
        }

        public clsSwitchScheme(clsScheme Scheme, string strUserID) {            
            getSwitchScheme(Scheme, strUserID, null);
        }

        public clsSwitchScheme(clsScheme Scheme)
        {
            getSwitchScheme(Scheme, null, null);
        }        

        #endregion       

        #region "functions"

        private void getSwitchScheme(clsScheme Scheme, string strUserID, Nullable<int> intSwitchID)
        {            
            string strClientID = "0";
            string strSchemeID = "0";

            if (Scheme != null) 
            {
                strClientID = Scheme.propClient.propClientID.ToString();
                strSchemeID = Scheme.propSchemeID;
            }


            SqlCommand cmd = new SqlCommand();
            SqlDataReader dr;

            con.Open();
            cmd.Connection = con;
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "SWITCHScheme_HeaderGet";

            cmd.Parameters.Add("@param_strSchemeID", System.Data.SqlDbType.NVarChar).Value = strSchemeID;
            cmd.Parameters.Add("@param_strClientID", System.Data.SqlDbType.NVarChar).Value = strClientID;
            cmd.Parameters.Add("@param_intSwitchID", System.Data.SqlDbType.NVarChar).Value = intSwitchID;

            dr = cmd.ExecuteReader();

            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    this.propSwitchID = int.Parse(dr["SwitchID"].ToString());
                    this.propClient = new clsClient(dr["ClientID"].ToString());
                    this.propCreated_By = dr["Created_By"].ToString();
                    this.propDate_Created = DateTime.Parse(dr["Date_Created"].ToString());
                    this.propDescription = dr["Description"].ToString();
                    this.propScheme = Scheme != null ? Scheme : new clsScheme(this.propClient.propClientID, dr["SchemeID"].ToString());
                    this.propStatus = short.Parse(dr["Status"].ToString());
                    this.propStatusString = clsSwitch.getSwitchStringStatus(this.propStatus);
                    this.propSwitchDetails = clsSwitchSchemeDetails.getSwitchDetails(this.propScheme, this, false);                    

                    if (propStatus == (int) clsSwitch.enumSwitchStatus.Declined_Client)
                    {
                        foreach (clsSwitchSchemeDetails Details in this.propSwitchDetails)
                        {
                            clsSwitchSchemeDetails.removeSwitchDetails(Details.propSwitchDetailsID);
                        }
                        this.propSwitchDetails = clsSwitchSchemeDetails.getOriginalDetails(Scheme, this, false);
                    }

                    this.propSwitchDetailsContribution = clsSwitchSchemeDetails.getSwitchDetails(this.propScheme, this, true);
                    if (propStatus == (int)clsSwitch.enumSwitchStatus.Declined_Client)
                    {
                        foreach (clsSwitchSchemeDetails Details in this.propSwitchDetailsContribution)
                        {
                            clsSwitchSchemeDetails.removeSwitchDetails(Details.propSwitchDetailsID);                            
                        }
                        this.propSwitchDetailsContribution = clsSwitchSchemeDetails.getOriginalDetails(Scheme, this, true);
                    }
                }
            }
            else
            {
                this.propSwitchID = 0;
                this.propScheme = new clsScheme(strClientID, strSchemeID);
                this.propClient = new clsClient(strClientID);
                this.propStatus = (short)clsSwitch.enumSwitchStatus.Draft;
                this.propStatusString = clsSwitch.getSwitchStringStatus(this.propStatus);
                this.propCreated_By = strUserID;
                this.propSwitchDetails = clsSwitchSchemeDetails.getOriginalDetails(Scheme, this, false);
                this.propSwitchDetailsContribution = clsSwitchSchemeDetails.getOriginalDetails(Scheme, this, true);
            }

            con.Close();
            cmd.Dispose();
            con.Dispose();
        }

        public static int insertSwitchHeader(clsScheme Scheme, string strUserID, clsSwitch.enumSwitchStatus SwitchStatus, Nullable<int> intSwitchID, string strDescription)
        {
            SqlConnection con = new clsSwitchScheme().con;
            SqlCommand cmd = new SqlCommand();

            con.Open();
            cmd.Connection = con;
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "[SWITCHScheme_HeaderInsert]";

            cmd.Parameters.Add("@param_strSchemeID", System.Data.SqlDbType.NVarChar).Value = Scheme.propSchemeID;
            cmd.Parameters.Add("@param_strClientID", System.Data.SqlDbType.NVarChar).Value = Scheme.propClient.propClientID;
            cmd.Parameters.Add("@param_intStatus", System.Data.SqlDbType.SmallInt).Value = SwitchStatus;
            cmd.Parameters.Add("@param_strCreated_By", System.Data.SqlDbType.NVarChar).Value = strUserID;
            cmd.Parameters.Add("@param_intSwitchID", System.Data.SqlDbType.Int).Value = intSwitchID;
            cmd.Parameters.Add("@param_strDescription", System.Data.SqlDbType.NVarChar).Value = strDescription;

            return int.Parse(cmd.ExecuteScalar().ToString());

        }

        public static void deleteSwitch(int intSwitchID)
        {

            SqlConnection con = new clsSwitchScheme().con;
            SqlCommand cmd = new SqlCommand();

            con.Open();
            cmd.Connection = con;
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "[SWITCHScheme_Delete]";

            cmd.Parameters.Add("@param_SwitchID", System.Data.SqlDbType.Int).Value = intSwitchID;

            cmd.ExecuteNonQuery();

        }

        public static void updateSwitchHeader(int intSwitchID, clsSwitch.enumSwitchStatus SwitchStatus, string strDescription)
        {
            SqlConnection con = new clsSystem_DBConnection(clsSystem_DBConnection.strConnectionString.NavIntegrationDB).propConnection;
            SqlCommand cmd = new SqlCommand();

            con.Open();
            cmd.Connection = con;
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "[SWITCHSCheme_HeaderUpdate]";

            cmd.Parameters.Add("@param_intSwitchID", System.Data.SqlDbType.Int).Value = intSwitchID;
            cmd.Parameters.Add("@param_Status", System.Data.SqlDbType.SmallInt).Value = SwitchStatus;
            cmd.Parameters.Add("@param_Description", System.Data.SqlDbType.NVarChar).Value = strDescription;

            cmd.ExecuteNonQuery();

        }

        public static void updateSwitchHeader(int intSwitchID, clsSwitch.enumSwitchStatus SwitchStatus)
        {

            SqlConnection con = new clsSystem_DBConnection(clsSystem_DBConnection.strConnectionString.NavIntegrationDB).propConnection;
            SqlCommand cmd = new SqlCommand();

            con.Open();
            cmd.Connection = con;
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "[SWITCHSCheme_HeaderUpdate]";

            cmd.Parameters.Add("@param_intSwitchID", System.Data.SqlDbType.Int).Value = intSwitchID;
            cmd.Parameters.Add("@param_Status", System.Data.SqlDbType.SmallInt).Value = SwitchStatus;

            cmd.ExecuteNonQuery();

        }

        #endregion


        public class clsSwitchSchemeDetails {

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

        #region "constructors"

            public clsSwitchSchemeDetails() { }

            #endregion

        #region "functions"

            public static List<clsSwitchSchemeDetails> getOriginalDetails(clsScheme Scheme, clsSwitchScheme SwitchScheme , Boolean isForContribution)
            {
                if (Scheme == null) { return null; }
                string strClientID = Scheme.propClient.propClientID;

                List<clsSwitchSchemeDetails> listSwitchDetails = new List<clsSwitchSchemeDetails>();
                float fTotalAllocation = 0;
                double dPrice = 0;

                foreach (clsScheme.clsDetails SchemeDetails in Scheme.propDetails)
                {
                    clsSwitchSchemeDetails SwitchDetails = new clsSwitchSchemeDetails();
                    SwitchDetails.propFund = new clsFund(SchemeDetails.propFund.propFundID);
                    SwitchDetails.propAllocation = float.Parse(Math.Round(double.Parse(SchemeDetails.propAllocation.ToString()), 2).ToString());

                    if (Scheme.propClient.propCurrency != SwitchDetails.propFund.propCurrency)
                    {
                        double dConvertedValue = clsCurrency.convertToClientCurrency(strClientID, SchemeDetails.propFund.propPrice, SchemeDetails.propFund.propCurrency);
                        int intMarker = dConvertedValue.ToString().IndexOf('.');
                        string strIntegerPart = dConvertedValue.ToString().Substring(0, intMarker);
                        string strDecimalPart = dConvertedValue.ToString().Substring(intMarker, 4);
                        dPrice = Convert.ToDouble(strIntegerPart + strDecimalPart);
                    }
                    else
                    {
                        dPrice = Math.Round(SchemeDetails.propFund.propPrice, 4);
                    }

                    SwitchDetails.propCurrencyMultiplier = clsCurrency.getCurrencyMultiplier(strClientID, SchemeDetails.propFund.propCurrency);
                    SwitchDetails.propTotalValue = float.Parse(Math.Round(double.Parse(Scheme.propCC_TotalValue.ToString()), 0).ToString());

                    fTotalAllocation = fTotalAllocation + SwitchDetails.propAllocation;
                    SwitchDetails.propTotalAllocation = fTotalAllocation;
                    SwitchDetails.propIsDeletable = false;

                    SwitchDetails.propValue = float.Parse(((Math.Round(SwitchDetails.propAllocation, 2) / 100) * Math.Round(Scheme.propCC_TotalValue, 0)).ToString());
                    SwitchDetails.propUnits = Convert.ToDecimal((((Math.Round(SwitchDetails.propAllocation, 2) / 100) * Math.Round(Scheme.propCC_TotalValue, 0)) / dPrice).ToString());

                    SwitchDetails.propSwitchScheme = SwitchScheme;
                    SwitchDetails.propIsContribution = isForContribution;
                    
                    listSwitchDetails.Add(SwitchDetails);
                }

                return listSwitchDetails;
            }

            public static List<clsSwitchSchemeDetails> FundChange(int intFundID_old, int intFundID_new, List<clsSwitchSchemeDetails> listSwitchDetails, string strClientID, string strClientCurrency, Boolean isContribution)
            {
                List<clsSwitchSchemeDetails> newListSwitchDetails = new List<clsSwitchSchemeDetails>();
                float fTotalAllocation = 0;
                float fTotalValue = listSwitchDetails[0].propTotalValue;

                foreach (clsSwitchSchemeDetails SwitchDetail in listSwitchDetails)
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

            public static List<clsSwitchSchemeDetails> addFund(int intNewFundID, List<clsSwitchSchemeDetails> currentlistSwitchDetails, string strClientID, string strClientCurrency, string strUserID, Boolean isContribution)
            {

                float fTotalAllocation = 0;
                float fTotalValue = currentlistSwitchDetails[0].propTotalValue;

                foreach (clsSwitchScheme.clsSwitchSchemeDetails SwitchDetail in currentlistSwitchDetails)
                {
                    fTotalAllocation = fTotalAllocation + SwitchDetail.propAllocation;

                    if (SwitchDetail.propFund.propFundID == intNewFundID)
                    {
                        throw new Exception("Fund already exists.");
                    }
                }

                clsSwitchScheme.clsSwitchSchemeDetails NewSwitchDetail = new clsSwitchScheme.clsSwitchSchemeDetails();

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

            public static void removeSwitchDetails(int intSwitchDetailsID)
            {
                SqlConnection con = new clsSystem_DBConnection(clsSystem_DBConnection.strConnectionString.NavIntegrationDB).propConnection;
                con.Open();

                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;

                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = "[SWITCHScheme_DetailsDelete]";

                cmd.Parameters.Add("@param_intSwitchDetailsID", System.Data.SqlDbType.Int).Value = intSwitchDetailsID;

                cmd.ExecuteNonQuery();

                con.Close();
            }

            public static List<clsSwitchSchemeDetails> removeSwitchDetails(int intFundID, List<clsSwitchSchemeDetails> listSwitchDetails)
            {
                List<clsSwitchSchemeDetails> newListSwitchDetails = new List<clsSwitchSchemeDetails>();

                foreach (clsSwitchSchemeDetails SwitchDetail in listSwitchDetails)
                {
                    if (SwitchDetail.propFund.propFundID != intFundID)
                    {
                        newListSwitchDetails.Add(SwitchDetail);
                    }
                }

                return newListSwitchDetails;
            }

            public static void insertSwitchDetails(List<clsSwitchSchemeDetails> listSwitchDetails, string strUserID, int intSwitchID, Boolean isContribution)
            {
                SqlConnection con = new clsSwitchScheme().con;
                con.Open();

                foreach (clsSwitchSchemeDetails SwitchDetail in listSwitchDetails)
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = con;

                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "[SWITCHScheme_DetailsInsert]";

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

            public static List<clsSwitchSchemeDetails> getSwitchDetails(clsScheme Scheme, clsSwitchScheme SwitchScheme, Boolean isContribution)
            {
                SqlConnection con1 = new clsSystem_DBConnection(clsSystem_DBConnection.strConnectionString.NavIntegrationDB).propConnection;
                List<clsSwitchSchemeDetails> listSwitchDetails = new List<clsSwitchSchemeDetails>();

                SqlCommand cmd = new SqlCommand();
                SqlDataReader dr1;

                con1.Open();
                cmd.Connection = con1;
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                cmd.CommandText = "[SWITCHScheme_DetailsGet]";

                cmd.Parameters.Add("@param_intSwitchID", System.Data.SqlDbType.Int).Value = SwitchScheme.propSwitchID;
                cmd.Parameters.Add("@param_isContribution", System.Data.SqlDbType.Int).Value = isContribution;

                dr1 = cmd.ExecuteReader();

                double dPrice;
                float fTotalAllocation = 0;

                while (dr1.Read())
                {
                    dPrice = 0;

                    clsSwitchSchemeDetails newClsSwitchDetails = new clsSwitchSchemeDetails();

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

            public static void transferClientSwitchToIFA(List<clsSwitchScheme_Client.clsSwitchSchemeDetails_Client> listClientSwitchDetails, string strUserID, Boolean isContribution)
            {
                clsSwitchScheme SwitchIFA = new clsSwitchScheme(listClientSwitchDetails[0].propSwitchScheme.propSwitchID);

                string strClientID = SwitchIFA.propClient.propClientID;
                string strSchemeID = SwitchIFA.propScheme.propSchemeID;                
                
                List<clsSwitchScheme.clsSwitchSchemeDetails> listSwitchDetailsIFA = new clsSwitchScheme(new clsScheme(strClientID, strSchemeID)).propSwitchDetails;
                List<clsSwitchScheme.clsSwitchSchemeDetails> newListSwitchDetails = new List<clsSwitchScheme.clsSwitchSchemeDetails>();

                foreach (clsSwitchScheme_Client.clsSwitchSchemeDetails_Client SwitchDetails_Client in listClientSwitchDetails)
                {
                    clsSwitchScheme.clsSwitchSchemeDetails newSwitchDetails = new clsSwitchScheme.clsSwitchSchemeDetails();
                    newSwitchDetails.propSwitchScheme = new clsSwitchScheme(SwitchIFA.propSwitchID);
                    newSwitchDetails.propFund = new clsFund(SwitchDetails_Client.propFund.propFundID);
                    newSwitchDetails.propAllocation = SwitchDetails_Client.propAllocation;
                    newSwitchDetails.propIsDeletable = SwitchDetails_Client.propIsDeletable;
                    newSwitchDetails.propIsContribution = isContribution;

                    newListSwitchDetails.Add(newSwitchDetails);

                }

                clsSwitchScheme.clsSwitchSchemeDetails.insertSwitchDetails(newListSwitchDetails, strUserID, SwitchIFA.propSwitchID, isContribution);
            }

            public static void deleteAllDetails(Nullable<int> intSwitchID)
            {
                if (intSwitchID == null) { return; }

                SqlConnection con = new clsSwitchScheme().con;
                con.Open();

                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;

                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = "[SWITCHScheme_DetailsDeleteAll]";

                cmd.Parameters.Add("@param_SwitchID", System.Data.SqlDbType.Int).Value = intSwitchID;
                cmd.ExecuteNonQuery();
                
            }

            #endregion

        }

    }
}
