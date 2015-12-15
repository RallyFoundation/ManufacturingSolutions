<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="BackupRecords.aspx.cs" Inherits="DISConfigurationCloud.BackupRecords" %>

<%@ Register Src="~/UserControls/DBBackupDetail.ascx" TagPrefix="uc1" TagName="DBBackupDetail" %>

<asp:Content ID="HeaderContent" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <h2>
        Backup Records
    </h2>
    <p>
        
    </p>
    <div>
        <fieldset class="register">
            <legend>Backup Records</legend>
            <uc1:DBBackupDetail runat="server" ID="DBBackupDetail" IsAllowingSelection="false" />
        </fieldset>
    </div>
</asp:Content>
