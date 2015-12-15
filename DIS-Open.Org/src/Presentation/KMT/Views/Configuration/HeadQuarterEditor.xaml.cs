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
using DIS.Data.DataContract;

namespace DIS.Presentation.KMT.View.Configuration
{
    /// <summary>
    /// Interaction logic for HeadQuarterEditor.xaml
    /// </summary>
    public partial class HeadQuarterEditor : Window
    {
        public HeadQuarterEditorViewModel VM { get; private set; }

        public HeadQuarterEditor(HeadQuarter headQ = null)
        {
            InitializeComponent();

            VM = new HeadQuarterEditorViewModel(headQ);
            VM.View = this;
            DataContext = VM;
        }
    }
}
