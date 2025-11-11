-- Query for Task 5: User completion statistics
SELECT 
    u.Id AS UserID,
    u.Name,
    COUNT(CASE WHEN t.Completed = 1 THEN 1 END) AS CompletedCount,
    COUNT(t.Id) AS TotalCount,
    CASE 
        WHEN COUNT(t.Id) = 0 THEN 0 
        ELSE CAST(COUNT(CASE WHEN t.Completed = 1 THEN 1 END) AS FLOAT) / COUNT(t.Id) 
    END AS CompletionPct
FROM Users u
LEFT JOIN Todos t ON u.Id = t.UserId
GROUP BY u.Id, u.Name
ORDER BY CompletionPct DESC, u.Name;