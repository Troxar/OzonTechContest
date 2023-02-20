with problem_ids as
  (select
    id
  from problems
  where contest_id = (
    select
	  max(id) as id
    from contests)),

min_successful_times as
  (select
    user_id,
    problem_id,
    min(submitted_at) as submitted_at
  from submissions
  join problem_ids
    on submissions.problem_id = problem_ids.id
  where success
  group by user_id, problem_id),

successful_times as
  (select
    user_id,
    max(submitted_at) as submitted_at
  from min_successful_times
  group by user_id),

solved_count as
  (select
    user_id, 
    count(distinct case when success then problem_id end) as problem_count
  from submissions
  join problem_ids
    on submissions.problem_id = problem_ids.id
  group by user_id)

select
  rank () over (order by solved_count.problem_count desc, successful_times.submitted_at),
  solved_count.user_id,
  users.name as user_name,
  solved_count.problem_count,
  successful_times.submitted_at as latest_successful_submitted_at
from solved_count
join users
  on solved_count.user_id = users.id
left join successful_times
  on solved_count.user_id = successful_times.user_id
order by 
  rank, 
  latest_successful_submitted_at, 
  user_id