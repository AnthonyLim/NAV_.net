<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucHeader.ascx.cs" Inherits="NAV.Portfolio.UserControl.ucHeader" %>
<table class='main_info' border="0" cellpadding="0" cellspacing="0" width="100%">
    <thead align="left">
        <tr>
            <td colspan="2">
                <h2><asp:Label runat="server" ID="lblValue_ClientName"/></h2>
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