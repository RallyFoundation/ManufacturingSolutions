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
	/// Interaction logic for UnAssignKeys.xaml
	/// </summary>
	public partial class UnAssignKeys : Window
	{
		public UnAssignKeys()
		{
			this.InitializeComponent();
			
			// Insert code required on object creation below this point.
		}

		private void btnUnAssign_Click(object sender, System.Windows.RoutedEventArgs e)
		{
			// TODO: Add event handler implementation here.
			GoToNextWizard();
		}

		private void btnFinish_Click(object sender, System.Windows.RoutedEventArgs e)
		{
			// TODO: Add event handler implementation here.
			this.Window.Close();
		}

		private void btnCancel_Click(object sender, System.Windows.RoutedEventArgs e)
		{
			// TODO: Add event handler implementation here.
			this.Window.Close();
		}
		
		private void GoToNextWizard()
        {
            if (this.frame_productkeyselect.Visibility == Visibility.Visible)
            {
				btnUnAssign.Visibility = Visibility.Collapsed;
				btnCancel.Visibility = Visibility.Collapsed;
				btnFinish.Visibility = Visibility.Visible;
                this.frame_productkeyselect.Visibility = Visibility.Hidden;
                this.frame_summary.Visibility = Visibility.Visible;
                this.lbl_productkeyselect.FontWeight = System.Windows.FontWeights.Regular;
                this.lbl_summary.FontWeight = System.Windows.FontWeights.Bold;
            }
        }
	}
}