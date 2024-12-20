using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Domain;

namespace Mapping
{
    public class digipass_usersMap : EntityTypeConfiguration<digipass_users>
    {

        public digipass_usersMap()
        {
            // Primary Key
            this.HasKey(t => t.id);



            // Table & Column Mappings
            this.ToTable("digipass_users", "");
            this.Property(t => t.id).HasColumnName("id");
            this.Property(t => t.user_id).HasColumnName("user_id");
            this.Property(t => t.serial_no).HasColumnName("serial_no");
            this.Property(t => t.first_name).HasColumnName("first_name");
            this.Property(t => t.last_name).HasColumnName("last_name");
            this.Property(t => t.middle_name).HasColumnName("middle_name");
            this.Property(t => t.phone).HasColumnName("phone");
            this.Property(t => t.email).HasColumnName("email");
            this.Property(t => t.created_time).HasColumnName("created_time");

        }
    }
}
