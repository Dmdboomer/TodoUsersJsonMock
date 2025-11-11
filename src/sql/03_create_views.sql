-- View for Task 6: User Todo details view
CREATE OR ALTER VIEW dbo.vw_UserTodo
AS
SELECT 
    u.Id AS UserID,
    u.Name,
    u.City,
    u.CompanyName,
    t.Id AS TodoID,
    t.Title,
    t.Completed
FROM Users u
INNER JOIN Todos t ON u.Id = t.UserId;