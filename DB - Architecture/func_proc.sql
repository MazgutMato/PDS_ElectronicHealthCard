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

create or replace function get_hosp_count
(
    p_year integer,
    p_month integer,
    p_hosp char
)
return integer
is
    p_count integer;
begin
    select
    sum(case when  
             (  extract(year from date_start) < p_year and (date_end is null or extract(year from date_end) > p_year) ) or
             (  extract(year from date_start) = p_year and (date_end is null or extract(year from date_end) > p_year) and extract(month from date_start) <= p_month ) or
             (  extract(year from date_start) = p_year and extract(year from date_end) = p_year and extract(month from date_start) <= p_month and extract(month from date_end) >= p_month ) or  
             (  extract(year from date_start) < p_year and extract(year from date_end) = p_year and extract(month from date_end) >= p_month )           
             then 1 else 0 end) into p_count
    from hospital
    join hospitalization using(hospital_name)
        where extract(year from date_start) <= p_year and (date_end is null or extract(year from date_end) >= p_year) and hospital_name = p_hosp
        group by hospital_name;
    return p_count;     
end;
/