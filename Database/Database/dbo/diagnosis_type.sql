-- Table diagnosis_type

CREATE TABLE [diagnosis_type]
(
 [diagnosis_id] Char(5) NOT NULL,
 [description] Varchar(50) NOT NULL,
 [daily_costs] Money NOT NULL
)
GO
-- Add keys for table diagnosis_type

ALTER TABLE [diagnosis_type] ADD CONSTRAINT [PK_diagnosis_type] PRIMARY KEY ([diagnosis_id])