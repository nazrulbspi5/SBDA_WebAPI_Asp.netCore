using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SBDA.API.AuthModels
{
    public class UserManagerResponse
    {
        public string Message { get; set; }
        public bool IsSuccess { get; set; }
        public Dictionary<string, string> UserInfo { get; set; }
        public IEnumerable<string> Errors { get; set; }
        public DateTime? ExpireDate { get; set; }
    }
}
