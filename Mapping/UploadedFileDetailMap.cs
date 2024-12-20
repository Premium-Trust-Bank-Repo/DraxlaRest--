using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Domain;

namespace Mapping
{
    public class UploadedFileDetailMap : EntityTypeConfiguration<UploadedFileDetail>
    {
        public UploadedFileDetailMap()
        {
            // Primary Key
            this.HasKey(t => t.tbid);

            // Properties
            this.Property(t => t.batchNo)
                .HasMaxLength(255);

            this.Property(t => t.transportKey)
                .HasMaxLength(256);

            // Table & Column Mappings
            this.ToTable("UploadedFileDetails", "iTokenR2TokenRecords");
            this.Property(t => t.tbid).HasColumnName("tbid");
            this.Property(t => t.dateUploaded).HasColumnName("dateUploaded");
            this.Property(t => t.tokenType).HasColumnName("tokenType");
            this.Property(t => t.batchNo).HasColumnName("batchNo");
            this.Property(t => t.staticVector).HasColumnName("staticVector");
            this.Property(t => t.transportKey).HasColumnName("transportKey");
            this.Property(t => t.tokenCount).HasColumnName("tokenCount");
            this.Property(t => t.uploadedBy).HasColumnName("uploadedBy");
        }
    }
}
