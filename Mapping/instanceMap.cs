using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Domain;

namespace Mapping
{
    public class instanceMap : EntityTypeConfiguration<instance>
    {
        public instanceMap()
        {
            // Primary Key
            this.HasKey(t => t.id);
           
            // Table & Column Mappings
            this.ToTable("instance", "");
            this.Property(t => t.id).HasColumnName("id");
            this.Property(t => t.serial_no).HasColumnName("serial_no");
            this.Property(t => t.sequence_no).HasColumnName("sequence_no");
            this.Property(t => t.dp_type).HasColumnName("dp_type");
            this.Property(t => t.application_names).HasColumnName("application_names");
            this.Property(t => t.mode).HasColumnName("mode");
            this.Property(t => t.appl_count).HasColumnName("appl_count");
            this.Property(t => t.blob).HasColumnName("blob");
            this.Property(t => t.byte_blob).HasColumnName("byte_blob");
            this.Property(t => t.blob_backup).HasColumnName("blob_backup");
            this.Property(t => t.assign_date).HasColumnName("assign_date");
            this.Property(t => t.device_id).HasColumnName("device_id");
            this.Property(t => t.device_type).HasColumnName("device_type");
            this.Property(t => t.platform_details).HasColumnName("platform_details");
            this.Property(t => t.activation_medium).HasColumnName("activation_medium");
            this.Property(t => t.status).HasColumnName("status");
            this.Property(t => t.created_by).HasColumnName("created_by");
            this.Property(t => t.created_date).HasColumnName("created_date");

        }

    }
}
