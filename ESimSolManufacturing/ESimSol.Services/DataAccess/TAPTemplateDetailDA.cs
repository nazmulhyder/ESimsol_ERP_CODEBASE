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

    public class TAPTemplateDetailDA
    {
        public TAPTemplateDetailDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, TAPTemplateDetail oTAPTemplateDetail, EnumDBOperation eEnumDBOperation, Int64 nUserID, bool bIsInitialSave)
        {
            return tc.ExecuteReader("EXEC[SP_IUD_TAPTemplateDetail]"
                                    + "%n,%n,%n,%n,%n,%s,%n,%n,%s,%b", oTAPTemplateDetail.TAPTemplateDetailID, oTAPTemplateDetail.TAPTemplateID, oTAPTemplateDetail.OrderStepID, oTAPTemplateDetail.Sequence, oTAPTemplateDetail.BeforeShipment, oTAPTemplateDetail.Remarks, nUserID, (int)eEnumDBOperation, "", bIsInitialSave);
        }

        public static void Delete(TransactionContext tc, TAPTemplateDetail oTAPTemplateDetail, EnumDBOperation eEnumDBOperation, Int64 nUserID, string sTAPTemplateDetailIDs )
        {
            tc.ExecuteNonQuery("EXEC[SP_IUD_TAPTemplateDetail]"+ 
                                    "%n,%n,%n,%n,%n,%s,%n,%n,%s,%b", oTAPTemplateDetail.TAPTemplateDetailID, oTAPTemplateDetail.TAPTemplateID, oTAPTemplateDetail.OrderStepID, oTAPTemplateDetail.Sequence, oTAPTemplateDetail.BeforeShipment, oTAPTemplateDetail.Remarks, nUserID, (int)eEnumDBOperation, sTAPTemplateDetailIDs,false);
        }

        public static void UpDown(TransactionContext tc, TAPTemplateDetail oTAPTemplateDetail, bool bIsREfresh)
        {
            tc.ExecuteNonQuery("EXEC[SP_TAPTampleteDetail_UPDown]" + "%n,%n,%b,%b", oTAPTemplateDetail.TAPTemplateID, oTAPTemplateDetail.TAPTemplateDetailID, bIsREfresh,oTAPTemplateDetail.bIsUp);
        }
        //UpDown
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_TAPTemplateDetail WHERE TAPTemplateDetailID=%n", nID);
        }

        public static IDataReader Gets(int id, TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_TAPTemplateDetail where TAPTemplateID =%n Order By Sequence, OrderStepSequence", id);
            
        }

        public static IDataReader GetsByTampleteType(int nTempleteType, TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_TAPTemplateDetail where TAPTemplateID IN (SELECT TAPTemplateID FROM TAPTemplate WHERE ISNULL(TampleteType,0) = %n) Order By TAPTemplateID, TAPTemplateDetailID", nTempleteType);
        }
        public static IDataReader Gets( TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_TAPTemplateDetail Order By Sequence");
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        #endregion
    }
    
 
}
