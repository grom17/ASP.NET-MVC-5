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

        public List<object> GetRates()
        {
            var rates = DBHelper.Instance.GetRates();
            List<object> data = new List<object>();
            List<object> usaDataResult = new List<object>();
            List<object> chinaDataResult = new List<object>();
            List<object> japanDataResult = new List<object>();
            long defaultDateTicks = new DateTime(1970, 1, 1).Ticks;
            foreach (var rate in rates)
            {
                if (rate.Code == "R01235")
                    usaDataResult.Add(new object[] { ToJsonTicks(rate.Date), rate.Value.ToString() });
                else if (rate.Code == "R01820")
                    japanDataResult.Add(new object[] { ToJsonTicks(rate.Date), rate.Value.ToString() });
                else chinaDataResult.Add(new object[] { ToJsonTicks(rate.Date), FormatCNY(rate.Value).ToString() });
            }
            data.Add(usaDataResult);
            data.Add(japanDataResult);
            data.Add(chinaDataResult);
            return data;
        }

        private double FormatCNY(double value)
        {
            if (value > 20)
                return value / 10;
            else return value;
        }

        private long ToJsonTicks(DateTime value)
        {
            long defaultDateTicks = new DateTime(1970, 1, 1).Ticks;
            return value.Ticks - defaultDateTicks;
        }
    }
}