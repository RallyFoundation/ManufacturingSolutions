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
using DIS.Presentation.KMT.ViewModel;

namespace DIS.Presentation.KMT.View.Configuration
{
    public partial class TpiConfigurationView : IConfigurationPage
    {
        public TpiConfigurationViewModel VM { get; private set; }

        public TpiConfigurationView()
        {
            InitializeComponent();
            VM = new TpiConfigurationViewModel();
            DataContext = VM;
        }

        public bool Save()
        {
            return VM.Save();
        }
    }
}