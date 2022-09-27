using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Infra.Interfaces;
using Microsoft.Extensions.Logging;

namespace Infra.Helpers
{
    public class IO : IIO
    {
        private readonly ILogger<IO> _logger;
        public IO(ILogger<IO> logger) =>
            _logger = logger;
        public void DownloadFile(string url, string destinationPath)
        {
            const int BUFFER_SIZE = 64 * 1024;
            var uri = new Uri(url);
            var destinationFilePath = Path.Combine(destinationPath, uri.Segments.LastOrDefault());

            CreateDirectory(destinationFilePath);

            using (var outputFileStream = File.Create(destinationFilePath, BUFFER_SIZE))
            {
                _logger.LogInformation($"I/O: Start download from {url} to {destinationPath}");

                // TODO: download file chunks in parallel
                try
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
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message);
                    File.Delete(destinationFilePath);
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

            try
            {
                _logger.LogInformation($"I/O: Start extract from {filePath} to {destionationFolder}");
                System.IO.Compression.ZipFile.ExtractToDirectory(filePath, destionationFolder);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
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
                // it means that url does not exists  
            }

            return isUrlAvailable;
        }


        private void CreateDirectory(string destinationPath)
        {
            var directoryName = System.IO.Path.GetDirectoryName(destinationPath);

            if (!Directory.Exists(directoryName))
            {
                try
                {

                    _logger.LogInformation($"I/O: Start create directory {destinationPath}");
                    System.IO.Directory.CreateDirectory(directoryName);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message);
                    throw ex;
                }
            }
        }
    }
}