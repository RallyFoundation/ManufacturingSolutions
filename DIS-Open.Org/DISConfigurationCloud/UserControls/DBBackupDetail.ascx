<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DBBackupDetail.ascx.cs" Inherits="DISConfigurationCloud.UserControls.DBBackupDetail" %>
<script type="text/javascript">
    function SetRadioButtonSelection(sender)
    {
        var container = document.getElementById("DIS.BackupDetailList");

        if (container != null)
        {
            var inputs = container.getElementsByTagName("input");

            if ((inputs != null) && (inputs.length > 0))
            {
                for (var i = 0; i < inputs.length; i++)
                {
                    if ((inputs[i].type == "radio") && (inputs[i].checked == sender.checked) && (inputs[i].id != sender.id))
                    {
                        inputs[i].checked = false;
                    }
                }
            }
        }
    }
</script>
<div id="DIS.BackupDetailList">
    <asp:GridView ID="GridViewDBBackupDetail" runat="server" AutoGenerateColumns="false">
    <Columns>
        <asp:TemplateField>
            <ItemTemplate>
                <asp:RadioButton ID="RadioButtonSelect" runat="server" onclick="SetRadioButtonSelection(this);" />
            </ItemTemplate>
        </asp:TemplateField>
        <asp:BoundField DataField="Position" HeaderText="Position" SortExpression="Position" HeaderStyle-HorizontalAlign="Left" />
        <asp:BoundField DataField="BackupSetGUID" HeaderText="Backup Set GUID" SortExpression="BackupSetGUID" HeaderStyle-HorizontalAlign="Left" />
        <asp:BoundField DataField="DatabaseName" HeaderText="Database Name" SortExpression="DatabaseName" HeaderStyle-HorizontalAlign="Left" />
        <asp:BoundField DataField="ServerName" HeaderText="Server\Instance Name" SortExpression="ServerName" HeaderStyle-HorizontalAlign="Left" />
        <asp:BoundField DataField="BackupSize" HeaderText="Backup Size" SortExpression="BackupSize" HeaderStyle-HorizontalAlign="Left" />
        <asp:BoundField DataField="BackupStartDate" HeaderText="Backup Start Date" SortExpression="BackupStartDate" HeaderStyle-HorizontalAlign="Left" />
        <asp:BoundField DataField="BackupFinishDate" HeaderText="Backup Finish Date" SortExpression="BackupFinishDate" HeaderStyle-HorizontalAlign="Left" />
        <asp:BoundField DataField="BackupTypeDescription" HeaderText="Backup Type" SortExpression="BackupTypeDescription" HeaderStyle-HorizontalAlign="Left" />
        <asp:BoundField DataField="DeviceType" HeaderText="Device Type" SortExpression="DeviceType" HeaderStyle-HorizontalAlign="Left" />
        <asp:BoundField DataField="UserName" HeaderText="User Name" SortExpression="UserName" HeaderStyle-HorizontalAlign="Left" />
        <asp:BoundField DataField="DatabaseCreationDate" HeaderText="Database Creation Date" SortExpression="Database Creation Date" HeaderStyle-HorizontalAlign="Left" /> 
        <asp:BoundField DataField="RecoveryModel" HeaderText="Recovery Model" SortExpression="RecoveryModel" HeaderStyle-HorizontalAlign="Left" />
    </Columns>
</asp:GridView>
</div>
