using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;

namespace NAV
{
    public class clsClient
    {
        SqlConnection con = new clsSystem_DBConnection(clsSystem_DBConnection.strConnectionString.NavIntegrationDB).propConnection;

        #region properties

        private String strClientID;
        public String propClientID { get { return strClientID; } set { strClientID = value; } }

        private String strSurname;
        public String propSurname { get { return strSurname; } set { strSurname = value; } }

        private String strForename;
        public String propForename { get { return strForename; } set { strForename = value; } }

        private String strSalesperson;
        public String propSalesperson { get { return strSalesperson; } set { strSalesperson = value; } }

        private String strServicingOffice;
        public String propServicingOffice { get { return strServicingOffice; } set { strServicingOffice = value; } }

        private String strCountry;
        public String propCountry { get { return strCountry; } set { strCountry = value; } }

        private String strValuationFrequency;
        public String propValuationFrequency { get { return strValuationFrequency; } set { strValuationFrequency = value; } }

        private String strClientNumber;
        public String propClientNumber { get { return strClientNumber; } set { strClientNumber = value; } }

        private String strCategory;
        public String propCategory { get { return strCategory; } set { strCategory = value; } }

        private String strCurrency;
        public String propCurrency { get { return strCurrency; } set { strCurrency = value; } }

        private Boolean boolHTML;
        public Boolean propHTML { get { return boolHTML; } set { boolHTML = value; } }

        private String strCode;
        public String propCode { get { return strCode; } set { strCode = value; } }

        private int intIFA_ID;
        public int propIFA_ID { get { return intIFA_ID; } set { intIFA_ID = value; } }

        private Boolean boolOLDeleted;
        public Boolean propOLDeleted { get { return boolOLDeleted; } set { boolOLDeleted = value; } }

        private String strManagerID;
        public String propManagerID { get { return strManagerID; } set { strManagerID = value; } }

        private String strRegionID;
        public String propRegionID { get { return strRegionID; } set { strRegionID = value; } }

        private String strAgentID;
        public String propAgentID { get { return strAgentID; } set { strAgentID = value; } }

        private String strCreatedBy;
        public String propCreatedBy { get { return strCreatedBy; } set { strCreatedBy = value; } }

        private String strAdministratorID;
        public String propAdministratorID { get { return strAdministratorID; } set { strAdministratorID = value; } }

        private String strCoordinatorID;
        public String propCoordinatorID { get { return strCoordinatorID; } set { strCoordinatorID = value; } }

        private int intLanguage;
        public int propLanguage { get { return intLanguage; } set { intLanguage = value; } }

        private DateTime dtIFAUpdatedDate;
        public DateTime propIFAUpdatedDate { get { return dtIFAUpdatedDate; } set { dtIFAUpdatedDate = value; } }

        private String strIFAUpdatedBy;
        public String propIFAUpdatedBy { get { return strIFAUpdatedBy; } set { strIFAUpdatedBy = value; } }

        private String strEmailWork;
        public String propEmailWork {get { return strEmailWork; } set { strEmailWork = value; }}

        private String strEmailPersonal;
        public String propEmailPersonal { get { return strEmailPersonal; } set { strEmailPersonal = value; } }

        #endregion

        public clsClient(string strClientID) { getClient(strClientID); }

        private void getClient(string strClientID)
        {
            SqlCommand cmd = new SqlCommand();
            SqlDataReader dr;

            con.Open();
            cmd.Connection = con;
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "SWITCH_ClientGet";

            cmd.Parameters.Add("@param_ClientID", System.Data.SqlDbType.NVarChar).Value = strClientID;
            dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                this.strClientID = dr["ClientID"].ToString();
                this.strSurname = dr["Surname"].ToString();
                this.strForename = dr["Forenames"].ToString();
                this.strSalesperson = dr["Salesperson"].ToString();
                this.strServicingOffice = dr["ServicingOffice"].ToString();
                this.strCountry = dr["Country"].ToString();
                this.strValuationFrequency = dr["ValuationFrequency"].ToString();
                this.strClientNumber = dr["ClientNumber"].ToString();
                this.strCategory = dr["Category"].ToString();
                this.strCurrency = dr["Currency"].ToString();
                this.boolHTML = dr["HTML"] != System.DBNull.Value ? bool.Parse(dr["HTML"].ToString()) : false;
                this.strCode = dr["Code"].ToString();
                this.intIFA_ID = dr["IFA_ID"] != System.DBNull.Value ? int.Parse(dr["IFA_ID"].ToString()) : 0;
                this.boolOLDeleted = dr["OLDeleted"] != System.DBNull.Value ? bool.Parse(dr["OLDeleted"].ToString()) : false;
                this.strManagerID = dr["ManagerID"].ToString();
                this.strRegionID = dr["RegionID"].ToString();
                this.strAgentID = dr["AgentID"].ToString();
                this.strCreatedBy = dr["CreatedBy"].ToString();
                this.strAdministratorID = dr["AdministratorID"].ToString();
                this.strCoordinatorID = dr["CoordinatorID"].ToString();
                this.intLanguage = dr["Language"] != System.DBNull.Value ? int.Parse(dr["Language"].ToString()) : 0;
                this.dtIFAUpdatedDate = dr["IFAUpdatedDate"] != System.DBNull.Value ? DateTime.Parse(dr["IFAUpdatedDate"].ToString()) : DateTime.ParseExact("01/01/1800", "dd/MM/yyyy", null);
                this.strIFAUpdatedBy = dr["IFAUpdatedBy"].ToString();
                this.propEmailPersonal = dr["PersonalEmail"].ToString();
                this.propEmailWork = dr["WorkEmail"].ToString();
            }
            con.Close();
            cmd.Dispose();
            con.Dispose();
        }
    }
}
