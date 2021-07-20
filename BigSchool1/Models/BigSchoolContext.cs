using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace BigSchool1.Models
{
    public partial class BigSchoolContext : DbContext
    {
        public BigSchoolContext()
            : base("name=BigSchoolContext")
        {
        }

        public virtual DbSet<Attendence> Attendences { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Cours> Courses { get; set; }
        public virtual DbSet<Following> Followings { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>()
                .HasMany(e => e.Courses)
                .WithRequired(e => e.Category)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Cours>()
                .HasMany(e => e.Attendences)
                .WithRequired(e => e.Cours)
                .HasForeignKey(e => e.CourseId)
                .WillCascadeOnDelete(false);
        }
    }
}
