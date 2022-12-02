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