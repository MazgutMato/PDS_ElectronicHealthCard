create or replace procedure set_person_inf
    (
    p_id in char,
    p_first_name in varchar2,
    p_last_name in varchar2,
    p_phone in varchar2,
    p_email in varchar2
    )
    IS
    BEGIN
        update person
            set person_inf = person_inf(p_first_name, p_last_name,p_phone,p_email)
        where person_id = p_id;   
    END;
/

create or replace function get_person_inf
    (
    p_id in char
    )
    return varchar2
    IS
    info person.person_inf%TYPE;
    BEGIN
        select person_inf into info from person
            where person_id = p_id;
        return info.first_name || ';' || info.last_name || ';' 
            || info.phone || ';' || info.email || ';';
    END;
/

create or replace function id_to_birthdate
(
    p_person_id char
)
return date
is
    str_date varchar(10);
begin  
    --Convert id to date
    if(substr(p_person_id, 1,2) > to_char(sysdate, 'YY')) then
        str_date := '19' || substr(p_person_id, 1,2);
    else
        str_date := '20' || substr(p_person_id, 1,2);
    end if;

    str_date := str_date || '.' || mod(substr(p_person_id,3,2),50);
    str_date := str_date || '.'|| substr(p_person_id,5,2);
    return to_date( str_date, 'YYYY.MM.DD' );        
end;
/