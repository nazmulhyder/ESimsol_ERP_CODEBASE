SELECT TOP 10 * FROM View_DUDeliveryChallan WHERE BUID=1 AND ISNULL(ApproveBy,0)!=0 AND DUDeliveryChallanID NOT IN (SELECT HH.PKValue FROM VoucherMapping AS HH WHERE HH.TableName='DUDeliveryChallan' AND HH.PKColumnName='DUDeliveryChallanID' AND HH.VoucherSetup=3) ORDER BY DUDeliveryChallanID ASC
SELECT HH.ChallanDate FROM DUDeliveryChallan AS HH WHERE HH.DUDeliveryChallanID=1
SELECT HH.ChallanNo FROM DUDeliveryChallan AS HH WHERE HH.DUDeliveryChallanID=1
SELECT HH.PINo FROM View_DUDeliveryChallan AS HH WHERE HH.DUDeliveryChallanID=1
SELECT * FROM View_DUDeliveryChallan AS HH WHERE HH.DUDeliveryChallanID=1
SELECT 'Receivable Account(DElivery)' AS FixedAccountHead FROM DUDeliveryChallan WHERE DUDeliveryChallanID=1
SELECT 'Sales A/C(Delivery)' AS SalesAccount FROM DUDeliveryChallan WHERE DUDeliveryChallanID=1
SELECT MM.CurrencyID FROM View_ExportSC AS MM WHERE MM.ExportPIID=1
SELECT 78 AS CRate FROM DUDeliveryChallan WHERE DUDeliveryChallanID=1
SELECT HH.Note FROM DUDeliveryChallan AS HH WHERE HH.DUDeliveryChallanID=1
SELECT ISNULL(SUM(HH.Qty*HH.UnitPrice),0)  FROM View_DUDeliveryChallanDetail AS HH WHERE HH.DUDeliveryChallanID=4512
SELECT MM.ACCostCenterID FROM ACCostCenter AS MM WHERE MM.ReferenceType=1 AND MM.ReferenceObjectID =(SELECT HH.PartyID FROM View_DUDeliveryChallan AS HH WHERE DUDeliveryChallanID=524)

SELECT DISTINCT HH.ExportPIID,HH.PINo,HH.PIDate,HH.DUDeliveryChallanID  FROM View_DUDeliveryChallanDetail AS HH WHERE HH.DUDeliveryChallanID=1

SELECT HH.ChallanNo FROM DUDeliveryChallan AS HH WHERE HH.DUDeliveryChallanID=%n
with PI No :
SELECT HH.PINo FROM View_DUDeliveryChallan AS HH WHERE HH.DUDeliveryChallanID=%n