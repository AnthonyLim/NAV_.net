<%@ Page Title="" Language="C#" MasterPageFile="~/NAV.Master" AutoEventWireup="true" CodeBehind="EmailTemplate_List.aspx.cs" Inherits="NAV.Admin.EmailTemplate_List" ValidateRequest="false" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<br />&nbsp;
<br />
<ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"></ajaxToolkit:ToolkitScriptManager>

    <style type="text/css">
        .modalBox
        {
            background-color : #f5f5f5;
            border-width: 3px;
            border-style: solid;
            border-color: Blue;
            padding: 3px;
        }
            
        .modalBackground
        {
            background-color: Gray;
            filter: alpha(opacity=50);
            opacity: 0.5;
        }
        .multilineTextbox
        { 
            resize: none;
        }
        
    </style>
<br />&nbsp;
<br />
<table width="100%" cellpadding="0" cellspacing="0" border="0">
    <tr>
        <td>
            <div id="table1_container">
            <asp:GridView runat="server" ID="gvEmailTemplates" Width="100%" AutoGenerateColumns="false" UseAccessibleHeader="true" CssClass="table1">
                <Columns>
                    <asp:TemplateField HeaderText="">
                        <HeaderStyle CssClass="t1_column_color1" BorderStyle="None" VerticalAlign="Middle" HorizontalAlign="Center"/>
                        <ItemStyle HorizontalAlign="Left" BorderStyle="None" CssClass="t1_column_color1" />
                        <FooterStyle BorderStyle="None" CssClass="t1_column_color1"/>
                        <ItemTemplate>
                            <asp:LinkButton runat="server" id="lbtnRemoveTemplate" OnClientClick="return confirm('Proceed deleting this template?')"  OnCommand="lbtnRemoveTemplate_Click"  Text="Delete" Font-Size="Small" CommandArgument='<%#Eval("propEmailTemplateID")%>' ></asp:LinkButton>&nbsp;||&nbsp;<asp:LinkButton runat="server" id="lbtnEditTemplate" Text="Edit" Font-Size="Small" CommandArgument='<%#Eval("propEmailTemplateID")%>' OnCommand="lbtnEditTemplate_Click" ></asp:LinkButton>                            
                        </ItemTemplate>
                    </asp:TemplateField>                
                    <asp:TemplateField HeaderText="Template&nbsp;Name">
                        <ItemStyle HorizontalAlign="Left"  BorderStyle="None" CssClass="t1_column_color2" />
                        <HeaderStyle CssClass="t1_column_color2" BorderStyle="None" VerticalAlign="Middle" HorizontalAlign="Center" />
                        <FooterStyle BorderStyle="None" CssClass="t1_column_color2"/>
                        <ItemTemplate>
                            <asp:HiddenField runat="server" ID="hfGvEmailTemplateID" Value='<% #Eval("propEmailTemplateID") %>'/>
                            <asp:Label runat="server" ID="lblGvTemplateName" Text='<% #Eval("propTemplateName") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Description">
                        <HeaderStyle CssClass="t1_column_color1" BorderStyle="None" VerticalAlign="Middle" HorizontalAlign="Center"/>
                        <ItemStyle HorizontalAlign="Left" BorderStyle="None" CssClass="t1_column_color1" />
                        <FooterStyle BorderStyle="None" CssClass="t1_column_color1"/>
                        <ItemTemplate>
                            <asp:Label runat="server" ID="lblGvTemplateFor" Text='<% #Eval("propDescription") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>    
                    <asp:TemplateField HeaderText="Message">
                        <ItemStyle HorizontalAlign="Right"  BorderStyle="None" CssClass="t1_column_color2" Width="50%" />
                        <HeaderStyle CssClass="t1_column_color2" BorderStyle="None" VerticalAlign="Middle" HorizontalAlign="Center" Width="50%" />
                        <FooterStyle BorderStyle="None" CssClass="t1_column_color2" Width="50%"/>
                        <ItemTemplate>
                            <asp:TextBox runat="server" ID="txtGvMessage" Text='<% #Eval("propBody") %>' Wrap="true" rows="2" Columns="80" ReadOnly="true" TextMode="MultiLine" BorderStyle="None" BorderWidth="0"></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>                        
                </Columns>   
                <EmptyDataTemplate>
                    <asp:Label runat="server" ID="lblNoTemplateFound" Text="No Template Found!"></asp:Label>
                </EmptyDataTemplate>
            </asp:GridView>        
            </div>
        </td>
    </tr>
    <tr>
        <td align="left">
            <input type="button" id="btnAdd" onclick='popUp_AddEmailTemplate();' value="   Add   " />
        </td>
    </tr>
</table>

<ajaxToolkit:ModalPopupExtender ID="mpeEmailTemplateMaintenance" runat="server" TargetControlID="Button2" PopupControlID="PopupEmailTemplateMaintenance" BackgroundCssClass="modalBackground" DropShadow="true" BehaviorID="bhvrEmailTemplateMaintenance" />
<asp:Panel runat="server" ID="PopupEmailTemplateMaintenance" CssClass="modalBox" ScrollBars="Both" style="display:none;overflow:auto;" Height="700px" Width="600px">    
<br />
<table>
    <tr>
        <td align="right">
            <asp:Label runat="server" ID="lblTemplateName" Text="Template&nbsp;Name: " Font-Bold="true" Font-Size="Medium"></asp:Label>
        </td>
        <td>
            <asp:HiddenField runat="server" ID="hfEmailTemplateID"/>
            <asp:TextBox runat="server" ID="txtTemplateName" MaxLength="100" Width="400" ValidationGroup="vgTemplateMaintenance"></asp:TextBox><br />
            <asp:RequiredFieldValidator runat="server" ID="rfvTemplateName" ControlToValidate="txtTemplateName" ValidationGroup="vgTemplateMaintenance" ErrorMessage="Template Name is required" Font-Size="X-Small" Display="Dynamic"/>
        </td>
    </tr>
    <tr>
        <td align="right">
            <asp:Label runat="server" ID="lblDescription" Text="Details: " Font-Bold="true" Font-Size="Medium"></asp:Label>
        </td>
        <td>
            <asp:TextBox runat="server" ID="txtDescription" MaxLength="100" Width="400" ValidationGroup="vgTemplateMaintenance"></asp:TextBox><br />
            <asp:RequiredFieldValidator runat="server" ID="rfvDescription" ControlToValidate="txtDescription" ValidationGroup="vgTemplateMaintenance" ErrorMessage="Description is required" Font-Size="X-Small" Display="Dynamic"/>
        </td>
    </tr>
    <tr>
        <td align="right" valign="top">
            <asp:Label runat="server" ID="lblBody" Text="Body: " Font-Bold="true" Font-Size="Medium"></asp:Label>
        </td>
        <td align="left">
            <asp:TextBox runat="server" ID="txtBody" TextMode="MultiLine" Rows="35" Columns="47" ValidationGroup="vgTemplateMaintenance" ></asp:TextBox><br />
            <asp:RequiredFieldValidator runat="server" ID="rfvBody" ControlToValidate="txtBody" ValidationGroup="vgTemplateMaintenance" ErrorMessage="Body is empty" Font-Size="X-Small" Display="Dynamic"/>            
        </td>
    </tr>
    <tr>
        <td>
            &nbsp;
        </td>
        <td  colspan="2" align="left">
            <asp:ValidationSummary runat="server" ID="vsSwitchDetails" ShowSummary="false" ValidationGroup="vgTemplateMaintenance" EnableClientScript="true" HeaderText="" />
            <asp:Button runat="server" ID="btnAddTemplate" Text="  Save  " OnClick="btnAddTemplate_Click" ValidationGroup="vgTemplateMaintenance" CausesValidation="true" />
            <asp:Button runat="server" ID="btnEditTemplate" Text="  Save  " OnClick="btnEditTemplate_Click" ValidationGroup="vgTemplateMaintenance" CausesValidation="true"  />
            <asp:Button runat="server" ID="btnSMSTemplateMaintenanceCancel" Text="   Cancel   " OnClick="btnEmailTemplateMaintenanceCancel_Click" />
            
        </td>
    </tr>
</table>

</asp:Panel>

<asp:Button ID="Button2" runat="server"  Text="" style="display:none;"/>
<script language="javascript" type="text/javascript">

    function popUp_AddEmailTemplate() {
        clearMaintenanceField();        
        $find("bhvrEmailTemplateMaintenance", null).show();
    }

    function popUp_EditSMSTemplate(TemplateID) {
        $find("bhvrSMSTemplateMaintenance", null).show();
    }    

    function clearMaintenanceField() {
        document.getElementById("ctl00_ContentPlaceHolder1_txtTemplateName").value = '';
        document.getElementById("ctl00_ContentPlaceHolder1_txtDescription").value = '';
        document.getElementById("ctl00_ContentPlaceHolder1_txtBody").value = '';
    }
    
    
    
</script>
</asp:Content>
