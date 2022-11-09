CREATE TRIGGER [isnuranceInsertTrigger]
ON insurance
INSTEAD OF INSERT
AS
BEGIN
	declare @person_id char(10);
	SELECT @person_id=person_id from inserted;

	declare @check_insured integer;	
	SELECT @check_insured=COUNT(*) from insurance ins
		WHERE ins.person_id = @person_id
		AND ins.date_end is null;

	IF (@check_insured >= 1) 
	BEGIN
		RAISERROR('Person is already insured',15,1);
		RETURN;
	END

	-- Insert is OK
	INSERT INTO insurance(person_id,comp_id, date_start,date_end)
	SELECT person_id,comp_id, date_start,date_end
	FROM INSERTED
END
