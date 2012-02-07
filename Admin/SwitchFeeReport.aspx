<%@ Page Title="" Language="C#" MasterPageFile="~/NAV.Master" AutoEventWireup="true" CodeBehind="SwitchFeeReport.aspx.cs" Inherits="NAV.Admin.SwitchFeeReport" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<br />
<br />
<table class='main_info' border="0" cellpadding="0" cellspacing="0" width="100%">
    <thead align="left">
        <tr>
            <td colspan="2">
                <h2><asp:Label runat="server" ID="lblTitleValue" Text="Switch Fee Report"/></h2>
            </td>
        </tr>        
    </thead>
    <tbody class='member_info'>
        <tr> 
            <td style="width:1%" align="left">
                <asp:Label ID="lblIFA" runat="server" Text="Select IFA :"></asp:Label>
            </td>
            <td style="width:99%" align="left">
                <asp:DropDownList ID="ddlIFAList" runat="server"></asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td style="width:15%" align="left"><asp:Label ID="lblDate_Title" runat="server" Text="Covered Period"></asp:Label></td>
            <td style="width:15%" nowrap="nowrap" align="left">
                <asp:DropDownList ID="ddlStartDateDay" runat="server" ></asp:DropDownList>
                &nbsp;
                <asp:DropDownList ID="ddlStartDateMonth" runat="server"></asp:DropDownList>
                &nbsp;
                <asp:DropDownList ID="ddlStartDateYear" runat="server" DataTextField="Year"></asp:DropDownList>
                &nbsp; - &nbsp;
                <asp:DropDownList ID="ddlEndDateDay" runat="server" DataTextField="Day"></asp:DropDownList>
                &nbsp;
                <asp:DropDownList ID="ddlEndDateMonth" runat="server" DataTextField="Month"></asp:DropDownList>
                &nbsp;
                <asp:DropDownList ID="ddlEndDateYear" runat="server" DataTextField="Year" ></asp:DropDownList>
            </td>
        </tr>
        <tr><td>&nbsp;</td></tr>
        <tr>
            <td style="width:1%" align="left"><asp:Button ID="btnViewReport" runat="server" Text="View Report" OnClick="btnViewReport_Click" OnClientClick="return checkDate()" /></td>
        </tr>
    </tbody>
</table>
<asp:Panel ID="panelSwitchFeeReport" runat="server" Visible="false">
<table border="0" align="left" cellpadding="2" cellspacing="0" width="100%">
    <tr>
        <td style="width:15%" align="left"><asp:Label ID="lblIFA_Name_Title" runat="server" Text="IFA Name"></asp:Label></td>
        <td style="width:85%" align="left"><asp:Label ID="lblIFA_Name_Value" runat="server"></asp:Label></td>
    </tr>
    <tr>
        <td colspan="4">
            <div id="table1_container">
            <asp:GridView ID="gvSwitchFeeReport" runat="server" AutoGenerateColumns="false" DataKeyNames="" Width="100%" ShowFooter="true" BorderStyle="None" CellPadding="0" CellSpacing="0" UseAccessibleHeader="true" CssClass="table1" EmptyDataText="No record found">
                <Columns>
                    <asp:BoundField DataField="propPer_Switch_Fee" HeaderText="Rate"
                                    HeaderStyle-BorderStyle="None" HeaderStyle-VerticalAlign="Middle" HeaderStyle-CssClass="t1_column_color1" 
                                    ItemStyle-BorderStyle="None" ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="t1_column_color1"
                                    FooterStyle-BorderStyle="None" FooterStyle-CssClass="t1_column_color1"/>
                    <asp:TemplateField HeaderText="No. of Switches">
                        <HeaderStyle BorderStyle="None" VerticalAlign="Middle" HorizontalAlign="Center" CssClass="t1_column_color2" />
                        <ItemStyle BorderStyle="None" HorizontalAlign="Center" CssClass="t1_column_color2" />
                        <FooterStyle BorderStyle="None" CssClass="t1_column_color2" />
                        <ItemTemplate>
                            <asp:LinkButton ID="lbtnSwitchDetails" runat="server" Text='<%#Eval("propQuantity")%>' OnCommand="lbtnSwitchDetails_Click" CommandArgument='<%#Eval("propSwitches")%>'></asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>                
                    <%--<asp:BoundField DataField="propQuantity" HeaderText="No. of Switches" 
                                    HeaderStyle-BorderStyle="None" HeaderStyle-VerticalAlign="Middle" HeaderStyle-CssClass="t1_column_color2" 
                                    ItemStyle-BorderStyle="None" ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="t1_column_color2" 
                                    FooterStyle-BorderStyle="None" FooterStyle-CssClass="t1_column_color2"/>--%>
                    <asp:BoundField DataField="propFees_Due" HeaderText="Fees Due"
                                    HeaderStyle-BorderStyle="None" HeaderStyle-VerticalAlign="Middle" HeaderStyle-CssClass="t1_column_color1" 
                                    ItemStyle-BorderStyle="None" ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="t1_column_color1"
                                    FooterStyle-BorderStyle="None" FooterStyle-CssClass="t1_column_color1"/>
                </Columns>
            </asp:GridView>
            </div>
        </td>
    </tr>
</table>
</asp:Panel>
<script language="javascript" type="text/javascript">

    function checkDate() {

        var strStartDateDay = document.getElementById('ctl00_ContentPlaceHolder1_ddlStartDateDay').value;
        var strStartDateMonth = document.getElementById('ctl00_ContentPlaceHolder1_ddlStartDateMonth').value;
        var strStartDateYear = document.getElementById('ctl00_ContentPlaceHolder1_ddlStartDateYear').value;

        var strEndDateDay = document.getElementById('ctl00_ContentPlaceHolder1_ddlEndDateDay').value;
        var strEndDateMonth = document.getElementById('ctl00_ContentPlaceHolder1_ddlEndDateMonth').value;
        var strEndDateYear = document.getElementById('ctl00_ContentPlaceHolder1_ddlEndDateYear').value;

        var strStartDate = strStartDateMonth + '/' + strStartDateDay + '/' + strStartDateYear;
        var strEndDate = strEndDateMonth + '/' + strEndDateDay + '/' + strEndDateYear;
        
        var dtStartDate = new Date(strStartDate);
        var dtEndDate = new Date(strEndDate);
        var today = new Date();

        if (dtStartDate > dtEndDate) {
            //something is wrong
            alert('Start date must not greater than End date!');
            return false;
        }
        else if (dtEndDate < dtStartDate) {
            alert('End date must not less than Start date!');
            return false;
        }
        else if (dtStartDate > today) {
            //something else is wrong
            alert('You cannot enter a date in the future!');
            return false;
        }
        else if (dtEndDate > today) {
            alert('You cannot enter a date in the future!');
            return false;
        }
        // if script gets this far through all of your fields
        // without problems, it's ok and you can submit the form
        return true;
    }

</script>
</asp:Content>