/*
Created: 20. 11. 2022
Modified: 20. 11. 2022
Model: SP - database
Database: Oracle 19c
*/


-- Create tables section -------------------------------------------------

-- Table person

CREATE TYPE person_inf as OBJECT(
  first_name Varchar2(20 ),
  last_name Varchar2(30 ),
  phone Varchar2(16 ),
  email Varchar2(40 )
)
/

CREATE TABLE person(
  person_id Char(10 ) NOT NULL,
  ZIP Char(5 ) NOT NULL,
  person_inf person_inf NOT NULL
)


-- Create indexes for table person

CREATE INDEX "IX_Relationship2" ON "person" ("ZIP")
/

-- Add keys for table person

ALTER TABLE "person" ADD CONSTRAINT "PK_person" PRIMARY KEY ("person_id")
/

-- Table insurance_comp

CREATE TABLE "insurance_comp"(
  "comp_id" Char(3 ) NOT NULL,
  "comp_name" Varchar2(30 ) NOT NULL
)
/

-- Add keys for table insurance_comp

ALTER TABLE "insurance_comp" ADD CONSTRAINT "PK_insurance_comp" PRIMARY KEY ("comp_id")
/

-- Table hospital

CREATE TABLE "hospital"(
  "hospital_name" Varchar2(20 ) NOT NULL,
  "ZIP" Char(5 ) NOT NULL,
  "capacity" Integer NOT NULL
        CONSTRAINT "check_capacity" CHECK ("capacity" > 0)
)
/

-- Create indexes for table hospital

CREATE INDEX "IX_Relationship1" ON "hospital" ("ZIP")
/

-- Add keys for table hospital

ALTER TABLE "hospital" ADD CONSTRAINT "PK_hospital" PRIMARY KEY ("hospital_name")
/

-- Table city

CREATE TABLE "city"(
  "ZIP" Char(5 ) NOT NULL,
  "city_name" Varchar2(20 ) NOT NULL
)
/

-- Add keys for table city

ALTER TABLE "city" ADD CONSTRAINT "PK_city" PRIMARY KEY ("ZIP")
/

-- Table insurance

CREATE TABLE "insurance"(
  "person_id" Char(10 ) NOT NULL,
  "comp_id" Char(3 ) NOT NULL,
  "date_start" Date NOT NULL,
  "date_end" Date,
  CONSTRAINT "check_insurance_date" CHECK (date_start <= date_end)
)
/

-- Add keys for table insurance

ALTER TABLE "insurance" ADD CONSTRAINT "PK_insurance" PRIMARY KEY ("person_id","comp_id","date_start")
/

-- Table hospitalization

CREATE TABLE "hospitalization"(
  "person_id" Char(10 ) NOT NULL,
  "hospital_name" Varchar2(20 ) NOT NULL,
  "date_start" Date NOT NULL,
  "date_end" Date,
  CONSTRAINT "check_hosp_date" CHECK (date_start <= date_end)
)
/

-- Add keys for table hospitalization

ALTER TABLE "hospitalization" ADD CONSTRAINT "PK_hospitalization" PRIMARY KEY ("date_start","hospital_name","person_id")
/

-- Table payment

CREATE TABLE "payment"(
  "payment_id" Integer NOT NULL,
  "hospital_name" Varchar2(20 ) NOT NULL,
  "comp_id" Char(3 ) NOT NULL,
  "payment_date" Date NOT NULL,
  "payment_period" Date NOT NULL,
  "details" XMLType NOT NULL
)
/

-- Add keys for table payment

ALTER TABLE "payment" ADD CONSTRAINT "PK_payment" PRIMARY KEY ("hospital_name","comp_id","payment_id")
/

-- Table diagnoses_type

CREATE TABLE "diagnoses_type"(
  "diagnosis_id" Char(5 ) NOT NULL,
  "description" Varchar2(50 ) NOT NULL,
  "daily_costs" Number(15,2) NOT NULL
)
/

-- Add keys for table diagnoses_type

ALTER TABLE "diagnoses_type" ADD CONSTRAINT "PK_diagnoses_type" PRIMARY KEY ("diagnosis_id")
/

-- Table diagnoses

CREATE TABLE "diagnoses"(
  "date_start" Date NOT NULL,
  "hospital_name" Varchar2(20 ) NOT NULL,
  "person_id" Char(10 ) NOT NULL,
  "diagnosis_id" Char(5 ) NOT NULL,
  "document" Long raw
)
/

-- Add keys for table diagnoses

ALTER TABLE "diagnoses" ADD CONSTRAINT "PK_diagnoses" PRIMARY KEY ("date_start","hospital_name","person_id","diagnosis_id")
/


-- Create foreign keys (relationships) section ------------------------------------------------- 

ALTER TABLE "payment" ADD CONSTRAINT "RelationshipPaymentInsurance" FOREIGN KEY ("comp_id") REFERENCES "insurance_comp" ("comp_id")
/



ALTER TABLE "payment" ADD CONSTRAINT "RelationshipPaymentHospital" FOREIGN KEY ("hospital_name") REFERENCES "hospital" ("hospital_name")
/



ALTER TABLE "diagnoses" ADD CONSTRAINT "RelationshipDIagnosesType" FOREIGN KEY ("diagnosis_id") REFERENCES "diagnoses_type" ("diagnosis_id")
/



ALTER TABLE "diagnoses" ADD CONSTRAINT "RelationshipDIagnosesHospitalization" FOREIGN KEY ("date_start", "hospital_name", "person_id") REFERENCES "hospitalization" ("date_start", "hospital_name", "person_id")
/



ALTER TABLE "hospitalization" ADD CONSTRAINT "RelationshipPersonHospitalization" FOREIGN KEY ("person_id") REFERENCES "person" ("person_id")
/



ALTER TABLE "hospitalization" ADD CONSTRAINT "RelationshipHospitalHopsitalization" FOREIGN KEY ("hospital_name") REFERENCES "hospital" ("hospital_name")
/



ALTER TABLE "hospital" ADD CONSTRAINT "RelationshipCityHospital" FOREIGN KEY ("ZIP") REFERENCES "city" ("ZIP")
/



ALTER TABLE "insurance" ADD CONSTRAINT "RelationshipInsuranceComp" FOREIGN KEY ("comp_id") REFERENCES "insurance_comp" ("comp_id")
/



ALTER TABLE "insurance" ADD CONSTRAINT "RelationshipPersonInsurance" FOREIGN KEY ("person_id") REFERENCES "person" ("person_id")
/



ALTER TABLE "person" ADD CONSTRAINT "RelationshipCityPerson" FOREIGN KEY ("ZIP") REFERENCES "city" ("ZIP")
/





