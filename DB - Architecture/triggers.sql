-- BEFORE INSERT ON HOSPITALIZATION
create or replace TRIGGER hospitalization_Insert_Trigger
BEFORE INSERT ON hospitalization
FOR EACH ROW
DECLARE
    check_insured integer;
    hospital_max_cap integer;
	hospital_curr_cap integer;
    check_hospitalized integer;
BEGIN	
	--Check if person is Insured	
	select count(*) into check_insured from insurance ins
		WHERE ins.person_id = :new.person_id
		AND date_end is null;

	if (check_insured <= 0) then
        raise_application_error(-20000,'Person is not insured!');
	end if;

	--Check Hospital Capacity	
	SELECT capacity into hospital_max_cap FROM hospital WHERE hospital_name = :new.hospital_name;

	SELECT count(*) into hospital_curr_cap FROM hospitalization hosp 
		WHERE hosp.hospital_name = :new.hospital_name 
		AND hosp.date_end is null;

	if (hospital_curr_cap >= hospital_max_cap) then
        raise_application_error(-20001,'Hospital capacity is full!');
	end if;

	--Check if person is not Hospitalized

	SELECT count(*) into check_hospitalized FROM hospitalization hosp 
		WHERE hosp.person_id = :new.person_id
		AND hosp.date_end is null;

	if (check_hospitalized >= 1) then
		raise_application_error(-20002,'Person is currently hospitalized!');
	end if;
END;
/

-- BEFORE INSERT ON INSURANCE
CREATE OR REPLACE TRIGGER isnurance_Insert_Trigger
BEFORE INSERT ON insurance
FOR EACH ROW
DECLARE
    check_insured integer;	
BEGIN
	SELECT COUNT(*) into check_insured from insurance ins
		WHERE ins.person_id = :new.person_id
		AND ins.date_end is null;

	if (check_insured >= 1) then
    raise_application_error(-2000,'Person is already insured');
	end if;
END;
/
 
-- BEFORE INSERT ON PAYMENT - SEQUENCE
CREATE SEQUENCE seq_payment_id 
    INCREMENT BY 1 START WITH 1;    

CREATE OR REPLACE TRIGGER payment_insert_trigger
BEFORE INSERT ON payment
FOR EACH ROW 
BEGIN
    SELECT seq_payment_id.nextval into :new.payment_id from dual;
END;
/


