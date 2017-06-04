<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="NewDatabase.aspx.cs" Inherits="DISConfigurationCloud.NewDatabase" %>

<%@ Register Src="~/UserControls/DBCreator.ascx" TagPrefix="uc1" TagName="DBCreator" %>

<asp:Content ID="HeaderContent" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <h2>
        Create a New Database
    </h2>
    <p>
        Use the form below to create a new database
    </p>
    <div>
      <fieldset class="register">
          <uc1:DBCreator runat="server" ID="DBCreator" />
      </fieldset>
    </div>
</asp:Content>
