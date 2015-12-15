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
using System.Windows.Markup;

namespace DIS
{
	/// <summary>
	/// Interaction logic for ExportKeys.xaml
	/// </summary>
	public partial class ExportKeys : Window
	{
		public ExportKeys()
		{
			this.InitializeComponent();
			
			// Insert code required on object creation below this point.
		}

		private void btnCancel_Click(object sender, System.Windows.RoutedEventArgs e)
		{
			// TODO: Add event handler implementation here.
			this.Window.Close();
		}

		private void btnNext_Click(object sender, System.Windows.RoutedEventArgs e)
		{
			GoToNextWizard();
		}

        private void GoToNextWizard()
        {
            if (this.frame_typeselect.Visibility == Visibility.Visible)
            {
                btnPrevious.Visibility = Visibility.Visible;
                this.frame_typeselect.Visibility = Visibility.Hidden;
                this.frame_keysselect.Visibility = Visibility.Visible;
                this.lbl_typeselect.FontWeight = System.Windows.FontWeights.Regular;
                this.lbl_keysselect.FontWeight = System.Windows.FontWeights.Bold;
            }
            else if (this.frame_keysselect.Visibility == Visibility.Visible)
            {
				btnNext.Visibility = Visibility.Collapsed;
				btnExport.Visibility = Visibility.Visible;
                this.frame_keysselect.Visibility = Visibility.Hidden;
                this.frame_locationselect.Visibility = Visibility.Visible;
                this.lbl_keysselect.FontWeight = System.Windows.FontWeights.Regular;
                this.lbl_locationselect.FontWeight = System.Windows.FontWeights.Bold;
            }
            else if (this.frame_locationselect.Visibility == Visibility.Visible)
            {
                btnExport.Visibility = Visibility.Collapsed;
				btnFinish.Visibility = Visibility.Visible;
				btnPrevious.Visibility = Visibility.Collapsed;
				btnCancel.Visibility = Visibility.Collapsed;
                this.frame_locationselect.Visibility = Visibility.Hidden;
                this.frame_finish.Visibility = Visibility.Visible;
                this.lbl_locationselect.FontWeight = System.Windows.FontWeights.Regular;
                this.lbl_finish.FontWeight = System.Windows.FontWeights.Bold;
            }
        }

        private void btnPrevious_Click(object sender, System.Windows.RoutedEventArgs e)
        {
        	GoToPreviousWizard();
        }

        private void GoToPreviousWizard()
        {
            if(this.frame_finish.Visibility == Visibility.Visible)
            {
                btnNext.Visibility = Visibility.Visible;
				btnFinish.Visibility = Visibility.Collapsed;
                frame_finish.Visibility = Visibility.Hidden;
                frame_locationselect.Visibility = Visibility.Visible;
                this.lbl_finish.FontWeight = System.Windows.FontWeights.Regular;
                this.lbl_locationselect.FontWeight = System.Windows.FontWeights.Bold;
            }
            else if (this.frame_locationselect.Visibility == Visibility.Visible)
            {
                this.frame_locationselect.Visibility = Visibility.Hidden;
                this.frame_keysselect.Visibility = Visibility.Visible;
                this.lbl_locationselect.FontWeight = System.Windows.FontWeights.Regular;
                this.lbl_keysselect.FontWeight = System.Windows.FontWeights.Bold;
				btnNext.Visibility = Visibility.Visible;
				btnExport.Visibility = Visibility.Collapsed;
            }
            else if (this.frame_keysselect.Visibility == Visibility.Visible)
            {
                btnPrevious.Visibility = Visibility.Collapsed;
                this.frame_keysselect.Visibility = Visibility.Hidden;
                this.frame_typeselect.Visibility = Visibility.Visible;
                this.lbl_keysselect.FontWeight = System.Windows.FontWeights.Regular;
                this.lbl_typeselect.FontWeight = System.Windows.FontWeights.Bold;
            }
            
        }

        private void btnExport_Click(object sender, System.Windows.RoutedEventArgs e)
        {
        	// TODO: Add event handler implementation here.
			GoToNextWizard();
        }

        private void btnFinish_Click(object sender, System.Windows.RoutedEventArgs e)
        {
        	// TODO: Add event handler implementation here.
			this.Window.Close();
        }
		
	}
}