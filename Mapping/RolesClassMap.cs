using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Domain;

namespace Mapping
{
    public class RolesClassMap : EntityTypeConfiguration<RolesClass>
    {
        public RolesClassMap()
        {
            // Primary Key
            this.HasKey(t => t.roleId);

            // Properties
            this.Property(t => t.roleName)
                .HasMaxLength(50);

            this.Property(t => t.status)
                .HasMaxLength(10);

            // Table & Column Mappings
            this.ToTable("RolesClasses", "iTokenR2SysAccess");
            this.Property(t => t.roleId).HasColumnName("roleId");
            this.Property(t => t.roleName).HasColumnName("roleName");
            this.Property(t => t.dateCreated).HasColumnName("dateCreated");
            this.Property(t => t.userId).HasColumnName("userId");
            this.Property(t => t.status).HasColumnName("status");
        }
    }
}
