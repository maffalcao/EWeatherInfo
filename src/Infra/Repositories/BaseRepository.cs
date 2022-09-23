using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Interfaces;
using Infra.Interfaces;

namespace Infra.Repositories
{
    public abstract class BaseRepository<T> where T : class
    {
        protected readonly IRedisClient _redis;
        protected readonly IIO _io;
        protected string _urlBase;
        protected string _folderBase;
        protected string deviceId;
        protected string sensorType;
        protected DateTime date;

        public BaseRepository(IRedisClient redis, IIO io)
        {
            _redis = redis;
            _io = io;
            _urlBase = "https://sigmaiotexercisetest.blob.core.windows.net/iotbackend";
            _folderBase = $"{Path.GetTempPath()}/iotbackend";

        }

        public abstract IList<T> GetDataFromCsvFile();

        public abstract IList<T> GetDataFromUrlCsvFile();

    }
}