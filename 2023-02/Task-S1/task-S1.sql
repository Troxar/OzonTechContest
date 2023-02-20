select 
  problems.id, 
  problems.contest_id, 
  problems.code
from problems
join submissions
  on submissions.problem_id = problems.id
where submissions.success
group by problems.id
having count(distinct submissions.user_id) >= 2
order by problems.id