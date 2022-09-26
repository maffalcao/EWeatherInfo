using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Infra.Interfaces;

namespace Infra.Helpers
{
    public class IO : IIO
    {
        public void DownloadFile(string url, string destinationPath)
        {
            try
            {
                const int BUFFER_SIZE = 16 * 1024;
                var uri = new Uri(url);
                var destinationFilePath = Path.Combine(destinationPath, uri.Segments.LastOrDefault());

                CreateDirectory(destinationPath);

                using (var outputFileStream = File.Create(destinationFilePath, BUFFER_SIZE))
                {
                    var req = WebRequest.Create(new Uri(url));
                    using (var response = req.GetResponse())
                    {
                        using (var responseStream = response.GetResponseStream())
                        {
                            var buffer = new byte[BUFFER_SIZE];
                            int bytesRead;
                            do
                            {
                                bytesRead = responseStream.Read(buffer, 0, BUFFER_SIZE);
                                outputFileStream.Write(buffer, 0, bytesRead);

                            } while (bytesRead > 0);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                var bla = 1;

            }

        }

        public bool FileExists(string filePath)
        {
            return File.Exists(filePath);
        }

        public void UnzipFile(string filePath, string destionationFolder)
        {
            CreateDirectory(destionationFolder);
            System.IO.Compression.ZipFile.ExtractToDirectory(filePath, destionationFolder);
        }

        public async Task<bool> IsUrlAvailable(string url)
        {
            var isUrlAvailable = false;
            var webRequest = WebRequest.Create(new Uri(url));
            WebResponse webResponse;
            try
            {
                webResponse = await webRequest.GetResponseAsync();
                isUrlAvailable = true;
            }
            catch
            {

            }

            return isUrlAvailable;
        }


        private void CreateDirectory(string destinationPath)
        {
            var directoryName = System.IO.Path.GetDirectoryName(destinationPath);
            System.IO.Directory.CreateDirectory(directoryName);
        }
    }
}