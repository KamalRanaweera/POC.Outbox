SELECT TOP (5) [Id]
      ,[MessageType]
      ,[EventName]
      ,[Payload]
      ,[CreatedAt]
      ,[Processed]
      ,[ProcessedAt]
  FROM [Outbox.StoreFront].[dbo].[EventMessages]
  Order By CreatedAt Desc


  SELECT TOP (5) [Id]
      ,[MessageType]
      ,[EventName]
      ,[Payload]
      ,[CreatedAt]
      ,[Processed]
      ,[ProcessedAt]
  FROM [Outbox.SimpleMessageBroker].[dbo].[EventMessages]
  Order By CreatedAt Desc


  SELECT TOP (5) [Id]
      ,[MessageType]
      ,[EventName]
      ,[Payload]
      ,[CreatedAt]
      ,[Processed]
      ,[ProcessedAt]
  FROM [Outbox.ShipmentProcessor].[dbo].[EventMessages]
  Order By CreatedAt Desc
