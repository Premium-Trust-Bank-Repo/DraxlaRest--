using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Domain;

namespace Mapping
{
    public class digipassMap : EntityTypeConfiguration<digipass>
    {
        public digipassMap()
        {
            // Primary Key
            this.HasKey(t => t.id);

            // Properties
            this.Property(t => t.serial_no)
                .HasMaxLength(46);

            this.Property(t => t.vds_appl_name)
                .HasMaxLength(200);

            this.Property(t => t.dp_type)
                .HasMaxLength(32);

            this.Property(t => t.blob)
                .HasMaxLength(10000000);

            this.Property(t => t.byte_blob)
                .HasMaxLength(10000000);

            this.Property(t => t.blob_backup)
                .HasMaxLength(2000000);

            this.Property(t => t.user_id)
                .HasMaxLength(255);

            this.Property(t => t.device_id)
                .HasMaxLength(8);

            this.Property(t => t.batch_no)
                .HasMaxLength(255);


            this.Property(t => t.auth_code)
                .HasMaxLength(2000);

            this.Property(t => t.token_type)
             .HasMaxLength(1);

            this.Property(t => t.mode)
            .HasMaxLength(20);

            // Table & Column Mappings
            this.ToTable("digipass", "");
            this.Property(t => t.id).HasColumnName("id");
            this.Property(t => t.serial_no).HasColumnName("serial_no");
            this.Property(t => t.vds_appl_name).HasColumnName("vds_appl_name");
            this.Property(t => t.dp_type).HasColumnName("dp_type");
            this.Property(t => t.mode).HasColumnName("mode");
            this.Property(t => t.blob).HasColumnName("blob");
            this.Property(t => t.byte_blob).HasColumnName("byte_blob");
            this.Property(t => t.blob_backup).HasColumnName("blob_backup");
            this.Property(t => t.blob_update).HasColumnName("blob_update");
            this.Property(t => t.user_id).HasColumnName("user_id");
            this.Property(t => t.assigned_date).HasColumnName("assigned_date");
            this.Property(t => t.assigned_by).HasColumnName("assigned_by");
            this.Property(t => t.expiry_date).HasColumnName("expiry_date");
            this.Property(t => t.is_enabled).HasColumnName("is_enabled");
            this.Property(t => t.direct_assign).HasColumnName("direct_assign");
            this.Property(t => t.last_active_time).HasColumnName("last_active_time");
            this.Property(t => t.created_date).HasColumnName("created_date");
            this.Property(t => t.modified_by).HasColumnName("modified_by");
            this.Property(t => t.modified_date).HasColumnName("modified_date");
            this.Property(t => t.bind_status).HasColumnName("bind_status");
            this.Property(t => t.device_id).HasColumnName("device_id");
            this.Property(t => t.device_type).HasColumnName("device_type");
            this.Property(t => t.platform_details).HasColumnName("platform_details");
            this.Property(t => t.activation_medium).HasColumnName("activation_medium");
            this.Property(t => t.batch_no).HasColumnName("batch_no");
            this.Property(t => t.auth_code).HasColumnName("auth_code");
            this.Property(t => t.token_type).HasColumnName("token_type");
            this.Property(t => t.payload_key).HasColumnName("payload_key");
            this.Property(t => t.sequence_no_threshold).HasColumnName("sequence_no_threshold");
            this.Property(t => t.sequence_no_used).HasColumnName("sequence_no_used");
            this.Property(t => t.custum_1).HasColumnName("custum_1");
            this.Property(t => t.custum_2).HasColumnName("custum_2");

        }
    }
}
