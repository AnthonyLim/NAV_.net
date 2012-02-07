using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Security.Cryptography;
using System.Data.SqlClient;

namespace NAV
{
    public class clsSecurityCode
    {
        #region property
        private string securityCode;
        public string propSecurityCode
        {
            get { return securityCode; }
            set { securityCode = value; }
        }
        private string encryptedCode;
        public string propEncryptedCode
        {
            get { return encryptedCode; }
        }
        private bool isExist;
        public bool propIsExist
        {
            get { return isExist; }
        }
        #endregion

        //constructor
        public clsSecurityCode(string securityCode)
        {
            encryptedCode = encryptCode(securityCode);
        }
        public clsSecurityCode(int intSecurityCodeLength)
        {
            securityCode = generateCode(intSecurityCodeLength);
            encryptedCode = encryptCode(securityCode);
            isExist = checkGeneratedCode();
        }

        #region public function
        public int insertEncryptedCode(int intSwitchID, string strClientID, string strPortfolioID)
        {
            SqlConnection con = new clsSystem_DBConnection(clsSystem_DBConnection.strConnectionString.NavIntegrationDB).propConnection;
            SqlCommand cmd = new SqlCommand();

            con.Open();
            cmd.Connection = con;
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "[SWITCH_GenerateCodeInsert]";
            cmd.Parameters.Add("@param_Code", System.Data.SqlDbType.NVarChar).Value = encryptedCode;
            cmd.Parameters.Add("@param_SwitchID", System.Data.SqlDbType.Int).Value = intSwitchID;
            cmd.Parameters.Add("@param_ClientID", System.Data.SqlDbType.NVarChar).Value = strClientID;
            cmd.Parameters.Add("@param_PortfolioID", System.Data.SqlDbType.NVarChar).Value = strPortfolioID;
            int result = (int)cmd.ExecuteScalar();
            con.Close();
            cmd.Dispose();

            return result;
        }
        public int insertEncryptedCode_Scheme(int intSwitchID, string strClientID, string strSchemeID)
        {
            SqlConnection con = new clsSystem_DBConnection(clsSystem_DBConnection.strConnectionString.NavIntegrationDB).propConnection;
            SqlCommand cmd = new SqlCommand();

            con.Open();
            cmd.Connection = con;
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "[SWITCHScheme_GenerateCodeInsert]";
            cmd.Parameters.Add("@param_Code", System.Data.SqlDbType.NVarChar).Value = encryptedCode;
            cmd.Parameters.Add("@param_SwitchID", System.Data.SqlDbType.Int).Value = intSwitchID;
            cmd.Parameters.Add("@param_ClientID", System.Data.SqlDbType.NVarChar).Value = strClientID;
            cmd.Parameters.Add("@param_SchemeID", System.Data.SqlDbType.NVarChar).Value = strSchemeID;
                                
            int result = (int)cmd.ExecuteScalar();
            con.Close();
            cmd.Dispose();

            return result;
        }
        public string ValidateSecurityCode(int intSwitchID, string strClientID, string strPortfolioID)
        {
            string strMessage = string.Empty;
            SqlConnection con = new clsSystem_DBConnection(clsSystem_DBConnection.strConnectionString.NavIntegrationDB).propConnection;
            SqlCommand cmd = new SqlCommand();

            con.Open();
            cmd.Connection = con;
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "[SWITCH_CheckSecurityCodeValid]";
            cmd.Parameters.Add("@param_Code", System.Data.SqlDbType.NVarChar).Value = encryptedCode;
            cmd.Parameters.Add("@param_SwitchID", System.Data.SqlDbType.Int).Value = intSwitchID;
            cmd.Parameters.Add("@param_ClientID", System.Data.SqlDbType.NVarChar).Value = strClientID;
            cmd.Parameters.Add("@param_PortfolioID", System.Data.SqlDbType.NVarChar).Value = strPortfolioID;
            strMessage = cmd.ExecuteScalar().ToString();
            con.Close();
            cmd.Dispose();

            return strMessage;
        }
        public string ValidateSecurityCodeScheme(int intSwitchID, string strClientID, string strSchemeID)
        {
            string strMessage = string.Empty;
            SqlConnection con = new clsSystem_DBConnection(clsSystem_DBConnection.strConnectionString.NavIntegrationDB).propConnection;
            SqlCommand cmd = new SqlCommand();

            con.Open();
            cmd.Connection = con;
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "[SWITCHScheme_CheckSecurityCodeValid]";
            cmd.Parameters.Add("@param_Code", System.Data.SqlDbType.NVarChar).Value = encryptedCode;
            cmd.Parameters.Add("@param_SwitchID", System.Data.SqlDbType.Int).Value = intSwitchID;
            cmd.Parameters.Add("@param_ClientID", System.Data.SqlDbType.NVarChar).Value = strClientID;
            cmd.Parameters.Add("@param_SchemeID", System.Data.SqlDbType.NVarChar).Value = strSchemeID;
            strMessage = cmd.ExecuteScalar().ToString();
            con.Close();
            cmd.Dispose();

            return strMessage;
        }
        #endregion

        # region private function
        private string generateCode(int length)
        {
            //is a base 16 128 bit generated string, from the result we get a random 4 to 16 maximun digits
            string guidResult = System.Guid.NewGuid().ToString();
            guidResult = guidResult.Replace("-", "");

            //based on the length get the length from the string
            guidResult = guidResult.Substring(0, length);

            //todo 
            //generated 
            return guidResult;
        }
        private string encryptCode(string strSecurityCode)
        {
            //string strEncryptCode = generateString(4);
            MD5CryptoServiceProvider md5Hasher = new MD5CryptoServiceProvider();
            byte[] hashedDataBytes;
            //byte[] stringDataBytes;

            UTF8Encoding encoder = new UTF8Encoding();
            //ASCIIEncoding encoder = new ASCIIEncoding(); 
            hashedDataBytes = md5Hasher.ComputeHash(encoder.GetBytes(strSecurityCode));
            //hashedDataBytes = md5Hasher.ComputeHash()

            string strEncryptCode = encoder.GetString(hashedDataBytes);

            return strEncryptCode;
        }
        private bool checkGeneratedCode()
        {
            return false;
        }
        # endregion
    }
}
