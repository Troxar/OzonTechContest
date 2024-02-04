with t1 as 
  (select
      tr.destination_id as "fulfilment_id",
      f.name as "fulfilment_name",
      tr.package_id as "package_id",
      pack.name as "package_name",
      tr.arrival_time as "time_in",
      lead(tr.departure_time) over (partition by package_id order by arrival_time) as "time_out"
  from transportations tr
  inner join fulfilments f
      on tr.destination_id = f.id
  inner join packages pack
      on tr.package_id = pack.id)
      
select
	*,
    time_out - time_in as "storage_time"
from t1
where time_out is not null
order by
	storage_time desc,
    time_in,
    fulfilment_id,
    package_id
