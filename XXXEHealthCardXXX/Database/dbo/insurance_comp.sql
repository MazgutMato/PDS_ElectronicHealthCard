-- Table insurance_comp

CREATE TABLE [insurance_comp]
(
 [comp_id] Char(3) NOT NULL,
 [comp_name] Varchar(30) NOT NULL
)
GO
-- Add keys for table insurance_comp

ALTER TABLE [insurance_comp] ADD CONSTRAINT [PK_insurance_comp] PRIMARY KEY ([comp_id])