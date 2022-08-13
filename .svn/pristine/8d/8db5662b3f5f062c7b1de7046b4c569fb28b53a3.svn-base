using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class WUSubContractYarnChallanDetailDA
    {
        public WUSubContractYarnChallanDetailDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, WUSubContractYarnChallanDetail oWUSubContractYarnChallanDetail, EnumDBOperation eEnumDBOperation, Int64 nUserID, string sID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_WUSubContractYarnChallanDetail]"
                                    + "%n, %n, %n, %n, %n, %n, %n, %n, %n, %s, %n, %n, %n, %s",
                                    oWUSubContractYarnChallanDetail.WUSubContractYarnChallanDetailID, oWUSubContractYarnChallanDetail.WUSubContractYarnChallanID, oWUSubContractYarnChallanDetail.WUSubContractID, oWUSubContractYarnChallanDetail.WUSubContractYarnConsumptionID, oWUSubContractYarnChallanDetail.IssueStoreID, oWUSubContractYarnChallanDetail.YarnID, oWUSubContractYarnChallanDetail.LotID, oWUSubContractYarnChallanDetail.MUnitID, oWUSubContractYarnChallanDetail.Qty, oWUSubContractYarnChallanDetail.Remarks, oWUSubContractYarnChallanDetail.BagQty, nUserID, (int)eEnumDBOperation, sID);
        }

        public static void Delete(TransactionContext tc, WUSubContractYarnChallanDetail oWUSubContractYarnChallanDetail, EnumDBOperation eEnumDBOperation, Int64 nUserID, string sID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_WUSubContractYarnChallanDetail]"
                                    + "%n, %n, %n, %n, %n, %n, %n, %n, %n, %s, %n, %n, %n, %s",
                                    oWUSubContractYarnChallanDetail.WUSubContractYarnChallanDetailID, oWUSubContractYarnChallanDetail.WUSubContractYarnChallanID, oWUSubContractYarnChallanDetail.WUSubContractID, oWUSubContractYarnChallanDetail.WUSubContractYarnConsumptionID, oWUSubContractYarnChallanDetail.IssueStoreID, oWUSubContractYarnChallanDetail.YarnID, oWUSubContractYarnChallanDetail.LotID, oWUSubContractYarnChallanDetail.MUnitID, oWUSubContractYarnChallanDetail.Qty, oWUSubContractYarnChallanDetail.Remarks, oWUSubContractYarnChallanDetail.BagQty, nUserID, (int)eEnumDBOperation, sID);
        }

        #endregion

        #region Get & Exist Function
        
        public static IDataReader Gets(TransactionContext tc, int id)
        {
            return tc.ExecuteReader("SELECT * FROM View_WUSubContractYarnChallanDetail WHERE WUSubContractYarnChallanID=%n", id);
        }

        #endregion
    }
}
