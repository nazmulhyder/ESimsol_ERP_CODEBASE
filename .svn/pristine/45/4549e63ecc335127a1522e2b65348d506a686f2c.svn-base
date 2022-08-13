using System;
using System.Data;
using System.Data.SqlClient;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.DataAccess;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class DUProGuideLineDA
    {
        public DUProGuideLineDA() { }

        #region Insert, Update, Delete

        public static IDataReader InsertUpdate(TransactionContext tc, DUProGuideLine oDUProGuideLine, EnumDBOperation eENumDBOperation, Int64 nUserId)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_DUProGuideLine]"
                                    + "%n,%n,%n,%n,%n,%n,%d,%n,	%s,%s,%s,%s, %n,%n,%n",
                                    oDUProGuideLine.DUProGuideLineID,
                                    oDUProGuideLine.BUID,
                                    oDUProGuideLine.OrderTypeInt,
                                    oDUProGuideLine.DyeingOrderID,
                                    oDUProGuideLine.WorkingUnitID,
                                    oDUProGuideLine.ContractorID,
                                    oDUProGuideLine.IssueDate,
                                    oDUProGuideLine.ProductTypeInt,
                                    oDUProGuideLine.Note,
                                    oDUProGuideLine.ChallanNo,
                                    oDUProGuideLine.VehicleNo,
                                    oDUProGuideLine.GateInNo,
                                    oDUProGuideLine.InOutTypeInt,
                                    nUserId,
                                    (int)eENumDBOperation);
        }
        public static void Delete(TransactionContext tc, DUProGuideLine oDUProGuideLine, EnumDBOperation eENumDBOperation, Int64 nUserId)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_DUProGuideLine]"
                                    + "%n,%n,%n,%n,%n,%n,%d,%n,	%s,%s,%s,%s, %n,%n,%n",
                                    oDUProGuideLine.DUProGuideLineID,
                                    oDUProGuideLine.BUID,
                                    oDUProGuideLine.OrderTypeInt,
                                    oDUProGuideLine.DyeingOrderID,
                                    oDUProGuideLine.WorkingUnitID,
                                    oDUProGuideLine.ContractorID,
                                    oDUProGuideLine.IssueDate,
                                    oDUProGuideLine.ProductTypeInt,
                                    oDUProGuideLine.Note,
                                    oDUProGuideLine.ChallanNo,
                                    oDUProGuideLine.VehicleNo,
                                    oDUProGuideLine.GateInNo,
                                    oDUProGuideLine.InOutTypeInt,
                                    nUserId,
                                    (int)eENumDBOperation);
        }
        #endregion

        #region Get & Exist Function

        public static IDataReader Get(int nDUProGuideLineID, TransactionContext tc)
        {
            return tc.ExecuteReader("select * from View_DUProGuideLine where DUProGuideLineID=%n", nDUProGuideLineID);
        }
        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_DUProGuideLine");
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        public static IDataReader Receive(TransactionContext tc, DUProGuideLine oDUProGuideLine, EnumDBOperation eENumDBOperation, Int64 nUserId)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_DUProGuideLine_Receive]"
                                    + "%n,%n,%n",
                                    oDUProGuideLine.DUProGuideLineID,
                                    nUserId,
                                    (int)eENumDBOperation);
        }

        public static IDataReader Return(TransactionContext tc, DUProGuideLine oDUProGuideLine, EnumDBOperation eENumDBOperation, Int64 nUserId)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_DUProGuideLine_Return]"
                                    + "%n,%n,%n",
                                    oDUProGuideLine.DUProGuideLineID,
                                    nUserId,
                                    (int)eENumDBOperation);
        }
        #endregion
    }

}
