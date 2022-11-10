/*
Created: 5. 11. 2022
Modified: 10. 11. 2022
Model: SP - database
Database: MS SQL Server 2019
*/


-- Create tables section -------------------------------------------------

-- Table person

CREATE TABLE [person]
(
 [person_id] Char(10) NOT NULL,
 [ZIP] Char(5) NOT NULL,
 [first_name] Varchar(20) NOT NULL,
 [last_name] Varchar(30) NOT NULL,
 [phone] Varchar(16) NULL,
 [email] Varchar(40) NULL
)
go

-- Create indexes for table person

CREATE INDEX [IX_Relationship4] ON [person] ([ZIP])
go

-- Add keys for table person

ALTER TABLE [person] ADD CONSTRAINT [PK_person] PRIMARY KEY ([person_id])
go

-- Table insurance_comp

CREATE TABLE [insurance_comp]
(
 [comp_id] Char(3) NOT NULL,
 [comp_name] Varchar(30) NOT NULL
)
go

-- Add keys for table insurance_comp

ALTER TABLE [insurance_comp] ADD CONSTRAINT [PK_insurance_comp] PRIMARY KEY ([comp_id])
go

-- Table hospital

CREATE TABLE [hospital]
(
 [hospital_name] Varchar(20) NOT NULL,
 [ZIP] Char(5) NOT NULL,
 [capacity] Int NOT NULL
        CONSTRAINT [check_capacity] CHECK ([capacity] > 0)
)
go

-- Create indexes for table hospital

CREATE INDEX [IX_Relationship9] ON [hospital] ([ZIP])
go

-- Add keys for table hospital

ALTER TABLE [hospital] ADD CONSTRAINT [PK_hospital] PRIMARY KEY ([hospital_name])
go

-- Table city

CREATE TABLE [city]
(
 [ZIP] Char(5) NOT NULL,
 [city_name] Varchar(20) NOT NULL
)
go

-- Add keys for table city

ALTER TABLE [city] ADD CONSTRAINT [PK_city] PRIMARY KEY ([ZIP])
go

-- Table insurance

CREATE TABLE [insurance]
(
 [person_id] Char(10) NOT NULL,
 [comp_id] Char(3) NOT NULL,
 [date_start] Date NOT NULL,
 [date_end] Date NULL,
  CONSTRAINT [check_insurance_date] CHECK (date_start <= date_end)
)
go

-- Add keys for table insurance

ALTER TABLE [insurance] ADD CONSTRAINT [PK_insurance] PRIMARY KEY ([person_id],[comp_id],[date_start])
go

-- Table hospitalization

CREATE TABLE [hospitalization]
(
 [person_id] Char(10) NOT NULL,
 [hospital_name] Varchar(20) NOT NULL,
 [date_start] Date NOT NULL,
 [date_end] Date NULL,
  CONSTRAINT [check_hosp_date] CHECK (date_start <= date_end)
)
go

-- Add keys for table hospitalization

ALTER TABLE [hospitalization] ADD CONSTRAINT [PK_hospitalization] PRIMARY KEY ([date_start],[hospital_name],[person_id])
go

-- Table payment

CREATE TABLE [payment]
(
 [hospital_name] Varchar(20) NOT NULL,
 [comp_id] Char(3) NOT NULL,
 [payment_date] Date NOT NULL,
 [payment_period] Date NOT NULL,
 [details] Xml NOT NULL
)
go

-- Add keys for table payment

ALTER TABLE [payment] ADD CONSTRAINT [PK_payment] PRIMARY KEY ([hospital_name],[comp_id])
go

-- Table diagnosis_type

CREATE TABLE [diagnosis_type]
(
 [diagnosis_id] Char(5) NOT NULL,
 [description] Varchar(50) NOT NULL,
 [daily_costs] Money NOT NULL
)
go

-- Add keys for table diagnosis_type

ALTER TABLE [diagnosis_type] ADD CONSTRAINT [PK_diagnosis_type] PRIMARY KEY ([diagnosis_id])
go

-- Table diagnoses

CREATE TABLE [diagnoses]
(
 [date_start] Date NOT NULL,
 [hospital_name] Varchar(20) NOT NULL,
 [person_id] Char(10) NOT NULL,
 [diagnosis_id] Char(5) NOT NULL,
 [document] Image NULL
)
go

-- Add keys for table diagnoses

ALTER TABLE [diagnoses] ADD CONSTRAINT [PK_diagnoses] PRIMARY KEY ([date_start],[hospital_name],[person_id],[diagnosis_id])
go

-- Create foreign keys (relationships) section ------------------------------------------------- 


ALTER TABLE [person] ADD CONSTRAINT [Relationship4] FOREIGN KEY ([ZIP]) REFERENCES [city] ([ZIP]) ON UPDATE NO ACTION ON DELETE NO ACTION
go



ALTER TABLE [insurance] ADD CONSTRAINT [Relationship2] FOREIGN KEY ([person_id]) REFERENCES [person] ([person_id]) ON UPDATE NO ACTION ON DELETE NO ACTION
go



ALTER TABLE [insurance] ADD CONSTRAINT [Relationship3] FOREIGN KEY ([comp_id]) REFERENCES [insurance_comp] ([comp_id]) ON UPDATE NO ACTION ON DELETE NO ACTION
go



ALTER TABLE [hospital] ADD CONSTRAINT [Relationship9] FOREIGN KEY ([ZIP]) REFERENCES [city] ([ZIP]) ON UPDATE NO ACTION ON DELETE NO ACTION
go



ALTER TABLE [hospitalization] ADD CONSTRAINT [Relationship22] FOREIGN KEY ([hospital_name]) REFERENCES [hospital] ([hospital_name]) ON UPDATE NO ACTION ON DELETE NO ACTION
go



ALTER TABLE [hospitalization] ADD CONSTRAINT [Relationship23] FOREIGN KEY ([person_id]) REFERENCES [person] ([person_id]) ON UPDATE NO ACTION ON DELETE NO ACTION
go



ALTER TABLE [diagnoses] ADD CONSTRAINT [Relationship24] FOREIGN KEY ([date_start], [hospital_name], [person_id]) REFERENCES [hospitalization] ([date_start], [hospital_name], [person_id]) ON UPDATE NO ACTION ON DELETE NO ACTION
go



ALTER TABLE [diagnoses] ADD CONSTRAINT [Relationship25] FOREIGN KEY ([diagnosis_id]) REFERENCES [diagnosis_type] ([diagnosis_id]) ON UPDATE NO ACTION ON DELETE NO ACTION
go



ALTER TABLE [payment] ADD CONSTRAINT [Relationship26] FOREIGN KEY ([hospital_name]) REFERENCES [hospital] ([hospital_name]) ON UPDATE NO ACTION ON DELETE NO ACTION
go



ALTER TABLE [payment] ADD CONSTRAINT [Relationship28] FOREIGN KEY ([comp_id]) REFERENCES [insurance_comp] ([comp_id]) ON UPDATE NO ACTION ON DELETE NO ACTION
go




