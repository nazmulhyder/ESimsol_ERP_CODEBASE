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

    public class OrderStepDA
    {
        public OrderStepDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, OrderStep oOrderStep, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_OrderStep]" + "%n, %s, %s,%n,%n,%n, %s, %n, %n, %b, %b, %n, %n",
                                    oOrderStep.OrderStepID, oOrderStep.OrderStepName, oOrderStep.SubStepName, oOrderStep.StyleType, oOrderStep.StepType, oOrderStep.TnAStep, oOrderStep.Note, oOrderStep.RequiredDataType, oOrderStep.Sequence, oOrderStep.IsNotificationSend, oOrderStep.IsActive, (int)eEnumDBOperation, nUserID);
        }
        public static void Delete(TransactionContext tc, OrderStep oOrderStep, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_OrderStep]" + "%n, %s, %s,%n,%n,%n, %s, %n, %n, %b, %b, %n, %n",
                                    oOrderStep.OrderStepID, oOrderStep.OrderStepName, oOrderStep.SubStepName, oOrderStep.StyleType, oOrderStep.StepType, oOrderStep.TnAStep, oOrderStep.Note, oOrderStep.RequiredDataType, oOrderStep.Sequence, oOrderStep.IsNotificationSend, oOrderStep.IsActive, (int)eEnumDBOperation, nUserID);
        }
       
        public static void UpdateSequence(TransactionContext tc, OrderStep oOrderStep)
        {
            tc.ExecuteNonQuery("Update OrderStep  SET Sequence = " + oOrderStep.Sequence + " WHERE OrderStepID = " + oOrderStep.OrderStepID);
        }
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM OrderStep WHERE OrderStepID=%n", nID);
        }

        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM OrderStep Order By StyleType,StepType, Sequence");
        }
        
        public static IDataReader Gets(int nStyleType, TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM OrderStep WHERE StyleType = " + nStyleType + " Order By StepType, Sequence");
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }


        #endregion
    }  
    
    
}
