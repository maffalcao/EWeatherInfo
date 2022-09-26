using System;

namespace Domain.Entities
{
    public class MeasurementEntity
    {
        public DateTime Date { get; private set; }
        public decimal Value { get; private set; }

        public MeasurementEntity(DateTime date, decimal value)
        {
            Date = date;
            Value = value;
        }
    }
}