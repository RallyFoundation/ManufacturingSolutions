<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DBCreator.ascx.cs" Inherits="DISConfigurationCloud.UserControls.DBCreator" %>

<table>
    <tr>
        <td class="auto-style1">Server/Instance Name: </td>
        <td class="auto-style1">
            <asp:TextBox ID="TextBoxInstance" runat="server" CssClass="textEntry"/>
            <asp:RequiredFieldValidator ID="RFVTextBoxInstance" runat="server" ControlToValidate="TextBoxInstance" Display="Dynamic" CssClass="failureNotification" ErrorMessage="Server/Instance Name is required." ToolTip="Server/Instance Name is required.">*</asp:RequiredFieldValidator>
            <%--<asp:RegularExpressionValidator ID="REVTextBoxInstance" runat="server" ControlToValidate="TextBoxInstance" Display="Dynamic" CssClass="failureNotification" ValidationExpression="^[^(localhost)]" ErrorMessage="Server/Instance Name should not be starting with loop back address!" ToolTip="Server/Instance Name should not be starting with loop back address!" Text="!" />--%>
        </td>
        <td>
            <asp:HyperLink ID="HyperLinkHelpInstance" runat="server" Text="What's this?..." NavigateUrl="~/About.aspx" />
        </td>
    </tr>
    <tr>
        <td class="auto-style1">DB Server Login Name: </td>
        <td class="auto-style1">
            <asp:TextBox ID="TextBoxLoginName" runat="server" CssClass="textEntry" />
            <asp:RequiredFieldValidator ID="RFVTextBoxLoginName" runat="server" ControlToValidate="TextBoxLoginName" Display="Dynamic" CssClass="failureNotification" ErrorMessage="Database Login Name is required." ToolTip="Database Login Name is required.">*</asp:RequiredFieldValidator>
        </td>
        <td>
            <asp:HyperLink ID="HyperLinkLoginName" runat="server" Text="What's this?..." NavigateUrl="~/About.aspx"/>
        </td>
    </tr>
    <tr>
        <td class="auto-style1">DB Server Password: </td>
        <td class="auto-style1">
            <asp:TextBox ID="TextBoxPassword" runat="server" CssClass="textEntry"  />
            <asp:RequiredFieldValidator ID="RFVTextBoxPassword" runat="server" ControlToValidate="TextBoxPassword" Display="Dynamic" CssClass="failureNotification" ErrorMessage="Password cannot be empty." ToolTip="Password cannot be empty.">*</asp:RequiredFieldValidator>
        </td>
        <td>
            
        </td>
    </tr>
    <tr>
        <td class="auto-style1">Database Name: </td>
        <td class="auto-style1">
            <asp:TextBox ID="TextBoxDatabaseName" runat="server" CssClass="textEntry" />
            <asp:RequiredFieldValidator ID="RFVTextBoxDatabaseName" runat="server" ControlToValidate="TextBoxDatabaseName" Display="Dynamic" CssClass="failureNotification" ErrorMessage="Database Name cannot be empty." ToolTip="Database Name cannot be empty.">*</asp:RequiredFieldValidator>
            <asp:RegularExpressionValidator ID="REVTextBoxDatabaseName" runat="server" ControlToValidate="TextBoxDatabaseName" Display="Dynamic" CssClass="failureNotification" ValidationExpression="[a-zA-Z0-9_]{2,50}" ErrorMessage="Database Name should only be composed of alpha, numberic as well as the underscore ('_') characters, and is limited to 2-50 characters!" Text="!" />
        </td>
        <td>
            <asp:Button ID="BottonCreateDB" runat="server" Text="Create" OnClick="BottonCreateDB_Click" />
        </td>
    </tr>
</table>
<asp:ValidationSummary ID="validationSummary" runat="server" CssClass="failureNotification" DisplayMode="BulletList" ShowMessageBox="true" ShowSummary="false" HeaderText="The following error(s) occurred in your inputs, please double check and try again:" />