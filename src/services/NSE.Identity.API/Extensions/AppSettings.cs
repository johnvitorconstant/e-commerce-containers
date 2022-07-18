﻿namespace NSE.Identity.API.Extensions
{
    public class AppSettings
    {
        public string Secret { get; set; }
        public int HoursToExpire { get; set; }
        public string Issuer { get; set; }
        public string ValidAt { get; set; }
    }
}