CREATE database company;

use company
create table peoples
(
id UNIQUEIDENTIFIER primary key, 
email nvarchar(20) not null,
first_name nvarchar(20) not null,
last_name nvarchar(20) not null,
role nvarchar(20) check(role IN('manager', 'intern')),
)

create table internships
(
id UNIQUEIDENTIFIER primary key, 
intern_id UNIQUEIDENTIFIER foreign key references peoples(id),
manager_id UNIQUEIDENTIFIER foreign key references peoples(id),
start_date date not null,
end_date date not null,
)

create table internships_goals
(
id UNIQUEIDENTIFIER primary key, 
internship_id UNIQUEIDENTIFIER foreign key references internships(id),
deadline_date date not null,
name nvarchar(20) not null,
is_completed bit,
)