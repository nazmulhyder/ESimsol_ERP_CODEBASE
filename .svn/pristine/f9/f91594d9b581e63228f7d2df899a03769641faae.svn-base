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

    public class QCTemplateDetailDA
    {
        public QCTemplateDetailDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, QCTemplateDetail oQCTemplateDetail, EnumDBOperation eEnumDBOperation, Int64 nUserID, bool bIsInitialSave)
        {
            return tc.ExecuteReader("EXEC[SP_IUD_QCTemplateDetail]"
                                    + "%n,%n,%n,%n,%n,%n,%s,%b", oQCTemplateDetail.QCTemplateDetailID, oQCTemplateDetail.QCTemplateID, oQCTemplateDetail.QCStepID, oQCTemplateDetail.Sequence,  nUserID, (int)eEnumDBOperation, "", bIsInitialSave);
        }

        public static void Delete(TransactionContext tc, QCTemplateDetail oQCTemplateDetail, EnumDBOperation eEnumDBOperation, Int64 nUserID, string sQCTemplateDetailIDs )
        {
            tc.ExecuteNonQuery("EXEC[SP_IUD_QCTemplateDetail]"
                                    + "%n,%n,%n,%n,%n,%n,%s,%b", oQCTemplateDetail.QCTemplateDetailID, oQCTemplateDetail.QCTemplateID, oQCTemplateDetail.QCStepID, oQCTemplateDetail.Sequence, nUserID, (int)eEnumDBOperation, sQCTemplateDetailIDs, false);
        }
        public static void UpDown(TransactionContext tc, QCTemplateDetail oQCTemplateDetail, bool bIsREfresh)
        {
            tc.ExecuteNonQuery("EXEC[SP_QCTampleteDetail_UPDown]" + "%n,%n,%b,%b", oQCTemplateDetail.QCTemplateID, oQCTemplateDetail.QCTemplateDetailID, bIsREfresh,oQCTemplateDetail.bIsUp);
        }
        //UpDown
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_QCTemplateDetail WHERE QCTemplateDetailID=%n", nID);
        }

        public static IDataReader Gets(int id, TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_QCTemplateDetail where QCTemplateID =%n Order By Sequence, QCStepSequence", id);
            
        }

      
        public static IDataReader Gets( TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_QCTemplateDetail Order By Sequence");
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        #endregion
    }
    
 
}
