using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebDav;
using System.IO;
using System.Net.Http;
using System.Net;

namespace UnitTestWebDav
{
    class Program
    {
        static void Main(string[] args)
        {
            //testEmuDir();

            //testHfsUpload();

            testHfsUpload2(@"D:\WDS\WDS+.pptx", "http://minint-kuf9jnn:8080/Test/", "WDS", "W@lcome!");

            //testHfsUpload3(@"D:\WDS\WDS+-08211702.pptx", "http://minint-kuf9jnn:8080/Test/", "WDS", "W@lcome!");

            Console.Read();
        }

        static void testHfsUpload3(string filePath, string serverAddress, string userName, string password)
        {
            string authString = String.Format("{0}:{1}", userName, password);
            byte[] authBytes = System.Text.Encoding.UTF8.GetBytes(authString);
            string authBase64 = Convert.ToBase64String(authBytes);
            string basicAuthHeaderValue = String.Format("Basic {0}", authBase64);

            WebClient webClient = new WebClient();
            webClient.Headers.Add(HttpRequestHeader.Authorization, basicAuthHeaderValue);
            webClient.UploadFileAsync(new Uri(serverAddress), filePath);
            webClient.UploadProgressChanged += WebClientUploadProgressChanged;
        }

        static void WebClientUploadProgressChanged(object sender, UploadProgressChangedEventArgs e)
        {
            Console.WriteLine("Upload {0}% complete. ", e.ProgressPercentage);
        }

        static void testHfsUpload2(string filePath, string serverAddress, string userName, string password)
        {
            string authString = String.Format("{0}:{1}", userName, password);
            byte[] authBytes = System.Text.Encoding.UTF8.GetBytes(authString);
            string authBase64 = Convert.ToBase64String(authBytes);

            //string basicAuthHeaderValue = String.Format("Basic {0}", authBase64);

            string fileID = Guid.NewGuid().ToString();

            byte[] fileBytes = new byte[1024];

            HttpResponseMessage result = null;

            using (FileStream stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                fileBytes = new byte[stream.Length];
                stream.Read(fileBytes, 0, fileBytes.Length);
            }

            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", authBase64);

                using (var content = new MultipartFormDataContent("-------------------------acebdf13572468"))
                {
                    //content.Add(new StreamContent(new MemoryStream(fileBytes)), fileID, filePath.Substring(filePath.LastIndexOf("\\") + 1));

                    content.Add(new ByteArrayContent(fileBytes), fileID, fileID);

                    result = client.PostAsync(serverAddress, content).Result;
                }
            }

            Console.WriteLine(result.StatusCode);

            Console.WriteLine(result.ReasonPhrase);
        }

        async static void testHfsUpload()
        {
            string filePath = @"D:\WDS\WDS+-08211702.pptx";
            byte[] fileBytes = new byte[1024];

            using (FileStream stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                fileBytes = new byte[stream.Length];
                stream.Read(fileBytes, 0, fileBytes.Length);
            }

           await upload(fileBytes);
        }

        static async Task<string> upload(byte[] image)
        {
            string uploadAddress = "http://minint-kuf9jnn:8080/Test/";//"http://minint-et2evvt:280/WDS-Images/";

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", "V0RTOldAbGNvbWUh");

                using (var content = new MultipartFormDataContent("Upload----" + DateTime.Now.ToString(System.Globalization.CultureInfo.InvariantCulture)))
                {
                    string fileID = Guid.NewGuid().ToString();
                    content.Add(new StreamContent(new MemoryStream(image)), fileID, fileID);

                    using (
                       var message =
                           //await client.PostAsync("http://www.directupload.net/index.php?mode=upload", content))
                           
                           await client.PostAsync(uploadAddress, content))
                    {
                        var input = await message.Content.ReadAsStringAsync();

                        //return !string.IsNullOrWhiteSpace(input) ? Regex.Match(input, @"http://\w*\.directupload\.net/images/\d*/\w*\.[a-z]{3}").Value : null;

                        return input;
                    }
                }
            }
        }

        async static void testEmuDir()
        {
            string address = "http://minint-et2evvt:8080/WDS-Images/";

            using (var webDavClient = new WebDavClient())
            {
                var result = await webDavClient.Propfind(address);
                if (result.IsSuccessful)
                {
                    foreach (var res in result.Resources)
                    {
                        Console.WriteLine("Name: " + res.DisplayName);
                        Console.WriteLine("Is directory: " + res.IsCollection);
                        // other params
                    }
                }
                // continue ...
                else
                {
                    Console.WriteLine(result.Description);
                    Console.WriteLine(result.StatusCode);
                }
                    // handle an error
            }
        }
    }
}
