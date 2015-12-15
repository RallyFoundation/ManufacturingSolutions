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
	/// Interaction logic for ExportLogView.xaml
	/// </summary>
	public partial class ExportLogView : Window
	{
		public ExportLogView()
		{
			this.InitializeComponent();
			
			// Insert code required on object creation below this point.
		}

		private void btn_Cancel_Click(object sender, System.Windows.RoutedEventArgs e)
		{
			// TODO: Add event handler implementation here.
			this.Window.Close();
		}

		private void btn_ok_Click(object sender, System.Windows.RoutedEventArgs e)
		{
			// TODO: Add event handler implementation here.
			stpanel_location.Visibility = Visibility.Collapsed;
			stpanel_progress.Visibility = Visibility.Visible;
		}
	}
}