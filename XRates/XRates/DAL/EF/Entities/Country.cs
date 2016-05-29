using System.Collections.Generic;
namespace XRates.DAL.EF.Entities
{
    public class Country
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Currency { get; set; }

        //public ICollection<Rate> Rates { get; set; }
    }
}