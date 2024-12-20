using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Domain;

namespace Mapping
{
    public class rolesAssignmentMap : EntityTypeConfiguration<rolesAssignment>
    {
        public rolesAssignmentMap()
        {
            // Primary Key
            this.HasKey(t => t.roleAssgId);

            // Properties
            this.Property(t => t.status)
                .IsRequired()
                .HasMaxLength(1);

            // Table & Column Mappings
            this.ToTable("rolesAssignment", "iTokenR2SysAccess");
            this.Property(t => t.roleAssgId).HasColumnName("roleAssgId");
            this.Property(t => t.roleId).HasColumnName("roleId");
            this.Property(t => t.menuId).HasColumnName("menuId");
            this.Property(t => t.userId).HasColumnName("userId");
            this.Property(t => t.dateCreated).HasColumnName("dateCreated");
            this.Property(t => t.status).HasColumnName("status");
            this.Property(t => t.canInsert).HasColumnName("canInsert");
            this.Property(t => t.canDelete).HasColumnName("canDelete");
            this.Property(t => t.canUpdate).HasColumnName("canUpdate");
            this.Property(t => t.canauth).HasColumnName("canauth");
            this.Property(t => t.authId).HasColumnName("authId");

            // Relationships
            this.HasOptional(t => t.admin)
                .WithMany(t => t.rolesAssignments)
                .HasForeignKey(d => d.userId);

        }
    }
}
