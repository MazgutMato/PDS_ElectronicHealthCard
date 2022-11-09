-- Table hospital

CREATE TABLE [hospital]
(
 [hospital_name] Varchar(20) NOT NULL,
 [ZIP] Char(5) NOT NULL,
 [capacity] Int NOT NULL
        CHECK ([capacity] > 0)
)
GO
ALTER TABLE [hospital] ADD CONSTRAINT [Relationship9] FOREIGN KEY ([ZIP]) REFERENCES [city] ([ZIP]) ON UPDATE NO ACTION ON DELETE NO ACTION
GO
-- Add keys for table hospital

ALTER TABLE [hospital] ADD CONSTRAINT [PK_hospital] PRIMARY KEY ([hospital_name])
GO
-- Create indexes for table hospital

CREATE INDEX [IX_Relationship9] ON [hospital] ([ZIP])