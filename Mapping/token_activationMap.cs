using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Domain;

namespace Mapping
{
    public class token_activationMap : EntityTypeConfiguration<token_activation>
    {
        public token_activationMap()
        {
            // Primary Key
            this.HasKey(t => t.id);

            // Properties

            this.Property(t => t.user_id)
                .HasMaxLength(20);

            this.Property(t => t.token_serial_number)
                .HasMaxLength(50);

            this.Property(t => t.otp)
                .HasMaxLength(50);

            this.Property(t => t.validation_response)
                .HasMaxLength(5000);

            this.Property(t => t.operation)
                .HasMaxLength(200);
            
            // Table & Column Mappings
            this.ToTable("token_activation", "");
            this.Property(t => t.id).HasColumnName("id");
            this.Property(t => t.user_id).HasColumnName("user_id");
            this.Property(t => t.token_serial_number).HasColumnName("token_serial_number");
            this.Property(t => t.otp).HasColumnName("otp");
            this.Property(t => t.validation_response).HasColumnName("validation_response");
            this.Property(t => t.operation).HasColumnName("operation");
            this.Property(t => t.operation_date).HasColumnName("operation_date");
            this.Property(t => t.status).HasColumnName("status");
        }
    }
}
