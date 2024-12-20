using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Domain;

namespace Mapping
{
    public class menuControlMap : EntityTypeConfiguration<menuControl>
    {
        public menuControlMap()
        {
            // Primary Key
            this.HasKey(t => t.menuId);

            // Properties
            this.Property(t => t.menuId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.menuName)
                .IsRequired()
                .HasMaxLength(100);

            this.Property(t => t.url)
                .HasMaxLength(300);

            this.Property(t => t.description)
                .HasMaxLength(100);

            this.Property(t => t.tablename)
                .HasMaxLength(60);

            this.Property(t => t.imageUrl)
                .HasMaxLength(100);

            // Table & Column Mappings
            this.ToTable("menuControl", "iTokenR2SysAccess");
            this.Property(t => t.menuId).HasColumnName("menuId");
            this.Property(t => t.menuName).HasColumnName("menuName");
            this.Property(t => t.url).HasColumnName("url");
            this.Property(t => t.parent).HasColumnName("parent");
            this.Property(t => t.Priority).HasColumnName("Priority");
            this.Property(t => t.description).HasColumnName("description");
            this.Property(t => t.tablename).HasColumnName("tablename");
            this.Property(t => t.imageUrl).HasColumnName("imageUrl");
        }
    }
}
