-- Table diagnoses

CREATE TABLE [diagnoses]
(
 [date_start] Date NOT NULL,
 [hospital_name] Varchar(20) NOT NULL,
 [person_id] Char(10) NOT NULL,
 [diagnosis_id] Char(5) NOT NULL,
 [document] Image NULL
)
GO
ALTER TABLE [diagnoses] ADD CONSTRAINT [Relationship24] FOREIGN KEY ([date_start], [hospital_name], [person_id]) REFERENCES [hospitalization] ([date_start], [hospital_name], [person_id]) ON UPDATE NO ACTION ON DELETE NO ACTION
GO
ALTER TABLE [diagnoses] ADD CONSTRAINT [Relationship25] FOREIGN KEY ([diagnosis_id]) REFERENCES [diagnosis_type] ([diagnosis_id]) ON UPDATE NO ACTION ON DELETE NO ACTION
GO
-- Add keys for table diagnoses

ALTER TABLE [diagnoses] ADD CONSTRAINT [PK_diagnoses] PRIMARY KEY ([date_start],[hospital_name],[person_id],[diagnosis_id])