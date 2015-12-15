using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.IO;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Collections.ObjectModel;
using System.DirectoryServices.AccountManagement;

namespace PSInvokingTest
{
    class Program
    {
        static void Main(string[] args)
        {
            //ExecuteAsynchronously();

            //ExecuteScriptFile();

            //ExecuteScriptFileAsync();

            //UtilityTest();

            //UtilityTestAsync();

            //UtilityTestUSB();

            //GetDrives();

            //GetFileSysEntries();

            TestPrincipalPassword();

            Console.Read();
        }

        static void TestPrincipalPassword() 
        {
            Console.WriteLine("Enter the password of your current account:");

            string password = Console.ReadLine();

            bool result = false;

            using (PrincipalContext context = new PrincipalContext(ContextType.Machine))
            {
               result = context.ValidateCredentials("Administrator", password);
            }

            Console.WriteLine(result);
        }


        static void GetFileSysEntries() 
        {
           string[] items = System.IO.Directory.GetFileSystemEntries(@"E:\D\home\v-rawang\Documents\MAB\publish\MFG\SFT", "*", SearchOption.AllDirectories);

           Console.WriteLine(items.Length);

           foreach (var item in items)
           {
               Console.WriteLine(item);
           }

           Console.WriteLine(items.Length);
        }
        static void GetDrives() 
        {
            var drives = DriveInfo.GetDrives();
            foreach (var drive in drives)
            {
                Console.WriteLine(drive.DriveType);
                Console.WriteLine(drive.Name);
                //Console.WriteLine(drive.DriveFormat);
            }
        }

        static void UtilityTestUSB() 
        {
            var usbDevices = DISDeploymentCenter.Utility.GetUSBDevices();

            foreach (var usbDevice in usbDevices)
            {
                Console.WriteLine("Device ID: {0}, PNP Device ID: {1}, Description: {2}",
                    usbDevice.DeviceID, usbDevice.PnpDeviceID, usbDevice.Description);
            }

            Console.Read();
        }

        static void UtilityTest() 
        {
            string scriptfile = AppDomain.CurrentDomain.BaseDirectory + "\\test.ps1";

            Dictionary<string, object> parameters = new Dictionary<string,object>()
            {
                {"value01", Guid.NewGuid()},
                {"value02", (new Random()).Next()},
            };

            List<object> results =  DISDeploymentCenter.Utility.ExecutePSScriptFile(scriptfile, parameters);

            foreach (var item in results)
            {
                Console.WriteLine(item.ToString());
            }
        }

        static void UtilityTestAsync()
        {
            string scriptfile = AppDomain.CurrentDomain.BaseDirectory + "\\test.ps1";

            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"value01", Guid.NewGuid()},
                {"value02", (new Random()).Next()},
            };

            DISDeploymentCenter.Utility.ExecutePSScriptFileAsync(scriptfile, parameters, (o) =>
            {
                foreach (var item in o)
                {
                    Console.WriteLine(item.ToString());
                }

                return null;
            });
        }


        static void ExecuteScriptFile() 
        {
            RunspaceConfiguration runspaceConfiguration = RunspaceConfiguration.Create();

            Runspace runspace = RunspaceFactory.CreateRunspace(runspaceConfiguration);
            runspace.Open();

            RunspaceInvoke scriptInvoker = new RunspaceInvoke(runspace);

            Pipeline pipeline = runspace.CreatePipeline();

            string scriptfile = AppDomain.CurrentDomain.BaseDirectory + "\\test.ps1";

            //Here's how you add a new script with arguments
            Command myCommand = new Command(scriptfile);
            //CommandParameter testParam = new CommandParameter("key", "value");
            //myCommand.Parameters.Add(testParam);

            myCommand.Parameters.Add(new CommandParameter("value01", Guid.NewGuid().ToString()));

            myCommand.Parameters.Add(new CommandParameter("value02", (new Random()).Next().ToString()));

            pipeline.Commands.Add(myCommand);

            // Execute PowerShell script
            Collection<PSObject> results = pipeline.Invoke();

            foreach (var item in results)
            {
                Console.WriteLine(item.BaseObject.ToString());
            }
        }

        static void ExecuteScriptFileAsync()
        {
            RunspaceConfiguration runspaceConfiguration = RunspaceConfiguration.Create();

            Runspace runspace = RunspaceFactory.CreateRunspace(runspaceConfiguration);
            runspace.Open();

            RunspaceInvoke scriptInvoker = new RunspaceInvoke(runspace);

            Pipeline pipeline = runspace.CreatePipeline();

            string scriptfile = AppDomain.CurrentDomain.BaseDirectory + "\\test.ps1";

            //Here's how you add a new script with arguments
            Command myCommand = new Command(scriptfile);
            //CommandParameter testParam = new CommandParameter("key", "value");
            //myCommand.Parameters.Add(testParam);

            myCommand.Parameters.Add(new CommandParameter("value01", Guid.NewGuid().ToString()));

            myCommand.Parameters.Add(new CommandParameter("value02", (new Random()).Next().ToString()));

            pipeline.Commands.Add(myCommand);

            // Execute PowerShell script
            //Collection<PSObject> results = pipeline.Invoke();

            //foreach (var item in results)
            //{
            //    Console.WriteLine(item.BaseObject.ToString());
            //}

            pipeline.InvokeAsync();

            while (!pipeline.Output.EndOfPipeline)
            {
                Collection<PSObject> results = pipeline.Output.ReadToEnd();

                foreach (var item in results)
                {
                    Console.WriteLine(item.BaseObject.ToString());
                }
            }
        }

        /// <summary>
        /// Sample execution scenario 2: Asynchronous
        /// </summary>
        /// <remarks>
        /// Executes a PowerShell script asynchronously with script output and event handling.
        /// </remarks>
        static void ExecuteAsynchronously()
        {
            using (PowerShell PowerShellInstance = PowerShell.Create())
            {
                // this script has a sleep in it to simulate a long running script
                //PowerShellInstance.AddScript("$s1 = 'test1'; $s2 = 'test2'; $s1; write-error 'some error';start-sleep -s 7; $s2");

                //PowerShellInstance.AddScript("Invoke-Command");

                //PowerShellInstance.AddParameter("ScriptBlock", "test.ps1");

                PowerShellInstance.AddScript("Set-ExecutionPolicy");
                PowerShellInstance.AddParameter("ExecutionPolicy", "ByPass");

                PowerShellInstance.Invoke();

                PSCommand cmd = new PSCommand();
                String machinename = "localhost";
                String file = AppDomain.CurrentDomain.BaseDirectory + "\\test.ps1";
                cmd.AddCommand("Invoke-Command");
                cmd.AddParameter("ComputerName", machinename);
                //cmd.AddParameter("FilePath", file);
                cmd.AddParameter("ScriptBlock", ScriptBlock.Create(file));

                PowerShellInstance.Commands = cmd;

                // prepare a new collection to store output stream objects
                PSDataCollection<PSObject> outputCollection = new PSDataCollection<PSObject>();
                outputCollection.DataAdded += outputCollection_DataAdded;

                // the streams (Error, Debug, Progress, etc) are available on the PowerShell instance.
                // we can review them during or after execution.
                // we can also be notified when a new item is written to the stream (like this):
                PowerShellInstance.Streams.Error.DataAdded += Error_DataAdded;

                // begin invoke execution on the pipeline
                // use this overload to specify an output stream buffer
                IAsyncResult result = PowerShellInstance.BeginInvoke<PSObject, PSObject>(null, outputCollection);

                // do something else until execution has completed.
                // this could be sleep/wait, or perhaps some other work
                while (result.IsCompleted == false)
                {
                    Console.WriteLine("Waiting for pipeline to finish...");
                    Thread.Sleep(1000);

                    // might want to place a timeout here...
                }

                Console.WriteLine("Execution has stopped. The pipeline state: " + PowerShellInstance.InvocationStateInfo.State);

                foreach (PSObject outputItem in outputCollection)
                {
                    //TODO: handle/process the output items if required
                    Console.WriteLine(outputItem.BaseObject.ToString());
                }
            }
        }

        /// <summary>
        /// Event handler for when data is added to the output stream.
        /// </summary>
        /// <param name="sender">Contains the complete PSDataCollection of all output items.</param>
        /// <param name="e">Contains the index ID of the added collection item and the ID of the PowerShell instance this event belongs to.</param>
        static void outputCollection_DataAdded(object sender, DataAddedEventArgs e)
        {
            // do something when an object is written to the output stream
            Console.WriteLine("Object added to output.");
        }

        /// <summary>
        /// Event handler for when Data is added to the Error stream.
        /// </summary>
        /// <param name="sender">Contains the complete PSDataCollection of all error output items.</param>
        /// <param name="e">Contains the index ID of the added collection item and the ID of the PowerShell instance this event belongs to.</param>
        static void Error_DataAdded(object sender, DataAddedEventArgs e)
        {
            // do something when an error is written to the error stream
            Console.WriteLine("An error was written to the Error stream!");
        }
    }
}
