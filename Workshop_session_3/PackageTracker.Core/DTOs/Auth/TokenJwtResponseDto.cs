using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PackageTracker.Core.DTOs.Auth
{
    public class TokenJwtResponseDto
    {
        public required string TokenJWT { get; set; }
    }
}
