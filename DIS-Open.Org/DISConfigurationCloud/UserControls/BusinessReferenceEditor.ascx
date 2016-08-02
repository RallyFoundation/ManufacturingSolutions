<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="BusinessReferenceEditor.ascx.cs" Inherits="DISConfigurationCloud.UserControls.BusinessReferenceEditor" ViewStateMode="Enabled" EnableViewState="true" %>
<table border="0" style="width:100%; height:100%; border-width:0; border-style:none" >
    <tr>
        <td style="text-align:left">
            <asp:TextBox ID="txtNewBizRef" runat="server" MaxLength="100" Width="90%" ValidationGroup="InsertGroup"/>
            <asp:RequiredFieldValidator ID="RFVTextBoxBizRefID" runat="server" ControlToValidate="txtNewBizRef" Display="Dynamic" CssClass="failureNotification" ErrorMessage="Business Reference ID should not be empty!" ToolTip="Business Reference ID should not be empty!" ValidationGroup="InsertGroup">*</asp:RequiredFieldValidator>
            <asp:RegularExpressionValidator ID="REVTextBoxBizRefID" runat="server" ControlToValidate="txtNewBizRef" ValidationExpression="[^,]+" Display="Dynamic" CssClass="failureNotification" ErrorMessage="Business Reference ID should not be containing comma (,)!" ValidationGroup="InsertGroup">!</asp:RegularExpressionValidator>
            <asp:Button ID="btnAddBizRef" runat="server" Text="Add" OnClick="btnAddBizRef_Click" ValidationGroup="InsertGroup"/>
            <asp:ValidationSummary ID="validationSummaryInsert" runat="server" CssClass="failureNotification" DisplayMode="BulletList" ShowMessageBox="true" ShowSummary="false" HeaderText="The following error(s) occurred in your inputs, please double check and try again:" ValidationGroup="InsertGroup"/>
        </td>
    </tr>
    <tr>
        <td>
            <asp:GridView ID="gridViewBizRefs" runat="server" AutoGenerateColumns="false" Width="100%" Height="100%" ShowHeader="false" BorderStyle="None" OnRowEditing="gridViewBizRefs_RowEditing" OnRowCancelingEdit="gridViewBizRefs_RowCancelingEdit" OnRowDeleting="gridViewBizRefs_RowDeleting" OnRowUpdating="gridViewBizRefs_RowUpdating">
                <Columns>
                    <asp:TemplateField ItemStyle-Width="82%">
                        <ItemTemplate>
                            <%# Container.DataItem %>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txtRefID" runat="server" Text="<%# Container.DataItem %>" MaxLength="100" Width="96%" ValidationGroup="EditGroup"/>
                            <asp:RequiredFieldValidator ID="rfvTxtRefID" runat="server" ControlToValidate="txtRefID" Display="Dynamic" CssClass="failureNotification" ValidationGroup="EditGroup" ErrorMessage="Business Reference ID should not be empty!" ToolTip="Business Reference ID should not be empty!">*</asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="REVTextBoxBizRefID" runat="server" ControlToValidate="txtRefID" Display="Dynamic" ValidationExpression="[^,]+" CssClass="failureNotification" ErrorMessage="Business Reference ID should not be containing comma (,)!" ValidationGroup="EditGroup">!</asp:RegularExpressionValidator>
                            <asp:ValidationSummary ID="validationSummaryEdit" runat="server" CssClass="failureNotification" DisplayMode="BulletList" ShowMessageBox="true" ShowSummary="false" HeaderText="The following error(s) occurred in your inputs, please double check and try again:" ValidationGroup="EditGroup"/>
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:CommandField ShowEditButton="true" ShowCancelButton="true" ShowDeleteButton="true" ItemStyle-HorizontalAlign="Right" ButtonType="Button" CausesValidation="true" ValidationGroup="EditGroup"/>
                </Columns>
            </asp:GridView>
        </td>
    </tr>
</table>