<%@ Page Title="" Language="C#" MasterPageFile="~/NAV.Master" AutoEventWireup="true" CodeBehind="SchemeListDetails.aspx.cs" Inherits="NAV.Scheme.SchemeListDetails" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<%@ Register src="UserControl/ucHeader.ascx" tagname="ucHeader" tagprefix="uc1" %>

<%@ Register src="UserControl/ucCurrentHoldings.ascx" tagname="ucCurrentHoldings" tagprefix="uc2" %>

<%@ Register src="UserControl/ucSwitchDetails.ascx" tagname="ucSwitchDetails" tagprefix="uc3" %>

<%@ Register src="UserControl/ucContributions.ascx" tagname="ucContributions" tagprefix="uc4" %>

<%@ Register src="UserControl/ucProposalDisplay.ascx" tagname="ucProposalDisplay" tagprefix="uc5" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"></ajaxToolkit:ToolkitScriptManager>

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
<br />
<br />
<table width="100%">       
    <tr>
        <td>
            <uc1:ucHeader ID="ucHeader1" runat="server" />
        </td>
    </tr>
    <tr>
        <td>
            <uc2:ucCurrentHoldings ID="ucCurrentHoldings1" runat="server" />
        </td>
    </tr>
    <tr>
        <td>
            <uc4:ucContributions ID="ucContributions1" runat="server" />
        </td>
    </tr>
    <tr>
        <td>
            <uc5:ucProposalDisplay ID="ucProposalDisplay1" runat="server" Visible="false" />            
        </td>
    </tr>
    <tr>
        <td>
            <uc5:ucProposalDisplay ID="ucProposalDisplay2" runat="server" Visible="false" />
        </td>
    </tr>
    <tr>
        <td>
            <center>
                <input type="button" id="btnResetSecurityCode" runat="server" value="Reset Security Code" onclick="confirmResetCode()" />
            </center>
        </td>
    </tr>    
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
