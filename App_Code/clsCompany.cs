using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;

namespace NAV
{
    public class clsCompany
    {
        private SqlConnection con = new clsSystem_DBConnection(clsSystem_DBConnection.strConnectionString.NavIntegrationDB).propConnection;

        #region properties

        private int intCompanyID;
        public int propCompanyID { get { return intCompanyID; } set { intCompanyID = value; } }

        private String strCompany;
        public String propCompany { get { return strCompany; } set { strCompany = value; } }

        private String strCompanyAdd1;
        public String propCompanyAdd1 { get { return strCompanyAdd1; } set { strCompanyAdd1 = value; } }

        private String strCompanyAdd2;
        public String propCompanyAdd2 { get { return strCompanyAdd2; } set { strCompanyAdd2 = value; } }

        private String strCompanyAdd3;
        public String propCompanyAdd3 { get { return strCompanyAdd3; } set { strCompanyAdd3 = value; } }

        private String strCompanyCountry;
        public String propCompanyCountry { get { return strCompanyCountry; } set { strCompanyCountry = value; } }

        private String strCompanyTel;
        private String propCompanyTel { get { return strCompanyTel; } set { strCompanyTel = value; } }

        private String strCompanyFax;
        public String propCompanyFax { get { return strCompanyFax; } set { strCompanyFax = value; } }

        private String strCompanyEmail;
        public String propCompanyEmail { get { return strCompanyEmail; } set { strCompanyEmail = value; } }

        private String strCompanyWebSite;
        public String propCompanyWebSite { get { return strCompanyWebSite; } set { strCompanyWebSite = value; } }

        private String strCompanyType;
        public String propCompanyType { get { return strCompanyType; } set { strCompanyType = value; } }

        private int intFeedListID;
        public int propFeedListID { get { return intFeedListID; } set { intFeedListID = value; } }

        public bool setConfirmationStatus
        {
            set
            {
                con = new clsSystem_DBConnection(clsSystem_DBConnection.strConnectionString.NavIntegrationDB).propConnection;
                if (con.State == System.Data.ConnectionState.Open) { con.Close(); } con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = "SWITCH_SignedConfirmationSet";
                cmd.Parameters.Add("@CompanyID", System.Data.SqlDbType.Int).Value = this.propCompanyID;
                cmd.Parameters.Add("@status", System.Data.SqlDbType.Bit).Value = value;
                cmd.ExecuteNonQuery();                
            }
        }


        #endregion

        public clsCompany() { }

        public clsCompany(int intCompanyID) {
            getCompany(intCompanyID);
        }

        private void getCompany(int intCompanyID)
        {
            SqlCommand cmd = new SqlCommand();
            SqlDataReader dr;

            con.Open();
            cmd.Connection = con;
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "SWITCH_CompanyGet";

            cmd.Parameters.Add("@param_intCompanyID", System.Data.SqlDbType.NVarChar).Value = intCompanyID;
            dr = cmd.ExecuteReader();

            while (dr.Read())
            {          
                this.propCompanyID = int.Parse(dr["CompanyID"].ToString());
                this.propCompany = dr["Company"].ToString();
                this.propCompanyAdd1 = dr["CompanyAdd1"].ToString();
                this.propCompanyAdd2 = dr["CompanyAdd2"].ToString();
                this.propCompanyAdd3 = dr["CompanyAdd3"].ToString();
                this.propCompanyCountry = dr["CompanyCountry"].ToString();
                this.propCompanyTel = dr["CompanyTel"].ToString();
                this.propCompanyFax = dr["CompanyFax"].ToString();
                this.propCompanyEmail = dr["CompanyEmail"].ToString();
                this.propCompanyWebSite = dr["CompanyWebSite"].ToString();
                this.propCompanyType = dr["CompanyType"].ToString();
                this.propFeedListID = dr["FeedListID"] != System.DBNull.Value ? int.Parse(dr["FeedListID"].ToString()) : 0;
            }
            con.Close();
            cmd.Dispose();
            con.Dispose();
        }              

    }
}
