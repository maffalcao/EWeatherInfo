namespace Infra.Interfaces
{
    public interface IIO
    {
         public bool FileExists(string filePath);
         public bool UrlExists(string url);
         public void DownloadFile(string url, string destinationPath);
         public void UnzipFile(string path, string destionationFolder);
    }
}