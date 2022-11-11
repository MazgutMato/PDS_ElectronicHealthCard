-- Table diagnoses_type

CREATE TABLE [diagnoses_type]
(
 [diagnosis_id] Char(5) NOT NULL,
 [description] Varchar(50) NOT NULL,
 [daily_costs] Money NOT NULL
)
GO
-- Add keys for table diagnoses_type

ALTER TABLE [diagnoses_type] ADD CONSTRAINT [PK_diagnoses_type] PRIMARY KEY ([diagnosis_id])