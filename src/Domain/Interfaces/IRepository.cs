using System;

namespace Domain.Interfaces
{
    public interface IRepository<T> where T: class
    {
         public T GetData(string deviceId, string sensorType, DateTime date);
    }
}