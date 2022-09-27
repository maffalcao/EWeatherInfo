using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using CrossCutting.Settings;
using Infra.Interfaces;
using Microsoft.Extensions.Options;

namespace Infra.Repositories
{
    public abstract class BaseRepository<T> where T : class
    {
        protected const int NUMBER_OF_COLUMNS_IN_CSV = 2;
        protected readonly IRedisClient _redis;
        protected readonly IIO _io;
        protected readonly AppSettings _settings;
        protected string _urlBase;
        protected string _folderBase;
        protected string deviceId;
        protected string sensorType;
        protected DateTime date;

        public BaseRepository(IRedisClient redis, IIO io, IOptions<AppSettings> settings)
        {
            _redis = redis;
            _io = io;
            _urlBase = $"{settings.Value.UrlBase}/{settings.Value.FolderBase}";
            _folderBase = Path.Combine(Path.GetTempPath(), settings.Value.FolderBase);
            _settings = settings.Value;

        }

        protected void ThrowFormatException(string filePath, string currentLine) =>
            throw new CrossCutting.Exceptions.FormatException($"Error reading file {filePath}: data in line '{currentLine}' is not in the expected format");

        protected abstract List<T> GetDataFromFile();

        protected abstract Task<List<T>> GetDataFromUrlAsync();

    }
}