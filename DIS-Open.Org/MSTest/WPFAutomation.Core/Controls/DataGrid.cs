using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Automation;

namespace WPFAutomation.Core.Controls
{
    public class DataGrid
    {

        private AutomationElement _datagrid;
        public AutomationElement DatagridElement
        {
            get { return _datagrid; }
            set { _datagrid = value; }
        }

        private GridPattern _gridPattern;
        public GridPattern GridPattern
        {
            get { return _gridPattern; }
            set { _gridPattern = value; }
        }

        public DataGrid(AutomationElement ele, string automationID)
        {
            this._datagrid = Helper.ExtractElementByAutomationID(ele, automationID);
            Helper.ValidateArgumentNotNull(_datagrid, "DataGrid AutomationElement ");
            this._gridPattern = _datagrid.GetCurrentPattern(GridPattern.Pattern) as GridPattern;

        }

        /// <summary>
        /// Get cell value of DataGrid
        /// </summary>
        /// <param name="row"></param>
        /// <param name="column"></param>
        /// <returns></returns>
        public void SelectItem(int index)
        {
            AutomationElementCollection itemEle = Helper.ExtractElementByControlType(DatagridElement, ControlType.DataItem);
            Helper.ValidateArgumentNotNull(itemEle, "Items in the DataGrid ");
            SelectionItemPattern ss = itemEle[index].GetCurrentPattern(SelectionItemPattern.Pattern) as SelectionItemPattern;
            ss.Select();
        }

        /// <summary>
        /// select the first row according to column and id
        /// Wilson
        /// </summary>
        /// <param name="column"></param>
        /// <param name="id"></param>
        /// <returns>row index</returns>
        public int SelectItem(int column, string id)
        {
            List<string> idList = this.GetValueByColumn(column);
            int row = idList.IndexOf(id);
            if (row != -1)
                this.SelectItem(row);
            return row;
        }

        /// <summary>
        /// get row number
        /// Wilson
        /// </summary>
        public int RowCount { get { return GridPattern.Current.RowCount; } }

        /// <summary>
        /// Get cell value of DataGrid 
        /// </summary>
        /// <param name="row"></param>
        /// <param name="column"></param>
        /// <returns></returns>
        public string GetValue(int row, int column)
        {
            if (row < 0 || column < 0)
            {
                return null;
            }

            AutomationElement cellEle = GridPattern.GetItem(row, column);
            Helper.ValidateArgumentNotNull(cellEle, "Can not get cell AutomationELement from DataGrid.");
            Helper.TimeOutMillSec = 2500;
            AutomationElement valueEle = Helper.ExtractElementByClassName(cellEle, "TextBox");
            if (valueEle == null)
            {
                valueEle = Helper.ExtractElementByClassName(cellEle, "TextBlock");
                return valueEle.Current.Name;
            }
            Helper.TimeOutMillSec = 15000;
            ValuePattern tempPattern = valueEle.GetCurrentPattern(ValuePattern.Pattern) as ValuePattern;
            return tempPattern.Current.Value;
        }

        /// <summary>
        /// get a list of some column 
        /// Wilson
        /// </summary>
        /// <param name="column"></param>
        /// <returns></returns>
        public List<string> GetValueByColumn(int column)
        {
            if (column < 0 || column >= RowCount)
                return null;
            List<string> values = new List<string>();
            for (int i = 0; i < RowCount; i++)
                values.Add(GetValue(i, column));
            return values;
        }

    }
}
