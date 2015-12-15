<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CustomerDatabaseSelector.ascx.cs" Inherits="DISConfigurationCloud.UserControls.CustomerDatabaseSelector" %>
<fieldset class="register">
    <legend>Customer Name</legend>
    <asp:DropDownList ID="DropDownListCustomers" runat="server" DataTextField="Name" DataValueField="ID" AutoPostBack="true" OnSelectedIndexChanged="DropDownListCustomers_SelectedIndexChanged" />
</fieldset>
<fieldset class="register">
    <legend>Databases of the Customer</legend>
    <asp:RadioButtonList ID="RadioButtonListCustomerDatabases" runat="server" DataTextField="DBConnectionString" DataValueField="ID" RepeatDirection="Vertical" RepeatLayout="Flow" AutoPostBack="true"  OnSelectedIndexChanged="RadioButtonListCustomerDatabases_SelectedIndexChanged"/>
</fieldset>