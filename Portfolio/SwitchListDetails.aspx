<%@ Page Title="" Language="C#" MasterPageFile="~/NAV.Master" AutoEventWireup="true" CodeBehind="SwitchListDetails.aspx.cs" Inherits="NAV.SwitchResetCodeDetails" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

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
    .style1
    {
        width: 35%;
    }
    .style2
    {
        width: 65%;
    }
</style>

<script type="text/javascript">

    function confirmResetCode() {
        var result = confirm('Do you want to unlock this Portfolio?');
        if (result) {
            $find('bhvrResetSecurityCode', null).show();
        }
        else {
            $find('bhvrResetSecurityCode', null).hide();
        }
    }
    function checkMobile() {
        if (document.getElementById('ctl00_ContentPlaceHolder1_txtMobileNoResetCode').value == '') {
            alert('Please enter mobile number!');
            document.getElementById('ctl00_ContentPlaceHolder1_txtMobileNoResetCode').focus()
            return false;
        }
    }
    
</script>

<table class='main_info' border="0" cellpadding="0" cellspacing="0" width="100%">
<br />&nbsp;
<br />&nbsp;
<tr><td>
    <table  border="0" align="left" cellpadding="0" cellspacing="0" width="100%">
        <thead align="left">
        <tr>
            <td colspan="2">
                <h2><asp:Label runat="server" ID="lblValue_PortfolioName"/></h2>
            </td>
        </tr>        
    </thead>
    <tbody class='member_info'>
    <tr> 
        <td style="width:21%" align="left">
            <asp:Label runat="server" ID="lblTitle_Company" Text="Company" />
        </td>
        <td style="width:79%" align="left">
            <asp:Label runat="server" ID="lblValue_Company"/>
        </td>
    </tr>    
    <tr> 
        <td style="width:1%" nowrap="nowrap" align="left">
            <asp:Label runat="server" ID="lblTitle_PortfolioType" Text="Portfolio Type" />
        </td>
        <td style="width:1%" nowrap="nowrap" align="left">
            <asp:Label runat="server" ID="lblValue_PortfolioType"/>
        </td>
    </tr>    
    <tr> 
        <td style="width:1%" nowrap="nowrap" align="left">
            <asp:Label runat="server" ID="lblTitle_Currency" Text="Currency" />
        </td>
        <td style="width:1%" nowrap="nowrap" align="left">
            <asp:Label runat="server" ID="lblValue_Currency"/>
        </td>
    </tr>
    <tr>
        <td style="width:1%" nowrap="nowrap" align="left">
            <asp:Label runat="server" ID="lblTitle_AccountNumber" Text="AccountNumber" />
        </td>
        <td style="width:1%" nowrap="nowrap" align="left">
            <asp:Label runat="server" ID="lblValue_AccountNumber"/>
        </td>
    </tr>    
    <tr>
        <td style="width:1%" nowrap="nowrap" align="left">
            <asp:Label runat="server" ID="lblTitle_PlanStatus" Text="Plan Status" />
        </td>
        <td style="width:1%" nowrap="nowrap" align="left">
            <asp:Label runat="server" ID="lblValue_PlanStatus"/>
        </td>
    </tr>        
    <tr>
        <td style="width:1%" nowrap="nowrap" align="left">
            <asp:Label runat="server" ID="lblTitle_StartDate" Text="Start Date" />
        </td>
        <td style="width:1%" nowrap="nowrap" align="left">
            <asp:Label runat="server" ID="lblValue_StartDate"/>
        </td>
    </tr>
    <tr>
        <td style="width:1%" nowrap="nowrap" align="left">
            <asp:Label runat="server" ID="lblTitle_PolicyCategory" Text="Policy Category" />
        </td>
        <td style="width:1%" nowrap="nowrap" align="left">
            <asp:Label runat="server" ID="lblValue_PolicyCategory"/>
        </td>
    </tr>
    <tr>
        <td style="width:1%" nowrap="nowrap" align="left">
            <asp:Label runat="server" ID="lblTitle_Profile" Text="Profile" />
        </td>
        <td style="width:1%" nowrap="nowrap" align="left">
            <asp:Label runat="server" ID="lblValue_Profile"/>
        </td>
    </tr>    
    <tr>
        <td style="width:1%" nowrap="nowrap" align="left">
            <asp:Label runat="server" ID="lblTitle_SpecialistInformation" Text="Specialist Information" />
        </td>
        <td style="width:1%" nowrap="nowrap" align="left">
            <asp:Label runat="server" ID="lblValue_SpecialistInformation"/>
        </td>
    </tr>    
    <tr>
        <td style="width:1%" nowrap="nowrap" align="left">
            <asp:Label runat="server" ID="lblTitle_Discretionary" Text="Discretionary" />
        </td>
        <td style="width:1%" nowrap="nowrap" align="left">
            <asp:Label runat="server" ID="lblValue_Discretionary"/>
        </td>
    </tr>
    </tbody>   
    </table>
<br />&nbsp;
</td></tr>
<tr><td align="left">
    <asp:Label runat="server" ID="lblSwitchStatusTitle" Text="Status: "/>&nbsp;<asp:Label runat="server" ID="lblSwitchStatusValue"/>
    <br />&nbsp;
</td></tr>
<tr><td>
<div class="table2">
    <asp:GridView runat="server" ID="gvSwitchDetails" AutoGenerateColumns="false" Width="100%" ShowFooter="true" BorderStyle="None" CellPadding="0" CellSpacing="0" UseAccessibleHeader="true" CssClass="table2" >
    <Columns>
        <asp:TemplateField HeaderText="Fund Name">
            <HeaderStyle HorizontalAlign="Left" CssClass="t2_header" />
            <ItemStyle HorizontalAlign="Left" BorderStyle="None"/>
            <FooterStyle HorizontalAlign="Left"  CssClass="t2_results"/>
            <ItemTemplate><asp:Label runat="server" ID="gvSwitchDetailsItemLabelFundName" Text='<%#DataBinder.Eval(Container.DataItem, "propFund.propFundName")%>'></asp:Label></ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Price">
            <HeaderStyle HorizontalAlign="Right"  VerticalAlign="Top" CssClass="t2_header"/>
            <ItemStyle HorizontalAlign="Right"  BorderStyle="None"/>
            <FooterStyle CssClass="t2_results"/>
            <ItemStyle HorizontalAlign="Right" />
            <ItemTemplate>
                <asp:Label runat="server" ID="gvSwitchDetailsItemLabelCurrency" Text='<%#Eval("propFund.propCurrency")%>'></asp:Label>
                &nbsp;
                <asp:Label runat="server" ID="gvSwitchDetailsItemLabelPrice" Text='<%#Eval("propFund.propPrice", "{0:0.0000}")%>'></asp:Label>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Unit">
            <HeaderStyle HorizontalAlign="Right" VerticalAlign="Top" CssClass="t2_header" />
            <ItemStyle HorizontalAlign="Right" BorderStyle="None" />
            <FooterStyle HorizontalAlign="Right" CssClass="t2_results"/>
            <ItemTemplate>
                <asp:Label runat="server" ID="gvSwitchDetailsItemLabelUnit" Text='<%#Eval("propUnits", "{0:#,##0.0000}")%>'></asp:Label>
            </ItemTemplate>
        </asp:TemplateField>
        <%--<asp:BoundField HeaderText="Unit" DataField="propUnits" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:#,##0.0000}" />--%>
        <asp:TemplateField HeaderText="Value">
            <HeaderStyle HorizontalAlign="Right" VerticalAlign="Top" CssClass="t2_header" />
            <ItemStyle HorizontalAlign="Right" BorderStyle="None" />
            <FooterStyle HorizontalAlign="Right" CssClass="t2_results"/>
            <ItemTemplate>
                <asp:Label runat="server" ID="gvSwitchDetailsItemLabelValue" Text='<% #Eval("propValue", "{0:n0}") %>'/>
            </ItemTemplate>
            <FooterTemplate>
                Total:&nbsp;<asp:Label runat="server" ID="gvSwitchDetailsFooterLabelTotalValue" />
            </FooterTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Allocation">
            <HeaderStyle HorizontalAlign="Right"  VerticalAlign="Top" CssClass="t2_header" />
            <ItemStyle HorizontalAlign="Right"  BorderStyle="None" />
            <FooterStyle HorizontalAlign="Right" CssClass="t2_results" />
            <ItemTemplate>
                <asp:Label runat="server" ID="gvSwitchDetailsItemLabelTargetAllocation" Text='<%#Eval("propAllocation", "{0:0.00}")%>'></asp:Label>
            </ItemTemplate>
            <FooterTemplate>
                <asp:Label runat="server" ID="gvSwitchDetailsFooterLabelTotalTargetAllocation" />
            </FooterTemplate>
        </asp:TemplateField>
    </Columns>
    </asp:GridView>
</div>
</td></tr>
<tr><td>
    <center>
        <input type="button" id="btnResetSecurityCode" runat="server" value="Reset Security Code" onclick="confirmResetCode()" />
    </center>
</td></tr>
<tr><td>&nbsp;</td></tr>
<tr><td><asp:Button ID="btnPrevious" runat="server" Text="Back" OnClick="btnPrevious_Click" /></td></tr>
<tr><td></td></tr>
</table>
<div>
<ajaxToolkit:ModalPopupExtender ID="mpeResetSecurityCode" runat="server" TargetControlID="Button2" CancelControlID="btnSMSResetCancel" PopupControlID="panelPopupSendSMS" BackgroundCssClass="modalBackground" DropShadow="true" BehaviorID="bhvrResetSecurityCode"></ajaxToolkit:ModalPopupExtender>
<asp:Panel runat="server" ID="panelPopupSendSMS" style="display:none;" CssClass="modalBox" Width="350px">
<div style="text-align:left;width:100%;margin-top:1px;">
        <center>
            <table width="100%">
            <tr><td>&nbsp;</td></tr>
                <tr>
                    <td align="right" class="style1">
                        <asp:Label runat="server" ID="lblMobileNoResetCode" Text="Mobile no.:" Width="100%"></asp:Label>
                    </td>                
                    <td class="style2">
                        <asp:TextBox runat="server" ID="txtMobileNoResetCode"></asp:TextBox>
                    </td>
                </tr>
            </table>                
            <br />
            <asp:Button runat="server" ID="btnSMSResetSend" OnClick="btnSMSResetSend_Click" OnClientClick="return checkMobile()" Text="Send"/>&nbsp;&nbsp;<asp:Button runat="server" ID="btnSMSResetCancel" Text="Cancel"/>
            <br />
        </center>
    </div>
</asp:Panel>
</div>
<!-- Keep this control at the last to hide it, this will make validation of popup   -->
    <asp:Button ID="Button2" runat="server"  Text="" style="display:none;"/>
</asp:Content>
