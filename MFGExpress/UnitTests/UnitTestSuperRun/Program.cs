using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace UnitTestSuperRun
{
    class Program
    {
        static void Main(string[] args)
        {
            string fileDirectory = @"C:/Program Files";

            fileDirectory = @"C:\Program Files (x86)\Microsoft ASP.NET\ASP.NET Web Pages\v2.0\Visual Studio 2015\Snippets\HTML\1033\ASP.NET Web Pages";

            //fileDirectory = @"D:\MDOS\OrderKey\OrderKey\OrderKey\Implementation\BlanketKeyOrder.cs";

            //StringBuilder sPath = new StringBuilder(fileDirectory.Length);
            //GetShortPathName(fileDirectory, sPath, fileDirectory.Length);
            //fileDirectory = sPath.ToString();

            fileDirectory = GetShortPath(fileDirectory);
            Console.WriteLine(fileDirectory);

            fileDirectory = GetLongPath(fileDirectory);
            Console.WriteLine(fileDirectory);

            Console.Read();
        }

        [DllImport("kernel32.dll", EntryPoint = "GetShortPathNameA")]
        private static extern int GetShortPathName(string lpszLongPath, StringBuilder lpszShortPath, int cchBuffer);

        static string GetShortPath(string LongPath)
        {
            string shortPath = LongPath;

            StringBuilder sPath = new StringBuilder(LongPath.Length);
            GetShortPathName(LongPath, sPath, LongPath.Length);
            shortPath = sPath.ToString();

            return shortPath;
        }

        [DllImport("kernel32.dll", EntryPoint = "GetLongPathNameA")]
        static extern int GetLongPathName(string lpszShortPath, StringBuilder lpszLongPath, int cchBuffer);

        static string GetLongPath(string shortPath)
        {
            string longPath = shortPath;
            StringBuilder lPath = new StringBuilder(shortPath.Length);
            GetLongPathName(shortPath, lPath, shortPath.Length);

            longPath = lPath.ToString();

            return longPath;
        }
    }
}
