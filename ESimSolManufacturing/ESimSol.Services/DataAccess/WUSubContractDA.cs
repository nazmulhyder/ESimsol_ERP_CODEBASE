using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class WUSubContractDA
    {
        public WUSubContractDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, WUSubContract oWUSubContract, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_WUSubContract]"
                                    + "%n, %n, %s, %d, %n, %n, %n, %n, %n, %n,		%n, %s, %n, %s, %n, %n, %n, %s, %s, %n, %s,		%n, %s, %n, %n, %n, %n, %n, %n, %n, %n, %n,		%n, %d, %s, %d, %s, %n, %n, %n",
                                    oWUSubContract.WUSubContractID, oWUSubContract.BUID, oWUSubContract.JobNo, oWUSubContract.ContractDate, oWUSubContract.SupplierID, oWUSubContract.ContractPersonID, oWUSubContract.ContractBy, oWUSubContract.ContractStatusInt, oWUSubContract.YarnChallanStatusInt, oWUSubContract.FabricRcvStatusInt, oWUSubContract.PaymentModeInt, oWUSubContract.SONo, oWUSubContract.BuyerID, oWUSubContract.StyleNo, oWUSubContract.OrderTypeInt, oWUSubContract.FabricTypeID, oWUSubContract.CompositionID, oWUSubContract.Construction, oWUSubContract.GrayWidth, oWUSubContract.GrayPick, oWUSubContract.ReedSpace, oWUSubContract.TotalEnds, oWUSubContract.ReedCount, oWUSubContract.WeaveDesignID, oWUSubContract.WSCWorkTypeInt, oWUSubContract.OrderQty, oWUSubContract.MUnitID, oWUSubContract.Rate, oWUSubContract.RatePerPick, oWUSubContract.TotalAmount, oWUSubContract.CRate, oWUSubContract.CurrencyID, oWUSubContract.TransportationInt, oWUSubContract.ProdStartDate, oWUSubContract.ProdStartComments, oWUSubContract.ProdCompleteDate, oWUSubContract.Remarks, oWUSubContract.ApprovedBy, nUserID, (int)eEnumDBOperation);
        }

        public static void Delete(TransactionContext tc, WUSubContract oWUSubContract, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_WUSubContract]"
                                    + "%n, %n, %s, %d, %n, %n, %n, %n, %n, %n,		%n, %s, %n, %s, %n, %n, %n, %s, %s, %n, %s,		%n, %s, %n, %n, %n, %n, %n, %n, %n, %n, %n,		%n, %d, %s, %d, %s, %n, %n, %n",
                                    oWUSubContract.WUSubContractID, oWUSubContract.BUID, oWUSubContract.JobNo, oWUSubContract.ContractDate, oWUSubContract.SupplierID, oWUSubContract.ContractPersonID, oWUSubContract.ContractBy, oWUSubContract.ContractStatusInt, oWUSubContract.YarnChallanStatusInt, oWUSubContract.FabricRcvStatusInt, oWUSubContract.PaymentModeInt, oWUSubContract.SONo, oWUSubContract.BuyerID, oWUSubContract.StyleNo, oWUSubContract.OrderTypeInt, oWUSubContract.FabricTypeID, oWUSubContract.CompositionID, oWUSubContract.Construction, oWUSubContract.GrayWidth, oWUSubContract.GrayPick, oWUSubContract.ReedSpace, oWUSubContract.TotalEnds, oWUSubContract.ReedCount, oWUSubContract.WeaveDesignID, oWUSubContract.WSCWorkTypeInt, oWUSubContract.OrderQty, oWUSubContract.MUnitID, oWUSubContract.Rate, oWUSubContract.RatePerPick, oWUSubContract.TotalAmount, oWUSubContract.CRate, oWUSubContract.CurrencyID, oWUSubContract.TransportationInt, oWUSubContract.ProdStartDate, oWUSubContract.ProdStartComments, oWUSubContract.ProdCompleteDate, oWUSubContract.Remarks, oWUSubContract.ApprovedBy, nUserID, (int)eEnumDBOperation);
        }

        public static IDataReader Approve(TransactionContext tc, WUSubContract oWUSubContract, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_WUSubContract]"
                                    + "%n, %n, %s, %d, %n, %n, %n, %n, %n, %n,		%n, %s, %n, %s, %n, %n, %n, %s, %s, %n, %s,		%n, %s, %n, %n, %n, %n, %n, %n, %n, %n, %n,		%n, %d, %s, %d, %s, %n, %n, %n",
                                    oWUSubContract.WUSubContractID, oWUSubContract.BUID, oWUSubContract.JobNo, oWUSubContract.ContractDate, oWUSubContract.SupplierID, oWUSubContract.ContractPersonID, oWUSubContract.ContractBy, oWUSubContract.ContractStatusInt, oWUSubContract.YarnChallanStatusInt, oWUSubContract.FabricRcvStatusInt, oWUSubContract.PaymentModeInt, oWUSubContract.SONo, oWUSubContract.BuyerID, oWUSubContract.StyleNo, oWUSubContract.OrderTypeInt, oWUSubContract.FabricTypeID, oWUSubContract.CompositionID, oWUSubContract.Construction, oWUSubContract.GrayWidth, oWUSubContract.GrayPick, oWUSubContract.ReedSpace, oWUSubContract.TotalEnds, oWUSubContract.ReedCount, oWUSubContract.WeaveDesignID, oWUSubContract.WSCWorkTypeInt, oWUSubContract.OrderQty, oWUSubContract.MUnitID, oWUSubContract.Rate, oWUSubContract.RatePerPick, oWUSubContract.TotalAmount, oWUSubContract.CRate, oWUSubContract.CurrencyID, oWUSubContract.TransportationInt, oWUSubContract.ProdStartDate, oWUSubContract.ProdStartComments, oWUSubContract.ProdCompleteDate, oWUSubContract.Remarks, oWUSubContract.ApprovedBy, nUserID, (int)eEnumDBOperation);
        }

        public static void FinishYarnChallan(TransactionContext tc, WUSubContract oWUSubContract, Int64 nUserID)
        {
            tc.ExecuteNonQuery("UPDATE WUSubContract SET ApprovedBy = %n, YarnChallanStatus = 3 WHERE WUSubContractID = %n", nUserID, oWUSubContract.WUSubContractID);     
        }

        public static IDataReader AcceptRevise(TransactionContext tc, WUSubContract oWUSubContract, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_AcceptReviseWUSubContract]"
                                    + "%n, %n, %s, %d, %n, %n, %n, %n, %n, %n,		%n, %s, %n, %s, %n, %n, %n, %s, %s, %n, %s,		%n, %s, %n, %n, %n, %n, %n, %n, %n, %n, %n,		%n, %d, %s, %d, %s, %n, %n",
                                    oWUSubContract.WUSubContractID, oWUSubContract.BUID, oWUSubContract.JobNo, oWUSubContract.ContractDate, oWUSubContract.SupplierID, oWUSubContract.ContractPersonID, oWUSubContract.ContractBy, oWUSubContract.ContractStatusInt, oWUSubContract.YarnChallanStatusInt, oWUSubContract.FabricRcvStatusInt, oWUSubContract.PaymentModeInt, oWUSubContract.SONo, oWUSubContract.BuyerID, oWUSubContract.StyleNo, oWUSubContract.OrderTypeInt, oWUSubContract.FabricTypeID, oWUSubContract.CompositionID, oWUSubContract.Construction, oWUSubContract.GrayWidth, oWUSubContract.GrayPick, oWUSubContract.ReedSpace, oWUSubContract.TotalEnds, oWUSubContract.ReedCount, oWUSubContract.WeaveDesignID, oWUSubContract.WSCWorkTypeInt, oWUSubContract.OrderQty, oWUSubContract.MUnitID, oWUSubContract.Rate, oWUSubContract.RatePerPick, oWUSubContract.TotalAmount, oWUSubContract.CRate, oWUSubContract.CurrencyID, oWUSubContract.TransportationInt, oWUSubContract.ProdStartDate, oWUSubContract.ProdStartComments, oWUSubContract.ProdCompleteDate, oWUSubContract.Remarks, oWUSubContract.ApprovedBy, nUserID);
        }
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_WUSubContract WHERE WUSubContractID=%n", nID);
        }
        public static IDataReader GetRevise(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_WUSubContractLog WHERE WUSubContractLogID=%n", nID);
        }
        public static IDataReader Gets(TransactionContext tc, int buid)
        {
            return tc.ExecuteReader("SELECT * FROM View_WUSubContract WHERE BUID=%n AND ApprovedBy=0 Order By WUSubContractID", buid);
        }
        public static IDataReader Get(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }

        public static IDataReader GetsPrint(TransactionContext tc, string sSQL)
        {
            //return tc.ExecuteReader(sSQL);
            return tc.ExecuteReader("EXEC [SP_RPT_WUSubContract]" + "%s", sSQL);
        }
        #endregion
    }  
}
