-- Table city

CREATE TABLE [city]
(
 [ZIP] Char(5) NOT NULL,
 [city_name] Varchar(20) NOT NULL
)
GO
-- Add keys for table city

ALTER TABLE [city] ADD CONSTRAINT [PK_city] PRIMARY KEY ([ZIP])