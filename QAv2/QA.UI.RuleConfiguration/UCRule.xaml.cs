using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using QA.UI.RuleConfiguration.ViewModel;

namespace QA.UI.RuleConfiguration
{
    /// <summary>
    /// Interaction logic for UCEqualTo.xaml
    /// </summary>
    public partial class UCRule : UserControl
    {
        public UCRule()
        {
            InitializeComponent();

            this.viewModel = new RuleItem();
            this.DataContext = this.viewModel;
        }

        private RuleItem viewModel;

        private void ComboBoxRuleTypes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            this.viewModel.FieldlName = "Field1";
            this.viewModel.FieldlValue = "Value1";
            this.viewModel.RuleType = "EqualTo";
        }
    }
}
