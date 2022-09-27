using System.Threading.Tasks;

namespace Infra.Interfaces
{
    public interface IIO
    {
        public bool FileExists(string filePath);
        public Task<bool> IsUrlAvailable(string url);
        public void DownloadFile(string url, string destinationPath);
        public void UnzipFile(string filePath, string destionationFolder);
    }
}