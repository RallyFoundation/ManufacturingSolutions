<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="UserRoleManagement.aspx.cs" Inherits="ODataPlatform.Account.UserRoleManagement" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2>
        Manage User Role Assignment
    </h2>
    <p>
        Use the list below to manage role assignment for each user.
    </p>
    <br />
     <div>
        <asp:DropDownList ID="DropDownListRoles" runat="server" AutoPostBack="true" OnSelectedIndexChanged="DropDownListRoles_SelectedIndexChanged">
            <asp:ListItem>Master</asp:ListItem>
            <asp:ListItem Selected="True">Operator</asp:ListItem>
            <asp:ListItem>ForbiddenUser</asp:ListItem>
            <asp:ListItem>SupperUser</asp:ListItem>
        </asp:DropDownList>
         <br />
         <br />
        <asp:GridView ID="GridViewUsersInRole" runat="server" AllowPaging="True" AllowSorting="True" DataKeyNames="UserName" CssClass="DDGridView" RowStyle-CssClass="td" HeaderStyle-CssClass="th" CellPadding="6" AutoGenerateColumns="False" OnRowEditing="GridViewUsersInRole_RowEditing" OnRowUpdating="GridViewUsersInRole_RowUpdating" OnRowCancelingEdit="GridViewUsersInRole_RowCancelingEdit">
            <Columns>
                <asp:BoundField DataField="UserName" HeaderText="User Name" ReadOnly="true" />
                <asp:BoundField DataField="Email" HeaderText="Email" ReadOnly="true" />
                <asp:BoundField DataField="CreationDate" HeaderText="Creation Date" ReadOnly="true" />
                <asp:TemplateField HeaderText="Role">
                     <ItemTemplate>
                        <asp:Label ID="LabelRoleName" runat="server" Text="<%# DropDownListRoles.SelectedValue %>" />
                     </ItemTemplate>
                     <EditItemTemplate>
                        <asp:DropDownList ID="DropDownListRoleSelection" runat="server">
                            <asp:ListItem>Master</asp:ListItem>
                            <asp:ListItem>Operator</asp:ListItem>
                            <asp:ListItem>ForbiddenUser</asp:ListItem>
                            <asp:ListItem>SupperUser</asp:ListItem>
                        </asp:DropDownList>
                     </EditItemTemplate>
                </asp:TemplateField>
                <asp:CommandField HeaderText="Move to Role..." EditText="Move..." ShowEditButton="True" ShowHeader="True" />
            </Columns>

<HeaderStyle CssClass="th"></HeaderStyle>

            <PagerStyle CssClass="DDFooter"/>
                <EmptyDataTemplate>
                    There are currently no items.
                </EmptyDataTemplate>

<RowStyle CssClass="td"></RowStyle>
        </asp:GridView>
    </div>
</asp:Content>
