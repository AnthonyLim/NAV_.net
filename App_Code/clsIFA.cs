using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;

namespace NAV
{
    public class clsIFA
    {
        private SqlConnection con = new clsSystem_DBConnection(clsSystem_DBConnection.strConnectionString.NavIntegrationDB).propConnection;

        #region "properties"

        //  [IFA_ID]
        //,[IFA_Code]
        //,[IFA_Name]
        //,[IFA_Currency]
        //,[IFAUpdatedDate]
        //,[IFAAdd1]
        //,[IFAAdd2]
        //,[IFAAdd3]
        //,[IFACountry]
        //,[IFAPrincipal]
        //,[IFATel]
        //,[IFAFax]
        //,[IFAEmail]
        //,[IFAWebsiteContact]
        //,[IFATechContact]
        //,[IFADataEntryEmail]
        //,[IFAShowOnWeb]
        //,[SuperIFAID]
        //,[IncludeInBilling]
        //,[IFAODEEmail]
        //,[IFAPaymentReceived]
        //,[ContributionsOnWeb]
        //,[ClientNamesOnWeb]
        //,[ClientDetailsOnWeb]

        private int intIFA_ID;
        public int propIFA_ID { get { return intIFA_ID; } set { intIFA_ID = value; } }

        private string strIFA_Name;
        public string propIFA_Name { get { return strIFA_Name; } set { strIFA_Name = value; } }

        private string strIFAEmail;
        public string propIFAEmail { get { return strIFAEmail; } }

        #endregion

        public clsIFA(int intIFA_ID) {
            getIFAInfo(intIFA_ID);
        }
        public clsIFA() { }

        private void getIFAInfo(int intIFA_ID)
        {
            SqlCommand cmd = new SqlCommand();
            SqlDataReader dr;
            con.Open();
            cmd.Connection = con;
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "[SWITCH_IFAGet]";

            cmd.Parameters.Add("@param_intIFA_ID", System.Data.SqlDbType.Int).Value = intIFA_ID;

            dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                this.intIFA_ID = int.Parse(dr["IFA_ID"].ToString().Trim());
                this.strIFA_Name = dr["IFA_Name"].ToString().Trim();
                this.strIFAEmail = dr["IFAEmail"].ToString().Trim();
            }

            dr.Close();
            con.Close();
            cmd.Dispose();
            con.Dispose();
        }
        public static List<clsIFA> getIFAList()
        {
            List<clsIFA> oIFAList = new List<clsIFA>();
            SqlConnection con = new clsSystem_DBConnection(clsSystem_DBConnection.strConnectionString.NavIntegrationDB).propConnection;
            SqlCommand cmd = new SqlCommand();
            SqlDataReader dr;

            con.Open();
            cmd.Connection = con;
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "SWITCH_IFAGet";

            cmd.Parameters.Add("@param_intIFA_ID", System.Data.SqlDbType.Int).Value = 0;

            dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                clsIFA oIFA = new clsIFA();
                oIFA.propIFA_ID = int.Parse(dr["IFA_ID"].ToString().Trim());
                oIFA.propIFA_Name = dr["IFA_Name"].ToString().Trim();

                oIFAList.Add(oIFA);
            }
            return oIFAList;
        }
    }
}
