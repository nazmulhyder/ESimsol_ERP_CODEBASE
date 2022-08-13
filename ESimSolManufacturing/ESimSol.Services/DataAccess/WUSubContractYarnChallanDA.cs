using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class WUSubContractYarnChallanDA
    {
        public WUSubContractYarnChallanDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, WUSubContractYarnChallan oWUSubContractYarnChallan, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_WUSubContractYarnChallan]"
                                    + "%n, %n, %s, %d, %s, %s, %s, %s, %n, %n, %n",
                                    oWUSubContractYarnChallan.WUSubContractYarnChallanID, oWUSubContractYarnChallan.WUSubContractID, oWUSubContractYarnChallan.ChallanNo, oWUSubContractYarnChallan.ChallanDate, oWUSubContractYarnChallan.TruckNo, oWUSubContractYarnChallan.DriverName, oWUSubContractYarnChallan.DeliveryPoint, oWUSubContractYarnChallan.Remarks, oWUSubContractYarnChallan.ApprovedBy, nUserID, (int)eEnumDBOperation);
        }

        public static void Delete(TransactionContext tc, WUSubContractYarnChallan oWUSubContractYarnChallan, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_WUSubContractYarnChallan]"
                                    + "%n, %n, %s, %d, %s, %s, %s, %s, %n, %n, %n",
                                    oWUSubContractYarnChallan.WUSubContractYarnChallanID, oWUSubContractYarnChallan.WUSubContractID, oWUSubContractYarnChallan.ChallanNo, oWUSubContractYarnChallan.ChallanDate, oWUSubContractYarnChallan.TruckNo, oWUSubContractYarnChallan.DriverName, oWUSubContractYarnChallan.DeliveryPoint, oWUSubContractYarnChallan.Remarks, oWUSubContractYarnChallan.ApprovedBy, nUserID, (int)eEnumDBOperation);
        }

        public static IDataReader Approve(TransactionContext tc, WUSubContractYarnChallan oWUSubContractYarnChallan, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_WUSubContractYarnChallan]"
                                    + "%n, %n, %s, %d, %s, %s, %s, %s, %n, %n, %n",
                                    oWUSubContractYarnChallan.WUSubContractYarnChallanID, oWUSubContractYarnChallan.WUSubContractID, oWUSubContractYarnChallan.ChallanNo, oWUSubContractYarnChallan.ChallanDate, oWUSubContractYarnChallan.TruckNo, oWUSubContractYarnChallan.DriverName, oWUSubContractYarnChallan.DeliveryPoint, oWUSubContractYarnChallan.Remarks, oWUSubContractYarnChallan.ApprovedBy, nUserID, (int)eEnumDBOperation);
        }

        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_WUSubContractYarnChallan WHERE WUSubContractYarnChallanID=%n", nID);
        }
        
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        #endregion
    }
}
