CREATE TRIGGER [hospitalizationInsertTrigger]
ON hospitalization
INSTEAD OF INSERT
AS
BEGIN	
	declare @hospital_name varchar(30);
	declare @person_id char(10);
	declare @date_start date;

	select @hospital_name=hospital_name, @person_id = person_id,
		@date_start = date_start from inserted;

	--Check if person is Insured
	declare @check_insured integer;
	select @check_insured=count(*) from insurance ins
		WHERE ins.person_id = @person_id
		AND date_end is null
		GROUP BY ins.date_end;

	if (@check_insured <= 0)
	begin
		RAISERROR('Person is not insured!', 15, 1)
		RETURN
	end

	--Check Hospital Capacity
	declare @hospital_max_cap integer;
	declare @hospital_curr_cap integer;
	SELECT @hospital_max_cap=capacity FROM hospital WHERE hospital_name = @hospital_name;
	
	SELECT @hospital_curr_cap=count(*) FROM hospitalization hosp 
		WHERE hosp.hospital_name = @hospital_name 
		AND date_end is null
		GROUP BY hosp.hospital_name;
	
	IF (@hospital_curr_cap >= @hospital_max_cap)
	BEGIN
		RAISERROR('Hospital capacity is full!', 15, 1)
		RETURN
	END

	--Check if person is not Hospitalized
	declare @check_hospitalized integer;
	SELECT @check_hospitalized=count(*) FROM hospitalization hosp 
		WHERE hosp.person_id = @person_id
		AND hosp.date_end is null
		GROUP BY hosp.person_id;

	IF (@check_hospitalized >= 1) 
	BEGIN
		RAISERROR('Person is currently hospitalized!', 15, 1)
		RETURN
	END

	--Insert is OK
	INSERT INTO hospitalization(person_id,hospital_name, date_start,date_end)
	SELECT person_id,hospital_name, date_start,date_end
	FROM INSERTED;

END