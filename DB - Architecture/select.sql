-- Payments sum in hospital over all months in 1 year
select hospital_name, comp_id, 
    sum(case when extract(month from payment_period) = 1 then to_number(extractValue(payment.details, '/Payment/Amount')) else 0 end) first,
    sum(case when extract(month from payment_period) = 2 then to_number(extractValue(payment.details, '/Payment/Amount')) else 0 end) second,
    sum(case when extract(month from payment_period) = 3 then to_number(extractValue(payment.details, '/Payment/Amount')) else 0 end) third,
    sum(case when extract(month from payment_period) = 4 then to_number(extractValue(payment.details, '/Payment/Amount')) else 0 end) fourth,
    sum(case when extract(month from payment_period) = 5 then to_number(extractValue(payment.details, '/Payment/Amount')) else 0 end) fifth,
    sum(case when extract(month from payment_period) = 6 then to_number(extractValue(payment.details, '/Payment/Amount')) else 0 end) sixth,
    sum(case when extract(month from payment_period) = 7 then to_number(extractValue(payment.details, '/Payment/Amount')) else 0 end) seventh,
    sum(case when extract(month from payment_period) = 8 then to_number(extractValue(payment.details, '/Payment/Amount')) else 0 end) eighth,
    sum(case when extract(month from payment_period) = 9 then to_number(extractValue(payment.details, '/Payment/Amount')) else 0 end) ninth,
    sum(case when extract(month from payment_period) = 10 then to_number(extractValue(payment.details, '/Payment/Amount')) else 0 end) tenth,
    sum(case when extract(month from payment_period) = 11 then to_number(extractValue(payment.details, '/Payment/Amount')) else 0 end) eleventh,
    sum(case when extract(month from payment_period) = 12 then to_number(extractValue(payment.details, '/Payment/Amount')) else 0 end) twelfth
        from payment
        where hospital_name = 'Bwffrycaoehzfigf' and extract(year from payment_period) = 2011
        group by hospital_name, comp_id;
        
-- Daily Costs of Hospitalizations(Diagnoses based) in Hospital Over a Month
select diagnosis_id, days, daily_costs, daily_costs*days from        
    (select diagnosis_id, sum(dayz) days from
        (select diagnosis_id,(case when (date_start <= TRUNC(to_date('10.11.2022','DD.MM.YYYY'), 'MM')) 
                and ( NVL(date_end,sysdate) <= last_day(to_date('10.11.2022','DD.MM.YYYY'))) then  NVL(date_end,sysdate) - TRUNC(to_date('10.11.2022','DD.MM.YYYY'), 'MM') + 1                 
                   when (date_start <= TRUNC(to_date('10.11.2022','DD.MM.YYYY'), 'MM'))
                and ( NVL(date_end,sysdate) >= last_day(to_date('10.11.2022','DD.MM.YYYY'))) then last_day(to_date('10.11.2022','DD.MM.YYYY')) - TRUNC(to_date('10.11.2022','DD.MM.YYYY'), 'MM') + 1                
                   when (date_start >= TRUNC(to_date('10.11.2022','DD.MM.YYYY'), 'MM'))
                and ( NVL(date_end,sysdate) <= last_day(to_date('10.11.2022','DD.MM.YYYY'))) then NVL(date_end,sysdate) - date_start + 1                
                   when (date_start >= TRUNC(to_date('10.11.2022','DD.MM.YYYY'), 'MM'))
                and ( NVL(date_end,sysdate) >= last_day(to_date('10.11.2022','DD.MM.YYYY'))) then last_day(to_date('10.11.2022','DD.MM.YYYY')) - date_start + 1
              end) dayz    
            from hospitalization
            join diagnoses using(person_id, hospital_name, date_start)
            where hospital_name = 'Igykxtskl'
            and extract(year from date_start) <= 2022 and extract(month from date_start) <= 11
            and extract(year from NVL(date_end,sysdate)) >= 2022 and extract(month from NVL(date_end,sysdate)) >= 11)
        group by diagnosis_id)
    join diagnoses_type using(diagnosis_id);
    
-- Number of diagnoses cases in Hospitals, in Months Over a Year
select diagnosis_id,   
                    sum(case when  
                    (  extract(year from date_start) < :P_YEAR and (extract(year from NVL(date_end,sysdate)) > :P_YEAR) ) or  
                    (  extract(year from date_start) = :P_YEAR and (extract(year from NVL(date_end,sysdate)) > :P_YEAR) and extract(month from date_start) <= 1 ) or  
                    (  extract(year from date_start) = :P_YEAR and extract(year from NVL(date_end,sysdate)) = :P_YEAR and extract(month from date_start) <= 1 and extract(month from NVL(date_end,sysdate)) >= 1 ) or  
                    (  extract(year from date_start) < :P_YEAR and extract(year from NVL(date_end,sysdate)) = :P_YEAR and extract(month from NVL(date_end,sysdate)) >= 1 )  
                    then 1 else 0 end) first,  
                    sum(case when  
                    (  extract(year from date_start) < :P_YEAR and (extract(year from NVL(date_end,sysdate)) > :P_YEAR) ) or  
                    (  extract(year from date_start) = :P_YEAR and (extract(year from NVL(date_end,sysdate)) > :P_YEAR) and extract(month from date_start) <= 2 ) or  
                    (  extract(year from date_start) = :P_YEAR and extract(year from NVL(date_end,sysdate)) = :P_YEAR and extract(month from date_start) <= 2 and extract(month from NVL(date_end,sysdate)) >= 2 ) or  
                    (  extract(year from date_start) < :P_YEAR and extract(year from NVL(date_end,sysdate)) = :P_YEAR and extract(month from NVL(date_end,sysdate)) >= 2 )  
                    then 1 else 0 end) second,  
                    sum(case when  
                    (  extract(year from date_start) < :P_YEAR and (extract(year from NVL(date_end,sysdate)) > :P_YEAR) ) or  
                    (  extract(year from date_start) = :P_YEAR and (extract(year from NVL(date_end,sysdate)) > :P_YEAR) and extract(month from date_start) <= 3 ) or  
                    (  extract(year from date_start) = :P_YEAR and extract(year from NVL(date_end,sysdate)) = :P_YEAR and extract(month from date_start) <= 3 and extract(month from NVL(date_end,sysdate)) >= 3 ) or  
                    (  extract(year from date_start) < :P_YEAR and extract(year from NVL(date_end,sysdate)) = :P_YEAR and extract(month from NVL(date_end,sysdate)) >= 3 )  
                    then 1 else 0 end) third,  
                    sum(case when  
                    (  extract(year from date_start) < :P_YEAR and (extract(year from NVL(date_end,sysdate)) > :P_YEAR) ) or  
                    (  extract(year from date_start) = :P_YEAR and (extract(year from NVL(date_end,sysdate)) > :P_YEAR) and extract(month from date_start) <= 4 ) or  
                    (  extract(year from date_start) = :P_YEAR and extract(year from NVL(date_end,sysdate)) = :P_YEAR and extract(month from date_start) <= 4 and extract(month from NVL(date_end,sysdate)) >= 4 ) or  
                    (  extract(year from date_start) < :P_YEAR and extract(year from NVL(date_end,sysdate)) = :P_YEAR and extract(month from NVL(date_end,sysdate)) >= 4 ) 
                    then 1 else 0 end) fourth,  
                    sum(case when  
                    (  extract(year from date_start) < :P_YEAR and (extract(year from NVL(date_end,sysdate)) > :P_YEAR) ) or  
                    (  extract(year from date_start) = :P_YEAR and (extract(year from NVL(date_end,sysdate)) > :P_YEAR) and extract(month from date_start) <= 5 ) or  
                    (  extract(year from date_start) = :P_YEAR and extract(year from NVL(date_end,sysdate)) = :P_YEAR and extract(month from date_start) <= 5 and extract(month from NVL(date_end,sysdate)) >= 5 ) or  
                    (  extract(year from date_start) < :P_YEAR and extract(year from NVL(date_end,sysdate)) = :P_YEAR and extract(month from NVL(date_end,sysdate)) >= 5 )  
                    then 1 else 0 end) fifth,  
                    sum(case when  
                    (  extract(year from date_start) < :P_YEAR and (extract(year from NVL(date_end,sysdate)) > :P_YEAR) ) or  
                    (  extract(year from date_start) = :P_YEAR and (extract(year from NVL(date_end,sysdate)) > :P_YEAR) and extract(month from date_start) <= 6 ) or  
                    (  extract(year from date_start) = :P_YEAR and extract(year from NVL(date_end,sysdate)) = :P_YEAR and extract(month from date_start) <= 6 and extract(month from NVL(date_end,sysdate)) >= 6 ) or  
                    (  extract(year from date_start) < :P_YEAR and extract(year from NVL(date_end,sysdate)) = :P_YEAR and extract(month from NVL(date_end,sysdate)) >= 6 )  
                    then 1 else 0 end) sixth,  
                    sum(case when  
                    (  extract(year from date_start) < :P_YEAR and (extract(year from NVL(date_end,sysdate)) > :P_YEAR) ) or  
                    (  extract(year from date_start) = :P_YEAR and (extract(year from NVL(date_end,sysdate)) > :P_YEAR) and extract(month from date_start) <= 7 ) or  
                    (  extract(year from date_start) = :P_YEAR and extract(year from NVL(date_end,sysdate)) = :P_YEAR and extract(month from date_start) <= 7 and extract(month from NVL(date_end,sysdate)) >= 7 ) or  
                    (  extract(year from date_start) < :P_YEAR and extract(year from NVL(date_end,sysdate)) = :P_YEAR and extract(month from NVL(date_end,sysdate)) >= 7 )  
                    then 1 else 0 end) seventh,  
                    sum(case when  
                    (  extract(year from date_start) < :P_YEAR and (extract(year from NVL(date_end,sysdate)) > :P_YEAR) ) or  
                    (  extract(year from date_start) = :P_YEAR and (extract(year from NVL(date_end,sysdate)) > :P_YEAR) and extract(month from date_start) <= 8 ) or  
                    (  extract(year from date_start) = :P_YEAR and extract(year from NVL(date_end,sysdate)) = :P_YEAR and extract(month from date_start) <= 8 and extract(month from NVL(date_end,sysdate)) >= 8 ) or  
                    (  extract(year from date_start) < :P_YEAR and extract(year from NVL(date_end,sysdate)) = :P_YEAR and extract(month from NVL(date_end,sysdate)) >= 8 )  
                    then 1 else 0 end) eighth,  
                    sum(case when  
                    (  extract(year from date_start) < :P_YEAR and (extract(year from NVL(date_end,sysdate)) > :P_YEAR) ) or  
                    (  extract(year from date_start) = :P_YEAR and (extract(year from NVL(date_end,sysdate)) > :P_YEAR) and extract(month from date_start) <= 9 ) or  
                    (  extract(year from date_start) = :P_YEAR and extract(year from NVL(date_end,sysdate)) = :P_YEAR and extract(month from date_start) <= 9 and extract(month from NVL(date_end,sysdate)) >= 9 ) or  
                    (  extract(year from date_start) < :P_YEAR and extract(year from NVL(date_end,sysdate)) = :P_YEAR and extract(month from NVL(date_end,sysdate)) >= 9 )  
                    then 1 else 0 end) ninth,  
                    sum(case when  
                    (  extract(year from date_start) < :P_YEAR and (extract(year from NVL(date_end,sysdate)) > :P_YEAR) ) or  
                    (  extract(year from date_start) = :P_YEAR and (extract(year from NVL(date_end,sysdate)) > :P_YEAR) and extract(month from date_start) <= 10 ) or  
                    (  extract(year from date_start) = :P_YEAR and extract(year from NVL(date_end,sysdate)) = :P_YEAR and extract(month from date_start) <= 10 and extract(month from NVL(date_end,sysdate)) >= 10 ) or  
                    (  extract(year from date_start) < :P_YEAR and extract(year from NVL(date_end,sysdate)) = :P_YEAR and extract(month from NVL(date_end,sysdate)) >= 10 )  
                    then 1 else 0 end) tenth,  
                    sum(case when  
                    (  extract(year from date_start) < :P_YEAR and (extract(year from NVL(date_end,sysdate)) > :P_YEAR) ) or  
                    (  extract(year from date_start) = :P_YEAR and (extract(year from NVL(date_end,sysdate)) > :P_YEAR) and extract(month from date_start) <= 11 ) or  
                    (  extract(year from date_start) = :P_YEAR and extract(year from NVL(date_end,sysdate)) = :P_YEAR and extract(month from date_start) <= 11 and extract(month from NVL(date_end,sysdate)) >= 11 ) or  
                    (  extract(year from date_start) < :P_YEAR and extract(year from NVL(date_end,sysdate)) = :P_YEAR and extract(month from NVL(date_end,sysdate)) >= 11 )  
                    then 1 else 0 end) eleventh,  
                    sum(case when  
                    (  extract(year from date_start) < :P_YEAR and (extract(year from NVL(date_end,sysdate)) > :P_YEAR) ) or  
                    (  extract(year from date_start) = :P_YEAR and (extract(year from NVL(date_end,sysdate)) > :P_YEAR) and extract(month from date_start) <= 12 ) or  
                    (  extract(year from date_start) = :P_YEAR and extract(year from NVL(date_end,sysdate)) = :P_YEAR and extract(month from date_start) <= 12 and extract(month from NVL(date_end,sysdate)) >= 12 ) or  
                    (  extract(year from date_start) < :P_YEAR and extract(year from NVL(date_end,sysdate)) = :P_YEAR and extract(month from NVL(date_end,sysdate)) >= 12 )  
                    then 1 else 0 end) twelfth  
        from hospital  
        join hospitalization using(hospital_name)  
        join diagnoses using(person_id,hospital_name,date_start)  
        where extract(year from date_start) <= :P_YEAR and (extract(year from NVL(date_end,sysdate)) >= :P_YEAR) and hospital_name = :HOSPITAL_NAME  
        group by diagnosis_id;
        
-- Top 10 Most Times insured People
select rn, person_id, get_person_inf(person_id), ct  
    from (select row_number() over(order by count(person_id) desc) rn,  
            person_id, count(person_id) ct  
            from insurance   
            group by person_id) 
where rn <= 10