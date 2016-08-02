<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CustomerDetail.aspx.cs" Inherits="DISConfigurationCloud.CustomerDetail" %>

<%@ Register Src="~/UserControls/DBConnectionEditor.ascx" TagPrefix="uc1" TagName="DBConnectionEditor" %>
<%@ Register Src="~/UserControls/BusinessReferenceEditor.ascx" TagPrefix="uc2" TagName="BizRefEditor" %>

<asp:Content ID="HeaderContent" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <h2>
        Configuration Set
    </h2>
    <p>
        
    </p>
    <div>
        <fieldset class="register">
            <legend>Configuration Set Detail</legend>
            <asp:DetailsView ID="DetailsViewCustomer" runat="server" AutoGenerateRows="False"  OnDataBound="DetailsViewCustomer_DataBound" OnModeChanging="DetailsViewCustomer_ModeChanging" OnItemUpdating="DetailsViewCustomer_ItemUpdating" Width="100%" CommandRowStyle-HorizontalAlign="Right">
        <Fields>
            <asp:BoundField DataField="ID" HeaderText="Business ID" SortExpression="ID" ReadOnly="true" />
            <asp:BoundField DataField="Name" HeaderText="Business Name" SortExpression="Name" ReadOnly="true" />
            <%--<asp:BoundField DataField="ReferenceID" HeaderText="Business Reference ID" SortExpression="ReferenceID" ReadOnly="false"  />--%>
            <asp:TemplateField HeaderText="Business References">
                <ItemTemplate>
                    <uc2:BizRefEditor ID="bizRefEditor" runat="server" IsReadOnly="true" EnableViewState="true" />
                </ItemTemplate>
                <EditItemTemplate>
                    <uc2:BizRefEditor ID="bizRefEditor" runat="server" IsReadOnly="false" EnableViewState="true" />
                </EditItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Configurations">
                <ItemTemplate>
                    <asp:GridView ID="GridViewCustomerConfigs" runat="server" AutoGenerateColumns="false" BorderStyle="None">
                        <Columns>
                            <asp:BoundField DataField="ID" HeaderText="Configuration ID" SortExpression="ID" HeaderStyle-HorizontalAlign="Left" ReadOnly="true" />
                            <asp:BoundField DataField="ConfigurationType" HeaderText="Configuration Type" SortExpression="ConfigurationType" />
                            <asp:BoundField DataField="DBConnectionString" HeaderText="Database Connection String" SortExpression="DBConnectionString" />
                        </Columns>
                    </asp:GridView>
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:GridView ID="GridViewCustomerConfigs" runat="server" AutoGenerateColumns="false" BorderStyle="None" OnRowDataBound="GridViewCustomerConfigsEdit_RowDataBound">
                        <Columns>
                            <asp:BoundField DataField="ID" HeaderText="Configuration ID" SortExpression="ID" HeaderStyle-HorizontalAlign="Left" ReadOnly="true" />
                            <asp:TemplateField HeaderText="Configuration Type">
                                <ItemTemplate>
                                    <asp:RadioButtonList runat="server" ID="RadioButtonListConfigTypes" AutoPostBack="true" RepeatDirection="Horizontal" OnSelectedIndexChanged="RadioButtonListConfigTypes_SelectedIndexChanged">
                                        <asp:ListItem Value="0">OEM</asp:ListItem>
                                        <asp:ListItem Value="1">TPI</asp:ListItem>
                                        <asp:ListItem Value="2">Factory Floor</asp:ListItem>
                                    </asp:RadioButtonList>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Database Connection String">
                                <ItemTemplate>
                                    <uc1:DBConnectionEditor runat="server" ID="DBConnectionEditor"  IsAllowingCreatingNewDB="false" IsShowingApplyButton="true" OnApply="DBConnectionEditor_Apply"  />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </EditItemTemplate>
            </asp:TemplateField>
            <asp:CommandField ShowEditButton="True" ButtonType="Button" />
        </Fields>
    </asp:DetailsView>
        </fieldset>
    </div>
</asp:Content>
