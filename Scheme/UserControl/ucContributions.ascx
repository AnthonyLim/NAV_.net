<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucContributions.ascx.cs" Inherits="NAV.Scheme.UserControl.ucContributions" %>
<table  border="0" align="left" cellpadding="0" cellspacing="0" width="100%">
    <tr>
        <td  class="table1_header" align="left">           
            <b><asp:Label runat="server" ID="lblTitle_Contributions" Text="Contributions"/></b>
        </td>
    </tr>
    <tr>
        <td>
            <div id="table1_container">
            <asp:GridView runat="server" ID="gvContributions" AutoGenerateColumns="false" Width="100%" ShowFooter="true" BorderStyle="None" CellPadding="0" CellSpacing="0" UseAccessibleHeader="true" CssClass="table1">
                <Columns>
                    <asp:TemplateField HeaderText="Start Date" HeaderStyle-HorizontalAlign="Center" HeaderStyle-BorderStyle="None" FooterStyle-BorderStyle="None" HeaderStyle-VerticalAlign="Top">
                        <ItemStyle HorizontalAlign="Center"  BorderStyle="None" CssClass="t1_column_color2" />
                        <HeaderStyle CssClass="t1_column_color2" VerticalAlign="Middle" HorizontalAlign="Center" />
                        <FooterStyle BorderStyle="None" CssClass="t1_column_color2"/>
                        <ItemTemplate>
                            <asp:Label ID="lblStartDate" runat="server"  htmlencode="false" text='<%# CheckNull(Eval("propStartDate", "{0:dd/MM/yyyy}"))%>'  />
                        </ItemTemplate>                        
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="End Date" HeaderStyle-HorizontalAlign="Right" HeaderStyle-BorderStyle="None" FooterStyle-BorderStyle="None" HeaderStyle-VerticalAlign="Top">
                        <ItemStyle HorizontalAlign="Center"  BorderStyle="None" CssClass="t1_column_color1" />
                        <HeaderStyle CssClass="t1_column_color1" VerticalAlign="Middle" HorizontalAlign="Center" />
                        <FooterStyle BorderStyle="None" CssClass="t1_column_color1"/>
                        <ItemTemplate>                            
                            <asp:Label ID="lblEndDate" runat="server" htmlencode="false" text='<%# CheckNull(Eval("propEndDate", "{0:dd/MM/yyyy}"))%>'  />
                        </ItemTemplate>                        
                    </asp:TemplateField>
                    <asp:BoundField DataField="propValuationFrequency" HtmlEncode="false"
                                    HeaderText="Units" HeaderStyle-HorizontalAlign="Center" HeaderStyle-BorderStyle="None" HeaderStyle-VerticalAlign="Middle" HeaderStyle-CssClass="t1_column_color2"
                                    ItemStyle-HorizontalAlign="Center" ItemStyle-BorderStyle="None" ItemStyle-CssClass="t1_column_color2"
                                    FooterStyle-BorderStyle="None" FooterStyle-CssClass="t1_column_color2"/>
                    <asp:TemplateField HeaderText="Amount" HeaderStyle-HorizontalAlign="Right" HeaderStyle-BorderStyle="None" FooterStyle-BorderStyle="None" HeaderStyle-VerticalAlign="Top">
                        <ItemStyle HorizontalAlign="Right"  BorderStyle="None" CssClass="t1_column_color1" />
                        <HeaderStyle CssClass="t1_column_color1" VerticalAlign="Middle" HorizontalAlign="Center" />
                        <FooterStyle BorderStyle="None" CssClass="t1_column_color1"/>
                        <ItemTemplate>                            
                            <asp:Label runat="server" ID="gvDetailsValueFundCur" text='<%# Eval("propContributionAmount", "{0:n2}") %>'/>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
            </div>
        </td>
    </tr>
</table>