using System;
using System.Data;
using System.Data.SqlClient;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.DataAccess;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class FabricSalesContractDA
    {
        public FabricSalesContractDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, FabricSalesContract oFSC, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_FabricSalesContract]" + "%n, %s, %D, %n, %n, %n , %n, %n, %n, %n, %n,%n, %n, %n,  %b, %s, %s, %s, %s, %s,  %n,%b,%b,  %n, %n, %n",
                                    oFSC.FabricSalesContractID, oFSC.SCNo, oFSC.SCDate, oFSC.OrderType, oFSC.ContractorID, oFSC.BuyerID, oFSC.ContactPersonnelID, oFSC.MktAccountID, oFSC.CurrencyID, oFSC.LightSourceID, oFSC.LCTermID, oFSC.PaymentInstruction, oFSC.PaymentTypeInt, oFSC.CurrentStatusInt, oFSC.IsInHouse, oFSC.EndUse, oFSC.GarmentWash, oFSC.QualityParameters, oFSC.Emirzing, oFSC.Note, oFSC.BUID, oFSC.IsPrint, oFSC.IsOpenPI, oFSC.MktGroupID, nUserID, (int)eEnumDBOperation);
        }
        public static IDataReader InsertUpdateLog(TransactionContext tc, FabricSalesContract oFSC, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_FabricSalesContractLog]" + "%n, %s, %D, %n, %n, %n , %n, %n, %n, %n, %n, %n, %n, %n,  %b, %s, %s, %s, %s, %s, %b, %n,%b,%b,  %n, %n, %n",
                                    oFSC.FabricSalesContractID, oFSC.SCNo, oFSC.SCDate, oFSC.OrderType, oFSC.ContractorID, oFSC.BuyerID, oFSC.ContactPersonnelID, oFSC.MktAccountID, oFSC.CurrencyID, oFSC.LightSourceID, oFSC.LCTermID, oFSC.PaymentInstruction, oFSC.PaymentTypeInt, oFSC.CurrentStatusInt, oFSC.IsInHouse, oFSC.EndUse, oFSC.GarmentWash, oFSC.QualityParameters, oFSC.Emirzing, oFSC.Note, oFSC.IsRevise, oFSC.BUID, oFSC.IsPrint, oFSC.IsOpenPI, oFSC.MktGroupID, nUserID, (int)eEnumDBOperation);
        }
        public static void Delete(TransactionContext tc, FabricSalesContract oFSC, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_FabricSalesContract]" + "%n, %s, %D, %n, %n, %n , %n, %n, %n, %n, %n, %n, %n, %n,  %b, %s, %s, %s, %s, %s,  %n,%b,%b,  %n, %n, %n",
                                    oFSC.FabricSalesContractID, oFSC.SCNo, oFSC.SCDate, oFSC.OrderType, oFSC.ContractorID, oFSC.BuyerID, oFSC.ContactPersonnelID, oFSC.MktAccountID, oFSC.CurrencyID, oFSC.LightSourceID, oFSC.LCTermID, oFSC.PaymentInstruction, oFSC.PaymentTypeInt, oFSC.CurrentStatusInt, oFSC.IsInHouse, oFSC.EndUse, oFSC.GarmentWash, oFSC.QualityParameters, oFSC.Emirzing, oFSC.Note, oFSC.BUID, oFSC.IsPrint, oFSC.IsOpenPI, oFSC.MktGroupID, nUserID, (int)eEnumDBOperation);
        }
        public static IDataReader InsertUpdateInvoice(TransactionContext tc, FabricSalesContract oFSC, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_SampleInvoice_FabricSC]" + "%n, %s, %n, %n",
                                    oFSC.FabricSalesContractID, (int)oFSC.PaymentType,nUserID, (int)eEnumDBOperation);
        }
      
        public static IDataReader Copy(TransactionContext tc, FabricSalesContract oFabricSalesContract, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_CopyFabricSalesContract]" + "%n, %n", oFabricSalesContract.FabricSalesContractID, nUserID);
        }
        public static void UpdateInfo(TransactionContext tc, FabricSalesContract oFSC)
        {
            tc.ExecuteNonQuery("Update FabricSalesContract SET EndUse = %s, LightSourceID = %n,LightSourceIDTwo=%n, GarmentWash = %s, QualityParameters = %s, Emirzing = %s, QtyTolarance= %s,LCTermID=%n,PaymentInstruction=%n  WHERE FabricSalesContractID = %n", oFSC.EndUse, oFSC.LightSourceID,oFSC.LightSourceIDTwo, oFSC.GarmentWash, oFSC.QualityParameters, oFSC.Emirzing, oFSC.QtyTolarance, oFSC.LCTermID, oFSC.PaymentInstruction, oFSC.FabricSalesContractID);
        }

        public static void UpdateBySQL(TransactionContext tc, string sSQL)
        {
            tc.ExecuteNonQuery(sSQL);
        }
        public static IDataReader ReceiveFabric(TransactionContext tc, int nFSCID, DateTime dtReceive, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_Process_FabricSalesContract]" + "%n, %d, %n", nFSCID, dtReceive, nUserID);
        }
        
        #endregion

    
        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_FabricSalesContract WHERE FabricSalesContractID=%n", nID);
        }
        public static IDataReader GetByLog(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_FabricSalesContractLog WHERE FabricSalesContractLogID=%n", nID);
        }
      
        public static IDataReader Get(TransactionContext tc, string sPINo, int nTexTileUnit)
        {
            int nCount = 0;
            if (sPINo.Length > 15) { 
                nCount = 15;
                return tc.ExecuteReader("SELECT * FROM View_FabricSalesContract WHERE LEFT(PINo," + nCount + ") LIKE '%" + sPINo.Split('/')[0] + "/" + sPINo.Split('/')[1] + "/" + sPINo.Split('/')[2] + "/" + sPINo.Split('/')[3] + "%' AND RIGHT(PINo,3)='/" + sPINo.Split('/')[4] + "' AND TextileUnit=" + nTexTileUnit);

            }
            else { 
                nCount = 13;
                return tc.ExecuteReader("SELECT * FROM View_FabricSalesContract WHERE LEFT(PINo," + nCount + ") LIKE '%" + sPINo.Split('/')[0] + "/" + sPINo.Split('/')[1] + "/" + sPINo.Split('/')[2] + "%' AND RIGHT(PINo,3)='/" + sPINo.Split('/')[3] + "' AND TextileUnit=" + nTexTileUnit);
            }

        }

        public static IDataReader GetsYetToApproveByMktGroup(TransactionContext tc, Int64 nDBUserID)
        {
            return tc.ExecuteReader("SELECT * FROM View_FabricSalesContract as FSC where Isnull(ApproveBy,0)=0 and FSC.MktGroupID in ( Select MarketingAccount.MarketingAccountID from MarketingAccount where GroupID>0 and GroupID in (Select GroupID from MarketingAccount where UserID =%n) or UserID =%n) order by SCDate DESC", nDBUserID, nDBUserID);
        }
        public static IDataReader GetsYetToApproveAll(TransactionContext tc, Int64 nDBUserID)
        {
            return tc.ExecuteReader("SELECT * FROM View_FabricSalesContract as FSC where Isnull(ApproveBy,0)=0 order by SCDate DESC", nDBUserID, nDBUserID);
        }
        public static IDataReader GetsByPI(TransactionContext tc, int nPIID)
        {
            return tc.ExecuteReader("SELECT * FROM View_FabricSalesContract as FSC where FabricSalesContractID in (Select FabricSalesContractID from FabricSalesContractDetail where FabricSalesContractDetailID in (Select ExportPIDetail.OrderSheetDetailID from ExportPIDetail where exportPIID=%n))", nPIID);
        }

  

        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        public static IDataReader GetsReport(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader("EXEC [SP_FNExecutionOrderStatus] %s ", sSQL);
        }
        public static IDataReader GetLogID(TransactionContext tc, int nLogID)
        {
            return tc.ExecuteReader("SELECT * FROM [View_FabricSalesContractLog] WHERE LogOf=%n", nLogID);
        }
        public static IDataReader GetsLog(TransactionContext tc, int nFabricSalesContractID)
        {
            return tc.ExecuteReader("SELECT * FROM View_FabricSalesContractLog WHERE FabricSalesContractID=%n", nFabricSalesContractID);
        }

        public static IDataReader Gets(TransactionContext tc, int nContractorID, string sLCIDs)
        {
            return tc.ExecuteReader("SELECT * FROM View_FabricSalesContract WHERE ContractorID=%n AND ApprovedStatus in(0,1,2) AND LCID IN(%q)", nContractorID, sLCIDs);
        }

        public static void UpdateReviseNo(TransactionContext tc, int id, int ReviseNo, Int64 nUserId)
        {
            tc.ExecuteNonQuery("Update FabricSalesContract SET ReviseNo =%n  WHERE FabricSalesContractID = %n", ReviseNo,id);
        }

        #endregion
    }
}
