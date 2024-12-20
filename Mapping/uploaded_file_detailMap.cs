using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Domain;

namespace Mapping
{
    public class uploaded_file_detailMap : EntityTypeConfiguration<uploaded_file_details>
    {
        public uploaded_file_detailMap()
        {
            // Primary Key
            this.HasKey(t => t.id);

            // Properties
            this.Property(t => t.batch_no)
                .HasMaxLength(255);

            this.Property(t => t.transport_key)
                .HasMaxLength(256);

            // Table & Column Mappings
            this.ToTable("uploaded_file_details", "");
            this.Property(t => t.id).HasColumnName("id");
            this.Property(t => t.date_uploaded).HasColumnName("date_uploaded");
            this.Property(t => t.token_type).HasColumnName("token_type");
            this.Property(t => t.batch_no).HasColumnName("batch_no");
            this.Property(t => t.static_vector).HasColumnName("static_vector");
            this.Property(t => t.message_vector).HasColumnName("message_vector");
            this.Property(t => t.transport_key).HasColumnName("transport_key");
            this.Property(t => t.token_count).HasColumnName("token_count");
            this.Property(t => t.uploaded_by).HasColumnName("uploaded_by");
        }
    }
}
