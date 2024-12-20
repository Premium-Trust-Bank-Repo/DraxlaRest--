using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Domain;

namespace Mapping
{
    public class TokenTrailMap : EntityTypeConfiguration<TokenTrail>
    {
        public TokenTrailMap()
        {
            // Primary Key
            this.HasKey(t => t.tbid);

            // Properties
            this.Property(t => t.userId)
                .HasMaxLength(50);

            this.Property(t => t.tokenSerialNumber)
                .HasMaxLength(50);

            this.Property(t => t.operation)
                .HasMaxLength(50);

            this.Property(t => t.validationResponse)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("TokenTrail", "iTokenR2TokenRecords");
            this.Property(t => t.tbid).HasColumnName("tbid");
            this.Property(t => t.userId).HasColumnName("userId");
            this.Property(t => t.tokenSerialNumber).HasColumnName("tokenSerialNumber");
            this.Property(t => t.operation).HasColumnName("operation");
            this.Property(t => t.validationResponse).HasColumnName("validationResponse");
            this.Property(t => t.operationDate).HasColumnName("operationDate");
        }
    }
}
