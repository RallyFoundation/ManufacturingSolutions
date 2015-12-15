using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace DIS
{
	/// <summary>
	/// Interaction logic for ImportView.xaml
	/// </summary>
	public partial class ImportView : Window
	{
		public ImportView()
		{
			this.InitializeComponent();
			
			// Insert code required on object creation below this point.
		}

		private void btnNext_Click(object sender, System.Windows.RoutedEventArgs e)
		{
			// TODO: Add event handler implementation here.
			GoToNextWizard();
		}

		private void btnCancel_Click(object sender, System.Windows.RoutedEventArgs e)
		{
			// TODO: Add event handler implementation here.
			this.Window.Close();
		}

		private void btnFinish_Click(object sender, System.Windows.RoutedEventArgs e)
		{
			// TODO: Add event handler implementation here.
			this.Window.Close();
		}
		
		private void GoToNextWizard()
        {
            if (this.frame_locationselect.Visibility == Visibility.Visible)
            {
                this.frame_locationselect.Visibility = Visibility.Hidden;
                this.frame_importresult.Visibility = Visibility.Visible;
                this.lbl_locationselect.FontWeight = System.Windows.FontWeights.Regular;
                this.lbl_importresult.FontWeight = System.Windows.FontWeights.Bold;
				btnNext.Visibility = Visibility.Collapsed;
				btnFinish.Visibility = Visibility.Visible;
				btnCancel.Visibility = Visibility.Collapsed;
            }
        }
		
	}
}