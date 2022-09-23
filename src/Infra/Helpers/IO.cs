using System;
using System.IO;
using System.Net;
using Infra.Interfaces;

namespace Infra.Helpers
{
    public class IO : IIO
    {
        public void DownloadFile(string url, string destinationPath)
        {
            const int BUFFER_SIZE = 16 * 1024;

            CreateDirectory(destinationPath);
            using (var outputFileStream = File.Create(destinationPath, BUFFER_SIZE))
            {
                var req = WebRequest.Create(url);
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

        public bool FileExists(string filePath)
        {
            return File.Exists(filePath);
        }

        public void UnzipFile(string filePath, string destionationFolder)
        {
            CreateDirectory(destionationFolder);
            System.IO.Compression.ZipFile.ExtractToDirectory(filePath, destionationFolder);
        }

        public bool IsUrlAvailable(string url)
        {
            var isUrlAvailable = false;
            WebRequest webRequest = WebRequest.Create(url);
            WebResponse webResponse;
            try
            {
                webResponse = webRequest.GetResponse();
                isUrlAvailable = true;
            }
            catch
            {

            }

            return isUrlAvailable;
        }

        private void CreateDirectory(string destinationPath) =>
            System.IO.Directory.CreateDirectory(System.IO.Path.GetDirectoryName(destinationPath));
    }
}