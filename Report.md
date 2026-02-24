[TOC]

# Report



## 1. ERD

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

## 2. The ViewModels Used

In the VSMS.Web project, ViewModels are used to bridge the gap between your database models and what is actually displayed on the screen. They handle UI-specific logic, like populating dropdowns or managing checkbox lists, that doesn't belong in the core data entities.

Here is a breakdown of the specific ViewModels I implemented:

**Dropdown Support ViewModels**
These ViewModels are used to handle Foreign Key relationships (like assigning a Coordinator to an Organization) by providing the underlying model along with a list for a dropdown menu.

  - **ApplicationViewModel**: Contains an Application object and two lists (Volunteers and Opportunities). This allows the user to select a volunteer and an opportunity by name from a dropdown when creating a new application.

  - **CoordinatorViewModel**: Wraps the Coordinator model and provides an Organizations list so you can easily pick which organization a coordinator belongs to.

  - **OpportunityViewModel**: Similar to the coordinator version, it wraps an Opportunity and provides the Organizations list for the UI.

**Multi-Select (Many-to-Many) ViewModels**
These are more complex because they handle the Skill assignments. Since a Volunteer or an Opportunity can have many skills, we use these to render checkboxes.

  - **VolunteerSkillViewModel**: Contains the Volunteer data.
  - **AvailableSkills**: A list of all possible skills in the database (used to generate the checkboxes).
  - **SelectedSkillIds**: A list of GUIDs representing which checkboxes the user actually clicked.
  - **OpportunitySkillViewModel**: Works exactly like the volunteer version but for assigning "Required Skills" to a specific volunteer opportunity.

## 3. Team Contribution Summary

The project has been fully vertically sliced, all services are registered via Dependency Injection, and the database has been successfully migrated.

### Bo Yang: Project Initialization & Coordinators Management

**Status: Completed**

- **Infrastructure**: Initialized the ASP.NET Core MVC project, Models that discussed, configured EF Core with SQLite, and set up the AppDbContext.
- **Coordinators Module**: Built the CoordinatorService, and full CRUD views to manage helper Coordinators.
- **Navigation**: Established the initial _Layout.cshtml framework.

### Marieth: Volunteers User Profile & Organization

**Status: Completed**

- **Volunteer Logic**: Created the VolunteerViewModel to ensure that when a Organization is added, the system correctly provides a dropdown for selecting their parent Organization.
- **Services**: Delivered VolunteerService and CoordinatorService to handle personnel data lifecycle.

### Bo Zhang: Opportunities & Skill Taxonomy

**Status: Completed**

- **Skill System**: Developed the Skill Module to allow tagging opportunities with specific requirements.
- **Opportunity Management**: Built the OpportunityViewModel and OpportunitiesController, enabling organizations to post volunteer openings with specific skill tags.
- **Taxonomy**: Provided the UI for managing the system's list of available professional and helper skills.

### Chunxi Zhang: Application Flow, Migrations & Final Polish

**Status: Completed**

- **The "Glue"**: Implemented the Application Module, linking Volunteers to Opportunities.
- **Workflow**: Created the ApplicationViewModel with complex double-dropdown logic for selecting both participants and events.
- **Release Management**:
  - **UI Polish**: Performed a global cleanup of the action links (removed the | pipes) across all index views for a premium Bootstrap aesthetic.
  - **Final Validation**: Resolved all null-reference and dropdown validation warnings (e.g., implementing Guid.Empty checks).

## 4. Screenshots of all CRUD pages

This report documents the chronological end-to-end user journey including Create, Read (Index & Details), Update (Edit), and Delete operations for each module.

### 0. Home Page
![Home Page](Screenshots/00_HomePage.png)

### 1. Organizations Module
#### 1.1 Organization Index (Empty)

![Organization Index (Empty)](Screenshots/01_Org_Index_Empty.png)

#### 1.2 Organization Create (Filled)

![Organization Create (Filled)](Screenshots/02_Org_Create.png)

#### 1.3 Organization Index (Populated)

![Organization Index (Populated)](Screenshots/03_Org_Index_Populated.png)

#### 1.4 Organization Details

![Organization Details](Screenshots/04_Org_Details.png)

#### 1.5 Organization Edit (Filled)

![Organization Edit (Filled)](Screenshots/05_Org_Edit.png)

#### 1.6 Organization Delete Format

![Organization Delete Format](Screenshots/06_Org_Delete.png)

### 2. Skills Module

#### 2.1 Skill Index (Empty)
![Skill Index (Empty)](Screenshots/07_Skill_Index_Empty.png)

#### 2.2 Skill Create (Filled)
![Skill Create (Filled)](Screenshots/08_Skill_Create.png)

#### 2.3 Skill Index (Populated)
![Skill Index (Populated)](Screenshots/09_Skill_Index_Populated.png)

#### 2.4 Skill Details
![Skill Details](Screenshots/10_Skill_Details.png)

#### 2.5 Skill Edit (Filled)
![Skill Edit (Filled)](Screenshots/11_Skill_Edit.png)

#### 2.6 Skill Delete Format
![Skill Delete Format](Screenshots/12_Skill_Delete.png)

### 3. Coordinators Module
#### 3.1 Coordinator Create (Filled)
![Coordinator Create (Filled)](Screenshots/13_Coordinator_Create.png)

#### 3.2 Coordinator Index (Populated)
![Coordinator Index (Populated)](Screenshots/14_Coordinator_Index.png)

#### 3.3 Coordinator Details
![Coordinator Details](Screenshots/15_Coordinator_Details.png)

#### 3.4 Coordinator Edit (Filled)
![Coordinator Edit (Filled)](Screenshots/16_Coordinator_Edit.png)

#### 3.5 Coordinator Delete Format
![Coordinator Delete Format](Screenshots/17_Coordinator_Delete.png)

### 4. Volunteers Module
#### 4.1 Volunteer Create (Filled, Skills Checked)
![Volunteer Create (Filled, Skills Checked)](Screenshots/18_Volunteer_Create.png)

#### 4.2 Volunteer Index (Populated)
![Volunteer Index (Populated)](Screenshots/19_Volunteer_Index.png)

#### 4.3 Volunteer Details
![Volunteer Details](Screenshots/20_Volunteer_Details.png)

#### 4.4 Volunteer Edit (Filled)
![Volunteer Edit (Filled)](Screenshots/21_Volunteer_Edit.png)

#### 4.5 Volunteer Delete Format
![Volunteer Delete Format](Screenshots/22_Volunteer_Delete.png)

### 5. Opportunities Module
#### 5.1 Opportunity Create (Filled, Skills Checked)
![Opportunity Create (Filled, Skills Checked)](Screenshots/23_Opportunity_Create.png)

#### 5.2 Opportunity Index (Populated)
![Opportunity Index (Populated)](Screenshots/24_Opportunity_Index.png)

#### 5.3 Opportunity Details
![Opportunity Details](Screenshots/25_Opportunity_Details.png)

#### 5.4 Opportunity Edit (Filled)
![Opportunity Edit (Filled)](Screenshots/26_Opportunity_Edit.png)

#### 5.5 Opportunity Delete Format
![Opportunity Delete Format](Screenshots/27_Opportunity_Delete.png)

### 6. Applications Module
#### 6.1 Application Create (Filled)
![Application Create (Filled)](Screenshots/28_Application_Create.png)

#### 6.2 Application Index (Populated)
![Application Index (Populated)](Screenshots/29_Application_Index.png)

#### 6.3 Application Details
![Application Details](Screenshots/30_Application_Details.png)

#### 6.4 Application Edit (Filled)
![Application Edit (Filled)](Screenshots/31_Application_Edit.png)

#### 6.5 Application Delete Format
![Application Delete Format](Screenshots/32_Application_Delete.png)

