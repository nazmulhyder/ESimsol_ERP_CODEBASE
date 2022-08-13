using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class TwistingDA
    {
        public TwistingDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, Twisting oTwisting, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_Twisting]"
                                    + "%n,%d, %n,%n,%n,%n, %s,%n ,%s,  %n,%n,   %n,%n",
                                    oTwisting.TwistingID, oTwisting.ReceiveDate, 
                                    oTwisting.DyeingOrderID, oTwisting.ProductID_TW,
                                    oTwisting.Qty, oTwisting.WorkingUnitID,
                                    oTwisting.Note, oTwisting.StatusInt, oTwisting.MachineDes,
                                    oTwisting.TwistingOrderType, oTwisting.CompleteWorkingUnitID,
                                    nUserID,(int)eEnumDBOperation);
        }
        public static void Delete(TransactionContext tc, Twisting oTwisting, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_Twisting]"
                                    + "%n,%d, %n,%n,%n,%n, %s,%n ,%s,  %n,%n,  %n,%n",
                                    oTwisting.TwistingID, oTwisting.ReceiveDate,
                                    oTwisting.DyeingOrderID, oTwisting.ProductID_TW,
                                    oTwisting.Qty, oTwisting.WorkingUnitID,
                                    oTwisting.Note, oTwisting.StatusInt, oTwisting.MachineDes,
                                    oTwisting.TwistingOrderType, oTwisting.CompleteWorkingUnitID,
                                    nUserID, (int)eEnumDBOperation);
        }

        //public static IDataReader Approve(TransactionContext tc, Twisting oTwisting, EnumDBOperation eENumDBPurchaseInvoice, Int64 nUserId)
        //{
        //    return tc.ExecuteReader("EXEC [SP_IUD_Twisting]"
        //                             + "%n,%d, %n,%n,%n,%n, %s,%n ,%s,%n,%n",
        //                            oTwisting.TwistingID, oTwisting.ReceiveDate,
        //                            oTwisting.DyeingOrderID, oTwisting.ProductID_TW,
        //                            oTwisting.Qty, oTwisting.WorkingUnitID,
        //                            oTwisting.Note, oTwisting.StatusInt, oTwisting.MachineDes,
        //                            nUserId, (int)eENumDBPurchaseInvoice);
        //}
        //public static IDataReader UndoApprove(TransactionContext tc, Twisting oTwisting, EnumDBOperation eENumDBPurchaseInvoice, Int64 nUserId)
        //{
        //    return tc.ExecuteReader("EXEC [SP_IUD_Twisting]"
        //                             + "%n,%d, %n,%n,%n,%n, %s,%n ,%s,%n,%n",
        //                            oTwisting.TwistingID, oTwisting.ReceiveDate,
        //                            oTwisting.DyeingOrderID, oTwisting.ProductID_TW,
        //                            oTwisting.Qty, oTwisting.WorkingUnitID,
        //                            oTwisting.Note, oTwisting.StatusInt, oTwisting.MachineDes,
        //                            nUserId, (int)eENumDBPurchaseInvoice);
        //}
        //public static IDataReader Complete(TransactionContext tc, Twisting oTwisting, EnumDBOperation eENumDBPurchaseInvoice, Int64 nUserId)
        //{
        //    return tc.ExecuteReader("EXEC [SP_IUD_Twisting]"
        //                             + "%n,%d, %n,%n,%n,%n, %s,%n ,%s,%n,%n",
        //                            oTwisting.TwistingID, oTwisting.ReceiveDate,
        //                            oTwisting.DyeingOrderID, oTwisting.ProductID_TW,
        //                            oTwisting.Qty, oTwisting.WorkingUnitID,
        //                            oTwisting.Note, oTwisting.StatusInt, oTwisting.MachineDes,
        //                            nUserId, (int)eENumDBPurchaseInvoice);
        //}
      
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_Twisting WHERE TwistingID=%n", nID);
        }
        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_Twisting where isnull(CompletedByID,0)=0");
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        #endregion
    }
}
