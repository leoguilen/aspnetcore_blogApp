﻿using System.Collections.Generic;

namespace Medium.Core.Contracts.V1.Response
{
    public class AuthFailedResponse
    {
        public IEnumerable<string> Errors { get; set; }
    }
}
