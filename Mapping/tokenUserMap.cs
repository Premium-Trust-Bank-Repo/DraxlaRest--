using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Domain;

namespace Mapping
{
    public class tokenUserMap : EntityTypeConfiguration<tokenUser>
    {
        public tokenUserMap()
        {
            // Primary Key
            this.HasKey(t => t.tbid);

            // Properties
            this.Property(t => t.userId)
                .IsRequired()
                .HasMaxLength(255);

            this.Property(t => t.userName)
                .HasMaxLength(64);

            this.Property(t => t.phone)
                .HasMaxLength(64);

            this.Property(t => t.email)
                .HasMaxLength(255);

            this.Property(t => t.staticPwd)
                .HasMaxLength(1024);

            this.Property(t => t.linkUserId)
                .HasMaxLength(255);

            // Table & Column Mappings
            this.ToTable("tokenUsers", "iTokenR2TokenRecords");
            this.Property(t => t.tbid).HasColumnName("tbid");
            this.Property(t => t.userId).HasColumnName("userId");
            this.Property(t => t.userName).HasColumnName("userName");
            this.Property(t => t.phone).HasColumnName("phone");
            this.Property(t => t.email).HasColumnName("email");
            this.Property(t => t.staticPwd).HasColumnName("staticPwd");
            this.Property(t => t.linkUserId).HasColumnName("linkUserId");
            this.Property(t => t.lockCount).HasColumnName("lockCount");
            this.Property(t => t.locked).HasColumnName("locked");
            this.Property(t => t.disabled).HasColumnName("disabled");
            this.Property(t => t.roleId).HasColumnName("roleId");
            this.Property(t => t.lastPwdSetTime).HasColumnName("lastPwdSetTime");
            this.Property(t => t.createTime).HasColumnName("createTime");
            this.Property(t => t.modifyTime).HasColumnName("modifyTime");
            this.Property(t => t.lastAuthTime).HasColumnName("lastAuthTime");
            this.Property(t => t.expirationTime).HasColumnName("expirationTime");
        }
    }
}
