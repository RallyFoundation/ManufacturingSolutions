using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZXing;

namespace PowerShellOA3DPKSNBinder
{
    public partial class FormSNBarcode : Form
    {
        public FormSNBarcode()
        {
            InitializeComponent();
        }

        public void ShowBarcode(string BarcodeValue, BarcodeFormat BarcodeType, int ImageWidth, int ImageHeight, bool IsShowingBarcodeText) 
        {
            this.pictureBoxSNBarcode.Width = ImageWidth;
            this.pictureBoxSNBarcode.Height = ImageHeight;
            this.pictureBoxSNBarcode.Image = this.getBarcodeImage(BarcodeValue, BarcodeType, ImageWidth, ImageHeight, IsShowingBarcodeText);
        }

        private Bitmap getBarcodeImage(string barcodeValue, BarcodeFormat barcodeType, int imageWidth, int imageHeight, bool isShowingBarcodeText) 
        {
            BarcodeWriter barcodeWriter = new BarcodeWriter();

            barcodeWriter.Format = barcodeType;
            barcodeWriter.Options.PureBarcode = (!isShowingBarcodeText);
            barcodeWriter.Options.Width = imageWidth;
            barcodeWriter.Options.Height = imageHeight;
            Bitmap barcodeImage = barcodeWriter.Write(barcodeValue);

            return barcodeImage;
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
