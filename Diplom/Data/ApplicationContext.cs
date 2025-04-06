using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Diplom.Data;
using Diplom.Models;

namespace Diplom.Data
{
    internal class ApplicationContext : DbContext
    {
        public DbSet<Employees> Employees { get; set; }

        public DbSet<Cars> Cars { get; set; }
        
        public DbSet<CarStatus> CarStatus { get; set; }

        public DbSet<DevCar> DevCar { get; set; }



        public ApplicationContext() : base("MyDbContext") { }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Cars>()
                .Property(a => a.CarStatusID)
                .HasColumnName("CarStatusID");

        }


    }


}
