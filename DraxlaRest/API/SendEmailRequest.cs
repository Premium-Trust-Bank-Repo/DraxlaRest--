﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DraxlaRest.API
{
    public class SendEmailRequest
    {
        public string emailAddress { get; set; }
        public string messageBody { get; set; }
        public string messageSubject { get; set; }
    }

    public class sendemailResponse
    {
        public string responseMessage { get; set; }
    }
}