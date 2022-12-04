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
    --Check if datestart is earlier then birthDate
    if(MONTHS_BETWEEN(id_to_birthdate(:new.person_id),:new.date_start) > 0 ) then
        raise_application_error(-20000,'Date start is less then person birth!');
    end if;
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
    
    --Check if start date is in other hospitalization interval
        select count(*) into check_hospitalized from hospitalization
            where person_id = :new.person_id and date_start < :new.date_start and date_end > :new.date_start;
            
        if (check_hospitalized >= 1) then
            raise_application_error(-20003,'Hospitalization with same date already exists!');
        end if;
    
	--Check if person is not Hospitalized
    if(:new.date_end is null) then
        SELECT count(*) into check_hospitalized FROM hospitalization hosp 
		WHERE hosp.person_id = :new.person_id
		AND hosp.date_end is null;
        
        if (check_hospitalized >= 1) then
            raise_application_error(-20002,'Person is currently hospitalized!');
        end if;
    else 
        --Check if end date is in other hospitalization interval
        select count(*) into check_hospitalized from hospitalization
            where person_id = :new.person_id and date_start < :new.date_end and date_end > :new.date_end;
            
        if (check_hospitalized >= 1) then
            raise_application_error(-20003,'Hospitalization with same date already exists!');
        end if;
        
        --Check if date end is more then date start current hospitalization
        select count(*) into check_hospitalized from hospitalization
            where person_id = :new.person_id and date_start < :new.date_end and date_end is null;
            
        if (check_hospitalized >= 1) then
            raise_application_error(-20003,'Hospitalization with same date already exists!');
        end if;
        
        --Check if end is more then first left interval
        select min(rozdiel) into check_hospitalized from
        (
            select date_start - :new.date_start rozdiel from hospitalization
                where person_id = :new.person_id
        ) where rozdiel > 0;
        
        check_hospitalized := -1;
        if check_hospitalized >= 0 then
            if ((:new.date_end - :new.date_start) > check_hospitalized) then
                raise_application_error(-20003,'Hospitalization with same date already exists!');
            end if;
        end if;
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


