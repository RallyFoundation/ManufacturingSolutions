using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WebViewPlus
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            //if ((args != null) && (args.Length > 0))
            //{
            //    Application.Run(new FormWebView(null, args[0]));
            //}
            //else
            //{
            //    Application.Run(new FormWebView());
            //}

            if ((args != null) && (args.Length > 0))
            {
                if (args.Length == 1)
                {
                    Application.Run(new FormWebView(null, args[0]));
                }
                else
                {
                    string path = "";

                    for (int i = 0; i < args.Length; i++)
                    {
                        if (args[i].StartsWith("/") && args[i].Contains(":"))
                        {
                            break;
                        }

                        path += args[i];

                        if (i != (args.Length - 1))
                        {
                            path += " ";
                        }
                    }

                    Application.Run(new FormWebView(null, path));
                }
            }
            else
            {
                Application.Run(new FormWebView());
            }
        }
    }
}
