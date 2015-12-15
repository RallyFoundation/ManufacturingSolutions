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
	/// Interaction logic for MarkAsReturnView.xaml
	/// </summary>
	public partial class MarkAsReturnView : Window
	{
		public MarkAsReturnView()
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
		
		private void GoToNextWizard()
        {
            if (this.frame_keysselect.Visibility == Visibility.Visible)
            {
				btnMark.Visibility = Visibility.Collapsed;
				btnCancel.Visibility = Visibility.Collapsed;
				btnFinish.Visibility = Visibility.Visible;
                this.frame_keysselect.Visibility = Visibility.Hidden;
                this.frame_summary.Visibility = Visibility.Visible;
                this.lbl_keysselect.FontWeight = System.Windows.FontWeights.Regular;
                this.lbl_summary.FontWeight = System.Windows.FontWeights.Bold;
            }
        }

		private void btnFinish_Click(object sender, System.Windows.RoutedEventArgs e)
		{
			// TODO: Add event handler implementation here.
			this.Window.Close();
		}
	}
}