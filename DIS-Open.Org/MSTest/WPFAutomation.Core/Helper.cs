using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Automation;
using System.IO;
using System.Xml;

namespace WPFAutomation.Core
{
    /// <summary>
    /// Assistant class
    /// </summary>
    public class Helper
    {
        public static int TimeOutMillSec = 15000;

        /// <summary>
        /// Verify if the argument is null
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="argName"></param>
        public static void ValidateArgumentNotNull(object obj, string argName)
        {
            if (obj == null)
            {
                throw new ArgumentNullException(argName + " is null");
            }
        }

        /// <summary>
        /// Extract Window from parent window
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="titleName"></param>
        /// <returns></returns>
        public static AutomationElement ExtractElement(AutomationElement parent, string nameValue, TreeScope treeScope)
        {
            ValidateArgumentNotNull(parent, "Extract Window from parent window");
            Condition condition = new PropertyCondition(AutomationElement.NameProperty, nameValue);
            AutomationElement appElement;
            DateTime timeOut = DateTime.Now.AddMilliseconds(TimeOutMillSec);
            do
            {
                appElement = parent.FindFirst(treeScope, condition);
            } while (appElement == null && DateTime.Now < timeOut);

            return appElement;
        }

        public static AutomationElement ExtractElement(AutomationElement parent, string nameValue)
        {
            return ExtractElement(parent, nameValue, TreeScope.Children);
        }

        /// <summary>
        /// Exract control from parent window by AutomationID
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="automationID"></param>
        /// <returns></returns>
        public static AutomationElement ExtractElementByAutomationID(AutomationElement parent, string automationID)
        {
            ValidateArgumentNotNull(parent, "Extract Control from parent window by Automation ID, parent");
            ValidateArgumentNotNull(parent, "Extract Control from parent window by Automation ID, automation ID");
            Condition condition = new PropertyCondition(AutomationElement.AutomationIdProperty, automationID);
            AutomationElement appElement;
            DateTime timeOut = DateTime.Now.AddMilliseconds(TimeOutMillSec);
            do
            {
                appElement = parent.FindFirst(TreeScope.Descendants, condition);
            } while (appElement == null && DateTime.Now < timeOut);

            return appElement;
        }

        /// <summary>
        /// Exract control from parent window by name
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="className"></param>
        /// <returns></returns>
        public static AutomationElement ExtractElementByName(AutomationElement parent, string Name)
        {
            ValidateArgumentNotNull(parent, "Extract Control from parent window by Name, parent");
            ValidateArgumentNotNull(parent, "Extract Control from parent window by Name, Name");
            Condition condition = new PropertyCondition(AutomationElement.NameProperty, Name);
            AutomationElement appElement;
            DateTime timeOut = DateTime.Now.AddMilliseconds(TimeOutMillSec);
            do
            {
                appElement = parent.FindFirst(TreeScope.Descendants, condition);
            } while (appElement == null && DateTime.Now < timeOut);
            return appElement;
        }

        /// <summary>
        /// Exract control from parent window by class name
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="className"></param>
        /// <returns></returns>
        public static AutomationElement ExtractElementByClassName(AutomationElement parent, string className)
        {
            ValidateArgumentNotNull(parent, "Extract Control from parent window by Class Name, parent");
            ValidateArgumentNotNull(parent, "Extract Control from parent window by Class Name, Class Name");
            Condition condition = new PropertyCondition(AutomationElement.ClassNameProperty, className);
            AutomationElement appElement;
            DateTime timeOut = DateTime.Now.AddMilliseconds(TimeOutMillSec);
            do
            {
                appElement = parent.FindFirst(TreeScope.Descendants, condition);
            } while (appElement == null && DateTime.Now < timeOut);

            return appElement;
        }

        /// <summary>
        /// Exract items from parent window by Control Type
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="className"></param>
        /// <returns></returns>
        public static AutomationElementCollection ExtractElementByControlType(AutomationElement parent, ControlType type)
        {
            ValidateArgumentNotNull(parent, "Extract Control from parent window by Control Type, parent ");
            Condition condition = new PropertyCondition(AutomationElement.ControlTypeProperty, type);
            AutomationElementCollection appElement;
            DateTime timeOut = DateTime.Now.AddMilliseconds(TimeOutMillSec);
            do
            {
                appElement = parent.FindAll(TreeScope.Descendants, condition);
            } while (appElement == null && DateTime.Now < timeOut);

            return appElement;
        }

        /// <summary>
        /// Extract Window from parent window by runtime id
        /// </summary>                                            
        /// <param name="parent"></param>
        /// <param name="titleName"></param>
        /// <returns></returns>
        public static AutomationElement ExtractElementByProcessId(AutomationElement parent, int RuntimeID)
        {
            ValidateArgumentNotNull(parent, "Extract Control from parent window by Runtime ID, parent");
            ValidateArgumentNotNull(parent, "Extract Control from parent window by Runtiome ID, Runtime ID");
            Condition condition = new PropertyCondition(AutomationElement.RuntimeIdProperty, RuntimeID);
            AutomationElement appElement = parent.FindFirst(TreeScope.Descendants, condition);
            return appElement;
        }


        /// <summary>
        /// Compare two strings
        /// </summary>
        /// <param name="str1"></param>
        /// <param name="str2"></param>
        /// <returns></returns>
        public static bool CompareTwoStr(string str1, string str2)
        {
            ValidateArgumentNotNull(str1, "Compare two strings, String 1 ");
            ValidateArgumentNotNull(str2, "Compare two strings, String 2 ");
            return str1.CompareTo(str2) == 0 ? true : false;
        }

        /// <summary>
        /// Close test Window
        /// </summary>
        /// <param name="parent"></param>
        public static void CloseApp(AutomationElement parent)
        {
            ValidateArgumentNotNull(parent, "Close test window, parent");
            WindowPattern windowPattern = (WindowPattern)parent.GetCurrentPattern(WindowPattern.Pattern);
            windowPattern.Close();
        }


        //The path where thd log file created
        public static string logFilePath = "LOG_" + Helper.GetDateTimeString() + ".txt";

        public static void LogMessage(string message)
        {
            using (FileStream fs = File.Open(logFilePath, FileMode.Append, FileAccess.Write, FileShare.None))
            {
                StreamWriter sw = new StreamWriter(fs);
                //Log message
                sw.WriteLine(DateTime.Now.ToString() + " : " + message);
                sw.Close();
            }
        }

        /// <summary>
        /// Get log integer represent the date time.
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static string GetDateTimeString()
        {
            DateTime time = DateTime.Now;
            return time.Year.ToString() + time.Month.ToString() + time.Day.ToString() + time.Hour.ToString() + time.Minute.ToString() + time.Second.ToString();
        }

        /// <summary>
        /// Convert a XmlDocument To String
        /// </summary>
        /// <param name="xDoc"></param>
        /// <returns></returns>
        public static string XmlToString(XmlDocument xDoc)
        {
            using (StringWriter sw = new StringWriter())
            {
                XmlTextWriter tx = new XmlTextWriter(sw);
                xDoc.WriteTo(tx);
                return sw.ToString();
            }
        }
    }
}
