using System.Collections.Generic;

namespace Medium.Core.Contracts.V1.Response
{
    public class ErrorResponse
    {
        public IEnumerable<string> Errors { get; set; }
    }
}
