<%@ Page Title="" Language="C#" MasterPageFile="~/NAV.Master" AutoEventWireup="true" CodeBehind="SwitchList.aspx.cs" Inherits="NAV.SwitchList" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<br />&nbsp;
<br />&nbsp;
<table border="0" align="left" cellpadding="2" cellspacing="0" width="50%">
    <tbody class="member_info">
        <tr><td>&nbsp;</td></tr>
        <tr>
            <td style="width:1%" nowrap="nowrap" align="left"><asp:Label ID="lblSearchStatus" runat="server" Text="Status"></asp:Label></td>
            <td style="width:1%" nowrap="nowrap" align="left"><asp:DropDownList ID="ddlSearchStatus" runat="server"></asp:DropDownList></td>
        </tr>
        <tr>
            <td style="width:1%" nowrap="nowrap" align="left"><asp:Label ID="lblSearchClient" runat="server" Text="Client"></asp:Label></td>
            <td style="width:1%" nowrap="nowrap" align="left"><asp:TextBox ID="txtSearchClient" runat="server"></asp:TextBox></td>
        </tr>
        <tr>
            <td style="width:1%" nowrap="nowrap" align="left"><asp:Label ID="lblSearchInsuranceCompany" runat="server" Text="Insurance Company"></asp:Label></td>
            <td style="width:1%" nowrap="nowrap" align="left"><asp:TextBox ID="txtSearchInsuranceCompany" runat="server"></asp:TextBox></td>
        </tr>
        <tr>
            <td style="width:1%" nowrap="nowrap" align="left"><asp:Label ID="lblDateRange" runat="server" Text="Date Range:"></asp:Label></td>
            <td style="width:1%" nowrap="nowrap" align="left">
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
        <tr>
            <td>&nbsp;</td>
        </tr>
        <tr><td><asp:Button ID="btnSearch" runat="server" Text="Search" OnClick="btnSearch_Click" /></td></tr>
    </tbody>
</table>
<table border="0" align="left" cellpadding="0" cellspacing="0" width="100%">
<tr><td>&nbsp;</td></tr>
    <tr>
        <td>
            <div id="table1_container">
            <asp:GridView ID="gvSwitchList" runat="server" AutoGenerateColumns="false" DataKeyNames="propPortfolioID" Width="100%" ShowFooter="true" BorderStyle="None" CellPadding="0" CellSpacing="0" UseAccessibleHeader="true" CssClass="table1" EmptyDataText="No record found">
                <Columns>
                    <asp:TemplateField HeaderText="Switch ID" HeaderStyle-HorizontalAlign="Right" HeaderStyle-BorderStyle="None" FooterStyle-BorderStyle="None" HeaderStyle-VerticalAlign="Top">
                        <HeaderStyle CssClass="t1_column_color1" VerticalAlign="Middle" HorizontalAlign="Center"/>
                        <ItemStyle HorizontalAlign="Center" BorderStyle="None" CssClass="t1_column_color1" />                        
                        <FooterStyle BorderStyle="None" CssClass="t1_column_color1"/>
                        <ItemTemplate>
                            <asp:LinkButton ID="lnkbtnGVCodeResetSwitchID" runat="server" Text='<%#Eval("propSwitchID")%>' PostBackUrl='<%# String.Format("~/Portfolio/SwitchListDetails.aspx?SwitchID={0}&ClientID={1}&PortfolioID={2}&PortfolioForename={3}&PortfolioSurname={4}", Eval("propSwitchID"), Eval("propClientID"), Eval("propPortfolioID"), Eval("propSwitchClient.propForename"), Eval("propSwitchClient.propSurname"))%>' ></asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Client Name" HeaderStyle-HorizontalAlign="Right" HeaderStyle-BorderStyle="None" FooterStyle-BorderStyle="None" HeaderStyle-VerticalAlign="Top">
                        <HeaderStyle CssClass="t1_column_color2" VerticalAlign="Middle" HorizontalAlign="Center" />
                        <ItemStyle HorizontalAlign="Left"  BorderStyle="None" CssClass="t1_column_color2" />
                        <FooterStyle BorderStyle="None" CssClass="t1_column_color2"/>
                        <ItemTemplate>
                            <asp:Label ID="lblGVCodeResetForename" runat="server" Text='<%#Eval("propSwitchClient.propForename")%>' ></asp:Label>&nbsp;
                            <asp:Label ID="lblGVCodeResetSurname" runat="server" Text='<%#Eval("propSwitchClient.propSurname")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Portfolio Name" HeaderStyle-HorizontalAlign="Right" HeaderStyle-BorderStyle="None" FooterStyle-BorderStyle="None" HeaderStyle-VerticalAlign="Top">
                        <HeaderStyle CssClass="t1_column_color1" VerticalAlign="Middle" HorizontalAlign="Center"/>
                        <ItemStyle HorizontalAlign="Left" BorderStyle="None" CssClass="t1_column_color1" />                        
                        <FooterStyle BorderStyle="None" CssClass="t1_column_color1"/>
                        <ItemTemplate><asp:Label ID="lblGVCodeResetPortfolioName" runat="server" Text='<%#Eval("propPortfolio.propCompany")%>'></asp:Label></ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Created Date" HeaderStyle-HorizontalAlign="Right" HeaderStyle-BorderStyle="None" FooterStyle-BorderStyle="None" HeaderStyle-VerticalAlign="Top">
                        <HeaderStyle CssClass="t1_column_color2" VerticalAlign="Middle" HorizontalAlign="Center" />
                        <ItemStyle HorizontalAlign="Right"  BorderStyle="None" CssClass="t1_column_color2" />
                        <FooterStyle BorderStyle="None" CssClass="t1_column_color2"/>
                        <ItemTemplate>
                            <asp:Label runat="server" ID="gvSwitchListLabelCreatedDate" Text='<%#Eval("propDate_Created", "{0:dd/MM/yyyy}")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Switch Status" HeaderStyle-HorizontalAlign="Right" HeaderStyle-BorderStyle="None" FooterStyle-BorderStyle="None" HeaderStyle-VerticalAlign="Top">
                        <HeaderStyle CssClass="t1_column_color1" VerticalAlign="Middle" HorizontalAlign="Center"/>
                        <ItemStyle HorizontalAlign="Center" BorderStyle="None" CssClass="t1_column_color1" />                        
                        <FooterStyle BorderStyle="None" CssClass="t1_column_color1"/>
                        <ItemTemplate>
                            <asp:Label runat="server" ID="gvSwitchListLabelStatus" Text='<%#Eval("propStatusString")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderStyle-HorizontalAlign="Right" HeaderStyle-BorderStyle="None" FooterStyle-BorderStyle="None" HeaderStyle-VerticalAlign="Top">
                        <HeaderStyle CssClass="t1_column_color2" VerticalAlign="Middle" HorizontalAlign="Center"/>
                        <ItemStyle HorizontalAlign="Center" BorderStyle="None" CssClass="t1_column_color2" />  
                        <FooterStyle BorderStyle="None" CssClass="t1_column_color2"/>
                        <ItemTemplate>
                            <asp:LinkButton ID="lnkbtnGVHistory" runat="server" Text="History" PostBackUrl='<%# String.Format("~/Portfolio/SwitchHistory.aspx?SID={0}&CID={1}&PID={2}&SourcePage={3}", Eval("propSwitchID"), Eval("propClientID"), Eval("propPortfolioID"), "/ASPX/Portfolio/SwitchList.aspx")%>' ></asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderStyle-HorizontalAlign="Right" HeaderStyle-BorderStyle="None" FooterStyle-BorderStyle="None" HeaderStyle-VerticalAlign="Top">
                        <HeaderStyle CssClass="t1_column_color1" VerticalAlign="Middle" HorizontalAlign="Center" />
                        <ItemStyle HorizontalAlign="Right"  BorderStyle="None" CssClass="t1_column_color1" />
                        <FooterStyle BorderStyle="None" CssClass="t1_column_color1"/>
                        <ItemTemplate><asp:CheckBox ID="gvSwitchListCheckbox" runat="server" Visible='<%# bool.Parse(isCancelCheckBoxVisible(int.Parse(Eval("propStatus").ToString())).ToString().ToLower())%>'/></ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="propClientID" InsertVisible="true" Visible="false"/>
                    <asp:BoundField DataField="propPortfolioID" InsertVisible="true" Visible="false" />
                </Columns>
            </asp:GridView>
            </div>
        </td>
    </tr>
    <tr><td><div><center><asp:Button ID="btnSwitchCancel" runat="server" Text="Cancel Selected Switches" OnClick="btnSwitchCancel_Click" OnClientClick="return check_sel()" /></center></div></td></tr>
    <tr><td><asp:Button ID="btnPrevious" runat="server" Text="Back" OnClick="btnPrevious_Click" Visible="false" /></td></tr>
</table>
<script type="text/javascript">
    function check_sel() {
        //var loTable = document.all("ctl00_ContentPlaceHolder1_gvSwitchList"); // GridView Name
        count = 0;
        with (document.forms[0]) {
            for (var i = 0; i < elements.length; i++) {
                var e = elements[i];
                if (e.type == "checkbox" && e.checked == true && e.id.substring(e.id.lastIndexOf('_') + 1, e.id.length) == 'gvSwitchListCheckbox') // Your Checkbox Control id which is within Gridview
                {
                    count += 1;
                }
            }
        }
        if (count == 1) {
            return confirm('You are about to cancel ' + count + ' switch, are you sure you want to proceed?');
        }
        else if (count > 1) {
            return confirm('You are about to cancel ' + count + ' switches, are you sure you want to proceed?');
        }
        return false;
    }
</script>
</asp:Content>
