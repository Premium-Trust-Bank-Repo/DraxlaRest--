using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Domain;

namespace Mapping
{
    public class adminMap : EntityTypeConfiguration<admin>
    {
        public adminMap()
        {
            // Primary Key
            this.HasKey(t => t.userID);

            // Properties
            this.Property(t => t.userName)
                .HasMaxLength(20);

            this.Property(t => t.password)
                .HasMaxLength(200);

            this.Property(t => t.fullName)
                .HasMaxLength(100);

            this.Property(t => t.emailAddress)
                .HasMaxLength(70);

            this.Property(t => t.mobileNo)
                .HasMaxLength(20);

            this.Property(t => t.multipleLogin)
                .IsFixedLength()
                .HasMaxLength(1);

            this.Property(t => t.isFirstLogin)
                .IsFixedLength()
                .HasMaxLength(1);

            this.Property(t => t.enforcePswdChange)
                .IsFixedLength()
                .HasMaxLength(1);

            // Table & Column Mappings
            this.ToTable("admin", "iTokenR2Users");
            this.Property(t => t.userID).HasColumnName("userID");
            this.Property(t => t.userName).HasColumnName("userName");
            this.Property(t => t.password).HasColumnName("password");
            this.Property(t => t.roleID).HasColumnName("roleID");
            this.Property(t => t.fullName).HasColumnName("fullName");
            this.Property(t => t.emailAddress).HasColumnName("emailAddress");
            this.Property(t => t.mobileNo).HasColumnName("mobileNo");
            this.Property(t => t.lastLogoutDate).HasColumnName("lastLogoutDate");
            this.Property(t => t.lastLoginDate).HasColumnName("lastLoginDate");
            this.Property(t => t.currentLoginDate).HasColumnName("currentLoginDate");
            this.Property(t => t.multipleLogin).HasColumnName("multipleLogin");
            this.Property(t => t.accessDays).HasColumnName("accessDays");
            this.Property(t => t.passwordExpiry).HasColumnName("passwordExpiry");
            this.Property(t => t.status).HasColumnName("status");
            this.Property(t => t.isFirstLogin).HasColumnName("isFirstLogin");
            this.Property(t => t.enforcePswdChange).HasColumnName("enforcePswdChange");
            this.Property(t => t.loginStatus).HasColumnName("loginStatus");
            this.Property(t => t.loginCount).HasColumnName("loginCount");
            this.Property(t => t.createdBy).HasColumnName("createdBy");
            this.Property(t => t.dateCreated).HasColumnName("dateCreated");
            this.Property(t => t.authorisedBy).HasColumnName("authorisedBy");
            this.Property(t => t.dateAuthorised).HasColumnName("dateAuthorised");
            this.Property(t => t.modifiedBy).HasColumnName("modifiedBy");
            this.Property(t => t.dateModifiedBy).HasColumnName("dateModifiedBy");
            this.Property(t => t.source).HasColumnName("source");
            this.Property(t => t.accesstype).HasColumnName("accesstype");
        }
    }
}
