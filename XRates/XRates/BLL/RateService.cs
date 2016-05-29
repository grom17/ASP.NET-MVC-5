using System;
using System.Collections.Generic;
using XRates.DAL.EF;
using XRates.DAL.EF.Entities;
using XRates.DAL.WS;

namespace XRates.BLL
{
    public class RateService
    {
        private RateService() { }

        private static RateService mInstance = null;
        public static RateService Instance
        {
            get
            {
                if (mInstance == null)
                    mInstance = new RateService();
                return mInstance;
            }
        }

        public void UpdateRates(DateTime date)
        {
            var rates = WSHelper.Instance.GetCursOnDate(date, DBHelper.Instance.GetCodes());
            var ratesList = new List<Rate>();
            foreach (var rate in rates)
            {
                ratesList.Add(new Rate() { Date = rate.CursDate, Code = rate.Vcode, Value = rate.Vcurs });
            }
            DBHelper.Instance.AddRates(ratesList);
        }
    }
}