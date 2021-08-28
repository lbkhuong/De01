using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace De01.Model
{
    public partial class DBContextSV : DbContext
    {
        public DBContextSV()
            : base("name=DBContextSV")
        {
        }

        public virtual DbSet<Lop> Lop { get; set; }
        public virtual DbSet<SinhVien> SinhVien { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Lop>()
                .Property(e => e.MaLop)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Lop>()
                .HasMany(e => e.SinhVien)
                .WithRequired(e => e.Lop)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<SinhVien>()
                .Property(e => e.MaSV)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<SinhVien>()
                .Property(e => e.MaLop)
                .IsFixedLength()
                .IsUnicode(false);
        }
    }
}
