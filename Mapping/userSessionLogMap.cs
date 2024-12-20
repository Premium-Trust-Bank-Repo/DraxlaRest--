using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Domain;

namespace Mapping
{
    public class userSessionLogMap : EntityTypeConfiguration<userSessionLog>
    {
        public userSessionLogMap()
        {
            // Primary Key
            this.HasKey(t => t.tbid);

            // Properties
            this.Property(t => t.isActive)
                .IsFixedLength()
                .HasMaxLength(10);

            this.Property(t => t.clientName)
                .HasMaxLength(50);

            this.Property(t => t.ipAddress)
                .HasMaxLength(50);

            this.Property(t => t.mac)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("userSessionLog", "iTokenR2Users");
            this.Property(t => t.tbid).HasColumnName("tbid");
            this.Property(t => t.userID).HasColumnName("userID");
            this.Property(t => t.loginTime).HasColumnName("loginTime");
            this.Property(t => t.logoutTime).HasColumnName("logoutTime");
            this.Property(t => t.lastActionDate).HasColumnName("lastActionDate");
            this.Property(t => t.isActive).HasColumnName("isActive");
            this.Property(t => t.clientName).HasColumnName("clientName");
            this.Property(t => t.ipAddress).HasColumnName("ipAddress");
            this.Property(t => t.mac).HasColumnName("mac");

            // Relationships
            this.HasOptional(t => t.admin)
                .WithMany(t => t.userSessionLogs)
                .HasForeignKey(d => d.userID);

        }
    }
}
