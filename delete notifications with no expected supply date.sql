delete from RequestNotifications
where RequestID in 
(select Requests.RequestID from RequestNotifications inner join Requests
on RequestNotifications.RequestID = Requests.RequestID
where ExpectedSupplyDays is null and NotificationStatusID = 1)