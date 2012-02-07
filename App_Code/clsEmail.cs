using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Net.Mail;

namespace NAV
{
    public class clsEmail
    {

        String strSMTPHost = System.Configuration.ConfigurationManager.AppSettings["SMTPHost"].ToString();

        SqlConnection con = new clsSystem_DBConnection(clsSystem_DBConnection.strConnectionString.NavIntegrationDB).propConnection;

        public clsEmail(){}

        public static void Send(String strRecipient, String strSender, String strSubject, String strBody, int intSwitchID, string strClientID, enumEmailPurpose enumPurpose)
        {
            String strSMTPHost = new clsEmail().strSMTPHost;

            MailAddress from = new MailAddress(strSender);
            MailAddress to = new MailAddress(strRecipient);

            MailMessage message = new MailMessage(from, to);
            message.Subject = strSubject;
            message.Body = strBody;
            message.IsBodyHtml = true;

            SmtpClient client = new SmtpClient();
            client.Host = strSMTPHost;

            client.Send(message);

            insertEmailLog(intSwitchID, strRecipient, strClientID, strBody, enumPurpose);
        }

        public static void SendWithAttachment(String strRecipient, String strSender, String strSubject, String strBody, int intSwitchID, string strClientID, enumEmailPurpose enumPurpose, string strAttachment)
        {
            String strSMTPHost = new clsEmail().strSMTPHost;

            MailAddress from = new MailAddress(strSender);
            MailAddress to = new MailAddress(strRecipient);

            MailMessage message = new MailMessage(from, to);
            message.Subject = strSubject;
            message.Body = strBody;
            message.IsBodyHtml = true;
            message.Attachments.Add(new Attachment(strAttachment));

            SmtpClient client = new SmtpClient();
            client.Host = strSMTPHost;

            client.Send(message);

            insertEmailLog(intSwitchID, strRecipient, strClientID, strBody, enumPurpose);
        }

        public static void insertEmailLog(int intSwitchID, string strRecipient, string strClientID, string strMessage, enumEmailPurpose enumPurpose)
        {
            SqlConnection con = new clsSystem_DBConnection(clsSystem_DBConnection.strConnectionString.NavIntegrationDB).propConnection;
            SqlCommand cmd = new SqlCommand();

            con.Open();
            cmd.Connection = con;
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "[SWITCH_EmailLogInsert]";

            cmd.Parameters.Add("@param_intSwitchID", System.Data.SqlDbType.Int).Value = intSwitchID;
            cmd.Parameters.Add("@param_strRecipient", System.Data.SqlDbType.NVarChar).Value = strRecipient;
            cmd.Parameters.Add("@param_strClientID", System.Data.SqlDbType.NVarChar).Value = strClientID;
            cmd.Parameters.Add("@param_strMessage", System.Data.SqlDbType.NVarChar).Value = strMessage;
            cmd.Parameters.Add("@param_strPurpose", System.Data.SqlDbType.NVarChar).Value = enumPurpose.ToString(); 

            cmd.ExecuteNonQuery();
        }

        public static string generateEmailBody(String strTemplateName, String strIFAName, String strClientName, String strPortfolioName, String strContactNo, String strComment, string strCompanyName, string strSwitchID)
        {
            clsEmail.Template objTemplate = new clsEmail.Template(null, strTemplateName);
            String strBody = objTemplate.propBody;

            strBody = strBody.Replace("{%param_IFAName%}", strIFAName);
            strBody = strBody.Replace("{%param_ClientName%}", strClientName);
            strBody = strBody.Replace("{%param_PortfolioName%}", strPortfolioName);
            strBody = strBody.Replace("{%param_ContactNo%}", strContactNo);
            strBody = strBody.Replace("{%param_Comment%}", strComment);
            strBody = strBody.Replace("{%SwitchID%}", strSwitchID);
            strBody = strBody.Replace("{%Company%}", strCompanyName);

            //return HttpUtility.HtmlDecode(strBody);
            return strBody;
        }


        public class Template {
            
            #region properties

            private int intEmailTemplateID;
            public int propEmailTemplateID { get { return intEmailTemplateID; } set { intEmailTemplateID = value; } }

            private string strTemplateName;
            public string propTemplateName { get { return strTemplateName; } set { strTemplateName = value; } }

            private string strDescription;
            public string propDescription { get { return strDescription; } set { strDescription = value; } }

            private string strBody;
            public string propBody { get { return strBody; } set { strBody = value; } }

            #endregion

            #region "constructors"

            public Template() { }

            public Template(Nullable<int> intEmailTemplateID, string strTemplateName)
            {
                getEmailTemplate(intEmailTemplateID, strTemplateName);
            }

            #endregion

            #region "procedures"

            public void getEmailTemplate(Nullable<int> intEmailTemplateID, string strTemplateName)
            {
                SqlConnection con = new clsEmail().con;

                SqlCommand cmd = new SqlCommand();
                SqlDataReader dr;

                con.Open();
                cmd.Connection = con;
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = "SWITCH_EmailTemplateGet";

                cmd.Parameters.Add("@param_intEmailTemplateID", System.Data.SqlDbType.Int).Value = intEmailTemplateID;
                cmd.Parameters.Add("@param_strTemplateName", System.Data.SqlDbType.NVarChar).Value = strTemplateName;

                dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    this.propBody = dr["Body"].ToString();
                    this.propDescription = dr["Description"].ToString();
                    this.propEmailTemplateID = int.Parse(dr["EmailTemplateID"].ToString());
                    this.propTemplateName = dr["TemplateName"].ToString();
                }

                con.Close();
                cmd.Dispose();
            }

            public static void insertEmailTemplate(string strTemplateName, string strDescription, string strBody)
            {
                SqlConnection con = new clsEmail().con;
                SqlCommand cmd = new SqlCommand();

                con.Open();
                cmd.Connection = con;
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = "SWITCH_EmailTemplateInsert";

                cmd.Parameters.Add("@param_strTemplateName", System.Data.SqlDbType.NVarChar).Value = strTemplateName.Trim();
                cmd.Parameters.Add("@param_strDescription", System.Data.SqlDbType.NVarChar).Value = strDescription.Trim();
                cmd.Parameters.Add("@param_strBody", System.Data.SqlDbType.NVarChar).Value = strBody.Trim();

                try
                {
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw ex;
                }

                con.Close();
                cmd.Dispose();
            }

            public static void editEmailTemplate(int intEmailTemplateID, string strTemplateName, string strDescription, string strBody)
            {
                SqlConnection con = new clsEmail().con;
                SqlCommand cmd = new SqlCommand();

                con.Open();
                cmd.Connection = con;
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = "SWITCH_EmailTemplateUpdate";

                cmd.Parameters.Add("@param_intEmailTemplateID", System.Data.SqlDbType.Int).Value = intEmailTemplateID;
                cmd.Parameters.Add("@param_strTemplateName", System.Data.SqlDbType.NVarChar).Value = strTemplateName;
                cmd.Parameters.Add("@param_strDescription", System.Data.SqlDbType.NVarChar).Value = strDescription;
                cmd.Parameters.Add("@param_strBody", System.Data.SqlDbType.NVarChar).Value = strBody;

                try
                {
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw ex;
                }

                con.Close();
                cmd.Dispose();
            }

            public static void deleteEmailTemplate(int intEmailTemplateID)
            {

                SqlConnection con = new clsEmail().con;

                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;

                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = "[SWITCH_EmailTemplateDelete]";

                cmd.Parameters.Add("@param_intEmailTemplateID", System.Data.SqlDbType.Int).Value = intEmailTemplateID;

                cmd.ExecuteNonQuery();

            }

            public static List<Template> getListEmailTemplate()
            {
                SqlConnection con = new clsSystem_DBConnection(clsSystem_DBConnection.strConnectionString.NavIntegrationDB).propConnection;
                SqlCommand cmd = new SqlCommand();
                SqlDataReader dr;

                con.Open();
                cmd.Connection = con;
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = "SWITCH_EmailTemplateGetAll";

                dr = cmd.ExecuteReader();

                List<Template> newListTemplate = new List<Template>();

                while (dr.Read())
                {
                    Template newTemplate = new Template(int.Parse(dr["EmailTemplateID"].ToString()), null);
                    newListTemplate.Add(newTemplate);
                }

                con.Close();
                cmd.Dispose();

                return newListTemplate;
            }

            #endregion

        }

        public enum enumEmailPurpose { 
            ClientRequestingContact,
            ClientAmendmentsNotification,
            ClientRequestingContactScheme,
            ClientDeclinedSwitchPortfolio,
            ClientDeclinedSwitchScheme,
            ApproveSwitchNotification
        }
    }
}