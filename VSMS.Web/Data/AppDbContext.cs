using Microsoft.EntityFrameworkCore;
using VSMS.Web.Models;

namespace VSMS.Web.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        // DbSets
        public DbSet<User> Users { get; set; }
        public DbSet<Volunteer> Volunteers { get; set; }
        public DbSet<Coordinator> Coordinators { get; set; }
        public DbSet<Organization> Organizations { get; set; }
        public DbSet<Opportunity> Opportunities { get; set; }
        public DbSet<Application> Applications { get; set; }
        public DbSet<Skill> Skills { get; set; }
        public DbSet<VolunteerSkill> VolunteerSkills { get; set; }
        public DbSet<OpportunitySkill> OpportunitySkills { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // 1. Configure TPT (Table-Per-Type) inheritance mapping
            modelBuilder.Entity<Volunteer>().ToTable("Volunteers");
            modelBuilder.Entity<Coordinator>().ToTable("Coordinators");

            // 2. Configure composite primary key for VolunteerSkill many-to-many junction table
            modelBuilder.Entity<VolunteerSkill>()
                .HasKey(vs => new { vs.VolunteerId, vs.SkillId });

            modelBuilder.Entity<VolunteerSkill>()
                .HasOne(vs => vs.Volunteer)
                .WithMany(v => v.VolunteerSkills)
                .HasForeignKey(vs => vs.VolunteerId);

            modelBuilder.Entity<VolunteerSkill>()
                .HasOne(vs => vs.Skill)
                .WithMany(s => s.VolunteerSkills)
                .HasForeignKey(vs => vs.SkillId);

            // 3. Configure composite primary key for OpportunitySkill many-to-many junction table
            modelBuilder.Entity<OpportunitySkill>()
                .HasKey(os => new { os.OpportunityId, os.SkillId });

            modelBuilder.Entity<OpportunitySkill>()
                .HasOne(os => os.Opportunity)
                .WithMany(o => o.OpportunitySkills)
                .HasForeignKey(os => os.OpportunityId);

            modelBuilder.Entity<OpportunitySkill>()
                .HasOne(os => os.Skill)
                .WithMany(s => s.OpportunitySkills)
                .HasForeignKey(os => os.SkillId);
        }
    }
}
