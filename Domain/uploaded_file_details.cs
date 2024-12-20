using System;
using System.Collections.Generic;

namespace Domain
{
    public partial class uploaded_file_details
    {
        public int id { get; set; }
        public Nullable<System.DateTime> date_uploaded { get; set; }
        public string token_type { get; set; }
        public string batch_no { get; set; }
        public string static_vector { get; set; }
        public string message_vector { get; set; }
        public string transport_key { get; set; }
        public Nullable<int> token_count { get; set; }
        public string uploaded_by { get; set; }
  

        
    }
}
