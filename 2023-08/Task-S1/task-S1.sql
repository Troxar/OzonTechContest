select
	tr.destination_id as "fulfilment_id",
	f.name as "fulfilment_name",
	tr.package_id  as "package_id",
	pack.name  as "package_name",
	tr.arrival_time as "time_in",
	tr2.departure_time as "time_out",
	tr2.departure_time - tr.arrival_time as "storage_time"
from transportations tr
inner join transportations tr2 
	on tr.destination_id = tr2.source_id and tr.package_id = tr2.package_id
inner join fulfilments f
	on tr.destination_id = f.id
inner join packages pack
	on tr.package_id = pack.id
order by
	storage_time desc,
	time_in,
	fulfilment_id,
	package_id