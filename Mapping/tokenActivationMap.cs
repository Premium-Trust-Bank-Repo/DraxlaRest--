using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Domain;

namespace Mapping
{
    public class tokenActivationMap : EntityTypeConfiguration<tokenActivation>
    {
        public tokenActivationMap()
        {
            // Primary Key
            this.HasKey(t => t.tbid);

            // Properties

            
            
            // Table & Column Mappings
            this.ToTable("tokenActivation", "iTokenR2TokenRecords");
            this.Property(t => t.tbid).HasColumnName("tbid");
            this.Property(t => t.user_id).HasColumnName("user_id");
            this.Property(t => t.tokenSerialNumber).HasColumnName("tokenSerialNumber");
            this.Property(t => t.Otp).HasColumnName("Otp");
            this.Property(t => t.validationResponse).HasColumnName("validationResponse");
            this.Property(t => t.operation).HasColumnName("operation");
            this.Property(t => t.operationDate).HasColumnName("operationDate");
            this.Property(t => t.status).HasColumnName("status");
        }
    }
}
