ASP.NET WebForm Project using a slightly modified Northwind sample database.

The Northwind sample database used for this project is replicated from the Northwind sample database released by Microsoft, available from the following link:
https://github.com/Microsoft/sql-server-samples/tree/master/samples/databases/northwind-pubs

The Northwind used for the project is replicated from the original Northwind with the following changes:
  -  Table name for "[dbo].[Order Details]" renamed to "[dbo].[OrderDetails]", with the blankspace removed.
  -  Stored-procedure "dbo.spCustomersOrders_Get" is added to support the sample codes.
  -  Transaction dates for the orders table are updated by 22 years to reflect recent activities
     (Original data were created over 20 years ago)

SQL Script to create the Northwind sample database is provided in the \SampleDatabase folder.


