using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LmsBeApp_Group06.Services
{
    public class IdentityDefaultOptions
    {
       
        //lockout settings
        public double LockoutDefaultLockoutTimeSpanInMinutes { get; set; }
        public int LockoutMaxFailedAccessAttempts { get; set; }
        public bool LockoutAllowedForNewUsers { get; set; }

        //cookie settings
        public bool CookieHttpOnly { get; set; }
        public double CookieExpiration { get; set; }      
        public string AccessDeniedPath { get; set; }
        public bool SlidingExpiration { get; set; }
    }
}
