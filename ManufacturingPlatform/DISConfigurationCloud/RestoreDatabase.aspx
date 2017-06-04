<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="RestoreDatabase.aspx.cs" Inherits="DISConfigurationCloud.RestoreDatabase" %>

<%@ Register Src="~/UserControls/CustomerDatabaseSelector.ascx" TagPrefix="uc1" TagName="CustomerDatabaseSelector" %>
<%@ Register Src="~/UserControls/DBBackupDetail.ascx" TagPrefix="uc1" TagName="DBBackupDetail" %>


<asp:Content ID="HeaderContent" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <h2>
        Restore a Database
    </h2>
    <p>
        Choose a customer from the drop down list below, and then select a database to restore
    </p>
    <div>
        <uc1:CustomerDatabaseSelector runat="server" ID="CustomerDatabaseSelector" OnValueChanged="CustomerDatabaseSelector_ValueChanged"/>
        <fieldset class="register">
            <legend>Backup Records</legend>
            <uc1:DBBackupDetail runat="server" ID="DBBackupDetail" IsAllowingSelection="true" />
        </fieldset>
        <p class="submitButton">
            <asp:Button ID="ButtonRestoreDatabase" runat="server" Text="Restore" OnClick="ButtonRestoreDatabase_Click" /> 
        </p>
    </div>
</asp:Content>
