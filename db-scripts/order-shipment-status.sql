SELECT TOP (1000) [Id]
      ,[Name]
      ,[AvailableCount]
      ,[OnHoldCount]
      ,[SoldCount]
  FROM [Outbox.StoreFront].[dbo].[Inventory]


SELECT TOP (1000) [Id]
      ,[CustomerName]
      ,[CustomerAddress]
      ,[OrderStatus]
  FROM [Outbox.StoreFront].[dbo].[Orders]
  Order By Id Desc



SELECT TOP (1000) [Id]
    ,[OrderId]
    ,[ShipmentStatus]
    ,[CreatedAt]
    ,[UpdatedAt]
FROM [Outbox.ShipmentProcessor].[dbo].[Shipments]

