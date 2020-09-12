using System.Collections.Generic;

namespace Medium.Core.Contracts.V1.Response.Authentication
{
    public class AuthFailedResponse
    {
        public IEnumerable<string> Errors { get; set; }
    }
}
