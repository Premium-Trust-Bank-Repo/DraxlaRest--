using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Domain;

namespace Mapping
{
    public class TokenTranMap : EntityTypeConfiguration<TokenTran>
    {
        public TokenTranMap()
        {
            // Primary Key
            this.HasKey(t => t.id);

            // Properties
            this.Property(t => t.trans_name)
                .HasMaxLength(100);

            this.Property(t => t.userid)
                .HasMaxLength(50);

            this.Property(t => t.serial_no)
                .HasMaxLength(10);

            this.Property(t => t.from_acct)
                .HasMaxLength(50);

            this.Property(t => t.to_acct)
                .HasMaxLength(50);

            this.Property(t => t.trans_amt)
                .HasMaxLength(50);

            this.Property(t => t.confirm_code)
                .HasMaxLength(10);

            // Table & Column Mappings
            this.ToTable("TokenTrans", "iTokenR2TokenRecords");
            this.Property(t => t.id).HasColumnName("id");
            this.Property(t => t.trans_name).HasColumnName("trans_name");
            this.Property(t => t.userid).HasColumnName("userid");
            this.Property(t => t.serial_no).HasColumnName("serial_no");
            this.Property(t => t.from_acct).HasColumnName("from_acct");
            this.Property(t => t.to_acct).HasColumnName("to_acct");
            this.Property(t => t.trans_amt).HasColumnName("trans_amt");
            this.Property(t => t.status).HasColumnName("status");
            this.Property(t => t.trans_date).HasColumnName("trans_date");
            this.Property(t => t.confirm_code).HasColumnName("confirm_code");
            this.Property(t => t.confirm_date).HasColumnName("confirm_date");
        }
    }
}
