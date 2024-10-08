/****** Object:  StoredProcedure [dbo].[SP_IUD_LB_UserLocationMap]    Script Date: 1/12/2017 9:25:48 PM ******/
DROP PROCEDURE [dbo].[SP_IUD_LB_UserLocationMap]
GO
/****** Object:  StoredProcedure [dbo].[SP_IUD_LB_Location]    Script Date: 1/12/2017 9:25:48 PM ******/
DROP PROCEDURE [dbo].[SP_IUD_LB_Location]
GO
/****** Object:  StoredProcedure [dbo].[SP_IUD_LB_Location]    Script Date: 1/12/2017 9:25:48 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[SP_IUD_LB_Location]
(
	@LB_LocationID as int,
	@LB_IPV4 as varchar(50),
	@LB_KnownName as nvarchar(63),
	@LB_LocationNote as nvarchar(127),
	@LB_Is_Classified as bit,
	@LB_ClassificationDate as datetime,
	@LB_ClasifiedBy as int,
	@LB_FirstHitDate as datetime,
	@LB_FirstHitBy as int,
	@DBOperation as smallint
	
	--%n, %s, %s, %s, %b, %D, %n, %D, %n, %n
)
		
AS
BEGIN TRAN

IF(@DBOperation=1)
BEGIN	
		IF(@LB_IPV4<='')
		BEGIN
			ROLLBACK
				RAISERROR (N' No IP address!!',16,1);	
			RETURN
		END		

		--if same ip already recorded then not required to insert
		IF NOT EXISTS(SELECT * FROM LB_Location WHERE LB_Location.LB_IPV4= @LB_IPV4)
		BEGIN
			SET @LB_LocationID=(SELECT ISNULL(MAX(LB_LocationID),0)+1 FROM LB_Location)		
			SET @LB_FirstHitDate=getdate()	
			set @LB_ClassificationDate=null;	
			INSERT INTO [LB_Location] ([LB_LocationID], [LB_IPV4],       [LB_KnownName],          [LB_LocationNote],      [LB_Is_Classified], [LB_ClassificationDate], [LB_ClasifiedBy], [LB_FirstHitDate], [LB_FirstHitBy])
								VALUES (@LB_LocationID, @LB_IPV4,  ISNULL(@LB_KnownName,''),  ISNULL(@LB_LocationNote,''),  @LB_Is_Classified, @LB_ClassificationDate,  @LB_ClasifiedBy,  @LB_FirstHitDate,  @LB_FirstHitBy)

		END
		Select * from LB_Location Where LB_LocationID=@LB_LocationID
END
-- Edit request for classified status update, KnownName Change, LocationNote change
IF(@DBOperation=2)
BEGIN
	IF(@LB_LocationID<=0)
	BEGIN
		ROLLBACK
			RAISERROR (N'Invalid selection, Please Refresh and try again!!',16,1);	
		RETURN
	END	
	UPDATE [LB_Location]
    SET LB_KnownName = ISNULL(@LB_KnownName,''), [LB_LocationNote] = ISNULL(@LB_LocationNote,''), [LB_Is_Classified] = @LB_Is_Classified, [LB_ClassificationDate]=@LB_ClassificationDate, [LB_ClasifiedBy]=@LB_ClasifiedBy
	WHERE [LB_LocationID] = @LB_LocationID  
	
	Select * from LB_Location Where LB_LocationID=@LB_LocationID
END

COMMIT TRAN



GO
/****** Object:  StoredProcedure [dbo].[SP_IUD_LB_UserLocationMap]    Script Date: 1/12/2017 9:25:48 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:	Mohammad Shahjada Sagor
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[SP_IUD_LB_UserLocationMap]
(
	@LB_UserLocationMapID as int,
	@LB_UserID as int,
	@LB_LB_LocationID as int,
	@LB_ExpireDateTime as DateTime,
	@DBUserID as int,
	@DBOperation as smallint

	--%n, %n, %n, %D, %n, %n
)
	
AS
BEGIN TRAN

IF(@DBOperation=1 AND @DBOperation=2)
BEGIN	
	IF NOT EXISTS(SELECT * FROM USERS WHERE UserID= @LB_UserID)
	BEGIN
		ROLLBACK
			RAISERROR (N'Invalid User!',16,1);	
		RETURN
	END
	IF NOT EXISTS(SELECT * FROM LB_Location WHERE LB_LocationID= @LB_LB_LocationID)
	BEGIN
		ROLLBACK
			RAISERROR (N'Invalid User!',16,1);	
		RETURN
	END
END

IF(@DBOperation=1)
BEGIN			
		IF NOT EXISTS(SELECT * FROM LB_UserLocationMap Where LB_UserID=@LB_UserID AND LB_LB_LocationID=@LB_LB_LocationID)
		BEGIN
			SET @LB_UserLocationMapID =(SELECT ISNULL(MAX(LB_UserLocationMapID),0)+1 FROM LB_UserLocationMap)
			INSERT INTO LB_UserLocationMap ( LB_UserLocationMapID,  LB_UserID,   LB_LB_LocationID, LB_ExpireDateTime, LB_DBServerDateTime, LB_DBServerUserID)
    								VALUES ( @LB_UserLocationMapID, @LB_UserID, @LB_LB_LocationID, @LB_ExpireDateTime,      GETDATE(),      @DBUserID)    					    				  
		END
		ELSE
		BEGIN
			SET @LB_UserLocationMapID =(SELECT LB_UserLocationMapID FROM LB_UserLocationMap  WHERE LB_UserID= @LB_UserID And LB_LB_LocationID=@LB_LB_LocationID)
			Update LB_UserLocationMap SET  LB_ExpireDateTime = @LB_ExpireDateTime Where LB_UserLocationMapID= @LB_UserLocationMapID
		END
		SELECT * FROM View_LB_UserLocationMap  WHERE LB_UserLocationMapID= @LB_UserLocationMapID

END


IF(@DBOperation=2)
BEGIN
	IF NOT EXISTS(SELECT * FROM LB_UserLocationMap WHERE LB_UserLocationMapID= @LB_UserLocationMapID)
	BEGIN
		ROLLBACK
			RAISERROR (N'Invalid location bind user!',16,1);	
		RETURN
	END		
	Update LB_UserLocationMap SET LB_UserID = @LB_UserID,   LB_LB_LocationID = @LB_LB_LocationID, LB_ExpireDateTime = @LB_ExpireDateTime Where LB_UserLocationMapID= @LB_UserLocationMapID

	SELECT * FROM View_LB_UserLocationMap  WHERE LB_UserLocationMapID= @LB_UserLocationMapID
END

IF(@DBOperation=3)
BEGIN
	IF NOT EXISTS(SELECT * FROM LB_UserLocationMap WHERE LB_UserLocationMapID= @LB_UserLocationMapID)
	BEGIN
		ROLLBACK
			RAISERROR (N'Invalid location bind user!',16,1);	
		RETURN
	END	
	DELETE LB_UserLocationMap WHERE LB_UserLocationMapID= @LB_UserLocationMapID
END



COMMIT TRAN



GO
