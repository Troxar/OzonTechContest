select distinct
  users.id,
  users.name
from users
join orders
  on users.id = orders.user_id
order by name, id