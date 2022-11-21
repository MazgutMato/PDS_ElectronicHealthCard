-- Table hospitalization

CREATE TABLE [hospitalization]
(
 [person_id] Char(10) NOT NULL,
 [hospital_name] Varchar(20) NOT NULL,
 [date_start] Date NOT NULL,
 [date_end] Date NULL,
  CONSTRAINT [check_hosp_date] CHECK (date_start <= date_end)
)
GO
ALTER TABLE [hospitalization] ADD CONSTRAINT [Relationship22] FOREIGN KEY ([hospital_name]) REFERENCES [hospital] ([hospital_name]) ON UPDATE NO ACTION ON DELETE NO ACTION
GO
ALTER TABLE [hospitalization] ADD CONSTRAINT [Relationship23] FOREIGN KEY ([person_id]) REFERENCES [person] ([person_id]) ON UPDATE NO ACTION ON DELETE NO ACTION
GO
-- Add keys for table hospitalization

ALTER TABLE [hospitalization] ADD CONSTRAINT [PK_hospitalization] PRIMARY KEY ([date_start],[hospital_name],[person_id])