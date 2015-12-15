using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Windows.Automation;
using System.Threading;
using System.Reflection;

namespace WPFAutomation.Core
{
    public class Driver
    {
        private const int MAXTIME = 5000; // Total length in milliseconds to wait for the application start
        private const int TIMEWAIT = 100; // Timespan to wait till trying to find the window

        /// <summary>
        /// Start application and get the main window
        /// </summary>
        /// <param name="appPath"></param>
        /// <param name="arguments"></param>
        /// <returns></returns>
        public static AutomationElement StartApplication(string appPath)
        {
            Helper.ValidateArgumentNotNull(appPath, "appPath");

            Process process = new Process();
            ProcessStartInfo psi = new ProcessStartInfo();

            psi.FileName = appPath;
            process.StartInfo = psi;
            //Start application
            process.Start();

            //Check that if the main window had been launched
            int runningTime = 0;
            while (process.MainWindowHandle.Equals(IntPtr.Zero))
            {
                if (runningTime > MAXTIME)
                    throw new Exception("Time Out, could not find the application: " + appPath);
                Thread.Sleep(TIMEWAIT);

                runningTime += TIMEWAIT;

                process.Refresh();
            }

            return AutomationElement.FromHandle(process.MainWindowHandle);
        }

        /// <summary>
        /// Find application from desktop
        /// </summary>
        /// <param name="appPath"></param>
        /// <param name="arguments"></param>
        /// <returns></returns>
        public static AutomationElement FindApplicationByTitle(string titile)
        {
            Helper.ValidateArgumentNotNull(titile, "Title");

            var desktop = AutomationElement.RootElement;
            Condition condition = new PropertyCondition(AutomationElement.NameProperty, titile);
            return desktop.FindFirst(TreeScope.Descendants,condition);
        }

        /// <summary>
        /// Get type by path
        /// </summary>
        /// <returns></returns>
        public static Type GetTypeByPath(string assemblyName, string fullName)
        {
            Assembly assembly = null;
            try
            {
                assembly = Assembly.Load(assemblyName);
            }
            catch (Exception)
            {

             
            }

            if (assembly == null)
            {
                assembly = Assembly.LoadFile(assemblyName);
            }
            Helper.ValidateArgumentNotNull(assembly, "Load assembly Fail, " + assemblyName);
            Type type = assembly.GetType(fullName);
            Helper.ValidateArgumentNotNull(type, "Load type fail, " + fullName);
            return type;
        }


        /// <summary>
        /// Excute Method
        /// </summary>
        /// <param name="methodName"></param>
        public static object ExcuteMethodByName(string assemblypath, string fullName, string methodName,object[] parameters)
        {
            Type type = GetTypeByPath(assemblypath, fullName);
            MethodInfo method = type.GetMethod(methodName);
            object result = method.Invoke(Activator.CreateInstance(type), parameters);
            return result;
        }
    }
}
