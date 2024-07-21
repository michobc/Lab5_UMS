# LAB 5: Web API UMS (University Management System)

## MISSION
In this lab you will create a Web API UMS (University Management System).
An admin Create a course and set the Maximum number of students allowed and the
enrollment date range
A Teacher registers that s/he can teach this course and create the time slot that s/he can teach
at and can assign the course that they can teach to a time slot.
A student can enroll in a course if he is trying in the date range allowed.

## VISION
The Web API should be designed using the DDD architecture, use the Database provided run
the script in postgres console, Git is a must every feature should be on branch ex:
feature/{counter}-{small-description}, Create the required endpoints to apply the logic
described above.
Some of the GET endpoints must be using OData
Add Caching to your persistence layer for 30 min lifetime.
Add API versioning, you should be able to control from App settings.
Add a feature as a teacher I must be able to set a grade for a student for a course, the Grade
average of a student must be update every time a teacher adds a grade for a student and add a
Boolean value must be update in case the student average is greater than 15 the Boolean value
CanApplyToFrance is true.
As a Plus: Add upload profile picture for the user NB: The DB needs update to support this
feature save the files in wwwroot.

### Commands Used:
#### add / remove reference command:
dotnet add reference ../LabSession5.Infrastructure/LabSession5.Infrastructure.csproj
dotnet remove reference ../LabSession5.Infrastructure/LabSession5.Infrastructure.csproj

### Dependencies installed:
Microsoft.EntityFrameworkCore.Tools
Microsoft.EntityFrameworkCore.Design
Npgsql.EntityFrameworkCore.PostgreSQL

### Design patterns and logic:
Mediator Dp: to reduce chaotic dependencies between objects.
CQRS (Command and Query Responsibility Segregation): a pattern that separates read and update operations for a data store.

### Migrations and code-first:
dotnet ef migrations add InitialCreate
dotnet ef database update

# Michel BOU CHAHINE
## inmind.ai