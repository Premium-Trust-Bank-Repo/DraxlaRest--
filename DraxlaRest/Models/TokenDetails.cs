using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DraxlaRest.Models
{
    public class TokenDetails
    {
        public string code_word { get; set; }
        public int error_count { get; set; }
        public string is_locked { get; set; }
        public string last_time_shift { get; set; }
        public string last_time_used { get; set; }
        public string max_input_fields { get; set; }
        public string response_checksum { get; set; }
        public int response_length { get; set; }
        public string response_type { get; set; }
        public int time_step_used { get; set; }
        public string token_model { get; set; }
        public string triple_des { get; set; }
        public int use_count { get; set; }
        public int hash_code { get; set; }
        public string blob { get; set; }
        public int return_code { get; set; }
        public string return_msg { get; set; }
        public int sequence_no { get; set; }
        public string operation_type { get; set; }
        public string OTP { get; set; }

    }
}