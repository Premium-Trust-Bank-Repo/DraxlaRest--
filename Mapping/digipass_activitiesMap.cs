using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Domain;

namespace Mapping
{
    public class digipass_activitiesMap : EntityTypeConfiguration<digipass_activities>
    {
        public digipass_activitiesMap()
        {
            // Primary Key
            this.HasKey(t => t.id);

            // Properties
            this.Property(t => t.user_id)
                .HasMaxLength(50);

            this.Property(t => t.serial_no)
                .HasMaxLength(50);

            this.Property(t => t.operation)
                .HasMaxLength(200);

            this.Property(t => t.response)
                .HasMaxLength(200);

            // Table & Column Mappings
            this.ToTable("digipass_activities", "");
            this.Property(t => t.id).HasColumnName("id");
            this.Property(t => t.user_id).HasColumnName("user_id");
            this.Property(t => t.serial_no).HasColumnName("serial_no");
            this.Property(t => t.operation).HasColumnName("operation");
            this.Property(t => t.response).HasColumnName("response");
            this.Property(t => t.operation_date).HasColumnName("operation_date");
        }
    }
}
