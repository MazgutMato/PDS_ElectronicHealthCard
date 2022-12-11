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