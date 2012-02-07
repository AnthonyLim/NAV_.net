using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;

namespace NAV
{
    public class clsFund
    {
        private SqlConnection con = new clsSystem_DBConnection(clsSystem_DBConnection.strConnectionString.NavIntegrationDB).propConnection;

        #region properties

	    private int intFundID;
        public int propFundID { get {return intFundID;} set {intFundID = value;} }

	    private string strFundName;
        public string propFundName { get {return strFundName;} set {strFundName = value;}}

	    private int intFundManager;
        public int propFundManager {get {return intFundManager;} set {intFundManager = value;}}

	    private string strCurrency;
        public string propCurrency {get {return strCurrency;} set {strCurrency = value;}}

	    private float fPrice;
        public float propPrice { get {return fPrice;} set {fPrice = value;}}

	    private DateTime dtDatePriceUpdated;
        public DateTime propDatePriceUpdated { get {return dtDatePriceUpdated;}set {dtDatePriceUpdated = value;}}

        private int intCompanyID;
        public int propCompanyID {get { return intCompanyID; } set { intCompanyID = value; }}

        private string strSEDOL;
        public string propSEDOL{get { return strSEDOL; } set { strSEDOL = value; }}
         
        #endregion

        public clsFund() { }

        public clsFund(int intFundID) {
            getFundInfo(intFundID);
        }

        private void getFundInfo(int intFundID){
            SqlCommand cmd = new SqlCommand();
            SqlDataReader dr;
            con.Open();
            cmd.Connection = con;
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "[SWITCH_FundGet]";

            cmd.Parameters.Add("@param_intFundNameID", System.Data.SqlDbType.Int).Value = intFundID;
                                   
            dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                this.propCompanyID = dr["CompanyID"] == null ? int.Parse(dr["CompanyID"].ToString()) : 0;
                this.propCurrency = dr["Currency"].ToString();
                this.propDatePriceUpdated = dr["DatePriceUpdated"] != System.DBNull.Value ? DateTime.Parse(dr["DatePriceUpdated"].ToString()) : DateTime.ParseExact("01/01/1800", "dd/MM/yyyy", null);
                this.propFundID = intFundID;
                this.propFundManager = int.Parse(dr["FundManager"].ToString()) ;
                this.propFundName = dr["FundName"].ToString();
                this.propPrice = float.Parse(dr["Price"].ToString());
                this.propSEDOL = dr["SEDOL"].ToString();

            }

            dr.Close();
            con.Close();
            cmd.Dispose();
            con.Dispose();

        }        

        public static List<clsFund> getAllFunds(String strFundName){

            SqlConnection con = new clsSystem_DBConnection(clsSystem_DBConnection.strConnectionString.NavIntegrationDB).propConnection;
            List<clsFund> listFund = new List<clsFund>();

            SqlCommand cmd = new SqlCommand();
            SqlDataReader dr;
            
            con.Open();
            cmd.Connection = con;
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "[SWITCH_FundGetAll]";

            cmd.Parameters.Add("@param_strFundName", System.Data.SqlDbType.NVarChar).Value = strFundName;

            dr = cmd.ExecuteReader();

            while (dr.Read())
            {

                clsFund newFund = new clsFund();

                newFund.propCompanyID = dr["CompanyID"] == null ? int.Parse(dr["CompanyID"].ToString()) : 0;
                newFund.propCurrency = dr["Currency"].ToString();
                newFund.propDatePriceUpdated = dr["DatePriceUpdated"] != System.DBNull.Value ? DateTime.Parse(dr["DatePriceUpdated"].ToString()) : DateTime.ParseExact("01/01/1800", "dd/MM/yyyy", null);
                newFund.propFundID = int.Parse(dr["FundNameID"].ToString());
                newFund.propFundManager = int.Parse(dr["FundManager"].ToString());
                newFund.propFundName = dr["FundName"].ToString();
                newFund.propPrice = dr["Price"] != System.DBNull.Value ? float.Parse(dr["Price"].ToString()) : 0f;

                listFund.Add(newFund);

            }
            dr.Close();
            con.Close();
            cmd.Dispose();
            con.Dispose();

            return listFund;
        }

    }
}
