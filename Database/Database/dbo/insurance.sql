-- Table insurance

CREATE TABLE [insurance]
(
 [person_id] Char(10) NOT NULL,
 [comp_id] Char(3) NOT NULL,
 [date_start] Date NOT NULL,
 [date_end] Date NULL,
  CONSTRAINT [check_insurance_date] CHECK (date_start <= date_end)
)
GO
ALTER TABLE [insurance] ADD CONSTRAINT [Relationship2] FOREIGN KEY ([person_id]) REFERENCES [person] ([person_id]) ON UPDATE NO ACTION ON DELETE NO ACTION
GO
ALTER TABLE [insurance] ADD CONSTRAINT [Relationship3] FOREIGN KEY ([comp_id]) REFERENCES [insurance_comp] ([comp_id]) ON UPDATE NO ACTION ON DELETE NO ACTION
GO
-- Add keys for table insurance

ALTER TABLE [insurance] ADD CONSTRAINT [PK_insurance] PRIMARY KEY ([person_id],[comp_id],[date_start])