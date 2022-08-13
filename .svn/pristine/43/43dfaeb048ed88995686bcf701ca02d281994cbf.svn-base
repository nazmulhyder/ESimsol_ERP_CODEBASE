DECLARE
@ACCostCenterID as int, 
@IsBillRefApply as bit,
@ParentID as int,
@SubledgerRefConfigID	as int,
@AccountHeadID	as int,
@SubledgerID	as int,
@IsOrderRefApply	as bit,
@DBUserID as  int,	 
@DBOperation as smallint,
@DBServerDateTime as datetime
SET @DBServerDateTime = GETDATE()
SET @IsOrderRefApply=0
SET @DBUserID = -9

DELETE FROM SubledgerRefConfig

DECLARE Cur_AB1 CURSOR LOCAL FORWARD_ONLY KEYSET FOR
SELECT HH.ACCostCenterID, HH.IsBillRefApply, HH.ParentID FROM ACCostCenter AS HH WHERE HH.ACCostCenterID!=1 AND HH.ParentID!=1 AND HH.IsBillRefApply=1
OPEN Cur_AB1
FETCH NEXT FROM Cur_AB1 INTO  @ACCostCenterID, @IsBillRefApply, @ParentID
WHILE(@@Fetch_Status <> -1)
BEGIN
	
	DECLARE Cur_AB2 CURSOR LOCAL FORWARD_ONLY KEYSET FOR
	SELECT MM.AccountHeadID FROM AccountHeadConfigure AS MM WHERE MM.ReferenceObjectType=1 AND MM.ReferenceObjectID=@ParentID
	OPEN Cur_AB2
	FETCH NEXT FROM Cur_AB2 INTO  @AccountHeadID
	WHILE(@@Fetch_Status <> -1)
	BEGIN
		IF NOT EXISTS(SELECT * FROM SubledgerRefConfig AS HH WHERE HH.AccountHeadID=@AccountHeadID AND HH.SubledgerID=@ACCostCenterID)
		BEGIN
			SET @SubledgerRefConfigID=(SELECT ISNULL(MAX(SubledgerRefConfigID),0)+1 FROM SubledgerRefConfig)				
				INSERT INTO SubledgerRefConfig	(SubledgerRefConfigID,		AccountHeadID,		SubledgerID,		IsBillRefApply,		IsOrderRefApply,	DBUserID,		DBServerDateTime)
    									VALUES	(@SubledgerRefConfigID,		@AccountHeadID,		@ACCostCenterID,	@IsBillRefApply,	@IsOrderRefApply,	@DBUserID,		@DBServerDateTime)
		END
		ELSE
		BEGIN
			SET @SubledgerRefConfigID = ISNULL((SELECT HH.SubledgerRefConfigID FROM SubledgerRefConfig AS HH WHERE HH.AccountHeadID=@AccountHeadID AND HH.SubledgerID=@ACCostCenterID),0)
			UPDATE SubledgerRefConfig  SET AccountHeadID=@AccountHeadID, SubledgerID=@ACCostCenterID,	IsBillRefApply=@IsBillRefApply, IsOrderRefApply=@IsOrderRefApply, DBUserID=@DBUserID, DBServerDateTime=@DBServerDateTime  WHERE SubledgerRefConfigID= @SubledgerRefConfigID
		END
		FETCH NEXT FROM Cur_AB2 INTO  @AccountHeadID
	END
	CLOSE Cur_AB2
	DEALLOCATE Cur_AB2
	FETCH NEXT FROM Cur_AB1 INTO  @ACCostCenterID, @IsBillRefApply, @ParentID
END
CLOSE Cur_AB1
DEALLOCATE Cur_AB1






