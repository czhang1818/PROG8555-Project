# PROG8555-Project

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

