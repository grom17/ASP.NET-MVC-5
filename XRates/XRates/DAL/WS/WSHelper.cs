using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using XRates.Classes;
using XRates.DailyInfoWS;
using XRates.DAL.WS.Models;

namespace XRates.DAL.WS
{
    public class WSHelper
    {
        private DailyInfo _DIClient = new DailyInfo();
        private WSHelper() { }

        private static WSHelper mInstance = null;
        public static WSHelper Instance
        {
            get
            {
                if (mInstance == null)
                    mInstance = new WSHelper();
                return mInstance;
            }
        }

        public List<CurrencyModel> GetCursOnDate(DateTime date, string[] Codes)
        {
            DataSet courses = _DIClient.GetCursOnDate(date);
            DataTable dtTable = courses.Tables["ValuteCursOnDate"];
            List<CurrencyModel> ratesList = dtTable.DataTableToList<CurrencyModel>();
            ratesList = ratesList.Where(x => Array.Exists(Codes, c => c == x.Vcode)).ToList();
            return ratesList;
        }

        public List<CurrencyModel> GetCursDynamic(DateTime fromDate, DateTime toDate, string Code)
        {
            DataSet course = _DIClient.GetCursDynamic(fromDate, toDate, Code);
            DataTable dtTable = course.Tables["ValuteCursDynamic"];
            List<CurrencyModel> ratesList = dtTable.DataTableToList<CurrencyModel>();
            return ratesList;
        }
    }
}