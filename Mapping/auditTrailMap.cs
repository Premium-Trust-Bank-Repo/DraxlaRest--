using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Domain;

namespace Mapping
{
    public class auditTrailMap : EntityTypeConfiguration<auditTrail>
    {
        public auditTrailMap()
        {
            // Primary Key
            this.HasKey(t => t.tbid);

            // Properties
            this.Property(t => t.tableName)
                .HasMaxLength(50);

            this.Property(t => t.columnName)
                .HasMaxLength(50);

            this.Property(t => t.actionPerformed)
                .HasMaxLength(50);

            this.Property(t => t.oldValue)
                .HasMaxLength(50);

            this.Property(t => t.newValue)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("auditTrail", "iTokenR2Users");
            this.Property(t => t.tbid).HasColumnName("tbid");
            this.Property(t => t.userId).HasColumnName("userId");
            this.Property(t => t.tableName).HasColumnName("tableName");
            this.Property(t => t.columnName).HasColumnName("columnName");
            this.Property(t => t.actionPerformed).HasColumnName("actionPerformed");
            this.Property(t => t.oldValue).HasColumnName("oldValue");
            this.Property(t => t.newValue).HasColumnName("newValue");
            this.Property(t => t.date).HasColumnName("date");
        }
    }
}
