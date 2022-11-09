-- Table payment

CREATE TABLE [payment]
(
 [hospital_name] Varchar(20) NOT NULL,
 [comp_id] Char(3) NOT NULL,
 [payment_date] Date NOT NULL,
 [payment_period] Date NOT NULL,
 [details] Xml NOT NULL
)
GO
ALTER TABLE [payment] ADD CONSTRAINT [Relationship26] FOREIGN KEY ([hospital_name]) REFERENCES [hospital] ([hospital_name]) ON UPDATE NO ACTION ON DELETE NO ACTION
GO
ALTER TABLE [payment] ADD CONSTRAINT [Relationship28] FOREIGN KEY ([comp_id]) REFERENCES [insurance_comp] ([comp_id]) ON UPDATE NO ACTION ON DELETE NO ACTION
GO
-- Add keys for table payment

ALTER TABLE [payment] ADD CONSTRAINT [PK_payment] PRIMARY KEY ([hospital_name],[comp_id])