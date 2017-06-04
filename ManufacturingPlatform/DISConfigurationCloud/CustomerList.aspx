<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CustomerList.aspx.cs" Inherits="DISConfigurationCloud.CustomerList" %>
<asp:Content ID="HeaderContent" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <h2>
        Configuration Sets
    </h2>
    <p>
        The list below shows information for all of the configuration sets
    </p>
    <div>
        <fieldset class="register">
            <legend>Configuration Sets</legend>
            <asp:GridView ID="GridViewCustomers" runat="server" AllowPaging="True" PageSize="10" AutoGenerateColumns="False" DataSourceID="ObjectDataSourceCustomers" Width="100%" HeaderStyle-HorizontalAlign="Left" PagerSettings-Mode="NumericFirstLast" PagerStyle-HorizontalAlign="Right">
                <Columns>
                    <asp:HyperLinkField HeaderText="Business Name" DataTextField="Name" DataNavigateUrlFields="ID" DataNavigateUrlFormatString="~/CustomerDetail.aspx?CustomerID={0}" HeaderStyle-HorizontalAlign="Left" />
                    <asp:BoundField DataField="ID" HeaderText="Business ID" SortExpression="ID" HeaderStyle-HorizontalAlign="Left" />
                   <%-- <asp:BoundField DataField="ReferenceID" HeaderText="Business Reference ID" SortExpression="ReferenceID" HeaderStyle-HorizontalAlign="Left" />--%>
                </Columns>
            </asp:GridView>
            <asp:ObjectDataSource ID="ObjectDataSourceCustomers" runat="server" DataObjectTypeName="DISConfigurationCloud.MetaManagement.Customer" SelectMethod="ListCustomers" TypeName="DISConfigurationCloud.MetaManagement.MetaManager" UpdateMethod="UpdateCustomerConfiguration">
                <SelectParameters>
                    <asp:Parameter Name="IsIncludingConfigurations" Direction="Input" Type="Boolean"  DefaultValue="false"/>
                </SelectParameters>
            </asp:ObjectDataSource>
        </fieldset>
    </div>
</asp:Content>
