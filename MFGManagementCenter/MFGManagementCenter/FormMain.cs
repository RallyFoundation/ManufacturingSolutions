using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MetroFramework.Forms;

namespace DISManagementCenter
{
    public partial class FormMain : MetroForm
    {
        public FormMain()
        {
            InitializeComponent();
        }

        private FormDataMigration formDataMig;

        private void metroTileDataMigration_Click(object sender, EventArgs e)
        {
            if (this.formDataMig == null)
            {
                this.formDataMig = new FormDataMigration(this);
            }
            
            this.formDataMig.Show();
            this.Visible = false;
        }
    }
}
