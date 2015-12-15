<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DBConnectionEditor.ascx.cs" Inherits="DISConfigurationCloud.UserControls.DBConnectionEditor" %>

<table>
    <tr>
        <td class="auto-style1">Server/Instance Name: </td>
        <td class="auto-style1">
            <asp:TextBox ID="TextBoxInstance" runat="server" CssClass="textEntry"/>
            <asp:RequiredFieldValidator ID="RFVTextBoxInstance" runat="server" ControlToValidate="TextBoxInstance" Display="Dynamic" CssClass="failureNotification" ErrorMessage="Server/Instance Name is required." ToolTip="Server/Instance Name is required.">*</asp:RequiredFieldValidator>
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
            <asp:Button ID="BottonTestDBConnection" runat="server" Text="Connect" OnClick="BottonTestDBConnection_Click" />
        </td>
    </tr>
    <tr>
        <td class="auto-style1">Database Name: </td>
        <td class="auto-style1">
            <asp:DropDownList ID="DropDownListDBNames" runat="server" Width="320px" />
        </td>
        <td>
            <%--<asp:LinkButton ID="LinkButtonCreateDB" runat="server" Text="Create new..." OnClick="LinkButtonCreateDB_Click" CausesValidation="true" OnClientClick="window.showModalDialog('/Popup.aspx?CtlID=DBCreator', 'dialogHeight:300px;dialogWidth:500px;scroll:yes;resizable:no;help:no;')" /> --%>
            <asp:HiddenField ID="HFCustomerID" runat="server" />
            <asp:HiddenField ID="HFConfigurationID" runat="server" />
            <asp:HiddenField ID="HFConfigurationIndex" runat="server" />
            <asp:LinkButton ID="LinkButtonCreateDB" runat="server" Text="Create new..." OnClick="LinkButtonCreateDB_Click" CausesValidation="true" /> 
            <asp:Button ID="ButtonApply" runat="server" Text="Apply" OnClick="ButtonApply_Click" CausesValidation="true" Visible="false"/>        
        </td>
    </tr>
</table> 