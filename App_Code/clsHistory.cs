using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;

namespace NAV
{
    public class clsHistory
    {
        
        # region "property"

        private int intHistoryID;
        public int propHistoryID {get { return intHistoryID;} set { intHistoryID = value;}}

        private string strPortfolioID;
        public string propPortfolioID {get {return strPortfolioID; } set {strPortfolioID = value;}}

        private int intSwitchID;
        public int propSwitchID {get { return intSwitchID;} set { intSwitchID = value;}}

		private DateTime dtAction_Date;
        public DateTime propAction_Date {get { return dtAction_Date;} set {dtAction_Date = value;}}
			
        private Int16 intStatus;
        public Int16 propStatus {get {return intStatus;}set {intStatus = value;}}

        # endregion

        SqlConnection con = new clsSystem_DBConnection(clsSystem_DBConnection.strConnectionString.NavIntegrationDB).propConnection;

        public clsHistory() { }       

        public static int insertHeader(string strPortfolioID, int intSwitchID, Int16 sintStatus)
        {
            SqlConnection con = new clsHistory().con;
            SqlCommand cmd = new SqlCommand();

            con.Open();
            cmd.Connection = con;
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "[SWITCHHistory_HeaderInsert]";

            cmd.Parameters.Add("@param_PortfolioID", System.Data.SqlDbType.NVarChar).Value = strPortfolioID;
            cmd.Parameters.Add("@param_SwitchID", System.Data.SqlDbType.Int).Value = intSwitchID;
            cmd.Parameters.Add("@param_Status", System.Data.SqlDbType.Int).Value = sintStatus;

            return int.Parse(cmd.ExecuteScalar().ToString());
        }

        public static void insertDetailsIFA(int intHistoryID, List<clsSwitchDetails> SwitchDetails_IFA)
        {
            SqlConnection con = new clsHistory().con;
            con.Open();

            foreach (clsSwitchDetails SwitchDetail in SwitchDetails_IFA)
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;

                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = "[SWITCHHistory_DetailsInsert]";
               
                cmd.Parameters.Add("@param_HistoryID", System.Data.SqlDbType.Int).Value = intHistoryID;
                cmd.Parameters.Add("@param_SwitchDetailsID", System.Data.SqlDbType.Int).Value = SwitchDetail.propSwitchDetailsID;
                cmd.Parameters.Add("@param_FundID", System.Data.SqlDbType.Float).Value = SwitchDetail.propFund.propFundID;
                cmd.Parameters.Add("@param_Allocation", System.Data.SqlDbType.Float).Value = SwitchDetail.propAllocation;
                cmd.Parameters.Add("@param_Created_By", System.Data.SqlDbType.NVarChar).Value = SwitchDetail.propCreated_By;

                cmd.ExecuteNonQuery();
            }
        }

        public static void insertDetailsClient(int intHistoryID, List<clsSwitchDetails_Client> SwitchDetails_Client)
        {
            SqlConnection con = new clsHistory().con;
            con.Open();

            foreach (clsSwitchDetails_Client SwitchDetail in SwitchDetails_Client)
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;

                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = "[SWITCHHistory_DetailsInsert]";

                cmd.Parameters.Add("@param_HistoryID", System.Data.SqlDbType.Int).Value = intHistoryID;
                cmd.Parameters.Add("@param_SwitchDetailsID", System.Data.SqlDbType.Int).Value = SwitchDetail.propSwitchDetailsID;
                cmd.Parameters.Add("@param_FundID", System.Data.SqlDbType.Float).Value = SwitchDetail.propFund.propFundID;
                cmd.Parameters.Add("@param_Allocation", System.Data.SqlDbType.Float).Value = SwitchDetail.propAllocation;
                cmd.Parameters.Add("@param_Created_By", System.Data.SqlDbType.NVarChar).Value = SwitchDetail.propCreated_By;

                cmd.ExecuteNonQuery();
            }
        }

        public static void insertMessage(int intHistoryID, String strMessage)
        {
            SqlConnection con = new clsHistory().con;
            con.Open();

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;

            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "[SWITCHHistory_MessageInsert]";

            cmd.Parameters.Add("@param_HistoryID", System.Data.SqlDbType.Int).Value = intHistoryID;
            cmd.Parameters.Add("@param_Message", System.Data.SqlDbType.NVarChar).Value = strMessage;

            cmd.ExecuteNonQuery();

        }

        public static List<clsHistory> getListHistory(string strPortfolioID, int intSwitchID) {
            
            List<clsHistory> ListHistory = new List<clsHistory>();            
            SqlConnection con = new clsHistory().con;
                
            SqlCommand cmd = new SqlCommand();                
            SqlDataReader dr;

                con.Open();
                cmd.Connection = con;
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = "SWITCHHistory_HeaderGet";

                cmd.Parameters.Add("@param_strPortfolioID", System.Data.SqlDbType.NVarChar).Value = strPortfolioID;
                cmd.Parameters.Add("@param_intSwitchID", System.Data.SqlDbType.Int).Value = intSwitchID;

                dr = cmd.ExecuteReader();

                
            while (dr.Read())                
            {
                clsHistory History = new clsHistory();

                History.propAction_Date = DateTime.Parse(dr["Action_Date"].ToString());
                History.propHistoryID = int.Parse(dr["HistoryID"].ToString());
                History.propPortfolioID = dr["PortfolioID"].ToString();
                History.propStatus = Int16.Parse(dr["Status"].ToString());
                History.propSwitchID = int.Parse(dr["SwitchID"].ToString());

                ListHistory.Add(History);
            }

                con.Close();
                cmd.Dispose();

                return ListHistory;
        }

        public static List<clsSwitchDetails> getSwitchDetailsIFA(clsPortfolio Portfolio, int intSwitchID, int intHistoryID)
        {
            List<clsSwitchDetails> listSwitchDetails = new List<clsSwitchDetails>();

            SqlConnection con = new clsHistory().con;
            SqlCommand cmd = new SqlCommand();
            SqlDataReader dr1;

            con.Open();
            cmd.Connection = con;
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "[SWITCHHistory_DetailsGet]";

            cmd.Parameters.Add("@param_intHistoryID", System.Data.SqlDbType.Int).Value = intHistoryID;

            dr1 = cmd.ExecuteReader();

            float fTotalAllocation = 0;

            while (dr1.Read())
            {

                clsSwitchDetails newClsSwitchDetails = new clsSwitchDetails();

                newClsSwitchDetails.propAllocation = float.Parse(Math.Round(double.Parse(dr1["Allocation"].ToString()), 2).ToString());
                newClsSwitchDetails.propCreated_By = dr1["Created_By"].ToString();
                newClsSwitchDetails.propDate_Created = DateTime.Parse(dr1["Date_Created"].ToString());
                //newClsSwitchDetails.propDate_LastUpdate = DateTime.Parse(dr1["Date_LastUpdate"].ToString());
                newClsSwitchDetails.propFund = new clsFund(int.Parse(dr1["FundID"].ToString()));
                newClsSwitchDetails.propFundID = int.Parse(dr1["FundID"].ToString());
                newClsSwitchDetails.propSwitchDetailsID = int.Parse(dr1["SwitchDetailsID"].ToString());
                //newClsSwitchDetails.propSwitchID = int.Parse(dr1["SwitchID"].ToString());
                //newClsSwitchDetails.propUpdated_By = dr1["Updated_By"].ToString();
                //newClsSwitchDetails.propIsDeletable = dr1["isDeletable"].ToString().Equals("1") ? true : false;

                if (Portfolio.propPortfolioDetails[0].propClientCurrency != newClsSwitchDetails.propFund.propCurrency)
                {
                    newClsSwitchDetails.propUnits = clsSwitchDetails.computeUnits(newClsSwitchDetails.propAllocation,
                                                                                  float.Parse(Math.Round(double.Parse(Portfolio.propPortfolioDetails[0].propTotalCurrentValueClient.ToString()), 0).ToString()),
                                                                                  clsCurrency.convertToClientCurrency(new clsSwitch(intSwitchID).propClientID, newClsSwitchDetails.propFund.propPrice, newClsSwitchDetails.propFund.propCurrency));
                }
                else
                {
                    newClsSwitchDetails.propUnits = clsSwitchDetails.computeUnits(newClsSwitchDetails.propAllocation,
                                                                                  float.Parse(Math.Round(double.Parse(Portfolio.propPortfolioDetails[0].propTotalCurrentValueClient.ToString()), 0).ToString()),
                                                                                  newClsSwitchDetails.propFund.propPrice);
                }


                newClsSwitchDetails.propCurrencyMultiplier = clsCurrency.getCurrencyMultiplier(new clsSwitch(intSwitchID).propClientID, newClsSwitchDetails.propFund.propCurrency);
                //newClsSwitchDetails.propValue = clsSwitchDetails.computeValue(newClsSwitchDetails.propAllocation, Portfolio.propPortfolioDetails[0].propTotalCurrentValueClient);
                newClsSwitchDetails.propTotalValue = float.Parse(Math.Round(double.Parse(Portfolio.propPortfolioDetails[0].propTotalCurrentValueClient.ToString()), 0).ToString());
                newClsSwitchDetails.propValue = clsSwitchDetails.computeValue(newClsSwitchDetails.propAllocation, newClsSwitchDetails.propTotalValue);


                fTotalAllocation = fTotalAllocation + newClsSwitchDetails.propAllocation;
                newClsSwitchDetails.propTotalAllocation = fTotalAllocation;

                listSwitchDetails.Add(newClsSwitchDetails);

            }
            con.Close();
            cmd.Dispose();
            con.Dispose();

            return listSwitchDetails;
        }

        public static String getMessage(int intHistoryID)
        {
            String strMessage = "";

            SqlConnection con = new clsHistory().con;
           
            SqlCommand cmd = new SqlCommand();
            SqlDataReader dr;

            con.Open();
            cmd.Connection = con;
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "SWITCHHistory_MessageGet";

            cmd.Parameters.Add("@param_intHistoryID", System.Data.SqlDbType.Int).Value = intHistoryID;

            dr = cmd.ExecuteReader();

            while (dr.Read()) {
                strMessage = dr["Message"] != System.DBNull.Value ? dr["Message"].ToString() : "";     
            }

            con.Close();
            cmd.Dispose();
            con.Dispose();

            return strMessage;
        }


        public class clsHistoryScheme 
        { 

        # region "property"

        private int intHistoryID;
        public int propHistoryID {get { return intHistoryID;} set { intHistoryID = value;}}

        private string strSchemeID;
        public string propSchemeID {get {return strSchemeID; } set {strSchemeID = value;}}

        private int intSwitchID;
        public int propSwitchID {get { return intSwitchID;} set { intSwitchID = value;}}

		private DateTime dtAction_Date;
        public DateTime propAction_Date {get { return dtAction_Date;} set {dtAction_Date = value;}}
			
        private Int16 intStatus;
        public Int16 propStatus {get {return intStatus;}set {intStatus = value;}}

        # endregion

        SqlConnection con = new clsSystem_DBConnection(clsSystem_DBConnection.strConnectionString.NavIntegrationDB).propConnection;

        public clsHistoryScheme() { }       

        public static int insertHeader(string strSchemeID, int intSwitchID, Int16 sintStatus)
        {
            SqlConnection con = new clsHistory().con;
            SqlCommand cmd = new SqlCommand();

            con.Open();
            cmd.Connection = con;
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "[SWITCHSchemeHistory_HeaderInsert]";

            cmd.Parameters.Add("@param_SchemeID", System.Data.SqlDbType.NVarChar).Value = strSchemeID;
            cmd.Parameters.Add("@param_SwitchID", System.Data.SqlDbType.Int).Value = intSwitchID;
            cmd.Parameters.Add("@param_Status", System.Data.SqlDbType.Int).Value = sintStatus;

            return int.Parse(cmd.ExecuteScalar().ToString());
        }

        public static void insertDetailsIFA(int intHistoryID, List<clsSwitchScheme.clsSwitchSchemeDetails> SwitchDetails_IFA, Boolean isContribution)
        {
            if (SwitchDetails_IFA == null) { return ;}

            SqlConnection con = new clsHistory().con;
            con.Open();

            foreach (clsSwitchScheme.clsSwitchSchemeDetails SwitchDetail in SwitchDetails_IFA)
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;

                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = "[SWITCHSchemeHistory_DetailsInsert]";

                cmd.Parameters.Add("@param_HistoryID", System.Data.SqlDbType.Int).Value = intHistoryID;
                cmd.Parameters.Add("@param_SwitchDetailsID", System.Data.SqlDbType.Int).Value = SwitchDetail.propSwitchDetailsID;
                cmd.Parameters.Add("@param_FundID", System.Data.SqlDbType.Float).Value = SwitchDetail.propFund.propFundID;
                cmd.Parameters.Add("@param_Allocation", System.Data.SqlDbType.Float).Value = SwitchDetail.propAllocation;
                cmd.Parameters.Add("@param_Created_By", System.Data.SqlDbType.NVarChar).Value = SwitchDetail.propCreated_By;
                cmd.Parameters.Add("@param_isContribution", System.Data.SqlDbType.SmallInt).Value = isContribution;

                cmd.ExecuteNonQuery();
            }
        }

        public static void insertDetailsClient(int intHistoryID, List<clsSwitchScheme_Client.clsSwitchSchemeDetails_Client> SwitchDetails_Client, Boolean isContribution)
        {
            SqlConnection con = new clsHistory().con;
            con.Open();

            foreach (clsSwitchScheme_Client.clsSwitchSchemeDetails_Client SwitchDetail in SwitchDetails_Client)
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;

                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = "[SWITCHSchemeHistory_DetailsInsert]";

                cmd.Parameters.Add("@param_HistoryID", System.Data.SqlDbType.Int).Value = intHistoryID;
                cmd.Parameters.Add("@param_SwitchDetailsID", System.Data.SqlDbType.Int).Value = SwitchDetail.propSwitchDetailsID;
                cmd.Parameters.Add("@param_FundID", System.Data.SqlDbType.Float).Value = SwitchDetail.propFund.propFundID;
                cmd.Parameters.Add("@param_Allocation", System.Data.SqlDbType.Float).Value = SwitchDetail.propAllocation;
                cmd.Parameters.Add("@param_Created_By", System.Data.SqlDbType.NVarChar).Value = SwitchDetail.propCreated_By;
                cmd.Parameters.Add("@param_isContribution", System.Data.SqlDbType.SmallInt).Value = isContribution;

                cmd.ExecuteNonQuery();
            }
        }

        public static void insertMessage(int intHistoryID, String strMessage)
        {
            SqlConnection con = new clsHistory().con;
            con.Open();

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;

            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "[SWITCHSchemeHistory_MessageInsert]";

            cmd.Parameters.Add("@param_HistoryID", System.Data.SqlDbType.Int).Value = intHistoryID;
            cmd.Parameters.Add("@param_Message", System.Data.SqlDbType.NVarChar).Value = strMessage;

            cmd.ExecuteNonQuery();

        }

        public static List<clsHistory.clsHistoryScheme> getListHistory(string strSchemeID, int intSwitchID)
        {

            List<clsHistory.clsHistoryScheme> ListHistory = new List<clsHistory.clsHistoryScheme>();
            SqlConnection con = new clsHistory().con;

            SqlCommand cmd = new SqlCommand();
            SqlDataReader dr;

            con.Open();
            cmd.Connection = con;
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "SWITCHSchemeHistory_HeaderGet";

            cmd.Parameters.Add("@param_strSchemeID", System.Data.SqlDbType.NVarChar).Value = strSchemeID;
            cmd.Parameters.Add("@param_intSwitchID", System.Data.SqlDbType.Int).Value = intSwitchID;

            dr = cmd.ExecuteReader();


            while (dr.Read())
            {
                clsHistory.clsHistoryScheme History = new clsHistory.clsHistoryScheme();

                History.propAction_Date = DateTime.Parse(dr["Action_Date"].ToString());
                History.propHistoryID = int.Parse(dr["HistoryID"].ToString());
                History.propSchemeID = dr["SchemeID"].ToString();
                History.propStatus = Int16.Parse(dr["Status"].ToString());
                History.propSwitchID = int.Parse(dr["SwitchID"].ToString());

                ListHistory.Add(History);
            }

            con.Close();
            cmd.Dispose();

            return ListHistory;
        }

        public static List<clsSwitchScheme.clsSwitchSchemeDetails> getSwitchDetails(clsScheme Scheme, int intSwitchID, int intHistoryID, Boolean isContribution)
        {
            List<clsSwitchScheme.clsSwitchSchemeDetails> listSwitchDetails = new List<clsSwitchScheme.clsSwitchSchemeDetails>();

            SqlConnection con = new clsHistory().con;
            SqlCommand cmd = new SqlCommand();
            SqlDataReader dr1;

            con.Open();
            cmd.Connection = con;
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "[SWITCHSchemeHistory_DetailsGet]";

            cmd.Parameters.Add("@param_intHistoryID", System.Data.SqlDbType.Int).Value = intHistoryID;
            cmd.Parameters.Add("@param_isContribution", System.Data.SqlDbType.SmallInt).Value = isContribution;            

            dr1 = cmd.ExecuteReader();

            float fTotalAllocation = 0;

            while (dr1.Read())
            {

                clsSwitchScheme.clsSwitchSchemeDetails newClsSwitchDetails = new clsSwitchScheme.clsSwitchSchemeDetails();

                newClsSwitchDetails.propAllocation = float.Parse(Math.Round(double.Parse(dr1["Allocation"].ToString()), 2).ToString());
                newClsSwitchDetails.propCreated_By = dr1["Created_By"].ToString();
                newClsSwitchDetails.propDate_Created = DateTime.Parse(dr1["Date_Created"].ToString());
                //newClsSwitchDetails.propDate_LastUpdate = DateTime.Parse(dr1["Date_LastUpdate"].ToString());
                newClsSwitchDetails.propFund = new clsFund(int.Parse(dr1["FundID"].ToString()));
                newClsSwitchDetails.propSwitchDetailsID = int.Parse(dr1["SwitchDetailsID"].ToString());
                //newClsSwitchDetails.propSwitchID = int.Parse(dr1["SwitchID"].ToString());
                //newClsSwitchDetails.propUpdated_By = dr1["Updated_By"].ToString();
                //newClsSwitchDetails.propIsDeletable = dr1["isDeletable"].ToString().Equals("1") ? true : false;

                if (Scheme.propClient.propCurrency != newClsSwitchDetails.propFund.propCurrency)
                {
                    newClsSwitchDetails.propUnits = clsSwitchDetails.computeUnits(newClsSwitchDetails.propAllocation,
                                                                                  float.Parse(Math.Round(double.Parse(Scheme.propCC_TotalValue.ToString()), 0).ToString()),
                                                                                  clsCurrency.convertToClientCurrency(new clsSwitchScheme(intSwitchID).propClient.propClientID, newClsSwitchDetails.propFund.propPrice, newClsSwitchDetails.propFund.propCurrency));
                }
                else
                {
                    newClsSwitchDetails.propUnits = clsSwitchDetails.computeUnits(newClsSwitchDetails.propAllocation,
                                                                                  float.Parse(Math.Round(double.Parse(Scheme.propCC_TotalValue.ToString()), 0).ToString()),
                                                                                  newClsSwitchDetails.propFund.propPrice);
                }


                newClsSwitchDetails.propCurrencyMultiplier = clsCurrency.getCurrencyMultiplier(new clsSwitchScheme(intSwitchID).propClient.propClientID, newClsSwitchDetails.propFund.propCurrency);                
                newClsSwitchDetails.propTotalValue = float.Parse(Math.Round(double.Parse(Scheme.propCC_TotalValue.ToString()), 0).ToString());
                newClsSwitchDetails.propValue = clsSwitchDetails.computeValue(newClsSwitchDetails.propAllocation, newClsSwitchDetails.propTotalValue);


                fTotalAllocation = fTotalAllocation + newClsSwitchDetails.propAllocation;
                newClsSwitchDetails.propTotalAllocation = fTotalAllocation;

                listSwitchDetails.Add(newClsSwitchDetails);

            }
            con.Close();
            cmd.Dispose();
            con.Dispose();

            return listSwitchDetails;
        }

        public static String getMessage(int intHistoryID)
        {
            String strMessage = "";

            SqlConnection con = new clsHistory().con;

            SqlCommand cmd = new SqlCommand();
            SqlDataReader dr;

            con.Open();
            cmd.Connection = con;
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "SWITCHSchemeHistory_MessageGet";

            cmd.Parameters.Add("@param_intHistoryID", System.Data.SqlDbType.Int).Value = intHistoryID;

            dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                strMessage = dr["Message"] != System.DBNull.Value ? dr["Message"].ToString() : "";
            }

            con.Close();
            cmd.Dispose();
            con.Dispose();

            return strMessage;
        }

        }

    }
}

