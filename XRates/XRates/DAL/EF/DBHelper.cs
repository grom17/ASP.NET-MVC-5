using System;
using System.Collections.Generic;
using System.Linq;
using XRates.DAL.EF.Entities;
using XRates.DAL.WS;

namespace XRates.DAL.EF
{
    public class DBHelper
    {
        private static DBHelper mInstance = null;
        private const string connectionStringName = "RatesDB";
        private DBHelper() { }
        public static DBHelper Instance
        {
            get
            {
                if (mInstance == null)
                    mInstance = new DBHelper();
                return mInstance;
            }
        }

        public List<Country> GetCountries()
        {
            using (var db = new RatesContext(connectionStringName))
            {
                return db.Countries.ToList();
            }
        }

        public List<Rate> GetRates()
        {
            using (var db = new RatesContext(connectionStringName))
            {
                CheckDB(db);
                return db.Rates.ToList();
            }
        }

        public string[] GetCodes()
        {
            using (var db = new RatesContext(connectionStringName))
            {
                return db.Countries.Select(x => x.Code).ToArray();
            }
        }

        public void AddRates(List<Rate> rates)
        {
            using (var db = new RatesContext(connectionStringName))
            {
                db.Rates.AddRange(rates);
                db.SaveChanges();
            }
        }

        private void CheckDB(RatesContext db)
        {
            bool needSave = false;
            if (db.Countries.Count() == 0)
            {
                db.Countries.Add(new Country { Name = "USA", Currency = "USD", Code = "R01235" });
                db.Countries.Add(new Country { Name = "Japan", Currency = "JPY", Code = "R01820" });
                db.Countries.Add(new Country { Name = "China", Currency = "CNY", Code = "R01375" });
                needSave = true;
            }
            if (db.Rates.Count() == 0)
            {
                var rates = GetRates(GetCodes());
                db.Rates.AddRange(rates);
                needSave = true;
            }
            if (needSave) db.SaveChanges();
        }

        private List<Rate> GetRates(string[] Codes)
        {
            var ratesList = new List<Rate>();
            var countries = GetCountries();
            foreach (var code in Codes)
            {
                var rates = WSHelper.Instance.GetCursDynamic(new DateTime(2016, 4, 1), DateTime.Now, code);
                foreach (var rate in rates)
                {
                    var country = countries.Where(c => c.Code == rate.Vcode).FirstOrDefault();
                    ratesList.Add(new Rate() { Date = rate.CursDate, Code = rate.Vcode, Value = rate.Vcurs });
                }
            }
            return ratesList;
        }
    }
}