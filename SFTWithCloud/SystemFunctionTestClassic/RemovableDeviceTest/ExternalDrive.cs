using System.Resources;
using System.Windows.Forms;

namespace RemovableDeviceTest
{
    public partial class ExternalDrive : UserControl
    {

        /// <summary>
        /// Initializes a new instance of the ExternalDrive as UserControl class.
        /// Each ExternalDrive control is the UI representation of each external drive found on system.
        /// </summary>
        public ExternalDrive(string DriveLetter, string DriveName, string Type, string AvailableFreeSpace, string TotalFreeSpace, string TotalSize)
        {
            InitializeComponent();
            ResourceManager LocRM = new ResourceManager("win81FactoryTest.AppResources.Res", typeof(win81FactoryTest.TestForm).Assembly);

            DriveTitle.Text = DriveLetter + " - " + DriveName;
            DriveType.Text = LocRM.GetString("RemovableDriveType")  + ":    " + Type;
            DriveLbl1.Text = LocRM.GetString("RemovableAvailableFreeSpace") + ":    " + float.Parse(AvailableFreeSpace.ToString()) / 1073741824 + " GB";
            //DriveLbl2.Text = LocRM.GetString("RemovableTotalFreeSpace") + ":    " + float.Parse(TotalFreeSpace.ToString()) / 1073741824 + " GB";
            DriveLbl2.Text = LocRM.GetString("RemovableTotalSize") + ":    " + float.Parse(TotalSize.ToString()) / 1073741824 + " GB";
        }

    }
}
