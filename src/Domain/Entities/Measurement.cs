using System;

namespace Domain.Entities
{
    public class Measurement
    {
        public DateTime Date { get; private set; }
        public decimal Value { get; private set; }

        public Measurement(DateTime date, decimal value)
        {
            Date = date;
            Value = value;
        }
    }
}