using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Framework;
using ICS.Core.Utility;
using ICS.Core.DataAccess;


namespace ESimSol.Services.DataAccess
{

    public class PartsRequisitionDA
    {
        public PartsRequisitionDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, PartsRequisition oPartsRequisition, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC[SP_IUD_PartsRequisition]" + "%n,  %n, %n, %n,  %n, %n, %n, %d, %s, %s, %n, %n",
                                    oPartsRequisition.PartsRequisitionID, oPartsRequisition.BUID, oPartsRequisition.ServiceOrderID, oPartsRequisition.VehicleRegID, 
                                    oPartsRequisition.PRTypeInt, oPartsRequisition.RequisitionBy, oPartsRequisition.PRStatusInt, oPartsRequisition.IssueDate, 
                                    oPartsRequisition.Remarks, oPartsRequisition.Note, nUserID, (int)eEnumDBOperation);  //oPartsRequisition.StoreID,
        }

        public static void Delete(TransactionContext tc, PartsRequisition oPartsRequisition, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC[SP_IUD_PartsRequisition]" + "%n, %n, %n, %n, %n, %n, %n, %d, %s, %s, %n, %n",
                                    oPartsRequisition.PartsRequisitionID, oPartsRequisition.BUID, oPartsRequisition.ServiceOrderID, oPartsRequisition.VehicleRegID, 
                                    oPartsRequisition.PRTypeInt, oPartsRequisition.RequisitionBy, oPartsRequisition.PRStatusInt, oPartsRequisition.IssueDate, 
                                    oPartsRequisition.Remarks, oPartsRequisition.Note, nUserID, (int)eEnumDBOperation);   //oPartsRequisition.StoreID,
        }

        public static IDataReader ChangeStatus(TransactionContext tc, PartsRequisition oPartsRequisition, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC[SP_PartsRequisitionOperation]" + " %n, %n, %s, %n, %n, %n",
                                    oPartsRequisition.PartsRequisitionID, oPartsRequisition.PRStatusInt, oPartsRequisition.Remarks, oPartsRequisition.PRActionTypeInt, nUserID, (int)eEnumDBOperation);
        }

        #region Accept PartsRequisition Revise
        public static IDataReader AcceptPartsRequisitionRevise(TransactionContext tc, PartsRequisition oPartsRequisition, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_AcceptPartsRequisitionRevise]" + "%n, %s, %n, %n, %n, %n, %n, %n, %d, %s, %n",
                                    oPartsRequisition.PartsRequisitionID, oPartsRequisition.RequisitionNo, oPartsRequisition.BUID, oPartsRequisition.ServiceOrderID, oPartsRequisition.VehicleRegID, oPartsRequisition.PRTypeInt, oPartsRequisition.RequisitionBy, oPartsRequisition.PRStatusInt, oPartsRequisition.IssueDate, oPartsRequisition.Remarks, nUserID);    //oPartsRequisition.StoreID,
        }


        #endregion

        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_PartsRequisition WHERE PartsRequisitionID=%n", nID);
        }
        public static IDataReader GetLog(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_PartsRequisitionLog WHERE PartsRequisitionLogID=%n", nID);
        }

        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_PartsRequisition");
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        #endregion
    }  

  
}
