using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Framework;
using ICS.Core.Utility;
using ICS.Core.DataAccess;

namespace ESimSol.Services.DataAccess
{
    public class OrderRecapDA
    {
        public OrderRecapDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, OrderRecap oOrderRecap, EnumDBOperation eEnumDBOrderRecap, Int64 nUserId)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_OrderRecap]"
                                    + "%n, %n,%s,%n, %s,%s,%n,%n,%n, %s, %n, %n,%n,%n, %s,%s,%s,%s, %n,%d, %n, %n,%s, %d, %d,  %n, %n, %d, %n, %s,%s,%s,%s,%s, %n, %n, %s, %s,%d, %n,%n, %n,%n,%n, %n,%n",
                                    oOrderRecap.OrderRecapID, oOrderRecap.BUID, oOrderRecap.OrderRecapNo, (int)oOrderRecap.OrderRecapStatus, oOrderRecap.OrderTypeInInt, oOrderRecap.SLNo, oOrderRecap.BusinessSessionID, oOrderRecap.TechnicalSheetID, oOrderRecap.ProductID, oOrderRecap.CollectionNo, oOrderRecap.BuyerID, oOrderRecap.BuyerContactPersonID, oOrderRecap.MerchandiserID, (int)oOrderRecap.Incoterms,  oOrderRecap.GG, oOrderRecap.SpecialFinish,oOrderRecap.Count,oOrderRecap.Weight,   (int)oOrderRecap.TransportType, oOrderRecap.BoardDate, oOrderRecap.AgentID, oOrderRecap.FabricID, oOrderRecap.Description, oOrderRecap.OrderDate, oOrderRecap.ShipmentDate, oOrderRecap.CurrencyID, oOrderRecap.ApproveBy, oOrderRecap.ApproveDate, oOrderRecap.ProductionFactoryID,oOrderRecap.DeliveryTerm,oOrderRecap.PaymentTerm,oOrderRecap.RequiredSample,oOrderRecap.PackingInstruction,oOrderRecap.Assortment, oOrderRecap.LocalYarnSupplierID, oOrderRecap.ImportYarnSupplierID, oOrderRecap.CommercialRemarks, oOrderRecap.BarCodeNo, oOrderRecap.FactoryShipmentDate, oOrderRecap.CartonQty, oOrderRecap.QtyPerCarton, (int)oOrderRecap.AssortmentType, oOrderRecap.MachineQty, oOrderRecap.PAMID,  nUserId, (int)eEnumDBOrderRecap);
        }

        public static void Delete(TransactionContext tc, OrderRecap oOrderRecap, EnumDBOperation eEnumDBOrderRecap, Int64 nUserId)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_OrderRecap]"
                                    + "%n, %n,%s,%n, %s,%s,%n,%n,%n, %s, %n, %n,%n,%n, %s,%s,%s,%s, %n,%d, %n, %n,%s, %d, %d,  %n, %n, %d, %n, %s,%s,%s,%s,%s, %n, %n, %s, %s,%d, %n,%n, %n,%n,%n, %n,%n",
                                    oOrderRecap.OrderRecapID, oOrderRecap.BUID, oOrderRecap.OrderRecapNo, (int)oOrderRecap.OrderRecapStatus, oOrderRecap.OrderTypeInInt, oOrderRecap.SLNo, oOrderRecap.BusinessSessionID, oOrderRecap.TechnicalSheetID, oOrderRecap.ProductID, oOrderRecap.CollectionNo, oOrderRecap.BuyerID, oOrderRecap.BuyerContactPersonID, oOrderRecap.MerchandiserID, (int)oOrderRecap.Incoterms, oOrderRecap.GG, oOrderRecap.SpecialFinish, oOrderRecap.Count, oOrderRecap.Weight, (int)oOrderRecap.TransportType, oOrderRecap.BoardDate, oOrderRecap.AgentID, oOrderRecap.FabricID, oOrderRecap.Description, oOrderRecap.OrderDate, oOrderRecap.ShipmentDate, oOrderRecap.CurrencyID, oOrderRecap.ApproveBy, oOrderRecap.ApproveDate, oOrderRecap.ProductionFactoryID, oOrderRecap.DeliveryTerm, oOrderRecap.PaymentTerm, oOrderRecap.RequiredSample, oOrderRecap.PackingInstruction, oOrderRecap.Assortment, oOrderRecap.LocalYarnSupplierID, oOrderRecap.ImportYarnSupplierID, oOrderRecap.CommercialRemarks, oOrderRecap.BarCodeNo, oOrderRecap.FactoryShipmentDate, oOrderRecap.CartonQty, oOrderRecap.QtyPerCarton, (int)oOrderRecap.AssortmentType, oOrderRecap.MachineQty, oOrderRecap.PAMID, nUserId, (int)eEnumDBOrderRecap);
        }

        //AcceptRevise
        public static IDataReader AcceptRevise(TransactionContext tc, OrderRecap oOrderRecap, string sOrderRecapDetailsIDsInfo,  Int64 nUserId)
        {
            return tc.ExecuteReader("EXEC [SP_AcceptOrderRecapRevise]"
                                    + "%n,%n, %s, %n, %n, %s, %n, %n, %n, %s, %n, %n, %n, %n, %s, %s, %s, %s, %n, %d, %n, %n, %s, %d, %d, %n, %n, %s,%s, %s, %s, %s, %n, %n, %s, %s, %d, %n, %n, %n,%n, %n, %s",
                                    oOrderRecap.OrderRecapID, oOrderRecap.BUID, oOrderRecap.OrderRecapNo, (int)oOrderRecap.OrderRecapStatus, oOrderRecap.OrderTypeInInt, oOrderRecap.SLNo, oOrderRecap.BusinessSessionID, oOrderRecap.TechnicalSheetID, oOrderRecap.ProductID, oOrderRecap.CollectionNo, oOrderRecap.BuyerID, oOrderRecap.BuyerContactPersonID, oOrderRecap.MerchandiserID, (int)oOrderRecap.Incoterms, oOrderRecap.GG, oOrderRecap.SpecialFinish, oOrderRecap.Count, oOrderRecap.Weight, (int)oOrderRecap.TransportType, oOrderRecap.BoardDate, oOrderRecap.AgentID, oOrderRecap.FabricID, oOrderRecap.Description, oOrderRecap.OrderDate, oOrderRecap.ShipmentDate, oOrderRecap.CurrencyID,  oOrderRecap.ProductionFactoryID, oOrderRecap.DeliveryTerm, oOrderRecap.PaymentTerm, oOrderRecap.RequiredSample, oOrderRecap.PackingInstruction, oOrderRecap.Assortment, oOrderRecap.LocalYarnSupplierID, oOrderRecap.ImportYarnSupplierID, oOrderRecap.CommercialRemarks, oOrderRecap.BarCodeNo,  oOrderRecap.FactoryShipmentDate, oOrderRecap.CartonQty, oOrderRecap.QtyPerCarton, (int)oOrderRecap.AssortmentType,  oOrderRecap.MachineQty, nUserId, sOrderRecapDetailsIDsInfo);
        }
       
        public static void ActiveInActive(TransactionContext tc, OrderRecap oOrderRecap, Int64 nUserId)
        {
            tc.ExecuteNonQuery("UPDATE OrderRecap SET IsActive=%b WHERE OrderRecapID=%n", oOrderRecap.IsActive, oOrderRecap.OrderRecapID);
        }

        public static void ShippedUnShipped(TransactionContext tc, OrderRecap oOrderRecap, Int64 nUserId)
        {
            tc.ExecuteNonQuery("UPDATE OrderRecap SET IsShippedOut=%b WHERE OrderRecapID=%n", oOrderRecap.IsShippedOut, oOrderRecap.OrderRecapID);
        }

        public static IDataReader ChangeStatus(TransactionContext tc, OrderRecap oOrderRecap, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_OrderRecapOperation]"+ "%n,%n,%s,%n,%n",oOrderRecap.OrderRecapID,  (int)oOrderRecap.ActionType, oOrderRecap.Description,   nUserID, (int)eEnumDBOperation);
        }


        public static void UpdateCMValue(TransactionContext tc, int id, double nCMValue)
        {
            tc.ExecuteNonQuery("Update OrderRecap SET CMValue = "+nCMValue+" WHERE OrderRecapID = "+id);
        }

        public static void UpdateInfo(TransactionContext tc, OrderRecap oOrderRecap, Int64 nUserId)
        {
            tc.ExecuteNonQuery("Update OrderRecap SET ShipmentDate = %d, Description = %s, DBUserID = %n, DBServerDateTime = %d, TransportType=%n, ProductionFactoryID = %n, LocalYarnSupplierID=%n, ImportYarnSupplierID=%n, CommercialRemarks=%s  WHERE OrderRecapID = %n", oOrderRecap.ShipmentDate, oOrderRecap.Description, nUserId, DateTime.Now, (int)oOrderRecap.TransportType, oOrderRecap.ProductionFactoryID,  oOrderRecap.LocalYarnSupplierID, oOrderRecap.ImportYarnSupplierID, oOrderRecap.CommercialRemarks, oOrderRecap.OrderRecapID);
        }

        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_OrderRecap WHERE OrderRecapID=%n", nID);
        }

        public static IDataReader GetByLog(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_OrderRecapLog WHERE OrderRecapLogID=%n", nID);
        }
        public static IDataReader GetSContractCM(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT ISNULL(ApprovedBy,0) AS ApprovedBy FROM SalesContract WHERE SalesContractID = (SELECT top 1 SalesContractID FROM SalesContractDetail WHERE OrderRecapID = %n)", nID);
        }

        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_OrderRecap");
        }

        public static IDataReader GetsByBUWithOrderType(TransactionContext tc, int nBUIID, string sOTypes)
        {
            if(sOTypes!="" && sOTypes!=null)
            {
                return tc.ExecuteReader("SELECT * FROM View_OrderRecap AS SO WHERE SO.BUID =%n AND OrderType IN (" + sOTypes + ") AND ISNULL(OrderRecapStatus,0) = " + (int)EnumOrderRecapStatus.Initialized, nBUIID);
            }
            else
            {
                return tc.ExecuteReader("SELECT * FROM View_OrderRecap AS SO WHERE SO.BUID =%n AND ISNULL(OrderRecapStatus,0) = " + (int)EnumOrderRecapStatus.Initialized, nBUIID);
            }
        }

        public static IDataReader GetsDistinctSession(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT DISTINCT(YEAR(DBServerDateTime)) AS EntrySession FROM OrderRecap");
        }

        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
     
        
        public static IDataReader Gets_byTechnicalSheet(TransactionContext tc, long nTechnicalSheetID, int nOrderTypeId)
        {
            return tc.ExecuteReader("SELECT * FROM View_OrderRecap WHERE TechnicalSheetID=%n AND OrderType =%n", nTechnicalSheetID, nOrderTypeId);
        }
              

        public static double GetProductionPlanQty(TransactionContext tc, int nOrderRecapID)
        {
            object oProductionPlanQty = tc.ExecuteScalar("SELECT ISNULL(SUM(ONSQty),0) AS ProductionPlanQty FROM View_OrderRecap WHERE OrderRecapID =%n", nOrderRecapID);
            double nProductionPlanQty = 0;
            if (DBNull.Value == oProductionPlanQty || oProductionPlanQty == null)
            {
                return 0;
            }
            else
            {
                nProductionPlanQty = Convert.ToDouble(oProductionPlanQty);
            }
            return nProductionPlanQty;
        }
        #endregion
    }
}
