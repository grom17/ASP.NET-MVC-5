﻿using System;

namespace XRates.DAL.WS.Models
{
    public class CurrencyModel
    {
        public DateTime CursDate { get; set; }
        public string Vcode { get; set; }
        public double Vcurs { get; set; }
    }
}