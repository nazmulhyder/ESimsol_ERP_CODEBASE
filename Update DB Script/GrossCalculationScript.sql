
/****** Object:  View [dbo].[View_GrossSalaryCalculation]    Script Date: 11/5/2018 11:28:34 AM ******/
DROP VIEW [dbo].[View_GrossSalaryCalculation]
GO
/****** Object:  Table [dbo].[GrossSalaryCalculation]    Script Date: 11/5/2018 11:28:34 AM ******/
DROP TABLE [dbo].[GrossSalaryCalculation]
GO
/****** Object:  StoredProcedure [dbo].[SP_IUD_GrossSalaryCalculation]    Script Date: 11/5/2018 11:28:34 AM ******/
DROP PROCEDURE [dbo].[SP_IUD_GrossSalaryCalculation]
GO
/****** Object:  StoredProcedure [dbo].[SP_IUD_GrossSalaryCalculation]    Script Date: 11/5/2018 11:28:34 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SP_IUD_GrossSalaryCalculation]
(



	    @Param_GSCID AS int,
		@Param_SalarySchemeID AS INT,
		@Param_ValueOperator AS SMALLINT,
		@Param_CalculationOn AS INT,
		@Param_FixedValue DECIMAL(30,17),
		@Param_Operator AS SMALLINT,
		@Param_SalaryHeadID AS INT,
		@Param_PercentVelue AS DECIMAL(18,2),
		@Param_DBUserID AS int,
		@Param_DBOperation AS smallint
		
	    --%n,%n,%n,%n,%n,%n,%n,%n,%n,%n
  

)

AS
BEGIN TRAN	
DECLARE
	@PV_DBServerDateTime as datetime
	SET @PV_DBServerDateTime=Getdate()

IF(@Param_DBOperation=1)
BEGIN
	
	IF(@Param_CalculationOn=3 AND @Param_Operator !=0)
	BEGIN
		ROLLBACK
			RAISERROR (N'Your are not allowed to select an operator for the fixed value !!',16,1);	
		RETURN
	END	
	
	SET @Param_GSCID=(SELECT ISNULL(MAX(GSCID),0)+1 FROM GrossSalaryCalculation)
	
	INSERT INTO  GrossSalaryCalculation(GSCID,        SalarySchemeID ,       ValueOperator ,       CalculationOn ,       FixedValue ,       Operator ,       SalaryHeadID ,       PercentVelue,  DBUserID ,       DBServerDateTime)
                                   VALUES       (@Param_GSCID ,@Param_SalarySchemeID ,@Param_ValueOperator ,@Param_CalculationOn ,@Param_FixedValue ,@Param_Operator ,@Param_SalaryHeadID , @Param_PercentVelue, @Param_DBUserID ,@PV_DBServerDateTime)

	SELECT * FROM View_GrossSalaryCalculation WHERE GSCID= @Param_GSCID
END

IF(@Param_DBOperation=2)
BEGIN
	IF(@Param_GSCID<=0)
	BEGIN
		ROLLBACK
			RAISERROR (N'Your Selected SalarySchemeDetailCalculation Is Invalid Please Refresh and try again!!',16,1);	
		RETURN
	END	
   
   IF(@Param_CalculationOn=3 AND @Param_Operator !=0)
	BEGIN
		ROLLBACK
			RAISERROR (N'Your are not allowed to select an operator for the fixed value !!',16,1);	
		RETURN
	END	
   
	UPDATE GrossSalaryCalculation SET ValueOperator = @Param_ValueOperator
                                           , CalculationOn = @Param_CalculationOn
										   , FixedValue = @Param_FixedValue
                                           , Operator = @Param_Operator
                                           , PercentVelue=@Param_PercentVelue
      
	WHERE GSCID= @Param_GSCID
	
	SELECT * FROM View_GrossSalaryCalculation WHERE GSCID= @Param_GSCID
END

IF(@Param_DBOperation=3)
BEGIN
	IF(@Param_GSCID<=0)
	BEGIN
		ROLLBACK
			RAISERROR (N'Your Selected Employee Is Invalid Please Refresh and try again!!',16,1);	
		RETURN
	END	
			
	DELETE FROM GrossSalaryCalculation WHERE GSCID= @Param_GSCID
END

COMMIT TRAN




GO
/****** Object:  Table [dbo].[GrossSalaryCalculation]    Script Date: 11/5/2018 11:28:34 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[GrossSalaryCalculation](
	[GSCID] [int] NOT NULL,
	[SalarySchemeID] [int] NOT NULL,
	[ValueOperator] [smallint] NOT NULL,
	[CalculationOn] [int] NOT NULL,
	[FixedValue] [decimal](30, 17) NOT NULL,
	[Operator] [smallint] NOT NULL,
	[SalaryHeadID] [int] NOT NULL,
	[PercentVelue] [decimal](18, 2) NULL,
	[DBUserID] [int] NOT NULL,
	[DBServerDateTime] [datetime] NOT NULL,
 CONSTRAINT [PK_GrossSalaryCalculation] PRIMARY KEY CLUSTERED 
(
	[GSCID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  View [dbo].[View_GrossSalaryCalculation]    Script Date: 11/5/2018 11:28:34 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




CREATE VIEW [dbo].[View_GrossSalaryCalculation]
AS
SELECT     GrossSalaryCalculation.*
         , SalaryHead.Name AS SalaryHeadName
         
FROM       GrossSalaryCalculation 

LEFT JOIN SalaryHead ON GrossSalaryCalculation.SalaryHeadID = SalaryHead.SalaryHeadID






GO
