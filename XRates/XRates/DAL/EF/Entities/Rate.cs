using System;

namespace XRates.DAL.EF.Entities
{
    public class Rate
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public decimal Value { get; set; }
        public string Code { get; set; }

        //public Country Country { get; set; }
    }
}