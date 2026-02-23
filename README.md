# PROG8555-Project

**Group Members:**

**Bo Yang**

**Bo Zhang**

**Chunxi Zhang**

**Marieth Franciss**

## ERD

``` mermaid
erDiagram
    USER {
        uuid user_id PK
        string email
        string password_hash
        string role
        datetime created_at
    }

    VOLUNTEER {
        uuid volunteer_id PK, FK "References USER(user_id)"
        string name
        string phone_number
        float total_hours
    }

    COORDINATOR {
        uuid coordinator_id PK, FK "References USER(user_id)"
        uuid organization_id FK
        string name
        string job_title
        string phone_number
    }

    ORGANIZATION {
        uuid organization_id PK
        string name
        string contact_email
        string website
        boolean is_verified
    }

    OPPORTUNITY {
        uuid opportunity_id PK
        uuid organization_id FK
        string title
        datetime event_date
        string location
        int max_volunteers
    }

    APPLICATION {
        uuid app_id PK
        uuid volunteer_id FK
        uuid opportunity_id FK
        datetime submission_date
        string status
    }

    SKILL {
        uuid skill_id PK
        string name
        string category
        string description
        boolean requires_certification
    }

    %% Junction Table 1: Volunteer Skills
    VOLUNTEER_SKILL {
        uuid volunteer_id PK, FK
        uuid skill_id PK, FK
        string proficiency_level
        datetime acquired_date
    }

    %% Junction Table 2: Opportunity Skill Requirements
    OPPORTUNITY_SKILL {
        uuid opportunity_id PK, FK
        uuid skill_id PK, FK
        boolean is_mandatory
        string minimum_level
    }

    %% One-to-One / Identity Relationships (Shared Primary Key)
    USER ||--|| VOLUNTEER : "has profile"
    USER ||--|| COORDINATOR : "has profile"

    %% One-to-Many Relationships
    ORGANIZATION ||--o{ OPPORTUNITY : "publishes"
    ORGANIZATION ||--o{ COORDINATOR : "employs"
    VOLUNTEER ||--o{ APPLICATION : "submits"
    OPPORTUNITY ||--o{ APPLICATION : "receives"
    
    %% Many-to-Many Relationships (Junction Tables)
    VOLUNTEER ||--o{ VOLUNTEER_SKILL : "has"
    SKILL ||--o{ VOLUNTEER_SKILL : "belongs to"

    OPPORTUNITY ||--o{ OPPORTUNITY_SKILL : "requires"
    SKILL ||--o{ OPPORTUNITY_SKILL : "is required by"
```

## Models 

``` c#

// 1. Base Class: User (Corresponds to all login accounts)
public class User
{
    [Key]
    public Guid UserId { get; set; } = Guid.NewGuid();
    
    [Required]
    [EmailAddress]
    public string Email { get; set; }
    
    [Required]
    public string PasswordHash { get; set; }
    
    [Required]
    public string Role { get; set; } // e.g., "Volunteer", "Coordinator"
    
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}

// 2. Derived Class: Volunteer (Inherits from User, EF Core automatically uses UserId as PK/FK)
public class Volunteer : User
{
    [Required]
    [StringLength(100)]
    public string Name { get; set; }
    
    [Phone]
    public string PhoneNumber { get; set; }
    
    public float TotalHours { get; set; } = 0;

    // Navigation Properties
    public ICollection<Application> Applications { get; set; }
    public ICollection<VolunteerSkill> VolunteerSkills { get; set; }
}

// 3. Derived Class: Coordinator (Inherits from User)
public class Coordinator : User
{
    [Required]
    public Guid OrganizationId { get; set; } // Foreign Key
    
    [Required]
    [StringLength(100)]
    public string Name { get; set; }
    
    [Required]
    public string JobTitle { get; set; }
    
    [Phone]
    public string PhoneNumber { get; set; }

    // Navigation Property
    [ForeignKey("OrganizationId")]
    public Organization Organization { get; set; }
}

// 4. Organization Table
public class Organization
{
    [Key]
    public Guid OrganizationId { get; set; } = Guid.NewGuid();
    
    [Required]
    [StringLength(150)]
    public string Name { get; set; }
    
    [Required]
    [EmailAddress]
    public string ContactEmail { get; set; }
    
    [Url]
    public string Website { get; set; }
    
    public bool IsVerified { get; set; } = false;

    // Navigation Properties
    public ICollection<Opportunity> Opportunities { get; set; }
    public ICollection<Coordinator> Coordinators { get; set; }
}

// 5. Opportunity Table
public class Opportunity
{
    [Key]
    public Guid OpportunityId { get; set; } = Guid.NewGuid();
    
    [Required]
    public Guid OrganizationId { get; set; } // Foreign Key
    
    [Required]
    [StringLength(200)]
    public string Title { get; set; }
    
    [Required]
    public DateTime EventDate { get; set; }
    
    [Required]
    public string Location { get; set; }
    
    [Range(1, 1000)]
    public int MaxVolunteers { get; set; }

    // Navigation Properties
    [ForeignKey("OrganizationId")]
    public Organization Organization { get; set; }
    public ICollection<Application> Applications { get; set; }
    public ICollection<OpportunitySkill> OpportunitySkills { get; set; }
}

// 6. Application Table
public class Application
{
    [Key]
    public Guid AppId { get; set; } = Guid.NewGuid();
    
    [Required]
    public Guid VolunteerId { get; set; } // Foreign Key (Points to Volunteer's UserId)
    
    [Required]
    public Guid OpportunityId { get; set; } // Foreign Key
    
    public DateTime SubmissionDate { get; set; } = DateTime.Now;
    
    public string Status { get; set; } = "Pending";

    // Navigation Properties
    [ForeignKey("VolunteerId")]
    public Volunteer Volunteer { get; set; }
    
    [ForeignKey("OpportunityId")]
    public Opportunity Opportunity { get; set; }
}

// 7. Skill Dictionary Table
public class Skill
{
    [Key]
    public Guid SkillId { get; set; } = Guid.NewGuid();
    
    [Required]
    public string Name { get; set; }
    
    public string Category { get; set; }
    
    public string Description { get; set; }
    
    public bool RequiresCertification { get; set; }

    // Navigation Properties
    public ICollection<VolunteerSkill> VolunteerSkills { get; set; }
    public ICollection<OpportunitySkill> OpportunitySkills { get; set; }
}

// 8. Junction Table 1: Volunteer - Skill
public class VolunteerSkill
{
    public Guid VolunteerId { get; set; } // Composite PK / FK
    public Guid SkillId { get; set; }     // Composite PK / FK
    
    public string ProficiencyLevel { get; set; } 
    public DateTime AcquiredDate { get; set; }

    public Volunteer Volunteer { get; set; }
    public Skill Skill { get; set; }
}

// 9. Junction Table 2: Opportunity - Skill
public class OpportunitySkill
{
    public Guid OpportunityId { get; set; } // Composite PK / FK
    public Guid SkillId { get; set; }       // Composite PK / FK
    
    public bool IsMandatory { get; set; }
    public string MinimumLevel { get; set; }

    public Opportunity Opportunity { get; set; }
    public Skill Skill { get; set; }
}

```



## DbContext

``` c#

public class AppDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
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
            
        // 4. The remaining one-to-many relationships will be automatically inferred by EF Core 
        // based on Navigation Properties
    }
}
```

