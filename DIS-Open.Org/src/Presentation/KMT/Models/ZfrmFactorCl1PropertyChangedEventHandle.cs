using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime;
using System.Collections.ObjectModel;
using DIS.Data.DataContract;

namespace DIS.Presentation.KMT.Models
{
    public delegate void ZfrmFactorCl1PropertyChangedEventHandle(object sender, ZfrmFactorCl1PropertyChangedEventArgs e);

    public class ZfrmFactorCl1PropertyChangedEventArgs : EventArgs
    {
        // Summary:
        //     Initializes a new instance of the System.ComponentModel.PropertyChangedEventArgs
        //     class.
        //
        // Parameters:
        //   propertyName:
        //     The name of the property that changed.
        [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
        public ZfrmFactorCl1PropertyChangedEventArgs(string value)
        {
            this.Value = value;
        }

        // Summary:
        //     Gets the name of the property that changed.
        //
        // Returns:
        //     The name of the property that changed.
        public string Value { get; set; }
    }
}
