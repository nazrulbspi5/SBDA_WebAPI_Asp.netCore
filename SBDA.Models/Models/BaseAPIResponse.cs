using System;
using System.Collections.Generic;
using System.Text;

namespace SBDA.Models.Models
{
    public class BaseAPIResponse
    {
        public string Message { get; set; }
        public bool IsSuccess { get; set; }
    }
}
