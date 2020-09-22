USE SPPlog
declare @size INTEGER = 50;
declare @countno INTEGER;
SELECT @countno = count([ErrorId])  FROM [dbo].[ELMAH_Error]
/*
SELECT TOP (@countno-@size)
       [ErrorId]
      ,[TimeUtc]
      ,[Sequence]
  FROM [dbo].[ELMAH_Error]
  order by [Sequence] asc
GO
*/
DELETE FROM [dbo].[ELMAH_Error]
WHERE [ErrorId] IN (
    SELECT t.[ErrorId] FROM (
       SELECT TOP (@countno-@size)
               [ErrorId]
              ,[TimeUtc]
              ,[Sequence]
          FROM [dbo].[ELMAH_Error]
          order by [Sequence] asc
      ) t
    )
GO


/*
-- Print remaining rows
SELECT * FROM [dbo].[ELMAH_Error]
order by [Sequence] desc
*/