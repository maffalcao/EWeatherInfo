using System;

namespace Domain.Interfaces
{
    public interface IRepository<T>: T class
    {
         public T GetData(string deviceId, string sensorType, DateTime date);
    }
}