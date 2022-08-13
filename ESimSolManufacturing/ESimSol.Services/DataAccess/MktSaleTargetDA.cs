using System;
using ICS.Core.Framework;
using ICS.Core.Utility;
using ICS.Core;
using ESimSol.BusinessObjects;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.OleDb;
using ICS.Core.DataAccess;


namespace ESimSol.Services.DataAccess
{
    public class MktSaleTargetDA
    {
        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, MktSaleTarget oMktSaleTarget, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_MktSaleTarget]"
                                   + "%n,%n,%n,%d,%n,%n,%s,%s,%n,%n,%n,%n,%n,%s,%n,%n",
                                   oMktSaleTarget.MktSaleTargetID,
                                   oMktSaleTarget.MarketingAccountID,
                                   (int)oMktSaleTarget.OrderType,
                                   oMktSaleTarget.Date,
                                   oMktSaleTarget.Value,
                                   oMktSaleTarget.ContractorID,
                                   oMktSaleTarget.BuyerPosition,
                                   oMktSaleTarget.BPercent,
                                   oMktSaleTarget.BuyerPercentID,
                                   oMktSaleTarget.OrderQty,
                                   oMktSaleTarget.ProductID,
                                   oMktSaleTarget.WeaveType,
                                   oMktSaleTarget.FinishType,
                                   oMktSaleTarget.Construction,
                                   nUserID,
                                   (int)eEnumDBOperation);
        }

        public static void Delete(TransactionContext tc, MktSaleTarget oMktSaleTarget, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_MktSaleTarget]"
                                   + "%n,%n,%n,%d,%n,%n,%s,%s,%n,%n,%n,%n,%n,%s,%n,%n",
                                   oMktSaleTarget.MktSaleTargetID,
                                   oMktSaleTarget.MarketingAccountID,
                                   (int)oMktSaleTarget.OrderType,
                                   oMktSaleTarget.Date,
                                   oMktSaleTarget.Value,
                                   oMktSaleTarget.ContractorID,
                                   oMktSaleTarget.BuyerPosition,
                                   oMktSaleTarget.BPercent,
                                   oMktSaleTarget.BuyerPercentID,
                                   oMktSaleTarget.OrderQty,
                                   oMktSaleTarget.ProductID,
                                   oMktSaleTarget.WeaveType,
                                   oMktSaleTarget.FinishType,
                                   oMktSaleTarget.Construction,
                                   nUserID,
                                   (int)eEnumDBOperation);
        }

        #endregion
        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_MktSaleTarget WHERE MktSaleTargetID=%n", nID);
        }
        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_MktSaleTarget");
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        public static void UpdateApproval(TransactionContext tc, MktSaleTarget oMktSaleTarget, Int64 nUserID)
        {
            tc.ExecuteNonQuery("Update MktSaleTarget SET ApproveBy = %n,ApproveDate = %d WHERE MktSaleTargetID = %n", nUserID, DateTime.Now.ToString("dd MMM yyyy"), oMktSaleTarget.MktSaleTargetID);
        }

        #endregion
    }
}
