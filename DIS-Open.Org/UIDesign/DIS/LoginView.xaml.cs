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
	/// Interaction logic for LoginView.xaml
	/// </summary>
	public partial class LoginView : Window
	{
		public LoginView()
		{
			this.InitializeComponent();
			
			// Insert code required on object creation below this point.
		}

		private void btnlogin_Click(object sender, System.Windows.RoutedEventArgs e)
		{
			// TODO: Add event handler implementation here.
			if(txt_username.Text.ToLower() == "admin" && txt_password.Password.ToLower() == "admin")
			{
				MainWindow mainWindow = new MainWindow();
				this.Window.Close();
				mainWindow.ShowDialog();
			}
			else
			{
				lbl_errormsg.Visibility = Visibility.Visible;
				txt_username.Clear();
				txt_password.Clear();
			}
		}

		private void btnCancel_Click(object sender, System.Windows.RoutedEventArgs e)
		{
			// TODO: Add event handler implementation here.
			Application.Current.Shutdown();
		}
	}
}