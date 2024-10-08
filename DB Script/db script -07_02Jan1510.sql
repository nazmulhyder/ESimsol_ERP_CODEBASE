GO
/****** Object:  Table [dbo].[COSImage]    Script Date: 1/2/2017 3:07:42 PM ******/
DROP TABLE [dbo].[COSImage]
GO
/****** Object:  Table [dbo].[ClientOperationSetting]    Script Date: 1/2/2017 3:07:42 PM ******/
DROP TABLE [dbo].[ClientOperationSetting]
GO
/****** Object:  StoredProcedure [dbo].[SP_IUD_ClientOperationSetting]    Script Date: 1/2/2017 3:07:42 PM ******/
DROP PROCEDURE [dbo].[SP_IUD_ClientOperationSetting]
GO
/****** Object:  StoredProcedure [dbo].[SP_IUD_ClientOperationSetting]    Script Date: 1/2/2017 3:07:42 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SP_IUD_ClientOperationSetting]
(
	@ClientOperationSettingID	as int,
	@OperationType as smallint,
	@DataType as smallint,
	@Value as varchar(1000),
	@DBUserID	as int,
	@DBOperation as smallint
	--%n, %n, %n, %s, %n, %n
)		
AS
BEGIN TRAN
DECLARE 
@DBServerDateTime as datetime
SET @DBServerDateTime=Getdate()
IF(@DBOperation=1)
BEGIN			
	
	If EXISTS(SELECT * FROM ClientOperationSetting WHERE OperationType = @OperationType)
	BEGIN
		ROLLBACK
			RAISERROR (N'Already Exists this Operation Type, Please Find this Item. !!',16,1);	
		RETURN
	END
	SET @ClientOperationSettingID=(SELECT ISNULL(MAX(ClientOperationSettingID),0)+1 FROM ClientOperationSetting)
	INSERT INTO ClientOperationSetting	(ClientOperationSettingID,			OperationType,		DataType,		Value,			DBUserID,	DBServerDateTime)
    							VALUES	(@ClientOperationSettingID,			@OperationType,		@DataType,		@Value,	@DBUserID,	@DBServerDateTime)
    SELECT * FROM ClientOperationSetting  WHERE ClientOperationSettingID= @ClientOperationSettingID
END

IF(@DBOperation=2)
BEGIN
	IF(@ClientOperationSettingID<=0)
	BEGIN
		ROLLBACK
			RAISERROR (N'Your Selected Client Operation Setting Is Invalid Please Refresh and try again!!',16,1);	
		RETURN
	END	
	
	If EXISTS(SELECT * FROM ClientOperationSetting WHERE OperationType = @OperationType AND ClientOperationSettingID !=@ClientOperationSettingID)
	BEGIN
		ROLLBACK
			RAISERROR (N'Already Exists this Operation Type, Please Find this Item. !!',16,1);	
		RETURN
	END
	Update ClientOperationSetting   SET OperationType = @OperationType,		DataType = @DataType,		Value = @Value,	DBUserID = @DBUserID,	DBServerDateTime = @DBServerDateTime WHERE ClientOperationSettingID= @ClientOperationSettingID
	SELECT * FROM ClientOperationSetting  WHERE ClientOperationSettingID= @ClientOperationSettingID
END

IF(@DBOperation=3)
BEGIN
	IF(@ClientOperationSettingID<=0)
	BEGIN
		ROLLBACK
			RAISERROR (N'Your Selected ClientOperationSetting Is Invalid Please Refresh and try again!!',16,1);	
		RETURN
	END		
	DELETE FROM ClientOperationSetting  WHERE ClientOperationSettingID= @ClientOperationSettingID
END
COMMIT TRAN







GO
/****** Object:  Table [dbo].[ClientOperationSetting]    Script Date: 1/2/2017 3:07:42 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ClientOperationSetting](
	[ClientOperationSettingID] [int] NOT NULL,
	[OperationType] [smallint] NULL,
	[DataType] [smallint] NULL,
	[Value] [varchar](1000) NULL,
	[DBUserID] [int] NULL,
	[DBServerDateTime] [datetime] NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[COSImage]    Script Date: 1/2/2017 3:07:42 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING OFF
GO
CREATE TABLE [dbo].[COSImage](
	[COSImageID] [int] NOT NULL,
	[OperationType] [smallint] NULL,
	[COSVFormat] [smallint] NULL,
	[ImageTitle] [varchar](512) NULL,
	[LargeImage] [varbinary](max) NULL,
	[DBUserID] [int] NULL,
	[DBServerDateTime] [datetime] NULL,
 CONSTRAINT [PK_COSImage] PRIMARY KEY CLUSTERED 
(
	[COSImageID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
