using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NAV.Admin
{
    public partial class SMSTemplate_List : System.Web.UI.Page
    {
        #region "events"

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                PageMode(pageMode.view);
            }
        }

        protected void btnAddTemplate_Click(object sender, EventArgs e)
        {
            try
            {
                addSMSTemplate(this.txtTemplateName.Text.Trim(), this.txtTemplateFor.Text.Trim(), this.txtMessage.Text.Trim());
                ClientScript.RegisterStartupScript(this.GetType(), "alertSuccessAdd", "alert('New SMS Template added.');", true);
                PageMode(pageMode.view);
            }
            catch (Exception ex)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "alertErr", "alert('" + ex.Message.Replace("'", " ") + "');", true);
            }
        }

        protected void btnEditTemplate_Click(object sender, EventArgs e)
        {
            try
            {
                editSMSTemplate(Int16.Parse(this.hfSMSTemplateID.Value.Trim()), this.txtTemplateName.Text.Trim(), this.txtTemplateFor.Text.Trim(), this.txtMessage.Text.Trim());
                ClientScript.RegisterStartupScript(this.GetType(), "alertSuccessEdit", "alert('SMS Template edited.');", true);
                PageMode(pageMode.view);
            }
            catch (Exception ex)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "alertErr", "alert('" + ex.Message.Replace("'", " ") + "');", true);
            }
        }

        protected void btnSMSTemplateMaintenanceCancel_Click(object sender, EventArgs e)
        {            
            PageMode(pageMode.view);
        }

        protected void lbtnEditTemplate_Click(object sender, CommandEventArgs e)
        {
            this.populateFields(short.Parse(e.CommandArgument.ToString()));

            this.PageMode(pageMode.edit);

            this.mpeSMSTemplateMaintenance.Show();

            //ClientScript.RegisterStartupScript(this.GetType(), "ss", "alert('" + e.CommandArgument.ToString() + "');", true);

            //if (row.RowIndex == 1){
            //    row.BackColor = System.Drawing.Color.Teal;
            //}               
        }

        protected void lbtnRemoveTemplate_Click(object sender, CommandEventArgs e)
        {
            clsSMS.subclsSMSTemplate.deleteSMSTemplate(short.Parse(e.CommandArgument.ToString()));
            ClientScript.RegisterStartupScript(this.GetType(), "alertDeleteSuccess", "alert('Template was successfully deleted');", true);
            this.PageMode(pageMode.view);
        }

        #endregion

        #region "procedures"

        private void populateSMSList(List<clsSMS.subclsSMSTemplate> listSMSTemplate)
        {
            this.gvSMSTemplates.DataSource = listSMSTemplate;
            this.gvSMSTemplates.DataBind();
        }

        private void clearFields()
        {
            this.txtMessage.Text = "";
            this.txtTemplateFor.Text = "";
            this.txtTemplateName.Text = "";
        }

        private void populateFields(Int16 intSMSTemplateID)
        {
            clsSMS.subclsSMSTemplate objSMSTemplate = new clsSMS.subclsSMSTemplate(intSMSTemplateID);
            this.txtMessage.Text = objSMSTemplate.propMessage;
            this.txtTemplateFor.Text = objSMSTemplate.propTemplateFor;
            this.txtTemplateName.Text = objSMSTemplate.propTemplateName;
            this.hfSMSTemplateID.Value = objSMSTemplate.propSMSTemplateID.ToString();
        }

        private void addSMSTemplate(string strTemplateName, string strTemplateFor, string Message)
        {
            try
            {
                clsSMS.subclsSMSTemplate.addSMSTemplate(strTemplateName, strTemplateFor, Message);
            }
            catch (Exception err) {
                throw err;
            }
        }

        private void editSMSTemplate(Int16 intSMSTemplateID, string strTemplateName, string strTemplateFor, string Message)
        {
            try
            {
                clsSMS.subclsSMSTemplate.editSMSTemplate(intSMSTemplateID, strTemplateName, strTemplateFor, Message);
            }
            catch (Exception err)
            {
                throw err;
            }
        }

        private void lockGrid()
        {
            foreach (GridViewRow row in this.gvSMSTemplates.Rows)
            {
                LinkButton lbtnEditTemplate = (LinkButton)row.FindControl("lbtnEditTemplate");
                lbtnEditTemplate.Enabled = false;
            }
        }

        private void releaseGrid()
        {
            foreach (GridViewRow row in this.gvSMSTemplates.Rows)
            {
                LinkButton lbtnEditTemplate = (LinkButton)row.FindControl("lbtnEditTemplate");
                lbtnEditTemplate.Enabled = false;
            }
        }

        private void PageMode(pageMode pagemode)
        {
            switch (pagemode)
            {
                case pageMode.view:
                    clearFields();
                    releaseGrid();
                    this.btnAddTemplate.Visible = true;
                    this.btnEditTemplate.Visible = false;
                    populateSMSList(clsSMS.subclsSMSTemplate.propSMSTemplateList);
                    break;
                case pageMode.add:
                    break;
                case pageMode.edit:
                    //this.lockGrid();
                    this.btnAddTemplate.Visible = false;
                    this.btnEditTemplate.Visible = true;
                    break;
                default:
                    break;
            }
        }

        #endregion        

        private enum pageMode{
            view,
            add,
            edit
        }
    }
}