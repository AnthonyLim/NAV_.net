using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NAV.Admin
{
    public partial class EmailTemplate_List : System.Web.UI.Page
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
                clsEmail.Template.insertEmailTemplate(this.txtTemplateName.Text.Trim(), this.txtDescription.Text.Trim(), this.txtBody.Text.Trim());
                ClientScript.RegisterStartupScript(this.GetType(), "alertSuccessAdd", "alert('New Email Template added.');", true);
                PageMode(pageMode.view);
            }
            catch (Exception ex)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "alertErr", "alert('" + ex.Message.Replace("'", " ") + "');", true);
                this.mpeEmailTemplateMaintenance.Show();
            }
        }

        protected void btnEditTemplate_Click(object sender, EventArgs e)
        {
            try
            {
                clsEmail.Template.editEmailTemplate(int.Parse(this.hfEmailTemplateID.Value.ToString().Trim()), this.txtTemplateName.Text.Trim(), this.txtDescription.Text.Trim(), this.txtBody.Text.Trim() );
                ClientScript.RegisterStartupScript(this.GetType(), "alertSuccessEdit", "alert('Email Template edited.');", true);
                PageMode(pageMode.view);
            }
            catch (Exception ex)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "alertErr", "alert('" + ex.Message.Replace("'", " ") + "');", true);
                this.mpeEmailTemplateMaintenance.Show();
            }
        }

        protected void btnEmailTemplateMaintenanceCancel_Click(object sender, EventArgs e)
        {
            PageMode(pageMode.view);
        }

        protected void lbtnEditTemplate_Click(object sender, CommandEventArgs e)
        {
            this.populateFields(short.Parse(e.CommandArgument.ToString()));

            this.PageMode(pageMode.edit);

            this.mpeEmailTemplateMaintenance.Show();              
        }

        protected void lbtnRemoveTemplate_Click(object sender, CommandEventArgs e)
        {
            clsEmail.Template.deleteEmailTemplate(int.Parse(e.CommandArgument.ToString()));            
            ClientScript.RegisterStartupScript(this.GetType(), "alertDeleteSuccess", "alert('Template was successfully deleted');", true);
            this.PageMode(pageMode.view);
        }

        #endregion

        #region "procedures"

        private void populateEmailList(List<clsEmail.Template> listEmailTemplate)
        {
            this.gvEmailTemplates.DataSource = listEmailTemplate;
            this.gvEmailTemplates.DataBind();
        }

        private void clearFields()
        {
            this.txtBody.Text = "";
            this.txtDescription.Text = "";
            this.txtTemplateName.Text = "";
        }

        private void populateFields(int intEmailTemplateID)
        {            
            clsEmail.Template objTemplate = new clsEmail.Template(intEmailTemplateID, null);
            this.txtBody.Text = objTemplate.propBody;
            this.txtDescription.Text = objTemplate.propDescription;
            this.txtTemplateName.Text = objTemplate.propTemplateName;
            this.hfEmailTemplateID.Value = objTemplate.propEmailTemplateID.ToString();
        }

        private void addEmailTemplate(string strTemplateName, string strDescription, string strBody)
        {            
            try
            {
                clsEmail.Template.insertEmailTemplate(strTemplateName, strDescription, strBody);
            }
            catch (Exception err)
            {
                throw err;
            }
        }

        private void editEmailTemplate(int intEmailTemplateID, string strTemplateName, string strDescription, string strBody)
        {
            try
            {
                clsEmail.Template.editEmailTemplate(intEmailTemplateID, strTemplateName, strDescription, strBody);
            }
            catch (Exception err)
            {
                throw err;
            }            
        }

        private void lockGrid()
        {
            foreach (GridViewRow row in this.gvEmailTemplates.Rows)
            {
                LinkButton lbtnEditTemplate = (LinkButton)row.FindControl("lbtnEditTemplate");
                lbtnEditTemplate.Enabled = false;
            }
        }

        private void releaseGrid()
        {
            foreach (GridViewRow row in this.gvEmailTemplates.Rows)
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
                    populateEmailList(clsEmail.Template.getListEmailTemplate());
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

        private enum pageMode
        {
            view,
            add,
            edit
        }
    }
}
