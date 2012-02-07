<%@ Page Title="" Language="C#" MasterPageFile="~/NAV.Master" AutoEventWireup="true" CodeBehind="SwitchFeeReportList.aspx.cs" Inherits="NAV.Admin.SwitchFeeReportList" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<br />&nbsp;
<br />&nbsp;
<table class='main_info' border="0" cellpadding="0" cellspacing="0" width="100%">
    <thead align="left">
        <tr>
            <td colspan="2">
                <h2><asp:Label runat="server" ID="lblTitleValue" Text="Switch List Report"/></h2>
            </td>
        </tr>        
    </thead>
</table>
<table border="0" align="left" cellpadding="0" cellspacing="0" width="100%">
<tr>
    <td>
        <div id="table1_container">
            <asp:GridView ID="gvSwitchListReport" runat="server" AutoGenerateColumns="false" DataKeyNames="propPortfolioID" Width="100%" ShowFooter="true" BorderStyle="None" CellPadding="0" CellSpacing="0" UseAccessibleHeader="true" CssClass="table1" EmptyDataText="No record found">
                <Columns>
                    <asp:TemplateField HeaderText="Switch ID" HeaderStyle-HorizontalAlign="Right" HeaderStyle-BorderStyle="None" FooterStyle-BorderStyle="None" HeaderStyle-VerticalAlign="Top">
                        <HeaderStyle VerticalAlign="Middle" HorizontalAlign="Center" CssClass="t1_column_color1" />
                        <ItemStyle HorizontalAlign="Center" BorderStyle="None" CssClass="t1_column_color1" />
                            <ItemTemplate>
                                <asp:Label ID="gvItemLabelSwitchID" runat="server" Text='<%#Eval("propSwitchID") %>' ></asp:Label>
                            </ItemTemplate>
                        <FooterStyle BorderStyle="None" CssClass="t1_column_color1"/>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Client Name" HeaderStyle-HorizontalAlign="Right" HeaderStyle-BorderStyle="None" FooterStyle-BorderStyle="None" HeaderStyle-VerticalAlign="Top">
                        <HeaderStyle VerticalAlign="Middle" HorizontalAlign="Center" CssClass="t1_column_color2" />
                        <ItemStyle HorizontalAlign="Left"  BorderStyle="None" CssClass="t1_column_color2" />
                            <ItemTemplate>
                                <asp:Label ID="gvItemLabelForename" runat="server" Text='<%#Eval("propSwitchClient.propForename")%>' ></asp:Label>&nbsp;
                                <asp:Label ID="gvItemLabelSurname" runat="server" Text='<%#Eval("propSwitchClient.propSurname")%>'></asp:Label>
                            </ItemTemplate>
                        <FooterStyle BorderStyle="None" CssClass="t1_column_color2"/>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Portfolio Name" HeaderStyle-HorizontalAlign="Right" HeaderStyle-BorderStyle="None" FooterStyle-BorderStyle="None" HeaderStyle-VerticalAlign="Top">
                        <HeaderStyle VerticalAlign="Middle" HorizontalAlign="Center" CssClass="t1_column_color1"/>
                        <ItemStyle HorizontalAlign="Left" BorderStyle="None" CssClass="t1_column_color1" />
                            <ItemTemplate>
                                <asp:Label ID="gvItemLabelPortfolioName" runat="server" Text='<%#Eval("propPortfolio.propCompany")%>'></asp:Label>
                            </ItemTemplate>
                        <FooterStyle BorderStyle="None" CssClass="t1_column_color1"/>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Approved Date" HeaderStyle-HorizontalAlign="Right" HeaderStyle-BorderStyle="None" FooterStyle-BorderStyle="None" HeaderStyle-VerticalAlign="Top">
                        <HeaderStyle VerticalAlign="Middle" HorizontalAlign="Center" CssClass="t1_column_color2" />
                        <ItemStyle HorizontalAlign="Right"  BorderStyle="None" CssClass="t1_column_color2" />
                            <ItemTemplate>
                                <asp:Label runat="server" ID="gvItemLabelApprovedDate" Text='<%#Eval("propDate_Created", "{0:dd/MM/yyyy}")%>'></asp:Label>
                            </ItemTemplate>
                        <FooterStyle BorderStyle="None" CssClass="t1_column_color2"/>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </td>
</tr>
</table>
<div align="left">
<asp:Button ID="btnPrevious" runat="server" Text="Back" OnClick="btnPrevious_Click" />
</div>
</asp:Content>
