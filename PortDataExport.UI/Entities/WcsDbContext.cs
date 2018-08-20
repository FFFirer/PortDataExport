namespace PortDataExport.UI.Entities
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class WcsDbContext : DbContext
    {
        public WcsDbContext()
            : base("name=WcsDbContext")
        {
        }

        public virtual DbSet<alreadysortinfo> alreadysortinfoes { get; set; }
        public virtual DbSet<alreadysortinfohist> alreadysortinfohists { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<alreadysortinfo>()
                .Property(e => e.billCode)
                .IsUnicode(false);

            modelBuilder.Entity<alreadysortinfo>()
                .Property(e => e.trayCode)
                .IsUnicode(false);

            modelBuilder.Entity<alreadysortinfo>()
                .Property(e => e.pipeline)
                .IsUnicode(false);

            modelBuilder.Entity<alreadysortinfo>()
                .Property(e => e.sortTime)
                .IsUnicode(false);

            modelBuilder.Entity<alreadysortinfo>()
                .Property(e => e.turnNumber)
                .IsUnicode(false);

            modelBuilder.Entity<alreadysortinfo>()
                .Property(e => e.sortPortCode)
                .IsUnicode(false);

            modelBuilder.Entity<alreadysortinfo>()
                .Property(e => e.sortSource)
                .IsUnicode(false);

            modelBuilder.Entity<alreadysortinfo>()
                .Property(e => e.sendStatus)
                .IsUnicode(false);

            modelBuilder.Entity<alreadysortinfo>()
                .Property(e => e.userID)
                .IsUnicode(false);

            modelBuilder.Entity<alreadysortinfohist>()
                .Property(e => e.billCode)
                .IsUnicode(false);

            modelBuilder.Entity<alreadysortinfohist>()
                .Property(e => e.trayCode)
                .IsUnicode(false);

            modelBuilder.Entity<alreadysortinfohist>()
                .Property(e => e.pipeline)
                .IsUnicode(false);

            modelBuilder.Entity<alreadysortinfohist>()
                .Property(e => e.sortTime)
                .IsUnicode(false);

            modelBuilder.Entity<alreadysortinfohist>()
                .Property(e => e.turnNumber)
                .IsUnicode(false);

            modelBuilder.Entity<alreadysortinfohist>()
                .Property(e => e.sortPortCode)
                .IsUnicode(false);

            modelBuilder.Entity<alreadysortinfohist>()
                .Property(e => e.sortSource)
                .IsUnicode(false);

            modelBuilder.Entity<alreadysortinfohist>()
                .Property(e => e.sendStatus)
                .IsUnicode(false);
        }
    }
}
