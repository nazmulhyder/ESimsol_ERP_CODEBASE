using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class DUHardWindingDA
    {
        public DUHardWindingDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, DUHardWinding oDUHardWinding, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_DUHardWinding]"
                                    + "%n,%d ,%n,%n,%n ,%n,%n,%n,%n ,%s,%n,%d,%d ,%s,%s, %n,%n,%n,%n,%n, %b,%n,  %n,%n",
                                    oDUHardWinding.DUHardWindingID, oDUHardWinding.ReceiveDate, 
                                    oDUHardWinding.DyeingOrderID, oDUHardWinding.DURequisitionID, oDUHardWinding.ProductID, 
                                    oDUHardWinding.LotID, oDUHardWinding.Qty, oDUHardWinding.MUnitID, oDUHardWinding.BagNo, 
                                    oDUHardWinding.Note, oDUHardWinding.StatusInt, oDUHardWinding.StartDate, oDUHardWinding.EndDate,
                                    oDUHardWinding.MachineDes, oDUHardWinding.Operator, oDUHardWinding.NumOfCone, oDUHardWinding.MachineID,
                                    oDUHardWinding.RSShiftID, oDUHardWinding.Dia, oDUHardWinding.WarpWeftTypeInt, oDUHardWinding.IsRewinded, oDUHardWinding.RouteSheetID,
                                    nUserID,(int)eEnumDBOperation);
        }
        public static void Delete(TransactionContext tc, DUHardWinding oDUHardWinding, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_DUHardWinding]"
                                    + "%n,%d ,%n,%n,%n ,%n,%n,%n,%n ,%s,%n,%d,%d ,%s,%s, %n,%n,%n,%n,%n, %b,%n,  %n,%n",
                                    oDUHardWinding.DUHardWindingID, oDUHardWinding.ReceiveDate,
                                    oDUHardWinding.DyeingOrderID, oDUHardWinding.DURequisitionID, oDUHardWinding.ProductID,
                                    oDUHardWinding.LotID, oDUHardWinding.Qty, oDUHardWinding.MUnitID, oDUHardWinding.BagNo,
                                    oDUHardWinding.Note, oDUHardWinding.StatusInt, oDUHardWinding.StartDate, oDUHardWinding.EndDate,
                                    oDUHardWinding.MachineDes, oDUHardWinding.Operator, oDUHardWinding.NumOfCone, oDUHardWinding.MachineID,
                                    oDUHardWinding.RSShiftID, oDUHardWinding.Dia, oDUHardWinding.WarpWeftTypeInt, oDUHardWinding.IsRewinded, oDUHardWinding.RouteSheetID,
                                    nUserID, (int)eEnumDBOperation);
        }
        public static IDataReader SendToHWStore(TransactionContext tc, DUHardWinding oDUHardWinding, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_RouteSheet_InHW]" + " %n,%n, %s, %n,%n, %n",
                                     oDUHardWinding.RouteSheetID, oDUHardWinding.Qty, oDUHardWinding.Note, (int)oDUHardWinding.RSState, (int)oDUHardWinding.DyeingOrderDetailID, nUserID);
        }
        public static IDataReader QCDOne(TransactionContext tc, DUHardWinding oDUHardWinding, long nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_RouteSheet_QC_HW]" + "%n, %n,%b,%s,%n", oDUHardWinding.DUHardWindingID, oDUHardWinding.DyeingOrderDetailID, oDUHardWinding.IsQCDone, oDUHardWinding.Note, nUserID);
        }
        public static IDataReader SendToDelivery(TransactionContext tc, DUHardWinding oDUHardWinding, long nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_TransferRequisitionSlip_DeliveryStoreAutoRedHW]" + "%n,%n,%s, %n", oDUHardWinding.DUHardWindingID,oDUHardWinding.Qty, oDUHardWinding.Note, nUserID);
        }
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_DUHardWinding WHERE DUHardWindingID=%n", nID);
        }
        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_DUHardWinding");
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        public static IDataReader Receive(TransactionContext tc, DUHardWinding oDUHardWinding, long UserID)
        {
            string sSQL1 = SQLParser.MakeSQL("UPDATE DUHardWinding SET ReceiveDate= GETDATE() WHERE DUHardWindingID=%n", oDUHardWinding.DUHardWindingID);
            tc.ExecuteNonQuery(sSQL1);
            return tc.ExecuteReader("SELECT * FROM View_DUHardWinding WHERE DUHardWindingID=%n", oDUHardWinding.DUHardWindingID);
        }

        public static IDataReader DODAssign(TransactionContext tc, DUHardWinding oDUHardWinding, long UserID)
        {
            string sSQL1 = SQLParser.MakeSQL("UPDATE DUHardWinding SET DyeingOrderDetailID=%n where isnull(RouteSheetID,0)=0 and DUHardWindingID=%n", oDUHardWinding.DyeingOrderDetailID, oDUHardWinding.DUHardWindingID);
            tc.ExecuteNonQuery(sSQL1);
            return tc.ExecuteReader("SELECT * FROM View_DUHardWinding WHERE DUHardWindingID=%n", oDUHardWinding.DUHardWindingID);
        }

        public static IDataReader UpdateReceivedate(TransactionContext tc, DUHardWinding oDUHardWinding, long UserID)
        {
            string sSQL1 = SQLParser.MakeSQL("UPDATE DUHardWinding SET DUHardWinding.ReceiveDate=%D where DUHardWindingID=%n", oDUHardWinding.ReceiveDate, oDUHardWinding.DUHardWindingID);
            tc.ExecuteNonQuery(sSQL1);
            return tc.ExecuteReader("SELECT * FROM View_DUHardWinding WHERE DUHardWindingID=%n", oDUHardWinding.DUHardWindingID);
        }
        #endregion
    }
}
