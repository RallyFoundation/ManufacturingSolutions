<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CustomerWizard.aspx.cs" Inherits="DISConfigurationCloud.CustomerWizard" MasterPageFile="~/Site.Master" %>
<%@ Register src="UserControls/DBConnectionEditor.ascx" tagname="DBConnectionEditor" tagprefix="uc1" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <h2>
       <asp:Literal runat="server" Text="<%$ Resources:GlobalResource, Title_NewCustomer %>"  />
    </h2>
    <p>
       <asp:Literal runat="server" Text="<%$ Resources:GlobalResource, Desc_NewCustomer %>"  />
    </p>
    <asp:Wizard ID="WizardCustomer" runat="server" ActiveStepIndex="0" OnNextButtonClick="WizardCustomer_NextButtonClick" OnSideBarButtonClick="WizardCustomer_NextButtonClick" OnActiveStepChanged="WizardCustomer_ActiveStepChanged" OnFinishButtonClick="WizardCustomer_FinishButtonClick">
            <SideBarStyle Width="30%"  />
            <%--<StepStyle HorizontalAlign="Center" VerticalAlign="Middle" />--%>
            <WizardSteps>
                <asp:WizardStep ID="WizardStepCustomerID" runat="server" title="<%$ Resources:GlobalResource, Title_WizardStep1 %>">
                     <div>
                        <fieldset class="register"> 
                            <legend><asp:Literal runat="server" Text="<%$ Resources:GlobalResource, Legened_BusinessInformation %>"  /></legend>
                                <table>
                                    <tr>
                                        <td class="auto-style1"><asp:Literal runat="server" Text="<%$ Resources:GlobalResource, Label_BusinessID %>"  />:</td>
                                        <td class="auto-style1">
                                             <asp:TextBox ID="TextBoxBusinessID" runat="server" CssClass="textEntry" />
                                             <asp:RequiredFieldValidator ID="RFVTextBoxBusinessID" runat="server" ControlToValidate="TextBoxBusinessID" Display="Dynamic" CssClass="failureNotification" ErrorMessage="Business ID is required." ToolTip="Business ID is required.">*</asp:RequiredFieldValidator>
                                             <asp:RegularExpressionValidator ID="REVTextBoxBusinessID" runat="server" ControlToValidate="TextBoxBusinessID" Display="Dynamic" CssClass="failureNotification" ValidationExpression="[a-zA-Z0-9_-]{2,50}" ErrorMessage="Business ID should only be composed of alpha, numberic as well as the underscore ('_') and hyphen ('-') characters, and is limited to 2-50 characters!" Text="!" />
                                        </td>
                                        <td>
                                            
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="auto-style1"><asp:Literal runat="server" Text="<%$ Resources:GlobalResource, Label_BusinessName %>"  />:</td>
                                        <td class="auto-style1">
                                             <asp:TextBox ID="TextBoxCustomerName" runat="server" CssClass="textEntry" />
                                             <asp:RequiredFieldValidator ID="RFVCustomerNameRequired" runat="server" ControlToValidate="TextBoxCustomerName" Display="Dynamic" CssClass="failureNotification" ErrorMessage="Business Name is required." ToolTip="Business Name is required.">*</asp:RequiredFieldValidator>
                                             <asp:RegularExpressionValidator ID="REVTextBoxCustomerName" runat="server" ControlToValidate="TextBoxCustomerName" Display="Dynamic" CssClass="failureNotification" ValidationExpression="[a-zA-Z0-9_]{2,50}" ErrorMessage="Business Name should only be composed of alpha, numberic as well as the underscore ('_') characters, and is limited to 2-50 characters!" Text="!" />
                                        </td>
                                        <td>
                                            <%--<asp:HiddenField ID="HFCustomerID" runat="server" />--%>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="3">
                                            <asp:ValidationSummary ID="validationSummary" runat="server" CssClass="failureNotification" DisplayMode="BulletList" ShowMessageBox="true" ShowSummary="false" HeaderText="The following error(s) occurred in your inputs, please double check and try again:" />
                                        </td>
                                    </tr>
                                </table>            
                            </fieldset>
                         </div>
                </asp:WizardStep>
                <asp:WizardStep ID="WizardStepULS" runat="server" title="<%$ Resources:GlobalResource, Title_WizardStep2 %>">
                    <div>
                        <div style="width:100%; text-align:right">
                            <asp:LinkButton ID="linkButtonSkipStepULS" runat="server" Text="<%$ Resources:GlobalResource, ButtonText_Skip %>" CausesValidation="false" OnClientClick="return confirm('Are you sure to skip this step?');" OnClick="linkButtonSkipStepULS_Click" />
                        </div>
                        <fieldset class="register">
                            <legend><asp:Literal runat="server" Text="<%$ Resources:GlobalResource, Legend_ConfigurationType %>"  /></legend>
                             <asp:DropDownList ID="DropDownListConfigurationTypeULS" runat="server" Width="75%">
                                <asp:ListItem Text="OEM" Value="0" />
                                <asp:ListItem Text="TPI/ODM" Value="1" />
                             </asp:DropDownList>
                        </fieldset>
                        <fieldset class="register">
                             <legend><asp:Literal runat="server" Text="<%$ Resources:GlobalResource, Legend_DatabaseConnection %>"  /></legend>
                                <uc1:DBConnectionEditor ID="DBConnectionEditorULS" runat="server" />       
                        </fieldset>
                   </div>
                </asp:WizardStep>
                <asp:WizardStep ID="WizardStepDLS" runat="server" title="<%$ Resources:GlobalResource, Title_WizardStep3 %>">
                    <div>
                        <div style="width:100%; text-align:right">
                           <asp:LinkButton ID="linkButtonSkipStepDLS" runat="server" Text="<%$ Resources:GlobalResource, ButtonText_Skip %>" CausesValidation="false" OnClientClick="return confirm('Are you sure to skip this step?');" OnClick="linkButtonSkipStepDLS_Click"/>
                        </div>
                        <fieldset class="register">
                            <legend><asp:Literal runat="server" Text="<%$ Resources:GlobalResource, Legend_ConfigurationType %>"  /></legend>
                             <asp:DropDownList ID="DropDownListConfigurationTypeDLS" runat="server" Width="75%">
                                <asp:ListItem Text="TPI/ODM" Value="1" />
                                <asp:ListItem Text="Factory Floor" Value="2" />
                             </asp:DropDownList>
                        </fieldset>
                        <fieldset class="register">
                             <legend><asp:Literal runat="server" Text="<%$ Resources:GlobalResource, Legend_DatabaseConnection %>"  /></legend>
                                <uc1:DBConnectionEditor ID="DBConnectionEditorDLS" runat="server" />
                        </fieldset>
                   </div>
                </asp:WizardStep>
                <asp:WizardStep ID="WizardStepSummary" runat="server" Title="<%$ Resources:GlobalResource, Title_WizardStep4 %>">
                    <div>
                       <fieldset class="register">
                            <legend><asp:Literal runat="server" Text="<%$ Resources:GlobalResource, Legend_Summary %>"  /></legend>
                             <table>
                                 <tr>
                                     <td colspan="3">
                                        <b><asp:Literal runat="server" Text="<%$ Resources:GlobalResource, Legened_BusinessInformation %>"  />:</b>
                                        <br />
                                     </td>
                                 </tr>
                                 <tr>
                                     <td class="auto-style1">
                                         <asp:Literal runat="server" Text="<%$ Resources:GlobalResource, Label_BusinessName %>"  />: 
                                     </td>
                                     <td class="auto-style1">
                                         <asp:Label ID="LabelCustomerName" runat="server"/>
                                     </td>
                                     <td></td>
                                 </tr>
                                 <tr>
                                     <td colspan="3">
                                        <br />
                                        <b> <asp:Literal runat="server" Text="<%$ Resources:GlobalResource, Legend_UpLevelSystemDatabase %>"  />:</b>
                                        <br />
                                     </td>
                                 </tr>
                                 <tr>
                                     <td class="auto-style1">
                                         <asp:Literal runat="server" Text="<%$ Resources:GlobalResource, Legend_ConfigurationType %>"  />: 
                                     </td>
                                     <td class="auto-style1">
                                         <asp:Label ID="LabelConfigurationTypeULS" runat="server" />
                                     </td>
                                     <td></td>
                                 </tr>
                                 <tr>
                                     <td class="auto-style1">
                                         <asp:Literal runat="server" Text="<%$ Resources:GlobalResource, Legend_DatabaseConnectionString %>"  />: 
                                     </td>
                                     <td class="auto-style1">
                                         <asp:Label ID="LabelDBConnectionStringULS" runat="server" />
                                     </td>
                                     <td></td>
                                 </tr>
                                 <tr>
                                     <td colspan="3">
                                        <br />
                                        <b><asp:Literal runat="server" Text="<%$ Resources:GlobalResource, Legend_DownLevelSystemDatabase %>"  />:</b>
                                        <br />
                                     </td>
                                 </tr>
                                 <tr>
                                     <td class="auto-style1">
                                         <asp:Literal runat="server" Text="<%$ Resources:GlobalResource, Legend_ConfigurationType %>"  />: 
                                     </td>
                                     <td class="auto-style1">
                                         <asp:Label ID="LabelConfigurationTypeDLS" runat="server" />
                                     </td>
                                     <td></td>
                                 </tr>
                                 <tr>
                                     <td class="auto-style1">
                                          <asp:Literal runat="server" Text="<%$ Resources:GlobalResource, Legend_DatabaseConnectionString %>"  />: 
                                     </td>
                                     <td class="auto-style1">
                                         <asp:Label ID="LabelDBConnectionStringDLS" runat="server" />
                                     </td>
                                     <td></td>
                                 </tr>
                             </table>
                       </fieldset>  
                    </div>
                </asp:WizardStep>
             </WizardSteps>
        </asp:Wizard>
</asp:Content>
