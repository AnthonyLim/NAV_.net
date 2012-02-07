using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;

namespace NAV
{
    public class clsSystem_DBConnection
    {

        private String constr_NavIntegrationDB = System.Configuration.ConfigurationManager.ConnectionStrings["NavIntegrationDB"].ToString();
        //private String constr_NavGlobalDBwwwGUID = System.Configuration.ConfigurationManager.ConnectionStrings["NavGlobalDBwwwGUID"].ToString();

        private SqlConnection sqlconConnection;
        public SqlConnection propConnection { get { return sqlconConnection; }}


        public clsSystem_DBConnection(strConnectionString strConString)
        {
            SqlConnection con = new SqlConnection(getConnectionString(strConString));
            this.sqlconConnection = con;
        }

        private String getConnectionString(strConnectionString _strConnectionString) {
            switch (_strConnectionString)
            {
                case strConnectionString.NavIntegrationDB:
                    return this.constr_NavIntegrationDB;                    
                //case strConnectionString.NavGlobalDBwwwGUID:
                //    return this.constr_NavGlobalDBwwwGUID;                    
                default:
                    return "";
            }
        }

        public enum strConnectionString
        {
            NavIntegrationDB
        }

    }
}
