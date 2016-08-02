<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="BackupDatabase.aspx.cs" Inherits="DISConfigurationCloud.DatabaseBackupWizard" %>

<%@ Register Src="~/UserControls/CustomerDatabaseSelector.ascx" TagPrefix="uc1" TagName="CustomerDatabaseSelector" %>

<asp:Content ID="HeaderContent" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <h2>
        Backup a Database
    </h2>
    <p>
        Choose a customer from the drop down list below, and then select a database to backup
    </p>
    <div>
        <uc1:CustomerDatabaseSelector runat="server" ID="CustomerDatabaseSelector" />
        <p class="submitButton">
            <asp:Button ID="ButtonBackupDatabase" runat="server" Text="Backup" OnClick="ButtonBackupDatabase_Click" /> 
        </p>
    </div>
</asp:Content>
