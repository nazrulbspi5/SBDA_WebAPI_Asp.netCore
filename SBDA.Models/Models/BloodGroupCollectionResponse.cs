using System;
using System.Collections.Generic;
using System.Text;

namespace SBDA.Models.Models
{
    class BloodGroupCollectionResponse:BaseAPIResponse
    {
        public BloodGroup[] Records { get; set; }
    }
}
