using System;
using System.Collections.Generic;

namespace Domain
{
    public partial class UploadedFileDetail
    {
        public int tbid { get; set; }
        public Nullable<System.DateTime> dateUploaded { get; set; }
        public Nullable<int> tokenType { get; set; }
        public string batchNo { get; set; }
        public string staticVector { get; set; }
        public string messageVector { get; set; }
        public string transportKey { get; set; }
        public Nullable<int> tokenCount { get; set; }
        public Nullable<int> uploadedBy { get; set; }
    }
}
