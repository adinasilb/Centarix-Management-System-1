delete from RequestNotifications 
go
delete from Comments
go
delete from RequestLocationInstances
go
update LocationInstances set IsFull = 0
go
delete from Payments
go
delete from Materials
go
delete from FunctionLines
go
delete from FavoriteRequests
go
delete from ShareRequests
go
delete from Requests
go 
delete from ParentRequests
go
delete from ParentQuotes
go

delete  from products
go
delete from Invoices
go

