GO
/****** Object:  View [dbo].[View_CompanyWithLogo]    Script Date: 1/2/2017 3:02:48 PM ******/
DROP VIEW [dbo].[View_CompanyWithLogo]
GO
/****** Object:  View [dbo].[View_Company]    Script Date: 1/2/2017 3:02:48 PM ******/
DROP VIEW [dbo].[View_Company]
GO
/****** Object:  View [dbo].[View_BUWiseSubLedger]    Script Date: 1/2/2017 3:02:48 PM ******/
DROP VIEW [dbo].[View_BUWiseSubLedger]
GO
/****** Object:  View [dbo].[View_ACCostCenter]    Script Date: 1/2/2017 3:02:48 PM ******/
DROP VIEW [dbo].[View_ACCostCenter]
GO
/****** Object:  UserDefinedFunction [dbo].[FN_GetSLWBUName]    Script Date: 1/2/2017 3:02:48 PM ******/
DROP FUNCTION [dbo].[FN_GetSLWBUName]
GO
/****** Object:  UserDefinedFunction [dbo].[FN_GetSLWBUName]    Script Date: 1/2/2017 3:02:48 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[FN_GetSLWBUName]
(
	@SubLedgerID as int,
	@IsFullName as bit
)
RETURNS VARCHAR(MAX)
AS
--DECLARE 
--@SubLedgerID as int,
--@IsFullName as bit

--SET @SubLedgerID=85
--SET @IsFullName=0

BEGIN 
DECLARE 
@TempString as VARCHAR(MAX)

IF(@IsFullName=1)
BEGIN
	SELECT @TempString = COALESCE(@TempString + ', ', '') + HH.BUName FROM (SELECT DISTINCT BUSL.BUName
	FROM View_BUWiseSubLedger AS BUSL WHERE BUSL.SubLedgerID = @SubLedgerID) AS HH
END
ELSE
BEGIN
	SELECT @TempString = COALESCE(@TempString + ', ', '') + HH.BUShortName FROM (SELECT DISTINCT BUSL.BUShortName
	FROM View_BUWiseSubLedger AS BUSL WHERE BUSL.SubLedgerID = @SubLedgerID) AS HH
END
RETURN @TempString 
--SELECT @TempString
END


GO
/****** Object:  View [dbo].[View_ACCostCenter]    Script Date: 1/2/2017 3:02:48 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE View [dbo].[View_ACCostCenter]
AS
SELECT	ACCostCenter.ACCostCenterID,
		ACCostCenter.Code,
		ACCostCenter.Name,
		ACCostCenter.Description,
		ACCostCenter.ParentID,
		ACCostCenter.ReferenceType,
		ACCostCenter.ReferenceObjectID,
		ACCostCenter.ActivationDate,
		ACCostCenter.ExpireDate,
		ACCostCenter.IsActive,
		ACCostCenter.IsBillRefApply,
		ACCostCenter.IsChequeApply,
		dbo.FN_GetSLWBUName(ACCostCenter.ACCostCenterID, 0) AS BUName,
		(SELECT CC.Name FROM ACCostCenter AS CC WHERE CC.ACCostCenterID=ACCostCenter.ParentID) AS CategoryName,
		(SELECT CC.Code FROM ACCostCenter AS CC WHERE CC.ACCostCenterID=ACCostCenter.ParentID)+''+ACCostCenter.Code AS CategoryCode,
		(SELECT CC.IsBillRefApply FROM ACCostCenter AS CC WHERE CC.ACCostCenterID=ACCostCenter.ParentID) AS IsCategoryBillRefApply,
		(SELECT CC.IsChequeApply FROM ACCostCenter AS CC WHERE CC.ACCostCenterID=ACCostCenter.ParentID) AS IsCategoryChequeApply,
		ACCostCenter.Name+' ['+ACCostCenter.Code+']' AS NameCode
		                   
FROM	ACCostCenter
















GO
/****** Object:  View [dbo].[View_BUWiseSubLedger]    Script Date: 1/2/2017 3:02:48 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[View_BUWiseSubLedger]
AS
SELECT	BUWiseSubLedger.BUWiseSubLedgerID, 
		BUWiseSubLedger.BusinessUnitID, 
		BUWiseSubLedger.SubLedgerID, 		
		BusinessUnit.Code AS BUCode, 
		BusinessUnit.Name AS BUName, 
		BusinessUnit.ShortName AS BUShortName

FROM		BUWiseSubLedger 
INNER JOIN	BusinessUnit ON BUWiseSubLedger.BusinessUnitID = BusinessUnit.BusinessUnitID

GO
/****** Object:  View [dbo].[View_Company]    Script Date: 1/2/2017 3:02:48 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[View_Company]
AS
SELECT		Company.CompanyID, 
			Company.Name, 
			Company.Address, 
			Company.FactoryAddress,
			Company.Phone, 
			Company.Email, 
			Company.Note, 
			Company.WebAddress, 
			NULL AS OrganizationLogo, 
			Company.ImageSize, 
			Company.BaseCurrencyID,
			Company.OrganizationTitle, 
			NULL AS TitleImageSize,
			Company.BaseAddress,
			Currency.CurrencyName,
			Currency.Symbol AS CurrencySymbol,
            Currency.CurrencyName +'[ '+Currency.Symbol+' ]' AS CurrencyNameSymbol
FROM		Company 
LEFT OUTER JOIN	Currency ON Company.BaseCurrencyID = Currency.CurrencyID










GO
/****** Object:  View [dbo].[View_CompanyWithLogo]    Script Date: 1/2/2017 3:02:48 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[View_CompanyWithLogo]
AS
SELECT		Company.CompanyID, 
			Company.Name, 
			Company.Address, 
			Company.FactoryAddress,
			Company.Phone, 
			Company.Email, 
			Company.Note, 
			Company.WebAddress, 
			Company.OrganizationLogo, 
			Company.ImageSize, 
			Company.BaseCurrencyID,
			Company.OrganizationTitle, 
			Company.TitleImageSize,
			Company.BaseAddress,
			Currency.CurrencyName,
			Currency.Symbol AS CurrencySymbol,
            Currency.CurrencyName +'[ '+Currency.Symbol+' ]' AS CurrencyNameSymbol
FROM		Company 
LEFT OUTER JOIN	Currency ON Company.BaseCurrencyID = Currency.CurrencyID











GO
