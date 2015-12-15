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
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			this.InitializeComponent();

			// Insert code required on object creation below this point.
		}

		private void mainRibbon_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
		{
			// TODO: Add event handler implementation here.
            if (mainRibbon != null)
            {
                SetActiveFrame(mainRibbon.SelectedIndex);
            }
		}

        private void SetActiveFrame(int p)
        {
			//frame_home.Visibility = (p == 0) ? Visibility.Visible : Visibility.Hidden;
            frame_orderManagement.Visibility = (p == 0) ? Visibility.Visible : Visibility.Hidden;
            frame_keyManagement.Visibility = (p == 1) ? Visibility.Visible : Visibility.Hidden;
            frame_userManagement.Visibility = (p == 2) ? Visibility.Visible : Visibility.Hidden;
            frame_reports.Visibility = (p == 4) ? Visibility.Visible : Visibility.Hidden;
            frame_systemlog.Visibility = (p == 3) ? Visibility.Visible : Visibility.Hidden;
        }

        private void btnnew_Click(object sender, System.Windows.RoutedEventArgs e)
        {
        	// TODO: Add event handler implementation here.
			CreateOrder createOrder = new CreateOrder();
			createOrder.ShowDialog();
        }
		
		private void btnadduser_Click(object sender, System.Windows.RoutedEventArgs e)
        {
        	// TODO: Add event handler implementation here.
			CreateNewUser createNewUser = new CreateNewUser();
			createNewUser.ShowDialog();
        }
		
		private void btnexportkeys_Click(object sender, System.Windows.RoutedEventArgs e)
        {
        	// TODO: Add event handler implementation here.
			ExportKeys exportkeys = new ExportKeys();
			exportkeys.ShowDialog();
        }
		
		private void btnimportkeys_Click(object sender, System.Windows.RoutedEventArgs e)
        {
        	// TODO: Add event handler implementation here.
			ImportView importView = new ImportView();
			importView.ShowDialog();
        }
		
		private void btnMarkAsReturn_Click(object sender, System.Windows.RoutedEventArgs e)
        {
        	// TODO: Add event handler implementation here.
			MarkAsReturnView maReturnView = new MarkAsReturnView();
			maReturnView.ShowDialog();
        }
		
		
		private void btnassign_Click(object sender, System.Windows.RoutedEventArgs e)
        {
        	// TODO: Add event handler implementation here.
			AssignmentView assignmentView = new AssignmentView();
			assignmentView.ShowDialog();
        }
		
		private void btnUnassign_Click(object sender, System.Windows.RoutedEventArgs e)
        {
        	// TODO: Add event handler implementation here.
			UnAssignKeys unassignkeys = new UnAssignKeys();
			unassignkeys.ShowDialog();
        }
		
		private void btnreportKeys_Click(object sender, System.Windows.RoutedEventArgs e)
        {
        	// TODO: Add event handler implementation here.
			ReportKeysView reportKeysView = new ReportKeysView();
			reportKeysView.ShowDialog();
        }

        private void exitMenuItem_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            // TODO: Add event handler implementation here.
            Application.Current.Shutdown();
        }
		
		private void settingMenuItem_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            // TODO: Add event handler implementation here.
            ConfigurationView cView = new ConfigurationView();
			cView.ShowDialog();
        }
		
		private void aboutMenuItem_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            // TODO: Add event handler implementation here.
            AboutView aView = new AboutView();
			aView.ShowDialog();
        }
	}
}