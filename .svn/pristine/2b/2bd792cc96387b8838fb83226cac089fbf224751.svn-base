using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ESimSol.BusinessObjects;
using ICS.Core.Utility;
using ICS.Core.DataAccess;
using System.Data;

namespace ESimSol.Services.DataAccess
{
    public class DUClaimOrderDA
    {
        public DUClaimOrderDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, DUClaimOrder oDUCO, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_DUClaimOrder]" + "%n,%n,%n,%s, %d, %n, %n,  %s, %s,%s,%n,%n,%n,%n",
                                    oDUCO.DUClaimOrderID, oDUCO.DyeingOrderID, oDUCO.ParentDOID, oDUCO.ClaimOrderNo, oDUCO.OrderDate, oDUCO.ClaimType, oDUCO.OrderType,   oDUCO.Note, oDUCO.Note_Checked, oDUCO.Note_Approve,oDUCO.PaymentType,oDUCO.DUReturnChallanID, nUserID, (int)eEnumDBOperation);
        }
        public static void Delete(TransactionContext tc, DUClaimOrder oDUCO, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_DUClaimOrder]" + "%n,%n,%n,%s, %D, %n, %n, %s, %s,%s,%n,%n,%n,%n",
                                           oDUCO.DUClaimOrderID, oDUCO.DyeingOrderID, oDUCO.ParentDOID, oDUCO.ClaimOrderNo, oDUCO.OrderDate, oDUCO.ClaimType, oDUCO.OrderType, oDUCO.Note, oDUCO.Note_Checked, oDUCO.Note_Approve, oDUCO.PaymentType, oDUCO.DUReturnChallanID,nUserID, (int)eEnumDBOperation);
        }

        public static IDataReader InsertUpdate_Log(TransactionContext tc, DUClaimOrder oDUCO, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_DUClaimOrderLog]" + "%n,%n,%n,%s, %d, %n, %n,  %s, %s,%s,%b,%n,%n,%n",
                oDUCO.DUClaimOrderID, oDUCO.DyeingOrderID, oDUCO.ParentDOID, oDUCO.ClaimOrderNo, oDUCO.OrderDate, oDUCO.ClaimType, oDUCO.OrderType,   oDUCO.Note, oDUCO.Note_Checked, oDUCO.Note_Approve,oDUCO.IsRevise,oDUCO.PaymentType, nUserID, (int)eEnumDBOperation);
        }
        #endregion


        #region


        #endregion

        #region Get & Exist Function

        public static IDataReader Get(int nID, TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_DUClaimOrder WHERE DUClaimOrderID=%n", nID);
        }

        public static IDataReader GetsBy(TransactionContext tc, string sContractorID)
        {
            return tc.ExecuteReader("Select * from View_DUClaimOrder where ContractorID in (%q) ", sContractorID);
        }
        public static IDataReader GetsByPI(TransactionContext tc, int nExportPIID)
        {
            return tc.ExecuteReader("Select * from View_DUClaimOrder where ExportPIID=%n", nExportPIID);
        }
        public static IDataReader Gets(string sSQL, TransactionContext tc)
        {
            return tc.ExecuteReader(sSQL);
        }
        public static IDataReader InsertUpdate_DeliveryOrder(TransactionContext tc, DUClaimOrder oDUClaimOrder, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_DUDeliveryOrder_Auto]" + "%n,%s, %D, %n, %n, %n, %n, %n, %n, %n,%n,%n,%n,%n",
                                    oDUClaimOrder.DUClaimOrderID, oDUClaimOrder.ClaimOrderNo, oDUClaimOrder.OrderDate, oDUClaimOrder.ContractorID, 0, oDUClaimOrder.ContractorID, 0, oDUClaimOrder.ExportPIID, 0, 0, (int)EnumOrderType.ClaimOrder, oDUClaimOrder.DUClaimOrderID, nUserID, (int)eEnumDBOperation);
        }

        #endregion
    }
}
