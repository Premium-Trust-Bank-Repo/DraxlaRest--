using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using Domain;
using Mapping;
using DataAccessLayer;

namespace VacmanBoundContext
{
    public class VacmanContext : connRec.BaseContext2<VacmanContext>
    {
        public DbSet<digipass> digipasses { get; set; }
        public DbSet<token_activation> tokenActivations { get; set; }
        public DbSet<uploaded_file_details> uploadedFileDetails { get; set; }
        public DbSet<TokenTran> TokenTrans { get; set; }
        public DbSet<digipass_activities> digipass_activities { get; set; }
        public DbSet<instance> instances { get; set; }
        public DbSet<digipass_users> digipass_users { get; set; }



        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new digipassMap());
            modelBuilder.Configurations.Add(new  token_activationMap());
            modelBuilder.Configurations.Add(new uploaded_file_detailMap());
            modelBuilder.Configurations.Add(new TokenTranMap());
            modelBuilder.Configurations.Add(new digipass_activitiesMap());
            modelBuilder.Configurations.Add(new instanceMap());
        }
    }

}
