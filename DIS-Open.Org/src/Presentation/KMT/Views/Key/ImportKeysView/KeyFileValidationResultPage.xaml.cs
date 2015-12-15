using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Collections.ObjectModel;
using DIS.Data.DataContract;
using DIS.Presentation.KMT.Properties;

namespace DIS.Presentation.KMT.Views.Key.ImportKeysView
{
    /// <summary>
    /// Interaction logic for KeyFileValidationResultPage.xaml
    /// </summary>
    public partial class KeyFileValidationResultPage : Window
    {
        public KeyFileValidationResultPage(ObservableCollection<KeyOperationResult> ItemSource)
        {
            this.keyOperationResults = ItemSource;

            this.resultMessage = String.Format(MergedResources.ImportKey_ValidationResultSummary, this.keyOperationResults.Count((o) => (o.FailedType == KeyErrorType.Duplicate)), this.keyOperationResults.Count((o) => (o.FailedType == KeyErrorType.InvalidOriginalFile)));

            this.DataContext = this;

            InitializeComponent();
        }

        private ObservableCollection<KeyOperationResult> keyOperationResults = null;

        private string resultMessage = "";

        /// <summary>
        /// 
        /// </summary>
        public ObservableCollection<KeyOperationResult> KeyOperationResults
        {
            get 
            {
                return this.keyOperationResults; 
            }
            set
            {
                this.keyOperationResults = value;
            }
        }

        public string ResultMsg 
        { 
            get { return this.resultMessage; } 
            set { this.resultMessage = value; } 
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
