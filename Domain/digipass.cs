using System;
using System.Collections.Generic;

namespace Domain
{
    public partial class digipass
    {
        public int id { get; set; }
        public string serial_no { get; set; }
        public string vds_appl_name { get; set; }
        public string dp_type { get; set; }
        public string mode { get; set; }
        public string blob { get; set; }
        public byte[] byte_blob { get; set; }
        public string blob_backup { get; set; }
        public string blob_update { get; set; } 
        public string user_id { get; set; }
        public string created_by { get; set; }
        public Nullable<System.DateTime> assigned_date { get; set; }
        public Nullable<System.DateTime> expiry_date { get; set; }
        public Nullable<int> is_enabled { get; set; }
        public Nullable<int> direct_assign { get; set; }
        public Nullable<System.DateTime> last_active_time { get; set; }
        public Nullable<System.DateTime> created_date { get; set; }
        public Nullable<System.DateTime> modified_date { get; set; }
        public Nullable<int> bind_status { get; set; }
        public string device_id { get; set; }
        public Nullable<int> device_type { get; set; }
        public string platform_details { get; set; }
        public string activation_medium { get; set; }

        public string batch_no { get; set; }
        public string auth_code { get; set; }
        public string reauth_code { get; set; }       
        public string token_type { get; set; }
        public Nullable<int> token_state { get; set; }
        public int branch_id { get; set; }
        public string request_id { get; set; }
        public string enabled_by { get; set; }
        public string assigned_by { get; set; }
        public string modified_by { get; set; }
        public Nullable<int> user_type { get; set; }
        public string activation_vector { get; set; }
        public Nullable<int> sequence_no_threshold { get; set; }
        public Nullable<int> sequence_no_used { get; set; }    
        public string payload_key { get; set; }
        public string activation_msg2 { get; set; }
        public string custum_1 { get; set; }
        public string custum_2 { get; set; }


    }
}
