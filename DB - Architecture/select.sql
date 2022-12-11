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