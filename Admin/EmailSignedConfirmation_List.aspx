<%@ Page Title="" Language="C#" MasterPageFile="~/NAV.Master" AutoEventWireup="true" CodeBehind="EmailSignedConfirmation_List.aspx.cs" Inherits="NAV.Admin.EmailSignedConfirmation_List" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
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
        table#<%=gvList.ClientID%>
        {
            border-style:None;
            width:100%;
            border-collapse:collapse;
        }
        table#<%=gvList.ClientID%> td,table#<%=gvList.ClientID%> th
        {
            border-style:None;
            vertical-align:middle;
            padding-top:0.8em;
            padding-bottom:0.6em;
        }
        table#<%=gvList.ClientID%> th
        {
            color:#00135D;
        }
        h3#page-heading
        {
            clear:both;
            padding-bottom:20px;
            color:#00135D;
            font-size:150%;
        }
    </style>
    <script type="text/javascript" src="https://www.google.com/jsapi?key=ABQIAAAAJIc7LVNGdtica6PhZe_sFRQqgDN7nsaPpAk-csjZoyzusnQgwBSfA71pi0A-I91r0MmWO8okG_Zq6Q"></script>
    <script type="text/javascript">
        google.load("jquery", "1.7.1");
        // google.load("jqueryui", "1.8.1");
    </script>
    <script>
        $ = jQuery.noConflict();
        $(function() {
            $('#<%=gvList.ClientID %> input[type=checkbox]').click(
                function(event) {
                    //event.preventDefault();
                    $this = $(this);
                    if ($this.is(':checked')) {
                        setStatus($this, 'checked');
                    } else {
                        setStatus($this, 'unchecked');
                    }
                }
            );
            var setStatus = function(jObject, status) {
                status = status == 'checked' ? 'checked' : 'unchecked';
                $this = $(this);
                $.post(
                    $('form:first').attr('action'),
                    $('form:first').serialize() + '&cbox=' + jObject.parents('span').attr('class') + '&status=' + status,
                    function(data, status) {
                        console.log(status);
                    }
                );
            }
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<br />&nbsp;
<br />
<br />
<h3 id="page-heading">Signed Confirmation Requirement</h3>
<div id="table1_container">
    <asp:GridView runat="server" BorderStyle="None" ID="gvList" AutoGenerateColumns="false" >
        <Columns>
            <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="t1_column_color1" HeaderStyle-Width="20%" HeaderStyle-CssClass="t1_column_color1" HeaderText="Requires Signed Confirmation">
                <ItemTemplate>
                    <asp:CheckBox runat="server" ID="cbHasSignedConfirmation" Checked='<%# Eval("propSignedConfirmation") %>' CssClass='<%# Eval("propCompanyID") %>' />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="propCompany" ItemStyle-CssClass="t1_column_color2" HeaderStyle-Width="80%" HeaderStyle-CssClass="t1_column_color2" HeaderText="Company" />
        </Columns>    
    </asp:GridView>
</div>
<br />&nbsp;
<br />
</asp:Content>