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

    public class QCStepDA
    {
        public QCStepDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, QCStep oQCStep, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_QCStep]"
                                    + "%n,%n,%s,%n,%n,%n, %n,%n,%s,%b",
                                    oQCStep.QCStepID, oQCStep.ParentID, oQCStep.QCStepName,  oQCStep.QCDataType, oQCStep.Sequence,oQCStep.ProductionStepID, (int)eEnumDBOperation, nUserID,"",false);
        }

        public static void Delete(TransactionContext tc, QCStep oQCStep, EnumDBOperation eEnumDBOperation, Int64 nUserID, string sChildOrdreStepIDS, bool bIsChildDelete)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_QCStep]"
                                    + "%n,%n,%s,%n,%n,%n, %n,%n,%s,%b",
                                    oQCStep.QCStepID, oQCStep.ParentID, oQCStep.QCStepName, oQCStep.QCDataType, oQCStep.Sequence, oQCStep.ProductionStepID, (int)eEnumDBOperation, nUserID, sChildOrdreStepIDS, bIsChildDelete);
        }
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_QCStep WHERE QCStepID=%n", nID);
        }

        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_QCStep Order By ParentID, Sequence");
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }


        #endregion
    }  
    
    
}
