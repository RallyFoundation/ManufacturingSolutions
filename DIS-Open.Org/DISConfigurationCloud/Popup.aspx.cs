using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
//using DISConfigurationCloud.Security;
using Platform.DAAS.OData.Security;

namespace DISConfigurationCloud
{
    public partial class Popup : AuthPageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            this.loadControl();
        }

        private void loadControl() 
        {
            string controlID = Request.QueryString["CtlID"];

            if (!String.IsNullOrEmpty(controlID))
            {
                string controlPath = String.Format("~/UserControls/{0}.ascx", controlID);

                Control control = this.LoadControl(controlPath);

                this.ControlContainer.Controls.Add(control);
            }
        }
    }
}