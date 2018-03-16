using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management.Automation;
using ZXing;

namespace PowerShellOA3DPKSNBinder
{
    [Cmdlet(VerbsCommon.Show, "SNBarcode")]
    public class ShowSNBarcodeCommand : Cmdlet
    {
        [Parameter(Position = 0, Mandatory = true, HelpMessage = "The value of barcode.")]
        public string BarcodeValue { get; set; }

        [Parameter(Position = 1, Mandatory = true, HelpMessage = "The type of barcode.")]
        public BarcodeFormat BarcodeType { get; set; }

        [Parameter(Position = 2, Mandatory = true, HelpMessage = "The width of barcode image.")]
        public int ImageWidth { get; set; }

        [Parameter(Position = 3, Mandatory = true, HelpMessage = "The height of barcode image.")]
        public int ImageHeight { get; set; }

        [Parameter(Position = 4, Mandatory = false, HelpMessage = "Whether to show barcode text.")]
        public bool IsShowingBarcodeText { get; set; }
        protected override void ProcessRecord()
        {
            //base.ProcessRecord();

            FormSNBarcode formSNBarcode = new FormSNBarcode();

            formSNBarcode.ShowBarcode(this.BarcodeValue, this.BarcodeType, this.ImageWidth, this.ImageHeight, this.IsShowingBarcodeText);

            formSNBarcode.ShowDialog();
        }
    }
}
