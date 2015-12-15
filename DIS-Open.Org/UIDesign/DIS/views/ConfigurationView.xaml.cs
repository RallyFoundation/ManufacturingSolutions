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
	/// Interaction logic for ConfigurationView.xaml
	/// </summary>
	public partial class ConfigurationView : Window
	{
		public ConfigurationView()
		{
			this.InitializeComponent();
			
			// Insert code required on object creation below this point.
		}

		private void btncancel_setting_Click(object sender, System.Windows.RoutedEventArgs e)
		{
			// TODO: Add event handler implementation here.
			this.Window.Close();
		}

		private void lstbox_setting_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
		{
			// TODO: Add event handler implementation here.
            SetActivePane();
		}

        private void SetActivePane()
        {
            if (frame_systemsetting != null && frame_accountsetting != null && frame_backup != null)
            {
                frame_systemsetting.Visibility = (lstbox_setting.SelectedIndex == 0) ? Visibility.Visible : Visibility.Hidden;
                frame_accountsetting.Visibility = (lstbox_setting.SelectedIndex == 1) ? Visibility.Visible : Visibility.Hidden;
                frame_backup.Visibility = (lstbox_setting.SelectedIndex == 2) ? Visibility.Visible : Visibility.Hidden;
            }
        }
	}
}