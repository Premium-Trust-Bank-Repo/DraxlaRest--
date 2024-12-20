using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Domain
{
    public class activation
    {
        public string RegistrationIdentifier { get; set; }
        public string Nonce { get; set; }
        public string AuthorizationCode { get; set; }
        public string ReauthorizationCode { get; set; }
        public string Passcode { get; set; }
        public string Accesscode { get; set; }
       //public string Password { get; set; }

    }
}