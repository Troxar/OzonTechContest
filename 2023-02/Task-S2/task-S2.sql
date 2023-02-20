with t1 as
  (select 
    submissions.user_id, 
    count(distinct case when success then problems.contest_id end) as solved_at_least_one_contest_count,
    count(distinct problems.contest_id) as take_part_contest_count
  from submissions
  join problems
    on submissions.problem_id = problems.id
  group by submissions.user_id)

select 
  users.id, 
  users.name, 
  coalesce(t1.solved_at_least_one_contest_count, 0) as solved_at_least_one_contest_count,
  coalesce(t1.take_part_contest_count, 0) as take_part_contest_count
from users
left join t1
  on users.id = t1.user_id
order by 
  solved_at_least_one_contest_count desc, 
  take_part_contest_count desc,
  id