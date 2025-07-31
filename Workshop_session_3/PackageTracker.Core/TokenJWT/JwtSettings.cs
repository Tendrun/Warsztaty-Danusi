using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PackageTracker.Core.TokenJWT
{
    public class JwtSettings
    {
        public string SecretKey { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public int ExpiryInMinutes { get; set; }
    }

    // DataAccessSettings.cs
    public class DataAccessSettings
    {
        public string Strategy { get; set; }
    }
}
