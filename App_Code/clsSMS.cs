using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using SmsNav;

namespace NAV
{
    public class clsSMS
    {
        private SqlConnection con = new clsSystem_DBConnection(clsSystem_DBConnection.strConnectionString.NavIntegrationDB).propConnection;

        #region properties

        private SmsNav.Sms SMSobj;
        public SmsNav.Sms propSMSobj { get { return SMSobj; } }

        private Sms.responseStruct ResponseObj;
        public Sms.responseStruct propResponseObj {get {return ResponseObj;}}

        private string strSMSGateWayUserName;
        public string propSMSGateWayUserName {get {return strSMSGateWayUserName;}}

        private string strSMSGateWayPassword;
        public string propSMSGateWayPassword {get {return strSMSGateWayPassword;}}

        private string strSMSGateWayAppID;
        public string propSMSGateWayAppID { get { return strSMSGateWayAppID; } }

        private string strSMSGateWayURL;
        public string propSMSGateWayURL { get { return strSMSGateWayURL; } }

        private string strErrorMsg;
        public string propErrorMsg { get { return strErrorMsg; } }

        private string strReturnID;
        public string propReturnID { get { return strReturnID; } }

        #endregion

        public clsSMS(string strUserID){
                        
            this.SMSobj = new SmsNav.Sms();
            this.ResponseObj = new SmsNav.Sms.responseStruct();
            this.strSMSGateWayUserName = System.Configuration.ConfigurationManager.AppSettings["SMSGateWayUsername"].ToString();
            this.strSMSGateWayPassword = System.Configuration.ConfigurationManager.AppSettings["SMSGateWayPassword"].ToString();
            this.strSMSGateWayAppID = System.Configuration.ConfigurationManager.AppSettings["SMSGateWayAppID"].ToString();
            this.strSMSGateWayURL = System.Configuration.ConfigurationManager.AppSettings["SMSGateWayURL"].ToString();
        }

        public void sendMessage(string strContactNo, string strMessage) {
            this.ResponseObj = propSMSobj.sendSms(propSMSGateWayUserName, propSMSGateWayPassword, propSMSGateWayAppID, strContactNo, strMessage, propSMSGateWayURL);            
            this.strErrorMsg = propResponseObj.strError != null ? propResponseObj.strError.ToString() : "";
            this.strReturnID = propResponseObj.strMessage != null ? propResponseObj.strMessage.ToString() : "";

            if (propResponseObj.strMessage.StartsWith("ERR"))
            {
                this.strReturnID = "";
                this.strErrorMsg = propResponseObj.strMessage;
            }
            else {
                this.strErrorMsg = "";
            }
        }

        public static string getSMSMessage(int intSMSID)
        {
            string resultMessage = string.Empty;
            SqlConnection con = new clsSystem_DBConnection(clsSystem_DBConnection.strConnectionString.NavIntegrationDB).propConnection;
            SqlCommand cmd = new SqlCommand();

            con.Open();
            cmd.Connection = con;
            cmd.CommandType = System.Data.CommandType.Text;
            cmd.CommandText = string.Format("SELECT SMS_Message FROM NavIntegrationDB.dbo.SwitchSMSMessage WHERE SMS_ID = {0}", intSMSID);

            resultMessage = cmd.ExecuteScalar().ToString();

            con.Close();
            cmd.Dispose();

            return resultMessage;
        }

        public static string getMobileNumber(string strClientID)
        {
            SqlConnection con = new clsSystem_DBConnection(clsSystem_DBConnection.strConnectionString.NavIntegrationDB).propConnection;
            SqlCommand cmd = new SqlCommand();

            con.Open();
            cmd.Connection = con;
            cmd.CommandType = System.Data.CommandType.Text;
            cmd.CommandText = string.Format("SELECT [Mobile] FROM [NavGlobalDBwwwGUID].[dbo].[ClientWebDetails] WHERE ClientID = '{0}'", strClientID);

            string strMobileNumber = cmd.ExecuteScalar().ToString();

            con.Close();
            cmd.Dispose();

            return strMobileNumber;
        }

        public class subclsSMSTemplate
        {
            private SqlConnection con = new clsSystem_DBConnection(clsSystem_DBConnection.strConnectionString.NavIntegrationDB).propConnection;

            #region properties

            private Int16 intSMSTemplateID;
            public Int16 propSMSTemplateID {get {return intSMSTemplateID;} set {intSMSTemplateID = value;}}

            private string strTemplateName;
            public string propTemplateName {get {return strTemplateName;} set {strTemplateName = value;}}

            private string strTemplateFor;
            public string propTemplateFor {get {return strTemplateFor;} set {strTemplateFor = value;}}

            private string strMessage;
            public string propMessage {get { return strMessage; } set { strMessage = value; }}
            
            public static List<subclsSMSTemplate> propSMSTemplateList { get { return getListSMSTemplate(); }}

            public const string strPortfolioNameVariable = "{%PortfolioName%}";

            public const string strSecurityCodeVariable = "{%SecurityCode%}";

            #endregion

            public subclsSMSTemplate(){}

            public subclsSMSTemplate(Int16 intSMSTemplateID)
            {
                getTemplateInfo(intSMSTemplateID, null);
            }

            public subclsSMSTemplate(enumSMSTemplateID SMSTemplateID)
            {
                Int16 intSMSTemplateID = (Int16)SMSTemplateID;
                getTemplateInfo(intSMSTemplateID, null);
            }

            private void getTemplateInfo(Nullable<Int16> intSMSTemplateID, string strTemplateName_Nullable)
            {                    
                SqlCommand cmd = new SqlCommand();
                SqlDataReader dr;
                con.Open();
                cmd.Connection = con;
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = "[SWITCH_SMSTemplateGet]";

                cmd.Parameters.Add("@param_sintSMSTemplateID", System.Data.SqlDbType.SmallInt).Value = intSMSTemplateID;
                cmd.Parameters.Add("@param_TemplateName", System.Data.SqlDbType.NVarChar).Value = strTemplateName_Nullable;

                dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    this.intSMSTemplateID = short.Parse(dr["SMSTemplateID"].ToString().Trim());
                    this.strTemplateName = dr["TemplateName"].ToString().Trim();
                    this.strTemplateFor = dr["TemplateFor"].ToString().Trim();
                    this.strMessage = dr["Message"].ToString().Trim();
                }

                dr.Close();
                con.Close();
                cmd.Dispose();
                con.Dispose();                
            }

            private static List<subclsSMSTemplate> getListSMSTemplate() {
                SqlConnection con = new clsSystem_DBConnection(clsSystem_DBConnection.strConnectionString.NavIntegrationDB).propConnection;

                List<subclsSMSTemplate> listSMSTemplate = new List<subclsSMSTemplate>();
                

                SqlCommand cmd = new SqlCommand();
                SqlDataReader dr;
                con.Open();
                cmd.Connection = con;
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = "[SWITCH_SMSTemplateGetAll]";

                dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    subclsSMSTemplate SMSTemplate = new subclsSMSTemplate();

                    SMSTemplate.intSMSTemplateID = short.Parse(dr["SMSTemplateID"].ToString());
                    SMSTemplate.strTemplateName = dr["TemplateName"].ToString();
                    SMSTemplate.strTemplateFor = dr["TemplateFor"].ToString();
                    SMSTemplate.strMessage = dr["Message"].ToString();

                    listSMSTemplate.Add(SMSTemplate);
                }

                dr.Close();
                con.Close();
                cmd.Dispose();
                con.Dispose();

                return listSMSTemplate;

            }

            public enum enumSMSTemplateID
            {
                ProposeSwitch = 1,
                SecurityCode = 2,
                Reset = 3,
                ProposeSchemeSwitch = 4,
                SecurityCodeScheme = 5                
            }

            public static string getSMSTemplate(string strTemplateName)
            {
                string strSMSTemplate = "error Retrieving SMS Template!";

                try
                {
                    clsSMS.subclsSMSTemplate Template = new clsSMS.subclsSMSTemplate();

                    Template.getTemplateInfo(null, strTemplateName);
                    strSMSTemplate = Template.propMessage;
                }
                catch 
                { 

                }

                return strSMSTemplate;
            }

            public static void addSMSTemplate(string strTemplateName, string strTemplateFor, string Message)
            {
                SqlConnection con = new clsSystem_DBConnection(clsSystem_DBConnection.strConnectionString.NavIntegrationDB).propConnection;

                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;

                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = "[SWITCH_SMSTemplateInsert]";

                cmd.Parameters.Add("@param_TemplateName", System.Data.SqlDbType.NVarChar).Value = strTemplateName;
                cmd.Parameters.Add("@param_TemplateFor", System.Data.SqlDbType.NVarChar).Value = strTemplateFor;
                cmd.Parameters.Add("@param_Message", System.Data.SqlDbType.NVarChar).Value = Message;

                try {
                    cmd.ExecuteNonQuery();
                } 
                catch(Exception ex){
                    throw ex;
                }
                
            }

            public static void editSMSTemplate(Int16 intSMSTemplateID, string strTemplateName, string strTemplateFor, string Message)
            {
                SqlConnection con = new clsSystem_DBConnection(clsSystem_DBConnection.strConnectionString.NavIntegrationDB).propConnection;

                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;

                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = "[SWITCH_SMSTemplateUpdate]";

                cmd.Parameters.Add("@param_sintSMSTemplateID", System.Data.SqlDbType.SmallInt).Value = intSMSTemplateID;
                cmd.Parameters.Add("@param_strTemplateName", System.Data.SqlDbType.NVarChar).Value = strTemplateName;
                cmd.Parameters.Add("@param_strTemplateFor", System.Data.SqlDbType.NVarChar).Value = strTemplateFor;
                cmd.Parameters.Add("@param_strMessage", System.Data.SqlDbType.NVarChar).Value = Message;

                try
                {
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw ex;
                }

            }

            public static void deleteSMSTemplate(Int16 intSMSTemplateID)
            {
                SqlConnection con = new clsSystem_DBConnection(clsSystem_DBConnection.strConnectionString.NavIntegrationDB).propConnection;

                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;

                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = "[SWITCH_SMSTemplateDelete]";

                cmd.Parameters.Add("@param_sintSMSTemplateID", System.Data.SqlDbType.SmallInt).Value = intSMSTemplateID;

                try
                {
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw ex;
                }

            }

            public static String convertSMSMessage(string strRawMessage, clsIFA IFA, clsClient Client, clsPortfolio Portfolio, String strSecurityCode, clsScheme Scheme)
            {
                String strConvertedMessage = strRawMessage;                

                if (IFA != null)
                {
                    strConvertedMessage = strConvertedMessage.Replace("{%param_IFAName%}", IFA.propIFA_Name);
                }

                if (Client != null) 
                {
                    strConvertedMessage = strConvertedMessage.Replace("{%param_ClientName%}", Client.propForename + " " + Client.propSurname);
                }

                if (Portfolio != null)
                {
                    strConvertedMessage = strConvertedMessage.Replace("{%param_PortfolioName%}", Portfolio.propCompany);
                }

                if (strSecurityCode != null)
                {
                    strConvertedMessage = strConvertedMessage.Replace("{%SecurityCode%}", strSecurityCode);
                }

                if (Scheme != null) 
                {
                    strConvertedMessage = strConvertedMessage.Replace("{%param_SchemeName%}", Scheme.propCompany.propCompany);
                }
                
                return HttpUtility.HtmlDecode(strConvertedMessage);
            }
        }

    }
}