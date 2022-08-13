IF NOT EXISTS (SELECT * FROM sys.columns where Name = N'BUID' and Object_ID = Object_ID(N'Voucher'))
BEGIN
   ALTER TABLE Voucher
   ADD BUID int
END
GO

GO
/****** Object:  View [dbo].[View_BankAccount]    Script Date: 12/1/2016 1:00:07 PM ******/
DROP VIEW [dbo].[View_BankAccount]
GO
/****** Object:  View [dbo].[View_Voucher]    Script Date: 12/1/2016 1:00:07 PM ******/
DROP VIEW [dbo].[View_Voucher]
GO
/****** Object:  View [dbo].[View_VoucherDetail]    Script Date: 12/1/2016 1:00:07 PM ******/
DROP VIEW [dbo].[View_VoucherDetail]
GO
/****** Object:  UserDefinedFunction [dbo].[FN_VoucherNo]    Script Date: 12/1/2016 1:00:07 PM ******/
DROP FUNCTION [dbo].[FN_VoucherNo]
GO
/****** Object:  StoredProcedure [dbo].[SP_VoucherNo]    Script Date: 12/1/2016 1:00:07 PM ******/
DROP PROCEDURE [dbo].[SP_VoucherNo]
GO
/****** Object:  StoredProcedure [dbo].[SP_IUD_VoucherDetail]    Script Date: 12/1/2016 1:00:07 PM ******/
DROP PROCEDURE [dbo].[SP_IUD_VoucherDetail]
GO
/****** Object:  StoredProcedure [dbo].[SP_IUD_Voucher]    Script Date: 12/1/2016 1:00:07 PM ******/
GO
/****** Object:  StoredProcedure [dbo].[SP_IUD_Voucher]    Script Date: 12/1/2016 1:00:07 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

GO
/****** Object:  StoredProcedure [dbo].[SP_IUD_VoucherDetail]    Script Date: 12/1/2016 1:00:07 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SP_IUD_VoucherDetail]
(	
	@VoucherDetailID	int,
	@VoucherID	int,	
	@AreaID	int,
	@ZoneID	int,
	@SiteID	int,
	@ProductID	int,
	@DeptID	int,
	@AccountHeadID	int,
	@CostCenterID	int,
	@CurrencyID	int,
	@IsDebit	bit,
	@AmountInCurrency	decimal(30, 17),
	@ConversionRate	decimal(30, 17),
	@Amount	decimal(30, 17),
	@Narration	varchar(512),	
	@DBOperation as smallint,
	@VoucherDetailIDs varchar(512)
	--%n, %n, %n, %n, %n, %n, %n, %n, %n, %n, %b, %n, %n, %n, %s, %n, %s
)	
AS
BEGIN TRAN
IF(@DBOperation=1)
BEGIN			
	IF(ISNULL(@VoucherID,0)<=0)
	BEGIN
		ROLLBACK
			RAISERROR (N'Invalid voucher. Please try again!!~',16,1);	
		RETURN
	END	
	IF(ISNULL(@AccountHeadID,0)<=0)
	BEGIN
		ROLLBACK
			RAISERROR (N'Invalid Account Head. Please try again!!~',16,1);	
		RETURN
	END
	IF NOT EXISTS(SELECT * FROM ChartsOfAccount AS TT WHERE TT.AccountType=5 AND TT.AccountHeadID=@AccountHeadID)
	BEGIN
		ROLLBACK
			RAISERROR (N'Selected Account Head Not a Ledger/No Existence in Database Please try again!!~',16,1);	
		RETURN
	END
	IF(ISNULL(@CurrencyID,0)<=0)
	BEGIN
		ROLLBACK
			RAISERROR (N'Invalid Currency. Please try again!!~',16,1);	
		RETURN
	END
	IF(ISNULL(@AmountInCurrency,0)<=0)
	BEGIN
		ROLLBACK
			RAISERROR (N'Invalid Amount In Currency. Please try again!!~',16,1);	
		RETURN
	END
	IF(ISNULL(@ConversionRate,0)<=0)
	BEGIN
		ROLLBACK
			RAISERROR (N'Invalid Conversion Rate. Please try again!!~',16,1);	
		RETURN
	END
	IF(ISNULL(@Amount,0)<=0)
	BEGIN
		ROLLBACK
			RAISERROR (N'Invalid Amount. Please try again!!~',16,1);	
		RETURN
	END
	SET @VoucherDetailID=(SELECT ISNULL(MAX(VoucherDetailID),0)+1 FROM VoucherDetail)		
	INSERT INTO VoucherDetail	(VoucherDetailID,	VoucherID,	AreaID,		ZoneID,		SiteID,		ProductID,	DeptID,		AccountHeadID,	CostCenterID,	CurrencyID,		IsDebit,	AmountInCurrency,	ConversionRate,		Amount,		Narration)
    					VALUES  (@VoucherDetailID,	@VoucherID,	@AreaID,	@ZoneID,	@SiteID,	@ProductID,	@DeptID,	@AccountHeadID,	@CostCenterID,	@CurrencyID,	@IsDebit,	@AmountInCurrency,	@ConversionRate,	@Amount,	@Narration)			
	SELECT * FROM View_VoucherDetail WHERE VoucherDetailID=@VoucherDetailID
END
IF(@DBOperation=2)
BEGIN	
	IF(@VoucherDetailID<=0)
	BEGIN
		ROLLBACK
			RAISERROR (N'Invalid voucher. Please try again!!~',16,1);	
		RETURN
	END	
	IF(ISNULL(@VoucherID,0)<=0)
	BEGIN
		ROLLBACK
			RAISERROR (N'Invalid voucher. Please try again!!~',16,1);	
		RETURN
	END	
	IF(ISNULL(@AccountHeadID,0)<=0)
	BEGIN
		ROLLBACK
			RAISERROR (N'Invalid Account Head. Please try again!!~',16,1);	
		RETURN
	END
	IF(ISNULL(@CurrencyID,0)<=0)
	BEGIN
		ROLLBACK
			RAISERROR (N'Invalid Currency. Please try again!!~',16,1);	
		RETURN
	END
	IF(ISNULL(@AmountInCurrency,0)<=0)
	BEGIN
		ROLLBACK
			RAISERROR (N'Invalid Amount In Currency. Please try again!!~',16,1);	
		RETURN
	END
	IF(ISNULL(@ConversionRate,0)<=0)
	BEGIN
		ROLLBACK
			RAISERROR (N'Invalid Conversion Rate. Please try again!!~',16,1);	
		RETURN
	END
	IF(ISNULL(@Amount,0)<=0)
	BEGIN
		ROLLBACK
			RAISERROR (N'Invalid Amount. Please try again!!~',16,1);	
		RETURN
	END	
	UPDATE VoucherDetail set VoucherID=@VoucherID,	AreaID=@AreaID,		ZoneID=@ZoneID,		SiteID=@SiteID,		ProductID=@ProductID,	DeptID=@DeptID,		AccountHeadID=@AccountHeadID,	CostCenterID=@CostCenterID,	CurrencyID=@CurrencyID,		IsDebit=@IsDebit,	AmountInCurrency=@AmountInCurrency,	ConversionRate=@ConversionRate,		Amount=@Amount,		Narration=@Narration where VoucherDetailID=@VoucherDetailID		
	SELECT * FROM View_VoucherDetail WHERE VoucherDetailID=@VoucherDetailID
END
IF(@DBOperation=3)
BEGIN
	IF(@VoucherID<=0)
	BEGIN
		ROLLBACK
			RAISERROR (N'Your Selected Voucher Is Invalid Please Refresh and try again!!~',16,1);	
		RETURN
	END				
	DELETE FROM VoucherDetail WHERE VoucherID=@VoucherID AND VoucherDetailID NOT IN (SELECT * FROM dbo.SplitInToDataSet(@VoucherDetailIDs,','))
END
COMMIT TRAN





GO
/****** Object:  StoredProcedure [dbo].[SP_VoucherNo]    Script Date: 12/1/2016 1:00:07 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SP_VoucherNo] 
(
	@BUID as int,
	@VoucherTypeID as int,
	@VoucherDate as date
)
AS
BEGIN TRAN
--DECLARE
--@BUID as int,
--@VoucherTypeID as int,
--@VoucherDate as date

--SET @BUID = 1
--SET @VoucherTypeID=1
--SET @VoucherDate='01 Dec 2016'

DECLARE 
@VoucherCodeID as int,		
@VoucherCodeType as smallint,		
@Value as varchar(512),		
@Length as int,	
@Restart as smallint,		
@Sequence as int,
@TempVoucherNo as varchar(512),

--Extra 
@Temp as varchar(512),
@NumericStartIndex as int,
@TempVoucherCodeType as smallint,
@TempLength as int,
@TempSequence as int,
@TempNumericPart as int	


SET @TempVoucherNo=''
DECLARE Cur_AB1 CURSOR LOCAL FORWARD_ONLY KEYSET FOR
SELECT VoucherCodeID, VoucherCodeType, Value, [Length], [Restart], Sequence FROM VoucherCode WHERE VoucherTypeID=@VoucherTypeID ORDER BY Sequence
OPEN Cur_AB1
	FETCH NEXT FROM Cur_AB1 INTO  @VoucherCodeID, @VoucherCodeType, @Value, @Length, @Restart, @Sequence
	WHILE(@@Fetch_Status <> -1)
	BEGIN 				
		--EnumVoucherCodeType{None = 0, Text = 1, DateVariation = 2, Numeric = 3, Separator = 4}
		IF(@VoucherCodeType IN(1,4)) -- Text = 1, Separator = 4
		BEGIN
			SET @TempVoucherNo=@TempVoucherNo+@Value
		END
				
		IF(@VoucherCodeType =2) --DateVariation = 2
		BEGIN
			--EnumDisplayPart{Month = 0,Year = 1}
			IF(@Value='Month')
			BEGIN
				IF(@Length=3)
				BEGIN
					SET @TempVoucherNo=@TempVoucherNo+SUBSTRING((SELECT DATENAME(MONTH,@VoucherDate)),1,3)
				END
				ELSE
				BEGIN
					SET @TempVoucherNo=@TempVoucherNo+(SELECT DATENAME(MONTH,@VoucherDate))
				END
				
			END
			IF(@Value='Year')
			BEGIN
				IF(@Length=2)
				BEGIN
					SET @TempVoucherNo=@TempVoucherNo+SUBSTRING((SELECT DATENAME(YEAR,@VoucherDate)),3,2)
				END
				ELSE
				BEGIN
					SET @TempVoucherNo=@TempVoucherNo+(SELECT DATENAME(YEAR,@VoucherDate))
				END
			END			
		END
		
		IF(@VoucherCodeType =3) -- Numeric = 3
		BEGIN
			--find Numeric part start indes
			DECLARE 
			@Flag as bit
			SET @Flag=1
			
			--EnumRestartPeriod{None = 0,Monthly = 1,Yearly = 2}
			--check restart factor for None
			IF(@Restart=0)
			BEGIN
				IF NOT EXISTS(SELECT * FROM Voucher WHERE  VoucherTypeID=@VoucherTypeID)
				BEGIN
					SET @Flag=0
					SET @TempVoucherNo=@TempVoucherNo+(SELECT Value FROM VoucherCode WHERE VoucherCodeType=3 AND VoucherTypeID=@VoucherTypeID)
				END
			END

			--check restart factor for month
			IF(@Flag=1)
			BEGIN
				IF(@Restart=1)
				BEGIN
					IF NOT EXISTS(SELECT * FROM Voucher WHERE  DATEPART(MONTH,VoucherDate)=DATEPART(MONTH,@VoucherDate) AND DATEPART(YEAR,VoucherDate)=DATEPART(YEAR,@VoucherDate) AND VoucherTypeID=@VoucherTypeID)
					BEGIN
						SET @Flag=0
						SET @TempVoucherNo=@TempVoucherNo+(SELECT Value FROM VoucherCode WHERE VoucherCodeType=3 AND VoucherTypeID=@VoucherTypeID)
					END
				END
			END
			
			--check restart factor for year			
			IF(@Flag=1)
			BEGIN
				IF(@Restart=2)
				BEGIN
					IF NOT EXISTS(SELECT * FROM Voucher WHERE  DATEPART(YEAR,VoucherDate)=DATEPART(YEAR,@VoucherDate) AND VoucherTypeID=@VoucherTypeID)
					BEGIN
						SET @Flag=0
						SET @TempVoucherNo=@TempVoucherNo+(SELECT Value FROM VoucherCode WHERE VoucherCodeType=3 AND VoucherTypeID=@VoucherTypeID)
					END
				END
			END


			
			--get max voucher no
			IF(@Flag=1)
			BEGIN
				SET @NumericStartIndex=0
				DECLARE Cur_AB2 CURSOR LOCAL FORWARD_ONLY KEYSET FOR
				SELECT VoucherCodeType,[Length],Sequence FROM VoucherCode WHERE VoucherTypeID=@VoucherTypeID ORDER BY Sequence
				OPEN Cur_AB2
					FETCH NEXT FROM Cur_AB2 INTO  @TempVoucherCodeType,@TempLength, @TempSequence
					WHILE(@@Fetch_Status <> -1)
					BEGIN 
						IF(@TempVoucherCodeType=3)
						BEGIN
							BREAK
						END
						SET @NumericStartIndex=@NumericStartIndex+@TempLength
						FETCH NEXT FROM Cur_AB2 INTO  @TempVoucherCodeType,@TempLength, @TempSequence
					END
				CLOSE Cur_AB2
				DEALLOCATE Cur_AB2				
							
				--SET @Temp='CP-Dec/12-85471'	
				--get max VoucherNo
				--EnumRestartPeriod{None = 0,Monthly = 1,Yearly = 2}	
				IF(@Restart=0)
				BEGIN
					SET @TempNumericPart=(SELECT ISNULL(MAX(V.VoucherNoNumericPart),0)+1 FROM View_Voucher AS V WHERE V.VoucherTypeID=@VoucherTypeID AND V.BUID=@BUID)
				END		
				IF(@Restart=1)
				BEGIN
					SET @TempNumericPart=(SELECT ISNULL(MAX(V.VoucherNoNumericPart),0)+1 FROM View_Voucher AS V WHERE V.VoucherTypeID=@VoucherTypeID AND V.BUID=@BUID AND MONTH(V.VoucherDate)=MONTH(@VoucherDate) AND YEAR(V.VoucherDate)=YEAR(@VoucherDate))
				END
				IF(@Restart=2)
				BEGIN
					SET @TempNumericPart=(SELECT ISNULL(MAX(V.VoucherNoNumericPart),0)+1 FROM View_Voucher AS V WHERE V.VoucherTypeID=@VoucherTypeID AND V.BUID=@BUID AND YEAR(V.VoucherDate)=YEAR(@VoucherDate))
				END				
				SET @TempVoucherNo=@TempVoucherNo+	RIGHT('0000000000' + CONVERT(VARCHAR(10), @TempNumericPart), @Length)--Hope fully voucher code numeric part not more then 10 digit
			END
		END		
		FETCH NEXT FROM Cur_AB1 INTO  @VoucherCodeID, @VoucherCodeType, @Value, @Length, @Restart, @Sequence
	END
CLOSE Cur_AB1
DEALLOCATE Cur_AB1

DECLARE
@VoucherID as int,
@VoucherNo as Varchar(512),
@BaseCurrencyID as Varchar(512),
@BaseCurrencyNameSymbol as Varchar(512),
@BaseCurrencySymbol as Varchar(512),
@BUShortName as Varchar(512)

SET @BUShortName = ''
SET @BaseCurrencyID=(SELECT C.BaseCurrencyID FROM Company AS C WHERE CompanyID=1)
SET @BaseCurrencyNameSymbol =(SELECT CU.CurrencyName+'['+CU.Symbol+']' FROM Currency AS CU WHERE CU.CurrencyID=@BaseCurrencyID)
SET @BaseCurrencySymbol =(SELECT CU.Symbol FROM Currency AS CU WHERE CU.CurrencyID=@BaseCurrencyID)
SET @BUShortName = ISNULL((SELECT HH.ShortName FROM BusinessUnit AS HH WHERE HH.BusinessUnitID=@BUID),'')+'/'


SET @VoucherNo=@BUShortName+@TempVoucherNo
SELECT @BUID AS BUID,   @VoucherNo AS VoucherNo, (SELECT VoucherName FROM VoucherType WHERE VoucherTypeID=@VoucherTypeID) AS VoucherName, (SELECT NumberMethod FROM VoucherType WHERE VoucherTypeID=@VoucherTypeID) AS NumberMethod, @BaseCurrencyID AS CurrencyID, @BaseCurrencySymbol AS CUSymbol
COMMIT TRAN





GO
/****** Object:  UserDefinedFunction [dbo].[FN_VoucherNo]    Script Date: 12/1/2016 1:00:07 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[FN_VoucherNo] 
(
	@BUID as int,
	@VoucherTypeID as int,
	@VoucherDate as date	
)
RETURNS varchar (512)
AS
BEGIN
--DECLARE
--@VoucherTypeID as int,
--@VoucherDate as date
--SET @VoucherTypeID=5
--SET @VoucherDate='01 Feb 2013'

DECLARE 
@VoucherCodeID as int,		
@VoucherCodeType as smallint,		
@Value as varchar(512),		
@Length as int,	
@Restart as smallint,		
@Sequence as int,
@TempVoucherNo as varchar(512),

--Extra 
@Temp as varchar(512),
@NumericStartIndex as int,
@TempVoucherCodeType as smallint,
@TempLength as int,
@TempSequence as int,
@TempNumericPart as int	


SET @TempVoucherNo=''
DECLARE Cur_AB1 CURSOR LOCAL FORWARD_ONLY KEYSET FOR
SELECT VoucherCodeID, VoucherCodeType, Value, [Length], [Restart], Sequence FROM VoucherCode WHERE VoucherTypeID=@VoucherTypeID AND VoucherTypeID IN (SELECT VoucherTypeID FROM VoucherType) ORDER BY Sequence
OPEN Cur_AB1
	FETCH NEXT FROM Cur_AB1 INTO  @VoucherCodeID, @VoucherCodeType, @Value, @Length, @Restart, @Sequence
	WHILE(@@Fetch_Status <> -1)
	BEGIN 				
		--EnumVoucherCodeType{None = 0, Text = 1, DateVariation = 2, Numeric = 3, Separator = 4}
		IF(@VoucherCodeType IN(1,4)) -- Text = 1, Separator = 4
		BEGIN
			SET @TempVoucherNo=@TempVoucherNo+@Value
		END
				
		IF(@VoucherCodeType =2) --DateVariation = 2
		BEGIN
			--EnumDisplayPart{Month = 0,Year = 1}
			IF(@Value='Month')
			BEGIN
				IF(@Length=3)
				BEGIN
					SET @TempVoucherNo=@TempVoucherNo+SUBSTRING((SELECT DATENAME(MONTH,@VoucherDate)),1,3)
				END
				ELSE
				BEGIN
					SET @TempVoucherNo=@TempVoucherNo+(SELECT DATENAME(MONTH,@VoucherDate))
				END
				
			END
			IF(@Value='Year')
			BEGIN
				IF(@Length=2)
				BEGIN
					SET @TempVoucherNo=@TempVoucherNo+SUBSTRING((SELECT DATENAME(YEAR,@VoucherDate)),3,2)
				END
				ELSE
				BEGIN
					SET @TempVoucherNo=@TempVoucherNo+(SELECT DATENAME(YEAR,@VoucherDate))
				END
			END			
		END
		
		IF(@VoucherCodeType =3) -- Numeric = 3
		BEGIN
			--find Numeric part start indes
			DECLARE 
			@Flag as bit
			SET @Flag=1
			
			--EnumRestartPeriod{None = 0,Monthly = 1,Yearly = 2}
			--check restart factor for None
			IF(@Restart=0)
			BEGIN
				IF NOT EXISTS(SELECT * FROM Voucher WHERE  VoucherTypeID=@VoucherTypeID)
				BEGIN
					SET @Flag=0
					SET @TempVoucherNo=@TempVoucherNo+(SELECT Value FROM VoucherCode WHERE VoucherCodeType=3 AND VoucherTypeID=@VoucherTypeID)
				END
			END
			--check restart factor for month
			IF(@Restart=1)
			BEGIN
				IF NOT EXISTS(SELECT * FROM Voucher WHERE DATEPART(MONTH,VoucherDate)=DATEPART(MONTH,@VoucherDate) AND DATEPART(YEAR,VoucherDate)=DATEPART(YEAR,@VoucherDate) AND VoucherTypeID=@VoucherTypeID)
				BEGIN
					SET @Flag=0
					SET @TempVoucherNo=@TempVoucherNo+(SELECT Value FROM VoucherCode WHERE VoucherCodeType=3 AND VoucherTypeID=@VoucherTypeID AND VoucherTypeID IN (SELECT VoucherTypeID FROM VoucherType))
				END
			END
			
			--check restart factor for year			
			IF(@Flag=1)
			BEGIN
				IF(@Restart=2)
				BEGIN
					IF NOT EXISTS(SELECT * FROM Voucher WHERE DATEPART(YEAR,VoucherDate)=DATEPART(YEAR,@VoucherDate) AND VoucherTypeID=@VoucherTypeID)
					BEGIN
						SET @Flag=0
						SET @TempVoucherNo=@TempVoucherNo+(SELECT Value FROM VoucherCode WHERE VoucherCodeType=3 AND VoucherTypeID=@VoucherTypeID AND VoucherTypeID IN (SELECT VoucherTypeID FROM VoucherType))
					END
				END
			END
			
			--get max voucher no
			IF(@Flag=1)
			BEGIN
				SET @NumericStartIndex=0
				DECLARE Cur_AB2 CURSOR LOCAL FORWARD_ONLY KEYSET FOR
				SELECT VoucherCodeType,[Length],Sequence FROM VoucherCode WHERE VoucherTypeID=@VoucherTypeID AND VoucherTypeID IN (SELECT VoucherTypeID FROM VoucherType) ORDER BY Sequence
				OPEN Cur_AB2
					FETCH NEXT FROM Cur_AB2 INTO  @TempVoucherCodeType,@TempLength, @TempSequence
					WHILE(@@Fetch_Status <> -1)
					BEGIN 
						IF(@TempVoucherCodeType=3)
						BEGIN
							BREAK
						END
						SET @NumericStartIndex=@NumericStartIndex+@TempLength
						FETCH NEXT FROM Cur_AB2 INTO  @TempVoucherCodeType,@TempLength, @TempSequence
					END
				CLOSE Cur_AB2
				DEALLOCATE Cur_AB2				
							
				--SET @Temp='CP-Dec/12-85471'	
				--get max VoucherNo
				--EnumRestartPeriod{None = 0,Monthly = 1,Yearly = 2}	
				IF(@Restart=0)
				BEGIN
					SET @TempNumericPart=(SELECT ISNULL(MAX(V.VoucherNoNumericPart),0)+1 FROM View_Voucher AS V WHERE V.VoucherTypeID=@VoucherTypeID AND V.BUID=@BUID)
				END				
				IF(@Restart=1)
				BEGIN
					SET @TempNumericPart=(SELECT ISNULL(MAX(V.VoucherNoNumericPart),0)+1 FROM View_Voucher AS V WHERE V.VoucherTypeID=@VoucherTypeID AND V.BUID=@BUID AND MONTH(V.VoucherDate)=MONTH(@VoucherDate) AND YEAR(V.VoucherDate)=YEAR(@VoucherDate))
				END
				IF(@Restart=2)
				BEGIN
					SET @TempNumericPart=(SELECT ISNULL(MAX(V.VoucherNoNumericPart),0)+1 FROM View_Voucher AS V WHERE V.VoucherTypeID=@VoucherTypeID AND V.BUID=@BUID AND YEAR(V.VoucherDate)=YEAR(@VoucherDate))
				END
				SET @TempVoucherNo=@TempVoucherNo+	RIGHT('0000000000' + CONVERT(VARCHAR(10), @TempNumericPart), @Length)--Hope fully voucher code numeric part not more then 10 digit
			END
		END		
		FETCH NEXT FROM Cur_AB1 INTO  @VoucherCodeID, @VoucherCodeType, @Value, @Length, @Restart, @Sequence
	END
CLOSE Cur_AB1
DEALLOCATE Cur_AB1

DECLARE
@VoucherID as int,
@VoucherNo as Varchar(512)

SET @VoucherNo=@TempVoucherNo
--SELECT @VoucherNo AS VoucherNo
RETURN @VoucherNo
END



GO
/****** Object:  View [dbo].[View_VoucherDetail]    Script Date: 12/1/2016 1:00:07 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[View_VoucherDetail]
AS
SELECT	VD.VoucherDetailID, 
		VD.VoucherID, 
		Voucher.BUID, 
		VD.AreaID,
		VD.ZoneID,
		VD.SiteID,
		VD.ProductID,
		VD.DeptID,		
		VD.AccountHeadID, 
		VD.CostCenterID,
		VD.CurrencyID, 
		VD.AmountInCurrency, 
		VD.ConversionRate, 
		VD.Amount, 			
        VD.IsDebit,
        VD.Narration, 
        COA.AccountCode AS AccountHeadCode, 
        COA.AccountHeadName,		
        Voucher.OperationType,
		Voucher.VoucherDate,
		Voucher.VoucherTypeID,
		Voucher.AuthorizedBy,		
		CU.CurrencyName AS CUName,
		CU.Symbol AS CUSymbol,		
		ISNULL(Area.LocCode,'00') AS AreaCode,
		Area.Name AS AreaName,
		Area.ShortName AS AreaShortName,
		ISNULL(Zone.LocCode,'00') AS ZoneCode,
		Zone.Name AS ZoneName,
		Zone.ShortName AS ZoneShortName,
		ISNULL([Site].LocCode, '0000') AS SiteCode,
		[Site].Name AS SiteName,
		[Site].ShortName AS SiteShortName,
		ISNULL(VProduct.ProductCode,'00000') AS PCode,
		VProduct.ProductName AS PName,
		VProduct.ShortName AS PShortName,
		ISNULL(Dept.Code,'00') AS DeptCode,
		Dept.Name AS DeptName,		
		ISNULL(CC.Code,'0000') AS CCCode,
		CC.Name AS CCName,
		(SELECT HH.IsAreaEffect FROM BusinessUnit AS HH WHERE HH.BusinessUnitID=Voucher.BUID) AS IsAreaEffect,
		(SELECT HH.IsZoneEffect FROM BusinessUnit AS HH WHERE HH.BusinessUnitID=Voucher.BUID) AS IsZoneEffect,
		(SELECT HH.IsSiteEffect FROM BusinessUnit AS HH WHERE HH.BusinessUnitID=Voucher.BUID) AS IsSiteEffect,
		(SELECT TT.IsCostCenterApply FROM COA_AccountHeadConfig AS TT WHERE TT.AccountHeadID=VD.AccountHeadID) AS IsCostCenterApply,
		(SELECT TT.IsBillRefApply FROM COA_AccountHeadConfig AS TT WHERE TT.AccountHeadID=VD.AccountHeadID) AS IsBillRefApply,
		(SELECT TT.IsInventoryApply FROM COA_AccountHeadConfig AS TT WHERE TT.AccountHeadID=VD.AccountHeadID) AS IsInventoryApply,
		(SELECT TT.IsOrderReferenceApply FROM COA_AccountHeadConfig AS TT WHERE TT.AccountHeadID=VD.AccountHeadID) AS IsOrderReferenceApply,
		COA.AccountOperationType,
		(SELECT TT.IsPaymentCheque FROM VoucherType AS TT WHERE TT.VoucherTypeID=Voucher.VoucherTypeID) AS IsPaymentCheque
				
FROM        VoucherDetail AS VD
INNER JOIN ChartsOfAccount AS COA ON VD.AccountHeadID = COA.AccountHeadID
INNER JOIN Currency AS CU ON VD.CurrencyID = CU.CurrencyID
LEFT OUTER JOIN Location AS Area ON VD.AreaID = Area.LocationID
LEFT OUTER JOIN Location AS Zone ON VD.ZoneID = Zone.LocationID
LEFT OUTER JOIN Location AS [Site] ON VD.SiteID = [Site].LocationID
LEFT OUTER JOIN VProduct ON VD.ProductID = VProduct.VProductID
LEFT OUTER JOIN Department AS Dept ON VD.DeptID = Dept.DepartmentID
LEFT OUTER JOIN ACCostCenter AS CC ON VD.CostCenterID = CC.ACCostCenterID
LEFT OUTER JOIN Voucher ON VD.VoucherID = Voucher.VoucherID





GO
/****** Object:  View [dbo].[View_Voucher]    Script Date: 12/1/2016 1:00:07 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[View_Voucher]
AS
SELECT	Voucher.VoucherID,		
		Voucher.BUID,
		Voucher.VoucherTypeID,
		(ISNULL((SELECT HH.ShortName FROM BusinessUnit AS HH WHERE HH.BusinessUnitID=Voucher.BUID),'')+'/'+ Voucher.VoucherNo) as VoucherNo,
		Voucher.Narration,
		Voucher.ReferenceNote,
		Voucher.VoucherDate,		
		Voucher.AuthorizedBy,
		VoucherType.VoucherName,
		VoucherType.VoucherCategory,
		VoucherType.NumberMethod,
		Voucher.LastUpdatedDate,
		Voucher.OperationType,
		Voucher.DBUserID,
		Voucher.DBServerDate,
		Voucher.PrintCount,				
		Voucher.BatchID,
		Voucher.TaxEffect,
		Voucher.CounterVoucherID,
		BusinessUnit.Code AS BUCode,
		BusinessUnit.Name AS BUName,
		BusinessUnit.ShortName AS BUShortName,
		Authorized.UserName AS AuthorizedByName,
		Prepared.UserName AS PreparedByName,
		(SELECT COM.BaseCurrencyID FROM Company AS COM WHERE COM.CompanyID=1) AS BaseCurrencyID,
		(SELECT TT.Symbol FROM Currency AS TT WHERE TT.CurrencyID=(SELECT COM.BaseCurrencyID FROM Company AS COM WHERE COM.CompanyID=1)) AS BaseCUSymbol,
		(SELECT TOP 1 VD.CurrencyID FROM VoucherDetail AS VD WHERE VD.VoucherID=Voucher.VoucherID) As CurrencyID,
		(SELECT TOP 1 VD.CUSymbol FROM View_VoucherDetail AS VD WHERE VD.VoucherID=Voucher.VoucherID) As CUSymbol,
		(SELECT TOP 1 VD.ConversionRate FROM VoucherDetail AS VD WHERE VD.VoucherID=Voucher.VoucherID) As CRate,
		(SELECT ISNULL(SUM(VD.AmountInCurrency),0) FROM VoucherDetail AS VD WHERE VD.VoucherID=Voucher.VoucherID AND VD.IsDebit=1) As VoucherAmount,
		(SELECT ISNULL(SUM(Amount),0) FROM VoucherDetail AS VD WHERE VD.VoucherID=Voucher.VoucherID AND VD.IsDebit=1) As TotalAmount,
		dbo.FN_GetVoucherNoNumericPart(Voucher.VoucherID) as VoucherNoNumericPart,		
		(SELECT VB.BatchNO FROM VoucherBatch AS VB WHERE VB.VoucherBatchID=Voucher.BatchID) AS VoucherBatchNO

FROM	Voucher 
LEFT OUTER JOIN   BusinessUnit ON Voucher.BUID = BusinessUnit.BusinessUnitID
LEFT OUTER JOIN   VoucherType ON Voucher.VoucherTypeID = VoucherType.VoucherTypeID 
LEFT OUTER JOIN   Users AS Prepared ON Voucher.DBUserID = Prepared.UserID
LEFT OUTER JOIN   Users AS Authorized ON Voucher.AuthorizedBy = Authorized.UserID




















GO
/****** Object:  View [dbo].[View_BankAccount]    Script Date: 12/1/2016 1:00:07 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[View_BankAccount]
AS
SELECT      BA.CurrentLimit
		  , BA.BankAccountID
		  , BA.AccountName
		  , BA.AccountNo
		  , BA.BankID
		  , BA.BankBranchID
		  , BA.AccountType
		  , BA.LimitAmount
		  , BA.BusinessUnitID
		  , BA.AccountName + ' [' + BA.AccountNo + ']' AS BankAccountName
		  , B.Name + ' [' + BB.BranchName + ']' AS BankNameBranch
		  , B.Name + ' [' + BA.AccountNo + ']' AS BankNameAccountNo
		  , B.Name as BankName
		  , B.ShortName as BankShortName
		  , BB.BranchName as BranchName
		  , BU.Name AS BusinessUnitName 
		  , BU.Code AS BusinessUnitCode
		  , BU.Name+ ' ['+BU.Code+']' AS BusinessUnitNameCode

FROM         dbo.BankAccount AS BA 
left outer JOIN	dbo.BankBranch  AS BB ON BA.BankBranchID = BB.BankBranchID 
Left outer JOIN	dbo.Bank AS B ON BA.BankID = B.BankID
left outer JOIN dbo.BusinessUnit AS BU ON BA.BusinessUnitID=BU.BusinessUnitID









GO
ALTER VIEW [dbo].[View_VoucherType]
AS
SELECT		VoucherType.VoucherTypeID, 
			VoucherType.VoucherName, 			
			VoucherType.VoucherCategory, 
			VoucherType.NumberMethod,
			VoucherType.PrintAfterSave, 
			VoucherType.MustNarrationEntry, 
			VoucherType.IsProductRequired,
			VoucherType.IsDepartmentRequired,
			VoucherType.IsPaymentCheque,
			dbo.FN_VoucherNo(1, VoucherType.VoucherTypeID, GETDATE()) AS VoucherNumberFormat--Get voucher no format from function
FROM			VoucherType 













GO


