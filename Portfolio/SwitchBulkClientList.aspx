<%@ Page Title="" Language="C#" MasterPageFile="~/NAV.Master" AutoEventWireup="true" CodeBehind="SwitchBulkClientList.aspx.cs" Inherits="NAV.Portfolio.SwitchBulkClientList" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"></ajaxToolkit:ToolkitScriptManager>

<table border="0" align="left" cellpadding="0" cellspacing="0" width="100%">
<br />
<br />
<tr>&nbsp;</tr>
<tr>&nbsp;</tr>
<tr><td>
    <table  border="0" align="left" cellpadding="0" cellspacing="0" width="100%">
        <thead align="left">
            <tr>
                <td colspan="2">
                    <h2><asp:Label runat="server" ID="lblTitle_BulkSwitchList" Text="List of Portfolio in this Model"/></h2>
                </td>
            </tr>        
        </thead>
    </table>
    </td></tr>
    <tr>
        <td>
            <div id="Div2">
            <asp:GridView ID="gvClientListInModel" runat="server" AutoGenerateColumns="false" DataKeyNames="propClientID, propPortfolioID" Width="100%" ShowFooter="true" BorderStyle="None" CellPadding="0" CellSpacing="0" UseAccessibleHeader="true" CssClass="table1" EmptyDataText="No record found">
                <Columns>
                    <asp:TemplateField HeaderText="Switch ID" HeaderStyle-HorizontalAlign="Right" HeaderStyle-BorderStyle="None" FooterStyle-BorderStyle="None" HeaderStyle-VerticalAlign="Top">
                        <HeaderStyle CssClass="t1_column_color1" HorizontalAlign="Center" VerticalAlign="Middle" />
                        <ItemStyle CssClass="t1_column_color1" HorizontalAlign="Center" BorderStyle="None" />                        
                        <FooterStyle CssClass="t1_column_color1" BorderStyle="None" />
                        <HeaderTemplate><asp:LinkButton ID="lnkbtnSwitchID" runat="server" Text="Switch ID" CommandName="SwitchID" OnClick="SortClientList" ></asp:LinkButton></HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblSwitchID" runat="server" Text='<%#Eval("propSwitch.propSwitchID")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderStyle-HorizontalAlign="Right" HeaderStyle-BorderStyle="None" FooterStyle-BorderStyle="None" HeaderStyle-VerticalAlign="Top">
                        <HeaderStyle CssClass="t1_column_color2" HorizontalAlign="Center" VerticalAlign="Middle" />
                        <ItemStyle CssClass="t1_column_color2" HorizontalAlign="Left"  BorderStyle="None" />
                        <FooterStyle CssClass="t1_column_color2" BorderStyle="None" />
                        <HeaderTemplate><asp:LinkButton ID="lnkbtnClientName" runat="server" Text="Client Name" CommandName="ClientName" OnClick="SortClientList"></asp:LinkButton></HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label ID="gvBulkSwitchListLabelClientName" runat="server" Text='<%#Eval("propClient.propForename") + " " + Eval("propClient.propSurname") %>' ></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Portfolio Name" HeaderStyle-HorizontalAlign="Right" HeaderStyle-BorderStyle="None" FooterStyle-BorderStyle="None" HeaderStyle-VerticalAlign="Top">
                        <HeaderStyle CssClass="t1_column_color1" HorizontalAlign="Center" VerticalAlign="Middle" />
                        <ItemStyle CssClass="t1_column_color1"  HorizontalAlign="Left" BorderStyle="None" />                        
                        <FooterStyle CssClass="t1_column_color1" BorderStyle="None" />
                        <HeaderTemplate><asp:LinkButton ID="lnkbtnPortfolioName" runat="server" Text="Portfolio Name" CommandName="PortfolioName" OnClick="SortClientList"></asp:LinkButton></HeaderTemplate>
                        <ItemTemplate>
                                <asp:Label ID="gvBulkSwitchListLabelPortfolioName" runat="server" Text='<%#Eval("propCompany")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Discretionary" HeaderStyle-HorizontalAlign="Right" HeaderStyle-BorderStyle="None" FooterStyle-BorderStyle="None" HeaderStyle-VerticalAlign="Top">
                        <HeaderStyle CssClass="t1_column_color2" VerticalAlign="Middle" HorizontalAlign="Center" />
                        <ItemStyle CssClass="t1_column_color2" HorizontalAlign="Center" BorderStyle="None" />
                        <FooterStyle CssClass="t1_column_color2" BorderStyle="None" />
                        <HeaderTemplate><asp:LinkButton ID="lnkbtnDiscretionary" runat="server" Text="Discretionary" CommandName="Discretionary" OnClick="SortClientList"></asp:LinkButton></HeaderTemplate>
                        <ItemTemplate><asp:Label ID="lblDiscretionary" runat="server" Text='<%#int.Parse((Eval("propMFPercent")).ToString()) > 0 ? "Yes": "No" %>' /></ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Customized" HeaderStyle-HorizontalAlign="Right" HeaderStyle-BorderStyle="None" FooterStyle-BorderStyle="None" HeaderStyle-VerticalAlign="Top">
                        <HeaderStyle CssClass="t1_column_color1" HorizontalAlign="Center" VerticalAlign="Middle" />
                        <ItemStyle CssClass="t1_column_color1" HorizontalAlign="Center" BorderStyle="None" />
                        <FooterStyle CssClass="t1_column_color1" BorderStyle="None" />
                        <HeaderTemplate><asp:LinkButton ID="lnkbtnCustomized" runat="server" Text="Customized" CommandName="Customized" OnClick="SortClientList"></asp:LinkButton></HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label runat="server" ID="lblCustomized" Text='<%#((int)Eval("propSwitchTemp.propModelID")) > 0 ? "Yes": "No" %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderStyle-HorizontalAlign="Right" HeaderStyle-BorderStyle="None" FooterStyle-BorderStyle="None" HeaderStyle-VerticalAlign="Top">
                        <HeaderStyle CssClass="t1_column_color2" VerticalAlign="Middle" HorizontalAlign="Center"/>
                        <ItemStyle CssClass="t1_column_color2" HorizontalAlign="Center" BorderStyle="None" />  
                        <FooterStyle CssClass="t1_column_color2" BorderStyle="None" />
                        <ItemTemplate>
                            <asp:LinkButton ID="lbtnModify" runat="server" Text="Modify" PostBackUrl='<%# String.Format("~/Portfolio/SwitchBulkCustomized.aspx?MID={0}&MGID={1}&MPID={2}&CID={3}&PID={4}", Eval("propModelID"), Eval("propModelGroupID"), Eval("propModelPortfolioID"), Eval("propClientID"), Eval("propPortfolioID"))%>' ></asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Selected" HeaderStyle-HorizontalAlign="Right" HeaderStyle-BorderStyle="None" FooterStyle-BorderStyle="None" HeaderStyle-VerticalAlign="Top">
                        <HeaderStyle CssClass="t1_column_color1" VerticalAlign="Middle" HorizontalAlign="Center" />
                        <ItemStyle CssClass="t1_column_color1" HorizontalAlign="Right"  BorderStyle="None" />
                        <FooterStyle CssClass="t1_column_color1" BorderStyle="None" />
                        <ItemTemplate><asp:CheckBox ID="cboxSelected" runat="server" /></ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
            </div>
        </td>
    </tr>
    <tr><td>&nbsp;</td></tr>
    <tr><td><div><center>
                        <asp:Button ID="btnPreviousPage" runat="server" Text="Back" OnClick="btnPreviousPage_Click" />
                        &nbsp;
                        <asp:Button ID="btnBulkSwitch" runat="server" Text="Bulk Switch" OnClick="btnBulkSwitch_Click" OnClientClick="return check_sel('ctl00_ContentPlaceHolder1_gvClientListInModel')" />
    </center></div></td></tr>
</table>

<div>
    <!-- Keep this control at the last to hide it, this will make validation of popup   -->
    <asp:Button ID="btnTest" runat="server"  Text="" style="display:none;" />
    
    <ajaxToolkit:ModalPopupExtender ID="mpeClientPopup" runat="server" BehaviorID="bhvrClientPopup" TargetControlID="btnTest" PopupControlID="panelClientPopup" CancelControlID="btnCancelBulkSwitch" BackgroundCssClass="modalBackground" DropShadow="true" />
    <asp:Panel ID="panelClientPopup" runat="server" style="display:none; width:350px" CssClass="modalBox" >
        <div style="text-align:right; width:100%; margin-top:5px;">
            <asp:GridView runat="server" ID="gvClientAndMobileNumberList" AutoGenerateColumns="false" DataKeyNames="propClientID" Width="100%" BorderStyle="Solid" BorderWidth="1">
                <Columns>
                    <asp:TemplateField HeaderText="Client Name" HeaderStyle-HorizontalAlign="Right" HeaderStyle-BorderStyle="None" FooterStyle-BorderStyle="None" HeaderStyle-VerticalAlign="Top" >
                        <HeaderStyle CssClass="t1_column_color1" HorizontalAlign="Center" VerticalAlign="Middle" />
                        <ItemStyle CssClass="t1_column_color1" HorizontalAlign="Left"  BorderStyle="None" />
                        <FooterStyle CssClass="t1_column_color1" BorderStyle="None" />
                        <ItemTemplate>
                            <asp:Label ID="gvClientAndMobileNumberListLabelClientName" runat="server" Text='<%#Eval("propForename") + " " + Eval("propSurname") %>' ></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Mobile Number">
                        <HeaderStyle CssClass="t1_column_color2" HorizontalAlign="Center" BorderStyle="None" VerticalAlign="Middle" />
                        <ItemStyle CssClass="t1_column_color2" HorizontalAlign="Left" BorderStyle="None"  />                        
                        <FooterStyle CssClass="t1_column_color2" BorderStyle="None" />
                        <ItemTemplate>
                            <asp:TextBox runat="server" ID="txtMobileNumber" Text='<%#Eval("propMobileNumber")%>' onkeypress="javascript:return onlyNumbers(this,event);"></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
        <div>
            <center>
                <asp:Button runat="server" ID="btnProceedBulkSwitch" Text="Proceed" OnClick="btnProceedBulkSwitch_Click" />
                &nbsp;
                <asp:Button runat="server" ID="btnCancelBulkSwitch" Text="Cancel" />
            </center>
        </div>
    </asp:Panel>
    <asp:Panel runat="server" ID="panelSwitchPopup3" style="display:none;" CssClass="modalBox" Width="350px">
        <div style="text-align:left;width:100%;margin-top:1px;">
        <h4>Portfolio Description</h4>
            <table width="100%">
                <tr><td><asp:TextBox ID="txtDescription" runat="server" TextMode="MultiLine" CssClass="multilineTextbox" Rows="10" width="95%"></asp:TextBox></td></tr>
                <tr><td>&nbsp;</td></tr>
                <tr><td><asp:Label ID="lblSMSMobileNo" runat="server" Text="Mobile no.:" Width="35%"></asp:Label><asp:TextBox runat="server" ID="txtSMSMobileNo" width="60%" ></asp:TextBox></td></tr>
            </table>
        <center>
            <br />
            <asp:Button runat="server" ID="btnSwitchSendSave" Text="Save" OnClick="btnSwitchSendSave_Click" />&nbsp;&nbsp;
            <asp:Button runat="server" ID="btnSwitchSendCancel" Text="Cancel" />
            <br />
        </center>
        </div>
    </asp:Panel>
</div>
<script type="text/javascript">

    function onlyNumbers(sender, e) {
        if (window.event)
            charCode = window.event.keyCode; // IE
        else
            charCode = e.which; // Firefox
        if (charCode == 8 || charCode == 0) {
            return true;
        }
        if ((!(charCode > 47 && charCode < 58)) && charCode != 46) { return false; }

        if (charCode == 46) {
            if ((count(sender.value, '\\.') == 1) || sender.value == '') {
                return false;
            }
        }
        return true;
    }
    function check_sel(gridId) {
        var grid = document.getElementById(gridId); // GridView Name
        var gridElements = grid.getElementsByTagName("input");
        count = 0;
        for (var i = 0; i < gridElements.length; i++) {
            var e = gridElements[i];
            if (e.type == "checkbox" && e.checked == true && e.id.substring(e.id.lastIndexOf('_') + 1, e.id.length) == 'cboxSelected') // Your Checkbox Control id which is within Gridview
            {
                count += 1;
            }
        }
        if (count == 0) {
            alert('Please select at least one to proceed Bulk Switch');
            return false;
        }
        else {
            return true;
            //$find('bhvrClientPopup', null).show();
        }
    }
    function showhidePopup(mpe) {
        var popup = $find(mpe);
        var viewState = '<%=ViewState["HasDiscretionary"]%>'
        var hasDiscretionary = new Boolean(Number(viewState));
        if (hasDiscretionary == true) {
            $find('bhvrSwitchPopup3', null).show();
            popup.hide();
        }
        else {
            $find('bhvrSwitchPopup2', null).show();
            popup.hide();
        }
    }
</script>
</asp:Content>
