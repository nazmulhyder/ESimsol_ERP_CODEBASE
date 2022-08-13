using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class DUSoftWindingDA
    {
        public DUSoftWindingDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, DUSoftWinding oDUSoftWinding, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_DUSoftWinding]"
                                    + "%n,%d ,%n,%n,%n ,%n,%n,%n,%n ,%s,%n,%d,%d ,%n,%s,%n,%n,%n,%n,%n,%n",
                                    oDUSoftWinding.DUSoftWindingID, oDUSoftWinding.ReceiveDate, 
                                    oDUSoftWinding.DyeingOrderID, oDUSoftWinding.DURequisitionID, oDUSoftWinding.ProductID, 
                                    oDUSoftWinding.LotID, oDUSoftWinding.Qty, oDUSoftWinding.MUnitID, oDUSoftWinding.BagNo, 
                                    oDUSoftWinding.Note, oDUSoftWinding.StatusInt, oDUSoftWinding.StartDate, oDUSoftWinding.EndDate,
                                    oDUSoftWinding.MachineID, oDUSoftWinding.Operator, oDUSoftWinding.NumOfCone, oDUSoftWinding.@RSShiftID,oDUSoftWinding.GainLossTypeInt,oDUSoftWinding.QtyGainLoss,
                                    nUserID,(int)eEnumDBOperation);
        }
        public static void Delete(TransactionContext tc, DUSoftWinding oDUSoftWinding, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_DUSoftWinding]"
                                     + "%n,%d ,%n,%n,%n ,%n,%n,%n,%n ,%s,%n,%d,%d ,%n,%s,%n,%n,%n,%n,%n,%n",
                                    oDUSoftWinding.DUSoftWindingID, oDUSoftWinding.ReceiveDate,
                                    oDUSoftWinding.DyeingOrderID, oDUSoftWinding.DURequisitionID, oDUSoftWinding.ProductID,
                                    oDUSoftWinding.LotID, oDUSoftWinding.Qty, oDUSoftWinding.MUnitID, oDUSoftWinding.BagNo,
                                    oDUSoftWinding.Note, oDUSoftWinding.StatusInt, oDUSoftWinding.StartDate, oDUSoftWinding.EndDate,
                                    oDUSoftWinding.MachineID, oDUSoftWinding.Operator, oDUSoftWinding.NumOfCone, oDUSoftWinding.@RSShiftID, oDUSoftWinding.GainLossTypeInt, oDUSoftWinding.QtyGainLoss,
                                    nUserID, (int)eEnumDBOperation);
        }

        public static void UpdateRSLot(TransactionContext tc,int nDUSoftWindingID, Int64 nUserId)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_RSRawLot_Soft]" + "%n,%n", nDUSoftWindingID, nUserId);
        }
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_DUSoftWinding WHERE DUSoftWindingID=%n", nID);
        }
        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_DUSoftWinding");
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        public static IDataReader Gets_Report(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader("EXEC [SP_RPT_DUSoftWinding] %s, %n",sSQL,0);
        }
        #endregion
    }
}
