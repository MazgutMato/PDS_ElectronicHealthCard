/*
Created: 05.11.2022
Modified: 09.11.2022
Model: SP - database
Database: MS SQL Server 2019
*/


-- Create tables section -------------------------------------------------

-- Table person

CREATE TABLE [person]
(
 [person_id] Char(10) NOT NULL,
 [first_name] Varchar(20) NOT NULL,
 [last_name] Varchar(30) NOT NULL,
 [ZIP] Char(5) NULL
)
GO
-- Create foreign keys (relationships) section ------------------------------------------------- 


ALTER TABLE [person] ADD CONSTRAINT [Relationship4] FOREIGN KEY ([ZIP]) REFERENCES [city] ([ZIP]) ON UPDATE NO ACTION ON DELETE NO ACTION
GO
-- Add keys for table person

ALTER TABLE [person] ADD CONSTRAINT [PK_person] PRIMARY KEY ([person_id])
GO
-- Create indexes for table person

CREATE INDEX [IX_Relationship4] ON [person] ([ZIP])